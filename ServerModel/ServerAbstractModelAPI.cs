using System.Text.Json;
using Logic;

namespace ServerModel;

public abstract class ServerAbstractModelAPI
{
    public static ServerAbstractModelAPI createInstance(ServerAbstractLogicAPI? logicAPI = null)
    {
        return new ServerModelAPI (logicAPI ?? ServerAbstractLogicAPI.createInstance());
    }

    private readonly ServerAbstractLogicAPI logicAPI;
    
    public abstract void DeserializeString(string json);
    
    public event Action<string>? OnBoatsListReady;
    public event Action<string>? OnTimePassed;
    
    public abstract void OnClientConnected();
    
    private class ServerModelAPI : ServerAbstractModelAPI
    {
        private readonly ServerAbstractLogicAPI myLogicAPI;
            
        public override void OnClientConnected()
        {
            PrepareAndSendBoatsList();
        }
        
        public ServerModelAPI(ServerAbstractLogicAPI? logicAPI)
        {
            myLogicAPI = logicAPI;
            
            myLogicAPI.actualTime.Subscribe(
                x =>PrepareAndSendTimePassed(x),  // onNext
                ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                () => Console.WriteLine("End of streaming.")           // onCompleted
            );
        }
        
        public override void DeserializeString(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var message = JsonSerializer.Deserialize<BuyBoatMessage>(json, options);

                if (message?.Type == "buy")
                {
                    myLogicAPI.buyBoat(message.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't deserialize: {e.Message}");
            }   
        }
        
        private class BuyBoatMessage
        {
            public string Type { get; set; } = "";
            public int Id { get; set; }
        }

        private void PrepareAndSendTimePassed(int actualTime)
        {
            Console.WriteLine(actualTime);
            var message = new
            {
                type = "timeUpdate",
                time = actualTime
            };

            string json = JsonSerializer.Serialize(message);
            OnTimePassed?.Invoke(json);
        }

        public void PrepareAndSendBoatsList()
        {
            var boats = GetBoatsFromDatabase();
            var message = new
            {
                type = "boatsListUpdate",
                boats = boats
            };

            string json = JsonSerializer.Serialize(message);
            OnBoatsListReady?.Invoke(json);
        }

        private List<BoatDTO> GetBoatsFromDatabase()
        {
            var boats = myLogicAPI.GetAllBoats();
            List <BoatDTO> boatsDTO = new List<BoatDTO>();
            
            foreach (var boat in boats)
            {
                var newBoat = new BoatDTO { Id = boat.Id, Name = boat.Name, Description = boat.Description, Price = boat.Price };
                boatsDTO.Add(newBoat);
            }
            
            return boatsDTO;
        }
    }
}
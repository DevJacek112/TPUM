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

        public void PrepareAndSendBoatsList()
        {
            var boats = GetBoatsFromDatabase();
            var message = new
            {
                type = "boats",
                boats = boats
            };
            
            Console.WriteLine($"Sending boats to database: {JsonSerializer.Serialize(message)}");

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
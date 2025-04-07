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
            var message = JSONManager.DeserializeRawMessage(json);

            if (message?.Type == "buy")
            {
                var id = JSONManager.DeserializePayload<int>(message.Message);
                myLogicAPI.buyBoat(id);
            }
        }

        private void PrepareAndSendTimePassed(int actualTime)
        {
            string json = JSONManager.Serialize("timeUpdated", actualTime);
            OnTimePassed?.Invoke(json);
        }

        private void PrepareAndSendBoatsList()
        {
            var boats = GetBoatsFromDatabase();

            string json = JSONManager.Serialize("boatsListUpdated", boats);
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
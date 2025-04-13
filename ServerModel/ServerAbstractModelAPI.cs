using System.Net.WebSockets;
using Logic;

namespace ServerModel;

public abstract class ServerAbstractModelAPI
{
    public static ServerAbstractModelAPI createInstance(ServerAbstractLogicAPI? logicAPI = null)
    {
        return new ServerModelAPI (logicAPI ?? ServerAbstractLogicAPI.createInstance());
    }

    private readonly ServerAbstractLogicAPI logicAPI;
    
    public abstract void DeserializeString(WebSocket specificSocket, string json);
    
    public event Action<WebSocket, string>? OnBoatsListReady;
    public event Action<string>? OnTimePassed;
    
    public abstract void OnClientConnected(WebSocket webSocket);
    
    private class ServerModelAPI : ServerAbstractModelAPI
    {
        private readonly ServerAbstractLogicAPI myLogicAPI;

        private List<ClientInfo> clients;
        PriceFilterDTO defaultPriceFilter;
        public ServerModelAPI(ServerAbstractLogicAPI? logicAPI)
        {
            myLogicAPI = logicAPI;
            defaultPriceFilter = new PriceFilterDTO();
            myLogicAPI.actualTime.Subscribe(
                x =>PrepareAndSendTimePassed(x),  // onNext
                ex => Console.WriteLine($"Error: {ex.Message}"),         // onError
                () => Console.WriteLine("End of streaming.")           // onCompleted
            );
            
            clients = new List<ClientInfo>();
        }
        
        public override void OnClientConnected(WebSocket webSocket)
        {
            clients.Add(new ClientInfo(webSocket, defaultPriceFilter));
            PrepareAndSendBoatsList(webSocket);
        }
        
        public override void DeserializeString(WebSocket specificSocket, string json)
        {
            var message = JSONManager.DeserializeRawMessage(json);

            if (message?.Type == "buy")
            {
                var id = JSONManager.DeserializePayload<int>(message.Message);
                myLogicAPI.buyBoat(id);
                foreach (var client in clients)
                {
                    PrepareAndSendBoatsList(client.webSocket);
                }
            }
            
            else if (message?.Type == "priceFilter")
            {
                var priceFilter = JSONManager.DeserializePayload<PriceFilterDTO>(message.Message);

                foreach (var client in clients)
                {
                    if (client.webSocket == specificSocket)
                    {
                        client.filter = priceFilter;
                        PrepareAndSendBoatsList(specificSocket);
                    }
                }
            }
        }

        private void PrepareAndSendTimePassed(int actualTime)
        {
            string json = JSONManager.Serialize("timeUpdated", actualTime);
            OnTimePassed?.Invoke(json);
        }

        private void PrepareAndSendBoatsList(WebSocket socket)
        {
            foreach (var client in clients)
            {
                if (client.webSocket == socket)
                {
                    var boats = GetBoatsFromDatabase(client.filter);
                    string json = JSONManager.Serialize("boatsListUpdated", boats);
                    OnBoatsListReady?.Invoke(socket, json);
                }
            }
        }

        private List<BoatDTO> GetBoatsFromDatabase(PriceFilterDTO filter)
        {
            var boats = myLogicAPI.GetAllBoats();
            List <BoatDTO> boatsDTO = new List<BoatDTO>();
            
            foreach (var boat in boats)
            {
                if (boat.Price >= filter.MinPrice && boat.Price <= filter.MaxPrice)
                {
                    //Console.WriteLine("dodaje lodz o cenie " + boat.Price);
                    Console.WriteLine(filter.MinPrice);
                    var newBoat = new BoatDTO { Id = boat.Id, Name = boat.Name, Description = boat.Description, Price = boat.Price };
                    boatsDTO.Add(newBoat);
                }
            }
            
            return boatsDTO;
        }
    }
}
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ClientData;

public abstract class ClientAbstractDataAPI
{
    public static ClientAbstractDataAPI createInstance()
    {
        return new ClientDataAPI();
    }
    
    public abstract ObservableCollection<BoatDTO> GetAllBoats();
    
    private Subject<int> actualTimeSubject = new Subject<int>();
    public IObservable<int> actualTime => actualTimeSubject.AsObservable();

    public abstract void BuyBoatById(int id);

    private class ClientDataAPI : ClientAbstractDataAPI
    {
        private readonly ObservableCollection<BoatDTO> _boats = new();
        private readonly ClientWebSocketAPI _clientWebSocket;
        public ClientDataAPI()
        {
            _clientWebSocket = new ClientWebSocketAPI();
            _clientWebSocket.OnRawMessageReceived += HandleMessage;
            
            _ = _clientWebSocket.ConnectAsync();
        }

        private void UpdateBoats(List<BoatDTO> boatsFromServer)
        {
            _boats.Clear();
            foreach (var boat in boatsFromServer)
                _boats.Add(boat);
        }

        public override ObservableCollection<BoatDTO> GetAllBoats()
        {
            return _boats;
        }

        public BoatDTO? GetBoatById(int id)
        {
            return _boats.FirstOrDefault(b => b.Id == id);
        }

        public override void BuyBoatById(int id)
        {
            SendBuyBoatMessageAsync(id);
            var boat = GetBoatById(id);
            if (boat != null)
            {
                _boats.Remove(boat);
            }
        }
        
        private void SendBuyBoatMessageAsync(int boatId)
        {
            var json = JSONManager.Serialize("buy", boatId);
            _ = _clientWebSocket.SendRawJsonAsync(json);
        }
        
        private void HandleMessage(string json)
        {
            try
            {
                var message = JSONManager.DeserializeRawMessage(json);

                if (message?.Type == "boatsListUpdated")
                {
                    var boats = JSONManager.DeserializePayload<List<BoatDTO>>(message.Message);
                    if (boats is not null)
                    {
                        UpdateBoats(boats);
                    }
                }
                else if (message?.Type == "timeUpdated")
                {
                    var time = JSONManager.DeserializePayload<int>(message.Message);
                    actualTimeSubject.OnNext(time);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with message: {ex.Message}");
            }
        }

    }

}
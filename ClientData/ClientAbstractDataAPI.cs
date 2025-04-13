using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ClientData.DTO;

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
    
    private Subject<int> actualBoatCountSubject = new Subject<int>();
    public IObservable<int> actualBoatCount => actualBoatCountSubject.AsObservable();
    
    private Subject<int> actualClientsCountSubject = new Subject<int>();
    public IObservable<int> actualClientsCount => actualClientsCountSubject.AsObservable();

    
    public abstract void BuyBoatById(int id);
    public abstract void SetPriceFilter(float minPrice, float maxPrice);

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

        public override void BuyBoatById(int id)
        {
            SendBuyBoatMessageAsync(id);
        }

        public override void SetPriceFilter(float minPrice, float maxPrice)
        {
            SendSetFilterMessageAsync(minPrice, maxPrice);
        }

        private void SendBuyBoatMessageAsync(int boatId)
        {
            var json = JSONManager.Serialize("buy", boatId);
            _ = _clientWebSocket.SendRawJsonAsync(json);
        }
        
        private void SendSetFilterMessageAsync(float minPrice, float maxPrice)
        {
            var filters = new PriceFilterDTO{MinPrice = minPrice, MaxPrice = maxPrice};
            var json = JSONManager.Serialize("priceFilter", filters);
            _ = _clientWebSocket.SendRawJsonAsync(json);
            Console.WriteLine(json);
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
                else if (message?.Type == "diagnosticsUpdated")
                {
                    var diagnostics = JSONManager.DeserializePayload<DiagnosticsDTO>(message.Message);
                    actualTimeSubject.OnNext(diagnostics.serverTimeOnline);
                    actualBoatCountSubject.OnNext(diagnostics.numberOfAllBoats);
                    actualClientsCountSubject.OnNext(diagnostics.numberOfActiveClients);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with message: {ex.Message}");
            }
        }

    }

}
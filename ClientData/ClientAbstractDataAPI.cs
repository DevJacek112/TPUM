using System.Collections.ObjectModel;
using System.Text.Json;

namespace ClientData;

public abstract class ClientAbstractDataAPI
{
    public static ClientAbstractDataAPI createInstance()
    {
        return new ClientDataAPI();
    }
    
    public abstract ObservableCollection<BoatDTO> GetAllBoats();
    public abstract BoatDTO? GetBoatById(int id);

    public abstract void BuyBoatById(int id);

    private class ClientDataAPI : ClientAbstractDataAPI
    {
        private ObservableCollection<BoatDTO> _boats = new();
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

        public override BoatDTO? GetBoatById(int id)
        {
            return _boats.FirstOrDefault(b => b.Id == id);
        }

        public override void BuyBoatById(int id)
        {
            _clientWebSocket.SendBuyBoatMessageAsync(id);
            var boat = GetBoatById(id);
            if (boat != null)
            {
                _boats.Remove(boat);
            }
        }
        
        private void HandleMessage(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                string type = root.GetProperty("type").GetString();

                if (type == "boats")
                {
                    var boatsJson = root.GetProperty("boats").ToString();
                    var boats = JsonSerializer.Deserialize<List<BoatDTO>>(boatsJson);

                    if (boats is not null)
                        UpdateBoats(boats);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy analizie wiadomości: {ex.Message}");
            }
        }
    }

}
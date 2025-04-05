using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ClientData;

public class WebSocketClientAPI
{
    private ClientWebSocket _socket = new();

    public async Task ConnectAsync()
    {
        await _socket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
        Console.WriteLine("✅ Połączono z serwerem WebSocket");

        _ = ListenAsync();
    }

    public async Task SendBuyBoatMessageAsync(int boatId)
    {
        var message = new
        {
            type = "buy",
            id = boatId
        };

        string json = JsonSerializer.Serialize(message);
        var buffer = Encoding.UTF8.GetBytes(json);
        await _socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine($"📤 Wysłano: {json}");
    }

    // Event, na który inne klasy będą subskrybować, by odebrać surowe dane
    public event Action<string>? OnRawMessageReceived;

    private async Task ListenAsync()
    {
        var buffer = new byte[1024];
        while (_socket.State == WebSocketState.Open)
        {
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string response = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"📨 Odebrano: {response}");

            // Wywołanie eventu po odebraniu surowych danych
            OnRawMessageReceived?.Invoke(response);
        }
    }
}
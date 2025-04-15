using System.Net.WebSockets;
using System.Text;

namespace ClientData;

public class ClientWebSocketAPI
{
    private ClientWebSocket _socket = new();
    public event Action<string>? OnRawMessageReceived;
    
    public async Task ConnectAsync()
    {
        while (_socket.State != WebSocketState.Open)
        {
            _socket = new ClientWebSocket();
            try
            {
                await _socket.ConnectAsync(new Uri("ws://localhost:7312/ws"), CancellationToken.None);
                _ = ListenAsync();
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Trying to connect...");
                await Task.Delay(5000, CancellationToken.None);
            }
        }
    }

    public async Task SendRawJsonAsync(string json)
    {
        await _socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(json)), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    
    private async Task ListenAsync()
    {
        var buffer = new byte[1024];
        while (_socket.State == WebSocketState.Open)
        {
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            OnRawMessageReceived?.Invoke(message);
        }
    }
}
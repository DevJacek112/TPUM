using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ClientData;

public class ClientWebSocketAPI
{
    private ClientWebSocket _socket = new();
    public event Action<string>? OnRawMessageReceived;
    
    public async Task ConnectAsync()
    {
        await _socket.ConnectAsync(new Uri("ws://localhost:7312/ws"), CancellationToken.None);
        _ = ListenAsync();
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
            string response = Encoding.UTF8.GetString(buffer, 0, result.Count);
            OnRawMessageReceived?.Invoke(response);
        }
    }
}
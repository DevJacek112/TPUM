using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ServerModel;

public class WebSocketServerAPI
{
    private readonly HttpListener _httpListener;

    public WebSocketServerAPI()
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add("http://localhost:5000/ws/");
    }

    public async Task StartAsync()
    {
        _httpListener.Start();
        Console.WriteLine("🔌 Serwer nasłuchuje na ws://localhost:5000/ws");

        while (true)
        {
            var context = await _httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                _ = HandleConnectionAsync(wsContext.WebSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async Task HandleConnectionAsync(WebSocket socket)
    {
        var buffer = new byte[1024];

        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Console.WriteLine($"Otrzymano: {json}");

            try
            {
                var message = JsonSerializer.Deserialize<BuyBoatMessage>(json);
                if (message?.Type == "buy")
                {
                    Console.WriteLine($"🛒 Zakup łodzi o ID: {message.Id}");
                    // TODO: Wywołaj model/logic tutaj
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Błąd deserializacji: {e.Message}");
            }
        }
    }

    private class BuyBoatMessage
    {
        public string Type { get; set; } = "";
        public int Id { get; set; }
    }
}
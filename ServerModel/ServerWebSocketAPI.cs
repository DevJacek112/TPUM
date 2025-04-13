using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ServerModel;

public class ServerWebSocketAPI
{
    private readonly HttpListener _httpListener;
    private readonly ServerAbstractModelAPI _serverAbstractModel;
    private readonly List<WebSocket> _connectedSockets = new();
    private readonly object _lock = new();
    public ServerWebSocketAPI()
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add("http://localhost:7312/ws/");
        _serverAbstractModel = ServerAbstractModelAPI.createInstance();
        
        //boat list update
        _serverAbstractModel.OnBoatsListReady += async (socket, json) =>
        {
            await SendRawJsonAsync(socket, json);
        };
        
        //time passed
        _serverAbstractModel.OnTimePassed += (json) =>
        {
            SendToAll(json);
        };
    }

    public async Task StartAsync()
    {
        _httpListener.Start();

        while (true)
        {
            var context = await _httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                _ = HandleConnectionAsync(wsContext.WebSocket);
                lock (_lock)
                {
                    _connectedSockets.Add(wsContext.WebSocket);
                }
                _serverAbstractModel.OnClientConnected(wsContext.WebSocket);
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
            
            _serverAbstractModel.DeserializeString(socket, json);
            
            Console.WriteLine(json);
        }
    }

    private async Task SendRawJsonAsync(WebSocket specificSocket, string json)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        var segment = new ArraySegment<byte>(buffer);
        
        lock (_lock)
        {
            foreach (var socket in _connectedSockets)
            {
                if (socket.State == WebSocketState.Open && socket == specificSocket)
                {
                    _ = socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

    private async void SendToAll(string json)
    {
        foreach (var socket in _connectedSockets)
        {
            await SendRawJsonAsync(socket, json);
        }
    }

}
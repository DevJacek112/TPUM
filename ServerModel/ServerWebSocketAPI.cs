using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ServerModel;

public class ServerWebSocketAPI
{
    private readonly HttpListener _httpListener;
    private readonly ServerAbstractModelAPI _serverAbstractModel;
    private WebSocket _connectedSocket;
    public ServerWebSocketAPI()
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add("http://localhost:7312/ws/");
        _serverAbstractModel = ServerAbstractModelAPI.createInstance();
        
        //boat list update
        _serverAbstractModel.OnBoatsListReady += async (json) =>
        {
            await SendRawJsonAsync(json);
        };
        
        //time passed
        _serverAbstractModel.OnTimePassed += async (json) =>
        {
            await SendRawJsonAsync(json);
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
                _connectedSocket = wsContext.WebSocket;
                _serverAbstractModel.OnClientConnected();
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
            
            _serverAbstractModel.DeserializeString(json);
        }
    }

    private async Task SendRawJsonAsync(string json)
    {
        if (_connectedSocket != null && _connectedSocket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);
            await _connectedSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
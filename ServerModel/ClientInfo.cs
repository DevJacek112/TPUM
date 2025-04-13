using System.Net.WebSockets;

namespace ServerModel;

public class ClientInfo
{
    public WebSocket webSocket { get; set; }
    public PriceFilterDTO filter { get; set; }

    public ClientInfo(WebSocket socket, PriceFilterDTO filter)
    {
        this.filter = filter;
        webSocket = socket;
    }
}
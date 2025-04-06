namespace ServerModel
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var server = new ServerWebSocketAPI();
            await server.StartAsync();
            
        }
    }
}

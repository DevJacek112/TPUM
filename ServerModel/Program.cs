using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

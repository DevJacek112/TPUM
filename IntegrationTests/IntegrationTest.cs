using ServerModel;
using ClientData;
using JSONManager = ServerModel.JSONManager;

namespace IntegrationTests
{
    [TestClass]
    public sealed class IntegrationTest
    {
        private static bool _serverStarted = false;

        [TestInitialize]
        public void Setup()
        {
            if (!_serverStarted)
            {
                var server = new ServerWebSocketAPI();
                _ = Task.Run(() => server.StartAsync());
                Thread.Sleep(1000);
                _serverStarted = true;
            }
        }

        [TestMethod]
        public async Task BuyBoatIntegration()
        {
            var client = new ClientWebSocketAPI();
            string? receivedMessage = null;
            var tcs = new TaskCompletionSource<string>();
            client.OnRawMessageReceived += (message) =>
            {
                receivedMessage = message;
                tcs.TrySetResult(message);
            };
            await client.ConnectAsync();
            string jsonBuy = JSONManager.Serialize("buy", 1);
            await client.SendRawJsonAsync(jsonBuy);
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("boatsListUpdated"), $"Unexpected message: {message}");
        }

        [TestMethod]
        public async Task FilterUpdateIntegration()
        {
            var client = new ClientWebSocketAPI();
            var tcs = new TaskCompletionSource<string>();

            client.OnRawMessageReceived += (msg) =>
            {
                if (msg.Contains("boatsListUpdated"))
                    tcs.TrySetResult(msg);
            };

            await client.ConnectAsync();
            string jsonFilter = JSONManager.Serialize("filter", 5000);
            await client.SendRawJsonAsync(jsonFilter);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("boatsListUpdated"), $"Unexpected message: {message}");
        }

        [TestMethod]
        public async Task DiagnosticsRequestIntegration()
        {
            var client = new ClientWebSocketAPI();
            var tcs = new TaskCompletionSource<string>();

            client.OnRawMessageReceived += (msg) =>
            {
                if (msg.Contains("diagnosticsUpdated"))
                    tcs.TrySetResult(msg);
            };

            await client.ConnectAsync();
            string jsonDiag = JSONManager.Serialize("diagnostics", "testdiag");
            await client.SendRawJsonAsync(jsonDiag);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("diagnosticsUpdated"), $"Unexpected message: {message}");
        }
    }
}

namespace IntegrationTests
{
    [TestClass]
    public sealed class IntegrationTest
    {
        private static bool _serverStarted = false;
        private static readonly object _lock = new();

        private async Task EnsureServerStartedAsync()
        {
            if (!_serverStarted)
            {
                lock (_lock)
                {
                    if (!_serverStarted)
                    {
                        var server = new ServerModel.ServerWebSocketAPI();
                        _ = Task.Run(() => server.StartAsync());
                        _serverStarted = true;
                    }
                }

                await WaitForServerAsync();
            }
        }

        private async Task WaitForServerAsync(int timeoutMs = 60000)
        {
            var client = new ClientData.ClientWebSocketAPI();
            var start = DateTime.UtcNow;
            Exception? lastException = null;

            while ((DateTime.UtcNow - start).TotalMilliseconds < timeoutMs)
            {
                try
                {
                    await client.ConnectAsync();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    await Task.Delay(100);
                }
            }

            throw new Exception("Failed to connect to server within timeout", lastException);
        }

        [TestMethod]
        public async Task BuyBoatIntegration()
        {
            await EnsureServerStartedAsync();

            var client = new ClientData.ClientWebSocketAPI();
            var tcs = new TaskCompletionSource<string>();

            client.OnRawMessageReceived += (message) =>
            {
                tcs.TrySetResult(message);
            };

            await client.ConnectAsync();
            string jsonBuy = ServerModel.JSONManager.Serialize("buy", 1);
            await client.SendRawJsonAsync(jsonBuy);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("boatsListUpdated"), $"Unexpected message: {message}");
        }

        [TestMethod]
        public async Task FilterUpdateIntegration()
        {
            await EnsureServerStartedAsync();

            var client = new ClientData.ClientWebSocketAPI();
            var tcs = new TaskCompletionSource<string>();

            client.OnRawMessageReceived += (msg) =>
            {
                if (msg.Contains("boatsListUpdated"))
                    tcs.TrySetResult(msg);
            };

            await client.ConnectAsync();
            string jsonFilter = ServerModel.JSONManager.Serialize("filter", 5000);
            await client.SendRawJsonAsync(jsonFilter);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("boatsListUpdated"), $"Unexpected message: {message}");
        }

        [TestMethod]
        public async Task DiagnosticsRequestIntegration()
        {
            await EnsureServerStartedAsync();

            var client = new ClientData.ClientWebSocketAPI();
            var tcs = new TaskCompletionSource<string>();

            client.OnRawMessageReceived += (msg) =>
            {
                if (msg.Contains("diagnosticsUpdated"))
                    tcs.TrySetResult(msg);
            };

            await client.ConnectAsync();
            string jsonDiag = ServerModel.JSONManager.Serialize("diagnostics", "testdiag");
            await client.SendRawJsonAsync(jsonDiag);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            Assert.IsTrue(completed == tcs.Task, "No message received in time.");
            var message = tcs.Task.Result;
            Assert.IsTrue(message.Contains("diagnosticsUpdated"), $"Unexpected message: {message}");
        }
    }
}

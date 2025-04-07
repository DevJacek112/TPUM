using ClientData;
using ClientLogic;

namespace ClientTests
{
    [TestClass]
    public sealed class ClientDataTests
    {
        
        private ClientAbstractDataAPI dataAPI;
        private ClientAbstractLogicAPI logicAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = ClientAbstractDataAPI.createInstance();
            logicAPI = ClientAbstractLogicAPI.createInstance(dataAPI);
        }
        
        [TestMethod]
        public void TestUpdateBoatsFromServerMessage()
        {
            var boats = new List<BoatDTO>
            {
                new() { Id = 1, Name = "Lodka1", Description = "Opis", Price = 100 },
                new() { Id = 2, Name = "Lodka2", Description = "Opis", Price = 200 }
            };

            var json = JSONManager.Serialize("boatsListUpdated", boats);
            
            InvokeHandleMessage(dataAPI, json);
            
            var currentBoats = dataAPI.GetAllBoats();
            Assert.AreEqual(2, currentBoats.Count);
            Assert.AreEqual("Lodka1", currentBoats[0].Name);
        }
        
        [TestMethod]
        public void TestBuyBoatRemovesFromList()
        {
            var boats = new List<BoatDTO>
            {
                new() { Id = 5, Name = "LodkaX", Description = "Opis", Price = 300 }
            };

            var json = JSONManager.Serialize("boatsListUpdated", boats);
            InvokeHandleMessage(dataAPI, json);
            dataAPI.BuyBoatById(5);
            Assert.AreEqual(0, dataAPI.GetAllBoats().Count);
        }
        
        [TestMethod]
        public void TestTimeUpdateEvent()
        {
            var json = JSONManager.Serialize("timeUpdated", 123);
            int? receivedTime = null;
            using var subscription = dataAPI.actualTime.Subscribe(t => receivedTime = t);
            InvokeHandleMessage(dataAPI, json);
            Assert.AreEqual(123, receivedTime);
        }
        
        private void InvokeHandleMessage(ClientAbstractDataAPI api, string json)
        {
            var dataAPIType = api.GetType();
            var method = dataAPIType.GetMethod("HandleMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(api, new object[] { json });
        }
    }
}

using ClientData;
using ClientLogic;
using ServerModel;
using BoatDTO = ClientData.BoatDTO;
using JSONManager = ClientData.JSONManager;

namespace ClientTests
{
    [TestClass]
    public class ClientLogicTests
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
        public void GetAllBoatsTest()
        {
            var boats = new List<BoatDTO>
            {
                new() { Id = 1, Name = "Lodka1", Description = "Opis", Price = 100 },
                new() { Id = 2, Name = "Lodka2", Description = "Opis", Price = 200 }
            };
            var json = JSONManager.Serialize("boatsListUpdated", boats);
            InvokeHandleMessage(dataAPI, json);
            var result = logicAPI.GetAllBoats();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Lodka1", result[0].Name);
        }

        private void InvokeHandleMessage(ClientAbstractDataAPI api, string json)
        {
            var type = api.GetType();
            var method = type.GetMethod("HandleMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(api, new object[] { json });
        }
    }
}

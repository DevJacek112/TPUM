using Data;

namespace Tests
{
    [TestClass]
    public sealed class ServerDataTests
    {
        private ServerAbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = ServerAbstractDataAPI.createInstance();
        }

        [TestMethod]
        public void AddBoat()
        {
            dataAPI.AddBoat(14, "Lodka14", "Mala lodka", 100.0f);
            var boatTest = dataAPI.GetAllBoats()[13];

            Assert.AreEqual(14, dataAPI.GetAllBoats().Count);
            Assert.AreEqual("Lodka14", boatTest.Name);
            Assert.AreEqual("Mala lodka", boatTest.Description);
            Assert.AreEqual(100.0f, boatTest.Price);
        }

        [TestMethod]
        public void GetBoatById()
        {
            dataAPI.AddBoat(14, "Lodka", "Duza lodz", 110.0f);
            var boatTest = dataAPI.GetBoatById(14);

            Assert.AreEqual(14, dataAPI.GetAllBoats().Count);
            Assert.AreEqual("Lodka", boatTest.Name);
            Assert.AreEqual("Duza lodz", boatTest.Description);
            Assert.AreEqual(110.0f, boatTest.Price);
        }

        [TestMethod]
        public void RemoveBoat()
        {
            Assert.AreEqual(13, dataAPI.GetAllBoats().Count);
            dataAPI.AddBoat(0, "Lodka", "Duza lodz", 110.0f);
            Assert.AreEqual(14, dataAPI.GetAllBoats().Count);
            dataAPI.RemoveBoat(0);
            Assert.AreEqual(13, dataAPI.GetAllBoats().Count);
        }
    }
    
}

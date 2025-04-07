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
            dataAPI.AddBoat(4, "Lodka4", "Mala lodka", 100.0f);
            var boatTest = dataAPI.GetAllBoats()[3];

            Assert.AreEqual(4, dataAPI.GetAllBoats().Count);
            Assert.AreEqual("Lodka4", boatTest.Name);
            Assert.AreEqual("Mala lodka", boatTest.Description);
            Assert.AreEqual(100.0f, boatTest.Price);
        }

        [TestMethod]
        public void GetBoatById()
        {
            dataAPI.AddBoat(4, "Lodka", "Duza lodz", 110.0f);
            var boatTest = dataAPI.GetBoatById(4);

            Assert.AreEqual(4, dataAPI.GetAllBoats().Count);
            Assert.AreEqual("Lodka", boatTest.Name);
            Assert.AreEqual("Duza lodz", boatTest.Description);
            Assert.AreEqual(110.0f, boatTest.Price);
        }

        [TestMethod]
        public void RemoveBoat()
        {
            dataAPI.AddBoat(0, "Lodka", "Duza lodz", 110.0f);
            var boatTest = dataAPI.GetBoatById(0);

            Assert.AreEqual(4, dataAPI.GetAllBoats().Count);
            dataAPI.RemoveBoat(0);
            Assert.AreEqual(3, dataAPI.GetAllBoats().Count);
        }
    }
    
}

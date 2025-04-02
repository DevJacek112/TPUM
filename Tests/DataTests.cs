using Data;

namespace Tests
{
    [TestClass]
    public sealed class DataTests
    {
        private AbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = AbstractDataAPI.createInstance();
        }

        [TestMethod]
        public void AddBoat()
        {
            dataAPI.AddBoat(0, "Lodka1", "Mala lodka", 100.0f);
            var boatTest = dataAPI.GetAllBoats()[0];

            Assert.AreEqual(dataAPI.GetAllBoats().Count, 1);
            Assert.AreEqual(boatTest.Name, "Lodka1");
            Assert.AreEqual(boatTest.Description, "Mala lodka");
            Assert.AreEqual(boatTest.Price, 100.0f);
        }

        [TestMethod]
        public void GetBoatById()
        {
            dataAPI.AddBoat(0, "Lodka", "Duza lodz", 110.0f);
            var boatTest = dataAPI.GetBoatById(0);

            Assert.AreEqual(dataAPI.GetAllBoats().Count, 1);
            Assert.AreEqual(boatTest.Name, "Lodka");
            Assert.AreEqual(boatTest.Description, "Duza lodz");
            Assert.AreEqual(boatTest.Price, 110.0f);
        }

        [TestMethod]
        public void RemoveBoat()
        {
            dataAPI.AddBoat(0, "Lodka", "Duza lodz", 110.0f);
            var boatTest = dataAPI.GetBoatById(0);

            Assert.AreEqual(dataAPI.GetAllBoats().Count, 1);
            dataAPI.RemoveBoat(0);
            Assert.AreEqual(dataAPI.GetAllBoats().Count, 0);
        }
    }
    
}

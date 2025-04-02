using Logic;
using Data;

namespace Tests
{
    [TestClass]
    public sealed class LogicTests
    {
        private AbstractLogicAPI logicAPI;
        private AbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = AbstractDataAPI.createInstance();
            logicAPI = AbstractLogicAPI.createInstance();
        }

        [TestMethod]
        public void addBoat()
        {
            Assert.AreEqual(logicAPI.GetAllBoats().Count, 0);
            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(logicAPI.GetAllBoats().Count, 1);
            Assert.AreEqual(logicAPI.GetAllBoats()[0].Name, "Lodka1");
        }

        [TestMethod]
        public void buyBoat()
        {
            Assert.AreEqual(false, logicAPI.buyBoat(1));
            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(true, logicAPI.buyBoat(1));
            Assert.AreEqual(logicAPI.GetAllBoats().Count, 0);
        }
    }
}

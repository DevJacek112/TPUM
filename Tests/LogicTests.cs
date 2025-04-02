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
            Assert.AreEqual(3, logicAPI.GetAllBoats().Count);
            logicAPI.addBoat("Lodka4", "Mala lodka", 100.0f);
            Assert.AreEqual(4, logicAPI.GetAllBoats().Count);
            Assert.AreEqual("Lodka4", logicAPI.GetAllBoats()[3].Name);
        }

        [TestMethod]
        public void buyBoat()
        {
            Assert.AreEqual(false, logicAPI.buyBoat(4));
            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(true, logicAPI.buyBoat(4));
            Assert.AreEqual(3, logicAPI.GetAllBoats().Count);
        }
    }
}

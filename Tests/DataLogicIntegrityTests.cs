using Data;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public sealed class DataLogicIntegrityTests
    {
        private AbstractLogicAPI logicAPI;
        private AbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = AbstractDataAPI.createInstance();
            logicAPI = AbstractLogicAPI.createInstance(dataAPI);
        }

        [TestMethod]
        public void buyBoatIntegrity()
        {
            Assert.AreEqual(0, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(0, logicAPI.GetAllBoats().Count);

            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(1, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(1, logicAPI.GetAllBoats().Count);

            Assert.AreEqual(true, logicAPI.buyBoat(1));
            Assert.AreEqual(0, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(0, logicAPI.GetAllBoats().Count);

        }

    }
}

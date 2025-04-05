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
        private ServerLogicAPI logicAPI;
        private ServerAbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = ServerAbstractDataAPI.createInstance();
            logicAPI = ServerLogicAPI.createInstance(dataAPI);
        }

        [TestMethod]
        public void buyBoatIntegrity()
        {
            Assert.AreEqual(3, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(3, logicAPI.GetAllBoats().Count);

            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(4, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(4, logicAPI.GetAllBoats().Count);

            Assert.AreEqual(true, logicAPI.buyBoat(1));
            Assert.AreEqual(3, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(3, logicAPI.GetAllBoats().Count);

        }

    }
}

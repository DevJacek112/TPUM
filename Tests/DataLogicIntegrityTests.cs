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
        private ServerAbstractLogicAPI logicAPI;
        private ServerAbstractDataAPI dataAPI;

        [TestInitialize]
        public void Setup()
        {
            dataAPI = ServerAbstractDataAPI.createInstance();
            logicAPI = ServerAbstractLogicAPI.createInstance(dataAPI);
        }

        [TestMethod]
        public void buyBoatIntegrity()
        {
            Assert.AreEqual(13, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(13, logicAPI.GetAllBoats().Count);

            logicAPI.addBoat("Lodka1", "Mala lodka", 100.0f);
            Assert.AreEqual(14, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(14, logicAPI.GetAllBoats().Count);

            Assert.AreEqual(true, logicAPI.buyBoat(1));
            Assert.AreEqual(13, dataAPI.GetAllBoats().Count);
            Assert.AreEqual(13, logicAPI.GetAllBoats().Count);

        }

    }
}

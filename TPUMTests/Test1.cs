namespace TPUMTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethodAdd()
        {
            TPUM.Calculator calculator = new TPUM.Calculator();
            Assert.AreEqual(calculator.Add(2, 3), 5);
        }

        [TestMethod]
        public void TestMethodSub()
        {
            TPUM.Calculator calculator = new TPUM.Calculator();
            Assert.AreEqual(calculator.Sub(6, 2), 4);
        }
    }
}

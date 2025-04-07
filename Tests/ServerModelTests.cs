namespace Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerModel;
using Logic;

[TestClass]
public class ServerModelTests
{
    private ServerAbstractModelAPI modelAPI;
    private ServerAbstractLogicAPI logicAPI;

    [TestInitialize]
    public void Setup()
    {
        logicAPI = ServerAbstractLogicAPI.createInstance();
        modelAPI = ServerAbstractModelAPI.createInstance(logicAPI);
    }
    
    [TestMethod]
    public void OnClientConnectedTest()
    {
        string? resultJson = null;
        modelAPI.OnBoatsListReady += (json) => resultJson = json;
        modelAPI.OnClientConnected();
        string expectedJson = JSONManager.Serialize("boatsListUpdated", logicAPI.GetAllBoats().Select(b => new BoatDTO
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            Price = b.Price
        }).ToList());

        Assert.AreEqual(expectedJson, resultJson);
    }

    [TestMethod]
    public void DeserializeStringTest()
    {
        Assert.AreEqual(3, logicAPI.GetAllBoats().Count);
        
        string json = "{\"Type\":\"buy\",\"Message\":2}\n";
        modelAPI.DeserializeString(json);
        
        Assert.AreEqual(2, logicAPI.GetAllBoats().Count);
    }
}
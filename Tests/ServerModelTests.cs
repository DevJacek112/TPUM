using System.Net.WebSockets;

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
        WebSocket socket = new ClientWebSocket();
        modelAPI.OnBoatsListReady += (socket ,json) => resultJson = json;
        modelAPI.OnClientConnected(socket);
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
        Assert.AreEqual(13, logicAPI.GetAllBoats().Count);
        
        string json = "{\"Type\":\"buy\",\"Message\":2}\n";
        WebSocket socket = new ClientWebSocket();
        modelAPI.DeserializeString(socket, json);
        
        Assert.AreEqual(12, logicAPI.GetAllBoats().Count);
    }
}
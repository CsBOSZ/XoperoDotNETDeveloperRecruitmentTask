using Moq;
using xopS;
using xopS.Services;

namespace xopC.IntegrationTests;

[TestClass]
public class StateTest
{
    private State _myState;
    // private Mock<IHttpDevice> mock;
    public static Device DeviceFactor()
    {
        return new Device("unix", "test", "test", "amd", 4, new DateTime(2000,2,2));
    }
    

    [TestMethod]
    public void GetPage_0()
    {
        
        Mock<IHttpDevice> mock = new Mock<IHttpDevice>();
        mock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new List<DeviceOdt>() { DeviceFactor(),DeviceFactor(),DeviceFactor() });
        
        _myState = new State(0, "t", 0, 0,new List<DeviceOdt>(), false,mock.Object);
        
        
        _myState.GetPage(0);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(0,IndexPage);
        Assert.AreEqual("",IndexName);
        Assert.AreEqual(3,Page);
        
    }
    [TestMethod]
    public void GetPage_1()
    {
        
        Mock<IHttpDevice> mock = new Mock<IHttpDevice>();
        mock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new List<DeviceOdt>() { DeviceFactor() });

        _myState = new State(0, "t", 1, 0,new List<DeviceOdt>(), false,mock.Object);
        
        
        _myState.GetPage(1);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(1,IndexPage);
        Assert.AreEqual("",IndexName);
        Assert.AreEqual(1,Page);
        
    }
    
    [TestMethod]
    public void SearchPage0()
    {
        
        Mock<IHttpDevice> mock = new Mock<IHttpDevice>();
        mock.Setup(x => x.Search(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new List<DeviceOdt>() { DeviceFactor(),DeviceFactor(),DeviceFactor() });
        mock.Setup(x => x.GetSearchPageCount(It.IsAny<string>()))
            .ReturnsAsync(1);
        
        _myState = new State(3, "t", 1, 0,new List<DeviceOdt>(), false,mock.Object);
        
        _myState.Search("test",0);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int MaxPageSearch = _myState.MaxPageSearch;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(0,IndexPage);
        Assert.AreEqual("test",IndexName);
        Assert.AreEqual(1,MaxPageSearch);
        Assert.AreEqual(3,Page);
        
    }

    [TestMethod]
    public void SearchPage1()
    {
        
        Mock<IHttpDevice> mock = new Mock<IHttpDevice>();
        mock.Setup(x => x.Search(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new List<DeviceOdt>() { DeviceFactor() });
        mock.Setup(x => x.GetSearchPageCount(It.IsAny<string>()))
            .ReturnsAsync(1);
        
        _myState = new State(3, "t", 1, 0,new List<DeviceOdt>(), false,mock.Object);
        
        _myState.Search("test",1);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int MaxPageSearch = _myState.MaxPageSearch;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(1,IndexPage);
        Assert.AreEqual("test",IndexName);
        Assert.AreEqual(1,MaxPageSearch);
        Assert.AreEqual(1,Page);
        
    }
    


}
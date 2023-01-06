using xopS;
using xopS.Services;

namespace xopC.IntegrationTests;

[TestClass]
public class StateTest
{
    private State _myState;
    private DeviceService _deviceService;
    
    public static Device DeviceFactor()
    {
        return new Device("unix", "test", "test", "amd", 4, new DateTime(2000,2,2));
    }
    
    [TestInitialize]
    public void Init()
    {
        _myState = new State(0, "", 0, 0,new List<DeviceOdt>(), false);
      _deviceService = new DeviceService();
      _deviceService.DeleteAll();
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());

    }
    
    [TestCleanup]
    public void Clean()
    {
        
        _myState = new State(0, "", 0, 0,new List<DeviceOdt>(), false);
        
        
    }

    [TestMethod]
    public void GetPage()
    {
        _myState.GetPage(0);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int MaxPage = _myState.MaxPage;
        int MaxPageSearch = _myState.MaxPageSearch;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(0,IndexPage);
        Assert.AreEqual("",IndexName);
        Assert.AreEqual(1,MaxPage);
        Assert.AreEqual(0,MaxPageSearch);
        Assert.AreEqual(3,Page);
        
    }
    
    [TestMethod]
    public void GetPage1()
    {
        _myState.GetPage(1);
        
        // ----
        
        int IndexPage = _myState.IndexPage;
        string IndexName = _myState.IndexName;
        int MaxPage = _myState.MaxPage;
        int MaxPageSearch = _myState.MaxPageSearch;
        int Page = _myState.Page.Count();
        
        // ----
        
        Assert.AreEqual(1,IndexPage);
        Assert.AreEqual("",IndexName);
        Assert.AreEqual(1,MaxPage);
        Assert.AreEqual(0,MaxPageSearch);
        Assert.AreEqual(2,Page);
        
    }





}
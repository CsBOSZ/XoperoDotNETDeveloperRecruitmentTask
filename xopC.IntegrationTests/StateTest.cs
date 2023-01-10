using Moq;
using xopS;
using xopS.Services;

namespace xopC.IntegrationTests;

[TestClass]
public class StateTest
{
    private State _myState;
    private DeviceService _deviceService;
    private Mock<IHttpDevice> mock;
    public static Device DeviceFactor()
    {
        return new Device("unix", "test", "test", "amd", 4, new DateTime(2000,2,2));
    }
    
    
    
    [TestInitialize]
    public void Init()
    {

      _deviceService = new DeviceService();
      _deviceService.DeleteAll();
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      _deviceService.Add(DeviceFactor());
      
      mock = new Mock<IHttpDevice>();
      // mock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<bool>()))
      //     .Callback((int p, bool m) =>
      //     {
      //         return m ? _deviceService.GetDevicesPage(p) : DeviceService.ToOdt(_deviceService.GetDevicesPage(p));
      //     });
      // mock.Setup(x => x.Search(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<bool>()))
      //     .Callback((string n,int p, bool m) =>
      //     {
      //         return m ? _deviceService.SearchByName(n, p) : DeviceService.ToOdt(_deviceService.SearchByName(n, p));
      //     });
      mock.Setup(x => x.GetSearchPageCount(It.IsAny<string>()))
          .Callback((string n) => _deviceService.Pages(n));
      _myState = new State(0, "", 0, 0,new List<DeviceOdt>(), false,mock.Object);

    }
    
    [TestCleanup]
    public void Clean()
    {
       
        
        
        
    }

    // [TestMethod]
    // public void GetPage()
    // {
    //     _myState.GetPage(0);
    //     
    //     // ----
    //     
    //     int IndexPage = _myState.IndexPage;
    //     string IndexName = _myState.IndexName;
    //     int MaxPage = _myState.MaxPage;
    //     int MaxPageSearch = _myState.MaxPageSearch;
    //     int Page = _myState.Page.Count();
    //     
    //     // ----
    //     
    //     Assert.AreEqual(0,IndexPage);
    //     Assert.AreEqual("",IndexName);
    //     Assert.AreEqual(1,MaxPage);
    //     Assert.AreEqual(0,MaxPageSearch);
    //     Assert.AreEqual(3,Page);
    //     
    // }
    //
    // [TestMethod]
    // public void GetPage1()
    // {
    //     _myState.GetPage(1);
    //     
    //     // ----
    //     
    //     int IndexPage = _myState.IndexPage;
    //     string IndexName = _myState.IndexName;
    //     int MaxPage = _myState.MaxPage;
    //     int MaxPageSearch = _myState.MaxPageSearch;
    //     int Page = _myState.Page.Count();
    //     
    //     // ----
    //     
    //     Assert.AreEqual(1,IndexPage);
    //     Assert.AreEqual("",IndexName);
    //     Assert.AreEqual(1,MaxPage);
    //     Assert.AreEqual(0,MaxPageSearch);
    //     Assert.AreEqual(2,Page);
    //     
    // }


    [TestMethod]
    public async Task GetPage1()
    {
        
       Console.WriteLine(await _myState.HttpDevice.GetSearchPageCount("test"));
       Console.WriteLine(_deviceService.Pages("test"));
        
       Console.WriteLine();
    }


}
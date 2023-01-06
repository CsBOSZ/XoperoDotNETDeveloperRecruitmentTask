using System.Text;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using xopS.Services;

namespace xopS.IntegrationTests.Controllers;

[TestClass]
public class DeviceControllerTest
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private DeviceService _deviceService;

    public static Device DeviceFactor()
    {
        return new Device("unix", "test", "test", "amd", 4, new DateTime(2000,2,2));
    }

    [TestInitialize]
    public void Init()
    {
        
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        _deviceService = new DeviceService();
        
        
    }
    
    [TestCleanup]
    public void Clean()
    {

        _deviceService.DeleteAll();

    }
    
    [TestMethod]
    public async Task GetPages()
    {

        // ------
        
        var response = await _client.GetAsync("/Device/pages");
        
        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response.StatusCode);

    }

    [TestMethod]
    public async Task GetPagesByName()
    {

        // ------
        
        var response = await _client.GetAsync("/Device/pages/test");
        
        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response.StatusCode);

    }

    [TestMethod]
    public async Task GetDevices()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("/Device/0");
        var response2 = await _client.GetAsync("/Device/1");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }

    [TestMethod]
    public async Task SearchDevice()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("Device/searchByName/test");
        var response2 = await _client.GetAsync("Device/searchByName/p");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }
    
    [TestMethod]
    public async Task SearchDevice_byPage()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("Device/searchByName/test/0");
        var response2 = await _client.GetAsync("Device/searchByName/p/0");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }
    
    
    [TestMethod]
    public async Task DeviceOdt_GetDevices()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("/DeviceOdt/0");
        var response2 = await _client.GetAsync("/DeviceOdt/1");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }

    [TestMethod]
    public async Task DeviceOdt_SearchDevice()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("DeviceOdt/searchByName/test");
        var response2 = await _client.GetAsync("DeviceOdt/searchByName/p");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }
    
    [TestMethod]
    public async Task DeviceOdt_SearchDevice_byPage()
    {
        _deviceService.Add(DeviceFactor());
        
        // ------
        
        var response1 = await _client.GetAsync("DeviceOdt/searchByName/test/0");
        var response2 = await _client.GetAsync("DeviceOdt/searchByName/p/0");

        // ------
        
        Assert.AreEqual(System.Net.HttpStatusCode.OK,response1.StatusCode);
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound,response2.StatusCode);
        
    }
    
    [TestMethod]
    public async Task Post_device()
    {

        var httpContext = new StringContent(JsonConvert.SerializeObject(DeviceFactor()),Encoding.UTF8,"application/json");
        var req = await _client.PostAsync("Device",httpContext);
        
        // ------

        var response = _deviceService.SearchByName("test").Count();

        // ------

        Assert.AreEqual(System.Net.HttpStatusCode.OK,req.StatusCode);
        Assert.AreEqual(1,response);

    }
    
    [TestMethod]
    public async Task Delete_device()
    {
        _deviceService.Add(DeviceFactor());
        var req = await _client.DeleteAsync("Device/0");
        
        // ------

        var response = _deviceService.GetDevicesPage(0).Count();

        // ------

        Assert.AreEqual(System.Net.HttpStatusCode.OK,req.StatusCode);
        Assert.AreEqual(0,response);

    }
}
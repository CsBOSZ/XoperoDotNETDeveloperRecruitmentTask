using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using xopS.Services;

namespace xopS.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{

   private readonly IDeviceService _deviceService;
   private readonly IHubContext<DeviceHub> _hub;
   
   public DeviceController(IDeviceService deviceService,IHubContext<DeviceHub> deviceHub)
   {

      _deviceService = deviceService;
      _hub = deviceHub;

   } 
   
   [HttpGet("pages")]
   public int GetPages()
   {
      return _deviceService.Pages();
   }
   
   [HttpGet("pages/{name}")]
   public int GetPagesByName([FromRoute]string name)
   {
      return _deviceService.Pages(name);
   }

   [HttpGet("{page}")]
   public IEnumerable<Device> GetDevices([FromRoute]int page)
   {
      return _deviceService.GetDevicesPage(page);
   }
   
   [HttpGet("searchByName/{name}")]
   public IEnumerable<Device> SearchDevice([FromRoute]String name)
   {
      return _deviceService.SearchByName(name);
   }
   [HttpGet("searchByName/{name}/{page}")]
   public IEnumerable<Device> SearchDevicePage([FromRoute]String name,[FromRoute]int page)
   {
      return _deviceService.SearchByName(name,page);
   }
   
   [HttpGet("/DeviceOdt/{page}")]
   public IEnumerable<DeviceOdt> GetDevicesOdt([FromRoute]int page)
   {
      return DeviceService.ToOdt(_deviceService.GetDevicesPage(page));
   }
   
   [HttpGet("/DeviceOdt/searchByName/{name}")]
   public IEnumerable<DeviceOdt> SearchDeviceOdt([FromRoute]String name)
   {
      return DeviceService.ToOdt(_deviceService.SearchByName(name));
   }
   
   [HttpGet("/DeviceOdt/searchByName/{name}/{page}")]
   public IEnumerable<Device> SearchDevicePageOdt([FromRoute]String name,[FromRoute]int page)
   {
      return _deviceService.SearchByName(name,page);
   }
   
   [HttpPost]
   public void PostDevice([FromBody] Device device)
   {
      _deviceService.Add(device);
      _hub.Clients.All.SendAsync("RefreshDevices",_deviceService.GetDevicesPage(0),_deviceService.Pages());
   }

   [HttpDelete("{id}")]
   public void DeleteDevise([FromRoute] int id)
   {
      _deviceService.Remove(id);
      _hub.Clients.All.SendAsync("RefreshDevices",_deviceService.GetDevicesPage(0),_deviceService.Pages());  
   }


   }
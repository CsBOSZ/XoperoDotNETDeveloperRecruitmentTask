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
   public ActionResult<IEnumerable<Device>> GetDevices([FromRoute]int page)
   {
      var devices = _deviceService.GetDevicesPage(page).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
   }
   
   [HttpGet("searchByName/{name}")]
   public ActionResult<IEnumerable<Device>> SearchDevice([FromRoute]String name)
   {
      var devices = _deviceService.SearchByName(name).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
   }
   [HttpGet("searchByName/{name}/{page}")]
   public ActionResult<IEnumerable<Device>> SearchDevicePage([FromRoute]String name,[FromRoute]int page)
   {
      var devices = _deviceService.SearchByName(name,page).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
   }
   
   [HttpGet("/DeviceOdt/{page}")]
   public ActionResult<IEnumerable<DeviceOdt>> GetDevicesOdt([FromRoute]int page)
   {
      var devices =  DeviceService.ToOdt(_deviceService.GetDevicesPage(page)).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
   }
   
   [HttpGet("/DeviceOdt/searchByName/{name}")]
   public ActionResult<IEnumerable<DeviceOdt>> SearchDeviceOdt([FromRoute]String name)
   {
      var devices =  DeviceService.ToOdt(_deviceService.SearchByName(name)).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
   }
   
   [HttpGet("/DeviceOdt/searchByName/{name}/{page}")]
   public ActionResult<IEnumerable<DeviceOdt>> SearchDevicePageOdt([FromRoute]String name,[FromRoute]int page)
   {
      var devices =  _deviceService.SearchByName(name,page).ToList();
      return devices.Count == 0 ? NotFound() : Ok(devices);
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
using Microsoft.AspNetCore.SignalR;
using xopS.Services;

namespace xopS.Controllers;

public class DeviceHub : Hub
{
    //
    // private readonly IDeviceService _deviceService;
    //
    // public DeviceHub(IDeviceService deviceService)
    // {
    //     _deviceService = deviceService;
    // }

    // public Task AddDevice(Device device)
    // {
    //     _deviceService.Add(device);
    //     Console.WriteLine("jjjjjjjjjjjjjj");
    //      return Clients.All.SendAsync("AddDevices");
    // }
    public Task RefreshDevices(IEnumerable<Device> devices,int pages)
    {
        return Clients.All.SendAsync("RefreshDevices",devices,pages);
    }
    
}
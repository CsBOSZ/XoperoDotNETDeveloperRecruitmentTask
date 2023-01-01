using Microsoft.AspNetCore.SignalR;

namespace xopS.Controllers;

public class DeviceHub : Hub
{

    public Task RefreshDevices(IEnumerable<Device> devices,int pages)
    {
        return Clients.All.SendAsync("RefreshDevices",devices,pages);
    }
    
}
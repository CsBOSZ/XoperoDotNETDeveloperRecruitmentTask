using Flurl.Http;
using xopS;

namespace xopC;

public class HttpDevice : IHttpDevice
{
    readonly string BaseUrl;

    public HttpDevice(string baseUrl)
    {
        BaseUrl = baseUrl;
        // http://localhost:5076/
    }

    public async Task<List<DeviceOdt>> GetPage(int p,bool mode)
    {
        
            var tmp = $"{BaseUrl}{(mode ? "Device" : "DeviceOdt")}/{p}".GetAsync();
            return mode ? new List<DeviceOdt>(await tmp.ReceiveJson<List<Device>>()) : await tmp.ReceiveJson<List<DeviceOdt>>();
            
    }
    
    public async Task<List<DeviceOdt>> Search(String name,int p,bool mode)
    {
        
            var tmp =  $"{BaseUrl}{(mode ? "Device" : "DeviceOdt")}/searchByName/{name}/{p}".GetAsync();
            return mode ? new List<DeviceOdt>(await tmp.ReceiveJson<List<Device>>()) : await tmp.ReceiveJson<List<DeviceOdt>>();
            
    }

    public async Task<int> GetSearchPageCount(string name)
    {
        
        return await $"{BaseUrl}Device/pages/{name}".GetAsync().ReceiveJson<int>();
    } 

    public async Task PostDevice(Device device)
    {
       await $"{BaseUrl}Device".PostJsonAsync(device);
    } 
    
}
using xopS;

namespace xopC;

public interface IHttpDevice
{
    Task<List<DeviceOdt>> GetPage(int p,bool mode);
    Task<List<DeviceOdt>> Search(String name,int p,bool mode);
    Task<int> GetSearchPageCount(string name);
    Task PostDevice(Device device);
}
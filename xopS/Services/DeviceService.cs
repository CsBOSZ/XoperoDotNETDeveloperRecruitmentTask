namespace xopS.Services;



public class DeviceService : IDeviceService
{

    private static List<Device> _devices = new List<Device>();
    private readonly int _size;
    

    private void Sort()
    {
        _devices = _devices.OrderBy(x => x.UserName).ThenByDescending(x => x.Time).ToList();
    }

    public static IEnumerable<DeviceOdt> ToOdt(IEnumerable<Device> devices)
    {

        return devices.Select(x => new DeviceOdt(x));

    }

    public DeviceService(int size)
    {
        _size = size;
    }

    public DeviceService()
    {
        _size = 3;
    }

    public IEnumerable<Device> GetDevicesPage(int page)
    {
        
        for (int i = 0+(page*_size); i < _size+(page*_size); i++)
        {
            if (_devices.Count - 1 >= i)
                yield return _devices[i];
        }
        
    }

    public void Add(Device device)
    {
        _devices.Add(device);
        Sort();
    }

    public void Remove(int id)
    {
        _devices.RemoveAt(id);
        Sort();
    }

    public int Pages() => _devices.Count % _size == 0 ? _devices.Count / _size -1 : _devices.Count / _size;
    
    public IEnumerable<Device> SearchByName(string name)
    {
        foreach (var device in _devices.Where(x => x.UserName.Contains(name)))
        {
            yield return device;
        }

        foreach (var device in _devices.Where(x => x.MachineName.Contains(name)))
        {
            yield return device;
        }
    }

    public int Pages(string name)
    {
        var count = SearchByName(name).Count();
        return count % _size == 0 ? count / _size - 1 : count / _size;
    }

    public IEnumerable<Device> SearchByName(string name,int page)
    {
        List<Device> device = new List<Device>( SearchByName(name));
        for (int i = 0+(page*_size); i < _size+(page*_size); i++)
        {
            if (device.Count() - 1 >= i)
                yield return device[i];
        }
    }
}
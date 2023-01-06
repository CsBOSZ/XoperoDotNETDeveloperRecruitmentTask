using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using xopS.Services;

namespace xopS.Tests.Services;

[TestClass]
public class DeviceServiceTest
{
    private DeviceService _deviceService;

    public static Device DeviceFactor(string machineName, string userName , int time)
    {
        return new Device("unix", machineName, userName, "amd", 4, new DateTime(2000,2,time));
    }
    
    public static Device DeviceFactor()
    {
        return new Device("unix", "test", "test", "amd", 4, new DateTime(2000,2,2));
    }

    [TestInitialize]
    public void Init()
    {

        _deviceService = new DeviceService();
       

    }
    
    [TestCleanup]
    public void Clean()
    {

        _deviceService.DeleteAll();

    }
    
    [TestMethod]
    [DataRow(
            new [] {1,2,3}, 
        new [] {"a","a","b"},
                new [] {1,0,2}
        )]
    
    [DataRow(
        new [] {1,2,3}, 
        new [] {"a","b","c"},
        new [] {0,1,2}
    )]
    
    [DataRow(
        new [] {1,2,3}, 
        new [] {"a","a","a"},
        new [] {2,1,0}
    )]
    public void Sort(int[] time,string[] name,int[] index)
    {
        Device[] tab= new Device[3];
        for (int i = 0; i < 3; i++)
        {
            tab[i] = DeviceFactor(name[i], name[i], time[i]);
            _deviceService.Add(tab[i]);
            
        }

        //---
        
        var page1 = _deviceService.GetDevicesPage(0).ToList();
        
        _deviceService.Remove(2);
        
        var page2 = _deviceService.GetDevicesPage(0).ToList();
        
        //---
        
        for (int i = 0; i < 2; i++){
            Assert.AreEqual(tab[index[i]],page1[i]);
        }
        
        Assert.AreEqual(2,page2.Count);
        
        for (int i = 0; i < 1; i++){
            Assert.AreEqual(tab[index[i]],page2[i]);
        }

    }

    [TestMethod]
    public void GetDevicesPage_Pagination()
    {
        for (int i = 0; i < 5; i++)
        {
            _deviceService.Add(DeviceFactor());
        }

        //---
        
        List<Device> page0 = _deviceService.GetDevicesPage(0).ToList();
        List<Device> page1 = _deviceService.GetDevicesPage(1).ToList();

        DeviceService ds = new DeviceService(4);
        
        List<Device> page2 = ds.GetDevicesPage(0).ToList();
        List<Device> page3 = ds.GetDevicesPage(1).ToList();
        //---
        
        Assert.AreEqual(3,page0.Count);
        Assert.AreEqual(2,page1.Count);
        Assert.AreEqual(4,page2.Count);
        Assert.AreEqual(1,page3.Count);

    }

    [TestMethod]
    [DataRow(1,0)]
    [DataRow(2,0)]
    [DataRow(3,0)]
    [DataRow(4,1)]
    [DataRow(5,1)]
    [DataRow(6,1)]
    [DataRow(7,2)]
    [DataRow(8,2)]
    [DataRow(9,2)]
    [DataRow(45,14)]
    [DataRow(46,15)]
    public void Pages(int elements,int pages)
    {
        
        for (int i = 0; i < elements; i++)
        {
            _deviceService.Add(DeviceFactor());
        }

        //---
        
        int j = _deviceService.Pages();
        
        //---
        
        Assert.AreEqual(pages,j);

    }
    
    [TestMethod]
    [DataRow(1,0)]
    [DataRow(2,0)]
    [DataRow(3,0)]
    [DataRow(4,1)]
    [DataRow(5,1)]
    [DataRow(6,1)]
    [DataRow(7,2)]
    [DataRow(8,2)]
    [DataRow(9,2)]
    [DataRow(45,14)]
    [DataRow(46,15)]
    public void Pages_byName(int elements,int pages)
    {
        
        for (int i = 0; i < elements; i++)
        {
            _deviceService.Add(DeviceFactor());
        }

        //---
        
        int j = _deviceService.Pages("t");
        
        //---
        
        Assert.AreEqual(pages,j);

    }
    
    [TestMethod]
    public void SearchByName()
    {
        Device[] tab= new Device[5];
        for (int i = 0; i < 5; i++)
        {
            tab[i] = DeviceFactor($"{i}", $"{i+1}", 2);
            _deviceService.Add(tab[i]);
        }

        //---

        var search1 = tab[2];
        var search2 = tab[3];
        var results = _deviceService.SearchByName("3");
        
        //---
        
        Assert.IsTrue(results.Contains(search1));
        Assert.IsTrue(results.Contains(search2));
    }
    
    [TestMethod]
    public void SearchByName_Pagination()
    {
        for (int i = 0; i < 5; i++)
        {
            _deviceService.Add(DeviceFactor());
        }

        //---
        
        List<Device> page0 = _deviceService.SearchByName("t",0).ToList();
        List<Device> page1 = _deviceService.SearchByName("t",1).ToList();

        //---
        
        Assert.AreEqual(3,page0.Count);
        Assert.AreEqual(2,page1.Count);

    }
    
    [TestMethod]
    public void ToOdt()
    {

        List<Device> devices = new List<Device>() { DeviceFactor(), DeviceFactor(), DeviceFactor() };

        //---

        List<DeviceOdt> deviceOdts = DeviceService.ToOdt(devices).ToList();
        
        //---

        Assert.AreEqual(3,deviceOdts.Count);
        Assert.AreEqual(devices[1].OSVersion,deviceOdts[1].OSVersion);
        Assert.AreEqual(devices[1].MachineName,deviceOdts[1].MachineName);
        Assert.AreEqual(devices[1].UserName,deviceOdts[1].UserName);
        Assert.AreEqual(devices[1].Time,deviceOdts[1].Time);
    }
    
}
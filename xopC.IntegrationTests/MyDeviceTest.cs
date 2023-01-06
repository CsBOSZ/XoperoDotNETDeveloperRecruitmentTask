namespace xopC.IntegrationTests;

[TestClass]
public class MyDeviceTest
{

    [TestMethod]
    public void GetMyDevice()
    {

        var myDevice = MyDevice.GetMyDevice();
        
        Assert.IsNotNull(myDevice);

    }
    
    
}
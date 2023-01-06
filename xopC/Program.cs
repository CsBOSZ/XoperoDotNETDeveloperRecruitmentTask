using Microsoft.AspNetCore.SignalR.Client;
using xopS;
using System.Text.RegularExpressions;
using Flurl.Http;
using xopC;
using xopS.Services;


Device myDevice = MyDevice.GetMyDevice();

State myState = new State(0, "", 0, 0,new List<DeviceOdt>(), false);


string url = "http://localhost:5076/devicehub";
HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();

connection.On<IEnumerable<Device>,int>("RefreshDevices", (devices,pages) =>
{
    myState.Page.Clear();
    myState.Page.AddRange(myState.Mode ? devices : DeviceService.ToOdt(devices));
    myState.IndexPage = 0;
    myState.MaxPage = pages;
    myState.Print();
});
await connection.StartAsync();


// --------

var post = await "http://localhost:5076/Device".PostJsonAsync(myDevice);

// await connection.InvokeAsync<Task>("AddDevice", myDevice);

for (;;)
{
    
    var r = Console.ReadLine();

    if (r != null)
    {
        string p = null;
        string s = null;
        
        bool cm = r.Contains("-cm");

        if (Regex.IsMatch(r, "P<.*>"))
        {
            p = r.Substring(r.IndexOf("P") + 1);
            p = p.Substring(p.IndexOf("<")+1, p.IndexOf(">") - p.IndexOf("<") - 1).Trim();
        }

        if (Regex.IsMatch(r, "S<.*>"))
        {
            s = r.Substring(r.IndexOf("S") + 1);
            s = s.Substring(s.IndexOf("<")+1, s.IndexOf(">") - s.IndexOf("<") - 1);
        }


        if (cm)
        {
            myState.Mode = !myState.Mode;
        }

        if (cm && s is null && p is null)
        {
            if (myState.IndexName == "")
            {
                myState.GetPage(myState.IndexPage);
            }
            else
            {
                myState.Search(myState.IndexName,0);
            }
            
        }
        else if (s is not null)
        {
            if (p is not null)
            {
                myState.Search(s, int.TryParse(p, out var resultP) ? resultP : 0);
            }
            else
            {
                myState.Search(s,0);
            }

        }
        else if (p is not null)
        {
            myState.GetPage(int.TryParse(p, out var resultP) ? resultP : 0);
        }
    }
}






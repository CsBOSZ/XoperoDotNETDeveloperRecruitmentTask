using System.Collections;
using Microsoft.AspNetCore.SignalR.Client;
using xopS;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;
using Flurl.Http;


string processorName = null;

if (System.Environment.OSVersion.Platform == PlatformID.Unix)
{
    Process process = new Process();
    process.StartInfo.FileName = "cat";
    process.StartInfo.Arguments = "/proc/cpuinfo";
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardOutput = true;
    process.Start();

    string output = process.StandardOutput.ReadToEnd();

    processorName = output.Split('\n').FirstOrDefault(l => l.StartsWith("model name"));
    if (processorName is not null)
    {
        processorName = processorName.Substring(processorName.IndexOf(":") + 1).Trim();
    }
    else
    {
        processorName = output;
    }
}
else
{
    string query = "SELECT Name FROM Win32_Processor";
    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

    foreach (ManagementObject obj in searcher.Get())
    {
        processorName = obj["Name"].ToString();
    }
}

// --------

Device myDevice = new Device(
    System.Environment.OSVersion.VersionString,
    System.Environment.MachineName,
    System.Environment.UserName,
    processorName,
    System.Environment.ProcessorCount
);

int indexPage = 0;
String indexName = "";
int maxPage = 4;
List<Device> page = null;
List<DeviceOdt> pageOdt = null;
bool mode = false;

// --------

void Print()
{
    Console.Clear();
    Console.SetCursorPosition(0, 0);
    Console.WriteLine(
                    $"""
                    options:
                        Pages: p<0-{maxPage}> | 'p<1>'
                        Searching: s<name> | 's<arch>' or 's<arch> p<1>'
                        change mode: | -cm '-cm' or 'p<2> -cm'
                    [
                    """
        );
    foreach (var device in (IEnumerable)(mode ? page : pageOdt))
    {   
        Console.WriteLine("{");
        Console.WriteLine(device.ToString());
        Console.WriteLine("}");
    }
    
    Console.WriteLine($"]\n page: {(indexPage>=0 ? indexPage : "Searching")}");
    Console.Write("------------\n option: ");
}

async void GetPage(int p)
{
    if (p>=0&&p<=maxPage)
    {
        if (mode)
        {
            page = await $"http://localhost:5076/Device/{p}".GetAsync().ReceiveJson<List<Device>>();
        }
        
        indexPage = p;
        indexName = "";
        Print();
    }  
}

async void Search(String name,int p)
{
    
    page = await $"http://localhost:5076/Device/searchByName/{name}".GetAsync().ReceiveJson<List<Device>>();
    indexPage = -1;
    indexName = name;
    Print();

}

// --------

string url = "http://localhost:5076/devicehub";
HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();
    

connection.On<IEnumerable<Device>,int>("RefreshDevices", (devices,pages) =>
{
    page = devices.ToList();
    indexPage = 0;
    maxPage = pages;
    Print();

});
await connection.StartAsync();

// --------

var post = await "http://localhost:5076/Device".PostJsonAsync(myDevice);


for (;;)
{
    
    var r = Console.ReadLine();

    if (r != null)
    {
        string p = null;
        string s = null;

        if (Regex.IsMatch(r, "p<.*>"))
        {
            p = r.Substring(r.IndexOf("p") + 1);
            p = p.Substring(p.IndexOf("<")+1, p.IndexOf(">") - p.IndexOf("<") - 1).Trim();
        }

        if (Regex.IsMatch(r, "s<.*>"))
        {
            s = r.Substring(r.IndexOf("p") + 1);
            s = s.Substring(s.IndexOf("<")+1, s.IndexOf(">") - s.IndexOf("<") - 1);
        }

       
        
        
        if (s is not null)
        {
            if (p is not null)
            {
                Search(s, int.TryParse(p, out var resultP) ? resultP : 0);
            }
            else
            {
                Search(s,0);
            }

        }
        else if (p is not null)
        {
            GetPage(int.TryParse(p, out var resultP) ? resultP : 0);
        }
        
        
        // if (r.StartsWith("-p"))
        // {
        //     if (int.TryParse(r.Substring(r.IndexOf("p") + 1).Trim(), out var resultP))
        //     {
        //         GetPage(resultP);
        //     }
        // }
        // else if (r.StartsWith("-s"))
        // {
        //     Search(r.Substring(r.IndexOf("s") + 1).Trim());
        // }

    }
}






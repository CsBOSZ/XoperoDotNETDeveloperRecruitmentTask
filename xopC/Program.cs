using Microsoft.AspNetCore.SignalR.Client;
using xopS;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;
using Flurl.Http;
using xopS.Services;


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
    // TODO nie wiem czy dziala nie mam windows 
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
int maxPage = 0;
int maxPageSearch = 0;
List<DeviceOdt> page = new List<DeviceOdt>();
bool mode = false;

// --------

void Print()
{
    Console.Clear();
    Console.SetCursorPosition(0, 0);
    Console.WriteLine(
                    $"""
                    options:
                        Pages: P<0-{maxPage}> | 'P<1>'
                        Searching: S<name> P<{(maxPageSearch == 0? maxPageSearch : $"0-{maxPageSearch}")}> | 'S<arch>' or 'S<arch> P<{maxPageSearch}>'
                        change mode: -cm | '-cm' or 'p<2> -cm'
                    [
                    """
        );
    foreach (var device in page)
    {   
        Console.WriteLine("{");
        Console.WriteLine(device.ToString());
        Console.WriteLine("}");
    }
    
    Console.WriteLine($"]\n page: {(indexName == "" ? indexPage : $"{indexPage}\n Searching: {indexName}")}");
    Console.Write("------------\n option: ");
}

async void GetPage(int p)
{
    if (p>=0&&p<=maxPage)
    {
        page.Clear();
        var tmp = $"http://localhost:5076/{(mode ? "Device" : "DeviceOdt")}/{p}".GetAsync();
        page.AddRange(mode ?await tmp.ReceiveJson<List<Device>>() : await tmp.ReceiveJson<List<DeviceOdt>>());
        indexPage = p;
        indexName = "";
        maxPageSearch = 0;
        Print();
    }
    else
    {
        Console.Write("\n option: ");
    }
}

async void Search(String name,int p)
{
    
    maxPageSearch = await $"http://localhost:5076/Device/pages/{name}".GetAsync().ReceiveJson<int>();
    if (p >= 0 && p <= maxPageSearch)
    {
        page.Clear();
        var tmp =  $"http://localhost:5076/{(mode ? "Device" : "DeviceOdt")}/searchByName/{name}/{p}".GetAsync();
        page.AddRange(mode ?await tmp.ReceiveJson<List<Device>>() : await tmp.ReceiveJson<List<DeviceOdt>>());
        indexPage = p;
        indexName = name;
        Print();
    }
    else
    {
        Console.Write("\n option: ");
    }
}

// --------

string url = "http://localhost:5076/devicehub";
HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();

connection.On<IEnumerable<Device>,int>("RefreshDevices", (devices,pages) =>
{
    page.Clear();
    page.AddRange(mode ? devices : DeviceService.ToOdt(devices));
    indexPage = 0;
    maxPage = pages;
    Print();
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
            mode = !mode;
        }

        if (cm && s is null && p is null)
        {
            if (indexName == "")
            {
                GetPage(indexPage);
            }
            else
            {
                Search(indexName,0);
            }
            
        }
        else if (s is not null)
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
    }
}






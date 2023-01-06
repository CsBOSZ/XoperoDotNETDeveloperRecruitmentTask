using Flurl.Http;
using xopS;

namespace xopC;



public class State
{
    public State(int indexPage, string indexName, int maxPage, int maxPageSearch, List<DeviceOdt> page, bool mode)
    {
        IndexPage = indexPage;
        IndexName = indexName;
        MaxPage = maxPage;
        MaxPageSearch = maxPageSearch;
        Page = page;
        Mode = mode;
    }

    public int IndexPage { get; set;}
    public string IndexName { get; set;}
    public int MaxPage { get; set;}
    public int MaxPageSearch { get; set;}
    public List<DeviceOdt> Page { get; set;}
    public bool Mode { get; set;}
    
    public void Print()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(
            $"""
                    options:
                        Pages: P<0-{MaxPage}> | 'P<1>'
                        Searching: S<name> P<{(MaxPageSearch == 0? MaxPageSearch : $"0-{MaxPageSearch}")}> | 'S<arch>' or 'S<arch> P<{MaxPageSearch}>'
                        change mode: -cm | '-cm' or 'p<2> -cm'
                    [
                    """ 
        );
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var device in Page)
        {   
            Console.WriteLine("{");
            Console.WriteLine(device.ToString());
            Console.WriteLine("}");
        }
    
        Console.WriteLine($"]\n page: {(IndexName == "" ? IndexPage : $"{IndexPage}\n Searching: {IndexName}")}");
        Console.Write("------------\n option: ");
    }
    
    public async void GetPage(int p)
    {
        if (p>=0&&p<=MaxPage)
        {
            Page = new List<DeviceOdt>();
            var tmp = $"http://localhost:5076/{(Mode ? "Device" : "DeviceOdt")}/{p}".GetAsync();
            Page.AddRange(Mode ?await tmp.ReceiveJson<List<Device>>() : await tmp.ReceiveJson<List<DeviceOdt>>());
            IndexPage = p;
            IndexName = "";
            MaxPageSearch = 0;
            Print();
        }
        else
        {
            Console.Write("\n option: ");
        }
    }
    
    public async void Search(String name,int p)
    {
    
        MaxPageSearch = await $"http://localhost:5076/Device/pages/{name}".GetAsync().ReceiveJson<int>();
        if (p >= 0 && p <= MaxPageSearch)
        {
            Page.Clear();
            var tmp =  $"http://localhost:5076/{(Mode ? "Device" : "DeviceOdt")}/searchByName/{name}/{p}".GetAsync();
            Page.AddRange(Mode ?await tmp.ReceiveJson<List<Device>>() : await tmp.ReceiveJson<List<DeviceOdt>>());
            IndexPage = p;
            IndexName = name;
            Print();
        }
        else
        {
            Console.Write("\n option: ");
        }
    }

}
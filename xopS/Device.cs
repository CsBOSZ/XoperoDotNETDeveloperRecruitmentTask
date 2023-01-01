namespace xopS;

public class Device 
{
    public string OSVersion { get; set; }
    public string MachineName { get; set; }
    public string UserName { get; set; }
    public string ProcessorName { get; set; }
    public int ProcessorCount { get; set; }
    
    public DateTime Time { get; set; }

    public Device(string osVersion, string machineName, string userName, string processorName, int processorCount)
    {
        OSVersion = osVersion;
        MachineName = machineName;
        UserName = userName;
        ProcessorName = processorName;
        ProcessorCount = processorCount;
        Time = DateTime.Now;
    }

    public override string ToString()
    {
        return $"""
                    osVersion = {OSVersion};
                    machineName = {MachineName};
                    userName = {UserName};
                    processorName = {ProcessorName };
                    processorCount = {ProcessorCount};
                    date = {Time.ToLongDateString()};
                    time = {Time.ToLongTimeString()};
                """;
    }
}
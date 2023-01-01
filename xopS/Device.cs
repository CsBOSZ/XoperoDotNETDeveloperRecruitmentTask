namespace xopS;

public class Device : DeviceOdt
{
    public string ProcessorName { get; set; }
    public int ProcessorCount { get; set; }
    
    public Device(string osVersion, string machineName, string userName, string processorName, int processorCount) : base(osVersion,machineName,userName)
    {
        ProcessorName = processorName;
        ProcessorCount = processorCount;
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
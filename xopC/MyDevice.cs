using System.Diagnostics;
using System.Management;
using xopS;

namespace xopC;

public class MyDevice
{

    public static Device GetMyDevice()
    {
        
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

        return new Device(
            System.Environment.OSVersion.VersionString,
            System.Environment.MachineName,
            System.Environment.UserName,
            processorName,
            System.Environment.ProcessorCount
        );
        
        
    }

}
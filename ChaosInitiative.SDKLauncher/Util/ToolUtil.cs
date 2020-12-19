using System;
using System.Diagnostics;

namespace SDKLauncher.Util
{
    public static class ToolUtil
    {
        public static Process LaunchTool(string binDirectory, string executableName, string arguments = "")
        {
            string extension = OperatingSystem.IsWindows() ? ".exe" : "";
            string executablePath = $"{binDirectory}/{executableName}{extension}";
            return Process.Start(executablePath, arguments);
        }
    }
}
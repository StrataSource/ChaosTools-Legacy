using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;

namespace SDKLauncher.Util
{
    public static class MountUtil
    {
        

        public static bool IsValidSearchPath(string searchPath)
        {
            return File.Exists($"{searchPath}/gameinfo.txt");
        }

        public static readonly FileDialogFilter GameInfoFileFilter = new FileDialogFilter()
        {
            Name = "Game Info",
            Extensions = new List<string>
            {
                "gameinfo.txt"
            }
        };
        
        public static string GetPlatformString()
        {
            string arch = Environment.Is64BitOperatingSystem ? "64" : "32";
            if(OperatingSystem.IsWindows())
                return $"win{arch}";
            if (OperatingSystem.IsLinux())
                return $"linux{arch}";
            if (OperatingSystem.IsMacOS())
                return $"osx{arch}";
            throw new Exception("Invalid OS. You need to run windows, linux or mac.");
        }
    }
}
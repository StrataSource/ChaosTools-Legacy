using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;

namespace ChaosInitiative.SDKLauncher.Util
{
    public static class MountUtil
    {
        public static bool IsValidSearchPath(string searchPath)
        {
            return File.Exists($"{searchPath}/gameinfo.txt");
        }

        public static readonly FileDialogFilter GameInfoFileFilter = new()
        {
            Name = "Game Info",
            Extensions = new List<string>
            {
                "txt"
            }
        };

        private static readonly Dictionary<PlatformID, string> PlatformNames = new()
        {
            { PlatformID.Win32NT, "win" },
            { PlatformID.MacOSX, "osx" },
            { PlatformID.Unix, "linux" },
        };
        
        public static string GetPlatformString()
        {
            string arch = Environment.Is64BitOperatingSystem ? "64" : "32";
            string name = PlatformNames[Environment.OSVersion.Platform];
            return name + arch;
        }
    }
}
using System;

namespace SDKLauncher.Util
{
    public static class SteamHelper
    {
        public static string DependencyNameWindows = "steam_api64.dll";
        public static string DependencyNameLinuxMac = "libsteam_api.so";

        public static string ApiDependencyName
        {
            get
            {
                if (OperatingSystem.IsWindows())
                    return DependencyNameWindows;

                if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                    return DependencyNameLinuxMac;

                throw new Exception("This program only works on windows, linux and mac");
            }
        }
    }
}
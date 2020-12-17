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

        public static FileDialogFilter GameInfoFileFilter = new FileDialogFilter()
        {
            Name = "Game Info",
            Extensions = new List<string>
            {
                "gameinfo.txt"
            }
        };
    }
}
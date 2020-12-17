using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using SDKLauncher.Util;
using Steamworks;

namespace SDKLauncher.Models
{
    public class Mount : INotifyPropertyChanged
    {

        public Mount()
        {
            SearchPaths = new ObservableCollection<string>();
            PrimarySearchPath = string.Empty;
        }

        // -------------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // -------------------------------------------------------------------------------------

        private string _mountPath;

        public string MountPath
        {
            get
            {
                if (AppId == 0) return _mountPath;

                AppId_t appid = new AppId_t((uint)AppId);
                if (!SteamApps.BIsAppInstalled(appid)) return null;

                string dir;
                SteamApps.GetAppInstallDir(appid, out dir, 1024);

                return dir;
            }
            set {
                _mountPath = value;
                NotifyPropertyChanged(nameof(MountPath));
                NotifyPropertyChanged(nameof(AvailableSearchPaths));
            }
        }
        private long _appId;
        public long AppId 
        {
            get => _appId;
            set
            {
                _appId = value;
                NotifyPropertyChanged(nameof(MountPath));
                NotifyPropertyChanged(nameof(AvailableSearchPaths));
            }
        }

        public string PrimarySearchPath { get; set; }
        public bool IsRequired { get; set; }
        public ObservableCollection<string> SearchPaths { get; set; }

        public string BinDirectory
        {
            get
            {
                string binPath = $"{MountPath}/bin";
                string platformSpecificBinPath = $"{binPath}/{PlatformString}";

                if (Directory.Exists(platformSpecificBinPath))
                {
                    binPath = platformSpecificBinPath;
                }

                return binPath;
            }
        }

        public string PrimarySearchPathDirectory => $"{MountPath}/{PrimarySearchPath}";

        // Don't you love it? :)
        public List<string> AvailableSearchPaths =>
            string.IsNullOrWhiteSpace(MountPath) ? new List<string>() :
                Directory.GetDirectories(MountPath)                                                    // Get all subdirectories
                    .Where(MountUtil.IsValidSearchPath)                                                // That have a gameinfo
                    .Where(d => !SearchPaths.Contains(Path.GetDirectoryName(d) ?? ""))       // Which are not already 
                    .Select(d => d.Split(Path.DirectorySeparatorChar).Last())
                    .ToList();

        private string PlatformString
        {
            get
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
}
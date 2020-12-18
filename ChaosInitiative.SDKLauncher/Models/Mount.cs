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
        // TODO: Move this to some reusable interface
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
                try
                {
                    if (SteamApps.IsAppInstalled(AppId))
                    {
                        return SteamApps.AppInstallDir(AppId);
                    }
                } catch(NullReferenceException) {}

                return null;
            }
            set {
                _mountPath = value;
                NotifyPropertyChanged(nameof(MountPath));
                NotifyPropertyChanged(nameof(AvailableSearchPaths));
            }
        }
        private int _appId;
        public int AppId 
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
                string platformSpecificBinPath = $"{binPath}/{MountUtil.GetPlatformString()}";

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

        
    }
}
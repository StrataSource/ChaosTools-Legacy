using Steamworks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SDKLauncher.Models
{
    public class Mount : INotifyPropertyChanged
    {

        public Mount()
        {
            Namespaces = new ObservableCollection<string>();
        }

        // -------------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // -------------------------------------------------------------------------------------

        private string _path;

        public string Path
        {
            get
            {
                if (AppId == null) return _path;

                AppId_t appid = new AppId_t((uint)AppId);
                if (!SteamApps.BIsAppInstalled(appid)) return null;

                string dir;
                SteamApps.GetAppInstallDir(appid, out dir, 1024);

                return dir;
            }
            set {
                _path = value;
                NotifyPropertyChanged(nameof(Path));
                NotifyPropertyChanged(nameof(AvailableNamespaces));
            }
        }
        private uint? _appId;
        public uint? AppId 
        {
            get => _appId;
            set
            {
                _appId = value;
                NotifyPropertyChanged(nameof(Path));
                NotifyPropertyChanged(nameof(AvailableNamespaces));
            }
        }

        public string PrimaryNamespace { get; set; }
        public bool IsRequired { get; set; }
        public ObservableCollection<string> Namespaces { get; set; }

        public string GetBinDirectory()
        {
            string binPath = $"{Path}/bin";
            string platformSpecificBinPath = $"{binPath}/{GetPlatformString()}";

            if (Directory.Exists(platformSpecificBinPath))
            {
                binPath = platformSpecificBinPath;
            }

            return binPath;
        }
        // TODO: GetPlatformString doesn't detect mac yet! (mac gets treated the same as linux...)
        private string GetPlatformString()
        {
            string arch = Environment.Is64BitOperatingSystem ? "64" : "32";
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT: return $"win{arch}";
                case PlatformID.Unix: return $"linux{arch}";
                default: return null;
            }
        }

        // Don't you love it? :)
        public List<string> AvailableNamespaces
        {
            get =>
                string.IsNullOrWhiteSpace(Path) ? new List<string>() :
                Directory.GetDirectories(Path)
                .Where(d => File.Exists($"{d}/gameinfo.txt"))
                .Where(d => !Namespaces.Contains(System.IO.Path.GetDirectoryName(d) ?? ""))
                .Select(d => d.Split(System.IO.Path.DirectorySeparatorChar).Last())
                .ToList();

        }
    }
}

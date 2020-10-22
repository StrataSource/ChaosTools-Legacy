using Avalonia.Data;
using Steamworks;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace SDKLauncher.Models
{
    public class Mount : INotifyPropertyChanged
    {

        public Mount()
        {

        }

        public Mount(uint appid)
        {
            AppId = appid;
        }
        
        public Mount(string path)
        {
            Path = path;
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
            }
        }

        public string PrimaryNamespace { get; set; }
        public bool IsRequired { get; set; }
        //public ObservableCollection<Namespace> Namespaces { get; set; }
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
            string arch = System.Environment.Is64BitOperatingSystem ? "64" : "32";
            switch(System.Environment.OSVersion.Platform)
            {
                case System.PlatformID.Win32NT: return $"win{arch}";
                case System.PlatformID.Unix: return $"linux{arch}";
                default: return null;
            }
        }
    }
}

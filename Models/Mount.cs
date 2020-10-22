using Steamworks;
using System.IO;

namespace SDKLauncher.Models
{
    public class Mount
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
            set => _path = value;
        }
        public uint? AppId { get; set; }

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

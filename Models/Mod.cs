using Steamworks;

namespace SDKLauncher.Models
{
    public class Mod
    {
        public string Name { get; set; }
        private string _path;
        public string Path {
            get
            {
                if (AppId == null) return _path;

                AppId_t appid = new AppId_t((uint) AppId);
                if (!SteamApps.BIsAppInstalled(appid)) return null;

                string dir;
                SteamApps.GetAppInstallDir(appid, out dir, 1024);

                return dir;
            }
            set => _path = value;
        }
        public uint? AppId { get; set; }

    }
}

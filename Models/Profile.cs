using System.Collections.ObjectModel;

namespace SDKLauncher.Models
{
    public class Profile
    {
        public Mod Mod { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Mount> Mounts { get; set; } = new ObservableCollection<Mount>();

        public static Profile GetDefaultProfile()
        {
            return new Profile
            {
                Name = "P2CE - Default",
                Mod = new Mod
                {
                    Name = "Portal 2: Community Edition",
                    Mount = new Mount
                    {
                        AppId = 440000,
                        PrimaryNamespace = "p2ce"
                    }
                },
                Mounts =
                {
                    new Mount
                    {
                        AppId = 620,
                        IsRequired = true,
                        PrimaryNamespace = "portal2",
                        Namespaces =
                        {
                            "portal2",
                            "portal2_dlc1",
                            "portal2_dlc2"
                        }
                    }
                }
            };
        }
    }
}

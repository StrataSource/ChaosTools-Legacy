using DynamicData;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SDKLauncher.Models
{
    public class Profile
    {
        public Mod Mod { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Mount> Mounts { get; set; }

        public static Profile GetDefaultProfile()
        {
            return new Profile()
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
                }
            };
        }
    }
}

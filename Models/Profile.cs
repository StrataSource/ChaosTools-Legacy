using System.Collections.Generic;

namespace SDKLauncher.Models
{
    public class Profile
    {
        public Mod Mod { get; set; }
        public string Name { get; set; }
        public List<Mount> Mounts { get; set; }
    }
}

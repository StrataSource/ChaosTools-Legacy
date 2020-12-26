using System;

namespace ChaosInitiative.SDKLauncher.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public Mount Mount { get; set; }

        public bool Equals(Mod other)
        {
            return Name == other.Name && 
                   Equals(Mount, other.Mount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mod);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Mount);
        }
    }
}

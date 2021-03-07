using System;
using ReactiveUI.Fody.Helpers;

namespace ChaosInitiative.SDKLauncher.Models
{
    public class Mod
    {
        [Reactive]
        public string Name { get; set; }
        public Mount Mount { get; set; }
        public string ExecutableName { get; set; }
        public string LaunchArguments { get; set; }

        public bool Equals(Mod other)
        {
            return Name == other.Name && 
                   ExecutableName == other.ExecutableName &&
                   LaunchArguments == other.LaunchArguments &&
                   Equals(Mount, other.Mount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mod);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Mount, ExecutableName, LaunchArguments);
        }
    }
}

using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ChaosInitiative.SDKLauncher.Models
{
    public class Profile : ReactiveObject
    {
        [Reactive]
        public Mod Mod { get; set; }
        
        [Reactive]
        public string Name { get; set; }
        
        [Reactive]
        public Mount AdditionalMount { get; set; }
        
        public static Profile GetDefaultProfile()
        {
            return new Profile
            {
                Name = "P2CE - Default",
                Mod = new Mod
                {
                    Name = "Portal 2: Community Edition",
                    ExecutableName = "chaos",
                    LaunchArguments = "",
                    Mount = new Mount
                    {
                        AppId = 440000,
                        PrimarySearchPath = "p2ce",
                        IsRequired = true
                    }
                },
                AdditionalMount = new Mount()
            };
        }

        public bool Equals(Profile other)
        {
            return Equals(Mod, other.Mod) && 
                   Name == other.Name && 
                   AdditionalMount.Equals(other.AdditionalMount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Profile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mod, Name, AdditionalMount);
        }
        
    }
}

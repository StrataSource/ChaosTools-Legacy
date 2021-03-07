using ChaosInitiative.SDKLauncher.Models;
using NUnit.Framework;

namespace ChaosInitiative.SDKLauncher.Test
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void TestProfileIsValid()
        {
            Profile profile = Profile.GetDefaultProfile();
            
            Assert.That(profile.Name, Is.Not.Null.Or.Empty);
            Assert.That(profile.Mod, Is.Not.Null);
            Assert.That(profile.Mod.Name, Is.Not.Null.Or.Empty);
            Assert.That(profile.Mod.Mount, Is.Not.Null);
            Assert.That(profile.Mod.Mount.AppId, Is.Not.Null.And.Not.Zero);
            Assert.That(profile.Mod.Mount.IsRequired, Is.True);
            Assert.That(profile.Mod.Mount.PrimarySearchPath, Is.Not.Null.Or.Empty);
            Assert.That(profile.AdditionalMount, Is.Not.Null);
        }

        [Test]
        public void TestProfileEquals()
        {
            Profile profile1 = new Profile
            {
                Name = "profile",
                AdditionalMount =  
                new Mount
                {
                    AppId = 0,
                    IsRequired = true,
                    MountPath = "testpath",
                    SelectedSearchPaths = 
                    {
                        "path1", "path2"
                    },
                    PrimarySearchPath = "primary_search_path",
                },
                Mod = new Mod
                {
                    Name = "My mod",
                    Mount = new Mount
                    {
                        AppId = 0,
                        IsRequired = true,
                        MountPath = "maintestpath",
                        SelectedSearchPaths = 
                        {
                            "path3", "path4", "path5"
                        },
                        PrimarySearchPath = "main_primary_search_path",
                    }
                }
            };
            
            Profile profile2 = new Profile
            {
                Name = "profile",
                AdditionalMount = 
                new Mount
                {
                    AppId = 0,
                    IsRequired = true,
                    MountPath = "testpath",
                    SelectedSearchPaths = 
                    {
                        "path1", "path2"
                    },
                    PrimarySearchPath = "primary_search_path",
                },
                Mod = new Mod
                {
                    Name = "My mod",
                    Mount = new Mount
                    {
                        AppId = 0,
                        IsRequired = true,
                        MountPath = "maintestpath",
                        SelectedSearchPaths = 
                        {
                            "path3", "path4", "path5"
                        },
                        PrimarySearchPath = "main_primary_search_path",
                    }
                }
            };
            
            Assert.That(profile1, Is.EqualTo(profile2));
        }

        [Test]
        public void TestProfileHashCode()
        {
            Profile profile = Profile.GetDefaultProfile();
            Assert.That(profile.GetHashCode(), Is.Not.Zero);
        }
    }
}
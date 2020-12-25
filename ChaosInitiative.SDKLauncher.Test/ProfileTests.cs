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
            Assert.That(profile.Mounts, Is.Not.Empty);
        }
    }
}
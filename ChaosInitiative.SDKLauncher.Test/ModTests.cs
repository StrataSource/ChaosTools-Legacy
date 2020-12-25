using ChaosInitiative.SDKLauncher.Models;
using NUnit.Framework;

namespace ChaosInitiative.SDKLauncher.Test
{
    [TestFixture]
    public class ModTests
    {
        [Test]
        public void TestMods()
        {
            Mod mod = new Mod
            {
                Name = "Portal: Revolution",
                Mount = new Mount
                {
                    AppId = 601360,
                    IsRequired = false,
                    PrimarySearchPath = "revolution"
                }
            };
            
            Assert.That(mod.Name, Is.Not.Null.Or.Empty);
            Assert.That(mod.Mount, Is.Not.Null);
        }
    }
}
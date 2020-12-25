using System.Diagnostics;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using NUnit.Framework;
using Steamworks;

namespace ChaosInitiative.SDKLauncher.Test
{
    [TestFixture]
    public class ToolTestsWithSteam
    {

        [SetUp]
        public void Setup()
        {
            Assume.That(() => SteamClient.Init(440000), Throws.Nothing);
        }

        [TearDown]
        public void TearDown()
        {
            SteamClient.Shutdown();
        }
        
        [Test]
        public void TestLaunchTool()
        {
            Mount mount = new Mount()
            {
                AppId = 440000,
                IsRequired = true
            };
            Assume.That(mount.MountPath, Is.Not.Null.Or.Empty);

            Process process = null;
            Assert.That(() =>
            {
                process = ToolUtil.LaunchTool(mount.BinDirectory, "hammer");
                Assert.That(process.HasExited, Is.False);
            }, Throws.Nothing);
            
            process.Kill(true);
            Assert.That(process.HasExited, Is.True);
        }
    }
}
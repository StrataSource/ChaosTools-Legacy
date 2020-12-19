using System.IO;
using System.Text.Json;
using NUnit.Framework;
using SDKLauncher.Models;

namespace ChaosInitiative.SDKLauncher.Test
{
    [TestFixture]
    public class AppConfigTests
    {
        [Test]
        public void TestDefaultAppConfigIsValid()
        {
            AppConfig config = AppConfig.CreateDefaultConfig();
            Assert.That(config.DefaultProfileIndex, Is.Zero);
            
            Assert.That(config.Profiles, Is.Not.Empty);
        }

        [Test, Order(1)]
        public void TestSaveConfigWorks()
        {
            AppConfig config = AppConfig.CreateDefaultConfig();
            Assert.That(() =>
            {
                config.Save();
            }, Throws.Nothing);
            
            Assert.That(File.Exists("config.json"), Is.True);
        }

        [Test, Order(2)]
        public void TestLoadConfigWorks()
        {
            Assume.That(File.Exists("config.json"), Is.True);

            Assert.That(() =>
            {
                AppConfig config = AppConfig.LoadConfig();
                Assert.That(config, Is.Not.Null);
            }, Throws.Nothing);
            
            File.Delete("config.json");
        }

        [Test, Order(3)]
        public void TestLoadConfigFailsWhenConfigDoesNotExist()
        {
            Assume.That(File.Exists("config.json"), Is.False);
            Assert.That(AppConfig.LoadConfig, Throws.Exception.TypeOf<FileNotFoundException>());
        }
        
        [Test, Order(4)]
        public void TestLoadConfigFailsWhenConfigIsCorrupted()
        {
            Assume.That(File.Exists("config.json"), Is.False);
            File.WriteAllText("config.json", "nbdsjvkjfdsbjhvsbjhv");
            Assert.That(AppConfig.LoadConfig, Throws.Exception.TypeOf<JsonException>());
        }
    }
}

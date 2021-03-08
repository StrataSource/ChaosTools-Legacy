using System.IO;
using System.Text.Json;
using ChaosInitiative.SDKLauncher.Models;
using NUnit.Framework;

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
            
            Assert.That(AppConfig.IsConfigSaved, Is.True);
        }

        [Test, Order(2)]
        public void TestLoadConfigWorks()
        {
            Assume.That(AppConfig.IsConfigSaved, Is.True, "File config.json must exist on disk to load config");

            Assert.That(() =>
            {
                AppConfig config = AppConfig.LoadConfig();
                Assert.That(config, Is.Not.Null);
            }, Throws.Nothing);
            
            AppConfig.DeleteSavedConfig();
        }

        [Test, Order(3)]
        public void TestLoadConfigFailsWhenConfigDoesNotExist()
        {
            Assume.That(File.Exists("config.json"), Is.False, "File config.json must not exist on disk");
            Assert.That(AppConfig.LoadConfig, Throws.Exception.TypeOf<FileNotFoundException>());
        }
        
        [Test, Order(4)]
        public void TestLoadConfigFailsWhenConfigIsCorrupted()
        {
            Assume.That(AppConfig.IsConfigSaved, Is.False, "File config.json must not exist on disk");
            File.WriteAllText("config.json", "nbdsjvkjfdsbjhvsbjhv");
            Assert.That(AppConfig.LoadConfig, Throws.Exception.TypeOf<JsonException>());
            AppConfig.DeleteSavedConfig();
        }

        [Test, Order(5)]
        public void TestLoadConfigOrDefaultCreatesDefaultWhenFileNotFound()
        {
            Assume.That(AppConfig.IsConfigSaved, Is.False, "File config.json must not exist on disk");
            
            AppConfig loadedConfig = null;
            Assert.That(() => loadedConfig = AppConfig.LoadConfigOrCreateDefault(), Throws.Nothing);
            AppConfig defaultConfig = AppConfig.CreateDefaultConfig();
            
            Assert.That(loadedConfig, Is.Not.Null);
            Assert.That(loadedConfig.DefaultProfileIndex, Is.Zero);
            Assert.That(loadedConfig.Profiles.Count, Is.EqualTo(defaultConfig.Profiles.Count));
            
            Assert.That(AppConfig.IsConfigSaved, Is.True);
            AppConfig.DeleteSavedConfig();
        }

        [Test, Order(6)]
        public void TestLoadConfigOrDefaultThrowsWhenJsonCorrupted()
        {
            Assume.That(AppConfig.IsConfigSaved, Is.False, "File config.json must not exist on disk");
            File.WriteAllText("config.json", "nbdsjvkjfdsbjhvsbjhv");
            Assert.That(AppConfig.LoadConfigOrCreateDefault, Throws.Exception.TypeOf<JsonException>());
            AppConfig.DeleteSavedConfig();
        }
        
        [Test(Description = "Save a default config, add a profile to it, save, confirm that it saved properly")]
        public void TestAddProfileSaveConfig()
        {
            Assume.That(AppConfig.IsConfigSaved, Is.False, "File config.json must not exist on disk");
            AppConfig config = AppConfig.CreateDefaultConfig();
            config.Save();
            
            config.Profiles.Add(new Profile
            {
                Name = "Other Profile",
                AdditionalMount = null,
                Mod = new Mod
                {
                    Name = "Other Profile's Mod",
                    ExecutableName = "chaos",
                    LaunchArguments = "",
                    Mount = new Mount
                    {
                        AppId = 620
                    }
                }
            });
            
            config.Save();
            
            AppConfig loadedConfig = AppConfig.LoadConfig();
            Assert.That(loadedConfig, Is.Not.Null);
            Assert.That(loadedConfig.Profiles, Is.Not.Null.And.Count.EqualTo(2));
            Assert.That(loadedConfig.Profiles[1].Name, Is.EqualTo("Other Profile"));
        }

        [Test]
        public void TestAppConfigEquals()
        {
            AppConfig config1 = AppConfig.CreateDefaultConfig();
            AppConfig config2 = AppConfig.CreateDefaultConfig();
            
            Assert.That(config1, Is.EqualTo(config2));
        }

        [Test]
        public void TestAppConfigHashCode()
        {
            AppConfig config = AppConfig.CreateDefaultConfig();
            
            Assert.That(config.GetHashCode(), Is.Not.Zero);
        }
    }
}

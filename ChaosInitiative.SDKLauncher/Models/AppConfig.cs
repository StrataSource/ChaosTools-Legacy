using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ChaosInitiative.SDKLauncher.Models
{
    public class AppConfig
    {
        private const string ConfigName = "config.json";

        private static readonly JsonSerializerOptions ConfigJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        // ============================================

        public IList<Profile> Profiles { get; set; }
        private int _defaultProfileIndex;
        
        /// <summary>
        /// Property accessor for the profile index.
        /// Value is clamped between 0 and size of Profiles list
        /// </summary>
        public int DefaultProfileIndex { 
            get => _defaultProfileIndex;
            set => _defaultProfileIndex = Math.Clamp(value, 0, Math.Max(Profiles.Count - 1, 0));
        }

        // ============================================

        /// <summary>
        /// Saves the this config to config.json in the working directory
        /// </summary>
        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(this, ConfigJsonSerializerOptions);
            File.WriteAllText(ConfigName, jsonString);
        }
        
        /// <summary>
        /// Loads the config.json file. Throws JsonException
        /// </summary>
        public static AppConfig LoadConfig()
        {
            string jsonString = File.ReadAllText(ConfigName);
            
            //     This throws JsonException, which should be passed to the method caller for processing
            return JsonSerializer.Deserialize<AppConfig>(jsonString);
        }

        /// <summary>
        /// Checks if the config is saved to disk.
        /// This does not check if the config is valid or the current version, just if some file with that name exists
        /// </summary>
        /// <returns>Whether or not config.json exists in working directory</returns>
        public static bool IsConfigSaved() => File.Exists(ConfigName);

        /// <summary>
        /// Deletes the config if the file exists
        /// </summary>
        public static void DeleteSavedConfig()
        {
            if(IsConfigSaved())
                File.Delete(ConfigName);
        }
        
        /// <summary>
        /// This is the config that should be used when generating a default config template
        /// Default values:
        /// - DefaultProfileIndex: 0
        /// - Profile: Default Profile
        /// </summary>
        /// <returns>Instance of AppConfig using default values</returns>
        public static AppConfig CreateDefaultConfig()
        {
            return new AppConfig()
            {
                Profiles = new List<Profile>
                {
                    Profile.GetDefaultProfile()
                }
            };
        }
        
        /// <summary>
        /// Tries to load the config. If the file doesn't exist, a new default config will be created and saved.
        /// Throws JsonException if file exists but is corrupted
        /// </summary>
        /// <returns>Instance of AppConfig that is either loaded from disk or generated if file doesn't exist</returns>
        public static AppConfig LoadConfigOrCreateDefault()
        {
            AppConfig config;
            if (IsConfigSaved())
                config = LoadConfig();
            else
            {
                config = CreateDefaultConfig();
                config.Save();
            }
            return config;
        }

        public bool Equals(AppConfig other)
        {
            return _defaultProfileIndex == other._defaultProfileIndex && 
                   Profiles.SequenceEqual(other.Profiles);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AppConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_defaultProfileIndex, Profiles);
        }
    }

}
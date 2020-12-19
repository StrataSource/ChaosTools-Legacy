using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SDKLauncher.Models
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
            get => Math.Clamp(_defaultProfileIndex, 0, Profiles.Count - 1);
            set => _defaultProfileIndex = Math.Clamp(value, 0, Profiles.Count - 1);
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

    }

}
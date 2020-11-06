using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SDKLauncher.Models
{
    public class AppConfig
    {

        private static readonly string CONFIG_NAME = "config.json";
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // ============================================

        public List<Profile> Profiles { get; set; }
        private int _defaultProfileIndex;
        public int DefaultProfileIndex { 
            get => Math.Clamp(_defaultProfileIndex, 0, Profiles.Count - 1);
            set => _defaultProfileIndex = Math.Clamp(value, 0, Profiles.Count - 1);
        }

        // ============================================

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(this, JsonSerializerOptions);
            File.WriteAllText(CONFIG_NAME, jsonString);
        }
        
        public static AppConfig LoadConfig()
        {
            string jsonString = File.ReadAllText(CONFIG_NAME);

            if (String.IsNullOrWhiteSpace(jsonString)) throw new InvalidDataException("Default config cannot be loaded / is empty");

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

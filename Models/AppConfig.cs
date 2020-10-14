using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SDKLauncher.Models
{
    public class AppConfig
    {

        private static readonly string CONFIG_NAME = "config.json";
        private static readonly JsonSerializerOptions JSON_SERIALIZER_OPTIONS = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        public List<Mod> Mods { get; set; }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(this, JSON_SERIALIZER_OPTIONS);
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
                Mods = new List<Mod> {
                    new Mod()
                    {
                        Name = "Portal 2: Community Edition",
                        AppId = 440000
                    }
                }
            };

        }

    }

}

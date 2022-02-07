using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ADLS_Explorer
{
    internal class Settings
    {
        public List<AZContainer> AZContainers { get; set; } = new List<AZContainer>();

        public string MostRecent { get; set; }

        private static string SettingsFile
        {
            get
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dir = Path.Combine(dir, "JD", "ADLS Explorer");
                Directory.CreateDirectory(dir);
                return Path.Combine(dir, "settings.json");
            }
        }

        public static Settings Load()
        {
            try { return JsonSerializer.Deserialize<Settings>(File.ReadAllText(SettingsFile)); }
            catch { return new Settings(); }
        }

        public void Save()
        {
            File.WriteAllText(SettingsFile, JsonSerializer.Serialize(this));
        }
    }
}

using System.IO;
using TiqUtils.Serialize;

namespace TiqUtils.SettingsController
{
    public class SettingsController<T> where T: class, new()
    {
        private T _settings;
        public string StorageFolder { get; set; } = "Config";

        public string ConfigFileName { get; set; } = "settings.config";

        private string AssemblyDirectory { get; }

        private string ConfigPath => Path.Combine(AssemblyDirectory, StorageFolder, ConfigFileName);

        public T Settings
        {
            get => _settings ?? (_settings = Initialize());
            private set => _settings = value;
        }

        public SettingsController(string assemblyDirectory)
        {
            AssemblyDirectory = assemblyDirectory;
        }

        public T Initialize()
        {
            return Json.DeserializeDataFromFile<T>(ConfigPath) ?? new T();
        }

        public void Save()
        {
            Settings.SerializeDataJson(ConfigPath);
        }

        public void Reset()
        {
            Settings = new T();
        }
    }
}

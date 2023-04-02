using System.IO;
using TiqUtils.Serialize;

namespace TiqUtils.SettingsController
{
    public class SettingsController<T> where T: class, new()
    {
        private readonly bool _objectsHandling;
        private T _settings;
        public string StorageFolder { get; set; } = "Config";

        public string ConfigFileName { get; set; } = "settings.config";

        private string AssemblyDirectory { get; }

        private string ConfigPath => Path.Combine(this.AssemblyDirectory, this.StorageFolder, this.ConfigFileName);

        public T Settings
        {
            get => this._settings ?? (this._settings = this.Initialize());
            private set => this._settings = value;
        }

        public SettingsController(string assemblyDirectory, bool objectsHandling = false)
        {
            this._objectsHandling = objectsHandling;
            this.AssemblyDirectory = assemblyDirectory;
        }

        public T Initialize()
        {
            return Json.DeserializeDataFromFile<T>(this.ConfigPath, this._objectsHandling) ?? new T();
        }

        public void Save()
        {
            this.Settings.SerializeDataJson(this.ConfigPath, this._objectsHandling);
        }

        public void Reset()
        {
            this.Settings = new T();
        }
    }
}

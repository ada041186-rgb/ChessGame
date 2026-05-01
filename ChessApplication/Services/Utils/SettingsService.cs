using ChessApplication.DTO;
using ChessApplication.Interfaces.Utils;
using System.Text.Json;

namespace ChessApplication.Services.Utils
{
    public class SettingsService : ISettingsService
    {
        private readonly string _filePath = "settings.json";

        public SettingsData Load()
        {
            if (!File.Exists(_filePath))
                return new SettingsData();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
            }
            catch
            {
                return new SettingsData();
            }
        }

        public void Save(SettingsData settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}

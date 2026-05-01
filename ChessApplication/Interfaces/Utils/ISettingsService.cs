using ChessApplication.DTO;

namespace ChessApplication.Interfaces.Utils
{
    public interface ISettingsService
    {
        SettingsData Load();
        void Save(SettingsData settings);
    }
}

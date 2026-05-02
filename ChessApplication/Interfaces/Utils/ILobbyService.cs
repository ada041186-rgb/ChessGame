using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessLibrary.Enums;

namespace ChessApplication.Interfaces.Utils
{
    public interface ILobbyService
    {
        event Action<bool> IsConnected;
        event Action<Player> GameStarted;
        Task<bool> InitializeAsync(LobbyParams lobbyParams);
        void HandleLocalStartGame(DtoStartGame message);
        Task StartLanGameAsync(Player player);
        void Reset();
        Task DisconnectAsync();
    }
}
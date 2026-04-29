using System;
using System.Threading.Tasks;
using ChessGame.Model;

namespace ChessGame.Services.Interfaces
{
    public interface ILobbyService
    {
        event Action<bool> IsConnected;
        Task InitializeAsync(bool isHost, string ip = null);
        Task StartGameAsync();
    }
}
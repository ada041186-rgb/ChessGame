using ChessGame.Model;

namespace ChessGame.Services.Interfaces
{
    public interface INetworkService
    {
        Task StartServerAsync(int port);
        Task ConnectAsync(string ip, int port);

        Task SendAsync(DtoType type, IDtoMessage message);
        Task DisconnectAsync();
    }
}
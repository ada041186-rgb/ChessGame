using ChessApplication.DTO;

namespace ChessApplication.Interfaces.Network
{
    public interface INetworkService
    {
        Task<bool> StartServerAsync(int port);
        Task<bool> ConnectAsync(string ip, int port);

        Task SendAsync(DtoType type, IDtoMessage message);
        Task DisconnectAsync();
    }
}
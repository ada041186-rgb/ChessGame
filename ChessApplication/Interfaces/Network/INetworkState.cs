using ChessApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.Interfaces.Network
{
    public interface INetworkState
    {
        Task<bool> StartServerAsync(int port);
        Task<bool> ConnectAsync(string ip, int port);
        Task SendAsync(DtoType type, IDtoMessage message);
        Task DisconnectAsync();
    }
}

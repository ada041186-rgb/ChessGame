using ChessApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.Interfaces.Network
{
    public interface INetworkContext
    {
        Task<bool> StartServerInternal(int port);
        Task<bool> ConnectInternal(string ip, int port);
        Task SendInternal(DtoType type, IDtoMessage message);
        Task DisconnectInternal();

        void SetState(INetworkState state);
    }
}

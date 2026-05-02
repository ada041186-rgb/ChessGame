using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.NetworkStates.States
{
    public class ConnectedState : INetworkState
    {
        private readonly INetworkContext _ctx;

        public ConnectedState(INetworkContext ctx)
        {
            _ctx = ctx;
        }

        public Task<bool> StartServerAsync(int port)
            => Task.FromResult(false);

        public Task<bool> ConnectAsync(string ip, int port)
            => Task.FromResult(false);

        public async Task SendAsync(DtoType type, IDtoMessage message)
        {
            await _ctx.SendInternal(type, message);
        }

        public async Task DisconnectAsync()
        {
            await _ctx.DisconnectInternal();
        }
    }
}

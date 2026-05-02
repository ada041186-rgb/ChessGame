using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.NetworkStates.States
{
    public class ConnectingState : INetworkState
    {
        private readonly INetworkContext _ctx;

        public ConnectingState(INetworkContext ctx)
        {
            _ctx = ctx;
        }

        public Task<bool> StartServerAsync(int port)
            => Task.FromResult(false);

        public Task<bool> ConnectAsync(string ip, int port)
            => Task.FromResult(false);

        public Task SendAsync(DtoType type, IDtoMessage message)
        {
            throw new Exception("Send called while connecting");
        }

        public async Task DisconnectAsync()
        {
            await _ctx.DisconnectInternal();
            _ctx.SetState(new DisconnectedState(_ctx));
        }
    }
}

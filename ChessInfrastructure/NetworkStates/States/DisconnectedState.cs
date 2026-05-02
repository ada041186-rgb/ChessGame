using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using ChessInfrastructure.Network;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.NetworkStates.States
{
    public class DisconnectedState : INetworkState
    {
        private readonly INetworkContext _ctx;

        public DisconnectedState(INetworkContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> StartServerAsync(int port)
        {
            _ctx.SetState(new ConnectingState(_ctx));

            var result = await _ctx.StartServerInternal(port);

            _ctx.SetState(result
                ? new ConnectedState(_ctx)
                : new DisconnectedState(_ctx));

            return result;
        }

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            _ctx.SetState(new ConnectingState(_ctx));

            var result = await _ctx.ConnectInternal(ip, port);

            _ctx.SetState(result
                ? new ConnectedState(_ctx)
                : new DisconnectedState(_ctx));

            return result;
        }

        public Task SendAsync(DtoType type, IDtoMessage message)
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }
    }
}

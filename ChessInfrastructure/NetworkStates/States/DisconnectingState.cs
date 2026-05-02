using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.NetworkStates.States
{
    public class DisconnectingState : INetworkState
    {
        private readonly INetworkContext _ctx;

        public DisconnectingState(INetworkContext ctx)
        {
            _ctx = ctx;
        }

        public Task<bool> StartServerAsync(int port) => Task.FromResult(false);
        public Task<bool> ConnectAsync(string ip, int port) => Task.FromResult(false);
        public Task SendAsync(DtoType type, IDtoMessage message) => Task.CompletedTask;

        public Task DisconnectAsync() => Task.CompletedTask; 
    }
}

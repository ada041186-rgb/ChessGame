using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.DTO.Handlers
{
    public class DtoCastlingMoveHandlerKS : IMessageHandler<DtoCastlingMoveKS>
    {
        private readonly IGameService _gameService;
        private readonly IDtoMoveFactory _moveFactory;

        public DtoCastlingMoveHandlerKS(IDtoMoveFactory moveFactory, IGameService gameService)
        {
            _moveFactory = moveFactory;
            _gameService = gameService;
        }

        public Task HandleAsync(DtoCastlingMoveKS message)
        {
            var move = _moveFactory.GetMoveFromDTO(message);
            _gameService.TryMakeMove(move);
            return Task.CompletedTask;
        }
    }
}

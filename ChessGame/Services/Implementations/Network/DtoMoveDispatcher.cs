using ChessGame.Model;
using ChessGame.Model.DTO;
using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame.Services.Implementations
{
    public class DtoMoveDispatcher : IDtoMoveFactory
    {
        private readonly IEnumerable<ISpecificDtoMoveFactory> _factories;

        public DtoMoveDispatcher(IEnumerable<ISpecificDtoMoveFactory> factories)
        {
            _factories = factories;
        }

        public Move GetMoveFromDTO(IDtoMessage dtoMove)
        {
            var factory = _factories.FirstOrDefault(f => f.TargetDtoType == dtoMove.MessageType);

            return factory.GetMoveFromDTO(dtoMove);
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            var factory = _factories.FirstOrDefault(f => f.TargetMoveType == move.Type);

            return factory.GetMoveToDTO(move);
        }
    }
}
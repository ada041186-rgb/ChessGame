using ChessGame.Services.Interfaces.Utils;
using ChessGame.Services.Interfaces.Utils.PieceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Utils.PieceFactories
{
    public class PieceFactory : IPieceFactory
    {
        private readonly IEnumerable<ISubPieceFactory> _subFactories;

        public PieceFactory(IEnumerable<ISubPieceFactory> subFactories)
        {
            _subFactories = subFactories;
        }

        public Piece CreatePiece(PieceType type, Player color)
        {
            var factory = _subFactories.First(f => f.Type == type);


            return factory.Create(color);
        }
    }
}

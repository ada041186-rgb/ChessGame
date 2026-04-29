using ChessGame.Model.Abstractions;
using ChessGame.Services.Interfaces.Utils.PieceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Utils.PieceFactories
{
    public class PawnFactory : ISubPieceFactory
    {
        private readonly IMoveFactory _moveFactory;
        private readonly IEnumerable<IPromotionStrategy> _promotionStrategies;

        public PawnFactory(IMoveFactory moveFactory, IEnumerable<IPromotionStrategy> promotionStrategies) 
        {
            _moveFactory = moveFactory;
            _promotionStrategies = promotionStrategies;
        }
        public PieceType Type => PieceType.Pawn;

        public Piece Create(Player color)
        {
            return new Pawn(color, _moveFactory, _promotionStrategies);
        }
    }
}

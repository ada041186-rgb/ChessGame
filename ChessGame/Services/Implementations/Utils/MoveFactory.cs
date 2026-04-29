using ChessGame.Model;
using ChessGame.Model.Abstractions;
using ChessGame.Model.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Utils
{
    public class MoveFactory : IMoveFactory
    {
        public Move CreateNormalMove(Position from, Position to)
            => new NormalMove(from, to);

        public IEnumerable<Move> CreatePromotionMoves(Position from, Position to, IEnumerable<IPromotionStrategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                yield return new PawnPromotion(from, to, strategy);
            }
        }
    }
}

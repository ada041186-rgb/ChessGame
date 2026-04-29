using ChessGame.Model;
using ChessGame.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services
{
    public interface IMoveFactory
    {
        Move CreateNormalMove(Position from, Position to);
        IEnumerable<Move> CreatePromotionMoves(Position from, Position to, IEnumerable<IPromotionStrategy> strategies);
    }
}

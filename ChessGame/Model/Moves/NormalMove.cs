using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Moves
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.NormalMove;
        public override Position FromPos { get; }
        public override Position ToPos { get; }
        public NormalMove(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
        }
        public override void Execute(IBoard board)
        {
            var pieceMoved = board[FromPos];
            var pieceCaptured = board[ToPos];

            pieceMoved.HasMoved = true;

            board[ToPos] = pieceMoved;
            board[FromPos] = null;
        }
    }
}

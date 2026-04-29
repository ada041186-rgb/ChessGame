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

        private Piece _pieceMoved;
        private Piece _pieceCaptured;
        public NormalMove(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
        }
        public override void Execute(Board board)
        {
            _pieceMoved = board[FromPos];
            _pieceCaptured = board[ToPos];

            board[ToPos] = _pieceMoved;
            board[FromPos] = null;
        }

        public override void Undo(Board board)
        {
            board[FromPos] = _pieceMoved;
            board[ToPos] = _pieceCaptured;
        }
    }
}

using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.KingMoves
{
    public class CastleMoveQS : Move
    {
        public override MoveType Type => MoveType.CastleQS;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public CastleMoveQS(Position kingPos)
        {
            FromPos = kingPos;
            kingMoveDir = Direction.West;
            ToPos = new Position(kingPos.Row, 2);
            rookFromPos = new Position(kingPos.Row, 0);
            rookToPos = new Position(kingPos.Row, 3);

        }

        public override void Execute(IBoard board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);
        }
    }
}

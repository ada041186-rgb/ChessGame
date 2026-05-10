using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.KingMoves
{
    public class CastleMoveKS : Move
    {
        public override MoveType Type => MoveType.CastleKS;

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public CastleMoveKS(Position kingPos)
        {
            FromPos = kingPos;
            kingMoveDir = Direction.East;
            ToPos = new Position(kingPos.Row, 6);
            rookFromPos = new Position(kingPos.Row, 7);
            rookToPos = new Position(kingPos.Row, 5);

        }

        public override void Execute(IBoard board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);
        }
    }
}

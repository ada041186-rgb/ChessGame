using ChessGame.Model;
using ChessGame.Model.Abstractions;
using ChessGame.Model.Moves;
using ChessGame.Model.PromotionStrategies;
using ChessGame.Services;

namespace ChessGame
{
    public class Pawn : Piece
    {
        private readonly IMoveFactory _moveFactory;

        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private readonly Direction _forward;
        private static readonly Direction[] _diagonals = { Direction.West, Direction.East };
        private readonly IEnumerable<IPromotionStrategy> _promotionStrategies;

        public Pawn(Player player, IMoveFactory moveFactory, IEnumerable<IPromotionStrategy> promotionStrategies)
        {
            Color = player;
            _moveFactory = moveFactory;


            if (player == Player.White)
            {
                _forward = Direction.North;
            }
            else
            {
                _forward = Direction.South;
            }

            _promotionStrategies = promotionStrategies;
        }

        public override Piece Copy()
        {
            return new Pawn(Color, _moveFactory, _promotionStrategies)
            {
                HasMoved = this.HasMoved
            };
        }

        public override IEnumerable<Move> GetMoves(Position from, IBoard board)
        {
            return GetForwardMoves(from, board).Concat(GetDiagonalMoves(from, board));
        }

        private IEnumerable<Move> GetForwardMoves(Position from, IBoard board)
        {
            Position oneStep = from + _forward;
            if (!CanMoveForward(oneStep, board))
                yield break;

            foreach (var move in CreateMoves(from, oneStep))
            {
                yield return move;
            }

            Position twoStep = oneStep + _forward;
            if (!HasMoved && CanMoveForward(twoStep, board))
            {
                yield return _moveFactory.CreateNormalMove(from, twoStep);
            }
        }

        private IEnumerable<Move> GetDiagonalMoves(Position from, IBoard board)
        {
            foreach (var dir in _diagonals)
            {
                Position target = from + _forward + dir;

                if (CanCaptureDiagonal(target, board))
                {
                    foreach (var move in CreateMoves(from, target))
                        yield return move;
                }
            }
        }

        private IEnumerable<Move> CreateMoves(Position from, Position to)
        {
            if (IsPromotionRow(to))
            {
                return _moveFactory.CreatePromotionMoves(from, to, _promotionStrategies);
            }

            return new[] { _moveFactory.CreateNormalMove(from, to) };
        }

        private bool CanMoveForward(Position pos, IBoard board)
            => board.IsInside(pos) && board.IsEmpty(pos);

        private bool CanCaptureDiagonal(Position pos, IBoard board)
        {
            if (!board.IsInside(pos)) return false;
            var piece = board[pos];
            return piece != null && piece.Color != Color;
        }

        private bool IsPromotionRow(Position pos)
            => pos.Row == 0 || pos.Row == 7;

        public override bool CanCaptureOpponentKing(Position from, IBoard board)
        {
            return _diagonals.Any(dir =>
            {
                Position target = from + _forward + dir;
                if (!board.IsInside(target)) return false;
                var piece = board[target];
                return piece != null && piece.Color != Color && piece.Type == PieceType.King;
            });
        }
    }
}
using ChessGame.Model;
using ChessGame.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations
{
    public class BoardFactory : IBoardFactory
    {
        private readonly IEnumerable<IPromotionStrategy> _promotionStrategies;
        private readonly IMoveFactory _moveFactory;

        public BoardFactory(IEnumerable<IPromotionStrategy> promotionStrategies, IMoveFactory moveFactory)
        {
            _promotionStrategies = promotionStrategies;
            _moveFactory = moveFactory;
        }

        public IBoard CreateInitial()
        {
            IBoard board = new Board();

            PlaceBackRank(board, 0, Player.Black);
            PlaceBackRank(board, 7, Player.White);

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(Player.Black, _moveFactory, _promotionStrategies);
                board[6, i] = new Pawn(Player.White, _moveFactory, _promotionStrategies);
            }

            return board;
        }

        private void PlaceBackRank(IBoard board, int row, Player player)
        {
            board[row, 0] = new Rook(player);
            board[row, 1] = new Knight(player);
            board[row, 2] = new Bishop(player);
            board[row, 3] = new Queen(player);
            board[row, 4] = new King(player);
            board[row, 5] = new Bishop(player);
            board[row, 6] = new Knight(player);
            board[row, 7] = new Rook(player);
        }
    }
}

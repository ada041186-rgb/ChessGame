using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations
{
    public class BoardFactory : IBoardFactory
    {
        public Board CreateInitial()
        {
            var board = new Board();

            PlaceBackRank(board, 0, Player.Black);
            PlaceBackRank(board, 7, Player.White);

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(Player.Black);
                board[6, i] = new Pawn(Player.White);
            }

            return board;
        }

        private void PlaceBackRank(Board board, int row, Player player)
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

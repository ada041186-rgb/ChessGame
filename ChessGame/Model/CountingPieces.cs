using ChessGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public class CountingPieces : ICountringPieces
    {
        private readonly Dictionary<PieceType, int> whiteCount = new Dictionary<PieceType, int>();
        private readonly Dictionary<PieceType, int> blackCount = new Dictionary<PieceType, int>();

        public int TotalCount { get; private set; }

        public CountingPieces()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }
        public void Increment(Player color, PieceType type)
        {
            if (color == Player.White)
            {
                whiteCount[type]++;
            }
            else if (color == Player.Black) 
            { 
                blackCount[type]++; 
            }
        }
        public int GetBlackPieces(PieceType type)
        {
            return blackCount[type];
        }

        public int GetWhitePieces(PieceType type)
        {
            return whiteCount[type];
        }

    }
}

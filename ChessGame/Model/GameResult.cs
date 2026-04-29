using ChessGame.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public record GameResult
    {
        public Player Winner { get; init; }
        public EndGameTypes Type { get; init; }

        public GameResult(Player winner, EndGameTypes type)
        {
            Winner = winner;
            Type = type;
        }
    }
}

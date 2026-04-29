using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessGame.Model.Enums
{
    public enum EndGameTypes
    {
        Checkmate,
        Stalemate,
        ThreefoldRepetition,
        FiftyMoveRule,
        InsufficientMaterial
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGame.Model
{
    public static class Images
    {
        private static Dictionary<PieceType, ImageSource> WhiteImageSource = new Dictionary<PieceType, ImageSource>
        {
            [PieceType.Pawn] = LoadImage("/Assets/PawnW.png"),
            [PieceType.Bishop] = LoadImage("/Assets/BishopW.png"),
            [PieceType.Knight] = LoadImage("/Assets/KnightW.png"),
            [PieceType.Rook] = LoadImage("/Assets/RookW.png"),
            [PieceType.Queen] = LoadImage("/Assets/QueenW.png"),
            [PieceType.King] = LoadImage("/Assets/KingW.png")
        };

        private static Dictionary<PieceType, ImageSource> BlackImageSource = new Dictionary<PieceType, ImageSource>
        {
            [PieceType.Pawn] = LoadImage("/Assets/PawnB.png"),
            [PieceType.Bishop] = LoadImage("/Assets/BishopB.png"),
            [PieceType.Knight] = LoadImage("/Assets/KnightB.png"),
            [PieceType.Rook] = LoadImage("/Assets/RookB.png"),
            [PieceType.Queen] = LoadImage("/Assets/QueenB.png"),
            [PieceType.King] = LoadImage("/Assets/KingB.png")
        };

        public static ImageSource GetImage(Player color, PieceType pieceType)
        {
            return color switch
            {
                Player.White => WhiteImageSource[pieceType],
                Player.Black => BlackImageSource[pieceType],
                _ => null
            };
        }
        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }


            return piece.Color switch
            {
                Player.White => WhiteImageSource[piece.Type],
                Player.Black => BlackImageSource[piece.Type],
                _ => null
            };
        }
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.DTO.Messages
{
    [DtoType(DtoType.PromotionMove)]
    public class DtoPromotionMove : IDtoMessage
    {
        public DtoType MessageType => DtoType.PromotionMove;
        public Position FromPos { get; set; }
        public Position ToPos { get; set; }
        public PieceType PromotionPieceType { get; set; }

        public DtoPromotionMove() { }
        public DtoPromotionMove(Position from, Position to, PieceType type)
        {
            FromPos = from;
            ToPos = to;
            PromotionPieceType = type;
        }
    }
}

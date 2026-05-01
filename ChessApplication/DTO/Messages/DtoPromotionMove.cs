using ChessLibrary.Enums;
using ChessLibrary.ValueObjects;

namespace ChessApplication.DTO.Messages
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

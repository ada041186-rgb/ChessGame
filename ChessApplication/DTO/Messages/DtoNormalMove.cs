using ChessLibrary.ValueObjects;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.NormalMove)]
    public class DtoNormalMove : IDtoMessage
    {
        public DtoType MessageType => DtoType.NormalMove;
        public Position FromPos { get; set; }
        public Position ToPos { get; set; }

        public DtoNormalMove() { }
        public DtoNormalMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
    }
}
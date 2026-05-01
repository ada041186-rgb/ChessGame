using ChessLibrary.ValueObjects;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.DoubleMove)]
    public class DtoDoubleMove : IDtoMessage
    {
        public DtoType MessageType => DtoType.DoubleMove;
        public Position FromPos { get; set; }
        public Position ToPos { get; set; }

        public DtoDoubleMove() { }
        public DtoDoubleMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
    }
}

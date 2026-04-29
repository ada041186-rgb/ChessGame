using ChessGame.Model.DTO;
using ChessGame.Model.DTO.Messages;
using System;
using System.Text.Json.Serialization;

namespace ChessGame.Model.Moves
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
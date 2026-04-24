using ChessGame.Model.DTO;
using ChessGame.Model.DTO.Messages;
using System;
using System.Text.Json.Serialization;

namespace ChessGame.Model.Moves
{
    [DtoType(DtoType.Move)]
    public class DtoMove : IDtoMessage
    {
        public MoveType Type { get; set; }
        public int FromPosRow { get; set; }
        public int FromPosColumn { get; set; }
        public int ToPosRow { get; set; }
        public int ToPosColumn { get; set; }
        public DtoMove() { }
        public DtoMove(Move move)
        {
            Type = move.Type;
            FromPosRow = move.FromPos.Row;
            FromPosColumn = move.FromPos.Column;
            ToPosRow = move.ToPos.Row;
            ToPosColumn = move.ToPos.Column;
        }
    }
}
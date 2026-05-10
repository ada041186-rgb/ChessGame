using ChessLibrary.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.CastlingMoveQS)]

    public class DtoCastlingMoveQS : IDtoMessage
    {
        public DtoType MessageType => DtoType.CastlingMoveQS;
        public Position FromPos { get; set; }
        public Position ToPos { get; set; }

        public DtoCastlingMoveQS() { }
        public DtoCastlingMoveQS(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
    }
}

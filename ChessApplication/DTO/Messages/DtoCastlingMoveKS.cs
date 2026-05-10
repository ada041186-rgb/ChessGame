using ChessLibrary.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.CastlingMoveKS)]

    public class DtoCastlingMoveKS : IDtoMessage
    {
        public DtoType MessageType => DtoType.CastlingMoveKS;
        public Position FromPos { get; set; }
        public Position ToPos { get; set; }

        public DtoCastlingMoveKS() { }
        public DtoCastlingMoveKS(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
    }
}

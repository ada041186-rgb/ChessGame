using ChessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.Resign)]
    public class DtoResign : IDtoMessage
    {
        public DtoType MessageType => DtoType.Resign;

        public DtoResign() { }
    }
}

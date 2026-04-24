using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.DTO
{
    public class NetworkMessage
    {
        public DtoType DtoType { get; set; }
        public string Payload { get; set; }
    }
}

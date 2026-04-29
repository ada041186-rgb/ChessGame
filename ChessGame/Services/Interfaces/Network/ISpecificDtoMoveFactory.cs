using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services
{
    public interface ISpecificDtoMoveFactory
    {
        DtoType TargetDtoType { get; }
        MoveType TargetMoveType { get; }

        Move GetMoveFromDTO(IDtoMessage dtoMove);
        IDtoMessage GetMoveToDTO(Move move);
    }
}

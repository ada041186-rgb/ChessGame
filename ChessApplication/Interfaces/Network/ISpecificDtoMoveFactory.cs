using ChessApplication.DTO;
using ChessLibrary.Enums;
using ChessLibrary.Moves;

namespace ChessApplication.Interfaces.Network
{
    public interface ISpecificDtoMoveFactory
    {
        DtoType TargetDtoType { get; }
        MoveType TargetMoveType { get; }

        Move GetMoveFromDTO(IDtoMessage dtoMove);
        IDtoMessage GetMoveToDTO(Move move);
    }
}

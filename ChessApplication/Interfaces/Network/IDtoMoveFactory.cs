using ChessApplication.DTO;
using ChessLibrary.Moves;

namespace ChessApplication.Interfaces.Network
{
    public interface IDtoMoveFactory
    {
        Move GetMoveFromDTO(IDtoMessage dtoMove);
        IDtoMessage GetMoveToDTO(Move move);
    }
}

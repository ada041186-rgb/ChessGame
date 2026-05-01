using ChessApplication.DTO;

namespace ChessApplication.Interfaces.Network
{
    public interface IDtoResolver
    {
        IDtoMessage Deserialize(NetworkMessage message);
    }
}

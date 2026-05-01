using ChessApplication.DTO;

namespace ChessApplication.Interfaces.Network
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync(IDtoMessage message);
    }
}

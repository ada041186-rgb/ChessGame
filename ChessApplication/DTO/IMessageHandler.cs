namespace ChessApplication.DTO
{
    public interface IMessageHandler<T> where T : IDtoMessage
    {
        Task HandleAsync(T message);
    }
}

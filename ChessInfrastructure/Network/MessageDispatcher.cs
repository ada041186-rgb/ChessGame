using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;

namespace ChessInfrastructure.Network
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceProvider _provider;

        public MessageDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task DispatchAsync(IDtoMessage message)
        {
            var messageType = message.GetType();

            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);

            var handler = _provider.GetService(handlerType);

            if (handler == null)
                throw new Exception($"No handler for {messageType.Name}");

            var method = handlerType.GetMethod("HandleAsync");

            await (Task)method.Invoke(handler, new object[] { message })!;
        }
    }
}

using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using static System.Net.Mime.MediaTypeNames;

namespace ChessInfrastructure.Network
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceProvider _provider;
        private readonly SynchronizationContext _uiContext;

        public MessageDispatcher(IServiceProvider provider, SynchronizationContext uiContext)
        {
            _provider = provider;
            _uiContext = uiContext;
        }

        public Task DispatchAsync(IDtoMessage message)
        {
            var messageType = message.GetType();
            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
            var handler = _provider.GetService(handlerType);

            if (handler == null)
                throw new Exception($"No handler for {messageType.Name}");

            var method = handlerType.GetMethod("HandleAsync");

            var tcs = new TaskCompletionSource();

            _uiContext.Post(async _ =>
            {
                try
                {
                    await (Task)method.Invoke(handler, new object[] { message })!;
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }
    }
}

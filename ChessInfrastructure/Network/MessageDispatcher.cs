using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;

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
            if (method == null)
                throw new Exception($"HandleAsync not found for {messageType.Name}");

            var tcs = new TaskCompletionSource<bool>();

            _uiContext.Post(async _ =>
            {
                try
                {
                    var task = (Task)method.Invoke(handler, new object[] { message })!;
                    await task.ConfigureAwait(true);

                    tcs.SetResult(true);
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
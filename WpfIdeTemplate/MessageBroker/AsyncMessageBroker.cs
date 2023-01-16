using SampleCompany.SampleProduct.CommonLibrary.MessageBroker;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.MessageBroker
{
    public class AsyncMessageBroker<TMessage> : IAsnycMessageBroker<TMessage>
    {
        private readonly AsyncMessageBrokerCore<TMessage> _core;

        public AsyncMessageBroker(AsyncMessageBrokerCore<TMessage> core)
        {
            _core = core;
        }

        public void Publish(TMessage message, CancellationToken cancellationToken = default)
        {
            ((IAsyncPublisher<TMessage>)_core).Publish(message, cancellationToken);
        }

        public ValueTask PublishAsync(TMessage message, CancellationToken cancellationToken = default)
        {
            return ((IAsyncPublisher<TMessage>)_core).PublishAsync(message, cancellationToken);
        }

        public IDisposable Subscribe(IAsyncMessageHandler<TMessage> asyncHandler)
        {
            return ((IAsyncSubscriber<TMessage>)_core).Subscribe(asyncHandler);
        }
    }
    public class AsyncMessageBrokerCore<TMessage> : IAsnycMessageBroker<TMessage>
    {
        private readonly ConcurrentDictionary<Guid, IAsyncMessageHandler<TMessage>> _handlers;
        public AsyncMessageBrokerCore()
        {
            _handlers = new ConcurrentDictionary<Guid, IAsyncMessageHandler<TMessage>>();
        }
        public void Publish(TMessage message, CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers.Values)
            {
                handler.HandleAsync(message, cancellationToken).Forget();
            }
        }

        public async ValueTask PublishAsync(TMessage message, CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers.Values)
            {
                await handler.HandleAsync(message, cancellationToken);
            }
        }

        public IDisposable Subscribe(IAsyncMessageHandler<TMessage> asyncHandler)
        {
            var key = Guid.NewGuid();
            _handlers.TryAdd(key, asyncHandler);
            return new Subscription(() => _handlers.TryRemove(key, out _));
        }
    }
    public static class TaskExtensions
    {
        public static async void Forget(this ValueTask task)
        {
            await task;
        }
    }
}

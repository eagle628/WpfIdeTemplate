using SampleCompany.SampleProduct.CommonLibrary.MessageBroker;
using System;
using System.Collections.Concurrent;

namespace SampleCompany.SampleProduct.MessageBroker
{
    public class MessageBroker<TMessage> : IMessageBroker<TMessage>
    {
        private readonly MessageBrokerCore<TMessage> _core;

        public MessageBroker(MessageBrokerCore<TMessage> core)
        {
            _core = core;
        }

        public void Publish(TMessage message)
        {
            ((IPublisher<TMessage>)_core).Publish(message);
        }

        public IDisposable Subscribe(IMessageHandler<TMessage> handler)
        {
            return ((ISubscriber<TMessage>)_core).Subscribe(handler);
        }
    }
    public class MessageBrokerCore<TMessage> : IMessageBroker<TMessage>
    {
        private readonly ConcurrentDictionary<Guid, IMessageHandler<TMessage>> _handlers;
        public MessageBrokerCore()
        {
            _handlers = new ConcurrentDictionary<Guid, IMessageHandler<TMessage>>();
        }
        public void Publish(TMessage message)
        {
            foreach (var handler in _handlers.Values)
            {
                handler.Handle(message);
            }
        }

        public IDisposable Subscribe(IMessageHandler<TMessage> handler)
        {
            var key = Guid.NewGuid();
            _handlers.TryAdd(key, handler);
            return new Subscription(() => _handlers.TryRemove(key, out _));
        }
    }
}

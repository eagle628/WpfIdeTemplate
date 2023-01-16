using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.CommonLibrary.MessageBroker
{
    public static partial class SubscriberExtensions
    {
        public static IDisposable Subscribe<TMessage>(this ISubscriber<TMessage> subscriber, Action<TMessage> handler)
        {
            return subscriber.Subscribe(new AnonymousMessageHandler<TMessage>(handler));
        }
        public static IDisposable Subscribe<TMessage>(this IAsyncSubscriber<TMessage> subscriber, Func<TMessage, CancellationToken, ValueTask> handler)
        {
            return subscriber.Subscribe(new AnonymousAsyncMessageHandler<TMessage>(handler));
        }

        private sealed class AnonymousMessageHandler<TMessage> : IMessageHandler<TMessage>
        {
            readonly Action<TMessage> handler;

            public AnonymousMessageHandler(Action<TMessage> handler)
            {
                this.handler = handler;
            }

            public void Handle(TMessage message)
            {
                handler.Invoke(message);
            }
        }

        private sealed class AnonymousAsyncMessageHandler<TMessage> : IAsyncMessageHandler<TMessage>
        {
            readonly Func<TMessage, CancellationToken, ValueTask> handler;

            public AnonymousAsyncMessageHandler(Func<TMessage, CancellationToken, ValueTask> handler)
            {
                this.handler = handler;
            }

            public ValueTask HandleAsync(TMessage message, CancellationToken cancellationToken)
            {
                return handler.Invoke(message, cancellationToken);
            }
        }
    }
}

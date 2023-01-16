using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Disposables;

namespace SampleCompany.SampleProduct.CommonLibrary.MessageBroker
{
    public static partial class SubscriberExtensions
    {
        public static IAsyncEnumerable<TMessage> AsAsyncEnumerable<TMessage>(this IAsyncSubscriber<TMessage> subscriber)
        {
            return new AsyncEnumerableAsyncSubscriber<TMessage>(subscriber);
        }
        private class AsyncEnumerableAsyncSubscriber<TMessage> : IAsyncEnumerable<TMessage>
        {
            readonly IAsyncSubscriber<TMessage> subscriber;

            public AsyncEnumerableAsyncSubscriber(IAsyncSubscriber<TMessage> subscriber)
            {
                this.subscriber = subscriber;
            }

            public IAsyncEnumerator<TMessage> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                var disposable = new SingleAssignmentDisposable();
                var e = new AsyncMessageHandlerEnumerator<TMessage>(disposable, cancellationToken);
                disposable.Disposable = subscriber.Subscribe(e);
                return e;
            }
        }

        private class AsyncMessageHandlerEnumerator<TMessage> : IAsyncEnumerator<TMessage>, IAsyncMessageHandler<TMessage>
        {
            Channel<TMessage> channel;
            CancellationToken cancellationToken;
            SingleAssignmentDisposable singleAssignmentDisposable;

            public AsyncMessageHandlerEnumerator(SingleAssignmentDisposable singleAssignmentDisposable, CancellationToken cancellationToken)
            {
                this.singleAssignmentDisposable = singleAssignmentDisposable;
                this.cancellationToken = cancellationToken;

                this.channel = Channel.CreateUnbounded<TMessage>(new UnboundedChannelOptions()
                {
                    SingleWriter = true,
                    SingleReader = true,
                    AllowSynchronousContinuations = true
                });
            }

            TMessage IAsyncEnumerator<TMessage>.Current
            {
                get
                {
                    if (channel.Reader.TryRead(out var msg))
                    {
                        return msg;
                    }
                    throw new InvalidOperationException("Message is not buffered in Channel.");
                }
            }

            ValueTask<bool> IAsyncEnumerator<TMessage>.MoveNextAsync()
            {
                return channel.Reader.WaitToReadAsync(cancellationToken);
            }

            ValueTask IAsyncMessageHandler<TMessage>.HandleAsync(TMessage message, CancellationToken cancellationToken)
            {
                channel.Writer.TryWrite(message);
                return default;
            }

            ValueTask IAsyncDisposable.DisposeAsync()
            {
                singleAssignmentDisposable.Dispose(); // unsubscribe message.
                return default;
            }
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.CommonLibrary.MessageBroker
{
    public interface IMessageBroker<TMessage> : IPublisher<TMessage>, ISubscriber<TMessage>
    {

    }
    public interface IAsnycMessageBroker<TMessage> : IAsyncPublisher<TMessage>, IAsyncSubscriber<TMessage>
    {

    }
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }
    public interface IAsyncMessageHandler<TMessage>
    {
        ValueTask HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
    public interface IPublisher<TMessage>
    {
        void Publish(TMessage message);
    }

    public interface ISubscriber<TMessage>
    {
        IDisposable Subscribe(IMessageHandler<TMessage> handler);
    }

    public interface IAsyncPublisher<TMessage>
    {
        void Publish(TMessage message, CancellationToken cancellationToken = default);
        ValueTask PublishAsync(TMessage message, CancellationToken cancellationToken = default);
    }

    public interface IAsyncSubscriber<TMessage>
    {
        IDisposable Subscribe(IAsyncMessageHandler<TMessage> asyncHandler);
    }
}

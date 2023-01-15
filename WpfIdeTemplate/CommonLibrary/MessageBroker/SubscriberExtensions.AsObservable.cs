using System;

namespace SampleCompany.SampleProduct.CommonLibrary
{
    public static partial class SubscriberExtensions
    {
        public static IObservable<TMessage> AsObservable<TMessage>(this ISubscriber<TMessage> subscriber)
        {
            return new ObservableSubscriber<TMessage>(subscriber);
        }
        private sealed class ObservableSubscriber<TMessage> : IObservable<TMessage>
        {
            readonly ISubscriber<TMessage> subscriber;

            public ObservableSubscriber(ISubscriber<TMessage> subscriber)
            {
                this.subscriber = subscriber;
            }

            public IDisposable Subscribe(IObserver<TMessage> observer)
            {
                return subscriber.Subscribe(new ObserverMessageHandler<TMessage>(observer));
            }
        }
        private sealed class ObserverMessageHandler<TMessage> : IMessageHandler<TMessage>
        {
            readonly IObserver<TMessage> observer;

            public ObserverMessageHandler(IObserver<TMessage> observer)
            {
                this.observer = observer;
            }

            public void Handle(TMessage message)
            {
                observer.OnNext(message);
            }
        }
    }
}

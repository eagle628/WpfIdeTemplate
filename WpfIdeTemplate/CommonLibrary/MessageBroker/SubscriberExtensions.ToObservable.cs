using System;
using System.Linq;

namespace SampleCompany.SampleProduct.CommonLibrary.MessageBroker
{
    public static partial class SubscriberExtensions
    {
        public static IObservable<TMessage> ToObservable<TMessage>(this IAsyncSubscriber<TMessage> subscriber)
        {
            return subscriber.AsAsyncEnumerable().ToObservable();
        }
    }
}

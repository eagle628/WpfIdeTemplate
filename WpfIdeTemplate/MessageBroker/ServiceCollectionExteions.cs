using Microsoft.Extensions.DependencyInjection;
using SampleCompany.SampleProduct.CommonLibrary;

namespace SampleCompany.SampleProduct.MessageBroker
{
    public static class ServiceCollectionExteions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services)
        {
            services.AddSingleton(typeof(MessageBrokerCore<>))
                    .AddSingleton(typeof(ISubscriber<>), typeof(MessageBroker<>))
                    .AddSingleton(typeof(IPublisher<>), typeof(MessageBroker<>))
                    .AddSingleton(typeof(AsyncMessageBrokerCore<>))
                    .AddSingleton(typeof(IAsyncSubscriber<>), typeof(AsyncMessageBroker<>))
                    .AddSingleton(typeof(IAsyncPublisher<>), typeof(AsyncMessageBroker<>));
            return services;
        }
    }
}

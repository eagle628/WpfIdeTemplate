using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using System;

namespace SampleCompany.SampleProduct.InMemoryLogger
{
    public static class InMemoryLoggerExtensions
    {
        public static ILoggingBuilder AddInMemoryLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, InMemoryLoggerProvider>(x =>
            {
                var store = x.GetRequiredService<IInMemoryLogStore>();
                var config = x.GetRequiredService<IOptionsMonitor<InMemoryLoggerConfiguration>>();
                return new InMemoryLoggerProvider(store, config);
            }));
            LoggerProviderOptions.RegisterProviderOptions<InMemoryLoggerConfiguration, InMemoryLoggerProvider>(builder.Services);
            return builder;
        }
        public static ILoggingBuilder AddInMemoryLogger(
            this ILoggingBuilder builder,
            Action<InMemoryLoggerConfiguration> configure)
        {
            builder.AddInMemoryLogger();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}

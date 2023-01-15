using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using System;
using System.Collections.Concurrent;

namespace SampleCompany.SampleProduct.InMemoryLogger
{
    public sealed class InMemoryLoggerProvider : ILoggerProvider
    {
        private readonly InMemoryLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);
        private readonly IInMemoryLogStore _store;

        public InMemoryLoggerProvider(IInMemoryLogStore store, IOptionsMonitor<InMemoryLoggerConfiguration> config)
        {
            _store = store;
            _currentConfig = config.CurrentValue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new InMemoryLogger(name, _store, GetCurrentConfig));
        }
        private InMemoryLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {

        }
    }
}

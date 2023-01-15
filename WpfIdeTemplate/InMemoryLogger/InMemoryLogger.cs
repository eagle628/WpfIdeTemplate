using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.CommonLibrary;
using System;

namespace SampleCompany.SampleProduct.InMemoryLogger
{
    public class InMemoryLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<InMemoryLoggerConfiguration> _getCurrentConfig;
        private readonly IInMemoryLogStore _store;

        public InMemoryLogger(string name, IInMemoryLogStore store, Func<InMemoryLoggerConfiguration> getCurrentConfig)
        {
            _name = name;
            _store = store;
            _getCurrentConfig = getCurrentConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return default!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            _store.Push(new LogData(message, logLevel));
        }
    }
}

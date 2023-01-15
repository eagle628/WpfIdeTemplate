using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCompany.SampleProduct.CommonLibrary
{
    public interface IInMemoryLogStore : INotifyPropertyChanged
    {
        public void Push(LogData logData);
        public IReadOnlyList<LogData> LogData { get; }
    }
    public readonly struct LogData
    {
        public string Message { get; }
        public LogLevel LogLevel { get; }

        public LogData(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }
    }
}

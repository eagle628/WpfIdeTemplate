using Microsoft.Extensions.Logging;

namespace SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger
{
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

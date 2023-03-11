using Grpc.Core;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;

namespace SampleCompany.SampleProduct.ApplicationEngine.Services
{
    public class ApplicationEngineLoggingService : ApplicationIEngineLogging.ApplicationIEngineLoggingBase
    {
        public override async Task Subscribe(LoggingSubscribeRequest request,
                                       IServerStreamWriter<LogData> responseStream,
                                       ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new LogData() { Message = $"Time at {DateTime.UtcNow}" });
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}

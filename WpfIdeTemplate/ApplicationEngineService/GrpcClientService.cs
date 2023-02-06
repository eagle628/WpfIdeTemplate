using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SampleCompany.SampleProduct.ApplicationEngineService
{
    public class GrpcClientService : BackgroundService
    {
        private readonly ILogger<GrpcClientService> _logger;
        private readonly string _serverExeFilePath;
        private readonly string _grpcAddress;
        private Process? _appEngineProcess;
        public GrpcClientService(GrpcClientServiceConfiguration configuration, ILogger<GrpcClientService> logger)
        {
            _logger = logger;
            _serverExeFilePath = configuration.ServerExeFilePath;
            _serverExeFilePath = @"C:\Users\NaoyaInoue\source\repos\WpfIdeTemplate\WpfIdeTemplate\Result\Debug\Engine\ApplicationEngine.exe";
            _grpcAddress = configuration.GrpcAddress;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var info = new ProcessStartInfo
            {
                FileName = _serverExeFilePath
            };

            _logger.LogInformation("Request Engine Start ...");
            _appEngineProcess = Process.Start(info);

            var channel = GrpcChannel.ForAddress(_grpcAddress);

            _logger.LogInformation("Wait Engine Connection Establish ...");
            await channel.ConnectAsync(CancellationToken.None);
            _logger.LogInformation("Complete Engine Connection Established.");
            channel.Dispose();
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_appEngineProcess is not null)
                {
                    if (!_appEngineProcess.HasExited)
                    {
                        _appEngineProcess.Kill();
                        _appEngineProcess.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
            await base.StopAsync(cancellationToken);
        }
    }
}

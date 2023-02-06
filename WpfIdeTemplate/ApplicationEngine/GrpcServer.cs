using ApplicationEngine.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace SampleCompany.SampleProduct.ApplicationEngine
{
    public class GrpcServer : BackgroundService
    {
        private readonly ILogger<GrpcServer> _logger;

        public GrpcServer(ILogger<GrpcServer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var host = Host.CreateDefaultBuilder()
                           .ConfigureWebHostDefaults(webHostBuilder =>
                           {
                               webHostBuilder.ConfigureKestrel(kestrel =>
                               {
                                   kestrel.ListenLocalhost(5145, options =>
                                   {
                                       options.Protocols = HttpProtocols.Http2;
                                   });
                               })
                                             .UseStartup<GrpcServerStartup>();
                           })
                           .Build();
            await host.StartAsync(stoppingToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }

    public class GrpcServerStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddSingleton<GreeterService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
            });
        }
    }
}

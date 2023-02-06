using Microsoft.AspNetCore.Hosting.Server;

namespace SampleCompany.SampleProduct.ApplicationEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<GrpcServer>();
                });
    }
}

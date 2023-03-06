using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
using SampleCompany.SampleProduct.ApplicationEngineService;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using SampleCompany.SampleProduct.CommonLibrary.UserSettings;
using SampleCompany.SampleProduct.InMemoryLogger;
using SampleCompany.SampleProduct.MainApp.View;
using SampleCompany.SampleProduct.MainApp.ViewModel;
using SampleCompany.SampleProduct.MessageBroker;
using SampleCompany.SampleProduct.PluginUtility;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace SampleCompany.SampleProduct.MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Host
        /// </summary>
        private readonly IHost _host;
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<App> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var applicationEngineLocation = Path.Combine(appLocation, @"Engine\ApplicationEngine.exe");
            

            _host = Host.CreateDefaultBuilder()
                        .ConfigureAppConfiguration((hostingContext, configuration) =>
                        {
                            configuration.Sources.Clear();
                            configuration.SetBasePath(appLocation)
                                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                         .AddJsonFile("usersettings.json", true, true);
                        })
                        .ConfigureServices((hostingContext, services) =>
                        {
                            services.AddSingleton<MainWindowViewModel>()
                                    .AddSingleton<MainWindow>()
                                    .AddSingleton<IInMemoryLogStore, InMemoryLogStore>()
                                    .AddMessageBroker()
                                    .AddSingleton(typeof(IConfiguration), hostingContext.Configuration)
                                    .AddSingleton<UserSettingsManager>()
                                    .AddSingleton(s => new GrpcClientServiceConfiguration(applicationEngineLocation, "http://localhost:5145"))
                                    .AddHostedService<GrpcClientService>();
                            //Grpc Client
                            var address = new Uri("http://localhost:5145");
                            services.AddGrpcClient<Greeter.GreeterClient>(options => options.Address = address);
                            services.AddGrpcClient<ApplicationInstanceManagement.ApplicationInstanceManagementClient>(options=>options.Address=address);
                        })
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders()
                                   //This ExMethod is optional. Automatically inject
                                   .AddConfiguration(hostingContext.Configuration)
                                   .AddDebug()
                                   .AddInMemoryLogger();
                        })
                        .Build();
            _logger = _host.Services.GetRequiredService<ILogger<App>>();
        }
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="e">Start up event args</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            _host.Services.GetRequiredService<MainWindow>().Show();
            base.OnStartup(e);  
        }
        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="e">Exit event args</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();
            base.OnExit(e);
        }
        /// <summary>
        /// Handle exception on not handling exception on UI thread.
        /// </summary>
        /// <param name="sender">Application</param>
        /// <param name="e">Dispatcher unhandled exception event args</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.LogCritical(e.Exception, "DispacherException");
        }
    }
}

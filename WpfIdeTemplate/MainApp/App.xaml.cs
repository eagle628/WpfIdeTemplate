﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
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
    public partial class App : Application, IAppServiceProvider
    {
        /// <summary>
        /// Host
        /// </summary>
        private readonly IHost _host;
        public T? GetService<T>() => _host.Services.GetService<T>();

        public object? GetService(Type serviceType) => _host.Services.GetService(serviceType);

        public T GetRequiredService<T>() where T : notnull => _host.Services.GetRequiredService<T>();

        public object GetRequiredService(Type serviceType) => _host.Services.GetRequiredService(serviceType);
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
            _host = Host.CreateDefaultBuilder()
                        .ConfigureAppConfiguration(config =>
                        {
                            config.SetBasePath(appLocation);
                        })
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton<MainWindowViewModel>()
                                    .AddSingleton<MainWindow>()
                                    .AddSingleton<IInMemoryLogStore, InMemoryLogStore>()
                                    .AddMessageBroker();
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddDebug()
                                   .AddInMemoryLogger()
                                   .SetMinimumLevel(LogLevel.Debug);
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

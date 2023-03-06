using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using SampleCompany.SampleProduct.CommonLibrary.UserSettings;
using SampleCompany.SampleProduct.DockingUtility;
using SampleCompany.SampleProduct.MainApp.View;
using SampleCompany.SampleProduct.PluginUtility;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace SampleCompany.SampleProduct.MainApp.ViewModel
{
    /// <summary>
    /// MainWaindowViewModel
    /// </summary>
    public sealed class MainWindowViewModel : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MainWindowViewModel> _logger;
        public IDockingViewModel ActiveDockingViewModel { get; set; }
        public ObservableCollection<IDocumentViewModel> DocumentsSource { get; }
        public ObservableCollection<IAnchorableViewModel> AnchorablesSource { get; }
        public  ReactiveCommand CallUserSettingCommand { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel(IServiceProvider serviceProvider, ILogger<MainWindowViewModel> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            DocumentsSource = new ObservableCollection<IDocumentViewModel>();
            AnchorablesSource = new ObservableCollection<IAnchorableViewModel>();

            var exefilePath = Assembly.GetExecutingAssembly().Location;
            var exeDir = Path.GetDirectoryName(exefilePath);

            var pluginDirectories = Directory.GetDirectories(Path.Combine(exeDir, "Plugin"));

            if (pluginDirectories is not null)
            {
                foreach (var pluginDirectory in pluginDirectories)
                {
                    var asmDirName = Path.GetFileName(pluginDirectory);
                    var asmPath = Path.Combine(pluginDirectory, $"{asmDirName}.dll");
                    var loadContext = new PluginLoadContext(asmPath);
                    var asm = loadContext.LoadFromAssemblyName(new AssemblyName(asmDirName));
                    if (asm is not null)
                    {
                        var types = asm.GetExportedTypes();

                        //We can Implement construction resolver by reflection,
                        //But, it is simple by IPluginProvider
                        var vmFactoryType = types.Single(o => typeof(IPluginProvider).IsAssignableFrom(o));
                        if (Activator.CreateInstance(vmFactoryType) is not IPluginProvider vmFactory) { continue; }
                        var vm = vmFactory.CreatePluginObject(_serviceProvider);
                        switch (vm)
                        {
                            case IAnchorableViewModel anchorableViewModel:
                                AnchorablesSource.Add(anchorableViewModel);
                                break;
                            case IDocumentViewModel documentViewModel:
                                DocumentsSource.Add(documentViewModel);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            CallUserSettingCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    var v = new UserSettingsView(new UserSettingsViewModel(_serviceProvider.GetRequiredService<UserSettingsManager>()));
                    v.ShowDialog();
                });
        }

        public void Dispose()
        {
            foreach (var disposable in DocumentsSource.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
            foreach (var disposable in AnchorablesSource.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}

using AvalonDock.Layout;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.CommonLibrary;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.LoggerAnchorablePlugin.ViewModel
{
    public class LoggerAnchorableViewModel : IAnchorableViewModel
    {
        public DataTemplate Template { get; }
        private readonly ResourceDictionary _resourceDictionary;
        public AnchorSide InitialLocation => AnchorSide.Bottom;

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly CompositeDisposable _disposables;
        private readonly IInMemoryLogStore _logStore;
        public ReadOnlyReactivePropertySlim<IReadOnlyList<LogData>?> LogData { get; }
        [StyleProperty(BindingMode.OneWay)]
        public string Title => "LoggerAnchorable";
        public LoggerAnchorableViewModel(IInMemoryLogStore logStore)
        {
            _disposables = new CompositeDisposable();
            _logStore = logStore;
            LogData = _logStore.ObserveProperty(o => o.LogData)
                               .Throttle(TimeSpan.FromMilliseconds(100))
                               .ToReadOnlyReactivePropertySlim()
                               .AddTo(_disposables);


            var asmName = Assembly.GetExecutingAssembly().GetName().Name;

            _resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(asmName + ";component/View/Template.xaml", UriKind.Relative),
            };

            Template = _resourceDictionary["4CE57453ECCA4D5E929AEA07D38DFBD3"] as DataTemplate
                ?? throw new NotImplementedException();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

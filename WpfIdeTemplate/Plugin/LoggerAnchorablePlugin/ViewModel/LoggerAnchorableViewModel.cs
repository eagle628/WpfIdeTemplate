using AvalonDock.Layout;
using Grpc.Core;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.Logging;
using ClientLogData = SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger.LogData;
using EngineLogData = SampleCompany.SampleProduct.ApplicationEngine.Proto.LogData;
using System.Reactive.Concurrency;

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
        private readonly ApplicationIEngineLogging.ApplicationIEngineLoggingClient _applicationIEngineLoggingClient;
        private readonly AsyncServerStreamingCall<EngineLogData> _asyncServerStreaming;
        public ReadOnlyReactivePropertySlim<IReadOnlyList<ClientLogData>?> LogData { get; }
        [StyleProperty(BindingMode.OneWay)]
        public string Title => "LoggerAnchorable";
        public LoggerAnchorableViewModel(IInMemoryLogStore logStore,
                                         ApplicationIEngineLogging.ApplicationIEngineLoggingClient engineLoggingClient)
        {
            _disposables = new CompositeDisposable();
            _logStore = logStore;
            _applicationIEngineLoggingClient = engineLoggingClient;
            _asyncServerStreaming = engineLoggingClient.Subscribe(new LoggingSubscribeRequest()).AddTo(_disposables);

            _asyncServerStreaming.ResponseStream.ReadAllAsync()
                                                .ToObservable()
                                                .SubscribeOn(ThreadPoolScheduler.Instance)
                                                .Subscribe(msg => logStore.Push(new ClientLogData(msg.Message, LogLevel.Debug)))
                                                .AddTo(_disposables);

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

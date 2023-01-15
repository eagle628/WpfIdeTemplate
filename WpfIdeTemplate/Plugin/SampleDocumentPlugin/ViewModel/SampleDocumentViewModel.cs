using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.CommonLibrary;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker.MessageStructure;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.SampleDocumentPlugin.ViewModel
{
    public class SampleDocumentViewModel : IDocumentViewModel
    {
        public DataTemplate Template { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        [StyleProperty(BindingMode.OneWay)]
        public string Title => "SampleDocument";
        public ReactivePropertySlim<string> Text0 { get; }
        public ReactivePropertySlim<string> Text1 { get; }
        public ReactivePropertySlim<string> MessageBox { get; }
        public AsyncReactiveCommand PublishCommand { get; }

        private readonly CompositeDisposable _disposables;
        private readonly ResourceDictionary _resourceDictionary;
        private readonly ILogger<SampleDocumentViewModel>_logger;
        private readonly IAsyncPublisher<SampleMessage> _asyncPublisher;

        public SampleDocumentViewModel(
            ILogger<SampleDocumentViewModel> logger,
            IAsyncPublisher<SampleMessage> asynPublisher)
        {
            _logger = logger;
            _asyncPublisher = asynPublisher;

            _disposables = new CompositeDisposable();
            Text0 = new ReactivePropertySlim<string>("Sample0").AddTo(_disposables);
            Text1 = new ReactivePropertySlim<string>("Sample1").AddTo(_disposables);

            MessageBox = new ReactivePropertySlim<string>("Initial", ReactivePropertyMode.DistinctUntilChanged).AddTo(_disposables);
            PublishCommand = new AsyncReactiveCommand().WithSubscribe(async () =>
                                                       {
                                                           _logger.LogDebug("Publish Start");
                                                           await Task.Delay(1000);
                                                           await _asyncPublisher.PublishAsync(new SampleMessage(MessageBox.Value));
                                                           _logger.LogDebug("Publish End");
                                                       })
                                                       .AddTo(_disposables);

            Text0.Pairwise()
                 .Subscribe(msg=>_logger.LogDebug($"Text0 Prop change from {msg.OldItem} to {msg.NewItem}"))
                 .AddTo(_disposables);

            Text1.Pairwise()
                 .Subscribe(msg=>_logger.LogDebug($"Text1 Prop change from {msg.OldItem} to {msg.NewItem}"))
                 .AddTo(_disposables);

            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            _resourceDictionary = new ResourceDictionary()
            {
                //href https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
                Source = new Uri(asmName + ";component/View/Template.xaml", UriKind.Relative),
            };
            //Modify Resource Key (Recommend GUID)
            Template = _resourceDictionary["702E307F7B8C4B13B99EAEBF5CD24362"] as DataTemplate
                ?? throw new NotImplementedException();
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

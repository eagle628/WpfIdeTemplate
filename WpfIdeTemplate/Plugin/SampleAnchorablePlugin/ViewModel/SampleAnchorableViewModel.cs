using AvalonDock.Layout;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker;
using SampleCompany.SampleProduct.CommonLibrary.MessageBroker.MessageStructure;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.SampleAnchorablePlugin.ViewModel
{
    public class SampleAnchorableViewModel : IAnchorableViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ReactivePropertySlim<string> UserInputText { get; }
        public ReadOnlyReactivePropertySlim<string?> DelayedViewText { get; }
        public ReactivePropertySlim<string?> PublishedMessage { get; }

        [StyleProperty(BindingMode.OneWay, ".Value")]
        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("SampleAnchorable");

        public AnchorSide InitialLocation => AnchorSide.Right;

        public DataTemplate Template { get; }

        private readonly CompositeDisposable _disposables;
        private readonly ResourceDictionary _resourceDictionary;
        private readonly ILogger<SampleAnchorableViewModel> _logger;
        private readonly IAsyncSubscriber<SampleMessage> _asynSubscriber;

        public SampleAnchorableViewModel(
            ILogger<SampleAnchorableViewModel> logger,
            IAsyncSubscriber<SampleMessage> asyncSubscriber)
        {
            _logger = logger;
            _asynSubscriber = asyncSubscriber;
            _disposables = new CompositeDisposable();

            UserInputText = new ReactivePropertySlim<string>("Welcome").AddTo(_disposables);
            DelayedViewText = UserInputText.Delay(TimeSpan.FromMilliseconds(100))
                                           .Select(o => o.ToUpper())
                                           .ToReadOnlyReactivePropertySlim()
                                           .AddTo(_disposables);

            UserInputText.Pairwise()
                         .Subscribe(msg => _logger.LogDebug($"UserInputText change from {msg.OldItem} to {msg.NewItem}"))
                         .AddTo(_disposables);

            PublishedMessage = new ReactivePropertySlim<string?>();

            _asynSubscriber.Subscribe((msg, ct) => 
                        {
                            PublishedMessage.Value = msg.Message; 
                            return ValueTask.CompletedTask;
                        })
                       .AddTo(_disposables);
            _asynSubscriber.Subscribe((msg, ct) =>
                        {
                            _logger.LogDebug($"{nameof(SampleAnchorableViewModel)} are published message : {msg.Message}");
                            return ValueTask.CompletedTask;
                        })
                       .AddTo(_disposables);

            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            _resourceDictionary = new ResourceDictionary()
            {
                //href https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
                Source = new Uri(asmName + ";component/View/Template.xaml", UriKind.Relative),
            };
            //Modify Resource Key (Recommend GUID)
            Template = _resourceDictionary["A05545C01A294DCEA9B4A652EEF237C5"] as DataTemplate
                ?? throw new NotImplementedException();
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

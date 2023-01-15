using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reflection;
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

        private readonly CompositeDisposable _disposables;
        private readonly ResourceDictionary _resourceDictionary;
        private readonly ILogger<SampleDocumentViewModel>_logger;

        public SampleDocumentViewModel(ILogger<SampleDocumentViewModel> logger)
        {
            _logger = logger;

            _disposables = new CompositeDisposable();
            Text0 = new ReactivePropertySlim<string>("Sample0").AddTo(_disposables);
            Text1 = new ReactivePropertySlim<string>("Sample1").AddTo(_disposables);

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

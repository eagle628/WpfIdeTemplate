using AvalonDock.Layout;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using SampleCompany.SampleProduct.DockingUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.MainApp.ViewModel
{
    /// <summary>
    /// MainWaindowViewModel
    /// </summary>
    public class MainWindowViewModel
    {
        public ReactiveCollection<IDocumentViewModel> DocumentsSource { get; }
        public ReactiveCollection<IAnchorableViewModel> AnchorablesSource { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            DocumentsSource = new ReactiveCollection<IDocumentViewModel>();
            AnchorablesSource = new ReactiveCollection<IAnchorableViewModel>();

            DocumentsSource.Add(new SampleDocument());
            AnchorablesSource.Add(new SampleAnchorable());
        }
    }

    public interface IDockingViewModel : INotifyPropertyChanged, IDisposable
    {
        
    }
    public interface IDocumentViewModel : IDockingViewModel
    {

    }
    public interface IAnchorableViewModel : IDockingViewModel 
    {

    }

    public class SampleDocument : IDocumentViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        [StyleProperty(BindingMode.OneWay)]
        public string Title => "SampleDocument";
        public ReactivePropertySlim<string> Text0 { get; }
        public ReactivePropertySlim<string> Text1 { get; }

        private readonly CompositeDisposable _disposables;
        public SampleDocument()
        {
            _disposables = new CompositeDisposable();
            Text0 = new ReactivePropertySlim<string>("Sample0").AddTo(_disposables);
            Text1 = new ReactivePropertySlim<string>("Sample1").AddTo(_disposables);
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
    public class SampleAnchorable : IAnchorableViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ReactivePropertySlim<string> UserInputText { get; }
        public ReadOnlyReactivePropertySlim<string?> DelayedViewText { get; }
        
        [StyleProperty(BindingMode.OneWay, ".Value")]
        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("SampleAnchorable");

        private readonly CompositeDisposable _disposables;
        public SampleAnchorable()
        {
            _disposables = new CompositeDisposable();

            UserInputText = new ReactivePropertySlim<string>("Welcome").AddTo(_disposables);
            DelayedViewText = UserInputText.Delay(TimeSpan.FromMilliseconds(100))
                                           .Select(o => o.ToUpper())
                                           .ToReadOnlyReactivePropertySlim()
                                           .AddTo(_disposables);
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
    internal class PaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AnchorableTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                SampleAnchorable => AnchorableTemplate,
                SampleDocument => DocumentTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }
}

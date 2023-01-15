﻿using AvalonDock.Layout;
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
    public interface IAnchorableViewModel : IDockingViewModel, IAnchorable
    {

    }
    public interface IAnchorable
    {
        public AnchorSide InitialLocation { get; }
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

        public AnchorSide InitialLocation => AnchorSide.Bottom;

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
    internal class LayoutUpdate : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is IAnchorable anch)
            {
                var initialLocation = anch.InitialLocation;
                var anchPane = layout.Descendents()
                               .OfType<LayoutAnchorablePane>()
                               .FirstOrDefault(d => d.GetSide() == initialLocation);

                if (anchPane == null)
                {
                    anchPane = CreateAnchorablePane(layout, Orientation.Horizontal, initialLocation);
                }
                anchPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }
        static LayoutAnchorablePane CreateAnchorablePane(LayoutRoot layout, Orientation orientation,
                    AnchorSide initLocation)
        {
            var parent = layout.Descendents().OfType<LayoutPanel>().First(d => d.Orientation == orientation);
            var toolsPane = new LayoutAnchorablePane();
            if (initLocation == AnchorSide.Left)
                parent.InsertChildAt(0, toolsPane);
            else
                parent.Children.Add(toolsPane);
            return toolsPane;
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorable)
        {
            // here set the initial dimensions (DockWidth or DockHeight, depending on location) of your anchorable
            switch (anchorable.GetSide())
            {
                case AnchorSide.Left:
                case AnchorSide.Right:
                    {
                        if (anchorable.Parent is LayoutAnchorablePane pane)
                        {
                            if (pane.DockWidth.Value < 150)
                            {
                                pane.DockWidth = new GridLength(150);
                            }
                        }
                        return;
                    }
                case AnchorSide.Bottom:
                    {
                        if (anchorable.Parent is LayoutAnchorablePane pane)
                        {
                            if (pane.DockHeight.Value < 150)
                            {
                                pane.DockHeight = new GridLength(150);
                            }
                        }
                        return;
                    }
                default:
                    break;
            }
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
            
        }
    }
}
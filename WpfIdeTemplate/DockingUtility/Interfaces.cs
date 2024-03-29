﻿using AvalonDock.Layout;
using System.ComponentModel;
using System.Windows;

namespace SampleCompany.SampleProduct.DockingUtility
{
    /// <summary>
    /// Docking Base View Model
    /// </summary>
    public interface IDockingViewModel : INotifyPropertyChanged, IDisposable
    {
        DataTemplate Template { get; }
    }
    /// <summary>
    /// Document View Model
    /// </summary>
    public interface IDocumentViewModel : IDockingViewModel
    {

    }
    /// <summary>
    /// Anchorable View Model
    /// </summary>
    public interface IAnchorableViewModel : IDockingViewModel
    {
        /// <summary>
        /// Anchorable Initial Location
        /// </summary>
        public AnchorSide InitialLocation { get; }
    }
}

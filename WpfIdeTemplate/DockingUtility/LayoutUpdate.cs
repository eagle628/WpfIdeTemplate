using AvalonDock.Layout;
using System.Windows;
using System.Windows.Controls;

namespace SampleCompany.SampleProduct.DockingUtility
{
    /// <summary>
    /// Layout Strategy for Avalon Dock Content
    /// </summary>
    public class LayoutUpdate : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is IAnchorableViewModel anch)
            {
                var initialLocation = anch.InitialLocation;
                var anchPane = layout.Descendents()
                               .OfType<LayoutAnchorablePane>()
                               .FirstOrDefault(d => d.GetSide() == initialLocation)
                               ??
                               CreateAnchorablePane(layout, Orientation.Horizontal, initialLocation);
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

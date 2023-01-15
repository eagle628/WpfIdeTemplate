using System.Windows;
using System.Windows.Controls;

namespace SampleCompany.SampleProduct.DockingUtility
{
    /// <summary>
    /// Pane Template Selector
    /// </summary>
    public class PaneTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                IDockingViewModel dockingViewModel => dockingViewModel.Template,
                _ => base.SelectTemplate(item, container)
            };
        }
    }
}

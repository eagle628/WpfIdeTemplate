using AvalonDock.Controls;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.DockingUtility
{
    /// <summary>
    /// Style Selector for avalon dock
    /// </summary>
    /// <remarks>
    /// This class can generate binding from <see cref="IDocumentViewModel"/> or <see cref="IAnchorableViewModel"/>
    /// to <see cref="LayoutDocumentItem"/> or <see cref="LayoutAnchorableItem"/>
    /// by properties by sepecified <see cref="StylePropertyAttribute"/>.
    /// </remarks>
    public class ContentPropertyStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            return container switch
            {
                LayoutDocumentItem => CreateStyle<LayoutDocumentItem>(item, container),
                LayoutAnchorableItem => CreateStyle<LayoutAnchorableItem>(item, container),
                _ => base.SelectStyle(item, container),
            };
        }

        private Style CreateStyle<T>(object item, DependencyObject container) where T : LayoutItem
        {
            if (container is not T layoutItem)
            {
                return base.SelectStyle(item, container);
            }
            var style = new Style
            {
                TargetType = typeof(T)
            };

            var props = item.GetType().GetProperties(BindingFlags.Public
                                                     | BindingFlags.Instance
                                                     | BindingFlags.FlattenHierarchy);
            foreach (var prop in props)
            {
                //Does property have a cuctom attribute?
                var attr = (StylePropertyAttribute?)prop.GetCustomAttributes(typeof(StylePropertyAttribute), true).FirstOrDefault();
                if (attr is null) { continue; }
                //Search Style DP in UI Class (By DP Coding Style)
                var fieldInfo = layoutItem.GetType().GetField($"{attr.StyleName ?? prop.Name}Property",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (fieldInfo is null) { continue; }
                //Convert Sylte DP
                var dp = (DependencyProperty?)fieldInfo.GetValue(null);
                if (dp is null) { continue; }
                //Generate binding between vm and v.
                string bindingPath = attr.AdditionalBindingPath is null ? prop.Name : $"{prop.Name}{attr.AdditionalBindingPath}";
                var binding = new Binding(bindingPath)
                {
                    Source = item,
                    Mode = attr.BindingMode
                };
                //make Setter
                var setter = new Setter
                {
                    Property = dp,
                    Value = binding
                };
                //Add Style
                style.Setters.Add(setter);
            }

            return style;
        }
    }
}

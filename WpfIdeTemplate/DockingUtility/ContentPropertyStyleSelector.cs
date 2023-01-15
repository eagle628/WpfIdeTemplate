using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using AvalonDock.Controls;

namespace SampleCompany.SampleProduct.DockingUtility
{
    /// <summary>
    /// Style Selector for avalon dock
    /// </summary>
    /// <remarks>
    /// AvalonDockのFrame側にStyleを伝搬させるためのSelector。
    /// <see cref="StylePropertyAttribute"/>をマークされたプロパティからBindingが自動生成される。
    /// </remarks>
    public class ContentPropertyStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            switch (container)
            {
                case LayoutDocumentItem:
                    return CreateStyle<LayoutDocumentItem>(item, container);
                case LayoutAnchorableItem:
                    return CreateStyle<LayoutAnchorableItem>(item, container);
                default:
                    return base.SelectStyle(item, container);
            }
        }

        private Style CreateStyle<T>(object item, DependencyObject container) where T : LayoutItem
        {
            //コンテナがTでない場合はとりあえず普通のStyleを返す
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
                //対象のプロパティのカスタム属性を探す
                var attr = (StylePropertyAttribute?)prop.GetCustomAttributes(typeof(StylePropertyAttribute), true).FirstOrDefault();
                //なければ飛ばす
                if (attr is null) { continue; }
                //UIクラスのスタイルDPを検索
                var fieldInfo = layoutItem.GetType().GetField($"{attr.StyleName ?? prop.Name}Property",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //なければ飛ばす
                if (fieldInfo is null) { continue; }
                //スタイルDPを取得
                var dp = (DependencyProperty?)fieldInfo.GetValue(null);
                if (dp is null) { continue; }//まずないが}
                //新しくVMとV間のBindingを作成する
                string bindingPath = attr.AdditionalBindingPath is null ? prop.Name : $"{prop.Name}{attr.AdditionalBindingPath}";
                var binding = new Binding(bindingPath)
                {
                    Source = item,
                    Mode = attr.BindingMode
                };
                //セッターを作成
                var setter = new Setter
                {
                    Property = dp,
                    Value = binding
                };
                //スタイルに追加
                style.Setters.Add(setter);
            }

            return style;
        }
    }
}

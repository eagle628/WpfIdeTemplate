using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SampleCompany.SampleProduct.DockingUtility
{

    /// <summary>
    /// Style Property Marker Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class StylePropertyAttribute : Attribute
    {
        /// <summary>
        /// Binding Mode
        /// </summary>
        public BindingMode BindingMode { get; }
        /// <summary>
        /// Additional Binding Path
        /// </summary>
        /// <remarks>In the case of null, property name use.</remarks>
        /// <example>
        /// In the case of ReactiveProperty, this property is ".Value".
        /// </example>
        public string? AdditionalBindingPath { get; }
        /// <summary>
        /// StyleName
        /// </summary>
        /// <remarks>In the case of null, property name use.</remarks>
        public string? StyleName { get; }

        public StylePropertyAttribute(BindingMode bindingMode = BindingMode.Default, string? additionalBindingPath = null, string? styleName = null)
        {
            BindingMode = bindingMode;
            AdditionalBindingPath = additionalBindingPath;
            StyleName = styleName;
        }
    }
}

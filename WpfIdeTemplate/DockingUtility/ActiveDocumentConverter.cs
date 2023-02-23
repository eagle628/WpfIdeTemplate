using System.Windows.Data;

namespace SampleCompany.SampleProduct.DockingUtility
{
    public class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IDockingViewModel)
                return value;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IDockingViewModel)
                return value;

            return Binding.DoNothing;
        }
    }
}

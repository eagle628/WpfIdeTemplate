using SampleCompany.SampleProduct.MainApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SampleCompany.SampleProduct.MainApp.View
{
    /// <summary>
    /// UserSettings.xaml の相互作用ロジック
    /// </summary>
    public partial class UserSettingsView : Window
    {
        public UserSettingsView(UserSettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}

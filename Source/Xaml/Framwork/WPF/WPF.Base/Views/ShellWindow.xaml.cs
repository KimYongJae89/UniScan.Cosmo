using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.Base.ViewModels;

namespace WPF.Base.Views
{
    public class ManuItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] ?? values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(type => Binding.DoNothing).ToArray();
        }
    }

    /// <summary>
    /// ShellWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShellWindow : MetroWindow
    {
        ShellViewModel ViewModel = new ShellViewModel();

        public ShellWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        public void Initialize(IEnumerable<NavigationMenuItem> navigationMenuItems, IEnumerable<NavigationMenuItem> navigationOptionMenuItems, ICustomSettingControl userControl = null)
        {
            ViewModel.Initialize(Menu.OptionsItemsSource as HamburgerMenuItemCollection, navigationMenuItems, navigationOptionMenuItems);
            (Resources["SettingPage"] as SettingPage).AppendCustomControl(userControl);
            Menu.SelectedItem = navigationMenuItems.First();
        }
    }
}

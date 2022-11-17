using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Base.Helpers;
using WPF.Base.ViewModels;

namespace WPF.Base.Views
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashWindow : MetroWindow
    {
        public SplashViewModel ViewModel = new SplashViewModel();

        public SplashWindow()
        {
            InitializeComponent();
        }

        public void Initialize(RelayCommand keyCommand)
        {
            ViewModel.Initialize(keyCommand);
            DataContext = ViewModel;
        }
    }
}

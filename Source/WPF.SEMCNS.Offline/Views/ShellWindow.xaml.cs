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
using MahApps.Metro.Controls;
using WPF.Base.Services;
using WPF.SEMCNS.Offline.ViewModels;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// ShellWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShellWindow : MetroWindow
    {
        ShellViewModel ViewModel = new ShellViewModel();

        public ShellWindow()
        {
            InitializeComponent();

            ShellNavigationService.Frame = mainFrame;
            
            DataContext = ViewModel;
        }

        public void Initialize(Type defualtPageType)
        {
            ViewModel.Initialize(NavigationStackPanel.Children, defualtPageType);
        }
    }
}

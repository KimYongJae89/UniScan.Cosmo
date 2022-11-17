using MahApps.Metro.Controls.Dialogs;
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
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.ViewModels;

namespace WPF.Base.Views
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : UserControl, IInitializable
    {
        SettingViewModel ViewModel = new SettingViewModel();

        public SettingPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            //ViewModel.Initialize(DialogCoordinator.Instance);
            //DataContext = ViewModel;
        }

        public void AppendCustomControl(ICustomSettingControl userControl)
        {
            ViewModel.Initialize(DialogCoordinator.Instance, userControl);
            DataContext = ViewModel;
        }
    }
}

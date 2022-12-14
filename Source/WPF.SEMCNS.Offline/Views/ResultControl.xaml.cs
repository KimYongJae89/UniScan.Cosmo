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
using WPF.SEMCNS.Offline.Services;
using WPF.SEMCNS.Offline.ViewModels;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// ModelPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultControl : UserControl
    {
        public ResultViewModel ViewModel = new ResultViewModel();

        public ResultControl()
        {
            InitializeComponent();

            this.DataContext = ViewModel;
            ViewModel.DialogCoordinator = DialogCoordinator.Instance; 
        }
    }
}

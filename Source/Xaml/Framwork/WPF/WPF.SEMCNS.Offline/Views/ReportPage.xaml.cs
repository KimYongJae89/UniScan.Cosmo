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
using WPF.Base.Helpers;
using WPF.SEMCNS.Offline.ViewModels;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReportPage : UserControl, IInitializable
    {
        ReportViewModel ViewModel = new ReportViewModel();

        public ReportPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ResultControl.ViewModel.Initialize(ViewModel);
            ImageControl.Intialize();
            DefectControl.Initialize(ImageControl.ViewModel);
            ViewModel.Initialize(ImageControl.ViewModel, DefectControl.ViewModel);

            DataContext = ViewModel;
        }
    }
}

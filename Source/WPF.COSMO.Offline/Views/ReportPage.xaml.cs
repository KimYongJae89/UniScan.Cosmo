using DynMvp.Devices;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using MahApps.Metro.Controls.Dialogs;
using WPF.Base.Services;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReportPage : UserControl, IInitializable
    {
        public ReportViewModel ViewModel = new ReportViewModel();

        public ReportPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ViewModel.Initialize(DialogCoordinator.Instance, FilterControl.ViewModel);
            //AxisImageControl.Intialize(false);
            //DefectListControl.Initialize(false);
            //SectionChart.Initialize(false);
            //SizeChart.Initialize(false);
            //SummaryListControl.Initialize(false);
            //CompareChart.Initialize(false);

            DataContext = ViewModel;
        }
    }
}

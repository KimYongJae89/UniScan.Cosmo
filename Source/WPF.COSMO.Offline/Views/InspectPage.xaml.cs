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
using WPF.COSMO.Offline.Services;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : UserControl, IInitializable
    {
        public InspectViewModel ViewModel = new InspectViewModel();

        public InspectPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            DefectStorage defectStorage = new DefectStorage();
            ViewModel.Initialize(DialogCoordinator.Instance, defectStorage, DefectItem, ChartItem);
            AxisImageControl.Initialize(defectStorage);
            CompareChart.Initialize(defectStorage);
            DefectListControl.Initialize(defectStorage, AxisImageControl.ViewModel.ZoomService);
            SummaryListControl.Initialize(defectStorage);
            SectionChart.Initialize(defectStorage);
            SizeChart.Initialize(defectStorage);

            DataContext = ViewModel;
        }
    }
}

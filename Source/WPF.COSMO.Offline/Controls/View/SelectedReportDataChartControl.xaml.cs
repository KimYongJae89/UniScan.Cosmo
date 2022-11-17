using MahApps.Metro.SimpleChildWindow;
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
using WPF.COSMO.Offline.Controls.ViewModel;

namespace WPF.COSMO.Offline.Controls.View
{
    /// <summary>
    /// SelectedReportDataChartControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectedReportDataChartControl : ChildWindow
    {
        public object ItemsSource
        {
            set
            {
                SelectedReportDataChartViewModel vm = this.DataContext as SelectedReportDataChartViewModel;
                vm.SetResultList(value);
            }
        }

        public string ChartTitle
        {
            set
            {
                SelectedReportDataChartViewModel vm = this.DataContext as SelectedReportDataChartViewModel;
                vm.ChartTitle = value;
            }
        }

        public Models.DefectViewMode DefectViewMode
        {
            set
            {
                SelectedReportDataChartViewModel vm = this.DataContext as SelectedReportDataChartViewModel;
                vm.DefectViewMode = value;
            }
        }

        public SelectedReportDataChartControl()
        {
            InitializeComponent();
        }
    }
}

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
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Controls.Views
{
    /// <summary>
    /// SectionChart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SectionChart : UserControl
    {
        SectionChartViewModel ViewModel = new SectionChartViewModel();

        public SectionChart()
        {
            InitializeComponent();
        }

        public void Initialize(DefectStorage defectStorage)
        {
            ViewModel.Initialize(defectStorage);
            DataContext = ViewModel;
        }
    }
}

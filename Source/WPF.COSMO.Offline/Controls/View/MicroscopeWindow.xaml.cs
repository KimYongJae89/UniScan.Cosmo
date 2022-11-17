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
using System.Windows.Shapes;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.View
{
    /// <summary>
    /// MicroscopeWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MicroscopeWindow : ChildWindow
    {
        MicroscopeViewModel ViewModel = new MicroscopeViewModel();

        public MicroscopeWindow()
        {
            InitializeComponent();
        }

        public void Initialize(CosmoLotNoInfo cosmoLotNoInfo, IEnumerable<Defect> defects)
        {
            ViewModel.Initilaize(this, imageCanvas, cosmoLotNoInfo, defects);

            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(() => DataContext = ViewModel));
        }

        private async void DefectList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var grid = sender as DataGrid;
                if (grid == null)
                    return;

                var defect = grid.SelectedItem as Defect;

                if (AxisGrabService.CheckDoorLock() == false)
                    return;

                if (await MicroscopeGrabService.MoveDefectPos(defect).ConfigureAwait(false) == false)
                    return;
            }
        }
    }
}

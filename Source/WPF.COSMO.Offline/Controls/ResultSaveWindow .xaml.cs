using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPF.Base.Models;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using InspectResult = WPF.COSMO.Offline.Services.InspectResult;

namespace WPF.COSMO.Offline.Controls
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultSaveWindow : ChildWindow
    {
        public ResultSaveProcess Process { get; set; } = new ResultSaveProcess();
        
        ReportInfo _reportInfo;
        InspectResult _inspectResult;
        
        public ResultSaveWindow(ReportInfo reportInfo, InspectResult inspectResult)
        {
            _reportInfo = reportInfo;
            _inspectResult = inspectResult;

            InitializeComponent();
        }
        
        private async void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ResultService.SaveResultAsync(Process, _reportInfo, _inspectResult);

            Close();
        }
    }
}
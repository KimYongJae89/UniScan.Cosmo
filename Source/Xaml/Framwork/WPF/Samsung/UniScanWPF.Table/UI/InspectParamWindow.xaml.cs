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

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// InspectParamWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectParamWindow : Window
    {
        public InspectParamWindow()
        {
            InitializeComponent();

            this.DataContext = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
            this.EtcGrid.DataContext = UniEye.Base.Settings.OperationSettings.Instance();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SystemManager.Instance().OperatorManager.InspectOperator.Settings.Save();
        }
    }
}

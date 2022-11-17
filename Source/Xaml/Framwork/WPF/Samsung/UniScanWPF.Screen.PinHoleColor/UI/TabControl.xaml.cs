using DynMvp.Base;
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
using UniEye.Base;
using UniEye.Base.Data;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// MainTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TabControl : UserControl
    {
        ModelWindow mw;
        InspectPage inspectPage;
        ReportPage reportPage;

        public TabControl()
        {
            InitializeComponent();

            mw = new ModelWindow(SystemManager.Instance().ModelManager);
            mw.Topmost = true;
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            inspectPage = new InspectPage();
            reportPage = new ReportPage();

            Frame.Navigate(inspectPage);
            InspectButton.Background = App.Current.Resources["MainBrush"] as Brush;
            InspectButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;
        }
        
        private void InspectButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(inspectPage);
            InspectButton.Background = App.Current.Resources["MainBrush"] as Brush;
            InspectButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;
            ReportButton.Background = Brushes.White;
            ReportButton.Foreground = App.Current.Resources["FontBrush"] as Brush;
        }

        private void ModelButton_Click(object sender, RoutedEventArgs e)
        {
            mw.Show();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(reportPage);
            InspectButton.Background = Brushes.White; 
            InspectButton.Foreground = App.Current.Resources["FontBrush"] as Brush; 
            ReportButton.Background = App.Current.Resources["MainBrush"] as Brush;
            ReportButton.Foreground = App.Current.Resources["FontWhiteBrush"] as Brush;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemState.Instance().GetOpState() != OpState.Idle)
                CustomMessageBox.Show(StringManager.GetString("Message", "Please, Stop the inspection."), "UniEye", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (CustomMessageBox.Show(StringManager.GetString("Message", "Do you want to exit program?"), "UniEye", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                SystemManager.Instance().Close();
        }
    }
}

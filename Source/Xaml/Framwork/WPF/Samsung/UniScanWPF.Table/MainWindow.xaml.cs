using DynMvp.Authentication;
using DynMvp.Base;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniScanWPF.Screen.Table.UI;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.UI;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, IUserHandlerListener
    {
        MainPage mainPage;
        ReportPage reportPage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().InitTableUnit();

            mainPage = new MainPage();
            mainPage.OnNavigated = mainPage_OnNavigated;
            reportPage = new ReportPage();

            mainFrame.Navigate(mainPage);

            statusStripGrid.Children.Add(new StatusStrip());
            MenuPanel.DataContext = InfoBox.Instance;

            programTitle.DataContext = CustomizeSettings.Instance();

            AlarmWindow alarmWindow = new AlarmWindow();

            UserHandler.Instance().AddListener(this);
            UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op", "op");
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show("Are you sure you want to exit the program ? ", "Exit", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                SystemManager.Instance().Close();
        }

        private void UserLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            if (logInWindow.ShowDialog() == true)
                UserHandler.Instance().CurrentUser = logInWindow.User;
        }

        public void UserChanged()
        {

        }

        private void InspectRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(mainPage);
            mainPage.Navigate(PageType.Inspect);
        }

        private void TeachRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(mainPage);
            mainPage.Navigate(PageType.Teach);
        }

        private void ModelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(mainPage);
            mainPage.Navigate(PageType.Model);
        }

        private void ReportRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(reportPage);
        }

        private void SettingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(mainPage);
            mainPage.Navigate(PageType.Setting);
            //if (SystemManager.Instance().MachineObserver.IoBox.InOnDoor1 == false || SystemManager.Instance().MachineObserver.IoBox.InOnDoor2 == false)
            //{
            //    CustomMessageBox.Show("Door is open !!", "Cancle", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
            //    return;
            //}

            //MachineOperator.MoveHome(0, null, new CancellationTokenSource());
        }

        private void mainPage_OnNavigated(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Inspect:
                    InspectRadioButton.IsChecked = true;
                    break;
                case PageType.Model:
                    ModelRadioButton.IsChecked = true;
                    break;
                case PageType.Teach:
                    TeachRadioButton.IsChecked = true;
                    break;
                case PageType.Setting:
                    SettingRadioButton.IsChecked = true;
                    break;
            }
        }


    }
}
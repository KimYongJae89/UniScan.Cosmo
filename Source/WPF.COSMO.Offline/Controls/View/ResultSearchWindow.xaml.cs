using MahApps.Metro.Controls.Dialogs;
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
    /// ResultSearchControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultSearchWindow : ChildWindow
    {
        ResultSearchViewModel ViewModel = new ResultSearchViewModel();

        public ResultSearchWindow()
        {
            InitializeComponent();

            ViewModel.Initialize(DialogCoordinator.Instance, FilterControl.ViewModel);

            this.DataContext = ViewModel;
        }

        private void GlyphButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

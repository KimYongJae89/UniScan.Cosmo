using DynMvp.Devices;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using WPF.Base.Helpers;
using WPF.Base.ViewModels;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Views
{
    public partial class DeveloperPage : UserControl, IInitializable
    {
        DeveloperViewModel ViewModel = new DeveloperViewModel();

        public DeveloperPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ViewModel.Initialize();
            DataContext = ViewModel;
        }
    }
}

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
using WPF.Base.ViewModels;

namespace WPF.Base.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelPage : UserControl, IInitializable
    {
        public ModelViewModel ViewModel = new ModelViewModel();

        public ModelPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ViewModel.Initialize(DialogCoordinator.Instance);

            DataContext = ViewModel;
        }
    }
}

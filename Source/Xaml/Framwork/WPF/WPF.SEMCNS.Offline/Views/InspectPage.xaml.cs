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
using WPF.SEMCNS.Offline.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using WPF.Base.Services;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : UserControl, IInitializable
    {
        public InspectViewModel ViewModel = new InspectViewModel();

        public InspectPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            ViewModel.Initialize(DialogCoordinator.Instance);
            ImageControl.Intialize();
            DefectControl.Initialize(ImageControl.ViewModel);

            DataContext = ViewModel;
        }
    }
}

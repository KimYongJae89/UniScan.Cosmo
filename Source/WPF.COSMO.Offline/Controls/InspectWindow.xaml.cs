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

namespace WPF.COSMO.Offline.Controls
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectWindow : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public InspectProcess Process { get; set; } = new InspectProcess();

        ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        Model_COSMO _model;
        CancellationTokenSource _cts;
        
        public InspectWindow(Model_COSMO model, CancellationTokenSource cts)
        {
            _model = model;
            _cts = cts;

            InitializeComponent();
        }

        void Cancel()
        {
            _cts.Cancel();

            this.Close(false);
        }
        
        private async void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await InspectService.InitializeAsync(_model, Process, _cts.Token);
            
            Close(await InspectService.InspectionAsync());
        }
    }
}
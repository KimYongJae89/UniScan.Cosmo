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
    public partial class ExcelExportWindow : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ExcelExportProcess Process { get; set; } = new ExcelExportProcess();

        ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        CancellationTokenSource _cts;
        DefectStorage _defectStorage;

        public ExcelExportWindow(DefectStorage defectStorage)
        {
            _defectStorage = defectStorage;
            InitializeComponent();
        }

        void Cancel()
        {
            _cts.Cancel();

            this.Close(false);
        }
        
        private async void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _cts = new CancellationTokenSource();

            ExcelExportService.Initialize(Process, _cts.Token);

            if (await ExcelExportService.InspectResultExport(_defectStorage) == true)
                Close();
        }
    }
}
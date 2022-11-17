using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.Base.Views;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using static WPF.COSMO.Offline.Models.PortMap;

namespace WPF.COSMO.Offline.ViewModels
{
    public class DeveloperViewModel
    {
        public IEnumerable<AxisGrabInfo> AxisGrabInfos => AxisGrabService.Settings.AxisGrabInfoList;
        public IEnumerable<ImageDevice> ImageDevices => SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList;
        public IEnumerable<AxisHandler> AxisHandlers => MotionService.Handlers;
        public IEnumerable<string> TriggerPorts => Enum.GetNames(typeof(OutPortName));
        public IEnumerable<ScanDirection> ScanDirectionEnum => Enum.GetValues(typeof(ScanDirection)).Cast<ScanDirection>();

        public ServiceSettings ServiceSettings => AxisGrabService.Settings;
        public MicroscopeServiceSettings MicroscopeSettings => MicroscopeGrabService.Settings;

        ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(Save));

        ICommand _axisDeviceAddCommand;
        public ICommand AxisDeviceAddCommand => _axisDeviceAddCommand ?? (_axisDeviceAddCommand = new RelayCommand(AxisGrabService.AddAxisDevice));

        ICommand _calibrationCommand;
        public ICommand CalibrationCommand => _calibrationCommand ?? (_calibrationCommand = new RelayCommand(Calibration));
        

        public void Initialize()
        {

        }

        public void Calibration()
        {
            CalibrationWindow calibrationWindow = new CalibrationWindow();
            calibrationWindow.ShowDialog();
        }

        public async void Save()
        {
            await AxisGrabService.SaveAxisGrabInfos();
            await MicroscopeGrabService.SaveMicroscopeServiceSettings();
        }
    }
}

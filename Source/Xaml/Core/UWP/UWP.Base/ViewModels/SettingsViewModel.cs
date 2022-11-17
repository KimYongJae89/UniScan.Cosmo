using Standard.DynMvp.Devices;
using Standard.DynMvp.Devices.ImageDevices;
using Standard.DynMvp.Base.Helpers;
using System; 
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Settings;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Standard.DynMvp.Devices.Helpers;
using Standard.DynMvp.Devices.MotionController;
using System.Collections.ObjectModel;
using Standard.DynMvp.Devices.LightController;

namespace UWP.Base.ViewModels
{
    public class SettingsViewModel : Observable
    {
        ManualResetEvent _setupEvent;

        public ObservableCollection<SettingData> OperationSettings { get; set; }
        public ObservableCollection<DeviceInfo> DeviceSettings { get; set; }

        ICommand _acceptCommand;
        public ICommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(Accept));

        ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand<object>(AddDevice));

        ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand<DeviceInfo>(RemoveDevice));

        public SettingsViewModel()
        {

        }

        public void Initialize(ManualResetEvent setupEvent)
        {
            DeviceInfo.DefaultAddCommand = AddCommand;
            DeviceInfo.DefaultRemoveCommand = RemoveCommand;

            _setupEvent = setupEvent;

            OperationSettings = SettingDataAttribute.GetProperties(Singleton<OperationSettings>.Instance);
            DeviceSettings = DeviceDataAttribute.GetProperties(Singleton<DeviceSettings>.Instance);
        }

        private void AddDevice(object arg)
        {
            if (arg is DeviceInfo)
            {
                var deviceInfo = arg as DeviceInfo;
                switch (deviceInfo.DeviceType)
                {
                    case DeviceType.FrameGrabber:
                        (deviceInfo as GrabberInfo).CreateCamera();
                        break;
                }
            }
            else if (arg is string)
            {
                switch (arg)
                {
                    case "MultiCam":
                        DeviceSettings.Add(new GrabberInfo(arg as string, GrabberType.MultiCam));
                        break;
                    case "GenTL":
                        DeviceSettings.Add(new GrabberInfo(arg as string, GrabberType.GenTL));
                        break;
                    case "VirtualGrabber":
                        DeviceSettings.Add(new GrabberInfo(arg as string, GrabberType.Virtual));
                        break;
                    case "AlphaMotion":
                        DeviceSettings.Add(new MotionInfo(arg as string, MotionType.AlphaMotionBx));
                        break;
                    case "VIT":
                        DeviceSettings.Add(new LightControllerInfo(arg as string, LightControllerType.VIT));
                        break;
                }
            }
        }

        private void RemoveDevice(DeviceInfo arg)
        {
            if (arg.DeviceType == DeviceType.Camera)
            {
                foreach (var info in DeviceSettings)
                {
                    if (info.DeviceType == DeviceType.FrameGrabber)
                        (info as GrabberInfo).CameraInfos.Remove(arg as CameraInfo);
                }
            }
            else
            {
                DeviceSettings.Remove(arg);
            }
        }

        private async void Accept()
        {
            var newDeviceSettings = new DeviceSettings();
            DeviceDataAttribute.SetProperties(newDeviceSettings, DeviceSettings);
            Singleton<DeviceSettings>.SetInstance(newDeviceSettings);

            await Singleton<OperationSettings>.Instance.SaveSettingsAsync();
            await Singleton<DeviceSettings>.Instance.SaveSettingsAsync();

            _setupEvent.Set();
        }
    }
}
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Base.Selectors
{
    public class DeviceDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GrabberDataTemplate { get; set; }
        public DataTemplate CameraDataTemplate { get; set; }
        public DataTemplate MotionControllerDataTemplate { get; set; }
        public DataTemplate LightControllerDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var data = item as DeviceInfo;

            switch (data.DeviceType)
            {
                case DeviceType.FrameGrabber:
                    return GrabberDataTemplate;
                case DeviceType.MotionController:
                    return MotionControllerDataTemplate;
                case DeviceType.DigitalIo:
                    break;
                case DeviceType.LightController:
                    return LightControllerDataTemplate;
                case DeviceType.DaqChannel:
                    break;
                case DeviceType.Camera:
                    return CameraDataTemplate;
                case DeviceType.DepthScanner:
                    break;
                case DeviceType.BarcodeReader:
                    break;
                case DeviceType.BarcodePrinter:
                    break;
                case DeviceType.Serial:
                    break;
            }

            return null;
        }
    }
}

using System;
using System.Drawing;
using System.Linq;
using System.Xml;

using System.Collections.Generic;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Devices.MotionController;
using System.Threading.Tasks;
using Windows.Storage;
using Standard.DynMvp.Base.Helpers;
using UWP.Base.Helpers;
using Standard.DynMvp.Devices.ImageDevices;
using Standard.DynMvp.Devices.Helpers;
using System.Windows.Input;
using Standard.DynMvp.Devices;

namespace UWP.Base.Settings
{
    public class DeviceSettings : Setting<DeviceSettings>
    {
        [DeviceData]
        public List<DeviceInfo> DeviceInfoList { get; set; } = new List<DeviceInfo>();

        public DeviceSettings() : base("Device")
        {
        }
    }
}
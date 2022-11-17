using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using DynMvp.Device.Device.MotionController;
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using UniScan.Common.Settings;
using UniScan.Monitor.Settings.Monitor;

namespace UniScan.Monitor.Device.Gravure
{
    public abstract class DeviceControllerExtender
    {
        protected DeviceController deviceController = null;
        public DeviceControllerExtender(DeviceController deviceController)
        {
            this.deviceController = deviceController;
        }
    }
}

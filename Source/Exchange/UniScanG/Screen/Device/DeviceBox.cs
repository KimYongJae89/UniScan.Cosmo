using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Device.Serial;
using DynMvp.UI;
using UniEye.Base.Device;

namespace UniScanG.Screen.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap)
        {
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            SerialDeviceHandler sdh = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialDevice sd = sdh.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder);
            if (sd != null)
            {
                SerialEncoder se = (SerialEncoder)sd;
                string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.DV, "11");
            }
        }
    }
}

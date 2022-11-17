using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Screen.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            InMachineRun,
            OutMachineStop
        }

        public PortMap() : base()
        {
            Initialize(typeof(IoPortName));
        }
    }
}

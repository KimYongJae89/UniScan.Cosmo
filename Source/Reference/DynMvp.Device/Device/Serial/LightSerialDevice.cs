using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Serial
{
    class LightSerialDeviceInfo : SerialDeviceInfo
    {
        public override SerialDevice BuildSerialDevice(bool virtualMode)
        {
            throw new NotImplementedException();
        }

        public override SerialDeviceInfo Clone()
        {
            throw new NotImplementedException();
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            base.CopyFrom(serialDeviceInfo);
        }
    }
}

using DynMvp.Device.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Serial
{
    public class SerialDeviceInfoFactory
    {
        public static SerialDeviceInfo CreateSerialDeviceInfo(ESerialDeviceType deviceType)
        {
            switch (deviceType)
            {
                case ESerialDeviceType.SerialEncoder:
                    return new SerialEncoderInfo();
                case ESerialDeviceType.SerialSensor:
                    return new SerialSensorInfo();
                default:
                    return null;
            }
        }
    }



    //public class SerialDeviceFactory
    //{
    //   
    //}
}

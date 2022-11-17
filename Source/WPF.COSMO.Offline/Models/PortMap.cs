using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.COSMO.Offline.Models
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum InPortName
        {
            InEmergency,
            InSpare,
            InDoor1, InDoor2,
            InServoOnX1, InServoOnX2, InServoOnY
        }

        public enum OutPortName
        {
            OutGreen, OutYellow, OutRed, OutBuzzer,
            OutGrab1, OutGrab2, OutIonizerPowerOn,
            OutSpare1, OutSpare2, OutSpare3, OutSpare4, OutSpare5,
            OutServoOn, OutDoor, OutIonizerAir, OutInnerLight
        }

        public PortMap() : base(false)
        {
            Initialize(typeof(InPortName), 0);
            Initialize(typeof(OutPortName), 0);
        }

        public void Initialize(Type ioPortNameType, int offset)
        {
            AddInPorts(ioPortNameType, offset);
            AddOutPorts(ioPortNameType, offset);
        }

        private void AddInPorts(Type ioPortNameType, int offset)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            int index = offset;
            foreach (string name in names)
                if (name.Substring(0, 2) == "In")
                    AddInPort(new IoPort(name, index++));
        }

        private void AddOutPorts(Type ioPortNameType, int offset)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            int index = offset;
            foreach (string name in names)
                if (name.Substring(0, 3) == "Out")
                    AddOutPort(new IoPort(name, index++));
        }

        public override IoPort[] GetTowerLampPort()
        {
            IoPort[] ports = new IoPort[4];
            ports[0] = GetOutPort("OutRed");
            ports[1] = GetOutPort("OutYellow");
            ports[2] = GetOutPort("OutGreen");
            ports[3] = GetOutPort("OutBuzzer");

            return ports;
        }
    }
}

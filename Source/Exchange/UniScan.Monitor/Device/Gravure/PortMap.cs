using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Settings;

namespace UniScan.Monitor.Device.Gravure
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            // in
            InEmergency, InDoorOpenR, InDoorOpenL, InAirPressure,   // TestBed
            InLaserAlive, InLaserReady, InLaserMarkDone, InLaserError, InLaserOutOfRange,   // LaserControl

            // out
            OutDoorOpen, OutVaccumOn, OutIonizerSol, OutAirFan, OutRoomLight, OutIonizer, OutTowerRed, OutTowerYellow, OutTowerGreen, OutTowerBuzzer,   // TestBed
            OutLaserAlive, OutLaserEmergency, OutLaserReset, OutLaserRun, OutLaserNG // LaserControl
        }

        public PortMap()
        {
            this.InPortList.ClearPort();
            this.OutPortList.ClearPort();
            Initialize(typeof(IoPortName));
        }

        public override void Load()
        {
            String filePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            if (File.Exists(filePath))
            {
                Load(filePath);
            }
            else
            {
                //Initialize(typeof(IoPortName));
            }
        }

        public override void Save()
        {
            String filePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            Save(filePath);
        }

        public override IoPort[] GetTowerLampPort()
        {
            return new IoPort[]
            {
                GetOutPort(IoPortName.OutTowerRed),
                GetOutPort(IoPortName.OutTowerYellow),
                GetOutPort(IoPortName.OutTowerGreen),
                GetOutPort(IoPortName.OutTowerBuzzer)
            };
        }
    }
}

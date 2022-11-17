using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Settings;

namespace UniScan.Inspector.Device.Gravure
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            OutVisionNG
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
                Save(filePath);
            }
        }

        public override void Save()
        {
            String filePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            Save(filePath);
        }
    }
}

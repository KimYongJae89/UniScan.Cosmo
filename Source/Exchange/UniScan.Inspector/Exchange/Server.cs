using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Inspector.Data;
using UniScan.Inspector.Settings.Inspector;

namespace UniScan.Inspector.Exchange
{
    class Server : TcpIpMachineIfServer
    {
        List<SlaveObj> slaveList = new List<SlaveObj>();
        internal List<SlaveObj> SlaveList
        {
            get { return slaveList; }
        }

        public Server(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            foreach (InspectorInfo inspectorInfo in InspectorSystemSettings.Instance().SlaveInfoList)
            {
                SlaveInfo slaveInfo = new SlaveInfo();
                slaveInfo.Path = inspectorInfo.Path;

                slaveList.Add(new SlaveObj(slaveInfo));
            }

            //Initialize();
            //Start();
        }

        public override void Initialize()
        {
            base.Initialize();
            //Start();
        }
    }
}

using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanM.EDMS.Operation;
//using UniEye.Base.Inspect;

namespace UniScanM.EDMS
{
    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new InspectRunner();
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new EDMS.Operation.InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            //dataExporterList.Add(new UniScanM.Data.InspectionResultDataExporter());
            dataExporterList.Add(new MachineIF.MachineIfDataExporter());
            dataExporterList.Add(new Data.ReportDataExporter());
        }

        public override void LoadAdditialSettings()
        {
            EDMS.Settings.EDMSSettings.CreateInstance();
            EDMS.Settings.EDMSSettings.Instance().Load();

            inspectStarter = new PLCInspectStarter();
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }
    }
}

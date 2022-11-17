using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniScanM.EDMS.Operation
{
    public class InspectRunnerExtender: UniScanM.Operation.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            return new EDMS.Data.InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as UniScanM.Data.Production;
            return production == null ? "0" : (production.LastInspectionNo + 1).ToString();
        }
    }
}

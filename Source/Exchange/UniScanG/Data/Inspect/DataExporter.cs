using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using DynMvp.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Data;
using UniScan.Common.Data;
using DynMvp.Base;

namespace UniScanG.Data.Inspect
{
    public class InspectorDataExporterG : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            FileHelper.ClearFolder(inspectionResult.ResultPath);
            foreach(KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
            {
                string key = pair.Key;
                IExportable exportable = pair.Value as IExportable;
                if (exportable != null)
                    exportable.Export(inspectionResult.ResultPath, cancellationToken);
            }
        }
    }

    internal class MonitorDataExporterG : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            FileHelper.ClearFolder(inspectionResult.ResultPath);
            foreach (KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
            {
                string key = pair.Key;
                IExportable exportable = pair.Value as IExportable;
                exportable.Export(inspectionResult.ResultPath, cancellationToken);
            }
        }
    }
}

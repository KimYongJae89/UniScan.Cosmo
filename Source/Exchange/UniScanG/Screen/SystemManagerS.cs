// custom
using UniScanG.UI;
using UniScanG.Data;
using UniEye.Base.Settings.UI;
using UniScan.Common.Settings;
using UniScan.Common.Settings.UI;
using UniScan.Common;
using UniEye.Base.Inspect;
using UniScanG.Screen.Inspect;
using UniEye.Base.Settings;
using DynMvp.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.FiducialFinder;
using UniEye.Base.UI;

namespace UniScanG.Screen
{
    public class MonitorSystemManagerS : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerMonitorS();
        }

        public override void InitializeDataExporter()
        {
            DataSetting.Instance().Load();
            dataExporterList.Add(new UniScanG.Data.Inspect.MonitorDataExporterG());
        }
    }

    public class InspectorSystemManagerS : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(FiducialFinderS.TypeName, ImagingLibrary.MatroxMIL, ""));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetInspector.TypeName, ImagingLibrary.MatroxMIL, ""));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetTrainer.TypeName, ImagingLibrary.MatroxMIL, ""));

            AlgorithmSetting.Instance().Load();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

            AlgorithmBuilder.SetAlgorithmEnabled(FiducialFinderS.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(SheetInspector.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(SheetTrainer.TypeName, true);
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerInspectorS();            
        }

        public override void InitializeDataExporter()
        {
            DataSetting.Instance().Load();

            dataExporterList.Add(new UniScanG.Data.Inspect.InspectorDataExporterG());
        }
    }
}

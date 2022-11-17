// custom
using UniScanG.UI;
using UniScanG.Data;
using UniEye.Base.Settings.UI;
using UniScan.Common.Settings;
using UniScan.Common.Settings.UI;
using UniScan.Common;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using DynMvp.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniEye.Base.UI;

namespace UniScanG.Gravure
{
    public class MonitorSystemManagerG : SystemManager
    {
        public MonitorSystemManagerG():base()
        {
        }

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
            return new InspectRunnerMonitorG();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanG.Data.Inspect.MonitorDataExporterG());
        }

        public override void LoadAdditialSettings()
        {
            UniScanG.Gravure.Settings.AdditionalSettings.CreateInstance();
            AdditionalSettings.Instance().Load();
        }
    }

    public class InspectorSystemManagerG : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

            ImagingLibrary imagingLibrary = OperationSettings.Instance().ImagingLibrary;
            bool useCuda = OperationSettings.Instance().UseCuda;
            switch (imagingLibrary)
            {
                case ImagingLibrary.MatroxMIL:
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetFinderBase.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Grey));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Trainer.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Grey));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Detector.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Grey));
                    if (useCuda)
                        AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Gpu));
                    else
                        AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.MatroxMIL, "", ImageType.Grey));
                    break;
                case ImagingLibrary.OpenCv:
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetFinderBase.TypeName, ImagingLibrary.OpenCv, "", ImageType.Grey));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Trainer.TypeName, ImagingLibrary.OpenCv, "", ImageType.Grey));
                    AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Detector.TypeName, ImagingLibrary.OpenCv, "", ImageType.Grey));
                    if (useCuda)
                        AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.OpenCv, "", ImageType.Gpu));
                    else
                        AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, ImagingLibrary.OpenCv, "", ImageType.Grey));
                    break;
            }
            //AlgorithmSetting.Instance().Load();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

            AlgorithmBuilder.SetAlgorithmEnabled(SheetFinderBase.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(Trainer.TypeName, true);
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerInspectorG();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanG.Data.Inspect.InspectorDataExporterG());
        }

        public override void LoadAdditialSettings()
        {
            UniScanG.Gravure.Settings.AdditionalSettings.CreateInstance();
            AdditionalSettings.Instance().Load();
        }
    }
}

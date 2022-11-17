using System;
using System.Collections.Generic;
using DynMvp.Vision;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.Watcher;

namespace UniScanG.Gravure
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<DynMvp.Vision.Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>() { CreateAlgorithm(SheetFinderBase.TypeName), CreateAlgorithm(Trainer.TypeName), CreateAlgorithm(CalculatorBase.TypeName), CreateAlgorithm(Detector.TypeName) , CreateAlgorithm(Watcher.TypeName) };
        }

        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == SheetFinderBase.TypeName)
            {
                Vision.SheetFinder.SheetBase.SheetFinderV2 sheetFinderV2 = new Vision.SheetFinder.SheetBase.SheetFinderV2();
                //Vision.SheetFinder.SheetBase.SheetFinderV2Param sheetFinderV2Param = sheetFinderV2?.Param as Vision.SheetFinder.SheetBase.SheetFinderV2Param;
                return sheetFinderV2;
            }

            if (algorithmType == Trainer.TypeName)
                return new Trainer();

            if (algorithmType == CalculatorBase.TypeName)
                return GetCalculator(AlgorithmSetting.Instance().CalculatorVersion);


            if (algorithmType == Detector.TypeName)
                return new Detector();

            if (algorithmType == Watcher.TypeName)
                return new Watcher();

            return base.CreateAlgorithm(algorithmType);
        }

        private Algorithm GetCalculator(CalculatorBase.Version calculatorVersion)
        {
            switch (calculatorVersion)
            {
                case CalculatorBase.Version.V1:
                    return new CalculatorV1();
                case CalculatorBase.Version.V2:
                    return new CalculatorV2();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.FiducialFinder;
using UniScanG.Screen.Vision.Trainer;

namespace UniScanG.Screen
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<DynMvp.Vision.Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>() { CreateAlgorithm(SheetInspector.TypeName), CreateAlgorithm(FiducialFinderS.TypeName) , CreateAlgorithm(SheetTrainer.TypeName) };
        }

        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == SheetInspector.TypeName)
                return new SheetInspector();

            if (algorithmType == FiducialFinderS.TypeName)
                return new FiducialFinderS();

            if (algorithmType == SheetTrainer.TypeName)
                return new SheetTrainer();

            return base.CreateAlgorithm(algorithmType);
        }
    }
}

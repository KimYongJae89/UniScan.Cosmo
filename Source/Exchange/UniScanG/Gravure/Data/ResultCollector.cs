using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace UniScanG.Gravure.Data
{
    public class ResultCollector : UniScanG.Data.ResultCollector
    {
        public SheetResult Collect(string path)
        {
            CalculatorResult cr = new CalculatorResult();
            cr.Import(path);

            DetectorResult dr = new DetectorResult();
            dr.Import(path);

            dr.SpandTime += cr.SpandTime;
            return dr;
        }

        public SheetResult CreateSheetResult()
        {
            return new DetectorResult();
        }
    }
}

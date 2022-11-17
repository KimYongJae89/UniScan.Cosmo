using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;

namespace UniScanG.Screen.Data
{
    public class ResultCollector : UniScanG.Data.ResultCollector
    {
        public SheetResult Collect(string path)
        {
            SheetResult sr = new UniScanG.Screen.Data.ScreenResult();
            sr.Import(path);
            return sr;
        }

        public SheetResult CreateSheetResult()
        {
            throw new NotImplementedException();
        }
    }
}

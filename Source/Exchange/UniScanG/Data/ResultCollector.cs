using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Data
{
    public interface ResultCollector
    {
        SheetResult Collect(string path);
        SheetResult CreateSheetResult();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;
using UniScanG.Inspect;

namespace UniScanG.Gravure.Inspect
{
    public abstract class GrabProcesserG : GrabProcesser
    {
        public abstract SheetImageSet GetLastSheetImageSet();
    }
}

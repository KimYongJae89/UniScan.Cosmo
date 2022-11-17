using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScan.Common.Data
{
    public interface IExportable
    {
        void Export(string path, CancellationToken cancellationToken);
    }
}

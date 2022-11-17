using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.InspData
{
    public class DbInspResultArchiver : InspResultArchiver
    {
        public void GetProbeResult(InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public List<InspectionResult> Load(string dataPath, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void Save(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

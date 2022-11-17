using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;

namespace UniEye.Base.Inspect
{
    public interface IInspectProcesser
    {
        ProcessTask Process(AlgoImage algoImage, InspectionResult inspectionResult, InspectionOption inspectionOption = null);
        bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1);
        void CancelProcess(ProcessTask inspectionTask=null);
    }
}

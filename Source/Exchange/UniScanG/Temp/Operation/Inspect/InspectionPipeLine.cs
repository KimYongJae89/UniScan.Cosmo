using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SamsungElectro.Operation.Inspect
{
    class InspectionPipeLine : ThreadHandler
    {
        ManualResetEvent isRunning = new ManualResetEvent(false);
        DynMvp.Vision.Algorithm algorithm = null;

        Image2D inspImage;
        InspectionResult inspectionResult;

        public TargetGroupInspectedDelegate PipeLineInspected = null;
        Task pipeLineTask = null;

        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool IsBusy
        {
            get { return isRunning.WaitOne(0); }
        }

        public AlgorithmParam Param
        {
            get { return algorithm.Param; }
        }

        public InspectionPipeLine(int id)
        {
            this.id = id;
        }

        public void Initialize(DynMvp.Vision.Algorithm algorithm, AlgorithmParam param)
        {
            this.algorithm = algorithm;
            this.algorithm.Param = param;
            isRunning.Reset();
            inspectionResult = null;
        }

        public void Start(Image2D inspImage, InspectionResult inspectionResult)
        {
            isRunning.Set();
            this.inspImage = inspImage;
            this.inspectionResult = inspectionResult;

            pipeLineTask = new Task(TaskProc);
            pipeLineTask.Start();
        }

        private void TaskProc()
        {
            try
            {
                SetThreadAffinity(this.id + 1);
                LogHelper.Debug(LoggerType.Inspection, string.Format("InspectionPipeLine::TaskProc Start. ID:{0}", this.id));

                DebugContext debugContext = new DebugContext();
                debugContext.SaveDebugImage = SamsungElectroSettings.Instance().IsDebug;
                debugContext.Path = Path.Combine(Configuration.TempFolder, string.Format("PipeLine{0}", this.id.ToString()));

                DynMvp.UI.RotatedRect rotatedRect = new DynMvp.UI.RotatedRect(0, 0, 0, 0, 0);
                if (inspImage != null)
                    rotatedRect = new DynMvp.UI.RotatedRect(0, 0, inspImage.Width, inspImage.Height, 0);

                debugContext.TimeProfile.StartCategory("PipeLine");

                AlgorithmInspectParam param = new AlgorithmInspectParam(inspImage, rotatedRect, rotatedRect, rotatedRect.ToRectangle().Size, null, debugContext);

                AlgorithmResult algorithmResult = algorithm.Inspect(param);
                debugContext.TimeProfile.Add("PipeLine", "Inspect");

                if (algorithmResult != null)
                {
                    algorithm.BuildMessage(algorithmResult);

                    ProbeResult visionProbeResult = new VisionProbeResult(null, algorithmResult, inspImage);
                    visionProbeResult.RegionInFov = rotatedRect;
                    visionProbeResult.RegionInProbe = rotatedRect;
                    inspectionResult.AddProbeResult(visionProbeResult);
                    visionProbeResult.Judgment = visionProbeResult.Judgment;

                    debugContext.TimeProfile.StopCategory("PipeLine");
                    debugContext.ExportTimeProfile();
                }

                if (this.PipeLineInspected != null)
                    PipeLineInspected(null, inspectionResult, null);

                LogHelper.Debug(LoggerType.Inspection, string.Format("InspectionPipeLine::TaskProc End. ID:{0}", this.id));
                inspectionResult = null;
            }
            finally
            {
                isRunning.Reset();
            }
        }

        public void Dispose()
        {
            isRunning.Dispose();
            algorithm.Clear();
            if(inspImage!=null)
                inspImage.Dispose();
            inspImage = null;
            if (pipeLineTask!=null)
                pipeLineTask.Dispose();
            pipeLineTask = null;
        }
    }
}


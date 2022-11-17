using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using DynMvp.Vision;

namespace UniEye.Base.Inspect
{
    /// <summary>
    /// 검사 객체 관리 안 함. None-Overlap(검사 중 검사요청 무시)
    /// </summary>
    public class DirectTriggerInspectRunner : InspectRunner
    {
        public IInspectProcesser InspectProcesser
        {
            get { return inspectProcesser; }
            set { inspectProcesser = value; }
        }

        protected IInspectProcesser inspectProcesser = null;
        
        public DirectTriggerInspectRunner() : base()
        {
        }

        //public override void PreExitWaitInspection()
        
        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            AlgoImage algoImage = null;
            try
            {
                ImageD imageD = imageDevice.GetGrabbedImage(ptr);
                algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                Debug.Assert(algoImage != null);

                inspectRunnerExtender.OnPreInspection();

                this.PreInspect();
                this.inspectProcesser.Process(algoImage, inspectionResult, inspectionOption);
                this.PostInspect();

                inspectRunnerExtender.OnPostInspection();

                algoImage.Dispose();

                ProductInspected(inspectionResult);
            }
            finally
            {
                algoImage?.Dispose();
            }
        }
    }
}

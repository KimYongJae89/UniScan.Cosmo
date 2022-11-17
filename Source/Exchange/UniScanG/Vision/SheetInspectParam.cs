using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Data.Vision;
using UniScanG.Inspect;

namespace UniScanG.Vision
{
    public class SheetInspectParam : DynMvp.Vision.AlgorithmInspectParam
    {
        public AlgoImage AlgoImage
        {
            get { return algoImage; }
            set { algoImage = value; }
        }
        
        public bool TestInspect
        {
            get { return testInspect; }
            set { testInspect = value; }
        }

        public RegionInfo RegionInfo
        {
            get { return regionInfo; }
            set { regionInfo = value; }
        }

        public ProcessBufferSet ProcessBufferSet
        {
            get { return processBufferSet; }
            set { processBufferSet = value; }
        }

        public SizeF FidOffset
        {
            get { return fidOffset; }
            set { fidOffset = value; }
        }

        private AlgoImage algoImage = null;
        private bool testInspect = false;
        private RegionInfo regionInfo = null;
        private ProcessBufferSet processBufferSet = null;
        private SizeF fidOffset = SizeF.Empty;

        
        public SheetInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext) : base(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext)
        {
        }

        public override void Dispose()
        {
            base.Dispose();

            algoImage.Dispose();
        }
    }
}

using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScan.Common.Settings;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.Screen.Vision
{
    public class ProcessBufferSetS : UniScanG.Inspect.ProcessBufferSet
    {
        //Alloc
        AlgoImage fiducial;
        public AlgoImage Fiducial
        {
            get { return fiducial; }
            set { fiducial = value; }
        }

        //Preview
        AlgoImage interestP;
        public AlgoImage InterestP
        {
            get { return interestP; }
            set { interestP = value; }
        }


        AlgoImage poleMask;
        public AlgoImage PoleMask
        {
            get { return poleMask; }
            set { poleMask = value; }
        }

        AlgoImage poleInspect;
        public AlgoImage PoleInspect
        {
            get { return poleInspect; }
            set { poleInspect = value; }
        }

        AlgoImage dielectricMask;
        public AlgoImage DielectricMask
        {
            get { return dielectricMask; }
            set { dielectricMask = value; }
        }

        AlgoImage dielectricInspect;
        public AlgoImage DielectricInspect
        {
            get { return dielectricInspect; }
            set { dielectricInspect = value; }
        }
        
        AlgoImage interestBin;
        public AlgoImage InterestBin
        {
            get { return interestBin; }
            set { interestBin = value; }
        }

        AlgoImage mask;
        public AlgoImage Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public ProcessBufferSetS(int width, int height) : base("",width, height)
        {
        }

        public ProcessBufferSetS(string algorithmTypeName, int width, int height) : base(algorithmTypeName,width, height)
        {
        }

        public override void BuildBuffers()
        {
            if (string.IsNullOrEmpty(algorithmTypeName))
                algorithmTypeName = SheetInspector.TypeName;

            fiducial = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height);
            bufferList.Add(fiducial);
            float resizeReatio = SystemTypeSettings.Instance().ResizeRatio;
            int previewWidth = (int)Math.Truncate(width * resizeReatio);
            int previewHeight = (int)Math.Truncate(height * resizeReatio);

            interestP = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, previewWidth, previewHeight);
            bufferList.Add(interestP);

            AllocImage(algorithmTypeName, width, height);
        }

        private void AllocImage(string algorithmTypeName, int width, int height)
        {
            bufferList.Add(poleMask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(poleInspect = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(dielectricMask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(dielectricInspect = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(interestBin = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
            bufferList.Add(mask = ImageBuilder.Build(algorithmTypeName, ImageType.Grey, width, height));
        }
    }
}

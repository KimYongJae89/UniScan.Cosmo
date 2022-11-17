using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Inspect
{
    public class FrameGrabProcesser : GrabProcesserG
    {
        SheetImageSet sheetImageSet = null;
        List<AlgoImage> motherAlgoImage = new List<AlgoImage>();
        int remainLengthPx = -1;
        public int RemainLengthPx { get => remainLengthPx; set => remainLengthPx = value; }

        public bool IsDone
        {
            get { return remainLengthPx == 0; }
        }

        public FrameGrabProcesser(int lengthPx)
        {
            this.remainLengthPx = lengthPx;
        }

        public override void Dispose()
        {
            sheetImageSet?.Dispose();
            sheetImageSet = null;
            this.motherAlgoImage.ForEach(f => f.Dispose());
            this.motherAlgoImage.Clear();
        }

        public override IntPtr GetGrabbedImagePtr()
        {
            throw new NotImplementedException();
        }

        public override SheetImageSet GetLastSheetImageSet()
        {
            return sheetImageSet;
        }

        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD imageD = (ImageD)imageDevice.GetGrabbedImage(ptr);
            imageD.ConvertFromData();   // 이거로 DataPtr을 정해주면 Mil에서 Alloc하지 않고 Pointer로 참조해감. (가상카메라에서 더 빨라짐)
            AlgoImage algoImage = ImageBuilder.Build(Vision.SheetFinder.SheetFinderBase.TypeName, imageD, ImageType.Grey);
            this.motherAlgoImage.Add(algoImage);

            Rectangle rectangle = new Rectangle(0, 0, algoImage.Width, Math.Min(remainLengthPx, algoImage.Height));
            if (rectangle.Width > 0 && rectangle.Height > 0)
            {
                AlgoImage subImage = algoImage.GetSubImage(rectangle);
                if (sheetImageSet == null)
                    sheetImageSet = new SheetImageSet(0);
                sheetImageSet.AddSubImage(subImage);
                remainLengthPx -= subImage.Height;
            }
        }

        public override void Start() { }

        public override void Stop() { }
    }
}

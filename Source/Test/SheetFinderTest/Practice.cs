using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Gravure.Inspect;
using System.Drawing;
using System.Threading;
using UniScanG.Gravure.Data;
using DynMvp.Device.Device.FrameGrabber;

namespace SheetFinderTest
{
    class Practice
    {
        public PracticeGrabbedDelegate FrameGrabbed = null;
        public PracticeGrabbedDelegate SheetFound = null;

        AlgoImage baseFrame = null;
        List<AlgoImage> frameList = null;
        int frameHeight;

        SheetGrabProcesserG grabProcesser = null;
        ThreadHandler workingThreadHandler = null;

        public void Start(ImageD templateImageD, SheetFinderV2Param param, int boundSize, int frameHeight)
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                baseFrame = ImageBuilder.Build(SheetFinderV2.TypeName, templateImageD, ImageType.Grey);
            });
            frameList = new List<AlgoImage>();

            grabProcesser = new SheetGrabProcesserG();
            grabProcesser.Initialize(param);

            this.frameHeight = frameHeight;
            SheetFinderV2 sheetFinderV2 = new SheetFinderV2();
            if (boundSize >= 0)
                sheetFinderV2.BoundSize = boundSize;
            grabProcesser.Algorithm = sheetFinderV2;
            grabProcesser.StartInspectionDelegate = grabProcesser_StartInspectionDelegate;
            grabProcesser.SetTestMode(true, @"d:\temp\Practice");
            grabProcesser.Start();

            workingThreadHandler = new ThreadHandler("WorkingThreadHandler", new System.Threading.Thread(ThreadProc), false);
            workingThreadHandler.Start();
        }

        private void grabProcesser_StartInspectionDelegate(DynMvp.Devices.ImageDevice imageDevice, IntPtr ptr)
        {
            SheetImageSet sheetImageSet = grabProcesser.GetLastSheetImageSet();
            ImageD imageD = sheetImageSet.ToImageD(0.1f);
            List<int> frameIds = new List<int>(sheetImageSet.PartImageList.ConvertAll<int>(f => (int)(f.ParentImage.Tag as CameraBufferTag)?.FrameId));
            SheetFound?.Invoke(imageD, sheetImageSet.Size, sheetImageSet.SheetNo, frameIds);
            sheetImageSet.Dispose();
        }

        public void Stop()
        {
            if (workingThreadHandler == null)
                return;

            workingThreadHandler.Stop();

            baseFrame.Dispose();
            grabProcesser.Dispose();

            baseFrame = null;
            grabProcesser = null;
        }

        private void ThreadProc()
        {
            Size frameSize = new Size(baseFrame.Width, frameHeight);

            ulong count = 0;
            while (workingThreadHandler.RequestStop == false)
            {
                Thread.Sleep(100);
                if (grabProcesser.IsBusy)
                    continue;

                AlgoImage frame = GetNextFrame(frameSize);
                frame.Tag = new CameraBufferTag((int)(count % 10), count);
                FrameGrabbed?.Invoke(frame.ToImageD(), frame.Size, (int)count, null);
                AddGrabProcesser(frame);
                count++;
            }
        }

        private void AddGrabProcesser(AlgoImage frame)
        {
            grabProcesser.ImageGrabbed(frame);
            frameList.Add(frame);
            while(frameList.Count>4)
            {
                frameList.First().Dispose();
                frameList.RemoveAt(0);
            }
        }

        int src = 0;
        int dst = 0;
        private AlgoImage GetNextFrame(Size frameSize)
        {
            Rectangle baseFrameRect = new Rectangle(Point.Empty, this.baseFrame.Size);

            Rectangle srcRect = new Rectangle(Point.Empty, frameSize);
            srcRect.Offset(0, src);

            Point dstPoint = Point.Empty;

            AlgoImage nextFrame = ImageBuilder.Build(this.baseFrame.LibraryType, this.baseFrame.ImageType, frameSize);
            while (srcRect.Height > 0)
            {
                Rectangle srcIntersectRect = Rectangle.Intersect(srcRect, baseFrameRect);
                nextFrame.Copy(this.baseFrame, srcIntersectRect.Location, dstPoint, srcIntersectRect.Size);

                dstPoint.Y += srcIntersectRect.Height;
                srcRect.Height -= srcIntersectRect.Height;
                srcRect.Y = 0;
            }
            src = (src + frameSize.Height) % this.baseFrame.Height;

            return nextFrame;
        }
    }
}

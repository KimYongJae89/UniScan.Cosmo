using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision
{
    public class AlgorithmCommon
    {
        Thread disposeThread;
        AutoResetEvent resetEvent;
        List<BlobRectList> disposeList;

        static AlgorithmCommon _instance;
        public static AlgorithmCommon Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmCommon();

            return _instance;
        }

        public AlgorithmCommon()
        {
            resetEvent = new AutoResetEvent(false);
            disposeList = new List<BlobRectList>();

            disposeThread = new Thread(Dispose);
            disposeThread.Start();
        }
        
        private void Dispose()
        {
            while (true)
            {
                if (disposeList.Count == 0)
                    resetEvent.WaitOne();

                lock (disposeList)
                {
                    if (disposeList.Count == 0)
                        continue;

                    BlobRectList blobRectList = disposeList.First();
                    disposeList.Remove(blobRectList);
                    blobRectList.Dispose();
                }
                          
            }
        }

        public void AddDisposeList(BlobRectList blobRectList)
        {
            lock (disposeList)
            {
                disposeList.Add(blobRectList);
                resetEvent.Set();
            }
        }

        public void CreateMaskImage(AlgoImage bin, AlgoImage mask, int minPatternArea, bool fillHole)
        {
            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);
            
            BlobParam patternBlobParam = new BlobParam();
            patternBlobParam.SelectBoundingRect = false;
            patternBlobParam.SelectLabelValue = true;
            patternBlobParam.AreaMin = minPatternArea;

            BlobRectList patternBlobRectList = imageProcessing.Blob(bin, patternBlobParam);
            List<BlobRect> blobRectList = patternBlobRectList.GetList();
            
            DrawBlobOption drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;

            mask.Clear();
            imageProcessing.DrawBlob(mask, patternBlobRectList, null, drawBlobOption);
            AddDisposeList(patternBlobRectList);

            if (fillHole == true)
                imageProcessing.FillHoles(mask, mask);
        }

        public void RefineSubResult(AlgoImage interest, AlgoImage binImage, Rectangle srcRect, BlobRect blobRect, Rectangle region, ref Screen.Data.SheetSubResult subResult)
        {
            Rectangle inflateRect = Rectangle.Truncate(blobRect.BoundingRect);
            inflateRect.Inflate(50, 50);
            inflateRect.Intersect(srcRect);

            AlgoImage subInterest = interest.GetSubImage(inflateRect);
            subResult.Image = subInterest.ToImageD().ToBitmap();
            subInterest.Dispose();

            if (binImage != null)
            {
                AlgoImage subInspect = binImage.GetSubImage(inflateRect);
                subResult.BinaryImage = subInspect.ToImageD().ToBitmap();
                subInspect.Dispose();
            }

            Rectangle offsetRect = Rectangle.Truncate(blobRect.BoundingRect);
            offsetRect.Offset(region.X, region.Y);
            
            subResult.Region = offsetRect;
            
            //subResult.RotatedRect = new DynMvp.UI.RotatedRect(blobRect.RotateRect.X + region.X, blobRect.RotateRect.Y + region.Y, blobRect.RotateRect.Width, blobRect.RotateRect.Height, blobRect.RotateRect.Angle);

            subResult.RealCenterPos = new PointF(subResult.Region.X * AlgorithmSetting.Instance().XPixelCal,
                subResult.Region.Y * AlgorithmSetting.Instance().YPixelCal);
        }

        public bool IsNecessaryDefect(BlobRect blobRect, List<BlobRect> notNecessaryList)
        {
            foreach (BlobRect NecessaryBlob in notNecessaryList)
            {
                if (NecessaryBlob.BoundingRect.Contains(blobRect.CenterPt) == true)
                    return true;
            }

            return false;
        }
        
        public  void MergeBlobs(int inflate, ref List<BlobRect> blobRectList)
        {
            bool merged = true;

            int tryNum = 0;
            while (merged == true)
            {
                merged = false;

                if (tryNum % 2 == 0)
                    blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X).ToList();
                else
                    blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.Y).ToList();

                for (int srcIndex = 0; srcIndex < blobRectList.Count; srcIndex++)
                {
                    BlobRect srcBlob = blobRectList[srcIndex];

                    int endSearchIndex = srcIndex + 1;

                    if (tryNum % 2 == 0)
                    {
                        for (int i = endSearchIndex; i < blobRectList.Count; i++)
                        {
                            if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }
                    else
                    {
                        for (int i = endSearchIndex; i < blobRectList.Count; i++)
                        {
                            if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }

                    RectangleF inflateRect = srcBlob.BoundingRect;
                    inflateRect.Inflate(inflate, inflate);

                    for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex && destIndex < blobRectList.Count; destIndex++)
                    {
                        BlobRect destBlob = blobRectList[destIndex];

                        if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
                        {
                            srcBlob = srcBlob + destBlob;
                            blobRectList[srcIndex] = srcBlob;

                            blobRectList.RemoveAt(destIndex);

                            endSearchIndex--;
                            destIndex--;

                            if (tryNum % 2 == 0)
                            {
                                for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
                                {
                                    if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
                                {
                                    if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }

                            if (merged == false)
                                merged = true;

                            inflateRect = srcBlob.BoundingRect;
                            inflateRect.Inflate(inflate, inflate);
                        }
                    }
                }

                if (merged == true)
                    tryNum++;
            }

            blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X + defect.BoundingRect.Y).ToList();
        }

        public void RemoveIntersectBlobs(ref List<BlobRect> blobRectList)
        {
            for (int srcIndex = 0; srcIndex < blobRectList.Count; srcIndex++)
            {
                BlobRect srcBlob = blobRectList[srcIndex];
                
                for (int destIndex = srcIndex + 1; destIndex < blobRectList.Count; destIndex++)
                {
                    BlobRect destBlob = blobRectList[destIndex];

                        
                    if (srcBlob.BoundingRect.IntersectsWith(destBlob.BoundingRect) == true)
                    {
                        if (srcBlob.Area >= destBlob.Area)
                        {
                            blobRectList.RemoveAt(destIndex);
                            destIndex--;
                        }
                        else
                        {
                            blobRectList.RemoveAt(srcIndex);
                            srcIndex--;
                            break;
                        }
                    }
                }
            }
        }
    }
}

using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Screen.Data;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision
{
    public class AlgorithmCommon
    {
        /// <summary>
        /// High-Hill 영역을 찾음
        /// </summary>
        /// <param name="data"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Point> FindHill(float[] data, float threshold, bool inverse = false, DebugContext debugContext = null)
        {
            if (threshold < 0)
                threshold = data.Average() * 1.0f;

            int[] foundeds = new int[data.Length];

            List<Point> hillList = new List<Point>();
            Point hill = new Point(-1, -1);
            bool onStart = false;   // 시작부터 Hill이면 무시
            for (int i = 0; i < data.Length; i++)
            {
                bool isHill = inverse ? data[i] <= threshold : data[i] >= threshold;
                if (isHill)
                {
                    if (onStart && hill.X < 0)
                        hill.X = i;
                }
                else
                {
                    onStart = true;
                    if (hill.X >= 0)
                    {
                        hill.Y = i;
                        for (int j = hill.X; j < hill.Y; j++)
                            foundeds[j] = 255 / (hillList.Count % 2 + 1);

                        hillList.Add(hill);
                        hill = new Point(-1, -1);
                    }
                }
            }

            if (debugContext != null && debugContext.SaveDebugImage)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Data,Founded");
                for (int i = 0; i < data.Length; i++)
                    sb.AppendLine(string.Format("{0},{1}", data[i], foundeds[i]));

                Directory.CreateDirectory(debugContext.FullPath);
                File.WriteAllText(Path.Combine(debugContext.FullPath, "FindHill.txt"), sb.ToString());
            }

            return hillList;
        }

        /// <summary>
        /// 변화가 큰 영역을 찾음
        /// </summary>
        /// <param name="data"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Point> FindHill2(float[] data, float threshold, DebugContext debugContext=null)
        {
            int searchLength = data.Length - 1;
            float[] absDiff = new float[searchLength];
            int[] foundeds = new int[absDiff.Length];
            
            List<Point> hillList = new List<Point>();
            Point hill = new Point(-1, -1);
            bool isHigh = false;
            for (int i = 0; i < searchLength; i++)
            {
                absDiff[i] = Math.Abs(data[i + 1] - data[i]);
                bool high = absDiff[i] < threshold;
                //if (isHigh == false && high == true)
                if (isHigh != high)
                {
                    if (hill.X < 0)
                    {
                        hill.X = i + 1;
                    }
                    else
                    {
                        hill.Y = i;
                        for (int j = hill.X; j < hill.Y; j++)
                            foundeds[j] = 255;

                        hillList.Add(hill);
                        hill = new Point(-1, -1);
                    }
                }
                isHigh = high;
            }

            if (hill.X >= 0)
            {
                hill.Y = searchLength - 1;
                for (int j = hill.X; j < hill.Y; j++)
                    foundeds[j] = 255;

                hillList.Add(hill);
                hill = new Point(-1, -1);
            }

            if (debugContext!=null && debugContext.SaveDebugImage)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Data,Diff,Founded");
                for (int i = 0; i < searchLength; i++)
                    sb.AppendLine(string.Format("{0},{1},{2}", data[i], absDiff[i], foundeds[i]));
                for (int i = searchLength; i < data.Length; i++)
                    sb.AppendLine(string.Format("{0}", data[i]));

                Directory.CreateDirectory(debugContext.FullPath);
                File.WriteAllText(Path.Combine(debugContext.FullPath, "FindHill2.txt"), sb.ToString());
            }

            return hillList;
        }

        /// <summary>
        /// 커널 사용
        /// </summary>
        /// <param name="data"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Point> FindHill3(float[] data, int kernalSize, float minimumValue, float diffThreshold, DebugContext debugContext=null)
        {
            float[] kernal = new float[kernalSize];
            int searchLength = data.Length - kernalSize;

            List<Point> hillList = new List<Point>();
            Point hill = new Point(-1, -1);
            float[] diffs = new float[searchLength];
            int[] foundeds = new int[searchLength];
            
            for (int i = 0; i < searchLength; i++)
            {
                Array.Copy(data, i, kernal, 0, kernalSize);
                float min = kernal.Min();
                float max = kernal.Max();
                float diff = diffs[i] = max - min;

                if (min > minimumValue && diff < diffThreshold)
                {
                    if (hill.X < 0)
                        hill.X = i;
                }
                else
                {
                    if (hill.X >= 0)
                    {
                        hill.Y = i + kernalSize - 1;
                            //for (int j = i; j < hill.Y; j++)
                            //    foundeds[j] = 255 / (hillList.Count % 2 + 1);
                        hillList.Add(hill);
                    }
                    hill = new Point(-1, -1);
                }
            }

            if (debugContext != null && debugContext.SaveDebugImage)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Data,Diff,Founded");
                //for (int i = 0; i < searchLength; i++)
                //    sb.AppendLine(string.Format("{0},{1},{2}", data[i], diffs[i], foundeds[i]));
                for (int i = searchLength; i < data.Length; i++)
                    sb.AppendLine(string.Format("{0}", data[i]));

                Directory.CreateDirectory(debugContext.FullPath);
                File.WriteAllText(Path.Combine(debugContext.FullPath, "FindHill3.txt"), sb.ToString());
            }
            return hillList;
        }

        /// <summary>
        /// 상관함수
        /// </summary>
        /// <param name="firstData"></param>
        /// <param name="secondData"></param>
        /// <returns></returns>
        public static float Correlation(float[] firstData, float[] secondData)
        {
            // Normalize
            //Normalize(firstData);
            //Normalize(secondData);

            List<Tuple<int, float>> scores = new List<Tuple<int, float>>();
            for (int tau = -(secondData.Length - 1); tau < firstData.Length; tau++)
            {
                int src = Math.Max(0, tau);
                int dst = Math.Min(firstData.Length, tau + secondData.Length);
                float score = 0;
                for (int i = src; i < dst; i++)
                {
                    float a = firstData[i];
                    float b = secondData[i - tau];
                    //score += Math.Min(a, b);
                    score += a * b;
                }

                scores.Add(new Tuple<int, float>(tau, score));
            }

            int debugLoop = Math.Max(firstData.Length, Math.Max(secondData.Length, scores.Count));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < debugLoop; i++)
            {
                sb.Append(string.Format("{0},", i < firstData.Length ? firstData[i].ToString() : ""));
                sb.Append(string.Format("{0},", i < secondData.Length ? secondData[i].ToString() : ""));
                sb.Append(string.Format("{0},", i < scores.Count ? scores.ElementAt(i).Item2.ToString() : ""));
                sb.Append(string.Format("{0},", i < scores.Count ? scores.ElementAt(i).Item1.ToString() : ""));
                sb.AppendLine();
            }
            //File.WriteAllText(@"d:\temp\Correlation.txt", sb.ToString());
            //Debug.WriteLine(sb.ToString());

            float maxScore = scores.Max(f => f.Item2);
            List<Tuple<int, float>> maxScores = scores.FindAll(f => f.Item2 == maxScore);
            float average = (float)Math.Round(maxScores.Average(f => f.Item1));
            return average;
            //List<Tuple<int, float>> sortedScores = scores.OrderByDescending(f => f.Item2).ToList();
            //return sortedScores.ElementAt(0).Item1;
        }

        private static void Normalize(float[] data)
        {
            float fMax = data.Max();
            float[] norData = new float[data.Length];
            int iteration = data.Length;
            for (int i = 0; i < iteration; i++)
            {
                data[i] = data[i] / fMax;
            }
        }

        public static void GetInspImage(AlgoImage srcAlgoImage, AlgoImage scaleBuffer, AlgoImage dstAlgoImage, float scaleFactor)
        {
            //srcAlgoImage.Save(@"C:\temp\srcAlgoImage.bmp");
            AlgoImage convertFrom = srcAlgoImage;
            if (scaleBuffer != null)
            {
                ScaleImage(srcAlgoImage, scaleBuffer, scaleFactor);
                //scaleBuffer.Save(@"C:\temp\scaleBuffer.bmp");
                convertFrom = scaleBuffer;
            }

            if (dstAlgoImage == null)
                return ;

            if (srcAlgoImage.IsCompatible(dstAlgoImage) == false)
            {
                //convertFrom.Save(@"C:\temp\convertFrom.bmp");
                DynMvp.Vision.ImageConverter.Convert(convertFrom, dstAlgoImage);
                //dstAlgoImage.Save(@"C:\temp\dstAlgoImage.bmp");
            }

            return ;
        }

        public static AlgoImage GetScaleImage(AlgoImage srcAlgoImage)
        {
            UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;

            if (curModel == null || curModel.ScaleFactor == 1)
                return srcAlgoImage.GetSubImage(new Rectangle(Point.Empty, srcAlgoImage.Size));

            Size newSize = new Size(srcAlgoImage.Width / curModel.ScaleFactor, srcAlgoImage.Height / curModel.ScaleFactor);
            AlgoImage dstAlgoImage = ImageBuilder.Build(srcAlgoImage.LibraryType, srcAlgoImage.ImageType, newSize);
            float scaleFactor = 1f / curModel.ScaleFactor;
            ScaleImage(srcAlgoImage, dstAlgoImage, scaleFactor);
            return dstAlgoImage;
        }

        public static void ScaleImage(AlgoImage srcAlgoImage, AlgoImage dstAlgoImage, float scaleF)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(dstAlgoImage);
            dstAlgoImage.Clear();
            if (srcAlgoImage is SheetImageSet)
            {
                Rectangle srcRect = Rectangle.Empty;
                Rectangle dstRect = Rectangle.Empty;
                SheetImageSet sheetImageSet = (SheetImageSet)srcAlgoImage;
                sheetImageSet.PartImageList.ForEach(f =>
                {
                    srcRect.Size = f.Size;
                    dstRect.Size = Size.Round(new SizeF(f.Width * scaleF, f.Height * scaleF));
                    AlgoImage subImage = dstAlgoImage.GetSubImage(dstRect);
                    ip.Resize(f, subImage);
                    subImage.Dispose();

                    srcRect.Y += srcRect.Height;
                    dstRect.Y += dstRect.Height;
                });
            }
            else
            {
                ip.Resize(srcAlgoImage, dstAlgoImage, scaleF);
            }
        }

        public static Point FindOffsetPosition(AlgoImage algoImage, Point basePosition)
        {
            Point diffPos = Point.Empty;
            SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            if (sheerFinder == null)
                return Point.Empty;

            int foundPosX = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 20);
            int foundPosY = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 20);
            if (foundPosX >= 0 && foundPosY >= 0)
                diffPos = new Point(foundPosX - basePosition.X, foundPosY - basePosition.Y);

            return diffPos;
        }

    }

    //public class AlgorithmCommon
    //{
    //    Thread disposeThread;
    //    AutoResetEvent resetEvent;
    //    List<BlobRectList> disposeList;

    //    static AlgorithmCommon _instance;
    //    public static AlgorithmCommon Instance()
    //    {
    //        if (_instance == null)
    //            _instance = new AlgorithmCommon();

    //        return _instance;
    //    }

    //    public AlgorithmCommon()
    //    {
    //        resetEvent = new AutoResetEvent(false);
    //        disposeList = new List<BlobRectList>();

    //        disposeThread = new Thread(Dispose);
    //        disposeThread.Start();
    //    }

    //    private void Dispose()
    //    {
    //        while (true)
    //        {
    //            if (disposeList.Count == 0)
    //                resetEvent.WaitOne();

    //            lock (disposeList)
    //            {
    //                if (disposeList.Count == 0)
    //                    continue;

    //                BlobRectList blobRectList = disposeList.First();
    //                disposeList.Remove(blobRectList);
    //                blobRectList.Dispose();
    //            }

    //        }
    //    }

    //    public void AddDisposeList(BlobRectList blobRectList)
    //    {
    //        lock (disposeList)
    //        {
    //            disposeList.Add(blobRectList);
    //            resetEvent.Set();
    //        }
    //    }

    //    public void CreateMaskImage(AlgoImage bin, AlgoImage mask, int minPatternArea, bool fillHole)
    //    {
    //        ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

    //        BlobParam patternBlobParam = new BlobParam();
    //        patternBlobParam.SelectBoundingRect = false;
    //        patternBlobParam.SelectLabelValue = true;
    //        patternBlobParam.AreaMin = minPatternArea;

    //        BlobRectList patternBlobRectList = imageProcessing.Blob(bin, patternBlobParam);
    //        List<BlobRect> blobRectList = patternBlobRectList.GetList();

    //        DrawBlobOption drawBlobOption = new DrawBlobOption();
    //        drawBlobOption.SelectBlob = true;

    //        mask.Clear();
    //        imageProcessing.DrawBlob(mask, patternBlobRectList, null, drawBlobOption);
    //        AddDisposeList(patternBlobRectList);

    //        if (fillHole == true)
    //            imageProcessing.FillHoles(mask, mask);
    //    }

    //    public void RefineSubResult(AlgoImage interest, AlgoImage binImage, Rectangle srcRect, BlobRect blobRect, Rectangle region, ref SheetSubResult subResult)
    //    {
    //        Rectangle inflateRect = Rectangle.Truncate(blobRect.BoundingRect);
    //        inflateRect.Inflate(50, 50);
    //        inflateRect.Intersect(srcRect);

    //        AlgoImage subInterest = interest.GetSubImage(inflateRect);
    //        subResult.Image = subInterest.ToImageD().ToBitmap();
    //        subInterest.Dispose();

    //        if (binImage != null)
    //        {
    //            AlgoImage subInspect = binImage.GetSubImage(inflateRect);
    //            subResult.BinaryImage = subInspect.ToImageD().ToBitmap();
    //            subInspect.Dispose();
    //        }

    //        Rectangle offsetRect = Rectangle.Truncate(blobRect.BoundingRect);
    //        offsetRect.Offset(region.X, region.Y);

    //        subResult.Region = offsetRect;

    //        subResult.RotatedRect = new DynMvp.UI.RotatedRect(blobRect.RotateRect.X + region.X, blobRect.RotateRect.Y + region.Y, blobRect.RotateRect.Width, blobRect.RotateRect.Height, blobRect.RotateRect.Angle);

    //        subResult.RealPos = new PointF(subResult.Region.X * AlgorithmSetting.Instance().XPixelCal,
    //            subResult.Region.Y * AlgorithmSetting.Instance().YPixelCal);
    //    }

    //    public bool IsNecessaryDefect(BlobRect blobRect, List<BlobRect> notNecessaryList)
    //    {
    //        foreach (BlobRect NecessaryBlob in notNecessaryList)
    //        {
    //            if (NecessaryBlob.BoundingRect.Contains(blobRect.CenterPt) == true)
    //                return true;
    //        }

    //        return false;
    //    }

    //    public  void MergeBlobs(int inflate, ref List<BlobRect> blobRectList)
    //    {
    //        bool merged = true;

    //        int tryNum = 0;
    //        while (merged == true)
    //        {
    //            merged = false;

    //            if (tryNum % 2 == 0)
    //                blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X).ToList();
    //            else
    //                blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.Y).ToList();

    //            for (int srcIndex = 0; srcIndex < blobRectList.Count; srcIndex++)
    //            {
    //                BlobRect srcBlob = blobRectList[srcIndex];

    //                int endSearchIndex = srcIndex + 1;

    //                if (tryNum % 2 == 0)
    //                {
    //                    for (int i = endSearchIndex; i < blobRectList.Count; i++)
    //                    {
    //                        if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
    //                            endSearchIndex = i;
    //                        else
    //                            break;
    //                    }
    //                }
    //                else
    //                {
    //                    for (int i = endSearchIndex; i < blobRectList.Count; i++)
    //                    {
    //                        if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
    //                            endSearchIndex = i;
    //                        else
    //                            break;
    //                    }
    //                }

    //                RectangleF inflateRect = srcBlob.BoundingRect;
    //                inflateRect.Inflate(inflate, inflate);

    //                for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex && destIndex < blobRectList.Count; destIndex++)
    //                {
    //                    BlobRect destBlob = blobRectList[destIndex];

    //                    if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
    //                    {
    //                        srcBlob = srcBlob + destBlob;
    //                        blobRectList[srcIndex] = srcBlob;

    //                        blobRectList.RemoveAt(destIndex);

    //                        endSearchIndex--;
    //                        destIndex--;

    //                        if (tryNum % 2 == 0)
    //                        {
    //                            for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
    //                            {
    //                                if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
    //                                    endSearchIndex = i;
    //                                else
    //                                    break;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
    //                            {
    //                                if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
    //                                    endSearchIndex = i;
    //                                else
    //                                    break;
    //                            }
    //                        }

    //                        if (merged == false)
    //                            merged = true;

    //                        inflateRect = srcBlob.BoundingRect;
    //                        inflateRect.Inflate(inflate, inflate);
    //                    }
    //                }
    //            }

    //            if (merged == true)
    //                tryNum++;
    //        }

    //        blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X + defect.BoundingRect.Y).ToList();
    //    }

    //    public void RemoveIntersectBlobs(ref List<BlobRect> blobRectList)
    //    {
    //        for (int srcIndex = 0; srcIndex < blobRectList.Count; srcIndex++)
    //        {
    //            BlobRect srcBlob = blobRectList[srcIndex];

    //            for (int destIndex = srcIndex + 1; destIndex < blobRectList.Count; destIndex++)
    //            {
    //                BlobRect destBlob = blobRectList[destIndex];


    //                if (srcBlob.BoundingRect.IntersectsWith(destBlob.BoundingRect) == true)
    //                {
    //                    if (srcBlob.Area >= destBlob.Area)
    //                    {
    //                        blobRectList.RemoveAt(destIndex);
    //                        destIndex--;
    //                    }
    //                    else
    //                    {
    //                        blobRectList.RemoveAt(srcIndex);
    //                        srcIndex--;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
//using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Detector
{
    public class Detector : Algorithm
    {
        public static string TypeName { get { return "Detector"; } }

        public Detector()
        {
            this.AlgorithmName = TypeName;
            this.param = new DetectorParam();
        }

        #region Abstract
        public override Algorithm Clone()
        {
            throw new NotImplementedException();
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            throw new NotImplementedException();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new DetectorResult();
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DebugContext debugContext = new DebugContext(algorithmInspectParam.DebugContext.SaveDebugImage, Path.Combine(algorithmInspectParam.DebugContext.FullPath, "Detector"));

            DetectorResult detectorResult = (DetectorResult)CreateAlgorithmResult();

            SheetInspectParam inspectParam = algorithmInspectParam as SheetInspectParam;
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.CalculatorBase.TypeName).Param as CalculatorParam;
            DetectorParam detectorParam = this.param as DetectorParam;
            CancellationToken cancellationToken = inspectParam.CancellationToken;

            SizeF pelSize = new SizeF(14, 14);
            Calibration calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                pelSize = calibration.PelSize;

            ProcessBufferSetG buffer = inspectParam.ProcessBufferSet as ProcessBufferSetG;
            RegionInfoG inspRegionInfo = inspectParam.RegionInfo as RegionInfoG;
            bool testInspect = inspectParam.TestInspect;

            AlgoImage fullImage = inspectParam.AlgoImage;
            AlgoImage sheetImage = buffer.ScaledImage;
            if (sheetImage == null)
                sheetImage = fullImage;

            AlgoImage blobImage = buffer.DetectorInsp;
            if (blobImage == null || !blobImage.IsCompatible(TypeName))
                blobImage = buffer.CalculatorResult;

            AlgoImage binalImage = buffer.DetectorBinal;           

            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);

            sheetImage.Save("1sheetImage.bmp", debugContext);
            blobImage.Save("2blobImage.bmp", debugContext);
            //dynThresholdImage.Save("DetectorDynThresholdImage.bmp", debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(blobImage);
            int maxDefectCount = detectorParam.MaximumDefectCount;
            int defectThreshold = detectorParam.DetectThresholdBase;

            int diffXPos = (int)inspectParam.FidOffset.Width;
            int diffYPos = (int)inspectParam.FidOffset.Height;
            Point diffPos = new Point(diffXPos, diffYPos);

            detectorResult.OffsetFound = inspectParam.FidOffset;
            detectorResult.SheetSize = new SizeF(fullImage.Width * pelSize.Width / 1000, fullImage.Height * pelSize.Height / 1000);
            //return detectorResult;

            //imageProcessing.Binarize(blobImage, blobImage, defectThreshold);
            int scaleFactor = 1;
            if (SystemManager.Instance() != null)
                scaleFactor = SystemManager.Instance().CurrentModel.ScaleFactor;

            int indexOf = calculatorParam.RegionInfoList.IndexOf(inspRegionInfo);
            int startRegionId = Math.Max(0, indexOf);
            int endRegionId = indexOf < 0 ? calculatorParam.RegionInfoList.Count : startRegionId + 1;
            //foreach (RegionInfoG regionInfoG in calculatorParam.RegionInfoList)
            //if (inspRegionInfo == null)
            // 모든 영역 검사
            {
                // 1. Threshold (if I>=1, I=MAX, else, I=0)
                ThresholdProcess(blobImage, binalImage, defectThreshold);
                binalImage.Save("3binalImage.bmp", debugContext);
                //binalImage.Save(@"C:\temp\3binalImage.bmp");
                for (int i = startRegionId; i < endRegionId; i++)
                {
                    Stopwatch sw2 = Stopwatch.StartNew();

                    if (cancellationToken.IsCancellationRequested)
                        break;

                    RegionInfoG regionInfo = calculatorParam.RegionInfoList[i];
                    if (regionInfo.Use == false)
                        continue;

                    Rectangle subRegion = regionInfo.Region;
                    subRegion.Offset(diffPos);
                    Rectangle adjustSubRegion = Rectangle.Intersect(imageRect, subRegion);
                    if (subRegion != adjustSubRegion)
                        continue;

                    AlgoImage subSheetImage = sheetImage.GetSubImage(subRegion);
                    AlgoImage subBlobImage = blobImage.GetSubImage(subRegion);
                    AlgoImage subBinalImage = binalImage.GetSubImage(subRegion);
                    //subSheetImage.Save(@"C:\temp\subSheetImage.bmp");
                    //subBlobImage.Save(@"C:\temp\subBlobImage.bmp");
                    //subBinalImage.Save(@"C:\temp\subBinalImage.bmp");
                    List<BlobRect> defectRectSubList = null;
                    try
                    {
                        DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("Region{0}", i)));

                        // 2. Blob
                        float minSize = Math.Min(AlgorithmSetting.Instance().MinBlackDefectLength, AlgorithmSetting.Instance().MinWhiteDefectLength) * 2.0f / 3.0f;
                        defectRectSubList = BlobProcess(subSheetImage, subBinalImage, regionInfo.GetPatRect(), minSize, newDebugContext);

                        if (defectRectSubList == null)
                            continue;

                        for (int j = 0; j < defectRectSubList.Count; j++)
                        {
                            Gravure.Data.SheetSubResult subResult = null;
                            subResult = CalculateBlob(fullImage, subSheetImage, subBlobImage, subBinalImage, scaleFactor, pelSize, regionInfo, j, defectRectSubList[j], debugContext);
                            if (subResult != null && subResult.IsValid)
                            {
                                subResult.Offset(subRegion.Location, scaleFactor, pelSize);
                                detectorResult.SheetSubResultList.Add(subResult);
                            }

                            if (testInspect == false && maxDefectCount > 0 && detectorResult.SheetSubResultList.Count >= maxDefectCount)
                                break;
                        }
                        //Debug.WriteLine(string.Format("Total Founded Defects: {0}", detectorResult.SheetSubResultList.Count));
                    }
                    //catch { }
                    finally
                    {
                        subSheetImage.Dispose();
                        subBlobImage.Dispose();
                        subBinalImage.Dispose();
                    }

                    if (testInspect == false && maxDefectCount > 0 && detectorResult.SheetSubResultList.Count >= maxDefectCount)
                        break;

                    sw2.Stop();
                    //Debug.WriteLine(string.Format("Detecting Time: {0}ms", sw2.ElapsedMilliseconds));
                }
            }
            //else
            //// 선택된 영역만 검사
            //{
            //    Rectangle subRegion = inspRegionInfo.Region;
            //    subRegion.Offset(diffPos);
            //    Rectangle adjustSubRegion = Rectangle.Intersect(imageRect, subRegion);
            //    if (subRegion == adjustSubRegion)
            //    {
            //        AlgoImage subAlgoImage = sheetImage.GetSubImage(subRegion);
            //        AlgoImage subBlobImage = blobImage.GetSubImage(subRegion);
            //        AlgoImage subBinalImage = binalImage.GetSubImage(subRegion);
                    
            //        List<BlobRect> defectRectSubList = BlobProcess(subAlgoImage, subBlobImage, subBinalImage, defectThreshold, inspRegionInfo, debugContext);
            //        defectRectSubList.ForEach(f => f.MoveOffset(subRegion.Location));

            //        subAlgoImage.Dispose();
            //        subBlobImage.Dispose();
            //        subBinalImage.Dispose();

            //        for (int i = 0; i < defectRectSubList.Count; i++)
            //        {
            //            if (cancellationToken.IsCancellationRequested)
            //                break;

            //            Gravure.Data.SheetSubResult subResult = CalculateBlob(fullSheetImage, sheetImage, blobImage, binalImage, scaleFactor, inspRegionInfo, i, defectRectSubList[i], debugContext);
            //            if (subResult != null)
            //            {
            //                lock (detectorResult)
            //                {
            //                    //Rectangle rectangle = subResult.Region;
            //                    //rectangle.Offset(inspRegionInfo.Region.Location);
            //                    //subResult.Region = rectangle;
            //                    //defectRectSubList.ForEach(f => f.MoveOffset(inspRegionInfo.Region.Location));
            //                    detectorResult.SheetSubResultList.Add(subResult);
            //                }
            //            }
            //        }
            //        //defectRectSubList.ForEach(f => f.MoveOffset(inspRegionInfo.Region.Location));
            //    }
            //}

            int detectCount = detectorResult.SheetSubResultList.Count;
            int saCount = detectorResult.SheetSubResultList.FindAll(f => ((Gravure.Data.SheetSubResult)f).ShapeType == ShapeType.Linear).Count;
            if (detectCount == 0)
            {
                detectorResult.Message = "No Defect";
            }
            else if (detectCount >= maxDefectCount)
            {
                detectorResult.Message = "Too Many Defects";
            }
            else
            {
                detectorResult.Message = string.Format("{0} Defects", detectCount);
            }

            sw.Stop();

            detectorResult.SpandTime = sw.Elapsed;//new TimeSpan(, sw.ElapsedMilliseconds);
            detectorResult.Good = (detectCount == 0);
            return detectorResult;
        }

        private void ThresholdProcess(AlgoImage subBlobImage, AlgoImage subBinalImage, int threshold)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subBlobImage);
            Rectangle imageRect = new Rectangle(Point.Empty, subBinalImage.Size);

            imageProcessing.Binarize(subBlobImage, subBinalImage, threshold);
            //imageProcessing.Close(subBinalImage, 1);
        }

        private void CalcDefectType(AlgoImage algoImage, BlobRect defectBlob, int binalValue, float poleAvg, float dielectricAvg, ref Gravure.Data.SheetSubResult sheetSubResult)
        {
            //float defectAroundImageValue = GetDefectImageValue(defectBlob, algoImage, new Size(5, 5));
            //sheetSubResult.PositionType = (defectAroundImageValue < binalValue * 0.8 ? PositionType.Pole : PositionType.Dielectric);

            // GetPositionType
            sheetSubResult.PositionType = GetPositionType(defectBlob, algoImage, new Size(5, 5), binalValue);

            // GetValueType
            if (sheetSubResult.PositionType == PositionType.Pole)
            {
                //float valueDiff = defectBlob.MaxValue - defectBlob.MinValue;
                if (defectBlob.MeanValue <= poleAvg)// * 1.3 /*|| valueDiff > 40*/)
                    sheetSubResult.ValueType = Data.ValueType.Dark;
                else
                    sheetSubResult.ValueType = Data.ValueType.Bright;
            }
            else if (sheetSubResult.PositionType == PositionType.Dielectric)
            {
                if (defectBlob.MeanValue < dielectricAvg * 1.3)
                    sheetSubResult.ValueType = Data.ValueType.Dark;
                else
                    sheetSubResult.ValueType = Data.ValueType.Bright;
            }

            // GetShapeType
            if (defectBlob.Compactness <= 2.5 || defectBlob.MaxFeretDiameter / defectBlob.MinFeretDiameter <= 2.0)
                sheetSubResult.ShapeType = ShapeType.Circular;
            else
                sheetSubResult.ShapeType = ShapeType.Linear;

            sheetSubResult.FillRate = defectBlob.Area * 1.0f / (defectBlob.BoundingRect.Width * defectBlob.BoundingRect.Height) * 100;
            //sheetSubResult.FillRate = defectBlob.Area * 1.0f / (defectBlob.MaxFeretDiameter * defectBlob.MinFeretDiameter) * 100;
        }

        private PositionType GetPositionType(BlobRect defectBlob, AlgoImage algoImage, Size inflate, int binValue)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            Rectangle blobRect, fullRect;
            blobRect = fullRect = Rectangle.Round(defectBlob.BoundingRect);
            fullRect.Inflate(inflate);
            fullRect.Intersect(new Rectangle(Point.Empty, algoImage.Size));

            Point subRectLoc = fullRect.Location;
            Size subRectSize = inflate;
            float[,] mapDark = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rectangle subRect = new Rectangle(subRectLoc, subRectSize);
                    subRect.Intersect(fullRect);

                    if (subRect.IsEmpty == false)
                    {
                        AlgoImage subAlgoImage = algoImage.GetSubImage(subRect);
                        mapDark[i, j] = imageProcessing.GetGreyAverage(subAlgoImage);
                        subAlgoImage.Dispose();
                    }

                    subRectLoc.X += subRectSize.Width;
                    subRectSize.Width = (j == 0 ? blobRect.Width : inflate.Width);
                }
                subRectLoc.X = fullRect.X;
                subRectLoc.Y += subRectSize.Height;
                subRectSize.Height = (i == 0 ? blobRect.Height : inflate.Height);
            }


            float[][] isInPole = new float[][]
            {
                new float[]{mapDark[0, 0] ,mapDark[0, 1] , mapDark[1, 0] }, //leftTop
                new float[]{mapDark[0, 1] , mapDark[0, 2] , mapDark[1, 2] },//rightTop
                new float[]{mapDark[1, 0] , mapDark[2, 0] , mapDark[2, 1] },//leftBottom
                new float[]{mapDark[1, 2] , mapDark[2, 1] , mapDark[2, 2] },//rightBottom;

                new float[]{mapDark[0, 1] , mapDark[2, 1] },//vertical
                new float[]{mapDark[1, 0] , mapDark[1, 2] },//horizen
                new float[]{mapDark[2, 0] , mapDark[0, 2] },//upperDiagonal
                new float[]{mapDark[0, 0] , mapDark[2, 2] }//lowerDiagonal
            };

            if (isInPole.Any(f=>Array.TrueForAll(f,g=>g<binValue)))
                return PositionType.Pole;
            return PositionType.Dielectric;
        }

        private float GetDefectImageValue(BlobRect defectBlob, AlgoImage algoImage, Size inflate)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float defectValue = defectBlob.MeanValue * defectBlob.Area;

            Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
            defectRect.Inflate(inflate);
            defectRect.Intersect(new Rectangle(Point.Empty, algoImage.Size));

            int rectArea = defectRect.Width * defectRect.Height;
            AlgoImage defectCenterImage = algoImage.GetSubImage(defectRect);
            float rectValue = imageProcessing.GetGreyAverage(defectCenterImage) * rectArea;
            defectCenterImage.Dispose();

            // 사각형 부분에서 불량 부분을 제외한 영역의 평균
            return (rectValue - defectValue) / (rectArea - defectBlob.Area);

            //float defectValue = defectBlob.MeanValue;

            //Rectangle defectAroundRect = Rectangle.Round(defectBlob.BoundingRect);
            //defectAroundRect.Inflate(inflate);
            //AlgoImage defectCenterImage = algoImage.GetSubImage(defectAroundRect);
            //float rectValue = imageProcessing.GetGreyAverage(defectCenterImage);
            //defectCenterImage.Dispose();

            //return rectValue;
        }

        private Gravure.Data.SheetSubResult CalculateBlob(AlgoImage fullSheetImage, AlgoImage sheetImage, AlgoImage blobImage, AlgoImage binalImage, 
            int scaleFactor, SizeF pelSize, RegionInfoG regionInfoG, int blobId, BlobRect defectBlob, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.CalculatorBase.TypeName).Param as CalculatorParam;

            SheetFinderBaseParam sheetFinderBaseParam = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName).Param as SheetFinderBaseParam;
            BaseXSearchDir baseXSearchDir = sheetFinderBaseParam.BaseXSearchDir;
            int binValue = calculatorParam.BinValue;
            Point basePoint = calculatorParam.BasePosition;
            float poleAvg = regionInfoG.PoleAvg;
            float dielectricAvg = regionInfoG.DielectricAvg;

            DetectorParam detectorParam = this.param as DetectorParam;

            Gravure.Data.SheetSubResult subResult = new Gravure.Data.SheetSubResult();

            RectangleF avgIntensityRect = RectangleF.Empty;
            bool isFalseNG = IsFalseNG(sheetImage, defectBlob, out avgIntensityRect);
            if (isFalseNG)
            {
                subResult.AvgIntensity = avgIntensityRect;
                subResult.IsValid = false;
                return subResult;
            }

            CalcDefectType(sheetImage, defectBlob, binValue, poleAvg, dielectricAvg, ref subResult);

            RectangleF boundRect = defectBlob.BoundingRect;

            PointF realLoc = new PointF(boundRect.X * pelSize.Width, boundRect.Y * pelSize.Height);
            SizeF realSize = new SizeF(boundRect.Width * pelSize.Width, boundRect.Height * pelSize.Height);
            RectangleF realRect = new RectangleF(realLoc, realSize);

            Rectangle defectRect = Rectangle.Round(boundRect);
            AlgoImage defectBlobImage = blobImage.GetSubImage(defectRect);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(blobImage);
            int maxSubtractValue = (int)ip.GetGreyMax(defectBlobImage);
            int avgSubtractValue = (int)ip.GetGreyAverage(defectBlobImage);

            if (detectorParam.FineSizeMeasure && false)
            {
                float maxValue = ip.GetGreyMax(defectBlobImage);
                int newThreshold = (int)Math.Round(maxValue * detectorParam.FineSizeMeasureThresholdMul);
                if (newThreshold > detectorParam.DetectThresholdBase)
                {
                    int scaleF = detectorParam.FineSizeMeasureSizeMul;
                    defectBlobImage.Save(string.Format("{0}_defectBlobImage.bmp", blobId), debugContext);
                    AlgoImage defectBinalImage = ImageBuilder.Build(defectBlobImage.LibraryType, defectBlobImage.ImageType, defectBlobImage.Width * scaleF, defectBlobImage.Height * scaleF);
                    ip.Resize(defectBlobImage, defectBinalImage, scaleF);
                    defectBlobImage.Save(string.Format("{0}_defectBinalImage1.bmp", blobId), debugContext);
                    ip.Binarize(defectBinalImage, newThreshold);
                    defectBlobImage.Save(string.Format("{0}_defectBinalImage2.bmp", blobId), debugContext);

                    BlobRectList blobRectList = ip.Blob(defectBinalImage, new BlobParam { SelectArea = true, SelectBoundingRect = true });
                    List<BlobRect> blobRects = blobRectList.GetList();
                    BlobRect blobRect = blobRects.Find(f => f.BoundingRect.Contains(new PointF(defectBinalImage.Width / 2.0f, defectBinalImage.Height / 2.0f)));
                    if (blobRect != null)
                    {
                        PointF location = PointF.Add(defectBlob.BoundingRect.Location, new SizeF(blobRect.BoundingRect.X / scaleF, blobRect.BoundingRect.Y / scaleF));
                        SizeF size = new SizeF(blobRect.BoundingRect.Width / scaleF, blobRect.BoundingRect.Height / scaleF);
                        boundRect = new RectangleF(location, size);
                        defectRect = Rectangle.Round(boundRect);

                        realLoc = new PointF(boundRect.X * pelSize.Width, boundRect.Y * pelSize.Height);
                        realSize = new SizeF(boundRect.Width * pelSize.Width, boundRect.Height * pelSize.Height);
                        realRect = new RectangleF(realLoc, realSize);
                    }

                    blobRectList.Dispose();
                    defectBinalImage.Dispose();
                }
            }
            defectBlobImage.Dispose();

            float boundSize =  Math.Min(realSize.Width, realSize.Height);
            //Debug.WriteLine(string.Format("BoundSize: {0}", boundSize));
            float minSize = -1;
            switch (subResult.PositionType)
            {
                case PositionType.Pole:
                    minSize = AlgorithmSetting.Instance().MinBlackDefectLength;
                    break;
                case PositionType.Dielectric:
                    //minSize = detectorParam.MinWhiteDefectLength;
                    minSize = AlgorithmSetting.Instance().MinWhiteDefectLength;
                    break;
            }
            if (minSize > boundSize)
                return null;

            // Calculate Coordinate
            Rectangle adjustDefectRect = new Rectangle(defectRect.X * scaleFactor, defectRect.Y * scaleFactor, defectRect.Width * scaleFactor, defectRect.Height * scaleFactor);
            
            subResult.ResultRect = new RotatedRect(adjustDefectRect, 0);
            subResult.Good = false;
            subResult.Region = adjustDefectRect;
            subResult.RealCenterPos = DrawingHelper.CenterPoint(realRect);
            subResult.RealRegion = realRect;
            subResult.Length = (int)defectBlob.Area;    // length가 면적? realLength는 안쓰나?
            subResult.SubtractValueMax = maxSubtractValue;
            subResult.SubtractValueMin = avgSubtractValue;

            float clipRectScale = 50f / scaleFactor;
            Rectangle clipRect1 = Rectangle.Inflate(adjustDefectRect, 50, 50);
            clipRect1.Intersect(new Rectangle(Point.Empty, sheetImage.Size));

            Rectangle clipRect2 = Rectangle.Round(RectangleF.Inflate(adjustDefectRect, clipRectScale, clipRectScale));
            clipRect2.Intersect(new Rectangle(Point.Empty, binalImage.Size));

            if (clipRect1.Width > 0 && clipRect1.Height > 0)
            {
                AlgoImage subImage = sheetImage.Clip(clipRect1);
                //ImageD subImageD = subImage.ToImageD();
                subResult.Image = subImage.ToBitmap();
                //subImageD.Dispose();
                subImage.Dispose();
            }

            if (clipRect2.Width > 0 && clipRect2.Height > 0)
            //if (clipRect1.Width > 0 && clipRect1.Height > 0)
            {
                AlgoImage subBinalImage = binalImage.Clip(clipRect2);
                //AlgoImage subBinalImage = binalImage.GetSubImage(clipRect1);
                //ImageD subBinalImageD = subBinalImage.ToImageD();
                subResult.BufImage = subBinalImage.ToBitmap();
                //subBinalImageD.Dispose();
                subBinalImage.Dispose();
            }

            return subResult;
        }

        private bool IsFalseNG(AlgoImage algoImage, BlobRect defectBlob, out RectangleF avgIntensityRect)
        {
            // 가성확인
            avgIntensityRect = RectangleF.Empty;

            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
            Rectangle adjustDefectRect = Rectangle.Intersect(Rectangle.Inflate(defectRect, 10, 10), imageRect);
            if (adjustDefectRect.Width == 0 || adjustDefectRect.Height == 0)
                return true;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float maxRect = Math.Max(defectBlob.BoundingRect.Width, defectBlob.BoundingRect.Height);
            float minRect = Math.Min(defectBlob.BoundingRect.Width, defectBlob.BoundingRect.Height);
            bool isLonged = (maxRect / minRect >= 2);
            float isFulled = defectBlob.Area*1.0f / (defectBlob.BoundingRect.Width * defectBlob.BoundingRect.Height);
            if (isLonged)
            {
                AlgoImage subAlgoImage = algoImage.GetSubImage(adjustDefectRect);
                float[] avgIntensity = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    int l = i == 2 ? adjustDefectRect.Width / 2 : 0;
                    int t = i == 3 ? adjustDefectRect.Height / 2 : 0;
                    int r = i == 0 ? adjustDefectRect.Width / 2 : adjustDefectRect.Width;
                    int b = i == 1 ? adjustDefectRect.Height / 2 : adjustDefectRect.Height;
                    Rectangle sRect = Rectangle.FromLTRB(l, t, r, b);
                    avgIntensity[i] = imageProcessing.GetGreyAverage(subAlgoImage, sRect);
                }
                subAlgoImage.Dispose();
                avgIntensityRect = RectangleF.FromLTRB(avgIntensity[0], avgIntensity[1], avgIntensity[2], avgIntensity[3]);

                if (defectBlob.BoundingRect.Width > defectBlob.BoundingRect.Height)
                    return (Math.Abs(avgIntensity[1] - avgIntensity[3]) > 70);
                else
                    return (Math.Abs(avgIntensity[0] - avgIntensity[2]) > 70);
            }
            else
            {
                return false;
            }
        }

        private List<BlobRect> BlobProcess(AlgoImage subAlgoImage, AlgoImage subBinalImage, Rectangle patternRect, float minSize, DebugContext debugContext)
        {
            try
            {
                subAlgoImage.Save("RoiAlgoImage.bmp", debugContext);

                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subBinalImage);
                Rectangle imageRect = new Rectangle(Point.Empty, subBinalImage.Size);

                BlobParam blobParam = new BlobParam();
                blobParam.MaxCount = 0;
                blobParam.SelectArea = true;
                blobParam.AreaMin = 4;
                blobParam.SelectCenterPt = true;
                blobParam.SelectMeanValue = true;
                blobParam.SelectMinValue = true;
                blobParam.SelectMaxValue = true;
                blobParam.SelectCompactness = true;
                blobParam.SelectFeretDiameter = true;
                blobParam.SelectBoundingRect = true;
                blobParam.EraseBorderBlobs = true;

                BlobRectList blobResult = imageProcessing.Blob(subBinalImage, blobParam, subAlgoImage);
                List<BlobRect> blobRectList = blobResult.GetList();
                blobResult.Dispose();

                RemoveBlobs(blobRectList, patternRect, minSize);

                RectangleF wholeRect = new RectangleF(0, 0, subBinalImage.Width, subBinalImage.Height);
                MergeBlobs(blobRectList, wholeRect, 0);

                return blobRectList;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - Detector::Process - {0}", ex.Message));
                return null;
            }
#endif
            finally { }
        }

        private void RemoveBlobs(List<BlobRect> blobRectList, Rectangle rectangle, float minSize)
        {
            SizeF pelSize = new SizeF(14, 14);
            Calibration calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                pelSize = calibration.PelSize;

            rectangle.Inflate(-1, -1);

            blobRectList.RemoveAll(f =>
            {
                Rectangle blobRect = Rectangle.Round(f.BoundingRect);
                if (Rectangle.Intersect(blobRect, rectangle) != blobRect)
                    return true;

                float realSize = Math.Min(f.BoundingRect.Width * pelSize.Width, f.BoundingRect.Height * pelSize.Height);
                if (realSize < minSize)
                    return true;

                return false;
            });
        }

        private void MergeBlobs(List<BlobRect> blobRectList, RectangleF wholeRect, int inflate)
        {
            if (inflate == 0)
                return;

            bool merged = true;

            List<BlobRect> mergedBlobRect = new List<BlobRect>(blobRectList);

            int tryNum = 0;
            while (merged == true)
            {
                merged = false;

                if (tryNum % 2 == 0)
                    mergedBlobRect = mergedBlobRect.OrderBy(defect => defect.BoundingRect.X).ToList();
                else
                    mergedBlobRect = mergedBlobRect.OrderBy(defect => defect.BoundingRect.Y).ToList();

                for (int srcIndex = 0; srcIndex < mergedBlobRect.Count; srcIndex++)
                {
                    BlobRect srcBlob = mergedBlobRect[srcIndex];

                    RectangleF inflateRect = srcBlob.BoundingRect;
                    inflateRect.Inflate(inflate, inflate);

                    int endSearchIndex = srcIndex;

                    if (tryNum % 2 == 0)
                    {
                        for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
                        {
                            if (mergedBlobRect[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }
                    else
                    {
                        for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
                        {
                            if (mergedBlobRect[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }

                    for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex; destIndex++)
                    {
                        BlobRect destBlob = mergedBlobRect[destIndex];

                        if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
                        {
                            srcBlob.Area += destBlob.Area;

                            srcBlob.BoundingRect = RectangleF.Union(srcBlob.BoundingRect, destBlob.BoundingRect);
                            srcBlob.CenterPt = new PointF((srcBlob.CenterPt.X + destBlob.CenterPt.X) / 2.0f, (srcBlob.CenterPt.Y + destBlob.CenterPt.Y) / 2.0f);
                            srcBlob.MinValue = Math.Min(srcBlob.MinValue, destBlob.MinValue);
                            srcBlob.MaxValue = Math.Max(srcBlob.MaxValue, destBlob.MaxValue);
                            srcBlob.MeanValue = (srcBlob.MeanValue + destBlob.MeanValue) / 2;

                            inflateRect = srcBlob.BoundingRect;
                            inflateRect.Inflate(inflate, inflate);

                            mergedBlobRect.RemoveAt(destIndex);

                            endSearchIndex--;
                            destIndex--;

                            if (tryNum % 2 == 0)
                            {
                                for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
                                {
                                    if (mergedBlobRect[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
                                {
                                    if (mergedBlobRect[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }

                            if (merged == false)
                                merged = true;
                        }
                    }
                }

                if (merged == true)
                    tryNum++;
            }

            blobRectList.Clear();
            blobRectList.AddRange(mergedBlobRect);
        }
    }
}

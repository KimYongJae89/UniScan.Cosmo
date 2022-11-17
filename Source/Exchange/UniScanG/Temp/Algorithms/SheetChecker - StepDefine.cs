//using UniScanG.Operation.Inspect;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DynMvp.Vision;
//using DynMvp.Base;
//using DynMvp.UI;
//using System.Drawing;
//using System.Diagnostics;
//using UniEye.Base.Settings;
//using UniScanG.Operation.Data;
//using DynMvp.Data;
//using System.Threading;
//using System.Runtime.InteropServices;
//using UniEye.Base;
//using System.IO;

//namespace UniScanG.Temp
//{
//    #region Misc
//    internal sealed class GravuerSIMD
//    {
//        [DllImport("GravuerCalculateWithSIMD.dll", CallingConvention = CallingConvention.StdCall)]
//        public static extern int GravuerCalculateWithSIMD(IntPtr pSrc, IntPtr pRef, IntPtr pMask, IntPtr pDst, int srcPtich, int dstPtich, byte nThreshold, int nWidth, int nHeight);
//    }

//    internal class AlgorithmSubStepParam : AlgorithmInspectParam
//    {
//        public AlgorithmSubStepParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
//            : base(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext) { }

//        public AlgorithmSubStepParam(AlgorithmInspectParam algorithmInspectParam)
//            : base(algorithmInspectParam) { }
//    }

//    internal class AlgorithmSubStepResult : DynMvp.Vision.AlgorithmResult
//    {

//    }

//    internal abstract class SheetCheckerStep : UniScanG.Common.Algorithm.Algorithm
//    {
//        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
//        {
//            throw new NotImplementedException();
//        }

//        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
//        {
//            throw new NotImplementedException();
//        }

//        public override DynMvp.Vision.Algorithm Clone()
//        {
//            throw new NotImplementedException();
//        }

//        public override string GetAlgorithmType()
//        {
//            return "SheetChecker";
//        }

//        public override string GetAlgorithmTypeShort()
//        {
//            throw new NotImplementedException();
//        }

//        public override List<AlgorithmResultValue> GetResultValues()
//        {
//            throw new NotImplementedException();
//        }

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            throw new NotImplementedException();
//        }

//        public override AlgorithmInspectParam CreateAlgorithmInspectParam
//            (ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
//        {
//            return base.CreateAlgorithmInspectParam(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext);
//        }

//        public override AlgorithmResult CreateAlgorithmResult()
//        {
//            AlgorithmResult algorithmResult = new AlgorithmResult(AlgorithmName);
//            return algorithmResult;
//        }
//    }

//    #endregion

//    #region CalculateStep

//    internal class SheetCheckerStepCalculate : SheetCheckerStep
//    {
//        public SheetCheckerStepCalculate(AlgorithmParam sheetCheckerParam)
//        {
//            this.AlgorithmName = "SheetCheckerStepCalculate";
//            this.param = sheetCheckerParam as SheetCheckerParam;
//        }

//        public override string ToString()
//        {
//            return "SheetCheckerStepCalculate";
//        }

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();

//            AlgorithmResult algorithmResult = CreateAlgorithmResult();

//            SheetCheckerInspectParam inspectParam = algorithmInspectParam as SheetCheckerInspectParam;
//            SheetImageSet inspImageSet = inspectParam.InspImageSet;
//            ProcessBuffer buffer = inspectParam.ProcessBuffer;

//            bool saveImage = ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Image) > 0);
//            DebugContext debugContext = new DebugContext(saveImage, Path.Combine(inspectParam.DebugContext.Path, "Calculate"));

//            TrainerParam param = (this.param as SheetCheckerParam).TrainerParam;
//            Size refImageSize = inspImageSet.GetImageSize();
//            int fidOffset = inspImageSet.fidXPos - param.FiducialXPos;

//            float fillRatio = 0.1f;

//            AlgoImage processedImages1 = buffer.ImageBuffer1;
//            AlgoImage processedImages2 = buffer.ImageBuffer2;
//            processedImages1.Clear();
//            processedImages2.Clear();

//            AlgoImage processedImages4 = buffer.SmallBuffer;
//            processedImages4.Clear();

//            List<SheetRange> toalProjectionRangeList = new List<SheetRange>();

//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(processedImages1);

//            int patternHeight = (int)Math.Round(param.RefPattern.PatternGroup.AverageHeight);

//            //foreach (ProjectionRegion projectionRegion in param.ProjectionRegionList)
//            Parallel.ForEach(param.ProjectionRegionList, (projectionRegion) =>
//             {
//                 Rectangle clipRect = projectionRegion.Region;
//                 clipRect.Offset(fidOffset, 0);
//                 clipRect.Intersect(Rectangle.FromLTRB(0, 0, refImageSize.Width, refImageSize.Height));
//                 if (clipRect.Width == 0 || clipRect.Height == 0)
//                     return;

//                 AlgoImage inspImage = inspImageSet.GetSubImage(clipRect);
//                 AlgoImage resultImage = processedImages1.GetSubImage(clipRect);
//                 AlgoImage tempImage = processedImages2.GetSubImage(clipRect);

//                 // Process to ROI
//                 DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, System.IO.Path.Combine(debugContext.Path, string.Format("ROI{0}", projectionRegion.Id)));
//                 inspImage.Save(String.Format("ROI.bmp"), newDebugContext);
//                 List<Rectangle> inspLineRectList = ProcessROI(inspImage, resultImage, tempImage, param.AutoThresholdValue, patternHeight, fillRatio, param.DefectThreshold, newDebugContext);
//                 lock (inspectParam.SheetCheckerAlgorithmResult.ResultValueList)
//                     inspectParam.SheetCheckerAlgorithmResult.ResultValueList.Add(new AlgorithmResultValue(String.Format("INSP_LINE_RECT_LIST_{0}", projectionRegion.Id), inspLineRectList));

//                 Rectangle smallClipRect = new Rectangle(clipRect.X / 5, clipRect.Y / 5, clipRect.Width / 5, clipRect.Height / 5);
//                 AlgoImage contourImage = processedImages4.GetSubImage(smallClipRect);

//                 imageProcessing.Resize(inspImage, contourImage, 0.2);

//                 imageProcessing.Binarize(contourImage, contourImage, param.AutoThresholdValue, true);

//                 BlobParam blobParam = new BlobParam();
//                 blobParam.AreaMin = param.MinPatternArea / 16.0f;
//                 blobParam.SelectLabelValue = true;

//                 BlobRectList blobRectList = imageProcessing.Blob(contourImage, blobParam);

//                 contourImage.Clear();
//                 DrawBlobOption drawBlobOption = new DrawBlobOption();
//                 drawBlobOption.SelectBlobContour = true;

//                 imageProcessing.DrawBlob(contourImage, blobRectList, null, drawBlobOption);

//                 imageProcessing.Dilate(contourImage, 1);

//                 imageProcessing.Not(contourImage, contourImage);
//                 imageProcessing.Resize(contourImage, tempImage, 5);

//                 imageProcessing.And(resultImage, tempImage, resultImage);

//                 //tempImage.Save("tempImage.bmp", new DebugContext(true, "c:\\"));
//                 //blackResultImage.Save("blackResultImage.bmp", new DebugContext(true, "c:\\"));

//                 blobRectList.Dispose();

//                 contourImage.Dispose();

//                 inspImage.Dispose();
//                 resultImage.Dispose();
//                 tempImage.Dispose();
//             });

//            processedImages1.Save("processedImages1.bmp", debugContext);
//            processedImages2.Save("processedImages2.bmp", debugContext);

//            sw.Stop();
//            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan(sw.ElapsedTicks);
//            algorithmResult.Good = true;
//            //LogHelper.Debug(LoggerType.Inspection, string.Format("SheetCheckerStepProcess Inspection Time: {0}[ms]", sw.ElapsedMilliseconds));

//            return algorithmResult;
//        }

//        private List<Rectangle> ProcessROI(AlgoImage inspImage, AlgoImage resultImage, AlgoImage tempImage, int triangle, int patternHeight, float fillRatio, int defectThreshold, DebugContext debugContext)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);
//            Rectangle imageRect = new Rectangle(Point.Empty, inspImage.Size);

//            List<Rectangle> inspLineRectList = GetInspLineRectList(inspImage, tempImage, triangle, patternHeight, fillRatio, debugContext);

//            if (inspLineRectList.Count >= 3)
//            {
//                for (int i = 0; i < inspLineRectList.Count; i++)
//                {
//                    // 검사 할 줄
//                    Rectangle insppRect = inspLineRectList[i];

//                    // 윗줄. 윗줄이 없으면 아래줄의 아래줄 사용
//                    Rectangle upperRect = (i == 0) ? inspLineRectList[i + 2] : inspLineRectList[i - 1];

//                    // 아래줄. 아래줄이 없으면 위줄의 위줄 사용
//                    Rectangle lowerRect = (i == inspLineRectList.Count - 1) ? inspLineRectList[i - 2] : inspLineRectList[i + 1];

//                    DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, System.IO.Path.Combine(debugContext.Path, string.Format("Line{0}", i)));

//                    ProcessLine(inspImage, resultImage, tempImage, insppRect, upperRect, lowerRect, newDebugContext);
//                }

//                imageProcessing.Binarize(resultImage, resultImage, defectThreshold);
//            }
//            return inspLineRectList;
//        }

//        private void ProcessLine(AlgoImage inspImage, AlgoImage resultImage, AlgoImage tempImage, Rectangle insppRect, Rectangle upperRect, Rectangle lowerRect, DebugContext debugContext)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

//            AlgoImage upper = inspImage.GetSubImage(upperRect);
//            AlgoImage inspp = inspImage.GetSubImage(insppRect);
//            AlgoImage lower = inspImage.GetSubImage(lowerRect);
//            upper.Save("0-1. A.bmp", debugContext);
//            inspp.Save("0-2. B.bmp", debugContext);
//            lower.Save("0-3. C.bmp", debugContext);

//            AlgoImage temp1 = tempImage.GetSubImage(upperRect);
//            AlgoImage temp2 = tempImage.GetSubImage(insppRect);
//            AlgoImage temp3 = tempImage.GetSubImage(lowerRect);

//            AlgoImage result = resultImage.GetSubImage(insppRect);

//            imageProcessing.Subtract(inspp, upper, temp1); //notABS
//            temp1.Save("1-1. B-A.bmp", debugContext);
//            imageProcessing.Subtract(inspp, lower, temp2); //notABS
//            temp2.Save("1-2. B-C.bmp", debugContext);
//            imageProcessing.Min(temp1, temp2, temp1); //어두운 불량
//            temp1.Save("1-3. Min.bmp", debugContext);

//            imageProcessing.Subtract(upper, inspp, temp2); //notABS
//            temp2.Save("2-1. A-B.bmp", debugContext);
//            imageProcessing.Subtract(lower, inspp, temp3); //notABS
//            temp3.Save("2-2. C-B.bmp", debugContext);
//            imageProcessing.Min(temp2, temp3, temp2); //밝은 불량
//            temp2.Save("2-3. Min.bmp", debugContext);

//            imageProcessing.Max(temp1, temp2, result);
//            result.Save("3. Max.bmp", debugContext);
//            //imageProcessing.Binarize(result, defectThreshold);
//            result.Save("4. Bin.bmp", debugContext);

//            upper.Dispose();
//            inspp.Dispose();
//            lower.Dispose();
//            temp1.Dispose();
//            temp2.Dispose();
//            temp3.Dispose();
//            result.Dispose();
//        }

//        private void ProcessLine2(AlgoImage inspImage, AlgoImage resultImage, AlgoImage tempImage, Rectangle insppRect, Rectangle upperRect, Rectangle lowerRect, DebugContext debugContext)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

//            AlgoImage upper = inspImage.GetSubImage(upperRect);
//            AlgoImage inspp = inspImage.GetSubImage(insppRect);
//            AlgoImage lower = inspImage.GetSubImage(lowerRect);

//            AlgoImage tempUpper = tempImage.GetSubImage(upperRect);
//            AlgoImage tempLower = tempImage.GetSubImage(lowerRect);

//            AlgoImage result = resultImage.GetSubImage(insppRect);

//            imageProcessing.Subtract(inspp, upper, tempUpper, true);
//            imageProcessing.Subtract(inspp, lower, tempLower, true);
//            imageProcessing.Min(tempUpper, tempLower, result);

//            upper.Dispose();
//            inspp.Dispose();
//            lower.Dispose();
//            tempUpper.Dispose();
//            tempLower.Dispose();
//            result.Dispose();
//        }

//        private List<Rectangle> GetInspLineRectList(AlgoImage inspImage, AlgoImage tempImage, int triangle, int patternHeight, float fillRatio, DebugContext debugContext)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

//            // V-Projection for Y-Align
//            Rectangle vProjectionRect = new Rectangle();

//            int projectionSize2 = inspImage.Width / 6;
//            Point vProjCenter = new Point(inspImage.Width / 2, inspImage.Height / 2);
//            vProjectionRect = Rectangle.FromLTRB(vProjCenter.X - projectionSize2, 0, vProjCenter.X + projectionSize2, inspImage.Height);
//            vProjectionRect.Intersect(new Rectangle(Point.Empty, inspImage.Size));

//            AlgoImage inspSubImage = inspImage.Clip(vProjectionRect);
//            inspSubImage.Save(String.Format("GetInspLineList_inspSubImage.bmp"), debugContext);

//            AlgoImage tempSubImage = tempImage.GetSubImage(vProjectionRect);
//            imageProcessing.Binarize(inspSubImage, tempSubImage, triangle, true);
//            tempSubImage.Save(String.Format("GetInspLineList_tempSubImage.bmp"), debugContext);

//            float[] vProjectionData = imageProcessing.Projection(tempSubImage, Direction.Vertical, ProjectionType.Sum);

//            List<Rectangle> inspLineRectList = new List<Rectangle>();
//            List<SheetRange> sheetRangeList = GravureSheetChecker.GetProjectionRangeList(vProjectionData, patternHeight, fillRatio, vProjectionRect.Width);
//            int inflateHeight = 5;
//            if (sheetRangeList.Count > 0)
//            {
//                int rectHeight = (int)Math.Round(sheetRangeList.Average(f => f.Length)) + inflateHeight;

//                foreach (SheetRange sheetRange in sheetRangeList)
//                {
//                    inspLineRectList.Add(new Rectangle(0, sheetRange.StartPos + ((sheetRange.EndPos - sheetRange.StartPos) / 2) - (rectHeight / 2), inspImage.Width, rectHeight));
//                    //inspLineRectList.Add(Rectangle.FromLTRB(0, sheetRange.StartPos, inspImage.Width, sheetRange.EndPos));
//                }
//            }
//            inspLineRectList.RemoveAll(f => f.Top < 0 || f.Bottom >= inspImage.Height);

//            tempSubImage.Clear();
//            inspSubImage.Dispose();
//            tempSubImage.Dispose();

//            return inspLineRectList;
//        }



//        #region Garbige
//        /*
//         * Rectangle refImageRect, inspImageRect;
//            bool ok=GetCalculateRect(refImageSize, new Size(inspW, inspH), new Point(fidOffset, nextInspStartLine), out refImageRect, out inspImageRect);
//            nextInspStartLine += refImageRect.Height;
//            if (ok == false)
//            {
//                Debug.Assert(false, "Calculate Area is worng");
//                continue;
//            }

//            AlgoImage refChildImage = refImage.GetSubImage(refImageRect);
//            AlgoImage maskChildImage = maskImage.GetSubImage(refImageRect);
//            AlgoImage inspChildImage = inspImage.GetSubImage(inspImageRect);

//            debugContext.TimeProfile.Add("InspectStep", "CreateChildImage");

//            if (useSimd)
//            {
//                processedImages[i] = ImageBuilder.Build(inspChildImage.LibraryType, inspChildImage.ImageType, inspChildImage.Size.Width, inspChildImage.Size.Height);
//                IntPtr inspPtr = inspChildImage.GetImagePtr();
//                IntPtr reffPtr = refChildImage.GetImagePtr();
//                IntPtr maskPtr = maskChildImage.GetImagePtr();
//                IntPtr destPtr = processedImages[i].GetImagePtr();
//                GravuerSIMD.GravuerCalculateWithSIMD(inspPtr, reffPtr, maskPtr, destPtr, inspChildImage.Pitch, processedImages[i].Pitch, (byte)param.BinarizeValue, inspChildImage.Size.Width, inspChildImage.Size.Height);
//            }
//            else
//            {
//                processedImages[i] = Process(refChildImage, maskChildImage, inspChildImage, thVal, debugContext);
//            }

//            refChildImage.Dispose();
//            maskChildImage.Dispose();
//            inspChildImage.Dispose();
//            */
//        #endregion
//    }

//    #endregion

//    #region DetectStep
//    internal class SheetCheckerStepDetect : SheetCheckerStep
//    {
//        public SheetCheckerStepDetect(AlgorithmParam param)
//        {
//            this.AlgorithmName = "SheetCheckerStepDetect";
//            this.param = param as SheetCheckerParam;
//        }

//        public override string ToString()
//        {
//            return "SheetCheckerStepDetect";
//        }

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();

//            AlgorithmResult algorithmResult = this.CreateAlgorithmResult();

//            SheetCheckerInspectParam inspectParam = algorithmInspectParam as SheetCheckerInspectParam;
//            SheetImageSet inspImageSet = inspectParam.InspImageSet;
//            ProcessBuffer buffer = inspectParam.ProcessBuffer;

//            bool saveImage = ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Image) > 0);
//            DebugContext debugContext = new DebugContext(saveImage, Path.Combine(inspectParam.DebugContext.Path, "Detect"));

//            SheetCheckerParam param = this.param as SheetCheckerParam;
//            TrainerParam trainParam = param.TrainerParam;

//            Rectangle imageRect = new Rectangle(Point.Empty, inspImageSet.GetImageSize());
//            AlgoImage blobImage = buffer.ImageBuffer1;

//            int maxDefectCount = param.MaximumDefectCount;

//            blobImage.Save("blobImage.bmp", debugContext);

//            bool disposNeed = false;
//            if (blobImage.ImageType == ImageType.Gpu)
//            {
//                // Cuda to MIL
//                ImagingLibrary lib = OperationSettings.Instance().ImagingLibrary;
//                AlgoImage newBlobImage = blobImage.ChangeImageType(lib, ImageType.Grey);
//                blobImage = newBlobImage;
//                disposNeed = true;
//            }

//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(blobImage);

//            int fidOffset = inspImageSet.fidXPos - trainParam.FiducialXPos;

//            //foreach (ProjectionRegion region in trainParam.ProjectionRegionList)
//            Parallel.ForEach(trainParam.ProjectionRegionList, (region) =>
//            {
//                Rectangle offsetRect = region.Region;
//                offsetRect.Offset(fidOffset, 0);
//                Rectangle intersectRect = Rectangle.Intersect(offsetRect, imageRect);
//                if (intersectRect.Width == 0 || intersectRect.Height == 0)
//                    return;

//                //if (algorithmResult.SubResultList.Count >= maxDefectCount)
//                //    break;

//                List<Rectangle> inspLineRectList = (List<Rectangle>)inspectParam.SheetCheckerAlgorithmResult.GetResultValue(String.Format("INSP_LINE_RECT_LIST_{0}", region.Id)).Value;

//                AlgoImage baseImage = inspImageSet.GetSubImage(intersectRect);
//                AlgoImage regionBlobImage = blobImage.GetSubImage(intersectRect);

//                List<BlobRect> defectRectList = Process(regionBlobImage, maxDefectCount);
//                regionBlobImage.Dispose();

//                //for (int blobIdx = 0; blobIdx < defectRectList.Count; blobIdx++)
//                //Parallel.ForEach(defectRectList, (defectBlob) =>
//                foreach (BlobRect defectBlob in defectRectList)
//                {
//                    //BlobRect defectBlob = defectRectList[blobIdx];
//                    Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
//                    Size clipRectSize = new Size(128, 128);
//                    Point centerPt = Point.Round(DrawingHelper.CenterPoint(defectRect));

//                    SheetCheckerSubResult subResult = new SheetCheckerSubResult();
//                    subResult.DefectType = CalcDefectType(imageProcessing, baseImage, centerPt, inspLineRectList, trainParam.AutoThresholdValue);
//                    // 극성 파악
//                    // 성형: 255
//                    // 전극: 0
//                    //float realWidth = defectBlob.BoundingRect.Width * MpisInspectorSystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width;
//                    //float realHeight = defectBlob.BoundingRect.Height * MpisInspectorSystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;

//                    switch (subResult.DefectType)
//                    {
//                        case SheetDefectType.BlackDefect:
//                            //if (realWidth < trainParam.BlackDefectMinArea || realHeight < trainParam.BlackDefectMinArea)
//                            continue;
//                            break;
//                        case SheetDefectType.WhiteDefect:
//                            //if (realWidth < trainParam.WhiteDefectMinArea || realHeight < trainParam.WhiteDefectMinArea)
//                            continue;
//                            break;
//                    }

//                    Rectangle clipRect = Rectangle.Round(DrawingHelper.FromCenterSize(centerPt, clipRectSize));
//                    Rectangle adjustClipRect = Rectangle.Intersect(new Rectangle(Point.Empty, baseImage.Size), clipRect);

//                    Rectangle globalDefectRect = defectRect;
//                    globalDefectRect.Offset(intersectRect.Location);
//                    Point globalCenterPt = Point.Add(centerPt, new Size(intersectRect.Location));

//                    //float realX = globalCenterPt.X * MpisInspectorSystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width;
//                    //float realY = globalCenterPt.Y * MpisInspectorSystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;

//                    subResult.Name = "Defect";
//                    subResult.ResultRect = new RotatedRect(globalDefectRect, 0);
//                    subResult.Good = false;
//                    //subResult.X = realX;
//                    //subResult.Y = realY;
//                    //subResult.Width = realWidth;
//                    //subResult.Height = realHeight;
//                    subResult.Area = (int)defectBlob.Area;

//                    Rectangle figureRect = new Rectangle(globalCenterPt, clipRectSize);
//                    figureRect.Offset(-clipRectSize.Width / 2, -clipRectSize.Height / 2);

//                    if (adjustClipRect.Width > 0 && adjustClipRect.Height > 0)
//                    {
//                        AlgoImage clipImage = baseImage.GetSubImage(adjustClipRect);
//                        ImageD image = clipImage.ToImageD();
//                        subResult.Image = image.ToBitmap();
//                        image.Dispose();
//                        clipImage.Dispose();
//                    }
//                    else
//                    {
//                        //Debug.Assert(false, "Clip Image Rect is Wrong");
//                    }

//                    //if (algorithmResult.SubResultList.Count < maxDefectCount)
//                    {
//                        lock (algorithmResult.SubResultList)
//                        {
//                            subResult.Index = algorithmResult.SubResultList.Count;
//                            subResult.BuildMessage();
//                            algorithmResult.SubResultList.Add(subResult);
//                        }
//                    }
//                }

//                baseImage.Dispose();
//                regionBlobImage.Dispose();

//            });

//            if (disposNeed)
//            {
//                blobImage.Dispose();
//            }

//            int detectCount = algorithmResult.SubResultList.Count;
//            if (detectCount == 0)
//            {
//                algorithmResult.Message = "No Defect";
//            }
//            else if (detectCount >= param.MaximumDefectCount)
//            {
//                algorithmResult.Message = "Too Many Defects";
//            }
//            else
//            {
//                algorithmResult.Message = string.Format("{0} Defects", detectCount);
//            }

//            sw.Stop();

//            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan(, sw.ElapsedMilliseconds);
//            algorithmResult.Good = true;
//            return algorithmResult;
//        }

//        private List<BlobRect> Process(AlgoImage algoImage, int maxDefectCount)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
//            List<BlobRect> blobDefectList = new List<BlobRect>();
//            //algoImage.Save("DetectStepAlgoImage.bmp", new DebugContext(true, "d:\\temp\\"));

//            BlobParam blobParam = new BlobParam();
//            blobParam.MaxCount = maxDefectCount;
//            blobParam.SelectArea = true;
//            blobParam.AreaMin = 4;
//            blobParam.SelectCenterPt = true;
//            //blobParam.SelectMeanValue = true;
//            //blobParam.SelectSigmaValue = true;
//            blobParam.SelectBoundingRect = true;
//            //blobParam.SigmaMin = 2;
//            blobParam.EraseBorderBlobs = true;

//            BlobRectList blobRectList = imageProcessing.Blob(algoImage, blobParam);
//            blobRectList.Dispose();

//            RectangleF wholeRect = new RectangleF(0, 0, algoImage.Width, algoImage.Height);
//            List<BlobRect> blobRectList2 = MergeBlobs(blobRectList, wholeRect, 10);
//            blobDefectList.AddRange(blobRectList2);

//            return blobDefectList;
//        }

//        private SheetDefectType CalcDefectType(ImageProcessing imageProcessing, AlgoImage baseImage, Point centerPt, List<Rectangle> inspLineRectList, int AutoThresholdValue)
//        {
//            float meanValue = 0;

//            for (int i = 0; i < inspLineRectList.Count; i++)
//            {
//                if (inspLineRectList[i].Contains(centerPt) == true)
//                {
//                    int yGap;
//                    Rectangle meanClipRect1;
//                    Rectangle meanClipRect2;

//                    if (i == 0)
//                    {
//                        yGap = inspLineRectList[i + 1].Y - inspLineRectList[i].Y;
//                        meanClipRect1 = new Rectangle(centerPt.X, centerPt.Y + yGap, 1, 1);
//                        meanClipRect2 = new Rectangle(centerPt.X, centerPt.Y + (yGap * 2), 1, 1);
//                    }
//                    else if (i == inspLineRectList.Count - 1)
//                    {
//                        yGap = inspLineRectList[i].Y - inspLineRectList[i - 1].Y;
//                        meanClipRect1 = new Rectangle(centerPt.X, centerPt.Y - yGap, 1, 1);
//                        meanClipRect2 = new Rectangle(centerPt.X, centerPt.Y - (yGap * 2), 1, 1);
//                    }
//                    else
//                    {
//                        yGap = inspLineRectList[i + 1].Y - inspLineRectList[i].Y;
//                        meanClipRect1 = new Rectangle(centerPt.X, centerPt.Y + yGap, 1, 1);
//                        meanClipRect2 = new Rectangle(centerPt.X, centerPt.Y - yGap, 1, 1);
//                    }

//                    AlgoImage averageImage1 = baseImage.GetSubImage(meanClipRect1);
//                    AlgoImage averageImage2 = baseImage.GetSubImage(meanClipRect1);

//                    meanValue += imageProcessing.GetGreyAverage(averageImage1);
//                    meanValue += imageProcessing.GetGreyAverage(averageImage2);

//                    averageImage1.Dispose();
//                    averageImage1.Dispose();

//                    break;
//                }
//            }

//            if (meanValue <= AutoThresholdValue)
//                return SheetDefectType.BlackDefect;
//            else
//                return SheetDefectType.WhiteDefect;
//        }

//        private List<BlobRect> MergeBlobs(BlobRectList blobRectList, RectangleF wholeRect, int inflate)
//        {
//            bool merged = true;
//            List<BlobRect> mergeList = blobRectList.GetList();

//            int tryNum = 0;
//            while (merged == true)
//            {
//                merged = false;

//                if (tryNum % 2 == 0)
//                    mergeList = mergeList.OrderBy(defect => defect.BoundingRect.X).ToList();
//                else
//                    mergeList = mergeList.OrderBy(defect => defect.BoundingRect.Y).ToList();

//                for (int srcIndex = 0; srcIndex < mergeList.Count; srcIndex++)
//                {
//                    BlobRect srcBlob = mergeList[srcIndex];

//                    RectangleF inflateRect = srcBlob.BoundingRect;
//                    inflateRect.Inflate(inflate, inflate);

//                    int endSearchIndex = srcIndex;

//                    if (tryNum % 2 == 0)
//                    {
//                        for (int i = endSearchIndex; i < mergeList.Count; i++)
//                        {
//                            if (mergeList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
//                                endSearchIndex = i;
//                            else
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        for (int i = endSearchIndex; i < mergeList.Count; i++)
//                        {
//                            if (mergeList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
//                                endSearchIndex = i;
//                            else
//                                break;
//                        }
//                    }

//                    for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex; destIndex++)
//                    {
//                        BlobRect destBlob = mergeList[destIndex];

//                        if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
//                        {
//                            srcBlob.Area += destBlob.Area;

//                            srcBlob.BoundingRect = RectangleF.Union(srcBlob.BoundingRect, destBlob.BoundingRect);
//                            srcBlob.CenterPt = new PointF((srcBlob.CenterPt.X + destBlob.CenterPt.X) / 2.0f, (srcBlob.CenterPt.Y + destBlob.CenterPt.Y) / 2.0f);

//                            inflateRect = srcBlob.BoundingRect;
//                            inflateRect.Inflate(inflate, inflate);

//                            mergeList.RemoveAt(destIndex);

//                            endSearchIndex--;
//                            destIndex--;

//                            if (tryNum % 2 == 0)
//                            {
//                                for (int i = endSearchIndex; i < mergeList.Count; i++)
//                                {
//                                    if (mergeList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
//                                        endSearchIndex = i;
//                                    else
//                                        break;
//                                }
//                            }
//                            else
//                            {
//                                for (int i = endSearchIndex; i < mergeList.Count; i++)
//                                {
//                                    if (mergeList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
//                                        endSearchIndex = i;
//                                    else
//                                        break;
//                                }
//                            }

//                            if (merged == false)
//                                merged = true;
//                        }
//                    }
//                }

//                if (merged == true)
//                    tryNum++;
//            }

//            return mergeList;
//        }
//    }
//    #endregion

//    #region SaveStep
//    internal class SheetCheckerStepSave : SheetCheckerStep
//    {
//        public SheetCheckerStepSave(AlgorithmParam param)
//        {
//            this.AlgorithmName = "SheetCheckerStepSave";
//            this.param = param as SheetCheckerParam;
//        }

//        public override string ToString()
//        {
//            return "SheetCheckerStepSave";
//        }

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            SheetCheckerParam param = this.param as SheetCheckerParam;
//            SheetCheckerInspectParam inspParam = algorithmInspectParam as SheetCheckerInspectParam;

//            bool saveImage = ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Image) > 0);
//            DebugContext debugContext = new DebugContext(saveImage, Path.Combine(algorithmInspectParam.DebugContext.Path, "Save"));

//            CancellationToken cancellationToken = inspParam.CancellationToken;
//            List<DataExporter> dataExporterList = SystemManager.Instance().DataExporterList;
//            InspectionResult inspectionResult = inspParam.InspectionResult;

//            AlgorithmResult algorithmResult = CreateAlgorithmResult();
//            Stopwatch sw = new Stopwatch();
//            sw.Start();

//            if (string.IsNullOrEmpty(inspectionResult.ResultPath) == false)
//            {
//                if (dataExporterList != null && inspectionResult != null)
//                    foreach (DataExporter dataExporter in dataExporterList)
//                        dataExporter.Export(inspectionResult, cancellationToken);
//            }

//            sw.Stop();
//            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan(sw.ElapsedTicks);
//            return algorithmResult;
//        }
//    }
//    #endregion
//}

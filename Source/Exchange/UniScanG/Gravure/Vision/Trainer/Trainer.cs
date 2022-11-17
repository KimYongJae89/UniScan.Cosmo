using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using System.Drawing.Imaging;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using System.Threading;
using UniScanG.Gravure.Vision.Watcher;

namespace UniScanG.Gravure.Vision.Trainer
{
    public class Trainer : Algorithm
    {
        public static string TypeName { get { return "SheetTrainer"; } }

        BackgroundWorker worker = null;

        public Trainer()
        {
            this.param = new TrainerParam();
        }

        #region Abstract
        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            Trainer clone = new Trainer();
            clone.CopyFrom(this);

            return clone;
        }

        public override void CopyFrom(DynMvp.Vision.Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            Trainer srcAlgorithm = (Trainer)algorithm;
            this.param.CopyFrom(srcAlgorithm.param);
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new System.NotImplementedException();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Teach(BackgroundWorker worker, Image2D currentImage, DoWorkEventArgs args)
        {
            TrainerParam param = this.param as TrainerParam;
            if (param == null)
                return;

            AlgoImage fullImage = null, scaleImage = null;
            this.worker = worker;
            try
            {
                fullImage = ImageBuilder.Build(GetAlgorithmType(), currentImage, ImageType.Grey, ImageBandType.Luminance);
                scaleImage = AlgorithmCommon.GetScaleImage(fullImage);
                
                DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "Trainer"));

                bool doPattern = true;
                bool doRegion = true;

                string argument = args.Argument as string;
                if (string.IsNullOrEmpty(argument) == false)
                {
                    doPattern = (argument == "Pattern");
                    doRegion = (argument == "Region");
                }

                string message = null;
                bool teachDone = true;
                if (doPattern && teachDone)
                    teachDone &= TrainPattern(scaleImage, out message, debugContext);

                if (doRegion && teachDone)
                    teachDone &= TrainRegion(scaleImage, out message, debugContext);

                //param.TeachDate = DateTime.Now;

                args.Result = message;
            }
            catch (OperationCanceledException)
            {
                args.Cancel = true;
            }
            finally
            {
                scaleImage?.Dispose();
                fullImage?.Dispose();
            }
        }

        public bool TrainPattern(AlgoImage trainImage, out string message, DebugContext debugContext)
        {
            TrainerParam param = this.param as TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);
            message = null;

            if (debugContext.SaveDebugImage)
            {
                if (Directory.Exists(debugContext.FullPath))
                    Directory.Delete(debugContext.FullPath, true);
                Directory.CreateDirectory(debugContext.FullPath);
            }

            AlgoImage patternImage = null;
            try
            {
                trainImage.Save("trainImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // 기준위치 파악
                worker.ReportProgress(0, StringManager.GetString(this.GetType().FullName, "Find Base Position"));
                SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
                int basePositionX = sheerFinder.FindBasePosition(trainImage, Direction.Horizontal, 20);
                int basePositionY = sheerFinder.FindBasePosition(trainImage, Direction.Vertical, 20);
                if (basePositionX < 0 || basePositionY < 0)
                {
                    message = "Can not find Base Position";
                    return false;
                }
                Point basePosition = new Point(basePositionX, basePositionY);
                ThrowIfCancellationPending();

                // 이진화 값 찾기
                worker.ReportProgress(25, StringManager.GetString(this.GetType().FullName, "Binalize"));
                int binValue = GetBinValue(trainImage);
                if (binValue == 0)
                {
                    message = "Can not find Pattern Threshold Value";
                    return false;
                }
                param.BinValue = binValue;
                ThrowIfCancellationPending();

                // 보조영상 생성
                worker.ReportProgress(50, StringManager.GetString(this.GetType().FullName, "Analyze Pattern"));
                patternImage = ImageBuilder.Build(trainImage); // 주 패턴만 하얗게 칠한 영상
                CreateMaskImage(trainImage, patternImage);
                patternImage.Save("patternImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // 패턴 찾기. 패턴 그룹 설정. 주 패턴 선정
                worker.ReportProgress(75, StringManager.GetString(this.GetType().FullName, "Grouping Patterns"));
                DebugContext patternGroupDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "PatternGroup"));
                List<SheetPatternGroup> patternGroupList = FindPatternGroup(worker, trainImage, patternImage, patternGroupDebugContext);

                CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.CalculatorBase.TypeName).Param as CalculatorParam;
                if (calculatorParam != null)
                {
                    // 이전 Base Position과 티칭 후 BasePosition 확인
                    Point oldBasePosition = calculatorParam.BasePosition;
                    if (!oldBasePosition.IsEmpty)
                    {
                        // Watch Item을 오프셋 만큼 이동
                        Point offset = Point.Subtract(basePosition, (Size)oldBasePosition);
                        WatcherParam watcherParam = AlgorithmPool.Instance().GetAlgorithm(Watcher.Watcher.TypeName).Param as WatcherParam;
                        for (int i = 0; i < watcherParam.WatchItemList.Count; i++)
                            watcherParam.WatchItemList[i].Offset(offset);
                    }

                    // 티칭 값 적용
                    calculatorParam.BinValue = binValue;
                    calculatorParam.BasePosition = basePosition;
                    calculatorParam.SheetSize = trainImage.Size;
                    calculatorParam.PatternGroupList = patternGroupList;
                    calculatorParam.RegionInfoList.ForEach(f => f.Dispose());
                    calculatorParam.RegionInfoList.Clear();
                }

                ThrowIfCancellationPending();

                worker.ReportProgress(100, StringManager.GetString(this.GetType().FullName, "Done"));
                Thread.Sleep(1000);

                return true;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                message = ex.Message;
                return false;
            }
#endif
            finally
            {
                patternImage?.Dispose();
            }
        }

        private void ThrowIfCancellationPending()
        {
            if(worker!=null && worker.CancellationPending)
                throw new OperationCanceledException();
        }

        public AlgoImage GetMajorPatternImage(AlgoImage trainImage)
        {
            AlgoImage patternImage = null, majorPatternImage = null;
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.CalculatorBase.TypeName).Param as CalculatorParam;
            try
            {
                patternImage = ImageBuilder.Build(trainImage);
                this.CreateMaskImage(trainImage, patternImage);

                // 주 패턴만 추출한 영상을 만듬
                majorPatternImage = ImageBuilder.Build(patternImage);
                List<SheetPatternGroup> majorPatternGroupList = calculatorParam.PatternGroupList.FindAll(f => f.Use);
                DrawPatternGroup(patternImage, majorPatternGroupList, majorPatternImage);
                return majorPatternImage;
            }
            finally
            {
                patternImage?.Dispose();
            }
        }

        public bool TrainRegion(AlgoImage trainImage, out string message, DebugContext debugContext)
        {
            TrainerParam param = this.param as TrainerParam;
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.CalculatorBase.TypeName).Param as CalculatorParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);
            message = null;
            AlgoImage patternImage = null, majorPattrnImage = null;

            try
            {
                // 선택한 주 패턴들
                List<SheetPatternGroup> majorPatternGroupList = calculatorParam.PatternGroupList.FindAll(f => f.Use);
                ThrowIfCancellationPending();

                // 패턴 이미지 가져옴. TrainPattern을 하지 않았으면 새로 만듬.
                worker.ReportProgress(0, StringManager.GetString(this.GetType().FullName, "Generate Pattern Image"));
                if (patternImage == null)
                {
                    patternImage = ImageBuilder.Build(trainImage);
                    this.CreateMaskImage(trainImage, patternImage);
                }
                patternImage.Save("patternImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // 주 패턴만 추출한 영상을 만듬
                worker.ReportProgress(25, StringManager.GetString(this.GetType().FullName, "Extract Major Pattern"));
                majorPattrnImage = ImageBuilder.Build(patternImage);
                DrawPatternGroup(patternImage, majorPatternGroupList, majorPattrnImage);
                majorPattrnImage.Save("majorPattrnImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // Conture 따기
                worker.ReportProgress(50, StringManager.GetString(this.GetType().FullName, "Build Contour Image"));
                ThrowIfCancellationPending();

                // 버 생성
                worker.ReportProgress(75, StringManager.GetString(this.GetType().FullName, "Generate Inspect Region Infomation"));
                List<RegionInfoG> roiRegionInfoList = FindRegionInfo(trainImage, majorPattrnImage, majorPatternGroupList, debugContext);
                if (roiRegionInfoList.Count > 0)
                {
                    float averageInspRegionCnt = (float)roiRegionInfoList.Average(f => f.InspectElementList.Count);
                    roiRegionInfoList.ForEach(f => f.Use = f.InspectElementList.Count > averageInspRegionCnt / 2);
                }

                if (debugContext.SaveDebugImage)
                {
                    AlgoImage debugGrayImage = ImageBuilder.Build(trainImage.LibraryType, trainImage.ImageType, trainImage.Width / 2, trainImage.Height / 2);
                    imageProcessing.Resize(trainImage, debugGrayImage);
                    //AlgoImage debugColorImage = debugGrayImage.ConvertToMilImage(ImageType.Color);
                    ////debugColorImage.Save(@"d:\tt.bmp");

                    //roiRegionInfoList.ForEach(f =>
                    //{
                    //    Rectangle regionRect = Rectangle.Round(f.Region);
                    //    regionRect.X /= 2;
                    //    regionRect.Y /= 2;
                    //    regionRect.Width /= 2;
                    //    regionRect.Height /= 2;
                    //    imageProcessing.DrawRect(debugColorImage, regionRect, (double)Color.Red.ToArgb(), false);

                    //    AlgoImage subDebugImage = debugColorImage.GetSubImage(regionRect);
                    //    foreach (Rectangle rect in f.AdjPatRegionList)
                    //    {
                    //        Rectangle lineRect = rect;
                    //        lineRect.X /= 2;
                    //        lineRect.Y /= 2;
                    //        lineRect.Width /= 2;
                    //        lineRect.Height /= 2;
                    //        imageProcessing.DrawRect(subDebugImage, lineRect, (double)Color.Yellow.ToArgb(), false);
                    //    }

                    //    subDebugImage.Dispose();
                    //});

                    //debugColorImage.Save("RoiRegions.bmp", debugContext);
                    //debugColorImage.Dispose();
                    debugGrayImage.Dispose();
                }

                //CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.Calculator.TypeName).Param as CalculatorParam;
                calculatorParam.RegionInfoList = roiRegionInfoList;

                DetectorParam detectorParam = AlgorithmPool.Instance().GetAlgorithm(Detector.Detector.TypeName).Param as DetectorParam;
                ThrowIfCancellationPending();

                worker.ReportProgress(100, StringManager.GetString(this.GetType().FullName, "Done"));
                Thread.Sleep(1000);

                return true;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                message = ex.Message;
                return false;
            }
#endif
            finally
            {
                patternImage?.Dispose();
                majorPattrnImage?.Dispose();
            }
        }

        private void DrawContureImage(AlgoImage patternImage, List<SheetPatternGroup> majorPatternGroupList, AlgoImage contureImage, DebugContext debugContext)
        {
            TrainerParam trainerParam = this.param as TrainerParam;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(patternImage);
            contureImage.Clear();

            AlgoImage weightedAddImage = null;
            List<AlgoImage> eachPatternImageList = new List<AlgoImage>();
            try
            {
                for (int i = 0; i < majorPatternGroupList.Count; i++)
                {
                    SheetPatternGroup majorPatternGroup = majorPatternGroupList[i];
                    int maxW = (int)(majorPatternGroup.PatternList.Max(g => g.BoundingRect.Width) * 1.5f);
                    int maxH = (int)(majorPatternGroup.PatternList.Max(g => g.BoundingRect.Height) * 1.5f);
                    Point dstCenter = new Point(maxW / 2, maxH / 2);
                    for (int j = 0; j < majorPatternGroup.PatternList.Count; j++)
                    {
                        ThrowIfCancellationPending();

                        BlobRect pattern = majorPatternGroup.PatternList[j];
                        AlgoImage eachPatternImage = ImageBuilder.Build(contureImage.LibraryType, contureImage.ImageType, maxW, maxH);

                        Rectangle srcRect = Rectangle.Round(pattern.BoundingRect);
                        //srcRect.Inflate(5, 5);
                        //AlgoImage singlePatternImage = patternImage.GetSubImage(srcRect);
                        //singlePatternImage.Save(string.Format("SinglePatternImage_{0}_{1}.bmp", i, j), debugContext);
                        //singlePatternImage.Dispose();

                        Rectangle dstRect = DrawingHelper.FromCenterSize(dstCenter, srcRect.Size);
                        eachPatternImage.Copy(patternImage, srcRect.Location, dstRect.Location, srcRect.Size);
                        //eachPatternImage.Save(string.Format("EachPatternImage_{0}_{1}.bmp", i, j), debugContext);

                        eachPatternImageList.Add(eachPatternImage);
                    }

                    weightedAddImage = ImageBuilder.Build(contureImage.LibraryType, contureImage.ImageType, maxW, maxH);
                    ip.WeightedAdd(eachPatternImageList.ToArray(), weightedAddImage);
                    weightedAddImage.Save(string.Format("{0}_0_weightedAdd.bmp", i), debugContext);
                    for (int w = 0; w < trainerParam.EdgeAverage; w++)
                        ip.Average(weightedAddImage);
                    weightedAddImage.Save(string.Format("{0}_1_Average.bmp", i), debugContext);
                    ip.Sobel(weightedAddImage);
                    weightedAddImage.Save(string.Format("{0}_2_Sobel.bmp", i), debugContext);

                    if (trainerParam.EdgeDilate.Width > 0)
                    {
                        int[,] kernel = new int[,] { { -1, -1, -1 }, { 1, 1, 1 }, { -1, -1, -1 } };
                        ip.Morphology(weightedAddImage, weightedAddImage, ImageProcessing.MorphologyType.Dilate, kernel, trainerParam.EdgeDilate.Width, true);
                    }
                    weightedAddImage.Save(string.Format("{0}_3_DilateW.bmp", i), debugContext);
                    if (trainerParam.EdgeDilate.Height > 0)
                    {
                        int[,] kernel = new int[,] { { -1, 1, -1 }, { -1, 1, -1 }, { -1, 1, -1 } };
                        ip.Morphology(weightedAddImage, weightedAddImage, ImageProcessing.MorphologyType.Dilate, kernel, trainerParam.EdgeDilate.Height, true);
                    }
                    //ip.Dilate(weightedAddImage, weightedAddImage, 3, 3, true);
                    weightedAddImage.Save(string.Format("{0}_4_DilateH.bmp", i), debugContext);
                    //ip.Mul(weightedAddImage, weightedAddImage, 2);
                    //weightedAddImage.Save(string.Format("{0}_4_weightedAddMultiply.bmp", i), debugContext);
                    ip.Clipping(weightedAddImage, 20, 140);
                    weightedAddImage.Save(string.Format("{0}_5_Clipping.bmp", i), debugContext);

                    Rectangle imageRect = new Rectangle(Point.Empty, contureImage.Size);

                    for (int j = 0; j < majorPatternGroup.PatternList.Count; j++)
                    {
                        BlobRect pattern = majorPatternGroup.PatternList[j];
                        Size patternSize = Size.Round(pattern.BoundingRect.Size);
                        Size drawSize = weightedAddImage.Size;

                        Rectangle srcRect = new Rectangle(Point.Empty, drawSize);

                        Size offset = new Size((drawSize.Width - patternSize.Width) / 2, (drawSize.Height - patternSize.Height) / 2);
                        Rectangle dstRect = srcRect;
                        dstRect.Offset(Point.Round(PointF.Subtract(pattern.BoundingRect.Location, offset)));
                        Rectangle adjustDstRect = Rectangle.Intersect(dstRect, imageRect);
                        //Size realDrawSize = adjustDstRect.Size;

                        AlgoImage subContourImage = contureImage.GetSubImage(adjustDstRect);
                        ip.Add(subContourImage, weightedAddImage, subContourImage);
                        subContourImage.Dispose();
                        //if(j%10==0)
                        //contureImage.Save(@"d:\temp\tt.bmp");

                        //contureImage.Copy(weightedAddImage, srcRect.Location, dstRect.Location, realDrawSize);

                    }

                    eachPatternImageList.ForEach(f => f.Dispose());
                    eachPatternImageList.Clear();
                    weightedAddImage.Dispose();
                    weightedAddImage = null;
                    //contureImage.Save(@"d:\temp\tt.bmp");
                }
            }
            finally
            {
                eachPatternImageList.ForEach(f => f.Dispose());
                eachPatternImageList.Clear();
                weightedAddImage?.Dispose();
                weightedAddImage = null;
            }
            //contureImage.Save("contureImage.bmp", debugContext);

        }

        private void DrawPatternGroup(AlgoImage patternImage, List<SheetPatternGroup> majorPatternGroupList, AlgoImage majorPattrnImage)
        {
            majorPattrnImage.Clear();
            foreach (SheetPatternGroup majorPatternGroup in majorPatternGroupList)
            {
                foreach (BlobRect blobRect in majorPatternGroup.PatternList)
                {
                    Point pt = Point.Round(blobRect.BoundingRect.Location);
                    Size sz = Size.Round(blobRect.BoundingRect.Size);
                    majorPattrnImage.Copy(patternImage, pt, pt, sz);
                }
            }
        }

        private List<SheetPatternGroup> FindPatternGroup(BackgroundWorker worker, AlgoImage trainImage, AlgoImage patternImage, DebugContext debugContext)
        {
            List<PatternInfo> patternInfoList = FindPattern(worker, patternImage, debugContext);

            if (debugContext.SaveDebugImage)
            {
                Directory.CreateDirectory(Path.Combine(debugContext.FullPath, "Pattern"));
                StringBuilder sb = new StringBuilder();
                PatternInfo[] patternInfos = patternInfoList.ToArray();
                sb.AppendLine("i,Area,Width,Hetght,MinFeret,MaxFeret,CFR");
                for (int i = 0; i < patternInfos.Length; i++)
                {
                    PatternInfo f = patternInfos[i];
                    sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", i, f.Area, f.BoundingRect.Width, f.BoundingRect.Height, f.MinFeretDiameter, f.MaxFeretDiameter, f.WaistRatio));
                    AlgoImage debugSave = trainImage.Clip(Rectangle.Round(f.BoundingRect));
                    debugSave.Save(Path.Combine(debugContext.FullPath, "Pattern", string.Format("Pattern_{0}.bmp", i)));
                    debugSave.Dispose();
                }
                File.WriteAllText(Path.Combine(debugContext.FullPath, @"pattern.csv"), sb.ToString());
            }

            List<SheetPatternGroup> sheetPatternGroupList = GetPatternGroup(worker, trainImage, patternImage, patternInfoList);
            sheetPatternGroupList.Sort((f, g) => (g.CountRatio).CompareTo(f.CountRatio));
            sheetPatternGroupList.ForEach(f => f.Use = (f.CountRatio > 0.4));
            //sheetPatternGroupList.Sort((f, g) => (g.CountRatio*g.AverageArea).CompareTo(f.CountRatio * f.AverageArea));

            if (debugContext.SaveDebugImage)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("group,pattern,CenterX,CenterY,Area,Width,Hetght,MinFeret,MaxFeret,CFR");
                AlgoImage debugImage = ImageBuilder.Build(patternImage);
                for (int i = 0; i < sheetPatternGroupList.Count; i++)
                {
                    debugImage.Clear();
                    for (int j=0; j< sheetPatternGroupList[i].PatternList.Count; j++ )
                    {
                        PatternInfo f = sheetPatternGroupList[i].PatternList[j];
                        PointF center = DrawingHelper.CenterPoint(f.BoundingRect);
                        sb.AppendLine(string.Format("{0},{1:F1},{2:F1},{3},{4},{5},{6},{7},{8},{9}", i, j, center.X, center.Y, f.Area, f.BoundingRect.Width, f.BoundingRect.Height, f.MinFeretDiameter, f.MaxFeretDiameter, f.WaistRatio));
                        Point pt = Point.Round(f.BoundingRect.Location);
                        Size sz = Size.Round(f.BoundingRect.Size);
                        debugImage.Copy(patternImage, pt, pt, sz);
                    }
                    debugImage.Save(Path.Combine(debugContext.FullPath, "PatterGroup", string.Format("SheetPatterGroup_{0}_C{1}.bmp", i, sheetPatternGroupList[i].NumPattern)));
                }
                debugImage.Dispose();
                File.WriteAllText(Path.Combine(debugContext.FullPath, @"patternGroup.csv"), sb.ToString());
            }
            return sheetPatternGroupList;
        }

        public void CreateMaskImage(AlgoImage trainImage, AlgoImage patternImage)
        {
            TrainerParam param = this.param as TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);

            AlgoImage binImage = null;
            BlobRectList patternBlobRectList = null;
            try
            {
                // 이진화 영상
                int binValue = param.BinValue + param.BinValueOffset;
                binValue = Math.Max(0, binValue);
                binValue = Math.Min(255, binValue);

                binImage = ImageBuilder.Build(trainImage);
                imageProcessing.Binarize(trainImage, binImage, binValue, true);

                // 보조영상 생성
                BlobParam patternBlobParam = new BlobParam();
                patternBlobParam.SelectArea = true;
                patternBlobParam.AreaMin = param.MinPatternArea;

                patternBlobRectList = imageProcessing.Blob(binImage, patternBlobParam);

                if (patternImage != null)
                {
                    DrawBlobOption drawPatternBlobOption = new DrawBlobOption();
                    drawPatternBlobOption.SelectBlob = true;
                    imageProcessing.DrawBlob(patternImage, patternBlobRectList, null, drawPatternBlobOption);
                    imageProcessing.FillHoles(patternImage, patternImage);
                    //patternImage.Save(@"d:\temp\patternImage.bmp");
                    imageProcessing.Open(patternImage, 3);
                    //patternImage.Save(@"d:\temp\patternImage2.bmp");
                }
            }
            finally
            {
                patternBlobRectList.Dispose();
                binImage?.Dispose();
            }
        }

        public List<PatternInfo> FindPattern(BackgroundWorker worker, AlgoImage patternImage, DebugContext debugContext)
        {
            TrainerParam param = this.param as TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(patternImage);

            BlobParam patternBlobParam = new BlobParam
            {
                AreaMin = param.MinPatternArea,
                EraseBorderBlobs = true,
                SelectCenterPt = true,
                SelectCompactness = true,
                SelectFeretDiameter = true,
                SelectSawToothArea= true
            };

            BlobRectList patternBlobRectList = imageProcessing.Blob(patternImage, patternBlobParam);
            List<PatternInfo> patternList = patternBlobRectList.GetList().ConvertAll<PatternInfo>(f => new PatternInfo(f));
            Task.Run(new Action(() => patternBlobRectList.Dispose()));

            // 허리부분 길이 재기??
            foreach (PatternInfo patternInfo in patternList)
            {
                Rectangle subRect = Rectangle.Round(patternInfo.BoundingRect);

                AlgoImage temp = patternImage.GetSubImage(subRect);

                subRect.Inflate(0, -(subRect.Height - 1) / 2);
                subRect.Inflate(0, 1);

                PointF centerPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, subRect.Size));
                AlgoImage patternSubImage = patternImage.GetSubImage(subRect);
                patternBlobParam.EraseBorderBlobs = false;
                BlobRectList waistBlobRectList = imageProcessing.Blob(patternSubImage, patternBlobParam);
                List<BlobRect> waistBlobList = waistBlobRectList.GetList();
                waistBlobRectList.Dispose();

                //if (waistBlobList.Count > 0)
                // 중심을 포함하는 Blob의 Width 비율
                {
                    BlobRect centerBlobRect = waistBlobList.Find(f => f.BoundingRect.Contains(centerPt));
                    if(centerBlobRect!=null)
                        patternInfo.WaistRatio = (int)(waistBlobList.Max(f => f.BoundingRect.Width) * 100 / patternInfo.BoundingRect.Width);
                }
                patternSubImage.Dispose();

                temp.Dispose();
            }

            return patternList;
        }

        private List<SheetPatternGroup> GetPatternGroup(BackgroundWorker worker, AlgoImage trainImage, AlgoImage patternImage, List<PatternInfo> patternList)
        {
            TrainerParam param = this.param as TrainerParam;

            patternList.Sort((f, g) => f.Area.CompareTo(g.Area));
            SheetPatternGroupG curSheetPatternGroup = new SheetPatternGroupG(patternList);

            List<SheetPatternGroup> sheetPatternGroupList = curSheetPatternGroup.DivideSubGroup(param.SheetPatternGroupThreshold);

            List<SheetPatternGroup> sheetPatternGroupList2 = new List<SheetPatternGroup>();
            while (sheetPatternGroupList.Count > 0)
            {
                ThrowIfCancellationPending();
                SheetPatternGroup searchSheetPatternGroup = sheetPatternGroupList[0];
                sheetPatternGroupList.RemoveAt(0);

                List<SheetPatternGroup> samePatternGroupList = sheetPatternGroupList.FindAll(f =>
                 {
                     if (f == null)
                         return false;

                     return IsSamePattern(f.GetAverageBlobRect(), searchSheetPatternGroup, param.SheetPatternGroupThreshold, 1.0f, 1.0f);
                 });
                sheetPatternGroupList.RemoveAll(f => samePatternGroupList.Contains(f));

                foreach (SheetPatternGroup samePatternGroup in samePatternGroupList)
                    searchSheetPatternGroup.Merge(samePatternGroup);

                if (searchSheetPatternGroup.PatternList.Count() > 3)
                {
                    searchSheetPatternGroup.CountRatio = searchSheetPatternGroup.PatternList.Count() * 1.0f / patternList.Count;
                    searchSheetPatternGroup.UpdateMaterImage(trainImage);
                    sheetPatternGroupList2.Add(searchSheetPatternGroup);
                }
            }

            return sheetPatternGroupList2;
        }

        private List<RegionInfoG> FindRegionInfo(AlgoImage trainImage, AlgoImage patternImage, List<SheetPatternGroup> majorSheetPatternGroupList, DebugContext debugContext)
        {
            //가정: 인접한 패턴간의 거리(d_w,d_h)는 현재 패턴의 크기(w,h)의 N배보다 가깝다.
            // d_w < N*w, d_h < N*h
            const int N = 2;

            List<RegionInfoG> regionInfoList = new List<RegionInfoG>();
            Rectangle imageRect = new Rectangle(Point.Empty, patternImage.Size);

            List<BlobRect> blobRectList = new List<BlobRect>();
            majorSheetPatternGroupList.ForEach(f => blobRectList.AddRange(f.PatternList));
            blobRectList = blobRectList.OrderBy(f => MathHelper.GetLength(Point.Empty, f.BoundingRect.Location)).ToList();

            while (blobRectList.Count > 0)
            {
                List<BlobRect> nearBlobRectList = new List<BlobRect>(); // 인접하여 그룹으로 묶인 블랍 목록
                List<BlobRect> searchBlobRectList = new List<BlobRect>();   // 탐색을 할블랍 목록

                searchBlobRectList.Add(blobRectList[0]);
                while (searchBlobRectList.Count > 0)
                {
                    BlobRect baseBlobRect = searchBlobRectList[0];
                    searchBlobRectList.RemoveAt(0);
                    nearBlobRectList.Add(baseBlobRect);
                    blobRectList.Remove(baseBlobRect);

                    RectangleF baseRectF = baseBlobRect.BoundingRect;
                    baseRectF.Inflate(baseRectF.Width * N / 2, baseRectF.Height * N / 2);
                    List<BlobRect> foundBlobRectList = blobRectList.FindAll(f =>
                    {
                        return baseRectF.IntersectsWith(f.BoundingRect);
                    });

                    foundBlobRectList.RemoveAll(f => nearBlobRectList.Contains(f) || searchBlobRectList.Contains(f));

                    searchBlobRectList.AddRange(foundBlobRectList);
                }

                if (nearBlobRectList.Count > 10)
                {
                    RectangleF rectangleF = nearBlobRectList[0].BoundingRect;
                    nearBlobRectList.ForEach(f => rectangleF = RectangleF.Union(rectangleF, f.BoundingRect));
                    float inflate = Math.Max(rectangleF.Width / 50.0f, rectangleF.Height / 50.0f); //2%씩 늘림
                    rectangleF.Inflate(inflate, inflate);
                    rectangleF.Intersect(imageRect);
                    Rectangle regionRect = Rectangle.Round(rectangleF);
                    regionRect.Width = regionRect.Width / 4 * 4;

                    AlgoImage regionTrainImage = null, regionPatternImage = null;
                    try
                    {
                        regionTrainImage = trainImage.GetSubImage(regionRect);
                        regionPatternImage = patternImage.GetSubImage(regionRect);
                        string debugSubPath = string.Format("RegionInfoG_{0}", regionInfoList.Count);
                        DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, debugSubPath));
                        RegionInfoG regionInfo = GetRegionInfo(regionRect, regionTrainImage, regionPatternImage, newDebugContext);

                        if (regionInfo != null)
                            regionInfoList.Add(regionInfo);
                    }
                    finally
                    {
                        regionPatternImage?.Dispose();
                        regionTrainImage?.Dispose();
                    }
                }
            }
            
            return regionInfoList;
        }

        /// <summary>
        /// 버 정보를 생성한다.
        /// </summary>
        /// <param name="regionRect"></param>
        /// <param name="regionPatternImage"></param>
        /// <param name="debugContext"></param>
        /// <returns></returns>
        private RegionInfoG GetRegionInfo(Rectangle regionRect, AlgoImage regionTrainImage, AlgoImage regionPatternImage, DebugContext debugContext)
        {
            TrainerParam trainerParam = this.param as TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(regionPatternImage);

            regionPatternImage.Save("0 regionPatternImage.bmp", debugContext);
            Rectangle patternImageRect = new Rectangle(Point.Empty, regionPatternImage.Size);

            AlgoImage regionAvgImage = regionPatternImage.Clone();
            imageProcessing.Erode(regionAvgImage, 5);
            float poleAvg = imageProcessing.GetGreyAverage(regionTrainImage, regionAvgImage);

            imageProcessing.Not(regionPatternImage, regionAvgImage);
            imageProcessing.Erode(regionAvgImage, 5);
            float dielectricAvg = imageProcessing.GetGreyAverage(regionTrainImage, regionAvgImage);
            regionAvgImage.Dispose();

            // 패턴 주기 파악
            Rectangle[,] rectangles = GetSubRegion(regionPatternImage, debugContext);
            if (rectangles == null || rectangles.GetLength(0) <= 1 || rectangles.GetLength(1)<=1)
                return null;

            Rectangle[,] adjustSubRegion = AdjustRect(rectangles, debugContext);

            // 썸네일 이미지 제작
            float scaleFactor = 512.0f / Math.Max(regionPatternImage.Width, regionPatternImage.Height);
            Size scaledSize = Size.Round(new SizeF(regionPatternImage.Width * scaleFactor, regionPatternImage.Height * scaleFactor));
            AlgoImage scaledImage = ImageBuilder.Build(Trainer.TypeName, ImageType.Grey, scaledSize.Width, scaledSize.Height);
            imageProcessing.Resize(regionPatternImage, scaledImage);

            RegionInfoG regionInfo = new RegionInfoG(scaledImage.ToImageD(), regionRect, rectangles, adjustSubRegion, poleAvg, dielectricAvg);
            regionInfo.SetSkipRegion(regionPatternImage);

            regionInfo.BuildInspRegion(regionTrainImage.ToImageD(), regionPatternImage.ToImageD(), trainerParam.SplitLineDirection, trainerParam.IsCrisscross);

            scaledImage.Dispose();

            if (debugContext.SaveDebugImage)
            {
                AlgoImage debugGrayImage = ImageBuilder.Build(regionTrainImage.LibraryType, regionTrainImage.ImageType, regionTrainImage.Width / 2, regionTrainImage.Height / 2);
                imageProcessing.Resize(regionTrainImage, debugGrayImage);
                //AlgoImage debugColorImage = debugGrayImage.ConvertToMilImage(ImageType.Color);

                //Rectangle drawRect = new Rectangle(0, 0, regionRect.Width, regionRect.Height);
                //drawRect.X /= 2;
                //drawRect.Y /= 2;
                //drawRect.Width /= 2;
                //drawRect.Height /= 2;
                //imageProcessing.DrawRect(debugColorImage, drawRect, (double)Color.LightGreen.ToArgb(), false);

                //foreach (Rectangle rect in regionInfo.AdjPatRegionList)
                //{
                //    Rectangle lineRect = rect;
                //    lineRect.X /= 2;
                //    lineRect.Y /= 2;
                //    lineRect.Width /= 2;
                //    lineRect.Height /= 2;
                //    imageProcessing.DrawRect(debugColorImage, lineRect, (double)Color.Yellow.ToArgb(), false);
                //}

                //foreach (Point pt in regionInfo.DontcareLocationList)
                //{
                //    Rectangle skipRect = regionInfo.AdjPatRegionList[pt.Y, pt.X];
                //    skipRect.X /= 2;
                //    skipRect.Y /= 2;
                //    skipRect.Width /= 2;
                //    skipRect.Height /= 2;
                //    imageProcessing.DrawRect(debugColorImage, skipRect, (double)Color.FromArgb(128, Color.Red).ToArgb(), true);
                //}

                //debugColorImage.Save(Path.Combine(debugContext.FullPath, "RoiRegions.bmp"));
                //debugColorImage.Dispose();
                debugGrayImage.Dispose();
            }

            return regionInfo;
        }

        /// <summary>
        /// 패턴이 속하는 Grid 생성.
        /// </summary>
        /// <param name="regionPatternImage"></param>
        /// <param name="debugContext"></param>
        /// <returns></returns>
        private Rectangle[,] GetSubRegion(AlgoImage regionPatternImage, DebugContext debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionPatternImage);
            Rectangle imageRect = new Rectangle(Point.Empty, regionPatternImage.Size);

            // 중앙영역 Projection. 패턴 주기 파악
            float[][] projData = new float[2][];
            for (int i = 0; i < 2; i++)
            // i==0 -> 중앙 가로방향 사각형 이미지. 상->하로 투영
            // i==1 -> 중앙 세로방향 사각형 이미지. 좌->우로 투영
            {
                int projRegionW = regionPatternImage.Width / (i == 0 ? 1 : 5);
                int projRegionH = regionPatternImage.Height / (i == 0 ? 5 : 1);

                Rectangle projRegion = new Rectangle(Point.Empty, regionPatternImage.Size);
                projRegion.Inflate(-(regionPatternImage.Width - projRegionW) / 2, -(regionPatternImage.Height - projRegionH) / 2);

                if (i == 2)
                    projRegion.X = 0;
                else if (i == 3)
                    projRegion.X = regionPatternImage.Width - projRegion.Width;

                AlgoImage projImage = regionPatternImage.GetSubImage(projRegion);
                projImage.Save(string.Format("0 projImage_{0}.bmp", i), debugContext);

                projData[i] = ip.Projection(projImage, i == 1 ? Direction.Vertical : Direction.Horizontal, ProjectionType.Mean);

                //StringBuilder sb = new StringBuilder();
                //Array.ForEach(projData[i], f => sb.AppendLine(f.ToString()));
                //File.WriteAllText(string.Format(@"d:\temp\projData{0}.txt", i), sb.ToString());

                projImage.Dispose();
            }
            
            // SubRegion 찾기
            Rectangle[,] subRegions = GetSubRegion(projData[0], projData[1], debugContext);

            int h = subRegions.GetLength(0);
            int w = subRegions.GetLength(1);
            // 비틀림 보정
            if(h>1 && w>1)
            {
                TrainerParam trainerParam = param as TrainerParam;
                Size projRectSize = new Size(regionPatternImage.Width / 5, regionPatternImage.Height);
                List<Point>[] hillLists = new List<Point>[2];
                for (int i = 0; i < 2; i++)
                {
                    Point projRectLoc = new Point((i == 0 ? 0 : regionPatternImage.Width - projRectSize.Width), 0);
                    Rectangle projRegion = new Rectangle(projRectLoc.X, projRectLoc.Y, projRectSize.Width, projRectSize.Height);
                    AlgoImage projImage = regionPatternImage.GetSubImage(projRegion);
                    float[] sizeProj = ip.Projection(projImage, Direction.Vertical, ProjectionType.Mean);
                    projImage.Dispose();

                    if (trainerParam.IsCrisscross)
                        hillLists[i] = AlgorithmCommon.FindHill3(sizeProj, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold);
                    else
                        hillLists[i] = AlgorithmCommon.FindHill(sizeProj, trainerParam.MinLineIntensity);
                }

                //regionPatternImage.Save(@"d:\temp\tt.bmp");
                if (hillLists[0].Count == hillLists[1].Count && hillLists[0].Count>0)
                {
                    float offset = hillLists[1][0].X - hillLists[0][0].X;
                    for (int x = 0; x < subRegions.GetLength(1); x++)
                    {
                        int subOffset = (int)Math.Round(offset * x / subRegions.GetLength(1) - offset / 2);
                        for (int y = 0; y < subRegions.GetLength(0); y++)
                        {
                            Rectangle subRegion = subRegions[y, x];
                            subRegion.Offset(0, subOffset);
                            subRegions[y, x] = subRegion;
                        }
                    }
                }
            }

            // 이미지 영역을 벗어나면 잘라냄
            for (int i = 0; i < subRegions.Length; i++)
            {
                Rectangle rect = subRegions[i / w, i % w];
                subRegions[i / w, i % w] = Rectangle.Intersect(rect, imageRect);
            }

            if (subRegions.Length == 0)
                return null;

            return subRegions;
        }

        private bool FindCrisscross(float[] v)
        {
            //지그제그 패턴: Y방향 프로젝션이 최소 - 최대 모양이 아니다.
            float maxVal = v.Max();
            float minxVal = v.Min();
            float bound = Math.Min(20, (maxVal - minxVal) / 2);

            int totalLength = v.Length;
            int maxLength = Array.FindAll(v, f => f > maxVal - 20).Length;
            int minLength = Array.FindAll(v, f => f < minxVal + 20).Length;
            float rate = (maxLength + minLength) * 1f / totalLength;
            return rate < 0.9f;
        }

        /// <summary>
        ///  커널을 써보자
        /// </summary>
        /// <param name="regionPatternImage"></param>
        /// <param name="debugContext"></param>
        /// <returns></returns>
        //private Rectangle[,] GetSubRegion2(AlgoImage regionPatternImage, List<Rectangle> patRectList, DebugContext debugContext)
        //{
        //    TrainerParam param = this.param as TrainerParam;
        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionPatternImage);
        //    Rectangle imageRect = new Rectangle(Point.Empty, regionPatternImage.Size);

        //    patRectList.Sort((f, g) => (f.Left + f.Right).CompareTo(g.Left + g.Right));

        //    List<List<Rectangle>> splitBlobRectList = new List<List<Rectangle>>();
        //    List<Rectangle> splitBlobRect = new List<Rectangle>();

        //    Point searchBasePoint = new Point(patRectList[0].Left, patRectList[0].Right);
        //    patRectList.ForEach(f =>
        //    {
        //        // Base Point에 걸쳐있다.
        //        if ((f.Left < searchBasePoint.Y && f.Right> searchBasePoint.X) == false)
        //        {
        //            splitBlobRectList.Add(splitBlobRect);
        //            splitBlobRect = new List<Rectangle>();
        //            searchBasePoint = new Point(f.Left, f.Right);
        //        }
        //        splitBlobRect.Add(f);
        //    });

        //    if (splitBlobRectList.Count == 0)
        //        return null;

        //    Rectangle[,] subRegion = new Rectangle[splitBlobRectList.Max(f => f.Count), splitBlobRectList.Count];
        //    for (int i = 0; i < splitBlobRectList.Count; i++)
        //        for (int j = 0; j < splitBlobRectList[i].Count; j++)
        //            subRegion[j, i] = splitBlobRectList[i][j];

        //    return subRegion;
        //}

        private Rectangle[,] GetSubRegion(float[] projDataX, float[] projDataY, DebugContext debugContext)
        {
            List<Point>[] hillLists = new List<Point>[2];
            List<Point> hillListX, hillListY;

            // White Area의 시작점/끝점을 찾음
            DebugContext hillListXDebugContext = new DebugContext(true, Path.Combine(debugContext.FullPath, "HillListX"));
            hillListX = hillLists[0] = AlgorithmCommon.FindHill(projDataX, 30, false,hillListXDebugContext);
            //hillListX.ForEach(f => Debug.WriteLine(string.Format("hillListX: Size {0}", f.Y - f.X)));

            TrainerParam trainerParam = param as TrainerParam;
            
            DebugContext hillListYDebugContext = new DebugContext(true, Path.Combine(debugContext.FullPath, "HillListY"));
            if (trainerParam.IsCrisscross)
                hillListY = hillLists[1] = AlgorithmCommon.FindHill3(projDataY, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold, hillListYDebugContext);
            else
                hillListY = hillLists[1] = AlgorithmCommon.FindHill(projDataY, trainerParam.MinLineIntensity,false, hillListYDebugContext);
            //hillListY.ForEach(f => Debug.WriteLine(string.Format("hillListY: Size {0}", f.Y - f.X)));

            if (hillListX.Count == 0 || hillListY.Count == 0)
                return new Rectangle[0, 0];

            // 패턴 격자 생성
            //AlgoImage debugImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(projDataX.Length, projDataY.Length));
            //debugImage.Dispose();
            Rectangle[,] rectangles = new Rectangle[hillListY.Count, hillListX.Count];
            for (int x = 0; x < hillLists[0].Count; x++)
            {
                for (int y = 0; y < hillLists[1].Count; y++)
                {
                    Rectangle rectangle = Rectangle.FromLTRB(hillLists[0][x].X, hillLists[1][y].X, hillLists[0][x].Y, hillLists[1][y].Y);
                    rectangles[y, x] = rectangle;
                }
            }
            return rectangles;
        }

        private Rectangle[,] AdjustRect(Rectangle[,] rectangles, DebugContext debugContext)
        {
            // 평균 마진 찾기
            StringBuilder debugStringBuilder = new StringBuilder();
            float averageMarginSizeW = 0, averageMarginSizeH = 0;

            debugStringBuilder.AppendLine("Pattern Area Height");
            int cntH = rectangles.GetLength(0) - 1;
            for (int y = 0; y < cntH; y++)
            {
                int margin = rectangles[y + 1, 0].Top - rectangles[y, 0].Bottom;
                debugStringBuilder.Append(string.Format("{0},", margin));
                averageMarginSizeH += margin;
            }
            debugStringBuilder.AppendLine();
            debugStringBuilder.AppendLine("Pattern Area Width");
            int cntW = rectangles.GetLength(1) - 1;
            for (int x = 0; x < cntW; x++)
            {
                int margin = rectangles[0, x].Left - rectangles[0, x].Right;
                debugStringBuilder.Append(string.Format("{0},", margin));
                averageMarginSizeW += margin;
            }
            File.WriteAllText(Path.Combine(debugContext.FullPath, "debug.txt"), debugStringBuilder.ToString());

            averageMarginSizeH /= (Math.Max(1, cntH) * 2);
            averageMarginSizeW /= (Math.Max(1, cntW) * 2);

            Point centerPos = new Point(rectangles.GetLength(1) / 2, rectangles.GetLength(0) / 2);
            //Thread th = new Thread(() => AdjustRect(debugImage, rectangles, centerPos, hillLists, inflateSize, -1), 1024 * 1024 * 10);
            //th.Start();
            //th.Join();

            return AdjustRect2(rectangles, centerPos);
        }

        private void AdjustRect(AlgoImage debugImage, Rectangle[,] rectangles, Point posIdx, List<Point>[] hillLists, Size inflateSize, int v)
        {
            // 모든 패턴 격자를 inflate하면 겹치거나 1~2픽셀이 붕 뜨는 경우 발생.
            // 중심부터 inflate를 수행하며 Grid 정렬 수행.

            if (posIdx.X < 0 || posIdx.Y < 0 || posIdx.X >= rectangles.GetLength(1) || posIdx.Y >= rectangles.GetLength(0))
                return;
            if (rectangles[posIdx.Y, posIdx.X].IsEmpty == false)
                return;

            ImageProcessing ip = null;
            if (debugImage != null)
                ip = AlgorithmBuilder.GetImageProcessing(debugImage);

            Rectangle rectangle = Rectangle.FromLTRB(hillLists[0][posIdx.X].X, hillLists[1][posIdx.Y].X, hillLists[0][posIdx.X].Y, hillLists[1][posIdx.Y].Y);
            rectangle.Inflate(inflateSize);
            int diff;
            if (v < 0)
            {
                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 0)   // left. compare with right
            {
                Rectangle right = rectangles[posIdx.Y, posIdx.X + 1];
                rectangle.Y = right.Y;
                rectangle.Height = right.Height;
                diff = right.Left - rectangle.Right;
                rectangle.Inflate(diff, 0);
                Debug.Assert(right.Left == rectangle.Right);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 1)   // top. compare with bottom
            {
                Rectangle bottom = rectangles[posIdx.Y + 1, posIdx.X];
                rectangle.X = bottom.X;
                rectangle.Width = bottom.Width;
                //rectangle.Y = target.Y - rectangle.Height;
                //rectangle.Height = target.Y - rectangle.Y;
                diff = bottom.Top - rectangle.Bottom;
                rectangle.Inflate(0, diff);
                Debug.Assert(bottom.Top == rectangle.Bottom);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 2)   // right. compare with left
            {
                Rectangle left = rectangles[posIdx.Y, posIdx.X - 1];
                rectangle.Y = left.Y;
                rectangle.Height = left.Height;
                diff = rectangle.Left - left.Right;
                rectangle.Inflate(diff, 0);
                Debug.Assert(left.Right == rectangle.Left);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 3)   // bottom. compare with top
            {
                Rectangle top = rectangles[posIdx.Y - 1, posIdx.X];
                rectangle.X = top.X;
                rectangle.Width = top.Width;
                //rectangle.Y = target.Y + target.Height;
                //rectangle.Height = rectangle.Bottom - target.Bottom;
                diff = rectangle.Top - top.Bottom;
                rectangle.Inflate(0, diff);
                Debug.Assert(top.Bottom == rectangle.Top);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            //Debug.Assert(rectangle.Width > 0 && rectangle.Height > 0);

            ip?.DrawRect(debugImage, rectangle, 255, false);
            
            //debugImage.Save(@"d:\temp\debugImage.bmp");

#if DEBUG
            if (posIdx.X + 1 < rectangles.GetLength(1) && rectangles[posIdx.Y, posIdx.X + 1].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y, posIdx.X + 1].Top == rectangle.Top && rectangles[posIdx.Y, posIdx.X + 1].Bottom == rectangle.Bottom);
            if (posIdx.X - 1 >= 0 && rectangles[posIdx.Y, posIdx.X - 1].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y, posIdx.X - 1].Top == rectangle.Top && rectangles[posIdx.Y, posIdx.X - 1].Bottom == rectangle.Bottom);
            if (posIdx.Y + 1 < rectangles.GetLength(0) && rectangles[posIdx.Y + 1, posIdx.X].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y + 1, posIdx.X].Left == rectangle.Left && rectangles[posIdx.Y + 1, posIdx.X].Right == rectangle.Right);
            if (posIdx.Y - 1 >= 0 && rectangles[posIdx.Y - 1, posIdx.X].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y - 1, posIdx.X].Left == rectangle.Left && rectangles[posIdx.Y - 1, posIdx.X].Right == rectangle.Right);
#endif

            AdjustRect(debugImage, rectangles, new Point(posIdx.X - 1, posIdx.Y), hillLists, inflateSize, 0);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X, posIdx.Y - 1), hillLists, inflateSize, 1);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X + 1, posIdx.Y), hillLists, inflateSize, 2);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X, posIdx.Y + 1), hillLists, inflateSize, 3);

            if (v < 0)
                debugImage?.Save(@"d:\temp\debugImage.bmp");
        }

        private Rectangle[,] AdjustRect2(Rectangle[,] rectangles, Point posIdx)
        {
            // 마진 맵
            Size size = new Size(rectangles.GetLength(1), rectangles.GetLength(0));
            Rectangle[,] margin = new Rectangle[rectangles.GetLength(0), rectangles.GetLength(1)];
            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    int l = x > 0 ? rectangles[y, x].Left - rectangles[y, x - 1].Right : -1;
                    int t = y > 0 ? rectangles[y, x].Top - rectangles[y - 1, x].Bottom : -1;
                    int r = x < size.Width - 1 ? rectangles[y, x + 1].Left - rectangles[y, x].Right : -1;
                    int b = y < size.Height - 1 ? rectangles[y + 1, x].Top - rectangles[y, x].Bottom : -1;
                    Rectangle rectangle = Rectangle.FromLTRB(l, t, r, b);
                    margin[y, x] = rectangle;
                }
            }

            Rectangle[,] adjustRectangles = new Rectangle[rectangles.GetLength(0), rectangles.GetLength(1)];
            Array.Copy(rectangles, adjustRectangles, Math.Min(rectangles.Length, adjustRectangles.Length));

            for (int x = 0; x < size.Width; x++)
            {
                Point averageMarginValue = Point.Empty;
                Point averageMarginCount = Point.Empty;
                for (int y = 0; y < size.Height; y++)
                {
                    if (margin[y, x].Left >= 0)
                    {
                        averageMarginValue.X += margin[y, x].Left;
                        averageMarginCount.X++;
                    }
                    if (margin[y, x].Right >= 0)
                    {
                        averageMarginValue.Y += margin[y, x].Right;
                        averageMarginCount.Y++;
                    }
                }

                averageMarginValue.X /= (Math.Max(1, averageMarginCount.X) * 2);
                averageMarginValue.Y /= (Math.Max(1, averageMarginCount.Y) * 2);
                for (int y = 0; y < size.Height; y++)
                {
                    int l;
                    if (x == 0)
                        l = adjustRectangles[y, x].Left - (averageMarginValue.X == 0 ? averageMarginValue.Y : averageMarginValue.X);
                    else
                        l = adjustRectangles[y, x - 1].Right;

                    int t = adjustRectangles[y, x].Top;
                    int r = adjustRectangles[y, x].Right + (averageMarginValue.Y == 0 ? averageMarginValue.X : averageMarginValue.Y);
                    int b = adjustRectangles[y, x].Bottom;
                    adjustRectangles[y, x] = Rectangle.FromLTRB(l, t, r, b);
                }
            }


            for (int y = 0; y < size.Height; y++)
            {
                Point averageMarginValue = Point.Empty;
                Point averageMarginCount = Point.Empty;
                for (int x = 0; x < size.Width; x++)
                {
                    if (margin[y, x].Top >= 0)
                    {
                        averageMarginValue.X += margin[y, x].Top;
                        averageMarginCount.X++;
                    }
                    if (margin[y, x].Bottom >= 0)
                    {
                        averageMarginValue.Y += margin[y, x].Bottom;
                        averageMarginCount.Y++;
                    }
                }

                averageMarginValue.X /= (Math.Max(1, averageMarginCount.X) * 2);
                averageMarginValue.Y /= (Math.Max(1, averageMarginCount.Y) * 2);
                for (int x = 0; x < size.Width; x++)
                {
                    int l = adjustRectangles[y, x].Left; 
                    int t;
                    if (y == 0)
                        t = adjustRectangles[y, x].Top - (averageMarginValue.X == 0 ? averageMarginValue.Y : averageMarginValue.X);
                    else
                        t = adjustRectangles[y - 1, x].Bottom;

                    int r = adjustRectangles[y, x].Right;
                    int b = adjustRectangles[y, x].Bottom + (averageMarginValue.Y == 0 ? averageMarginValue.X : averageMarginValue.Y);
                    adjustRectangles[y, x] = Rectangle.FromLTRB(l, t, r, b);
                }
            }

            return adjustRectangles;
        }

        private int GetBinValue(AlgoImage algoImage)
        {
            // 가로축 5등분. 각 영역의 Li Threshold 계산. 최대/최소를 제외한 3개 값의 평균 사용.
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            int n = 5;
            float width = algoImage.Width / n;
            float[] value = new float[n];
            for (int i = 0; i < 5; i++)
            {
                Rectangle region = Rectangle.FromLTRB((int)(width * i), 0, (int)(width * (i + 1)), algoImage.Height);
                AlgoImage subImage = algoImage.GetSubImage(region);
                value[i] = imageProcessing.Li(subImage);
                subImage.Dispose();
            }
            int result = (int)Math.Round((value.Sum() - value.Max() - value.Min()) / (value.Length - 2));
            return result;
        }

        public bool IsSamePattern(BlobRect blobRect, SheetPatternGroup patternGroup, float threshold, float imageScaleX, float imageScaleY)
        {
            TrainerParam param = this.param as TrainerParam;

            float inverseScaleX = 1.0f / imageScaleX;
            float inverseScaleY = 1.0f / imageScaleY;

            blobRect.Area *= inverseScaleX * inverseScaleY;
            blobRect.CenterPt = new PointF(blobRect.CenterPt.X * inverseScaleX, blobRect.CenterPt.Y * inverseScaleY);
            blobRect.CenterOffset = new PointF(blobRect.CenterOffset.X * inverseScaleX, blobRect.CenterOffset.Y * inverseScaleY);
            blobRect.BoundingRect = new RectangleF(blobRect.BoundingRect.X * inverseScaleX, blobRect.BoundingRect.Y * inverseScaleY,
                blobRect.BoundingRect.Width * inverseScaleX, blobRect.BoundingRect.Height * inverseScaleY);

            float halfDiffTol = (float)Math.Round(threshold / 2);

            float areaRatio = patternGroup.AverageArea / (patternGroup.AverageWidth * patternGroup.AverageHeight);
            float maxArea = Math.Min(patternGroup.AverageWidth * (patternGroup.AverageHeight + threshold), (patternGroup.AverageWidth + threshold) * patternGroup.AverageHeight) * areaRatio;
            float minArea = Math.Max(patternGroup.AverageWidth * (patternGroup.AverageHeight - threshold), (patternGroup.AverageWidth - threshold) * patternGroup.AverageHeight) * areaRatio;

            float minCenterOffsetX = patternGroup.AverageCenterOffsetX - halfDiffTol;
            float maxCenterOffsetX = patternGroup.AverageCenterOffsetX + halfDiffTol;

            float minCenterOffsetY = patternGroup.AverageCenterOffsetY - halfDiffTol;
            float maxCenterOffsetY = patternGroup.AverageCenterOffsetY + halfDiffTol;

            float minWidth = patternGroup.AverageWidth - threshold;
            float maxWidth = patternGroup.AverageWidth + threshold;

            float minHeight = patternGroup.AverageHeight - threshold;
            float maxHeight = patternGroup.AverageHeight + threshold;

            if (minArea > blobRect.Area || maxArea < blobRect.Area)
                return false;

            if (minCenterOffsetX > blobRect.CenterOffset.X || maxCenterOffsetX < blobRect.CenterOffset.X)
                return false;

            if (minCenterOffsetY > blobRect.CenterOffset.Y || maxCenterOffsetY < blobRect.CenterOffset.Y)
                return false;

            if (minWidth > blobRect.BoundingRect.Width || maxWidth < blobRect.BoundingRect.Width)
                return false;

            if (minHeight > blobRect.BoundingRect.Height || maxHeight < blobRect.BoundingRect.Height)
                return false;

            return true;
        }
    }
}

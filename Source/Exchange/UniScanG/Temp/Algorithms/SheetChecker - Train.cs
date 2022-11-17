//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Drawing;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Runtime.InteropServices;
//using System.ComponentModel;
//using System.Globalization;

//using DynMvp.Base;
//using DynMvp.UI;
//using DynMvp.Vision;
//using DynMvp.Vision.Matrox;
//using System.Windows;
//using System.IO;
//using UniEye.Base.Settings;
//using System.Drawing.Imaging;

//namespace UniScanG.Temp
//{
//    internal class SheetCheckerTrain : SheetCheckerStep
//    {
//        private object trainParam;

//        public SheetCheckerTrain(AlgorithmParam param)
//        {
//            //this.param = param;
//        }

//        private List<Rectangle> GetROIList(AlgoImage maskImage)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(maskImage);

//            List<Rectangle> roiList = new List<Rectangle>();
//            BlobParam roiBlobParam = new BlobParam();

//            BlobRectList blobRectList = imageProcessing.Blob(maskImage, roiBlobParam);
//            blobRectList.Dispose();

//            List<BlobRect> listOfBlobRect = blobRectList.GetList();

//            foreach (BlobRect blobRect in listOfBlobRect)
//            {
//                roiList.Add(Rectangle.Round(blobRect.BoundingRect));
//            }

//            return roiList;
//        }

//        private void CreateMaskImage(AlgoImage binImage, AlgoImage maskImage, AlgoImage contourImage)
//        {
//            SheetCheckerParam sheetChecker = this.param as SheetCheckerParam;
//            TrainerParam param = sheetChecker.TrainerParam as TrainerParam;

//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);

//            BlobParam patternBlobParam = new BlobParam();
//            patternBlobParam.SelectArea = true;
//            patternBlobParam.AreaMin = param.MinPatternArea;

//            BlobRectList patternBlobRectList = imageProcessing.Blob(binImage, patternBlobParam);

//            DrawBlobOption drawBlobOption = new DrawBlobOption();
//            drawBlobOption.SelectBlob = true;
//            imageProcessing.DrawBlob(maskImage, patternBlobRectList, null, drawBlobOption);

//            imageProcessing.FillHoles(maskImage, maskImage);

//            DrawBlobOption drawContourBlobOption = new DrawBlobOption();
//            drawContourBlobOption.SelectBlobContour = true;
//            if (contourImage != null)
//                imageProcessing.DrawBlob(contourImage, patternBlobRectList, null, drawContourBlobOption);

//            patternBlobRectList.Dispose();
//        }
        
//        void CreateMask(AlgoImage maskImage1, AlgoImage maskImage2, List<SheetRange> offsetDummyList, List<BlobRect> carePatternList, int width)
//        {
//            bool onROI = false;
//            int startPos = 0;

//            List<Rectangle> listOfROI = new List<Rectangle>();
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);

//            List<SheetRange> horizontalDummyList = new List<SheetRange>();
//            foreach (SheetRange dummy in offsetDummyList)
//            {
//                if (dummy.Type == RangeType.Horizontal)
//                    horizontalDummyList.Add(dummy);
//            }

//            horizontalDummyList.Sort((SheetRange x, SheetRange y) => x.StartPos.CompareTo(y.StartPos));

//            for (int i = 0; i < horizontalDummyList.Count; i++)
//            {
//                if (onROI == false)
//                {
//                    startPos = horizontalDummyList[i].EndPos;
//                    onROI = true;
//                }
//                else
//                {
//                    listOfROI.Add(new Rectangle(0, startPos, width, horizontalDummyList[i].StartPos));
//                    onROI = false;
//                }
//            }

//            if (carePatternList.Count != 0)
//            {
//                int[] minX = new int[listOfROI.Count];
//                int[] maxX = new int[listOfROI.Count];
//                int[] minY = new int[listOfROI.Count];
//                int[] maxY = new int[listOfROI.Count];

//                for (int i = 0; i < listOfROI.Count; i++)
//                {
//                    minX[i] = int.MaxValue;
//                    maxX[i] = 0;
//                    minY[i] = int.MaxValue;
//                    maxY[i] = 0;
//                }

//                Parallel.For(0, carePatternList.Count, i =>
//                {
//                    for (int j = 0; j < listOfROI.Count; j++)
//                    {
//                        if (listOfROI[j].Contains(Point.Round(carePatternList[i].CenterPt)) == true)
//                        {
//                            if (minX[j] > carePatternList[i].BoundingRect.X)
//                            {
//                                minX[j] = (int)carePatternList[i].BoundingRect.X;
//                            }

//                            if (maxX[j] < carePatternList[i].BoundingRect.Right)
//                            {
//                                maxX[j] = (int)carePatternList[i].BoundingRect.Right;
//                            }

//                            if (minY[j] > carePatternList[i].BoundingRect.Y)
//                            {
//                                minY[j] = (int)carePatternList[i].BoundingRect.Y;
//                            }

//                            if (maxY[j] < carePatternList[i].BoundingRect.Bottom)
//                            {
//                                maxY[j] = (int)carePatternList[i].BoundingRect.Bottom;
//                            }
//                        }
//                    }
//                });

//                for (int i = 0; i < listOfROI.Count; i++)
//                    listOfROI[i] = new Rectangle(minX[i], minY[i], maxX[i] - minX[i], maxY[i] - minY[i]);

//                foreach (Rectangle rect in listOfROI)
//                    imageProcessing.DrawRect(maskImage2, rect, 255, true);
//            }

//            imageProcessing.Or(maskImage1, maskImage2, maskImage1);
//        }

//        public bool IsSamePattern(BlobRect blobRect, PatternGroup patternGroup, float threshold, float imageScaleX, float imageScaleY)
//        {
//            SheetCheckerParam sheetCheckerParam = this.param as SheetCheckerParam;
//            TrainerParam param = sheetCheckerParam.TrainerParam;

//            float inverseScaleX = 1.0f / imageScaleX;
//            float inverseScaleY = 1.0f / imageScaleY;

//            blobRect.Area *= inverseScaleX * inverseScaleY;
//            blobRect.CenterPt = new PointF(blobRect.CenterPt.X * inverseScaleX, blobRect.CenterPt.Y * inverseScaleY);
//            blobRect.CenterOffset = new PointF(blobRect.CenterOffset.X * inverseScaleX, blobRect.CenterOffset.Y * inverseScaleY);
//            blobRect.BoundingRect = new RectangleF(blobRect.BoundingRect.X * inverseScaleX, blobRect.BoundingRect.Y * inverseScaleY,
//                blobRect.BoundingRect.Width * inverseScaleX, blobRect.BoundingRect.Height * inverseScaleY);

//            float halfDiffTol = (float)Math.Round(threshold / 2);

//            float areaRatio = patternGroup.AverageArea / (patternGroup.AverageWidth * patternGroup.AverageHeight);
//            float maxArea = Math.Min(patternGroup.AverageWidth * (patternGroup.AverageHeight + threshold), (patternGroup.AverageWidth + threshold) * patternGroup.AverageHeight) * areaRatio;
//            float minArea = Math.Max(patternGroup.AverageWidth * (patternGroup.AverageHeight - threshold), (patternGroup.AverageWidth - threshold) * patternGroup.AverageHeight) * areaRatio;

//            float minCenterOffsetX = patternGroup.AverageCenterOffsetX - halfDiffTol;
//            float maxCenterOffsetX = patternGroup.AverageCenterOffsetX + halfDiffTol;

//            float minCenterOffsetY = patternGroup.AverageCenterOffsetY - halfDiffTol;
//            float maxCenterOffsetY = patternGroup.AverageCenterOffsetY + halfDiffTol;

//            float minWidth = patternGroup.AverageWidth - threshold;
//            float maxWidth = patternGroup.AverageWidth + threshold;

//            float minHeight = patternGroup.AverageHeight - threshold;
//            float maxHeight = patternGroup.AverageHeight + threshold;

//            if (minArea > blobRect.Area || maxArea < blobRect.Area)
//                return false;

//            if (minCenterOffsetX > blobRect.CenterOffset.X || maxCenterOffsetX < blobRect.CenterOffset.X)
//                return false;

//            if (minCenterOffsetY > blobRect.CenterOffset.Y || maxCenterOffsetY < blobRect.CenterOffset.Y)
//                return false;

//            if (minWidth > blobRect.BoundingRect.Width || maxWidth < blobRect.BoundingRect.Width)
//                return false;

//            if (minHeight > blobRect.BoundingRect.Height || maxHeight < blobRect.BoundingRect.Height)
//                return false;

//            return true;
//        }

//        public void FindRefPattern(ref List<PatternGroup> refPatternList, List<BlobRect> patternList, float threshold)
//        {
//            List<PatternGroup> areaPatternGroup = new List<PatternGroup>();

//            patternList = patternList.OrderByDescending(x => x.Area).ToList();
//            PatternGroup patternGroup = new PatternGroup();
//            patternGroup.AddPattern(patternList[0]);
//            areaPatternGroup.Add(patternGroup);
//            patternGroup.GetAverageBlobRect();

//            SheetCheckerParam sheetChecker = this.param as SheetCheckerParam;
//            TrainerParam param = sheetChecker.TrainerParam as TrainerParam;
            
//            for (int i = 1; i < patternList.Count; i++)
//            {
//                double refMeanLength = (double)Math.Sqrt(patternGroup.AverageArea);
//                double meanLength = (double)Math.Sqrt(patternList[i].Area);

//                if (Math.Abs(refMeanLength - meanLength) > threshold)
//                {
//                    patternGroup = new PatternGroup();
//                    areaPatternGroup.Add(patternGroup);
//                }

//                patternGroup.AddPattern(patternList[i]);
//                patternGroup.GetAverageBlobRect();
//            }

//            List<PatternGroup> pgList = new List<PatternGroup>();

//            foreach (PatternGroup pg in areaPatternGroup)
//            {
//                List<PatternGroup> subPatternGroup = pg.DivideSubGroup(threshold);
//                pgList.AddRange(subPatternGroup);
//            }

//            pgList = pgList.OrderByDescending(x => x.NumPattern).ToList();

//            for (int i = 0; i < pgList.Count; i++)
//            {
//                bool isSamePattern = false;
//                int sameIndex = -1;
//                for (int j = i + 1; j < pgList.Count; j++)
//                {
//                    if (IsSamePattern(pgList[j].GetAverageBlobRect(), pgList[i], param.PatternGroupThreshold, 1.0f, 1.0f))
//                    {
//                        isSamePattern = true;
//                        sameIndex = j;
//                        break;
//                    }
//                }

//                if (isSamePattern == true)
//                {
//                    foreach (BlobRect blobRect in pgList[sameIndex].PatternList)
//                        pgList[i].AddPattern(blobRect);

//                    pgList.RemoveAt(sameIndex);

//                    i = -1;
//                }
//            }

//            refPatternList.AddRange(pgList);
//        }

//        public int PatternTrain(ref List<PatternGroup> refPatternList, AlgoImage patternInspectImage, int threshold)
//        {
//            SheetCheckerParam sheetChecker = this.param as SheetCheckerParam;
//            TrainerParam param = sheetChecker.TrainerParam as TrainerParam;

//            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

//            BlobParam patternBlobParam = new BlobParam();
//            patternBlobParam.AreaMin = param.MinPatternArea;
//            patternBlobParam.EraseBorderBlobs = true;
//            patternBlobParam.SelectCenterPt = true;
//            patternBlobParam.SelectCompactness = true;
//            patternBlobParam.SelectFeretDiameter = true;

//            patternBlobParam.AreaMin = param.MinPatternArea;
//            BlobRectList patternBlobRectList = imageProcessing.Blob(patternInspectImage, patternBlobParam);
//            List<BlobRect> patternList = patternBlobRectList.GetList();
//            Task task = new Task(new Action(() => patternBlobRectList.Dispose()));
//            task.Start();

//            if (patternList.Count == 0)
//                return 0;

//            refPatternList = new List<PatternGroup>();
//            FindRefPattern(ref refPatternList, patternList, param.PatternGroupThreshold);

//            return refPatternList.Max(x => x.PatternList.Count);
//        }

//        public float GetPatternPeriod(AlgoImage patternImage, Direction projectionDir, int threshold)
//        {
//            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

//            int imageLength = 0;
//            if (projectionDir == Direction.Horizontal)
//                imageLength = patternImage.Width;
//            else
//                imageLength = patternImage.Height;

//            int lenght = 2;
//            while (lenght < imageLength)
//                lenght *= 2;

//            int width = 1;
//            int height = 1;

//            if (projectionDir == Direction.Horizontal)
//                width = lenght;
//            else
//                height = lenght;

//            float[] projectionArray = imageProcessing.Projection(patternImage, projectionDir, ProjectionType.Mean);
//            byte[] imageArray = new byte[lenght];

//            for (int i = 0; i < projectionArray.Length; i++)
//                imageArray[i] = (byte)Math.Round(projectionArray[i]);

//            AlgoImage fftImage = ImageBuilder.Build(GetAlgorithmType(), ImageType.Grey, width, height);
//            fftImage.PutByte(imageArray);

//            float[,] fftArray = imageProcessing.FFT(fftImage);

//            float[] values = new float[fftArray.Length];
//            Buffer.BlockCopy(fftArray, 0, values, 0, fftArray.Length);
            
//            fftImage.Dispose();

//            float maxValue = 0;


//            int startIndex = (int)(lenght / (threshold * 3f));
//            int endIndex = lenght / threshold;

//            int maxIndex = startIndex;

//            for (int i = startIndex; i < endIndex; i++)
//            {
//                if (values[i] > maxValue)
//                {
//                    maxIndex = i;
//                    maxValue = values[i];
//                }
//            }

//            return (float)lenght / maxIndex;
//        }

//        public List<SheetRange> AutoMasking(AlgoImage patternImage, PatternGroup refPatternGroup, int width, int height)
//        {
//            SheetCheckerParam sheetChecker = this.param as SheetCheckerParam;
//            TrainerParam param = sheetChecker.TrainerParam as TrainerParam;

//            List<SheetRange> dummyRangeList = new List<SheetRange>();

//            float xPeriod = 50;//GetPatternPeriod(patternImage, Direction.Horizontal, (int)refPatternGroup.AverageWidth);
//            float yPeriod = 50; //GetPatternPeriod(patternImage, Direction.Vertical, (int)refPatternGroup.AverageHeight);

//            int xGap = 10;// (int)Math.Round(xPeriod - refPatternGroup.AverageWidth) / 2;
//            int yGap = 10;// (int)Math.Round(yPeriod - refPatternGroup.AverageHeight) / 2;

//            float dummySizeXThreshold = xPeriod;
//            float dummySizeYThreshold = yPeriod;

//            int startPos = 0;
//            int endPos = 0;
//            bool onNewMask = true;

            
//            for (int x = 0; x < width; x++)
//            {
//                bool onBlobRectPos = false;

//                foreach (BlobRect blobRect in refPatternGroup.PatternList)
//                {
//                    if (blobRect.BoundingRect.X <= x && x <= blobRect.BoundingRect.X + blobRect.BoundingRect.Width)
//                    {
//                        onBlobRectPos = true;
//                        break;
//                    }
//                }

//                if (onBlobRectPos == true)
//                {
//                    if (onNewMask == true)
//                    {
//                        if (endPos - startPos > dummySizeXThreshold)
//                        {
//                            dummyRangeList.Add(new SheetRange(RangeType.Vertical, Math.Max(0, Math.Min(width - 1,  startPos + xGap)), Math.Max(0, Math.Min(width - 1, endPos - xGap))));
//                        }
//                    }

//                    onNewMask = false;
//                }
//                else
//                {
//                    if (onNewMask == false)
//                    {
//                        startPos = x;
//                        onNewMask = true;
//                    }

//                    endPos = x;
//                }
//            }

//            if (onNewMask == true)
//            {
//                if (endPos - startPos > dummySizeXThreshold)
//                {
//                    dummyRangeList.Add(new SheetRange(RangeType.Vertical, Math.Max(0, Math.Min(width - 1, startPos + xGap)), Math.Max(0, Math.Min(width - 1, endPos - xGap))));
//                }
//            }

//            startPos = 0;
//            endPos = 0;
//            onNewMask = true;
//            for (int y = 0; y < height; y++)
//            {
//                bool onBlobRectPos = false;

//                foreach (BlobRect blobRect in refPatternGroup.PatternList)
//                {
//                    if (blobRect.BoundingRect.Y <= y && y <= blobRect.BoundingRect.Y + blobRect.BoundingRect.Height)
//                    {
//                        onBlobRectPos = true;
//                        break;
//                    }
//                }

//                if (onBlobRectPos == true)
//                {
//                    if (onNewMask == true)
//                    {
//                        if (endPos - startPos > dummySizeYThreshold)
//                        {
//                            dummyRangeList.Add(new SheetRange(RangeType.Horizontal, Math.Max(0, Math.Min(height - 1, startPos + yGap)), Math.Max(0, Math.Min(height - 1, endPos - yGap))));
//                        }
//                    }
//                    onNewMask = false;
//                }
//                else
//                {
//                    if (onNewMask == false)
//                    {
//                        startPos = y;
//                        onNewMask = true;
//                    }

//                    endPos = y;
//                }
//            }

//            if (onNewMask == true)
//            {
//                if (endPos - startPos > dummySizeYThreshold)
//                {
//                    dummyRangeList.Add(new SheetRange(RangeType.Horizontal, Math.Max(0, Math.Min(height - 1, startPos + yGap)), Math.Max(0, Math.Min(height - 1, endPos - yGap))));
//                }
//            }

//            bool existLeft = false;
//            bool existRight = false;

//            foreach (SheetRange dummy in dummyRangeList)
//            {
//                if (dummy.Type == RangeType.Vertical)
//                {
//                    if (dummy.StartPos == 0)
//                    {
//                        dummy.Type = RangeType.Left;
//                    }

//                    if (dummy.EndPos >= width - 1)
//                    {
//                        dummy.Type = RangeType.Right;
//                    }
//                }

//                if (existLeft == false)
//                    if (dummy.Type == RangeType.Left)
//                        existLeft = true;

//                if (existRight == false)
//                    if (dummy.Type == RangeType.Right)
//                        existRight = true;
//            }

//            return dummyRangeList;
//        }

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            SheetCheckerParam param = this.param as SheetCheckerParam;
//            TrainerParam trainerParam = param.TrainerParam;
//            TrainerInspectParam trainInspectParam = algorithmInspectParam as TrainerInspectParam;

//            TeachProgressUpdate teachProgressUpdate = trainInspectParam.TeachProgressUpdate;
//            string message = "";
//            DebugContext debugContext = trainInspectParam.DebugContext;
//            AlgorithmResult algorithmResult = CreateAlgorithmResult();
//            AlgoImage teachImage = trainInspectParam.TrainImage;

//            try
//            {
//                //sourceImage = ImageBuilder.Build(GetAlgorithmType(), trainParam.TrainImage, ImageType.Grey, ImageBandType.Luminance);

//                // Find Fiducial
//                FiducialFinder sheetCheckerFiducial = new FiducialFinder(param);
//                FiducialFinderInspectParam fiducialFinderInspectParam = new FiducialFinderInspectParam(teachImage,true, algorithmInspectParam);

//                AlgorithmResult fiducialResult = sheetCheckerFiducial.Inspect(fiducialFinderInspectParam);
//                sheetCheckerFiducial.Dispose();
//                if (fiducialResult.Good == false)
//                {
//                    fiducialResult.Good = false;
//                    fiducialResult.Message = "Can not found Fiducial";
//                    return fiducialResult;
//                }

//                Rectangle fidRect = (Rectangle)fiducialResult.GetResultValue("FidRect").Value;

//                // Teach
//                algorithmResult.Good = Train(teachImage, fidRect.Location, teachProgressUpdate, out message, debugContext);
//                algorithmResult.Message = message;
//            }
//            finally
//            {
               
//            }
//            return algorithmResult;
//        }

//        // No-Resize
//        public bool Train(AlgoImage trainImage, Point fidPoint, TeachProgressUpdate teachProgressUpdate, out string message, DebugContext debugContext)
//        {
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);

//            SheetCheckerParam sheetCheckerParam = this.param as SheetCheckerParam;
//            TrainerParam param = sheetCheckerParam.TrainerParam;
            
//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.ImageAlloc);
            

//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.FindFiducial);

//            param.FiducialXPos = fidPoint.X;
//            DebugHelper.SaveText(string.Format("{0}", fidPoint.X), "BaseFiducialXPos.txt", debugContext);

//            int width = trainImage.Width;
//            int height = trainImage.Height;

//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.Average);
            
//            //float triangleValue = imageProcessing.Triangle(resizeImages.interestRegionImage);

//            List<PatternGroup> patternList = null;

//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.FindPattern);

//            // 3-1. 패턴 갯수 사용하여 이진화값 조정
//            Rectangle sheetRect = Rectangle.Empty;
//            switch (param.FidPosition)
//            {
//                case FiducialPosition.Left:
//                    sheetRect = new Rectangle(fidPoint.X, fidPoint.Y, width - fidPoint.X, height - fidPoint.Y);
//                    break;
//                case FiducialPosition.Right:
//                    sheetRect = new Rectangle(0, fidPoint.Y, fidPoint.X, height - fidPoint.Y);
//                    break;
//            }
//            sheetRect.Intersect(new Rectangle(Point.Empty, trainImage.Size));

//            if (sheetRect.IsEmpty == true)
//            {
//                message = "ROI is Empty. Check Fid Position";
//                return false;
//            }

//            AlgoImage binImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, width, height);

//            AlgoImage sourceROIImage = trainImage.GetSubImage(sheetRect);
//            param.AutoThresholdValue = (int)Math.Round(imageProcessing.Li(sourceROIImage, null));//
//            if(param.AutoThresholdValue == 0)
//            {
//                message = "Can not find Pattern Threshold Value";
//                return false;
//            }
//            //int triangleValue = 90;
//            //sourceROIImage.Save("sourceROIImage.bmp", debugContext);
//            sourceROIImage.Dispose();

//            AlgoImage patternMaskImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, width, height);
//            AlgoImage dummyMaskImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, width, height);
//            AlgoImage edgeImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, width, height);
//            param.Traind = false;

//            // 4. 패턴 검출
//            imageProcessing.Binarize(trainImage, binImage, param.AutoThresholdValue, true);
//            binImage.Save("binImage.bmp", debugContext);
//            CreateMaskImage(binImage, patternMaskImage, edgeImage);

//            patternMaskImage.Save("patternMaskImage.bmp", debugContext);
//            edgeImage.Save("edgeImage.bmp", debugContext);

//            PatternTrain(ref patternList, patternMaskImage, param.AutoThresholdValue);

//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.CreateMask);
//            // 6. 패턴 검출 Mask 생성

//            if (patternList == null || patternList.Count == 0)
//            {
//                //MessageBox.Show("Can not found pattern");
//                patternMaskImage.Dispose();
//                dummyMaskImage.Dispose();
//                edgeImage.Dispose();
//                binImage.Dispose();
//                message = "PatternList is not founded";
//                return false;
//            }

//            PatternGroup refPatternGroup = patternList.OrderByDescending(x => x.PatternList.Count * x.AverageArea).First();

//            List<SheetRange> dummyList = new List<SheetRange>();
//            dummyList =  AutoMasking(patternMaskImage, refPatternGroup, width, height);
//            param.RefPattern = new SheetPattern(null, null, refPatternGroup);

//            // -------------------------------------------------------------//
//            dummyMaskImage.Clear();
//            foreach (SheetRange dummy in dummyList)
//            {
//                Rectangle dummyArea = new Rectangle();

//                switch (dummy.Type)
//                {
//                    case RangeType.Middle:
//                    case RangeType.Horizontal:
//                        dummyArea = new Rectangle(0, dummy.StartPos, width, dummy.EndPos - dummy.StartPos);
//                        break;
//                    case RangeType.Left:
//                    case RangeType.Right:
//                    case RangeType.Vertical:
//                        dummyArea = new Rectangle(dummy.StartPos, 0, dummy.EndPos - dummy.StartPos, height);
//                        break;
//                }

//                //dummyMaskImage.Save("dummyMaskImage.bmp", debugContext);
//                imageProcessing.DrawRect(dummyMaskImage, dummyArea, 255, true);
//            }
//            dummyMaskImage.Save("dummyMaskImage.bmp", debugContext);
            
//            imageProcessing.Not(edgeImage, edgeImage);
//            imageProcessing.Not(dummyMaskImage, dummyMaskImage);
//            imageProcessing.And(dummyMaskImage, edgeImage, edgeImage);

//            BlobParam dummyMaskParam = new BlobParam();
//            BlobRect leftTopBlob = new BlobRect();

//            bool addRects = false;
//            BlobRectList dummyMaskBlobList;

//            List<ProjectionRegion> projectionRegionList = new List<ProjectionRegion>();
            
//            int projectionRegionIndex = 0;
//            addRects = false;
//            dummyMaskBlobList = imageProcessing.Blob(dummyMaskImage, dummyMaskParam);
//            dummyMaskBlobList.Dispose();
//            projectionRegionList.Clear();
//            foreach (BlobRect dummyBlobRect in dummyMaskBlobList)
//            {
//                ProjectionRegion projectionRegion = new ProjectionRegion(projectionRegionIndex++);
//                projectionRegion.Region = Rectangle.Round(dummyBlobRect.BoundingRect);
//                projectionRegionList.Add(projectionRegion);
//            }

//            foreach (ProjectionRegion projectionRegion in projectionRegionList)
//            {
//                foreach (BlobRect pattern in refPatternGroup.PatternList)
//                {
//                    if (projectionRegion.Region.Contains(Point.Round(pattern.CenterPt)) == true)
//                    {
//                        Rectangle inspRect = Rectangle.Round(pattern.BoundingRect);
//                        inspRect.Inflate(new Size(10, 10));

//                        if (projectionRegion.InspRectList == null)
//                            projectionRegion.InspRectList = new List<Rectangle>();

//                        projectionRegion.InspRectList.Add(inspRect);
//                    }
//                }
//            }

//            Rectangle sourceRect = new Rectangle(0, 0, width, height);
//            bool projectionResult = true;

//            int patternHeight = (int)(refPatternGroup.AverageHeight * 0.7);
//            int patternWidth = (int)(refPatternGroup.AverageWidth * 0.9);
//            float fillRatio = 0.15f;
//            int projectionSize = 1000;

//            foreach (ProjectionRegion projectionRegion in projectionRegionList)
//            {
//                if (Rectangle.Intersect(projectionRegion.Region, sourceRect) != projectionRegion.Region)
//                {
//                    projectionResult = false;
//                    break;
//                }

//                AlgoImage patternMaskSubImage = binImage.GetSubImage(projectionRegion.Region); //패턴 마스크 영역 이미지
//                patternMaskSubImage.Save(string.Format("{0}_patternMaskSubImage.bmp", projectionRegion.Id), debugContext);

//                for (int vProjBase = 0; vProjBase < 2; vProjBase++)
//                {
//                    int vProjRectL = (vProjBase == 0 ? 0 : patternMaskSubImage.Width - projectionSize);
//                    int vProjRectT = 0;
//                    int vProjRectR = vProjRectL + projectionSize;
//                    int vProjRectB = patternMaskSubImage.Height;

//                    if (vProjRectL <0)
//                        vProjRectL = 0;

//                    if (vProjRectR >= patternMaskSubImage.Width)
//                        vProjRectR = patternMaskSubImage.Width;
//                    Rectangle vProjectionRect = Rectangle.FromLTRB(vProjRectL, vProjRectT, vProjRectR, vProjRectB);

//                    AlgoImage projectionSubImage = patternMaskSubImage.GetSubImage(vProjectionRect); //투영 영역 이미지
//                    projectionSubImage.Save(string.Format("{0}_projectionSubImage{1}.bmp", projectionRegion.Id, vProjBase), debugContext);

//                    float[] projectionData = imageProcessing.Projection(projectionSubImage, Direction.Vertical, ProjectionType.Sum);
//                    projectionSubImage.Dispose();

//                    List<SheetRange> vProjectionRangeList = GravureSheetChecker.GetProjectionRangeList(projectionData, patternHeight, fillRatio, vProjectionRect.Width);
//                    if (vProjectionRangeList.Count == 0)
//                    {
//                        //projectionResult = false;
//                        //patternMaskSubImage.Dispose();
//                        continue;
//                    }

//                    projectionRegion.RangeList[vProjBase] = new List<SheetRange>(vProjectionRangeList);

//                    StringBuilder debugLogStringBuilder = new StringBuilder();
//                    debugLogStringBuilder.AppendLine(string.Format("PatternRect: {0}", projectionRegion.Region.ToString()));
//                    debugLogStringBuilder.AppendLine("Index, Type, Start Y Pos, End Y Pos, Base X Pos");
//                    for (int j = 0; j < vProjectionRangeList.Count; j++)
//                    {
//                        //x projection
//                        int hProjRectL = (vProjBase == 0 ? 0 : patternMaskSubImage.Width - patternWidth * 40);
//                        int hProjRectT = vProjectionRangeList[j].StartPos;
//                        int hProjRectR = hProjRectL + patternWidth * 40;
//                        int hProjRectB = vProjectionRangeList[j].EndPos;
//                        if (j < vProjectionRangeList.Count - 1)
//                            hProjRectB = vProjectionRangeList[j + 1].StartPos;

//                        Rectangle hProjectionRect = Rectangle.FromLTRB(hProjRectL, hProjRectT, hProjRectR, hProjRectB);
//                        hProjectionRect.Intersect(new Rectangle(Point.Empty, patternMaskSubImage.Size));

//                        AlgoImage projectionSubImage2 = patternMaskSubImage.GetSubImage(hProjectionRect);
//                        projectionSubImage2.Save(string.Format("{0}_{1}_projectionSubImage{2}.bmp", projectionRegion.Id, j, vProjBase), debugContext);

//                        projectionData = imageProcessing.Projection(projectionSubImage2, Direction.Horizontal, ProjectionType.Sum);
//                        projectionSubImage2.Dispose();

//                        List<SheetRange> hProjectionRangeList = GravureSheetChecker.GetProjectionRangeList(projectionData, patternWidth, fillRatio, hProjectionRect.Height);
//                        if (hProjectionRangeList.Count > 1)
//                        {
//                            int fidInd = (vProjBase == 0 ? 1 : hProjectionRangeList.Count - 2);
//                            int midXPos = (hProjectionRangeList[fidInd].StartPos + hProjectionRangeList[fidInd].EndPos) / 2;
//                            switch (vProjBase)
//                            {
//                                case 0:
//                                    // 왼쪽 기준
//                                    vProjectionRangeList[j].XFiducialPt = midXPos;
//                                    vProjectionRangeList[j].Type = RangeType.Left;
//                                    break;
//                                case 1:
//                                    // 오른쪽 기준
//                                    vProjectionRangeList[j].XFiducialPt = hProjectionRect.Width - midXPos;
//                                    vProjectionRangeList[j].Type = RangeType.Right;
//                                    break;
//                            }
//                        }
//                        debugLogStringBuilder.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}", j, vProjectionRangeList[j].Type.ToString(), vProjectionRangeList[j].StartPos, vProjectionRangeList[j].EndPos, vProjectionRangeList[j].XFiducialPt));
//                    }
//                    DebugHelper.SaveText(debugLogStringBuilder.ToString(), string.Format("SubReagion{0} Teach Result{1}.txt", projectionRegion.Id, vProjBase), debugContext);
//                }
//                patternMaskSubImage.Dispose();
//            }

//            if (projectionResult == false)
//            {
//                patternMaskImage.Dispose();
//                dummyMaskImage.Dispose();
//                edgeImage.Dispose();
//                binImage.Dispose();
//                message = "Projection value is not founded";
//                return false;
//            }

//            param.ProjectionRegionList = projectionRegionList;

//            AlgoImage projectionImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, width, height);

//            foreach (ProjectionRegion projectionRegion in projectionRegionList)
//            {
//                AlgoImage projectionChildImage = projectionImage.GetSubImage(projectionRegion.Region);

//                // X-Dir Projection
//                foreach (SheetRange range in projectionRegion.RangeList[0])
//                {
//                    imageProcessing.DrawRect(projectionChildImage, Rectangle.FromLTRB(0, range.StartPos, width / 2, range.EndPos), 255, true);
//                }
//                foreach (SheetRange range in projectionRegion.RangeList[1])
//                {
//                    imageProcessing.DrawRect(projectionChildImage, new Rectangle(width / 2, range.StartPos, width, range.EndPos), 255, true);
//                }

//                // Y-Dir Projection
//                imageProcessing.DrawRect(projectionChildImage, new Rectangle(0, 0, projectionSize, projectionRegion.Region.Height), 128, true);
//                imageProcessing.DrawRect(projectionChildImage, new Rectangle(projectionChildImage.Width - projectionSize, 0, projectionSize, projectionRegion.Region.Height), 128, true);


//                AlgoImage sourceSubImage = trainImage.GetSubImage(projectionRegion.Region);
//                sourceSubImage.Save(string.Format("{0}_sourceSubImage.bmp", projectionRegion.Id), debugContext);
//                projectionChildImage.Save(string.Format("{0}_projectionRegion.bmp", projectionRegion.Id), debugContext);
//                projectionChildImage.Dispose();
//                sourceSubImage.Dispose();
//            }

//            param.Clear();

//            float scaleFactor = 0.1f;

//            int resizeWidth = (int)(width * scaleFactor);
//            int resizeHeight = (int)(height * scaleFactor);
//            AlgoImage resizeReffImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, resizeWidth, resizeHeight);
//            AlgoImage resizeEdgeMaskImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, resizeWidth, resizeHeight);
//            AlgoImage resizeDummyMaskImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, resizeWidth, resizeHeight);
//            AlgoImage resizeProjectionImage = ImageBuilder.Build(this.GetAlgorithmType(), ImageType.Grey, resizeWidth, resizeHeight);
            
//            imageProcessing.Resize(trainImage, resizeReffImage, scaleFactor);
//            imageProcessing.Resize(edgeImage, resizeEdgeMaskImage, scaleFactor);
//            imageProcessing.Resize(dummyMaskImage, resizeDummyMaskImage, scaleFactor);
//            imageProcessing.Resize(projectionImage, resizeProjectionImage, scaleFactor);

//            param.RefferenceImage = (Image2D)resizeReffImage.ToImageD();
//            param.InspectRegionInfoImage = Image2D.ToImage2D(CreateSmogImage(resizeReffImage.ToImageD().ToBitmap(), projectionRegionList, scaleFactor));
            
//            param.PatternList.Clear();
//            foreach (PatternGroup patternGroup in patternList)
//            {
//                BlobRect blobRect = patternGroup.GetAverageBlobRect();

//                switch (param.FidPosition)
//                {
//                    case FiducialPosition.Left:
//                        if (blobRect.CenterPt.X < fidPoint.X)
//                            continue;
//                        break;

//                    case FiducialPosition.Right:
//                        if (blobRect.CenterPt.X > fidPoint.X)
//                            continue;
//                        break;
//                }

//                AlgoImage patternImage = trainImage.GetSubImage(Rectangle.Round(blobRect.BoundingRect));
//                SheetPattern pattern = new SheetPattern(patternImage, blobRect, patternGroup, SheetPatternType.Care);
//                param.PatternList.Add(pattern);
//            }

//            resizeReffImage.Dispose();
//            resizeEdgeMaskImage.Dispose();
//            resizeDummyMaskImage.Dispose();
//            resizeProjectionImage.Dispose();
//            projectionImage.Dispose();
            
//            param.Traind = true;

//            patternMaskImage.Dispose();
//            dummyMaskImage.Dispose();
//            edgeImage.Dispose();
//            binImage.Dispose();

//            if (teachProgressUpdate != null)
//                teachProgressUpdate(ProgressType.End);

//            message = "OK";
//            return true;
//        }


//        public Bitmap CreateSmogImage(Bitmap original, List<ProjectionRegion> projectionRegionList, float scaleFactor)
//        {
//            Bitmap smogImage = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);

//            List<Rectangle> resizeRegionList = new List<Rectangle>();
            
//            foreach (ProjectionRegion projectionRegion in projectionRegionList)
//            {
//                Rectangle resizeRegion = projectionRegion.Region;

//                resizeRegion.X = (int)Math.Round(resizeRegion.X * scaleFactor);
//                resizeRegion.Y = (int)Math.Round(resizeRegion.Y * scaleFactor);
//                resizeRegion.Width = (int)Math.Round(resizeRegion.Width * scaleFactor);
//                resizeRegion.Height = (int)Math.Round(resizeRegion.Height * scaleFactor);

//                resizeRegionList.Add(resizeRegion);
//            }

//            //for (int i = 0; i < original.Width; i++)
//            //{
//            //    for (int j = 0; j < original.Height; j++)
//            //    {
//            //        bool contain = false;
//            //        foreach (Rectangle resizeRegion in resizeRegionList)
//            //        {
//            //            if (resizeRegion.Contains(i, j) == true)
//            //            {
//            //                contain = true;
//            //                break;
//            //            }
//            //        }

//            //        Color originalColor = original.GetPixel(i, j);
//            //        Color newColor;
//            //        if (contain == true)
//            //        {
//            //            //Color newColor = Color.FromArgb(Math.Min(originalColor.R * 2, 255), originalColor.G / 2, originalColor.B / 2);
//            //            newColor = Color.FromArgb(originalColor.R, originalColor.G / 2, originalColor.B / 2);
//            //        }
//            //        else
//            //        {
//            //            newColor = Color.FromArgb(originalColor.R, originalColor.G, originalColor.B);

//            //        }
//            //        smogImage.SetPixel(i, j, newColor);
//            //    }
//            //}

//            Pen rectPen = new Pen(Color.FromArgb(0, 0, 255, 255), 5);
//            Brush brush = new SolidBrush(Color.FromArgb(64, 0, 255, 255));
//            Graphics g = Graphics.FromImage(smogImage);
//            g.DrawImage(original, Point.Empty);
//            foreach (Rectangle rectangle in resizeRegionList)
//            {
//                g.FillRectangle(brush, rectangle);
//                g.DrawRectangle(rectPen, rectangle);
//            }
//            g.Dispose();
//            return smogImage;
//        }
//    }
//}

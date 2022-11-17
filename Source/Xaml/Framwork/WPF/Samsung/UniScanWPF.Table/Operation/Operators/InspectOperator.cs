using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class InspectOperator : Operator
    {
        const int inflate = 50;
        const int regionWidth = 40;

        InspectOperatorSettings settings;
        public InspectOperatorSettings Settings { get => settings; }

        List<Task> taskList;

        public InspectOperator()
        {
            settings = new InspectOperatorSettings();
            taskList = new List<Task>();
        }

        public void StartInspect(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Extract)
                return;

            this.OperatorState = OperatorState.Run;

            Task task = Task.Factory.StartNew(() =>
            {
                SystemManager.Instance().OperatorProcessed(Inspect((ExtractOperatorResult)operatorResult));
            }, cancellationTokenSource.Token);

            lock (taskList)
                taskList.Add(task);
        }

        public void WaitInspect()
        {
            Task.WaitAll(taskList.ToArray());

            taskList.Clear();

            SystemManager.Instance().OperatorCompleted(new InspectOperatorResult(resultKey,null, "Completed"));
        }

        private InspectOperatorResult Inspect(ExtractOperatorResult extractOperatorResult)
        {
            if (resultKey.Model.IsTaught() == false)
                return new InspectOperatorResult(resultKey, extractOperatorResult, "Not Teached");

            List<IResultObject> defectList = new List<IResultObject>();
            if (extractOperatorResult.BlobRectList == null)
                return new InspectOperatorResult(resultKey, extractOperatorResult, "No Pattern Exist");

            List<ShapeDefect> shapeDefectList = new List<ShapeDefect>();
            List<PatternDefect> patternDefectList = new List<PatternDefect>();
            List<MarginDefect> marginDefectList = new List<MarginDefect>();
            List<BlobRect> refBlobRectList = new List<BlobRect>();
            List<LengthMeasure> lengthMeasureList = new List<LengthMeasure>();
            List<MeanderMeasure> meanderMeasureList = new List<MeanderMeasure>();

            List<Task> subTaskList = new List<Task>();
            subTaskList.Add(Task.Factory.StartNew(() => { ShapeInspect(extractOperatorResult, refBlobRectList, shapeDefectList); }, cancellationTokenSource.Token));
            subTaskList.Add(Task.Factory.StartNew(() => { PatternInspect(extractOperatorResult, patternDefectList); }, cancellationTokenSource.Token));
            subTaskList.Add(Task.Factory.StartNew(() => { MarginInspect(extractOperatorResult, marginDefectList); }, cancellationTokenSource.Token));
            subTaskList.Add(Task.Factory.StartNew(() => { HeightMeasure(extractOperatorResult, lengthMeasureList); }, cancellationTokenSource.Token));
            subTaskList.Add(Task.Factory.StartNew(() => { MeanderMeasure(extractOperatorResult, meanderMeasureList); }, cancellationTokenSource.Token));

            Task.WaitAll(subTaskList.ToArray());

            if (cancellationTokenSource.IsCancellationRequested == true)
                return new InspectOperatorResult(resultKey, extractOperatorResult, "Cancelled");

            List <PatternDefect> containResult = new List<PatternDefect>();
            foreach (PatternDefect patternDefect in patternDefectList)
            {
                foreach (BlobRect blobRect in refBlobRectList)
                {
                    if (patternDefect.DefectBlob.BoundingRect.IntersectsWith(blobRect.BoundingRect) == true)
                    {
                        lock (containResult)
                            containResult.Add(patternDefect);

                        break;
                    }
                }
            }

            if (cancellationTokenSource.IsCancellationRequested == true)
                return new InspectOperatorResult(resultKey, extractOperatorResult, "Cancelled");

            System.Diagnostics.Debug.WriteLine(string.Format("Inspect {0}, Done", extractOperatorResult.ScanOperatorResult.FlowPosition));

            defectList.AddRange(lengthMeasureList.ToArray());
            defectList.AddRange(meanderMeasureList.ToArray());
            defectList.AddRange(shapeDefectList.ToArray());
            defectList.AddRange(containResult.ToArray());
            defectList.AddRange(marginDefectList.ToArray());
            return new InspectOperatorResult(resultKey, extractOperatorResult, defectList);
        }

        public void Inspect(List<ExtractOperatorResult> extractOperatorResultList, CancellationTokenSource cancellationTokenSource)
        {
            OperatorState = OperatorState.Run;

            List<InspectOperatorResult> InspectOperatorResultList = new List<InspectOperatorResult>();

            List<Task> tempTaskList = new List<Task>();
            foreach (ExtractOperatorResult extractOperatorResult in extractOperatorResultList)
            {
                tempTaskList.Add(Task.Factory.StartNew(() =>
                {
                    SystemManager.Instance().OperatorProcessed(Inspect(extractOperatorResult));
                }, cancellationTokenSource.Token));
            }

            Task.WaitAll(tempTaskList.ToArray());

            OperatorState = OperatorState.Idle;

            SystemManager.Instance().OperatorCompleted(new InspectOperatorResult(resultKey, null, "Completed"));
        }

        private void ShapeInspect(ExtractOperatorResult extractOperatorResult, List<BlobRect> refBlobRectList, List<ShapeDefect> shapeDefectList)
        {
            AlgoImage algoImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            RectangleF sourceRect = new RectangleF(0, 0, algoImage.Width, algoImage.Height);

            List<BlobRect> refList = new List<BlobRect>();
            List<ShapeDefect> defectList = new List<ShapeDefect>();
            Parallel.ForEach(extractOperatorResult.BlobRectList, blobRect =>
            {
                List<Tuple<PatternFeature, float>> minDiffList = null;
                float minDiffSumValue = float.MaxValue;

                foreach (PatternGroup patternGroup in resultKey.Model.InspectPatternList)
                {
                    List<Tuple<PatternFeature, float>> diffList = new List<Tuple<PatternFeature, float>>();
                    foreach (PatternFeature feature in Enum.GetValues(typeof(PatternFeature)))
                        diffList.Add(new Tuple<PatternFeature, float>(feature, patternGroup.GetDiffValue(feature, blobRect)));

                    float sumValue = diffList.Sum(diff => diff.Item2);
                    if (sumValue < minDiffSumValue)
                    {
                        minDiffSumValue = sumValue;
                        minDiffList = diffList;
                    }

                    if (Math.Abs(diffList.Max(diff => diff.Item2)) < Math.Max(1, settings.DiffThreshold / DeveloperSettings.Instance.Resolution))
                    {
                        lock (refList)
                            refList.Add(blobRect);

                        return;
                    }
                }
                
                foreach (PatternGroup patternGroup in resultKey.Model.CandidatePatternList)
                {
                    List<Tuple<PatternFeature, float>> diffList = new List<Tuple<PatternFeature, float>>();
                    foreach (PatternFeature feature in Enum.GetValues(typeof(PatternFeature)))
                        diffList.Add(new Tuple<PatternFeature, float>(feature, patternGroup.GetDiffValue(feature, blobRect)));

                    float sumValue = diffList.Sum(diff => diff.Item2);
                    if (sumValue < minDiffSumValue)
                    {
                        minDiffSumValue = sumValue;
                        minDiffList = diffList;
                    }

                    if (Math.Abs(diffList.Max(diff =>diff.Item2)) < Math.Max(1, settings.DiffThreshold / DeveloperSettings.Instance.Resolution))
                        return;
                }
                
                RectangleF rect = blobRect.BoundingRect;
                rect.Inflate(inflate, inflate);
                rect.Intersect(sourceRect);

                AlgoImage defectImage = algoImage.GetSubImage(Rectangle.Truncate(rect));
                lock (defectList)
                    defectList.Add(new ShapeDefect(blobRect, defectImage.ToBitmapSource(), minDiffList));

                defectImage.Dispose();
            });

            refBlobRectList.AddRange(refList.ToArray());
            shapeDefectList.AddRange(defectList.ToArray());
        }

        private void PatternInspect(ExtractOperatorResult extractOperatorResult, List<PatternDefect> commonDefectList)
        {
            AlgoImage patternMask = BufferManager.Instance().GetInspectBuffer();
            AlgoImage patternBuffer = BufferManager.Instance().GetInspectBuffer();

            AlgoImage algoImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            float patternIgnoreRangeLength = settings.PatternIgnoreRangeLength;
            if (patternIgnoreRangeLength < 0)
                patternIgnoreRangeLength = DeveloperSettings.Instance.Resolution * 5;
            imageProcessing.Erode(extractOperatorResult.MaskBuffer, patternMask, (int)(patternIgnoreRangeLength / DeveloperSettings.Instance.Resolution));
            
            RectangleF sourceRect = new RectangleF(0, 0, algoImage.Width, algoImage.Height);

            List<Tuple<float, Rectangle>> regionList = new List<Tuple<float, Rectangle>>();

            int regionNum = algoImage.Width / regionWidth;
            for (int i = 0; i < regionNum; i++)
            //Parallel.For(0, regionNum, i =>
            {
                Rectangle subRect = new Rectangle(i * regionWidth, 0, regionWidth, algoImage.Height);
                AlgoImage subImage = algoImage.GetSubImage(subRect);
                AlgoImage maskSubImage = patternMask.GetSubImage(subRect);
                AlgoImage binSubImage = patternBuffer.GetSubImage(subRect);

                int avg = (int)Math.Round(imageProcessing.GetGreyAverage(subImage, maskSubImage));
                if (avg != 0)
                {
                    //binSubImage.Copy(subImage);
                    imageProcessing.Average(subImage, binSubImage);
                    //imageProcessing.Average(binSubImage, binSubImage);
                    imageProcessing.Binarize(binSubImage, binSubImage, (int)Math.Max(0, avg - settings.PatternLower), (int)Math.Min(255, avg + settings.PatternUpper), true);
                    regionList.Add(new Tuple<float, Rectangle>(avg, subRect)); 
                }

                subImage.Dispose();
                maskSubImage.Dispose();
                binSubImage.Dispose();
            }
            
            imageProcessing.And(patternBuffer, patternMask, patternBuffer);
            
            BlobParam blobParam = new BlobParam();

            blobParam.MaxCount = 0;
            blobParam.SelectArea = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectMeanValue = true;
            blobParam.SelectMinValue = true;
            blobParam.SelectMaxValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectBoundingRect = true;
            blobParam.RotateWidthMin = settings.PatternMinDefectSize / DeveloperSettings.Instance.Resolution;

            if (settings.UsePatternMaxDefectSize)
                blobParam.RotateWidthMax = settings.PatternMaxDefectSize / DeveloperSettings.Instance.Resolution;

            BlobRectList blobRectList = imageProcessing.Blob(patternBuffer, blobParam, algoImage);
            BufferManager.Instance().AddDispoableObj(blobRectList);

            BufferManager.Instance().PutInspectBuffer(patternBuffer);
            BufferManager.Instance().PutInspectBuffer(patternMask);

            List<PatternDefect> pdList = new List<PatternDefect>();
            foreach (BlobRect blobRect in blobRectList.GetList())
            {
                List<Tuple<float, Rectangle>> foundList = regionList.FindAll(region => region.Item2.IntersectsWith(Rectangle.Round(blobRect.BoundingRect)));
                if (foundList.Count == 0)
                    continue;

                float avg = foundList.Average(f => f.Item1);
                
                RectangleF rect = blobRect.BoundingRect;
                rect.Inflate(inflate, inflate);
                rect.Intersect(sourceRect);
                AlgoImage defectImage = algoImage.GetSubImage(Rectangle.Truncate(rect));
                float defectValue = blobRect.MeanValue - avg;
                if (-settings.PatternLower > defectValue || defectValue > settings.PatternUpper)
                {
                    lock (pdList)
                        pdList.Add(new PatternDefect(blobRect, defectImage.ToBitmapSource(), defectValue));
                }
                else
                {
                    //patternBuffer.Save(@"d:\temp\patternBuffer.bmp");
                    //patternMask.Save(@"d:\temp\patternMask.bmp");
                    //algoImage.Save(@"d:\temp\algoImage.bmp");
                    lock (pdList)
                        pdList.Add(new PatternDefect(blobRect, defectImage.ToBitmapSource(), defectValue));
                }

                defectImage.Dispose();
            }

            commonDefectList.AddRange(pdList.ToList());
        }
        
        private void MarginInspect(ExtractOperatorResult extractOperatorResult, List<MarginDefect> commonDefectList)
        {
            string debugContextSubPath = string.Format("InspectorOperator_MarginInspect_{0}", extractOperatorResult.ScanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);
            
            AlgoImage marginMask = BufferManager.Instance().GetInspectBuffer();
            AlgoImage marginBuffer = BufferManager.Instance().GetInspectBuffer();

            AlgoImage backImage = extractOperatorResult.ScanOperatorResult.BackLightImage;
            AlgoImage topImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
            AlgoImage maskBuffer = extractOperatorResult.MaskBuffer;
            backImage.Save("backImage.bmp", debugContext);
            topImage.Save("topImage.bmp", debugContext);
            maskBuffer.Save("maskBuffer.bmp", debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(backImage);
            
            RectangleF sourceRect = new RectangleF(0, 0, backImage.Width, backImage.Height);
            
            // 테두리를 제거하여 패턴이 잘려서 그랩된 오류 제거
            Rectangle validRect = new Rectangle(Point.Empty, topImage.Size);
            int invalidLength = (int)Math.Round(SystemManager.Instance().OperatorManager.ScanOperator.Settings.OverlapUm / 2 / DeveloperSettings.Instance.Resolution);
            validRect.Inflate(-invalidLength, -invalidLength);
            marginMask.Clear(255);
            marginMask.Copy(maskBuffer, validRect.Location, validRect.Location, validRect.Size);
            marginMask.Save("marginBuffer0.bmp", debugContext);

            // 패턴 마스크를 NOT하여 마진 마스크 생성
            imageProcessing.Not(marginMask, marginMask);
            marginMask.Save("marginMask1.bmp", debugContext);

            // Erode 하여 마스크에 여유분 생성
            float marginIgnoreRangeLength = settings.MarginIgnoreRangeLength;
            if (marginIgnoreRangeLength < 0)
                marginIgnoreRangeLength = DeveloperSettings.Instance.Resolution * 5;
            imageProcessing.Erode(marginMask, (int)(marginIgnoreRangeLength / DeveloperSettings.Instance.Resolution));
            marginMask.Save("marginMask2.bmp", debugContext);
            imageProcessing.And(marginMask, extractOperatorResult.SheetBuffer, marginMask);
            marginMask.Save("marginMask3.bmp", debugContext);

            int binarizeValue = SystemManager.Instance().CurrentModel.BinarizeValueTop;
            imageProcessing.Binarize(topImage, marginBuffer, binarizeValue);
            marginBuffer.Save("marginBuffer1.bmp", debugContext);
            extractOperatorResult.SheetBuffer.Save("SheetBuffer.bmp", debugContext);
            imageProcessing.And(marginBuffer, extractOperatorResult.SheetBuffer, marginBuffer);
            marginBuffer.Save("marginBuffer2.bmp", debugContext);
            imageProcessing.Erode(marginBuffer, marginBuffer, (int)(marginIgnoreRangeLength / DeveloperSettings.Instance.Resolution));
            marginBuffer.Save("marginBuffer3.bmp", debugContext);

            float overallMarginAvg = imageProcessing.GetGreyAverage(topImage, marginBuffer);
            float limit = overallMarginAvg * 0.7f;
            List<Tuple<float, Rectangle>> regionList = new List<Tuple<float, Rectangle>>();

            int regionNum = topImage.Width / regionWidth;
            for (int i =  0; i < regionNum; i++)
            {
                Rectangle subRect = new Rectangle(i * regionWidth, 0, regionWidth, topImage.Height);
                AlgoImage subImage = topImage.GetSubImage(subRect);
                AlgoImage maskSubImage = marginBuffer.GetSubImage(subRect);
                AlgoImage binSubImage = marginBuffer.GetSubImage(subRect);

                subImage.Save("subImage.bmp", debugContext);
                maskSubImage.Save("maskSubImage.bmp", debugContext);
                int avg = (int)Math.Round(imageProcessing.GetGreyAverage(subImage, maskSubImage));
                //if (avg > limit)
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("MarginInspect: Subrect Avg: {0}", avg));
                    imageProcessing.Average(subImage, binSubImage);
                    imageProcessing.Binarize(binSubImage, binSubImage, (int)Math.Max(0, avg - settings.MarginLower), (int)Math.Min(255, avg + settings.MarginUpper), true);
                    binSubImage.Save("binSubImage.bmp", debugContext);
                    regionList.Add(new Tuple<float, Rectangle>(avg, subRect));
                }

                subImage.Dispose();
                maskSubImage.Dispose();
                binSubImage.Dispose();
            }
            marginBuffer.Save("marginBuffer4.bmp", debugContext);

            imageProcessing.And(marginBuffer, marginMask, marginBuffer);
            marginBuffer.Save("marginBuffer5.bmp", debugContext);

            BlobParam blobParam = new BlobParam();

            blobParam.MaxCount = 0;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectMeanValue = true;
            blobParam.SelectMinValue = true;
            blobParam.SelectMaxValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectBoundingRect = true;
            blobParam.RotateWidthMin = settings.MarginMinDefectSize / DeveloperSettings.Instance.Resolution;
            if (settings.UseMarginMaxDefectSize)
                blobParam.RotateWidthMax = settings.MarginMaxDefectSize / DeveloperSettings.Instance.Resolution;

            BlobRectList blobRectList = imageProcessing.Blob(marginBuffer, blobParam, topImage);
            BufferManager.Instance().AddDispoableObj(blobRectList);

            BufferManager.Instance().PutInspectBuffer(marginMask);
            BufferManager.Instance().PutInspectBuffer(marginBuffer);

            List<MarginDefect> mdList = new List<MarginDefect>();
            foreach (BlobRect blobRect in blobRectList.GetList())
            {
                if (RectangleF.Intersect(extractOperatorResult.SheetRect, blobRect.BoundingRect) != blobRect.BoundingRect)
                    continue;

                List<Tuple<float, Rectangle>> foundList = regionList.FindAll(region => region.Item2.IntersectsWith(Rectangle.Round(blobRect.BoundingRect)));
                if (foundList.Count == 0)
                    continue;

                float avg = 0;
                foundList.ForEach(found => avg += found.Item1);

                avg /= foundList.Count;

                RectangleF rect = blobRect.BoundingRect;
                rect.Inflate(inflate, inflate);
                rect.Intersect(sourceRect);

                AlgoImage defectImage = topImage.GetSubImage(Rectangle.Truncate(rect));

                lock (mdList)
                    //mdList.Add(new MarginDefect(blobRect, defectImage.ToBitmapSource(), blobRect.MeanValue < avg ? blobRect.MaxValue - avg : blobRect.MinValue - avg));
                    mdList.Add(new MarginDefect(blobRect, defectImage.ToBitmapSource(), blobRect.MeanValue - avg));

                defectImage.Dispose();
            }
            commonDefectList.AddRange(mdList.ToArray());
        }

        private void HeightMeasure(ExtractOperatorResult extractOperatorResult, List<LengthMeasure> lengthMeasureList)
        {
            float resolution = DeveloperSettings.Instance.Resolution;
            Point srcPt = new Point((extractOperatorResult.SheetRect.Left + extractOperatorResult.SheetRect.Right) / 2, extractOperatorResult.SheetRect.Top);
            Point dstPt = new Point((extractOperatorResult.SheetRect.Left + extractOperatorResult.SheetRect.Right) / 2, extractOperatorResult.SheetRect.Bottom);
            double value = extractOperatorResult.SheetRect.Height * resolution/1000;

            if (value > 0)
                lengthMeasureList.Add(new LengthMeasure(Direction.Vertical, srcPt, dstPt, value, false));
        }

        private void MeanderMeasure(ExtractOperatorResult extractOperatorResult, List<MeanderMeasure> meanderMeasureList)
        {
            string debugContextSubPath = string.Format("InspectorOperator_MeanderMeasure_{0}", extractOperatorResult.ScanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (extractOperatorResult.ScanOperatorResult.FlowPosition == 0)
            {
                AlgoImage backLightImage = extractOperatorResult.ScanOperatorResult.BackLightImage;
                Rectangle sheetRect = extractOperatorResult.SheetRect;
                if (sheetRect.Width==0 || sheetRect.Height==0)
                    return;

                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(backLightImage);
                AlgoImage clipImage = backLightImage.Clip(Rectangle.FromLTRB(0, sheetRect.Top, backLightImage.Width, sheetRect.Bottom));
                imageProcessing.Average(clipImage);
                imageProcessing.Average(clipImage);
                clipImage.Save("Measure.Average.bmp", debugContext);

                int subDataSize = clipImage.Height / 3;
                int subDataSrc = 0;
                int subDataDst = subDataSize;
                for (int idxArea = 0; idxArea < 3; idxArea++)
                {
                    AlgoImage subImage = clipImage.GetSubImage(new Rectangle(0, subDataSrc, clipImage.Width, subDataSize));
                    string fileName = string.Format(@"Measure.Average.Sub{0}", idxArea);
                    subImage.Save(string.Format(@"{0}.bmp", fileName), debugContext);
                    float[] subData = imageProcessing.Projection(subImage, DynMvp.Vision.Direction.Horizontal, ProjectionType.Mean);
                    subImage.Dispose();

                    int fallSrc = -1, fallDst = -1, fallMaxPos = -1;
                    float fallAcc = 0, fallMaxVal = 0;
                    int upCount = -1;
                    List<Tuple<int, int, int, float>> fallList = new List<Tuple<int, int, int, float>>();
                    for (int i = 1; i < subData.Length; i++)
                    {
                        float diff = subData[i] - subData[i - 1];
                        if (diff < 0) // Projection is falling 
                        {
                            upCount = 0;

                            if (fallSrc < 0)
                                fallSrc = i - 1;
                            fallDst = i;
                            fallAcc += diff;
                            if (diff < fallMaxVal)
                            {
                                fallMaxPos = i;
                                fallMaxVal = diff;
                            }
                        }
                        else  // Projection is rising or horizen 
                        {
                            upCount++;

                            if (upCount > (settings.MeanderSensitivity - 100) + 5)
                            {
                                if (fallSrc >= 0 && fallAcc <= (settings.MeanderSensitivity - 100) * 2.55f)
                                    fallList.Add(new Tuple<int, int, int, float>(fallSrc, fallDst, fallMaxPos, fallAcc));
                                fallSrc = fallDst = fallMaxPos = -1;
                                fallAcc = fallMaxVal = 0;
                                upCount = 0;
                            }
                        }
                    }
                    //fallList.Sort(Comp);
                    if (fallList.Count >= 3)
                    {
                        System.Diagnostics.Debug.WriteLine(fallList[0].ToString());
                        System.Diagnostics.Debug.WriteLine(fallList[1].ToString());
                        System.Diagnostics.Debug.WriteLine(fallList[2].ToString());

                        //int[] lines = new int[] { fallList[0].Item3, fallList[1].Item3, fallList[2].Item3 };
                        int[] lines = new int[] {
                            (fallList[0].Item1+ fallList[0].Item2)/2,
                            (fallList[1].Item1+ fallList[1].Item2)/2,
                            (fallList[2].Item1+ fallList[2].Item2)/2,
                           };
                        meanderMeasureList.Add(new MeanderMeasure(sheetRect.Top + ((subDataDst + subDataSrc) / 2), lines[0], lines[1], lines[2]));
                        if (debugContext.SaveDebugImage)
                            System.IO.File.WriteAllText(System.IO.Path.Combine(debugContext.FullPath, string.Format("{0}.txt", fileName)), string.Format("{0} - {1} - {2}", lines[0], lines[1], lines[2]));
                    }
                    subDataSrc = subDataDst;
                    subDataDst += subDataSize;
                }
                clipImage.Dispose();
            }

            //Debug.WriteLine(string.Format("MeanderMeasure {0}", stopwatch.Elapsed));
        }

        public void Inspect2()
        {
            OperatorState = OperatorState.Run;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                List<IResultObject> resultObjectList = new List<IResultObject>();

                ResultCombiner resultCombiner = SystemManager.Instance().OperatorManager.ResultCombiner;

                // filter height length measure data
                {
                    List<CanvasDefect> canvasDefectList = null;
                    lock (resultCombiner.CombineDefectList)
                    {
                        canvasDefectList = resultCombiner.CombineDefectList.ToList();
                    }
                    List<LengthMeasure> lengthMeasureList = canvasDefectList.FindAll(f => f.Defect is LengthMeasure).ConvertAll(f => (LengthMeasure)f.Defect);
                    lengthMeasureList.ForEach(f => f.IsValid = false);

                    // Update Height
                    List<LengthMeasure> verticalList = lengthMeasureList.FindAll(f => f.Direction == Direction.Vertical && f.LengthMm > 0);
                    if (verticalList.Count > 0)
                    {
                        int indexSrc = 0;
                        int indexDst = verticalList.Count - 1;
                        int indexMid = (indexSrc + indexDst) / 2;

                        if (indexSrc >= 0)
                            verticalList[indexSrc].IsValid = true;

                        if (indexMid >= 0)
                            verticalList[indexMid].IsValid = true;

                        if (indexDst >= 0)
                            verticalList[indexDst].IsValid = true;
                    }
                }

                ExtractOperatorResult extractOperatorResult = resultCombiner.ExtractOperatorResultArray.LastOrDefault() as ExtractOperatorResult;

                List<LengthMeasure> widthMeasureList = new List<LengthMeasure>();
                WidthMeasure(resultCombiner, widthMeasureList);

                List<ExtraMeasure> marginMeasure = new List<ExtraMeasure>();
                MarginMeasure(resultCombiner, marginMeasure);

                resultObjectList.AddRange(widthMeasureList);
                resultObjectList.AddRange(marginMeasure);

                resultCombiner.AddResult(new InspectOperatorResult(null, null, resultObjectList));
            }catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
            }

            OperatorState = OperatorState.Idle;

            Debug.WriteLine(string.Format("Inspect2 {0}", stopwatch.Elapsed));
        }

        private void WidthMeasure(ResultCombiner resultCombiner, List<LengthMeasure> lengthMeasureList)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DebugContext debugContext = this.GetDebugContext("InspectorOperator_WidthMeasure");
            float res = DeveloperSettings.Instance.Resolution;
            List<Point[]> vertexPointList = new List<Point[]>();
            ExtractOperatorResult[] operatorResults = resultCombiner.ExtractOperatorResultArray;

            ExtractOperatorResult srcResult = operatorResults.FirstOrDefault(f => f?.VertexPoints != null);
            ExtractOperatorResult dstResult = operatorResults.LastOrDefault(f => f?.VertexPoints != null);

            if (srcResult == null || dstResult == null)
                return;

            PointF[] src = new PointF[3] { srcResult.VertexPoints[0], srcResult.VertexPoints[3], Point.Empty };
            PointF[] dst = new PointF[3] { dstResult.VertexPoints[1], dstResult.VertexPoints[2], Point.Empty };
            src[2] = DrawingHelper.CenterPoint(src[0], src[1]);
            dst[2] = DrawingHelper.CenterPoint(dst[0], dst[1]);

            for (int i = 0; i < 3; i++)
            {
                PointF calcSrc = new PointF(srcResult.ScanOperatorResult.AxisPosition[0] - (src[i].X * res), srcResult.ScanOperatorResult.AxisPosition[1] + (src[i].Y * res));
                PointF calcDst = new PointF(dstResult.ScanOperatorResult.AxisPosition[0] - (dst[i].X * res), dstResult.ScanOperatorResult.AxisPosition[1] + (dst[i].Y * res));
                double calcDist = MathHelper.GetLength(calcSrc, calcDst) / 1000;

                PointF dispSrc = new PointF(srcResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.ResizeRatio + (src[i].X), srcResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.ResizeRatio + (src[i].Y));
                PointF dispDst = new PointF(dstResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.ResizeRatio + (dst[i].X), dstResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.ResizeRatio + (dst[i].Y));
                lengthMeasureList.Add(new LengthMeasure(Direction.Horizontal, dispSrc, dispDst, calcDist,true));
            }

            Debug.WriteLine(string.Format("WidthMeasure {0}", stopwatch.Elapsed));
        }

        private void MarginMeasure(ResultCombiner resultCombiner, List<ExtraMeasure> marginMeasure)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DebugContext debugContext = this.GetDebugContext("InspectorOperator_MarginMeasure");
            float res = DeveloperSettings.Instance.Resolution;

            List<ExtractOperatorResult> extractOperatorResultList = resultCombiner.ExtractOperatorResultArray.ToList();
            int src = extractOperatorResultList.FindIndex(f => f?.VertexPoints != null);
            int dst = extractOperatorResultList.FindLastIndex(f => f?.VertexPoints != null);
            if (src < 0 || dst < 0)
                return;

            int mid = (src + dst) / 2;

            List<MarginMeasurePoint> marginMeasurePointList = SystemManager.Instance().CurrentModel.MarginMeasurePointList;
            for (int i=0; i< marginMeasurePointList.Count; i++)
            {
                MarginMeasurePoint marginMeasurePoint = marginMeasurePointList[i];

                // 검사 대상 패턴 좌표 찾기
                System.Windows.Point offset = marginMeasurePoint.Point;
                double targetPointX = -1;  // 로봇좌표계 기준
                double targetPointY = -1;
                ExtractOperatorResult extractOperatorResult;

                switch (marginMeasurePoint.ReferencePos)
                {
                    case ReferencePos.LT:
                        extractOperatorResult = extractOperatorResultList[src];
                        targetPointX = extractOperatorResult.ScanOperatorResult.AxisPosition[0] - extractOperatorResult.VertexPoints[0].X * res - offset.X * 1000;
                        targetPointY = extractOperatorResult.ScanOperatorResult.AxisPosition[1] + extractOperatorResult.VertexPoints[0].Y * res + offset.Y * 1000;
                        break;
                    case ReferencePos.RT:
                        extractOperatorResult = extractOperatorResultList[dst];
                        targetPointX = extractOperatorResult.ScanOperatorResult.AxisPosition[0] - extractOperatorResult.VertexPoints[1].X * res + offset.X * 1000;
                        targetPointY = extractOperatorResult.ScanOperatorResult.AxisPosition[1] + extractOperatorResult.VertexPoints[1].Y * res + offset.Y * 1000;
                        break;
                    case ReferencePos.RB:
                        extractOperatorResult = extractOperatorResultList[dst];
                        targetPointX = extractOperatorResult.ScanOperatorResult.AxisPosition[0] - extractOperatorResult.VertexPoints[2].X * res + offset.X * 1000;
                        targetPointY = extractOperatorResult.ScanOperatorResult.AxisPosition[1] + extractOperatorResult.VertexPoints[2].Y * res - offset.Y * 1000;
                        break;
                    case ReferencePos.LB:
                        extractOperatorResult = extractOperatorResultList[src];
                        targetPointX = extractOperatorResult.ScanOperatorResult.AxisPosition[0] - extractOperatorResult.VertexPoints[3].X * res - offset.X * 1000;
                        targetPointY = extractOperatorResult.ScanOperatorResult.AxisPosition[1] + extractOperatorResult.VertexPoints[3].Y * res - offset.Y * 1000;
                        break;
                    //case ReferencePos.CT:
                    //    break;
                }

                PointF targetPoint = new PointF((float)targetPointX, (float)targetPointY);
                // 검사 대상 패턴이 있는 FlowPosition 찾기
                ExtractOperatorResult targetExtractOperatorResult = extractOperatorResultList.Find(g =>
                {
                    PointF fovPos = new PointF(g.ScanOperatorResult.AxisPosition[0], g.ScanOperatorResult.AxisPosition[1]);
                    SizeF fovSize = new SizeF(g.ScanOperatorResult.TopLightImage.Width * res, g.ScanOperatorResult.TopLightImage.Height * res);
                    RectangleF fovRect = new RectangleF(fovPos, fovSize);
                    fovRect.Offset(-fovSize.Width, 0);
                    return fovRect.Contains(targetPoint);
                });

                if (targetExtractOperatorResult == null)
                    continue;

                // 거리가 가장 가까운 BlobRect 찾기
                Size imageSize = targetExtractOperatorResult.MaskBuffer.Size;
                float targetPointXPx = (float)(-(targetPointX - targetExtractOperatorResult.ScanOperatorResult.AxisPosition[0]) / res);
                float targetPointYPx = (float)((targetPointY - targetExtractOperatorResult.ScanOperatorResult.AxisPosition[1]) / res);
                PointF targetPointPx = new PointF(targetPointXPx, targetPointYPx);
                List<BlobRect> orderBlobRectList = targetExtractOperatorResult.BlobRectList.OrderBy(f => MathHelper.GetLength(targetPointPx, f.CenterPt)).ToList();
                BlobRect targetBlobRect = orderBlobRectList.First();

                // BlobRect와 가장 인접한 패턴까지의 거리 구하기
                Tuple<RectangleF, Func<RectangleF, RectangleF, float>>[] tuples = new Tuple<RectangleF, Func<RectangleF, RectangleF, float>>[]
                {
                        new Tuple<RectangleF, Func<RectangleF,RectangleF,float>>( RectangleF.FromLTRB(0, targetBlobRect.BoundingRect.Top, targetBlobRect.BoundingRect.Left, targetBlobRect.BoundingRect.Bottom), new Func<RectangleF,RectangleF, float>((f,g)=>f.Right - g.Right)),
                        new Tuple<RectangleF, Func<RectangleF,RectangleF,float>>(RectangleF.FromLTRB(targetBlobRect.BoundingRect.Left, 0, targetBlobRect.BoundingRect.Right, targetBlobRect.BoundingRect.Top), new Func<RectangleF,RectangleF, float>((f,g)=>f.Bottom- g.Bottom)),
                        new Tuple<RectangleF, Func<RectangleF,RectangleF,float>>(RectangleF.FromLTRB(targetBlobRect.BoundingRect.Right, targetBlobRect.BoundingRect.Top, imageSize.Width, targetBlobRect.BoundingRect.Bottom), new Func<RectangleF,RectangleF, float>((f,g)=>g.Left - f.Left)),
                        new Tuple<RectangleF, Func<RectangleF,RectangleF,float>>(RectangleF.FromLTRB(targetBlobRect.BoundingRect.Left, targetBlobRect.BoundingRect.Bottom, targetBlobRect.BoundingRect.Right, imageSize.Height), new Func<RectangleF,RectangleF, float>((f,g)=>g.Top - f.Top))
                };

                // UI 좌표에 맞게 변환 후 리스트에 Add
                PointF dispPoint = new PointF(targetExtractOperatorResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.ResizeRatio,
                    targetExtractOperatorResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.ResizeRatio);
                RectangleF dispRect =targetBlobRect.BoundingRect;
                dispRect.Offset(dispPoint);

                float[] distUm = new float[]
                {
                    Math.Max(0,GetMarginDistancePx(orderBlobRectList, tuples[0]) * res),
                    Math.Max(0,GetMarginDistancePx(orderBlobRectList, tuples[1]) * res),
                    Math.Max(0,GetMarginDistancePx(orderBlobRectList, tuples[2]) * res),
                    Math.Max(0,GetMarginDistancePx(orderBlobRectList, tuples[3]) * res),
                };
                marginMeasure.Add(new Data.ExtraMeasure(marginMeasurePoint.ReferencePos, dispRect, distUm));
            }

            Debug.WriteLine(string.Format("MarginMeasure {0}", stopwatch.Elapsed));
        }

        private float GetMarginDistancePx(List<BlobRect> orderBlobRectList, Tuple<RectangleF, Func<RectangleF, RectangleF, float>> tuple)
        {
            RectangleF searchRect = tuple.Item1;
            Func<RectangleF, RectangleF, float> func = tuple.Item2;

            List<BlobRect> findAll = orderBlobRectList.FindAll(f => searchRect.IntersectsWith(f.BoundingRect));
            if (findAll.Count == 0)
                return -1;
            return findAll.Min(f => func(searchRect, f.BoundingRect));
        }

        public int Comp(Tuple<int, int, float> a, Tuple<int, int, float>  b)
        {
            //return a.Item3.CompareTo(b.Item3);
            return b.Item3.CompareTo(a.Item3);
        }
        //Comparison<Tuple<int, int, float>> comparison
    }


    public class InspectOperatorResult : OperatorResult
    {
        ExtractOperatorResult extractOperatorResult;
        List<IResultObject> defectList;
        DateTime inspectTime;
        
        public ExtractOperatorResult ExtractOperatorResult { get => extractOperatorResult; }
        public List<IResultObject> DefectList { get => defectList; }
        public DateTime InspectTime { get => inspectTime; }

        public InspectOperatorResult(ResultKey resultKey, ExtractOperatorResult extractOperatorResult, List<IResultObject> defectList) : base(ResultType.Inspect, resultKey, null)
        {
            this.inspectTime = DateTime.Now;
            this.extractOperatorResult = extractOperatorResult;
            this.defectList = defectList;
        }

        public InspectOperatorResult(ResultKey resultKey, ExtractOperatorResult extractOperatorResult, string exeptionMessage) : base(ResultType.Inspect, resultKey, exeptionMessage)
        {
            this.extractOperatorResult = extractOperatorResult;
            this.defectList = new List<IResultObject>();
        }
    }

    public class InspectOperatorSettings : OperatorSettings
    {
        bool usePatternMaxDefectSize = false;
        bool useMarginMaxDefectSize = false;
        float patternIgnoreRangeLength = -1;
        float marginIgnoreRangeLength = -1;
        float patternMinDefectSize = 50;
        float patternMaxDefectSize = 50000;
        float marginMinDefectSize = 50;
        float marginMaxDefectSize = 50000;
        float diffThreshold = 25;
        float patternLower = 20;
        float patternUpper = 20;
        float marginLower = 20;
        float marginUpper = 10;

        float meanderSensitivity = 95;

        public float PatternIgnoreRangeLength { get => patternIgnoreRangeLength; set => patternIgnoreRangeLength = value; }
        public float MarginIgnoreRangeLength { get => marginIgnoreRangeLength; set => marginIgnoreRangeLength = value; }

        public float PatternLower { get => patternLower; set => patternLower = value; }
        public float PatternUpper { get => patternUpper; set => patternUpper = value; }
        public float MarginLower { get => marginLower; set => marginLower = value; }
        public float MarginUpper { get => marginUpper; set => marginUpper = value; }

        public bool UsePatternMaxDefectSize { get => usePatternMaxDefectSize; set => usePatternMaxDefectSize = value; }
        public bool UseMarginMaxDefectSize { get => useMarginMaxDefectSize; set => useMarginMaxDefectSize = value; }
        public float PatternMinDefectSize { get => patternMinDefectSize; set => patternMinDefectSize = value; }
        public float PatternMaxDefectSize { get => patternMaxDefectSize; set => patternMaxDefectSize = value; }
        public float MarginMinDefectSize { get => marginMinDefectSize; set => marginMinDefectSize = value; }
        public float MarginMaxDefectSize { get => marginMaxDefectSize; set => marginMaxDefectSize = value; }

        public float MeanderSensitivity { get => meanderSensitivity; set => meanderSensitivity = Math.Min(100, Math.Max(0, value)); }

        public float DiffThreshold
        {
            get
            {
                return diffThreshold;
            }
            set
            {
                diffThreshold = value;
            }
        }
        
        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Inspect");
        }

        public override void Load(XmlElement xmlElement)
        {
            usePatternMaxDefectSize = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UsePatternMaxDefectSize", "False"));
            useMarginMaxDefectSize = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UseMarginMaxDefectSize", "False"));
            patternIgnoreRangeLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternIgnoreRangeLength", "-1"));
            marginIgnoreRangeLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MarginIgnoreRangeLength", "-1"));
            patternMinDefectSize = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternMinDefectSize", "50"));
            patternMaxDefectSize = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternMaxDefectSize", "50000"));
            marginMinDefectSize = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MarginMinDefectSize", "50"));
            marginMaxDefectSize = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MarginMaxDefectSize", "50000"));
            diffThreshold = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "DiffThreshold", "25"));
            patternLower = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternLower", "20"));
            patternUpper = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PatternUpper", "20"));
            marginLower = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MarginLower", "20"));
            marginUpper = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MarginUpper", "20"));
            meanderSensitivity = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MeanderSensitivity", "95"));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "UsePatternMaxDefectSize", usePatternMaxDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "UseMarginMaxDefectSize", useMarginMaxDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "PatternIgnoreRangeLength", patternIgnoreRangeLength.ToString());
            XmlHelper.SetValue(xmlElement, "MarginIgnoreRangeLength", marginIgnoreRangeLength.ToString());
            XmlHelper.SetValue(xmlElement, "PatternMinDefectSize", patternMinDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "PatternMaxDefectSize", patternMaxDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "MarginMinDefectSize", marginMinDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "MarginMaxDefectSize", marginMaxDefectSize.ToString());
            XmlHelper.SetValue(xmlElement, "DiffThreshold", diffThreshold.ToString());
            XmlHelper.SetValue(xmlElement, "PatternLower", patternLower.ToString());
            XmlHelper.SetValue(xmlElement, "PatternUpper", patternUpper.ToString());
            XmlHelper.SetValue(xmlElement, "MarginLower", marginLower.ToString());
            XmlHelper.SetValue(xmlElement, "MarginUpper", marginUpper.ToString());
            XmlHelper.SetValue(xmlElement, "MeanderSensitivity", meanderSensitivity.ToString());
        }
    }
}

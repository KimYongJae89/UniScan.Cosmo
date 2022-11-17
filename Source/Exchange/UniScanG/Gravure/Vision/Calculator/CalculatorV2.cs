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
using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScan.Common.Settings;
using System.Windows.Media.Imaging;

namespace UniScanG.Gravure.Vision.Calculator
{
    public class CalculatorV2 : CalculatorBase
    {
        //public static string TypeName { get { return "Calculator"; } }

        InspectRegion[] inspectRegions = new InspectRegion[0];

        public CalculatorV2()
        {
            this.AlgorithmName = CalculatorBase.TypeName;
            this.param = new CalculatorParam();

            //int workerThreads, completionPortThreads;
            //ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            //ThreadPool. out workerThreads, out completionPortThreads);
            //ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);
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
            return CalculatorBase.TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            throw new NotImplementedException();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }

        public override ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height)
        {
            return new ProcessBufferSetG2(scaleFactor, SystemTypeSettings.Instance().ResizeRatio, isMultiLayer, width, height);
        }
        #endregion

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new CalculatorResult();
        }

        public override void PrepareInspection()
        {
            CalculatorV2Extender calculatorFTrainer = new CalculatorV2Extender();
            inspectRegions = calculatorFTrainer.Train((CalculatorParam)this.param);
        }

        public override void ClearInspection()
        {
            Array.ForEach(inspectRegions, f => f.Dispose());
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            SheetInspectParam sheetInspectParam = algorithmInspectParam as SheetInspectParam;
            CalculatorParam calculatorParam = this.param as CalculatorParam;

            RegionInfoG inspRegionInfo = sheetInspectParam.RegionInfo as RegionInfoG;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;

            ProcessBufferSetG2 processBufferSet = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG2;
            if (processBufferSet == null)
                return null;    // null 리턴시 skip

            AlgoImage fullImage = sheetInspectParam.AlgoImage;

            AlgoImage previewBuffer = processBufferSet.PreviewBuffer;
            AlgoImage scaleImage = processBufferSet.ScaledImage;
            if (scaleImage == null)
                scaleImage = fullImage;

            AlgoImage inspImage = scaleImage;
            
            AlgoImage resultImage = processBufferSet.CalculatorResult;

            DebugContext debugContext = new DebugContext(algorithmInspectParam.DebugContext.SaveDebugImage, algorithmInspectParam.DebugContext.FullPath);
            inspImage.Save("SheetImage.bmp", debugContext);

            CalculatorResult calculatorResult = (CalculatorResult)CreateAlgorithmResult();

            // Create Preview image
            //Bitmap previewBitmap = null;
            //float ratio = SystemTypeSettings.Instance().ResizeRatio;
            //Task resizeTask = new Task(() => previewBitmap = BuildPreviewBitmap(previewBuffer, debugContext));
            //resizeTask.Start();
            processBufferSet.StartPreviewBitmapBuild(fullImage.Size, calculatorResult);

            Point offsetPos = Point.Empty;
            try
            {
                int binValue = calculatorParam.BinValue;
                offsetPos = AlgorithmCommon.FindOffsetPosition(scaleImage, calculatorParam.BasePosition);
                //Debug.WriteLine(string.Format("offsetPos: {0:F2},{1:F2}", offsetPos.X, offsetPos.Y));

                sheetInspectParam.FidOffset = new SizeF(offsetPos);
                //LogHelper.Debug(LoggerType.Inspection, string.Format("SheetOffset: {0:F2},{1:F2}", offsetPos.X, offsetPos.Y));

                int InspectRegionsLength = inspectRegions.Length;
                if (calculatorParam.ParallelOperation)
                {
                    Parallel.For(0, this.inspectRegions.Length, i =>
                    {
                        InspectRegion f = this.inspectRegions[i];
                        DebugContextG newDebugContext = new DebugContextG(debugContext, i, -1, -1);
                        InspectRegion(f, inspImage, resultImage, offsetPos, cancellationToken, newDebugContext);
                    });
                }
                else
                {
                    for (int i = 0; i < InspectRegionsLength; i++)
                    {
                        InspectRegion f = this.inspectRegions[i];
                        DebugContextG newDebugContext = new DebugContextG(debugContext, i, -1, -1);
                        InspectRegion(f, inspImage, resultImage, offsetPos, cancellationToken, newDebugContext);
                    }
                }             

                resultImage.Save("CalculatorResult.bmp", debugContext);
                //resultImage.Save(@"C:\temp\CalculatorResult.bmp");
            }
//#if DEBUG==false
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Error Occure in Calculator::Inspect ({0})", ex.Message));
                //DynMvp.UI.Touch.MessageForm.Show(null, sb.ToString());

                sb.AppendLine(ex.StackTrace);
                LogHelper.Error(LoggerType.Inspection, sb.ToString());
            }
//#endif
            finally { }

            //Debug.Assert(resizeTask.Status == TaskStatus.Created || resizeTask.Status == TaskStatus.RanToCompletion);
            //WaitResizeTask(resizeTask);

            List<AlgorithmResult> algorithmResultList = new List<AlgorithmResult>();

            //calculatorResult.PrevImage = previewBitmap;
            calculatorResult.SheetSize = sheetInspectParam.AlgoImage.Size;
            calculatorResult.SheetPosOffset = offsetPos;
            calculatorResult.SubResultList.AddRange(algorithmResultList);

            //previewBitmap.Save(@"C:\temp\previewBitmap.bmp");
            return calculatorResult;
        }

        private void WaitResizeTask(Task resizeTask)
        {
            Debug.WriteLine(resizeTask.Status);
            if (resizeTask.Status != TaskStatus.Created && !resizeTask.IsCompleted)
            {
                Stopwatch sw = Stopwatch.StartNew();
                resizeTask.Wait();
                Debug.WriteLine(string.Format("CalculatorV2::WaitResizeTask {0:F3} [ms]", sw.Elapsed.TotalMilliseconds));
            }
            //sw.Stop();
        }

        private Bitmap BuildPreviewBitmap(AlgoImage previewBuffer, DebugContext debugContext)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Debug.WriteLine(string.Format("Start: {0}", sw.Elapsed.TotalMilliseconds));

            Bitmap previewBitmap = null;

            {
                previewBitmap = previewBuffer.ToBitmap();
                Debug.WriteLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            }

            //{
            //    System.Windows.Media.Imaging.BitmapSource bitmapSource = resizeSheetImage.ToBitmapSource();
            //    sb.AppendLine(string.Format("ToBitmapSource: {0}", sw.Elapsed.TotalMilliseconds));
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
            //        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //        bmpBitmapEncoder.Save(stream);

            //        previewBitmap = new Bitmap(stream);
            //    }
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //    previewBitmap?.Save(@"C:\temp\previewBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //}

            //{
            //    ImageD previewImageD = resizeSheetImage.ToImageD();
            //    sb.AppendLine(string.Format("ToImageD: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewBitmap = previewImageD.ToBitmap();
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewImageD.Dispose();
            //}

            return previewBitmap;
        }

        //private Point AlignSheet(AlgoImage algoImage, Point basePosition)
        //{
        //    Point diffPos = Point.Empty;
        //    SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
        //    if (sheerFinder == null)
        //        return Point.Empty;

        //    int foundPosX = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 20);
        //    int foundPosY = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 20);
        //    if (foundPosX >= 0 && foundPosY >= 0)
        //        diffPos = new Point(foundPosX - basePosition.X, foundPosY - basePosition.Y);

        //    return diffPos;
        //}

        private void InspectRegion(InspectRegion inspectRegion, AlgoImage inspImage, AlgoImage resultImage, Point offsetPos, CancellationToken cancellationToken, DebugContextG debugContext)
        {
            //Debug.WriteLine(string.Format("CalculatorV2::InspectRegion({0}) Start", debugContext.LogName));
            CalculatorParam calculatorParam = this.param as CalculatorParam;

            inspectRegion.Build(inspImage, resultImage, offsetPos);

            if (inspectRegion.IsBuilded)
            {
                int InspectLineSetsLength = inspectRegion.InspectLineSets.Length;
                if (calculatorParam.ParallelOperation == false)
                {
                    for (int i = 0; i < InspectLineSetsLength; i++)
                    {
                        //Stopwatch sw = Stopwatch.StartNew();

                        DebugContextG newDebugContext = new DebugContextG(debugContext, debugContext.RegionId, i, -1);

                        //if (newDebugContext.IsForceDebug)
                        //    subInspImage.Save(string.Format(@"C:\temp\subInspImage_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")));

                        InspectLineSet inspectLineSet = inspectRegion.InspectLineSets[i];
                        InspectLineSet(inspectLineSet, cancellationToken, newDebugContext);

                        //sw.Stop();
                        //Debug.WriteLine(string.Format("InspectRegion {0} -1 -1 {1:F3} [ms]", newDebugContext.LogName, sw.Elapsed.TotalMilliseconds));
                    }
                }
                else
                {
                    //ParallelQuery<InspectLineSet> parallelQuery = inspectRegion.InspectLineSets.AsParallel();
                    //parallelQuery.ForAll(f => InspectLineSet(f, subInspImage, subBufImage, subResultImage, cancellationToken, debugContext));
                    Parallel.For(0, InspectLineSetsLength, i =>
                     {
                         DebugContextG newDebugContext = new DebugContextG(debugContext, debugContext.RegionId, i, -1);

                         InspectLineSet inspectLineSet = inspectRegion.InspectLineSets[i];
                         InspectLineSet(inspectLineSet, cancellationToken, newDebugContext);
                     });
                }

                inspectRegion.GetResult(resultImage, offsetPos);
                inspectRegion.Release();
            }
        }

        private void InspectLineSet(InspectLineSet inspectLineSet, CancellationToken cancellationToken, DebugContextG debugContext)
        {
            //Debug.WriteLine(string.Format("CalculatorV2::InspectLineSet({0}) Start", debugContext.LogName));

            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            //System.Diagnostics.Debug.WriteLine(string.Format("InspectLineSet {0} 1 Start {1}", debugContext.LogName, sw.Elapsed.TotalMilliseconds));

            //InspectLine[] inspectLines = inspectLineSet.Build(subInspImage, subBufImage, subResultImage);
            InspectLine[] inspectLines = inspectLineSet.InspectLines;
            InspectEdge inspectEdge = inspectLineSet.InspectEdge;
            //System.Diagnostics.Debug.WriteLine(string.Format("InspectLineSet {0} 2 Build {1}", debugContext.LogName, sw.Elapsed.TotalMilliseconds));

            int InspectLinesLength = inspectLines.Length;
            for (int i = 0; i < InspectLinesLength; i++)
            {
                DebugContextG newDebugContext = new DebugContextG(debugContext, debugContext.RegionId, debugContext.LineSetId, i);
                InspectLines(inspectLines[i], inspectEdge, inspectLineSet.ImageProcessing, newDebugContext);
            }
            //System.Diagnostics.Debug.WriteLine(string.Format("InspectLineSet {0} 3 Inspect {1}", debugContext.LogName, sw.Elapsed.TotalMilliseconds));

            //inspectLineSet.Release();
            //System.Diagnostics.Debug.WriteLine(string.Format("InspectLineSet {0} 4 Release {1}", debugContext.LogName, sw.Elapsed.TotalMilliseconds));
            //sw.Stop();
            //Debug.WriteLine(string.Format("CalculatorV2::InspectLineSet({0}) End", debugContext.LogName));
        }

        private void InspectLines(InspectLine inspectLine, InspectEdge inspectEdge, ImageProcessing ip, DebugContextG debugContext)
        {
            try
            {
                AlgoImage prev = inspectLine.PrevInspectLine.AlgoImage;
                AlgoImage insp = inspectLine.AlgoImage;
                AlgoImage next = inspectLine.NextInspectLine.AlgoImage;
                prev.Save("0-1. A.bmp", debugContext);
                insp.Save("0-2. B.bmp", debugContext);
                next.Save("0-3. C.bmp", debugContext);

                AlgoImage[] tempBuf = new AlgoImage[4];
                if (inspectLine.BufImage.Length == 1)
                {
                    tempBuf[0] = inspectLine.PrevInspectLine.BufImage[0];
                    tempBuf[1] = inspectLine.BufImage[0];
                    tempBuf[2] = inspectLine.NextInspectLine.BufImage[0];
                    tempBuf[3] = inspectLine.ResultImage;
                }
                else
                {
                    tempBuf[0] = inspectLine.BufImage[0];
                    tempBuf[1] = inspectLine.BufImage[1];
                    tempBuf[2] = inspectLine.BufImage[2];
                    tempBuf[3] = inspectLine.BufImage[3];
                }
                AlgoImage result = inspectLine.ResultImage;

                ip.Subtract(insp, prev, tempBuf[0]); //notABS
                tempBuf[0].Save("1-1. B-A.bmp", debugContext);
                ip.Subtract(insp, next, tempBuf[1]); //notABS
                tempBuf[1].Save("1-2. B-C.bmp", debugContext);
                ip.Min(tempBuf[0], tempBuf[1], tempBuf[2]); //어두운 불량
                tempBuf[2].Save("1-3. Min.bmp", debugContext);

                ip.Subtract(prev, insp, tempBuf[0]); //notABS
                tempBuf[0].Save("2-1. A-B.bmp", debugContext);
                ip.Subtract(next, insp, tempBuf[1]); //notABS
                tempBuf[1].Save("2-2. C-B.bmp", debugContext);
                ip.Min(tempBuf[0], tempBuf[1], tempBuf[3]); //밝은 불량
                tempBuf[3].Save("2-3. Min.bmp", debugContext);

                ip.Max(tempBuf[2], tempBuf[3], result);
                result.Save("3. Max.bmp", debugContext);

                //  엣지영역 제거
                if (inspectEdge != null && inspectEdge.IsBuilded)
                    ip.Subtract(result, inspectEdge.EdgeImage, result);
                result.Save("4. RemoveEdge.bmp", debugContext);

                // dontcare 영역 제거
                Rectangle imageRect = new Rectangle(Point.Empty, result.Size);
                Array.ForEach(inspectLine.IgnoreArea, f =>
                {
                    f.Inflate(40, 40);
                    f.Intersect(imageRect);
                    ip.Clear(result, f, Color.Black);
                });
                result.Save("5. RemoveDontcare.bmp", debugContext);
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - Calculator::InspectLine - {0}", ex.Message));
            }
#endif
            finally { }
        }
    }
}

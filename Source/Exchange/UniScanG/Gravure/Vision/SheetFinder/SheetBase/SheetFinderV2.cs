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
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.SheetFinder.SheetBase
{
    public class SheetFinderV2 : SheetFinderBase
    {
        int boundSize = 2500;
        public int BoundSize
        {
            get { return boundSize; }
            set { this.boundSize = value; }
        }

        public SheetFinderV2() : base()
        {
            this.AlgorithmName = TypeName;
            this.param = new SheetFinderV2Param();
        }

        #region Abstract

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

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SheetFinderV2Param fiducialFinderPJParam = (SheetFinderV2Param)this.param;
            
            AlgoImage algoImage = null;
            bool disposeNeed;
            if (algorithmInspectParam is SheetInspectParam)
            {
                algoImage = ((SheetInspectParam)algorithmInspectParam).AlgoImage;
                disposeNeed = false;
            }
            else
            {
                algoImage = ImageBuilder.Build(this.GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey);
                disposeNeed = true;
            }
            DebugContext debugContext = algorithmInspectParam.DebugContext;
            DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, TypeName));

            SheetFinderResult algorithmResult = FindFiducial(algoImage, fiducialFinderPJParam.FidSize, newDebugContext);

            if (disposeNeed)
                algoImage.Dispose();

            sw.Stop();
            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan();

            //if ((SamsungElectroSettings.Instance().SaveFiducialDebugData & SaveDebugData.Text) > 0)
            //    (MpisInspectorSystemManager.Instance().MainForm as Operation.UI.MainForm).WriteTimeLog("FiducialFinder", baseImage.Height, sw.ElapsedMilliseconds);

            return algorithmResult;
        }

        #endregion

        #region override

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new SheetFinderResult();
        }

        #endregion

        public override int GetBoundSize()
        {
            return boundSize;
        }

        public SheetFinderResult FindFiducial(AlgoImage algoImage, Size fidSize, DebugContext debugContext)
        {
            SheetFinderV2Param sheetFinderV2Param = this.param as SheetFinderV2Param;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            AlgoImage subAlgoImage = null;
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();

                // 탐색방향이 오른쪽->왼쪽: 왼쪽 영역 검사.
                // 탐색방향이 왼쪽->오른쪽: 오른쪽 영역 검사.
                //Rectangle subRect = new Rectangle(Point.Empty, new Size(algoImage.Width / 2, algoImage.Height));
                //if (sheetFinderV2Param.BaseXSearchDir == BaseXSearchDir.Left2Right)
                //    subRect.Offset(algoImage.Width - subRect.Right, 0);

                Rectangle subRect = new Rectangle(Point.Empty, algoImage.Size);
                subRect.Inflate(-algoImage.Size.Width / 4, 0);

                Debug.Assert(subRect.Width > 0 && subRect.Height > 0);

                subAlgoImage = algoImage.GetSubImage(subRect);
                subAlgoImage.Save("SubImage.bmp", debugContext);
                //subAlgoImage.Save(@"D:\temp\SubImage.bmp");
                // Projection                
                float[] projection = imageProcessing.Projection(subAlgoImage, Direction.Vertical, ProjectionType.Mean);

                // Binarize
                //float threshold = projection.Average() * sheetFinderV2Param.ProjectionBinalizeMul;
                float max = projection.Average();// projection.Max();
                float min = projection.Min();
                float threshold = ((max - min) * sheetFinderV2Param.ProjectionBinalizeMul) + min;
                //bool[] projection2 = new bool[projection.Length];
                //Parallel.For(0, projection.Length, i =>
                //{
                //    projection2[i] = (projection[i] >= threshold);
                //});

                // 패턴을 찾음.
                List<Point> whiteRangeList = new List<Point>();
                Point whiteRange = new Point(-1, -1);
                bool onStart = false;   // 시작부터 밝음이면 첫 블록은 무시
                for (int i = 0; i < projection.Length; i++)
                {
                    if (projection[i] > threshold)
                    {
                        if (onStart && whiteRange.X < 0)
                            whiteRange.X = i;
                    }
                    else
                    {
                        onStart = true;
                        if (whiteRange.X >= 0)
                        {
                            whiteRange.Y = i;
                            whiteRangeList.Add(whiteRange);
                            whiteRange = new Point(-1, -1);
                        }
                    }
                }

                //int skipCount = (int)(whiteRangeList.Count * 0.1);
                //int takeCount = whiteRangeList.Count - 2 * skipCount;
                whiteRangeList.Sort((f, g) => (f.Y - f.X).CompareTo(g.Y - g.X));
                //whiteRangeList.ForEach(f => Debug.WriteLine((f.Y - f.X).ToString()));
                //whiteRangeList = whiteRangeList.Skip(skipCount).Take(takeCount).ToList();

                List<Point> foundPointList = new List<Point>();
                if (whiteRangeList.Count > 0)
                {
                    double thresholdDist, thresholdDist2;
                    if (fidSize.Height > 0)
                    {
                        thresholdDist = fidSize.Height * 0.9;
                        thresholdDist2 = fidSize.Height * 1.1;
                    }
                    else
                    {
                        //thresholdDist = whiteRangeList.Average(f => f.Y - f.X) * sheetFinderV2Param.BlankLengthMul;
                        thresholdDist = MathHelper.Median(whiteRangeList.ConvertAll(f => f.Y - f.X)) * sheetFinderV2Param.BlankLengthMul;
                        thresholdDist2 = double.MaxValue;
                    }

                    whiteRangeList.ForEach(f =>
                    {
                        double dist = f.Y - f.X;
                        if (thresholdDist <= dist && dist <= thresholdDist2)
                            foundPointList.Add(f);
                    });

                    // for Save Debug
                    if (debugContext.SaveDebugImage)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("Projection,ProjectionThreshold,Binarize,DistThreshold,DistThreshold2,IsWhiteRange,"));
                        for (int i = 0; i < projection.Length; i++)
                        {
                            sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
                                  projection[i],
                                  threshold,
                                  projection[i] > threshold ? 255 : 0,
                                  thresholdDist, thresholdDist2,
                                  foundPointList.Find(f => (f.X <= i && i <= f.Y)).IsEmpty? 0 : 255));
                        }
                                
                        DebugHelper.SaveText(sb.ToString(), "Projection.txt", debugContext);
                    }

                }
                sw.Stop();
                foundPointList.Sort((f, g) => f.X.CompareTo(g.X));

                SheetFinderResult sheetFinderResult = (SheetFinderResult)CreateAlgorithmResult();
                sheetFinderResult.FoundedFiducialRectList.AddRange(foundPointList.ConvertAll<Rectangle>(f => Rectangle.FromLTRB(0, f.X, algoImage.Width, f.Y)));
                sheetFinderResult.Good = foundPointList.Count > 0;

                sheetFinderResult.SpandTime = sw.Elapsed;//new TimeSpan();

                return sheetFinderResult;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - SheetFinderV2::FindFiducial - {0}", ex.Message));
                return null;
            }
#endif
            finally
            {
                subAlgoImage?.Dispose();
            }

        }
    }
}

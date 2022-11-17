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
using UniScanG.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Watcher
{
    public enum WatcherLockFile { DateTime,PelWidth, PelHeight }
    public class Watcher : Algorithm
    {
        public static string TypeName { get { return "Watcher"; } }
        int iterate = 0;

        public static string LockFileName
        {
            get { return "Watcher.lock"; }
        }

        public static string MonitoringPath
        {
            get { return Path.Combine(PathSettings.Instance().Result, "Monitoring"); }
        }

        public static string MonitoringLockFile
        {
            get { return Path.Combine(MonitoringPath, LockFileName); }
        }

        public Watcher()
        {
            this.AlgorithmName = TypeName;
            this.param = new WatcherParam();
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
            return new AlgorithmResult();
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DebugContext debugContext = new DebugContext(algorithmInspectParam.DebugContext.SaveDebugImage, Path.Combine(algorithmInspectParam.DebugContext.FullPath, "Watcher"));
            
            SheetInspectParam inspectParam = algorithmInspectParam as SheetInspectParam;
            WatcherParam watcherParam = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName).Param as WatcherParam;
            CancellationToken cancellationToken = inspectParam.CancellationToken;

            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance() as AlgorithmSetting;
            int period = algorithmSetting.MonitoringPeriod;
            iterate = (iterate + 1) % period;
            if (iterate == 1)
            {
                AlgoImage algoImage = inspectParam.AlgoImage;
                Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                int diffXPos = (int)inspectParam.FidOffset.Width;
                int diffYPos = (int)inspectParam.FidOffset.Height;
                Point offset = new Point(diffXPos, diffYPos);
                algoImage.Save("algoImage.bmp", debugContext);

                if (Directory.Exists(MonitoringPath) == false)
                    Directory.CreateDirectory(MonitoringPath);

                bool exist = File.Exists(MonitoringLockFile);
                if (exist)
                {
                    LogHelper.Error(LoggerType.Error, "Watcher::Inspect - LockFile is Exist");
                }
                else
                {
                    // Save Images
                    Parallel.For(0, watcherParam.WatchItemList.Count, i =>
                    {
                        WatchItem watchItem = watcherParam.WatchItemList[i];

                        Rectangle adjustRect = watchItem.Rectangle;
                        adjustRect.Offset(offset);
                        if (Rectangle.Intersect(imageRect, adjustRect) == adjustRect)
                        {
                            string file = Path.Combine(MonitoringPath, watchItem.GetFileName());
                            AlgoImage subImage = algoImage.GetSubImage(adjustRect);
                            subImage.Save(file);
                            subImage.Dispose();
                        }
                    });

                    // Create Lock File
                    CreateLockFile();
                }
            }

            AlgorithmResult algorithmResult = CreateAlgorithmResult();
            algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan(, sw.ElapsedMilliseconds);
            algorithmResult.Good = true;
            return algorithmResult;
        }
        

        private void CreateLockFile()
        {
            FileStream fileStream = new FileStream(MonitoringLockFile, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.DateTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.PelWidth.ToString(), pelSize.Width));
            streamWriter.WriteLine(string.Format("{0},{1}", WatcherLockFile.PelHeight.ToString(), pelSize.Height));

            streamWriter.Close();
            fileStream.Close();
        }

        private void CalcDefectType(AlgoImage algoImage, BlobRect defectBlob, int binalValue, float poleAvg, float dielectricAvg, ref Gravure.Data.SheetSubResult sheetSubResult)
        {
            float defectAroundImageValue = GetDefectImageValue(defectBlob, algoImage, new Size(5, 5));
            sheetSubResult.PositionType = (defectAroundImageValue < binalValue ? PositionType.Pole : PositionType.Dielectric);

            if (sheetSubResult.PositionType == PositionType.Pole)
            {
                //float valueDiff = defectBlob.MaxValue - defectBlob.MinValue;
                if (defectBlob.MeanValue <= poleAvg * 1.3 /*|| valueDiff > 40*/)
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
            //patternImage.Save(@"d:\temp\tt.bmp");
            if (defectBlob.Compactness <= 2.5 || defectBlob.MaxFeretDiameter / defectBlob.MinFeretDiameter <= 2.0)
                sheetSubResult.ShapeType = ShapeType.Circular;
            else
                sheetSubResult.ShapeType = ShapeType.Linear;
        }

        private float GetDefectImageValue(BlobRect defectBlob, AlgoImage algoImage, Size inflate)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            float defectValue = defectBlob.MeanValue * defectBlob.Area;

            Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
            defectRect.Inflate(inflate);
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
    }
}

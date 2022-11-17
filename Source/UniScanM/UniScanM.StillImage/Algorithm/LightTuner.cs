using DynMvp.Base;
using DynMvp.Vision;
//using UniScanM.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Settings;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class LightTuner
    {
        public static LightTuner Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new LightTunerV1();
                case 1:
                    return new LightTunerV2();
            }
            return null;
        }

        public abstract void Tune(AlgoImage algoImage, InspectionResult inspectionResult);
    }

    public class LightTuneResult
    {
        int offsetLevel;
        int tryCount;
        public int currentBright = -1;

        public bool IsGood
        {
            get { return offsetLevel == 0; }
        }

        public int CurrentBright
        {
            get { return currentBright; }
            set { currentBright = value; }
        }

        public int OffsetLevel
        {
            get { return offsetLevel; }
            set { offsetLevel = value; }
        }

        public int TryCount
        {
            get { return tryCount; }

            set { tryCount = value; }
        }
    }

    public class LightTunerV1 : LightTuner
    {
        const int mul = 16;
        public override void Tune(AlgoImage algoImage, InspectionResult inspectionResult)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            // Clip display image
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
            Rectangle dispRect = DrawingHelper.FromCenterSize(imageCenter, new Size(1000, 1000));
            dispRect.Intersect(imageRect);
            inspectionResult.InspRectInSheet = dispRect;
            
            float avgValue = imageProcessing.GetGreyAverage(algoImage);
            long[] histogram = imageProcessing.Histogram(algoImage);
            //Array.ForEach(histogram, f => System.Diagnostics.Debug.WriteLine(f));
            LightTuneResult lightTuneResult = new LightTuneResult();
            lightTuneResult.currentBright = (int)avgValue;
            if (avgValue > 200)
            {
                lightTuneResult.OffsetLevel = -2 * mul;
            }
            else if (avgValue > 120)
            {
                lightTuneResult.OffsetLevel = -1 * mul;
            }
            else if (avgValue < 30)
            {
                lightTuneResult.OffsetLevel = +1 * mul;
            }
            else if (avgValue < 50)
            {
                lightTuneResult.OffsetLevel = +2 * mul;
            }
            else
            {
                lightTuneResult.OffsetLevel = 0;
            }
            //      brightnessResultItem.OffsetValue = 0;

            if (lightTuneResult.OffsetLevel != 0)
                inspectionResult.SetDefect();
            inspectionResult.LightTuneResult = lightTuneResult;
        }
    }

    public class LightTunerV2 : LightTuner
    {
        int offsetValue = 64; // 조명값 부호 변경 횟수

        public override void Tune(AlgoImage algoImage, InspectionResult inspectionResult)
        {
            LightTuneResult lightTuneResult = new LightTuneResult();
            inspectionResult.LightTuneResult = lightTuneResult;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            // Clip display image
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
            Rectangle dispRect = DrawingHelper.FromCenterSize(imageCenter, new Size(1000, 1000));
            dispRect.Intersect(imageRect);
            inspectionResult.InspRectInSheet = dispRect;
            AlgoImage displayImage = algoImage.GetSubImage(dispRect);
            inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
            displayImage.Dispose();

            int avgValue = (int)imageProcessing.GetGreyAverage(algoImage);
            long[] histogram = imageProcessing.Histogram(algoImage);
            
            List<long> lowerHistogram = histogram.Take(avgValue).ToList(); // 평균 미만 히스토그램
            List<long> upperHistogram = histogram.Skip(avgValue).ToList(); // 평균 이상 히스토그램

            if (lowerHistogram.Count == 0 || upperHistogram.Count == 0)
            {
                if (lowerHistogram.Count == 0)
                    lightTuneResult.OffsetLevel = 15;
                else
                    lightTuneResult.OffsetLevel = -15;

                lightTuneResult.currentBright = -1;
                inspectionResult.SetDefect();
                return;
            }

            // 두 히스토그램의 최대값
            long lowerMax = lowerHistogram.Max();
            int lowerMaxValue = lowerHistogram.IndexOf(lowerMax);

            long upperMax = upperHistogram.Max();
            int upperMaxValue = upperHistogram.IndexOf(upperMax) + (int)avgValue;
            lightTuneResult.currentBright = upperMaxValue;

            // 최대값의 차이가 10 미만이면 영상에 패턴이 없음.
            //if (upperMaxValue - lowerMaxValue < 10)
            //{
            //    inspectionResult.SetDefect();
            //    if (upperMaxValue > 200)
            //        offsetValue = GetOffsetValue(false);
            //    else if (upperMaxValue < 50)
            //        offsetValue = GetOffsetValue(true);
            //}
            //else
            {
                inspectionResult.SetGood();

                int offsetValue = 0;
                int targetValue = StillImageSettings.Instance().TargetIntensity;
                int targetValueVal = StillImageSettings.Instance().TargetIntensityVal;
                bool ok = true;

                if (upperMaxValue < targetValue - targetValueVal)
                {
                    offsetValue = GetOffsetValue(true);
                    inspectionResult.SetDefect();
                }
                else if (upperMaxValue > targetValue + targetValueVal)
                {
                    offsetValue = GetOffsetValue(false);
                    inspectionResult.SetDefect();
                }

                lightTuneResult.OffsetLevel = offsetValue;
            }
        }

        private int GetOffsetValue(bool increase)
        {
            // 조명값 변화의 부호가 반대방향이면 값을 절반으로 줄인다.
            float offset = this.offsetValue;
            if (increase)
            {
                offset /= (offset > 0) ? 1 : -2;
            }
            else
            {
                offset /= (offset < 0) ? 1 : -2;
            }

            
            this.offsetValue = (int)offset;
            LogHelper.Debug(LoggerType.Debug, string.Format("LightTuner::GetOffsetValue - {0}, {1}", increase, offset));
            return this.offsetValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.Settings;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.Detector.Dielectric
{
    public class DielectricInspector
    {
        DielectricInspectorParam param;
       
        public DielectricInspector(DielectricInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage dielectricInspect, AlgoImage dielectricMask, RegionInfoS regionInfo,
            ref List<Screen.Data.SheetSubResult> dielectricList, ref List<Screen.Data.SheetSubResult> pinHoleList, ref bool isReached)
        {
            if (param.UseLower == false && param.UseUpper == false)
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interest);
            
            if (param.UseLower == true && param.UseUpper == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Max(0, regionInfo.DielectricValue - param.LowerThreshold), Math.Min(255, regionInfo.DielectricValue + param.UpperThreshold), true);
            else if (param.UseLower == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Max(0, regionInfo.DielectricValue - param.LowerThreshold), true);
            else if (param.UseUpper == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Min(255, regionInfo.DielectricValue + param.UpperThreshold));
            //dielectricInspect.Save("dielectricInspect.bmp", new DebugContext(true, "d:\\"));
            //dielectricMask.Save("dielectricMask.bmp", new DebugContext(true, "d:\\"));

            imageProcessing.And(dielectricInspect, dielectricMask, dielectricInspect);
            
            BlobParam blobParam = new BlobParam();
            //blobParam.MaxCount = AlgorithmSetting.Instance().MaxDefectNum;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectMinValue = true;
            blobParam.SelectMaxValue = true;
            blobParam.SelectSigmaValue = true;
            blobParam.SelectCompactness = true;
            blobParam.SelectRotateRect = true;

            //전극이랑 달라요.. 밑에 3개
            blobParam.EraseBorderBlobs = true;
            
            BlobRectList blobRectList = imageProcessing.Blob(dielectricInspect, blobParam, interest);
            List<BlobRect> defectList = blobRectList.GetList();
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);

            if (blobRectList.IsReached == true)
            {
                isReached = true;
                return;
            }

            AlgorithmCommon.Instance().RemoveIntersectBlobs(ref defectList);
            //AlgorithmCommon.Instance().MergeBlobs(25, ref defectList);

            //ImageHelper.SaveImage(interest.ToImageD().ToBitmap(), "..\\interest.bmp");
            //ImageHelper.SaveImage(dielectricMask.ToImageD().ToBitmap(), "..\\dielectricMask.bmp");
            //ImageHelper.SaveImage(dielectricInspect.ToImageD().ToBitmap(), "..\\dielectricInspect.bmp");

            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);

            List<Screen.Data.SheetSubResult> tempDielectricList = new List<Screen.Data.SheetSubResult>();
            List<Screen.Data.SheetSubResult> tempPinHoleList = new List<Screen.Data.SheetSubResult>();
            
            //foreach (BlobRect blobRect in defectList)
            Parallel.ForEach(defectList, blobRect =>
            {
                Screen.Data.SheetSubResult subResult = new Screen.Data.SheetSubResult();

                subResult.Length = Math.Max(blobRect.BoundingRect.Width, blobRect.BoundingRect.Height);
                subResult.RealLength = Math.Max(blobRect.BoundingRect.Width * AlgorithmSetting.Instance().XPixelCal,
                    blobRect.BoundingRect.Height * AlgorithmSetting.Instance().YPixelCal);

                float lowerValue = 0;
                float upperValue = 0;
                if (regionInfo.DielectricValue > blobRect.MinValue)
                    lowerValue = regionInfo.DielectricValue - blobRect.MinValue;

                if (regionInfo.DielectricValue < blobRect.MaxValue)
                    upperValue = blobRect.MaxValue - regionInfo.DielectricValue;

                subResult.SetThreshold(param.LowerThreshold, param.UpperThreshold);
                subResult.LowerDiffValue = lowerValue;
                subResult.UpperDiffValue = upperValue;
                subResult.Compactness = blobRect.Compactness;

                if (blobRect.Compactness <= AlgorithmSetting.Instance().DielectricCompactness && lowerValue > 0)
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().PinHoleMinSize)
                        return;

                    subResult.DefectType = DefectType.PinHole;

                    lock (tempPinHoleList)
                        tempPinHoleList.Add(subResult);
                }
                else
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().DielectricMinSize)
                        return;

                    subResult.DefectType = DefectType.Dielectric;

                    lock (tempDielectricList)
                        tempDielectricList.Add(subResult);
                }

                AlgorithmCommon.Instance().RefineSubResult(interest, dielectricInspect, srcRect, blobRect, regionInfo.Region, ref subResult);
            });

            pinHoleList.AddRange(tempPinHoleList);
            dielectricList.AddRange(tempDielectricList);
        }
    }
}

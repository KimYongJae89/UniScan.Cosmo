using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.Settings;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.Detector.Pole
{
    public class PoleInspector
    {
        PoleInspectorParam param;

        public PoleInspector(PoleInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage poleInspect, AlgoImage poleMask, RegionInfoS regionInfo, List<BlobRect> notNecessaryList,
            ref List<Screen.Data.SheetSubResult> sheetAttackList, ref List<Screen.Data.SheetSubResult> poleCircleList, ref List<Screen.Data.SheetSubResult> poleLineList, ref bool isReached)
        {
            if (param.UseLower == false && param.UseUpper == false)
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interest);
            
            if (param.UseLower == true && param.UseUpper == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Max(0, regionInfo.PoleValue - param.LowerThreshold), Math.Min(255, regionInfo.PoleValue + param.UpperThreshold), true);
            else if (param.UseLower == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Max(0, regionInfo.PoleValue - param.LowerThreshold), true);
            else if (param.UseUpper == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Min(255, regionInfo.PoleValue + param.UpperThreshold));

            imageProcessing.And(poleInspect, poleMask, poleInspect);

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = AlgorithmSetting.Instance().MaxDefectNum;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectMaxValue = true;
            blobParam.SelectMinValue = true;
            //blobParam.SelectSigmaValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectCompactness = true;
            //blobParam.SelectRotateRect = true;

            imageProcessing.Dilate(poleInspect, 1);
            BlobRectList blobRectList = imageProcessing.Blob(poleInspect, blobParam, interest);
            List<BlobRect> defectList = blobRectList.GetList();
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);

            if (blobRectList.IsReached == true)
            {
                isReached = true;
                return;
            }
            
            defectList.RemoveAll(defect => AlgorithmCommon.Instance().IsNecessaryDefect(defect, notNecessaryList));

            AlgorithmCommon.Instance().RemoveIntersectBlobs(ref defectList);

            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);
            
            List<Screen.Data.SheetSubResult> tempSheetAttackList = new List<Screen.Data.SheetSubResult>();
            List<Screen.Data.SheetSubResult> tempPoleCircleList = new List<Screen.Data.SheetSubResult>();
            List<Screen.Data.SheetSubResult> tempPoleLineList = new List<Screen.Data.SheetSubResult>();

            //foreach (BlobRect blobRect in defectList)
            Parallel.ForEach(defectList, blobRect =>
            {
                Screen.Data.SheetSubResult subResult = new Screen.Data.SheetSubResult();

                subResult.Length = Math.Max(blobRect.BoundingRect.Width, blobRect.BoundingRect.Height);
                subResult.RealLength = Math.Min(blobRect.BoundingRect.Width * AlgorithmSetting.Instance().XPixelCal,
                    blobRect.BoundingRect.Height * AlgorithmSetting.Instance().YPixelCal);

                float lowerValue = 0;
                float upperValue = 0;

                if (regionInfo.PoleValue > blobRect.MinValue)
                    lowerValue = regionInfo.PoleValue - blobRect.MinValue;

                if (regionInfo.PoleValue < blobRect.MaxValue)
                    upperValue = blobRect.MaxValue - regionInfo.PoleValue;

                subResult.SetThreshold(param.LowerThreshold, param.UpperThreshold);
                subResult.LowerDiffValue = lowerValue;
                subResult.UpperDiffValue = upperValue;
                subResult.Compactness = blobRect.Compactness;

                if (blobRect.Compactness <= AlgorithmSetting.Instance().PoleCompactness)
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().PoleMinSize)
                        return;

                    subResult.DefectType = DefectType.PoleCircle;
                    
                    lock (tempPoleCircleList)
                        tempPoleCircleList.Add(subResult);
                }
                else
                {
                    if (lowerValue <= 0 || upperValue <= 0)
                    {
                        if (subResult.RealLength < AlgorithmSetting.Instance().PoleMinSize)
                            return;

                        subResult.DefectType = DefectType.PoleLine;
                        lock (tempPoleLineList)
                            tempPoleLineList.Add(subResult);
                    }
                    else
                    {
                        if (subResult.RealLength < AlgorithmSetting.Instance().SheetAttackMinSize)
                            return;

                        subResult.DefectType = DefectType.SheetAttack;

                        lock (tempSheetAttackList)
                            tempSheetAttackList.Add(subResult);
                    }
                }
                
                AlgorithmCommon.Instance().RefineSubResult(interest, poleInspect, srcRect, blobRect, regionInfo.Region, ref subResult);
            });

            sheetAttackList.AddRange(tempSheetAttackList);
            poleCircleList.AddRange(tempPoleCircleList);
            poleLineList.AddRange(tempPoleLineList);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Data;
using UniScanG.Data.Vision;
using UniScanG.Screen.Data;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.Detector.Shape
{
    public class ShapeInspector
    {
        ShapeInspectorParam param;
        
        public ShapeInspector(ShapeInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage maskR, RegionInfo regionInfo, ref List<ShapeResult> shapeList, ref List<BlobRect> notNecessaryList, ref List<BlobRect> needInspectList)
        {
            if (param.UseInspect == false  )
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(maskR);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            blobParam.EraseBorderBlobs = true;

            blobParam.AreaMin = param.MinPatternArea;

            BlobRectList blobRectList = imageProcessing.Blob(maskR, blobParam);
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);
            
            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);
            List<PatternInfo> tempNeedInspectList = new List<PatternInfo>();
            List<PatternInfo> tempNotNecessaryList = new List<PatternInfo>();
            List<ShapeResult> tempShapeList = new List<ShapeResult>();

            List<PatternInfo> shapeBlobRectList = blobRectList.GetList().ConvertAll(f=>new PatternInfo(f));
            //foreach (BlobRect blobRect in shapeBlobRectList)
            Parallel.ForEach(shapeBlobRectList, blobRect =>
            {
                ShapeDiffValue shapeDiffValue = new ShapeDiffValue(true);

                foreach (SheetPattern pattern in param.PatternList)
                {
                    ShapeDiffValue subShapeDiffValue = new ShapeDiffValue(false);
                    subShapeDiffValue.AreaDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Area, blobRect);
                    subShapeDiffValue.WidthDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Width, blobRect);
                    subShapeDiffValue.HeightDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Height, blobRect);
                    subShapeDiffValue.CenterXDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.CenterX, blobRect);
                    subShapeDiffValue.CenterYDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.CenterY, blobRect);

                    if (shapeDiffValue.SumDiff > subShapeDiffValue.SumDiff)
                    {
                        shapeDiffValue = subShapeDiffValue;
                        shapeDiffValue.SimilarPattern = pattern;
                    }
                }

                if (shapeDiffValue.SimilarPattern.NeedInspect == false)
                {
                    lock (tempNotNecessaryList)
                        tempNotNecessaryList.Add(blobRect);

                    return;
                }

                if (param.UseHeightDiffTolerence == true)
                    shapeDiffValue.SetTolerance(param.DiffTolerence, param.HeightDiffTolerence);
                else
                    shapeDiffValue.SetTolerance(param.DiffTolerence);
                
                if (shapeDiffValue.IsDefect())
                {
                    ShapeResult subResult = new ShapeResult();
                    subResult.ShapeDiffValue = shapeDiffValue;

                    Screen.Data.SheetSubResult sheetSubResult = (Screen.Data.SheetSubResult)subResult;
                    sheetSubResult.Length = Math.Max(blobRect.BoundingRect.Width, blobRect.BoundingRect.Height);
                    sheetSubResult.RealLength = Math.Max(blobRect.BoundingRect.Width * AlgorithmSetting.Instance().XPixelCal,
                        blobRect.BoundingRect.Height * AlgorithmSetting.Instance().YPixelCal);

                    AlgorithmCommon.Instance().RefineSubResult(interest, null, srcRect, blobRect, regionInfo.Region, ref sheetSubResult);

                    lock (tempShapeList)
                        tempShapeList.Add(subResult);
                }
                else
                {
                    if (shapeDiffValue.SimilarPattern.NeedInspect == true)
                        lock(tempNeedInspectList)
                            tempNeedInspectList.Add(blobRect);
                }
            });

            tempShapeList = tempShapeList.OrderBy(x => x.Region.X + x.Region.Y).ToList();

            needInspectList.AddRange(tempNeedInspectList);
            notNecessaryList.AddRange(tempNotNecessaryList);
            shapeList.AddRange(tempShapeList);
        }
    }
}

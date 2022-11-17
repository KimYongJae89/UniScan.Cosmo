using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScan.Common.Settings;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector.Dielectric;
using UniScanG.Screen.Vision.Detector.Pole;
using UniScanG.Screen.Vision.Detector.Shape;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.Detector
{
    public class SheetInspector : DynMvp.Vision.Algorithm
    {
        PoleInspector poleInspector;
        DielectricInspector dielectricInspector;
        ShapeInspector shapeInspector;

        public static string TypeName
        {
            get { return "SheetS_Inspector"; }
        }

        public SheetInspector()
        {
            param = new SheetInspectorParam();

            SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)this.param;

            poleInspector = new PoleInspector(sheetInspectorParam.PoleParam);
            dielectricInspector = new DielectricInspector(sheetInspectorParam.DielectricParam);
            shapeInspector = new ShapeInspector(sheetInspectorParam.ShapeParam);
        }
        
        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            
        }

        public override void CopyFrom(DynMvp.Vision.Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            SheetInspector srcAlgorithm = (SheetInspector)algorithm;
            this.param.CopyFrom(srcAlgorithm.param);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetInspector clone = new SheetInspector();
            clone.CopyFrom(this);

            return clone;
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return TypeName;
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new ScreenResult();
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new System.NotImplementedException();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            ScreenResult screenResult = (ScreenResult)CreateAlgorithmResult();
            
            SheetInspectorParam param = (SheetInspectorParam)this.Param;

            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)algorithmInspectParam;
            
            ProcessBufferSetS bufferSet = (ProcessBufferSetS)(sheetAlgorithmInspectParam).ProcessBufferSet;

            AlgoImage sourceImage = ImageBuilder.Build(GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey, ImageBandType.Luminance);
            Rectangle interestRect = new Rectangle(0, 0, sourceImage.Width, param.EmptyRegionPos);

            AlgoImage interest = sourceImage.GetSubImage(interestRect);
            int width = interest.Width;
            int height = interest.Height;

            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);
            imageProcessing.Resize(interest, bufferSet.InterestP, SystemTypeSettings.Instance().ResizeRatio, SystemTypeSettings.Instance().ResizeRatio);
            screenResult.PrevImage = bufferSet.InterestP.ToImageD().ToBitmap();
            
            int regionIndex = 1;
            foreach (RegionInfoS regionInfo in param.RegionInfoList)
            {
                Rectangle region = regionInfo.Region;
                int offsetWidth = (int)Math.Round(sheetAlgorithmInspectParam.FidOffset.Width);
                int offsetHeight = (int)Math.Round(sheetAlgorithmInspectParam.FidOffset.Height);
                region.Offset(offsetWidth, offsetHeight);
                
                ///////////////////////////////
                // SubImage
                ///////////////////////////////
                AlgoImage interestRegion = interest.GetSubImage(regionInfo.Region);

                AlgoImage interestBinRegion = bufferSet.InterestBin.GetSubImage(regionInfo.Region);
                AlgoImage maskRegion = bufferSet.Mask.GetSubImage(regionInfo.Region);

                AlgoImage poleInspectRegion = bufferSet.PoleInspect.GetSubImage(regionInfo.Region);
                AlgoImage poleMaskRegion = bufferSet.PoleMask.GetSubImage(regionInfo.Region);

                AlgoImage dielectricInspectRegion = bufferSet.DielectricInspect.GetSubImage(regionInfo.Region);
                AlgoImage dielectricMaskRegion = bufferSet.DielectricMask.GetSubImage(regionInfo.Region);
                ///////////////////////////////
                
                imageProcessing.Binarize(interestRegion, interestBinRegion, regionInfo.MeanValue, true);

                AlgorithmCommon.Instance().CreateMaskImage(interestBinRegion, maskRegion, param.ShapeParam.MinPatternArea, true);

                //Shape
                List<BlobRect> notNecessaryList = new List<BlobRect>();
                List<ShapeResult> shapeList = new List<ShapeResult>();
                List<BlobRect> needInspectBlobList = new List<BlobRect>();
                shapeInspector.Inspect(interestRegion, maskRegion, regionInfo, ref shapeList, ref notNecessaryList, ref needInspectBlobList);

                int needInspectCount = 0;
                param.ShapeParam.PatternList.ForEach(p => needInspectCount += p.PatternGroup.NumPattern);
                if (needInspectBlobList.Count < 100)
                {
                    screenResult.Good = false;

                    interestRegion.Dispose();
                    poleInspectRegion.Dispose();
                    poleMaskRegion.Dispose();
                    dielectricInspectRegion.Dispose();
                    dielectricMaskRegion.Dispose();

                    interest.Dispose();
                    sourceImage.Dispose();

                    stopwatch.Stop();
                    screenResult.SpandTime = stopwatch.Elapsed;

                    return screenResult;
                }
                    
                //Prepare
                imageProcessing.Not(maskRegion, dielectricMaskRegion);

                imageProcessing.Erode(maskRegion, poleMaskRegion, AlgorithmSetting.Instance().RemovalNum);
                imageProcessing.Erode(dielectricMaskRegion, dielectricMaskRegion, AlgorithmSetting.Instance().RemovalNum);

                List<Task> taskList = new List<Task>();

                //Pole Task
                List<Screen.Data.SheetSubResult> sheetAttackList = new List<Screen.Data.SheetSubResult>();
                List<Screen.Data.SheetSubResult> poleCircleList = new List<Screen.Data.SheetSubResult>();
                List<Screen.Data.SheetSubResult> poleLineList = new List<Screen.Data.SheetSubResult>();
                taskList.Add(Task.Run(() => poleInspector.Inspect(interestRegion, poleInspectRegion, poleMaskRegion, regionInfo, notNecessaryList, ref sheetAttackList, ref poleCircleList, ref poleLineList, ref screenResult.poleReached)));

                //Dielectric Task
                List<Screen.Data.SheetSubResult> dielectricList = new List<Screen.Data.SheetSubResult>();
                List<Screen.Data.SheetSubResult> pinHoleList = new List<Screen.Data.SheetSubResult>();
                taskList.Add(Task.Run(() => dielectricInspector.Inspect(interestRegion, dielectricInspectRegion, dielectricMaskRegion, regionInfo, ref dielectricList, ref pinHoleList, ref screenResult.dielectricReached)));

                //검사중에 해제해도 됨
                interestBinRegion.Dispose();
                maskRegion.Dispose();
                
                Task.WaitAll(taskList.ToArray());
                screenResult.AddSheetSubResult(sheetAttackList, poleCircleList, poleLineList, dielectricList, pinHoleList, shapeList);
                
                //검사 끝나고 해제
                interestRegion.Dispose();
                poleInspectRegion.Dispose();
                poleMaskRegion.Dispose();
                dielectricInspectRegion.Dispose();
                dielectricMaskRegion.Dispose();
            }

            interest.Dispose();
            sourceImage.Dispose();

            stopwatch.Stop();
            screenResult.SpandTime = stopwatch.Elapsed;

            return screenResult;
        }
    }
}

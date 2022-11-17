using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;
using UniScanWPF.Screen.PinHoleColor.PinHole.Settings;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    internal class PinHoleDetector : UniScanWPF.Screen.PinHoleColor.Inspect.Detector
    {
        public override Tuple<int, ConcurrentStack<AlgoImage>> GetBufferStack(ImageDevice targetDevice)
        {
            ConcurrentStack<AlgoImage> stack = new ConcurrentStack<AlgoImage>();

            for (int i = 0; i < PinHoleSettings.Instance().BufferNum; i++)
            {
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
            }

            return new Tuple<int, ConcurrentStack<AlgoImage>>(3, stack);
        }

        public override DetectorResult Detect(AlgoImage targetImage, AlgoImage[] buffers, DetectorParam detectorParam)
        {
            PinHoleDetectorResult pinHoleDetectorResult = new PinHoleDetectorResult();

            PinHoleDetectorParam param = detectorParam as PinHoleDetectorParam;
            
            int width = targetImage.Width;
            int height = targetImage.Height;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(targetImage);
            
            Rectangle interestRect = new Rectangle();
            Rectangle sheetRegionRect = new Rectangle(new Point(0, targetImage.Height / 2 - 100), new Size(targetImage.Width, 200));
            AlgoImage projectionImage = targetImage.GetSubImage(Rectangle.Round(sheetRegionRect));
            double[] edgePosition = EdgePositionFinder.SheetEdgePosition(projectionImage, new double[] { param.EdgeThreshold }, param.SearchDireciton);
            projectionImage.Dispose();

            switch (param.SearchDireciton)
            {
                case SearchDireciton.LeftToRight:
                    interestRect = new Rectangle((int)edgePosition[0] + param.SkipLength, 0, width - (int)edgePosition[0] - param.SkipLength, height);
                    break;
                case SearchDireciton.RightToLeft:
                    interestRect = new Rectangle(0, 0, width - (int)edgePosition[0] - param.SkipLength, height);
                    break;
            }

            if (edgePosition[0] == 0 || interestRect.Width <= 0)
            {
                pinHoleDetectorResult.Judgment = DynMvp.InspData.Judgment.Skip;
                return pinHoleDetectorResult;
            }

            AlgoImage sheetRegionImage = targetImage.GetSubImage(interestRect);
            float avgValue = imageProcessing.GetGreyAverage(sheetRegionImage);
            sheetRegionImage.Dispose();
            
            interestRect.Height = height;
            pinHoleDetectorResult.InterestRegion = interestRect;

            AlgoImage interestImage = targetImage.GetSubImage(Rectangle.Round(interestRect));
            AlgoImage binImage = buffers[0].GetSubImage(Rectangle.Round(interestRect));

            //int length = 50;

            //AlgoImage buffer1 = buffers[1].GetSubImage(Rectangle.Round(interestRect), false);
            //AlgoImage buffer2 = buffers[2].GetSubImage(Rectangle.Round(interestRect), false);
            //imageProcessing.CustomEdge(interestImage, sobelBinImage, buffer1, buffer2, length);

            //imageProcessing.Binarize(sobelBinImage, sobelBinImage, param.DefectThreshold);
            //imageProcessing.Translate(sobelBinImage, buffer1, new Point(-length * 2, -length * 2));

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = 10000;
            blobParam.SelectMeanValue = true;
            blobParam.SelectFeretDiameter = true;
            //blobParam.AvgMin = avgValue;

            //BlobRectList pinHoleRectList = imageProcessing.Blob(sobelBinImage, blobParam, interestImage);
            //pinHoleRectList.Dispose();

            //blobParam.AvgMin = 0;
            //blobParam.AvgMax = avgValue;
            //BlobRectList dustRectList = imageProcessing.Blob(buffer1, blobParam, interestImage);
            //dustRectList.Dispose();

            //sobelBinImage.Dispose();
            imageProcessing.Binarize(interestImage, binImage, Math.Max(0, (int)(avgValue - param.LowerThreshold)), Math.Min(255, (int)(avgValue + param.UpperThreshold)), true);
            BlobRectList defectList = imageProcessing.Blob(binImage, blobParam, interestImage);
            binImage.Dispose();
            interestImage.Dispose();
            
            AddDefectBlob(targetImage, defectList.GetList(), pinHoleDetectorResult, avgValue, interestRect.X);

            if (pinHoleDetectorResult.DefectList.Count == 0)
                pinHoleDetectorResult.Judgment = Judgment.Accept;
            
            return pinHoleDetectorResult;
        }

        void AddDefectBlob(AlgoImage algoImage, List<BlobRect> blobRectList, PinHoleDetectorResult pinHoleDetectorResult, float avgValue, int offset)
        {
            Rectangle srcRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            PinHoleSettings settings = PinHoleSettings.Instance();

            List<BlobRect> orderBlobRectList = blobRectList.OrderByDescending(blobRect => blobRect.Area).TakeWhile(blobRect => blobRect.MaxFeretDiameter * settings.PixelResolution >= settings.MinSize).ToList();

            int index = 0;
            foreach (BlobRect blobRect in orderBlobRectList)
            {
                if (pinHoleDetectorResult.DefectList.Count >= settings.MaxDefectNum)
                    break;

                Rectangle rect = Rectangle.Truncate(blobRect.BoundingRect);
                rect.Offset(offset, 0);
                
                Rectangle clipRect = Rectangle.Round(rect);
                clipRect.Inflate(20, 20);
                clipRect.Intersect(srcRect);

                if (clipRect.Width == 0 || clipRect.Height == 0)
                    continue;

                PinHoleDefectType defectType = blobRect.MeanValue >= avgValue ? PinHoleDefectType.PinHole : PinHoleDefectType.Dust;
                
                AlgoImage subImage = algoImage.GetSubImage(clipRect);
                BitmapSource defectImage = subImage.ToBitmapSource();
                subImage.Dispose();
                //바뀐코드

                PinHoleDefect defect = new PinHoleDefect(index++, defectType == PinHoleDefectType.Dust ? avgValue - blobRect.MeanValue : blobRect.MeanValue - avgValue, defectImage, clipRect, defectType);
                pinHoleDetectorResult.DefectList.Add(defect);
            }
        }
    }
}

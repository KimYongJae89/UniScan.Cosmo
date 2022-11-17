using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;
using UniScanM.Pinhole.Data;
using UniScanM.Pinhole.Settings;

namespace UniScanM.Pinhole.Algorithm
{
    public class PinholeChecker
    {
        EdgePositionFinder edgePositionFinder;

        public PinholeChecker()
        {
            edgePositionFinder = new EdgePositionFinder();
        }
        
        private bool GetAverageValue(int deviceIndex, AlgoImage algoImage, AlgoImage maskImage, ref int average)
        {
            int width = algoImage.Width;
            int height = algoImage.Height;

            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;

            int searchHeight = 10;

            int searchStartY = (algoImage.Height / 2) - (searchHeight / 2);
            Rectangle srcRect = new Rectangle(0, 0, width, height);
            Rectangle searchRect = new Rectangle(0, searchStartY, width, searchHeight);
            searchRect.Intersect(srcRect);
            if (searchRect.Width == 0 || searchRect.Height == 0)
                return false;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            AlgoImage srcSubImage = algoImage.GetSubImage(searchRect);
            AlgoImage maskSubImage = maskImage.GetSubImage(searchRect);

            average = (int)Math.Round(imageProcessing.GetGreyAverage(srcSubImage, maskSubImage));

            srcSubImage.Dispose();
            maskSubImage.Dispose();

            return true;
        }

        //public bool AutoSet(int deviceIndex, ImageD imageD)
        //{
        //    AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);

        //    int width = algoImage.Width;
        //    int height = algoImage.Height;

        //    Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    AlgoImage maskImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
        //    PinholeParamValue paramValue;
        //    if (deviceIndex == 0)
        //        paramValue = model.InspectParam.PinholeParamValue1;
        //    else
        //        paramValue = model.InspectParam.PinholeParamValue2;
            
        //    paramValue.BinarizeValue = (int)imageProcessing.Binarize(algoImage, maskImage, true);

        //    maskImage.Dispose();
        //    algoImage.Dispose();
        //    return true;
        //}

        //private void FindSheetRegion(Data.InspectionResult inspectionResult, AlgoImage algoImage, ref Rectangle interestRect, ref bool isSkip)
        //{
        //    Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
        //    PinholeParamValue paramValue;
        //    if (inspectionResult.DeviceIndex == 0)
        //        paramValue = model.InspectParam.PinholeParamValue1;
        //    else
        //        paramValue = model.InspectParam.PinholeParamValue2;

        //    int bin = paramValue.BinarizeValue;
        //    int passSize = PinholeSettings.Instance().SkipLength;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    AlgoImage maskImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);

        //    imageProcessing.Binarize(algoImage, maskImage, bin);

        //    imageProcessing.Dilate(maskImage, 10);
        //    imageProcessing.Erode(maskImage, 10);


        //    BlobParam interestBlobParam = new BlobParam();
        //    BlobRectList interestBlobRectList = imageProcessing.Blob(maskImage, interestBlobParam);

        //    interestBlobRectList.Dispose();
        //    maskImage.Dispose();

        //    BlobRect maxBlob = interestBlobRectList.GetMaxAreaBlob();
        //    if (maxBlob == null)
        //    {
        //        isSkip = true;
        //        return;
        //    }

        //    int width = algoImage.Width;
        //    int height = algoImage.Height;

        //    if (maxBlob.BoundingRect.X == 0)
        //        interestRect = new Rectangle((int)maxBlob.BoundingRect.Width, 0, width - (int)maxBlob.BoundingRect.Width, height);
        //    else
        //        interestRect = new Rectangle(0, 0, width - (int)maxBlob.BoundingRect.Width, height);

        //    // 왼쪽
        //    if (maxBlob.BoundingRect.X == 0)
        //        interestRect.X += passSize;

        //    interestRect.Width -= passSize;

        //    //    skipRect.X = interestRect.Right - passSize;

        //    //interestRect = new Rectangle(0, 0, width, height);

        //    //interestRect.Width -= passSize;

        //    if (interestRect.Width <= 0)
        //    {
        //        isSkip = true;
        //        return;
        //    }
        //    inspectionResult.InterestRegion = interestRect;

        //    return;
        //}
        
        public Data.InspectionResult Inspect(ImageD image, Data.InspectionResult inspectResult)
        {
            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
            PinholeParamValue paramValue;
            if (inspectResult.DeviceIndex == 0)
                paramValue = model.InspectParam.PinholeParamValue1;
            else
                paramValue = model.InspectParam.PinholeParamValue2;

            int edgeThreshold = paramValue.DefectThreshold;

            int width = image.Width;
            int height = image.Height;

            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
            AlgoImage algoImage2 = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, algoImage.ImageType, new Size((int)((float)algoImage.Width * 0.2f), (int)((float)algoImage.Height * 0.2f)));

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            imageProcessing.Resize(algoImage, algoImage2, 0.2, 0.2);
            inspectResult.DisplayBitmap = algoImage2.ToImageD().ToBitmap();
            algoImage2.Dispose();
              
            Rectangle interestRect = new Rectangle();
            Rectangle sheetRegionRect = new Rectangle(new Point(0, algoImage.Height / 2 - 100), new Size(algoImage.Width, 200));
            AlgoImage sheetRegionImage = algoImage.GetSubImage(Rectangle.Round(sheetRegionRect));
            double[] edgePosition = edgePositionFinder.SheetEdgePosition(sheetRegionImage, new double[] { paramValue.EdgeThreshold }, inspectResult.DeviceIndex == 0 ? SearchDireciton.LeftToRight : SearchDireciton.RightToLeft);
            sheetRegionImage.Dispose();

            //FindSheetRegion(inspectResult, sheetRegionImage, ref interestRect, ref isSkip);

            int passSize = PinholeSettings.Instance().SkipLength;
            if (inspectResult.DeviceIndex == 0) 
                interestRect = new Rectangle((int)edgePosition[0] + passSize, 0, width - (int)edgePosition[0] - passSize, height);
            else
                interestRect = new Rectangle(0, 0, width - (int)edgePosition[0] - passSize, height);
            
            if (edgePosition[0] == 0 || interestRect.Width <= 0)
            { 
                algoImage.Dispose();
                inspectResult.Judgment = Judgment.Skip;
                return inspectResult;
            }
            
            interestRect.Height = height;

            inspectResult.InterestRegion = interestRect;

            AlgoImage interestImage = algoImage.GetSubImage(Rectangle.Round(inspectResult.InterestRegion));

            int offsetX = (int)inspectResult.InterestRegion.X;
            AlgoImage interestAvgImage = algoImage.GetSubImage(new Rectangle(interestRect.X, algoImage.Height / 2 - 100, interestRect.Width, 200)); 
            float avgValue = imageProcessing.GetGreyAverage(interestAvgImage);
            interestAvgImage.Dispose();

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = 10000;
            blobParam.SelectMeanValue = true;
            //blobParam.BoundingRectMinX = (PinholeSettings.Instance().SmallSize.Width * 1000.0) / PinholeSettings.Instance().PixelResolution;
            //blobParam.BoundingRectMinY = (PinholeSettings.Instance().SmallSize.Height * 1000.0) / PinholeSettings.Instance().PixelResolution;
            
            AlgoImage sobelBinImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, interestImage.Width, interestImage.Height);
            imageProcessing.Sobel(interestImage, sobelBinImage);
            //sobelBinImage.Save(@"d:\temp\sobelBinImage.bmp");
            imageProcessing.Binarize(sobelBinImage, sobelBinImage, edgeThreshold);

            //AlgoImage outRangeBinImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, interestImage.Width, interestImage.Height);
            //imageProcessing.Binarize(interestImage, outRangeBinImage, edgeThreshold, );

            //AlgoImage blobImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, interestImage.Width, interestImage.Height);

            //imageProcessing.Binarize(interestImage, blobImage, lower, upper, true);
            //imageProcessing.And(blobImage, maskImage, blobImage);

            BlobRectList blobRectList = imageProcessing.Blob(sobelBinImage, blobParam, interestImage);
            sobelBinImage.Dispose();
            interestImage.Dispose();

            AddDefectBlob(algoImage, blobRectList, inspectResult, avgValue, offsetX);
            blobRectList.Dispose();

            if (inspectResult.NumDefect > 0)
                inspectResult.Judgment = Judgment.Reject;

            algoImage.Dispose();

            return inspectResult;
        }

        public long[] GetHisto(ImageD image)
        {
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            long[] histo = imageProcessing.Histogram(algoImage);
            algoImage.Dispose();

            return histo;
        }

        void AddDefectBlob(AlgoImage algoImage, BlobRectList blobList, Data.InspectionResult inspectResult, float avgValue, int offset)
        {
            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
            PinholeParamValue paramValue;
            if (inspectResult.DeviceIndex == 0)
                paramValue = model.InspectParam.PinholeParamValue1;
            else
                paramValue = model.InspectParam.PinholeParamValue2;

            Rectangle srcRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            List<BlobRect> blobRectList = blobList.GetList();

            //int bin = paramValue.BinarizeValue;

            int defectNum = 0;

            blobRectList = blobRectList.OrderByDescending(blobRect => blobRect.Area).ToList();

            CancellationTokenSource cts = new CancellationTokenSource();

            // Use ParallelOptions instance to store the CancellationToken
            //ParallelOptions po = new ParallelOptions();
            //po.CancellationToken = cts.Token;

            //Parallel.ForEach(blobRectList, blobRect =>
            LogHelper.Debug(LoggerType.Inspection, string.Format("Start Clip Defects, {0}EA", blobRectList.Count));
            foreach (BlobRect blobRect in blobRectList)
            {
                if (inspectResult.NumDefect >= PinholeSettings.Instance().MaxDefect)
                    return;

                float width = blobRect.BoundingRect.Width * PinholeSettings.Instance().PixelResolution;
                float height = blobRect.BoundingRect.Height * PinholeSettings.Instance().PixelResolution;

                if (width < PinholeSettings.Instance().SmallSize.Width * 1000 && height < PinholeSettings.Instance().SmallSize.Height * 1000)
                    continue;

                RectangleF boundingRect = blobRect.BoundingRect;
                boundingRect.Offset(offset, 0);

                Rectangle clipRect = Rectangle.Round(boundingRect);
                clipRect.Inflate(10, 10);
                clipRect.Intersect(srcRect);

                if (clipRect.Width == 0 || clipRect.Height == 0)
                    continue;

                Data.DefectType defectType = blobRect.MeanValue < avgValue ? Data.DefectType.Dust : Data.DefectType.Pinhole;

                //AlgoImage subImage = algoImage.GetSubImage(clipRect, false);

                //ImageD imageD = subImage.ToImageD();

                //Bitmap bitmap = imageD.ToBitmap();
                //subImage.Dispose();

                //바뀐코드
                AlgoImage subImage = algoImage.GetSubImage(clipRect);
                byte[] data = subImage.GetByte();
                Bitmap bitmap = ImageHelper.CreateBitmap(clipRect.Width, clipRect.Height, clipRect.Width, 1, data);
                subImage.Dispose();

                //바뀐코드

                PointF realPosition = new PointF((blobRect.CenterPt.X + offset) * PinholeSettings.Instance().PixelResolution /*mm*/ , (blobRect.CenterPt.Y * PinholeSettings.Instance().PixelResolution) + inspectResult.RollDistance);

                DefectInfo defectInfo = new DefectInfo(inspectResult.DeviceIndex, inspectResult.SectionIndex, defectNum,
                                                            boundingRect, blobRect.CenterPt, realPosition, defectType
                                                            , (int)blobRect.MinValue, (int)blobRect.MaxValue, bitmap);
                inspectResult.AddDefectInfo(defectInfo);

                defectNum++;
            }
            LogHelper.Debug(LoggerType.Inspection, "End Clip Defects");
        }
    }
}

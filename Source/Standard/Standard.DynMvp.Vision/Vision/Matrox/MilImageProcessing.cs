using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Standard.DynMvp.Base;

using Matrox.MatroxImagingLibrary;
using System.Threading.Tasks;
using System.Diagnostics;
using Standard.DynMvp.Devices;
using Standard.DynMvp.Vision.Vision.Matrox.Object;
using Standard.DynMvp.Vision.Matrox.Object;

namespace Standard.DynMvp.Vision.Matrox
{
    public class MilImageProcessing : ImageProcessing
    {
        //public static object lockObject = new object(); 

        public override double Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            int val = (int)MIL.MimBinarize(milSrcImage.Image, MIL.M_NULL, MIL.M_BIMODAL + MIL.M_GREATER_OR_EQUAL, 0, 255);

            if (inverse == false)
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_FIXED + MIL.M_GREATER_OR_EQUAL, val, MIL.M_NULL);
            else
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_FIXED + MIL.M_LESS_OR_EQUAL, val, MIL.M_NULL);

            return val;
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (inverse == false)
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_FIXED + MIL.M_GREATER_OR_EQUAL, threshold,MIL.M_NULL);
            else
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_FIXED + MIL.M_LESS_OR_EQUAL, threshold, MIL.M_NULL);
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (inverse == false)
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_IN_RANGE, thresholdLower, thresholdUpper);
            else
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_OUT_RANGE, thresholdLower, thresholdUpper);
        }
        
        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
        {
            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL_ID contextId = MIL.MimAlloc(MIL.M_DEFAULT_HOST, MIL.M_BINARIZE_ADAPTIVE_CONTEXT, MIL.M_DEFAULT, MIL.M_NULL);
            int kSize = Math.Min(srcImage.Height, 50);
            MIL.MimControl(contextId, MIL.M_LOCAL_DIMENSION, kSize);

            MIL.MimBinarizeAdaptive(contextId, milSrcImage.Image, MIL.M_NULL, MIL.M_NULL, milDestImage.Image, MIL.M_NULL, MIL.M_DEFAULT);

            MIL.MimFree(contextId);
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Binarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Binarize", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_PERCENTILE_VALUE + MIL.M_GREATER_OR_EQUAL, percent, MIL.M_NULL);
        }

        public override void Morphology(AlgoImage srcImage, AlgoImage destImage, MorphologyType type, int[,] kernalXY, int repeatNum, bool isGray = false)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Erode", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Erode", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            Size kernalSize = new Size(kernalXY.GetLength(0), kernalXY.GetLength(1));
            //float[,] normalized = new float[kernalSize.Width, kernalSize.Height];
            for (int i = 0; i < kernalSize.Width; i++)
            {
                for (int j = 0; j < kernalSize.Height; j++)
                {
                    if (kernalXY[i, j] < 0)
                        kernalXY[i, j] = MIL.M_DONT_CARE;
                }
            }

            MIL_ID structId = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, kernalSize.Width, kernalSize.Height, 32, MIL.M_STRUCT_ELEMENT + MIL.M_NON_PAGED, MIL.M_NULL);
            MIL.MbufPut2d(structId, 0, 0, kernalSize.Width, kernalSize.Height, kernalXY);

            switch (type)
            {
                case MorphologyType.Dilate:
                    MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structId, MIL.M_DILATE, repeatNum, isGray ? MIL.M_GRAYSCALE : MIL.M_BINARY);
                    break;
                case MorphologyType.Erode:
                    MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structId, MIL.M_ERODE, repeatNum, isGray ? MIL.M_GRAYSCALE : MIL.M_BINARY);
                    break;
            }

            MIL.MbufFree(structId);
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Erode", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Erode", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;


            if (useGray)
                MIL.MimErode(milSrcImage.Image, milDestImage.Image, numErode, MIL.M_GRAYSCALE);
            else
                MIL.MimErode(milSrcImage.Image, milDestImage.Image, numErode, MIL.M_BINARY);
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, int kernelSize, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Dilate", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Dilate", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (useGray)
                MIL.MimDilate(milSrcImage.Image, milDestImage.Image, numDilate, MIL.M_GRAYSCALE);
            else
                MIL.MimDilate(milSrcImage.Image, milDestImage.Image, numDilate, MIL.M_BINARY);
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Open", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Open", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 1);

            if (useGray)
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_OPEN, numOpen, MIL.M_GRAYSCALE);
            else
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_OPEN, numOpen, MIL.M_BINARY);

            MIL.MbufFree(structureElement);

            //if (useGray)
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numOpen, MIL.M_GRAYSCALE);
            //else
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numOpen, MIL.M_BINARY);
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Close", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Close", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 1);

            if (useGray)
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_CLOSE, numClose, MIL.M_GRAYSCALE);
            else
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_CLOSE, numClose, MIL.M_BINARY);

            MIL.MbufFree(structureElement);

            //if (useGray)
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numClose, MIL.M_GRAYSCALE);
            //else
            //    MIL.MimOpen(milSrcImage.Image, milDestImage.Image, numClose, MIL.M_BINARY);
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.TopHat", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.TopHat", "Destination");

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 0);

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (useGray)
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_TOP_HAT, numTopHat, MIL.M_GRAYSCALE);
            else
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_TOP_HAT, numTopHat, MIL.M_BINARY);
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numBottomHat, bool useGray)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.BottomHat", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.BottomHat", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL_ID structureElement = MIL.M_NULL;
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 3, 3, 32 + MIL.M_UNSIGNED, MIL.M_STRUCT_ELEMENT, ref structureElement);
            MIL.MbufClear(structureElement, 0);

            if (useGray)
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_BOTTOM_HAT, numBottomHat, MIL.M_GRAYSCALE);
            else
                MIL.MimMorphic(milSrcImage.Image, milDestImage.Image, structureElement, MIL.M_BOTTOM_HAT, numBottomHat, MIL.M_BINARY);
        }

        private MilImResult Stat(AlgoImage algoImage, int type, int condition = 0, double conditionLow = 0, double conditionHigh = 0)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Stat", "Source");
            MilGreyImage milGreyImage = algoImage as MilGreyImage;

            MilImResult imResult = new MilImResult(MIL.M_DEFAULT, MIL.M_STAT_LIST);
            MIL.MimStat(milGreyImage.Image, imResult.Id, type, condition, conditionLow, conditionHigh);

            return imResult;
        }

        private MilImResult Stat(AlgoImage algoImage, int type, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Stat(with Mask)", "Source");
            if (maskImage != null)
                MilImage.CheckGreyImage(maskImage, "MilImageProcessing.Stat(with Mask)", "Mask");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;
            MilGreyImage milMaskImage = maskImage as MilGreyImage;

            MilImResult imResult = new MilImResult(MIL.M_DEFAULT, MIL.M_STAT_LIST);
            if (milMaskImage != null)
                MIL.MimStat(milGreyImage.Image, imResult.Id, type, MIL.M_MASK, milMaskImage.Image, 0);
            else
                MIL.MimStat(milGreyImage.Image, imResult.Id, type, 0, 0, 0);

            return imResult;
        }

        public override void Count(AlgoImage algoImage , out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            numWhitePixel = 0;
            numGreyPixel = 0;
            numBlackPixel = 0;

            MIL_INT tempValue1 = 0;
            MilImResult whitePixelResult = Stat(algoImage, MIL.M_NUMBER, MIL.M_GREATER_OR_EQUAL, 255, 0);
            MIL.MimGetResult(whitePixelResult.Id, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref tempValue1);
            numWhitePixel = (int)tempValue1;
            whitePixelResult.Dispose();

            MIL_INT tempValue2 = 0;
            MilImResult blackPixelResult = Stat(algoImage, MIL.M_NUMBER, MIL.M_LESS_OR_EQUAL, 0, 0);
            MIL.MimGetResult(blackPixelResult.Id, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref tempValue2);
            numBlackPixel = (int)tempValue2;
            blackPixelResult.Dispose();

            numGreyPixel = (algoImage.Width * algoImage.Height) - numWhitePixel - numBlackPixel;
        }

        private double length(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public override void Count(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Count(with Mask)", "Source");
            MilImage.CheckGreyImage(maskImage, "MilImageProcessing.Count(with Mask)", "Mask");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;
            MilGreyImage milMaskImage = maskImage as MilGreyImage;

            numWhitePixel = numGreyPixel = numBlackPixel = 0;

            int imageSize = milGreyImage.Width * milGreyImage.Height;
            int maskSize = milMaskImage.Width * milMaskImage.Height;
            if (imageSize != maskSize)
            {
                LogHelper.Error(LoggerType.Error, "Count : image and mask size is different.");
                return;
            }

            byte[] imageData = new byte[imageSize];
            byte[] maskData = new byte[imageSize];

            milGreyImage.Get(imageData);
            milMaskImage.Get(maskData);

            int whitePixel = 0;
            int greyPixel = 0;
            int blackPixel = 0;

//            Parallel.For(0, imageSize, index =>
            for (int index = 0; index < imageSize; index++)
            {
                if (maskData[index] > 0)
                {
                    if (imageData[index] > 200)
                        whitePixel++;
                    else if (imageData[index] > 100)
                        greyPixel++;
                    else
                        blackPixel++;
                }
            }

            numWhitePixel = whitePixel;
            numGreyPixel = greyPixel;
            numBlackPixel = blackPixel;
        }

        public override void Count2(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override void Count2(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage(with Mask)", "Source");

            StatResult statResult = new StatResult();
            MilImResult result = null;

            //lock (MilImageProcessing.lockObject)
            result = Stat(algoImage, MIL.M_NUMBER + MIL.M_MEAN + MIL.M_MIN + MIL.M_MAX + MIL.M_STANDARD_DEVIATION + MIL.M_SUM_OF_SQUARES, maskImage);
            MIL.MimGetResult(result.Id, MIL.M_NUMBER + MIL.M_TYPE_MIL_DOUBLE, ref statResult.count);
            MIL.MimGetResult(result.Id, MIL.M_MEAN + MIL.M_TYPE_MIL_DOUBLE, ref statResult.average);
            MIL.MimGetResult(result.Id, MIL.M_MIN + MIL.M_TYPE_MIL_DOUBLE, ref statResult.min);
            MIL.MimGetResult(result.Id, MIL.M_MAX + MIL.M_TYPE_MIL_DOUBLE, ref statResult.max);
            MIL.MimGetResult(result.Id, MIL.M_STANDARD_DEVIATION + MIL.M_TYPE_MIL_DOUBLE, ref statResult.stdDev);
            MIL.MimGetResult(result.Id, MIL.M_SUM_OF_SQUARES + MIL.M_TYPE_MIL_DOUBLE, ref statResult.squareSum);

            result.Dispose();

            return statResult;
        }

        public override Color GetColorAverage(AlgoImage algoImage, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public override Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyAverage(AlgoImage algoImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage", "Source");

            float average = 0;

            MilImResult result = null;
            result = Stat(algoImage, MIL.M_MEAN);

            MIL.MimGetResult(result.Id, MIL.M_MEAN + MIL.M_TYPE_MIL_FLOAT, ref average);

            result.Dispose();

            return average;
        }

        public override float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyAverage(with Mask)", "Source");

            float average = 0;
            MilImResult result = null;
            result = Stat(algoImage, MIL.M_MEAN, maskImage);

            MIL.MimGetResult(result.Id, MIL.M_MEAN + MIL.M_TYPE_MIL_FLOAT, ref average);

            result.Dispose();

            return average;
        }

        public override float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyMax(with Mask)", "Source");

            float max = 0;
            MilImResult result = null;

            if (maskImage == null)
                result = Stat(algoImage, MIL.M_MAX);
            else
                result = Stat(algoImage, MIL.M_MAX, maskImage);

            MIL.MimGetResult(result.Id, MIL.M_MAX + MIL.M_TYPE_MIL_FLOAT, ref max);

            result.Dispose();

            return max;
        }

        public override float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetGreyMin(with Mask)", "Source");

            float min = 0;
            MilImResult result = null;

            if (maskImage == null)
                result = Stat(algoImage, MIL.M_MIN);
            else
                result = Stat(algoImage, MIL.M_MIN, maskImage);

            MIL.MimGetResult(result.Id, MIL.M_MIN + MIL.M_TYPE_MIL_FLOAT, ref min);

            result.Dispose();

            return min;
        }

        public override float GetStdDev(AlgoImage algoImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetStdDev", "Source");

            float stdDev = 0;
            MilImResult result = Stat(algoImage, MIL.M_STANDARD_DEVIATION);
            MIL.MimGetResult(result.Id, MIL.M_STANDARD_DEVIATION + MIL.M_TYPE_MIL_FLOAT, ref stdDev);

            result.Dispose();

            return stdDev;
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.GetStdDev", "Source");

            float stdDev = 0;
            MilImResult result = Stat(algoImage, MIL.M_STANDARD_DEVIATION, maskImage);
            MIL.MimGetResult(result.Id, MIL.M_STANDARD_DEVIATION + MIL.M_TYPE_MIL_FLOAT, ref stdDev);

            result.Dispose();

            return stdDev;
        }

        public override float[] Projection(AlgoImage algoImage, AlgoImage maskImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage algoImage, Direction projectionDir, ProjectionType projectionType)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Projection", "Source");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;

            int nbEntries;
            double projAngle;

            if (projectionDir == Direction.Horizontal)
            {
                projAngle = MIL.M_0_DEGREE;
                nbEntries = milGreyImage.Width;
            }
            else
            {
                projAngle = MIL.M_90_DEGREE;
                nbEntries = milGreyImage.Height;
            }

            float[] projection = new float[nbEntries];

            MilImResult projResult = new MilImResult(nbEntries, MIL.M_PROJ_LIST);
            if (projResult.Id == MIL.M_NULL)
                throw new AllocFailedException("[MilImageProcessing.Projection]");

            MIL.MimProject(milGreyImage.Image, projResult.Id, projAngle);

            MIL.MimGetResult(projResult.Id, MIL.M_VALUE + MIL.M_TYPE_MIL_FLOAT, projection);

            if(projectionType == ProjectionType.Mean)
                for (long index = 0; index < nbEntries; index++)
                projection[index] /= ((projectionDir == Direction.Horizontal)? milGreyImage.Height: milGreyImage.Width);

            projResult.Dispose();

            return projection;
        }

        public override void Sobel(AlgoImage algoImage, int size = 3)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Sobel", "Source");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;
            
            if(size != 3 && size != 5 && size != 7)
            {
                throw new Exception("Size is must 3, 5, 7.");
            }

            if (size == 3)
            {
                MIL.MimConvolve(milGreyImage.Image, milGreyImage.Image, MIL.M_EDGE_DETECT);
                //MIL.MimEdgeDetect
            }
            else
            {
                MIL_ID xKernel1 = MIL.M_NULL;
                MIL_ID xKernel2 = MIL.M_NULL;
                MIL_ID yKernel1 = MIL.M_NULL;
                MIL_ID yKernel2 = MIL.M_NULL;

                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref xKernel1);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref xKernel2);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref yKernel1);
                MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, size, size, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref yKernel2);

                SByte[,] xKernelData1 = null;
                SByte[,] xKernelData2 = null;
                SByte[,] yKernelData1 = null;
                SByte[,] yKernelData2 = null;

                if(size == 5)
                {
                    xKernelData1 = new SByte[5, 5] { { 5, 8, 10, 8, 5 }, { 4, 10, 20, 10, 4 }, { 0, 0, 0, 0, 0 }, { -4, -10, -20, -10, -4 }, { -5, -8, -10, -8, -5 } };
                    xKernelData2 = new SByte[5, 5] { { -5, -8, -10, -8, -5 }, { -4, -10, -20, -10, -4 }, { 0, 0, 0, 0, 0 }, { 4, 10, 20, 10, 4 }, { 5, 8, 10, 8, 5 } };
                    yKernelData1 = new SByte[5, 5] { { -5, -4, 0, 4, 5 }, { -8, -10, 0, 10, 8 }, { -10, -20, 0, 20, 10 }, { -8, -10, 0, 10, 8 }, { -5, -4, 0, 4, 5 } };
                    yKernelData2 = new SByte[5, 5] { { 5, 4, 0, -4, -5 }, { 8, 10, 0, -10, -8 }, { 10, 20, 0, -20, -10 }, { 8, 10, 0, -10, -8 }, { 5, 4, 0, -4, -5 } };
                }

                MIL.MbufPut(xKernel1, xKernelData1);
                MIL.MbufPut(xKernel2, xKernelData2);
                MIL.MbufPut(yKernel1, yKernelData1);
                MIL.MbufPut(yKernel2, yKernelData2);

                MIL.MbufControlNeighborhood(xKernel1, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControlNeighborhood(xKernel2, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControlNeighborhood(yKernel1, MIL.M_NORMALIZATION_FACTOR, 64);
                MIL.MbufControlNeighborhood(yKernel2, MIL.M_NORMALIZATION_FACTOR, 64);

                MIL_ID xGradientImage1 = MIL.M_NULL;
                MIL_ID xGradientImage2 = MIL.M_NULL;
                MIL_ID yGradientImage1 = MIL.M_NULL;
                MIL_ID yGradientImage2 = MIL.M_NULL;

                MIL_ID xGradientImage = MIL.M_NULL;
                MIL_ID yGradientImage = MIL.M_NULL;

                int width = milGreyImage.Width;
                int height = milGreyImage.Height;

                xGradientImage1 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                xGradientImage2 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage1 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage2 = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                xGradientImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                yGradientImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                MIL.MimConvolve(milGreyImage.Image, xGradientImage1, xKernel1);
                MIL.MimConvolve(milGreyImage.Image, xGradientImage2, xKernel2);

                MIL.MimConvolve(milGreyImage.Image, yGradientImage1, yKernel1);
                MIL.MimConvolve(milGreyImage.Image, yGradientImage2, yKernel2);

                MIL.MimArith(xGradientImage1, xGradientImage2, xGradientImage, MIL.M_ADD);
                MIL.MimArith(yGradientImage1, yGradientImage2, yGradientImage, MIL.M_ADD);

                MIL.MimArith(xGradientImage, yGradientImage, milGreyImage.Image, MIL.M_ADD);

                MIL.MbufFree(xKernel1);
                MIL.MbufFree(xKernel2);
                MIL.MbufFree(yKernel1);
                MIL.MbufFree(yKernel2);

                MIL.MbufFree(xGradientImage1);
                MIL.MbufFree(xGradientImage2);
                MIL.MbufFree(yGradientImage1);
                MIL.MbufFree(yGradientImage2);
                MIL.MbufFree(xGradientImage);
                MIL.MbufFree(yGradientImage);
            }
        }

        public override void And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage1, "MilImageProcessing.And", "Source1");
            MilImage.CheckGreyImage(algoImage2, "MilImageProcessing.And", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.And", "Destination");

            MilGreyImage milSrc1Image = algoImage1 as MilGreyImage;
            MilGreyImage milSrc2Image = algoImage2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_AND);
        }

        public override void Not(AlgoImage algoImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Not", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Not", "Destination");

            MilGreyImage milSrcImage = algoImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_NOT);
        }

        public double MatchShape(AlgoImage algoImage1, AlgoImage algoImage2)
        {
            throw new NotImplementedException();
        }

        public void AdaptiveThreshold(AlgoImage algoImage1, AlgoImage algoImage2, double param1)
        {
            throw new NotImplementedException();
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override void Canny(AlgoImage algoImage, double threshold, double thresholdLinking)
        {
            throw new NotImplementedException();
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        private void SelectBlobFeature(MilBlobObject blobFeatureList, BlobParam blobParam)
        {
            if (blobParam.MaxCount > 0)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_RETURN_PARTIAL_RESULTS, MIL.M_ENABLE);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MAX_BLOBS, blobParam.MaxCount);
            }
            
            if (blobParam.SelectBoundingRect == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_BOX);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_BOX, MIL.M_ENABLE);
            }

            if (blobParam.SelectRotateRect == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MIN_AREA_BOX, MIL.M_ENABLE);
            }
                
            if (blobParam.SelectCenterPt == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_CENTER_OF_GRAVITY_X);
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_CENTER_OF_GRAVITY_Y);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CENTER_OF_GRAVITY, MIL.M_ENABLE);
            }

            if (blobParam.SelectSawToothArea == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_CONVEX_HULL_FILL_RATIO);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_CONVEX_HULL, MIL.M_ENABLE);
            }

            if (blobParam.SelectLabelValue == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_LABEL_VALUE);
                //MIL.MblobControl(blobFeatureList.Id, MIL.M_LABEL_VALUE, MIL.M_ENABLE);
            }
            if (blobParam.SelectCompactness == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_COMPACTNESS);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_COMPACTNESS, MIL.M_ENABLE);
            }
            if (blobParam.SelectRoughness == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_ROUGHNESS);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_ROUGHNESS, MIL.M_ENABLE);
            }

            if (blobParam.SelectFeretDiameter == true)
            {
                MIL.MblobControl(blobFeatureList.Id, MIL.M_FERETS, MIL.M_ENABLE);
            }

            if (blobParam.SelectNumberOfHoles == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_NUMBER_OF_HOLES);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_NUMBER_OF_HOLES, MIL.M_ENABLE);
            }
            if (blobParam.SelectMeanValue == true)
            {
                // MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_MEAN_PIXEL);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MEAN_PIXEL, MIL.M_ENABLE);
            }
            if (blobParam.SelectMinValue == true)
            {
                // MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_MIN_PIXEL);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MIN_PIXEL, MIL.M_ENABLE);
            }
            if (blobParam.SelectMaxValue == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_MAX_PIXEL);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_MAX_PIXEL, MIL.M_ENABLE);
            }
            if (blobParam.SelectSigmaValue == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_SIGMA_PIXEL);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_SIGMA_PIXEL, MIL.M_ENABLE);
            }
            if (blobParam.SelectAspectRatio == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_BOX_ASPECT_RATIO);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_BOX_ASPECT_RATIO, MIL.M_ENABLE);
            }
            if (blobParam.SelectRectangularity == true)
            {
                //MIL.MblobSelectFeature(blobFeatureList.Id, MIL.M_RECTANGULARITY);
                MIL.MblobControl(blobFeatureList.Id, MIL.M_RECTANGULARITY, MIL.M_ENABLE);
            }
        }

        public void FilterBlob(MilBlobObject blobResult, MilBlobRectList blobRectList, BlobParam blobParam)
        {
            MIL.MblobSelect(blobResult.Id, MIL.M_INCLUDE, MIL.M_ALL_BLOBS, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL);
            
            if (blobParam.AreaMin > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, MIL.M_LESS, blobParam.AreaMin, MIL.M_NULL);

            if (blobParam.AvgMin > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, MIL.M_LESS, blobParam.AvgMin, MIL.M_NULL);

            if (blobParam.AvgMax > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, MIL.M_GREATER, blobParam.AvgMax, MIL.M_NULL);

            if (blobParam.AreaMax > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_AREA, MIL.M_GREATER, blobParam.AreaMax, MIL.M_NULL);

            if (blobParam.BoundingRectMinX > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_X_MIN, MIL.M_LESS, blobParam.BoundingRectMinX, MIL.M_NULL);

            if (blobParam.BoundingRectMinY > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_Y_MIN, MIL.M_LESS, blobParam.BoundingRectMinY, MIL.M_NULL);

            if (blobParam.BoundingRectMaxX > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_X_MAX, MIL.M_GREATER, blobParam.BoundingRectMaxX, MIL.M_NULL);

            if (blobParam.BoundingRectMaxY > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BOX_Y_MAX, MIL.M_GREATER, blobParam.BoundingRectMaxY, MIL.M_NULL);
            
            if (blobParam.SelectSigmaValue == true)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_SIGMA_PIXEL, MIL.M_LESS, blobParam.SigmaMin, MIL.M_NULL);

            if (blobParam.EraseBorderBlobs == true)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_BLOB_TOUCHING_IMAGE_BORDERS, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL);

            if (blobParam.SelectBorderBlobs == true)
                MIL.MblobSelect(blobResult.Id, MIL.M_INCLUDE_ONLY, MIL.M_BLOB_TOUCHING_IMAGE_BORDERS, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL);

            if (blobParam.RotateWidthMin > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_WIDTH, MIL.M_LESS, blobParam.RotateWidthMin, MIL.M_NULL);

            if (blobParam.RotateWidthMax > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_WIDTH, MIL.M_GREATER, blobParam.RotateWidthMax, MIL.M_NULL);
            
            if (blobParam.RotateHeightMin > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_HEIGHT, MIL.M_LESS, blobParam.RotateHeightMin, MIL.M_NULL);
            
            if (blobParam.RotateHeightMax > 0)
                MIL.MblobSelect(blobResult.Id, MIL.M_DELETE, MIL.M_MIN_AREA_BOX_HEIGHT, MIL.M_GREATER, blobParam.RotateHeightMax, MIL.M_NULL);

            GetBlobBox(blobResult, blobRectList, blobParam);
        }

        public void GetBlobBox(MilBlobObject blobResult, BlobRectList blobRectList, BlobParam blobParam)
        {
            blobRectList.Clear();

            double numBlobD = 0;
            double isReached = 0;
            MIL.MblobGetResult(blobResult.Id, MIL.M_MAX_BLOBS_END, ref isReached);

            if (isReached > 0)
            {
                blobRectList.IsReached = true;
                return;
            }
                
            MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_NUMBER, ref numBlobD);
            
            int numBlob = (int)numBlobD;
            if (numBlob > 0)
            {
                double[] leftArray = new double[numBlob];
                double[] rightArray = new double[numBlob];
                double[] topArray = new double[numBlob];
                double[] bottomArray = new double[numBlob];

                double[] centerXArray = new double[numBlob];
                double[] centerYArray = new double[numBlob];

                double[] areaArray = new double[numBlob];
                double[] labelNumArray = new double[numBlob];
                double[] sigmaValueArray = new double[numBlob];
                double[] minValueArray = new double[numBlob];
                double[] maxValueArray = new double[numBlob];
                double[] meanValueArray = new double[numBlob];
                double[] compactnessArray = new double[numBlob];
                double[] convexFillRatioArray = new double[numBlob];
                double[] aspectRetioArray = new double[numBlob];
                double[] rectangularityArray = new double[numBlob];
                double[] roughnessArray = new double[numBlob];

                double[] minFeretArray = new double[numBlob];
                double[] maxFeretArray = new double[numBlob];
                //RotateRect
                double[] rotateRectAngle = new double[numBlob];
                double[] rotateRectX1 = new double[numBlob];
                double[] rotateRectX2 = new double[numBlob];
                double[] rotateRectX3 = new double[numBlob];
                double[] rotateRectX4 = new double[numBlob];
                double[] rotateRectY1 = new double[numBlob];
                double[] rotateRectY2 = new double[numBlob];
                double[] rotateRectY3 = new double[numBlob];
                double[] rotateRectY4 = new double[numBlob];
                double[] rotateRectWidth = new double[numBlob];
                double[] rotateRectHeight = new double[numBlob];
                double[] rotateRectCenterX = new double[numBlob];
                double[] rotateRectCenterY = new double[numBlob];
                MIL_INT[] numberOfHoles = new MIL_INT[numBlob];
                
                if (blobParam.SelectRotateRect)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_ANGLE, rotateRectAngle);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_X1, rotateRectX1);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_X2, rotateRectX2);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_X3, rotateRectX3);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_X4, rotateRectX4);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_Y1, rotateRectY1);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_Y2, rotateRectY2);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_Y3, rotateRectY3);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_Y4, rotateRectY4);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_WIDTH, rotateRectWidth);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_HEIGHT, rotateRectHeight);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_CENTER_X, rotateRectCenterX);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_AREA_BOX_CENTER_Y, rotateRectCenterY);
                }

                if (blobParam.SelectCenterPt)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_CENTER_OF_GRAVITY_X + MIL.M_TYPE_DOUBLE, centerXArray);
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_CENTER_OF_GRAVITY_Y + MIL.M_TYPE_DOUBLE, centerYArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_CENTER_OF_GRAVITY_X, centerXArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_CENTER_OF_GRAVITY_Y, centerYArray);
                }

                if (blobParam.SelectBoundingRect == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_BOX_X_MIN + MIL.M_TYPE_DOUBLE, leftArray);
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_BOX_Y_MIN + MIL.M_TYPE_DOUBLE, topArray);
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_BOX_X_MAX + MIL.M_TYPE_DOUBLE, rightArray);
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_BOX_Y_MAX + MIL.M_TYPE_DOUBLE, bottomArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_BOX_X_MIN, leftArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_BOX_Y_MIN, topArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_BOX_X_MAX, rightArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_BOX_Y_MAX, bottomArray);
                }

                if (blobParam.SelectArea == true)
                {  //MIL.MblobGetResult(blobResult.Id, MIL.M_AREA + MIL.M_TYPE_DOUBLE, areaArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_AREA, areaArray);
                }
                if (blobParam.SelectLabelValue == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_LABEL_VALUE + MIL.M_TYPE_DOUBLE, labelNumArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_LABEL_VALUE, labelNumArray);
                }
                if (blobParam.SelectNumberOfHoles)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_NUMBER_OF_HOLES + MIL.M_TYPE_MIL_INT, numberOfHoles);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_NUMBER_OF_HOLES, numberOfHoles);
                }
                if (blobParam.SelectMinValue == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_MIN_PIXEL + MIL.M_TYPE_DOUBLE, minValueArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MIN_PIXEL, minValueArray);
                }
                if (blobParam.SelectMaxValue == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_MAX_PIXEL + MIL.M_TYPE_DOUBLE, maxValueArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MAX_PIXEL, maxValueArray);
                }
                if (blobParam.SelectMeanValue == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_MEAN_PIXEL + MIL.M_TYPE_DOUBLE, meanValueArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_MEAN_PIXEL, meanValueArray);
                }
                if (blobParam.SelectSigmaValue == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_SIGMA_PIXEL + MIL.M_TYPE_DOUBLE, sigmaValueArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_SIGMA_PIXEL, sigmaValueArray);
                }
                if (blobParam.SelectCompactness == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_COMPACTNESS + MIL.M_TYPE_DOUBLE, compactnessArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_COMPACTNESS, compactnessArray);
                }
                if (blobParam.SelectSawToothArea == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_CONVEX_HULL_FILL_RATIO + MIL.M_TYPE_DOUBLE, convexFillRatioArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_CONVEX_HULL_AREA, convexFillRatioArray);
                }
                if (blobParam.SelectAspectRatio == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_BOX_ASPECT_RATIO + MIL.M_TYPE_DOUBLE, aspectRetioArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_BOX_ASPECT_RATIO, aspectRetioArray);
                }
                if (blobParam.SelectRectangularity == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_RECTANGULARITY + MIL.M_TYPE_DOUBLE, rectangularityArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_RECTANGULARITY, rectangularityArray);
                }
                if (blobParam.SelectRoughness == true)
                {
                    //MIL.MblobGetResult(blobResult.Id, MIL.M_ROUGHNESS + MIL.M_TYPE_DOUBLE, roughnessArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_ROUGHNESS, roughnessArray);
                }
                if (blobParam.SelectFeretDiameter == true)
                {
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_FERET_MIN_DIAMETER, minFeretArray);
                    MIL.MblobGetResult(blobResult.Id, MIL.M_DEFAULT, MIL.M_FERET_MAX_DIAMETER, maxFeretArray);
                }
                
                for (int i = 0; i < numBlob; i++)
                {
                    BlobRect blobRect = new BlobRect();
                    blobRect.LabelNumber = (int)labelNumArray[i];
                    blobRect.Area = (long)areaArray[i];
                    blobRect.BoundingRect = new RectangleF((float)leftArray[i], (float)topArray[i], (float)(rightArray[i] - leftArray[i] + 1), (float)(bottomArray[i] - topArray[i] + 1));
                    if (blobParam.SelectCenterPt)
                        blobRect.CenterPt = new PointF((float)centerXArray[i], (float)centerYArray[i]);
                    else
                        blobRect.CenterPt = DrawingHelper.CenterPoint(blobRect.BoundingRect);
                    blobRect.CalcCenterOffset();
                    blobRectList.Append(blobRect);

                    blobRect.SigmaValue = (float)sigmaValueArray[i];
                    blobRect.MinValue = (float)minValueArray[i];
                    blobRect.MaxValue = (float)maxValueArray[i];
                    blobRect.MeanValue = (float)meanValueArray[i];
                    blobRect.Compactness = (float)compactnessArray[i];
                    blobRect.NumberOfHoles = (int)numberOfHoles[i];
                    blobRect.SawToothArea = (float)convexFillRatioArray[i] - (float)labelNumArray[i];
                    blobRect.AspectRetio = (float)aspectRetioArray[i];
                    blobRect.Rectangularity = (float)rectangularityArray[i];
                    blobRect.Roughness = (float)roughnessArray[i];
                    blobRect.MinFeretDiameter = (float)minFeretArray[i];
                    blobRect.MaxFeretDiameter = (float)maxFeretArray[i];

                    blobRect.RotateWidth = (float)rotateRectWidth[i];
                    blobRect.RotateHeight = (float)rotateRectHeight[i];
                    blobRect.RotateAngle = (float)rotateRectAngle[i];
                    blobRect.RotateXArray = new float[] { (float)rotateRectX1[i], (float)rotateRectX2[i], (float)rotateRectX4[i], (float)rotateRectX3[i] };
                    blobRect.RotateYArray = new float[] { (float)rotateRectY1[i], (float)rotateRectY2[i], (float)rotateRectY4[i], (float)rotateRectY3[i] };
                    blobRect.RotateCenterPt = new PointF((float)rotateRectCenterX[i], (float)rotateRectCenterY[i]);
                }
            }
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask = null)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.Blob", "Source");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;

            MilBlobObject blobFeatureList = new MilBlobObject();
            MilBlobRectList blobRectList = new MilBlobRectList();

            if (blobFeatureList.Id == MIL.M_NULL || blobRectList.BlobResult.Id == MIL.M_NULL)
                throw new AllocFailedException("[MilImageProcessing.Blob]");

            MilBlobObject blobResult = blobRectList.BlobResult;

            MIL.MblobControl(blobFeatureList.Id, MIL.M_IDENTIFIER_TYPE, MIL.M_BINARY);

            SelectBlobFeature(blobFeatureList, blobParam);
            
            if (greyMask != null)
            {
                MilGreyImage milGreyMaskImage = greyMask as MilGreyImage;
                MIL.MblobCalculate(blobFeatureList.Id, milGreyImage.Image, milGreyMaskImage.Image, blobResult.Id);
            }
            else
            {
                MIL.MblobCalculate(blobFeatureList.Id, milGreyImage.Image, MIL.M_NULL, blobResult.Id);
            }
            
            FilterBlob(blobResult, blobRectList, blobParam);

            blobFeatureList.Dispose();
            return blobRectList;
        }
        
        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
        {
            MilImage.CheckGreyImage(algoImage, "MilImageProcessing.DrawBlob", "AlgoImage");

            MilGreyImage milGreyImage = algoImage as MilGreyImage;

            MilBlobRectList milBlobRectList = (MilBlobRectList)blobRectList;
            
            if (blobRect == null)
            {
                if (drawBlobOption.SelectBlob)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS, MIL.M_INCLUDED_BLOBS, MIL.M_DEFAULT);
                if (drawBlobOption.SelectBlobContour)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS_CONTOUR, MIL.M_INCLUDED_BLOBS, MIL.M_DEFAULT);
                if (drawBlobOption.SelectHoles)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES, MIL.M_INCLUDED_BLOBS, MIL.M_DEFAULT);
                if (drawBlobOption.SelectHolesContour)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES_CONTOUR, MIL.M_INCLUDED_BLOBS, MIL.M_DEFAULT);
            }
            else
            {
                if (drawBlobOption.SelectBlob)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS, (int)blobRect.LabelNumber, MIL.M_DEFAULT);
                if (drawBlobOption.SelectBlobContour)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_BLOBS_CONTOUR, (int)blobRect.LabelNumber, MIL.M_DEFAULT);
                if (drawBlobOption.SelectHoles)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES, (int)blobRect.LabelNumber, MIL.M_DEFAULT);
                if (drawBlobOption.SelectHolesContour)
                    MIL.MblobDraw(MIL.M_DEFAULT, milBlobRectList.BlobResult.Id, milGreyImage.Image, MIL.M_DRAW_HOLES_CONTOUR, (int)blobRect.LabelNumber, MIL.M_DEFAULT);
            }
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDstImage = destImage as MilGreyImage;

            MIL.MimConvolve(milSrcImage.Image, milDstImage.Image, MIL.M_SMOOTH);
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            const int HIST_NUM_INTENSITIES = 256;
            MIL_ID HistResult = MIL.M_NULL;                       // Histogram buffer identifier.
            MilGreyImage milSrcImage = algoImage as MilGreyImage;
            MIL.MimAllocResult(MIL.M_DEFAULT_HOST, HIST_NUM_INTENSITIES, MIL.M_HIST_LIST, ref HistResult);
           // HistResult=MIL.MimAllocResult(MIL.M_DEFAULT_HOST, HIST_NUM_INTENSITIES, MIL.M_HIST_LIST, MIL.M_NULL);
            MIL.MimHistogram(milSrcImage.Image, HistResult);
            MIL_INT[] HistValues = new MIL_INT[HIST_NUM_INTENSITIES];
            MIL.MimGetResult(HistResult, MIL.M_VALUE, HistValues);

            long[] retVal = new long[HistValues.Length];
            for (int i = 0; i < HistValues.Length; i++) retVal[i] = (long)HistValues[i];
            MIL.MbufFree(HistResult);
            return retVal; 
        }
        public override void HistogramEqualization(AlgoImage algoImage)
        {
            MilGreyImage milSrcImage = algoImage as MilGreyImage;
            MIL.MimHistogramEqualize(milSrcImage.Image, milSrcImage.Image, MIL.M_HYPER_LOG, 0, 0, 255);
        }

        public override void FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            MIL.MblobReconstruct(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_FILL_HOLES, MIL.M_DEFAULT);
        }

        public override void WeightedAdd(AlgoImage[] srcImages, AlgoImage dstImage)
        {
            for(int i=0; i<srcImages.Length; i++)
                MilImage.CheckGreyImage(srcImages[i], "MilImageProcessing.WeightedAdd", "srcImages[i]");
            MilImage.CheckGreyImage(dstImage, "MilImageProcessing.WeightedAdd", "Destination");

            //Debug.Assert(Array.TrueForAll(srcImages, f => f.Size == dstImage.Size), "Image Size Missmatch");

            MilGreyImage milDstImage = dstImage as MilGreyImage;
            
            MIL_ID dstBuf = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, milDstImage.Width, milDstImage.Height, 32 + MIL.M_FLOAT, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            MIL.MbufClear(dstBuf, 0);

            //int size = 0;
            //MIL.MbufInquire(dstBuf, MIL.M_SIZE_BYTE, ref size);
            for (int i = 0; i < srcImages.Length; i++)
            {
                MilGreyImage milSrcImage = srcImages[i] as MilGreyImage;
                //MIL.MbufSave(string.Format(@"d:\temp\MIL\{0}A_srcBuf.bmp", i), milSrcImage.Image);

                MIL_ID tmpBuf = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, milSrcImage.Width, milSrcImage.Height, 32 + MIL.M_FLOAT, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                MIL.MbufCopy(milSrcImage.Image, tmpBuf);
                MIL.MimArith(tmpBuf, 255, tmpBuf, MIL.M_DIV_CONST);
                MIL.MimArith(tmpBuf, srcImages.Length, tmpBuf, MIL.M_DIV_CONST);
                //MIL.MbufSave(string.Format(@"d:\temp\MIL\{0}B_tmpBuf.bmp", i), tmpBuf);

                MIL.MimArith(dstBuf, tmpBuf, dstBuf, MIL.M_ADD);
                //MIL.MbufSave(string.Format(@"d:\temp\MIL\{0}C_dstBuf.bmp", i), dstBuf);

                MIL.MbufFree(tmpBuf);
            }

            MIL.MimArith(dstBuf, 255, dstBuf, MIL.M_MULT_CONST);
            MIL.MbufCopy(dstBuf, milDstImage.Image);
            MIL.MbufFree(dstBuf);
            //milDstImage.Save(@"d:\temp\tt.bmp");
        }

        public override void Add(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Subtract", "Source1");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Subtract", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Subtract", "Destination");

            MilGreyImage milSrc1Image = image1 as MilGreyImage;
            MilGreyImage milSrc2Image = image2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_ADD+ MIL.M_SATURATION);
        }

        public override void Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool isAbs = false)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Subtract", "Source1");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Subtract", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Subtract", "Destination");

            MilGreyImage milSrc1Image = image1 as MilGreyImage; 
            MilGreyImage milSrc2Image = image2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (isAbs == true)
                MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_SUB_ABS + MIL.M_SATURATION);
            else
                MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_SUB + MIL.M_SATURATION);
        }
        
        public override void Min(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Min", "Source1");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Min", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Min", "Destination");

            MilGreyImage milSrc1Image = image1 as MilGreyImage;
            MilGreyImage milSrc2Image = image2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_MIN);
        }

        public override void Max(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Max", "Source1");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Max", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Max", "Destination");

            MilGreyImage milSrc1Image = image1 as MilGreyImage;
            MilGreyImage milSrc2Image = image2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_MAX);
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.FillHoles", "Source");
            if (destImage != null)
            {
                MilImage.CheckGreyImage(destImage, "MilImageProcessing.FillHoles", "Destination");
            }

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            if (destImage == null)
                destImage = milSrcImage;

            MilGreyImage milDestImage = destImage as MilGreyImage;
            MIL.MimRank(milSrcImage.Image, milDestImage.Image, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_GRAYSCALE);
        }

        public override void Clear(AlgoImage image, byte value)
        {
            MilImage.CheckGreyImage(image, "MilImageProcessing.Clear", "image");
            
            MilGreyImage milSrcImage = image as MilGreyImage;

            MIL.MbufClear(milSrcImage.Image, value);
        }

        public override void Clear(AlgoImage image, Rectangle rect, Color value)
        {
            MilImage.CheckGreyImage(image, "MilImageProcessing.Clear", "image");

            MilGreyImage milSrcImage = image as MilGreyImage;

            if (rect.IsEmpty)
            {
                if (value == Color.Black)
                    MIL.MbufClear(milSrcImage.Image, MIL.M_COLOR_BLACK);
                else if (value == Color.Blue)
                    MIL.MbufClear(milSrcImage.Image, MIL.M_COLOR_BLUE);
                else if(value == Color.White)
                    MIL.MbufClear(milSrcImage.Image, MIL.M_COLOR_WHITE);
            }
            else
            {
                AlgoImage subImage = image.GetSubImage(rect);
                this.Clear(subImage, value);
                subImage.Dispose();
            }
        }

        public override void Clear(AlgoImage image, AlgoImage mask, Color value)
        {
            MilImage.CheckGreyImage(image, "MilImageProcessing.Clear", "image");
            MilImage.CheckGreyImage(image, "MilImageProcessing.Clear", "mask");

            MilGreyImage milSrcImage = image as MilGreyImage;
            MilGreyImage milMskImage = mask as MilGreyImage;

            throw new NotImplementedException();
        }

        public override void Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(algoImage1, "MilImageProcessing.Or", "Source1");
            MilImage.CheckGreyImage(algoImage2, "MilImageProcessing.Or", "Source2");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Or", "Destination");

            MilGreyImage milSrc1Image = algoImage1 as MilGreyImage;
            MilGreyImage milSrc2Image = algoImage2 as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL.MimArith(milSrc1Image.Image, milSrc2Image.Image, milDestImage.Image, MIL.M_OR);
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Or", "Source1");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Or", "Source2");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL.MimTranslate(milSrcImage.Image, milDestImage.Image, offset.X, offset.Y, MIL.M_DEFAULT);
        }

        public override void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled)
        {
            //MilImage.CheckGreyImage(srcImage, "MilImageProcessing.DarwRect", "Source");

            MilImage milSrcImage = srcImage as MilImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);

            if (milSrcImage is MilGreyImage)
            {
                MIL.MgraColor(milGraphicsContext, value);
            }
            else if (milSrcImage is MilColorImage)
            {
                int v = (int)value;
                int a = (v >> 24) & 0xff;
                int r = (v >> 16) & 0xff;
                int g = (v >> 8) & 0xff;
                int b = (v >> 0) & 0xff;
                MIL.MgraColor(milGraphicsContext, MIL.M_RGB888(r, g, b));
            }

            if (filled == true)
                MIL.MgraRectFill(milGraphicsContext, milSrcImage.Image, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            else
                MIL.MgraRect(milGraphicsContext, milSrcImage.Image, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

            MIL.MgraFree(milGraphicsContext);
        }
        
        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.EraseBoder", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.EraseBoder", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MIL.MblobReconstruct(milSrcImage.Image, MIL.M_NULL, milDestImage.Image, MIL.M_ERASE_BORDER_BLOBS, MIL.M_DEFAULT);
        }
        public override void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.ResconstructIncludeBlob", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.ResconstructIncludeBlob", "Destination");
            MilImage.CheckGreyImage(seedImage, "MilImageProcessing.ResconstructIncludeBlob", "Seed");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            MilGreyImage milSeedImage = seedImage as MilGreyImage;

            MIL.MblobReconstruct(milSrcImage.Image, milSeedImage.Image, milDestImage.Image, MIL.M_RECONSTRUCT_FROM_SEED, MIL.M_DEFAULT);
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.EdgeDetect", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.EdgeDetect", "Destination");
            
            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            MilGreyImage milMaskImage = null;

            if (maskImage != null)
            {
                MilImage.CheckGreyImage(maskImage, "MilImageProcessing.EdgeDetect", "Mask");
                milMaskImage = maskImage as MilGreyImage;
            }
                
            MIL_ID milEdgeContext = MIL.M_NULL;
            MIL_ID milEdgeResult = MIL.M_NULL;

            MIL.MedgeAlloc(MIL.M_DEFAULT_HOST, MIL.M_CONTOUR, MIL.M_DEFAULT, ref milEdgeContext);
            
            MIL.MedgeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref milEdgeResult);
            
            MIL.MedgeControl(milEdgeContext, MIL.M_FILTER_TYPE, MIL.M_SOBEL);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILTER_SMOOTHNESS, 0);

            MIL.MedgeControl(milEdgeContext, MIL.M_ACCURACY, MIL.M_HIGH);
            MIL.MedgeControl(milEdgeContext, MIL.M_ANGLE_ACCURACY, MIL.M_HIGH);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_MODE, MIL.M_MEDIUM);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_TYPE, MIL.M_HYSTERESIS);

            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_DISTANCE, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_ANGLE, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_FILL_GAP_CONTINUITY, 0);
            MIL.MedgeControl(milEdgeContext, MIL.M_THRESHOLD_TYPE, MIL.M_HYSTERESIS);

            MIL.MedgeControl(milEdgeContext, MIL.M_EXTRACTION_SCALE, scaleFactor);
            
            MIL.MedgeControl(milEdgeContext, MIL.M_STRENGTH, MIL.M_ENABLE);
            if (maskImage != null)
            {
                MIL.MedgeMask(milEdgeContext, milMaskImage.Image, MIL.M_DEFAULT);
            }

            MIL.MedgeCalculate(milEdgeContext, milSrcImage.Image, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, milEdgeResult, MIL.M_DEFAULT);

            //MIL.MedgeSelect(milEdgeResult, MIL.M_DELETE, MIL.M_AVERAGE_STRENGTH, MIL.M_LESS, 5000, MIL.M_NULL);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
            MIL.MedgeDraw(MIL.M_DEFAULT, milEdgeResult, milDestImage.Image, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);
            
            MIL.MedgeFree(milEdgeContext);
            MIL.MedgeFree(milEdgeResult);
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY)
        {
            MilImage.CheckMilImage(srcImage, "MilImageProcessing.Resize", "Source");
            MilImage.CheckMilImage(destImage, "MilImageProcessing.Resize", "Destination");

            MilImage milSrcImage = srcImage as MilImage;
            MilImage milDestImage = destImage as MilImage;

            if (scaleFactorX <= 0)
                scaleFactorX = MIL.M_FILL_DESTINATION;
            if (scaleFactorY <= 0)
                scaleFactorY = MIL.M_FILL_DESTINATION;
            MIL.MimResize(milSrcImage.Image, milDestImage.Image, scaleFactorX, scaleFactorY, MIL.M_BILINEAR /* MIL.M_NEAREST_NEIGHBOR*/);
        }

        public override void Stitch(AlgoImage srcImage1, AlgoImage srcImage2, AlgoImage destImage, Direction direction)
        {
            MilImage.CheckMilImage(srcImage1, "MilImageProcessing.Stitch", "Source1");
            MilImage.CheckMilImage(srcImage2, "MilImageProcessing.Stitch", "Source2");
            MilImage.CheckMilImage(destImage, "MilImageProcessing.Stitch", "Destination");

            MilImage milSrcImage1 = srcImage1 as MilImage;
            MilImage milSrcImage2 = srcImage2 as MilImage;
            MilImage milDestImage = destImage as MilImage;

            MilImage[] images = new MilImage[2] { milSrcImage1, milSrcImage2 };
            Rectangle copyRect = Rectangle.Empty;
            for (int i = 0; i < 2; i++)
            {
                MilImage image = images[i];
                switch (direction)
                {
                    case Direction.Horizontal:
                        copyRect = new Rectangle(copyRect.Right, 0, image.Width, image.Height);
                        break;
                    case Direction.Vertical:
                        copyRect = new Rectangle(0, copyRect.Bottom, image.Width, image.Height);
                        break;
                }
                MIL.MbufCopyClip(image.Image, milDestImage.Image, copyRect.X, copyRect.Y);
            }
        }

        public override float[,] FFT(AlgoImage srcImage)
        {
            if (srcImage == null)
                return null;

            int width = srcImage.Width;
            int height = srcImage.Height;

            MilGreyImage milSrcImage = srcImage as MilGreyImage;

            MIL_ID realImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, 32 + MIL.M_FLOAT, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            MIL_ID trasformImage = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, 32 + MIL.M_FLOAT, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

            MIL.MimTransform(milSrcImage.Image, MIL.M_NULL, realImage, trasformImage, MIL.M_FFT, MIL.M_FORWARD + MIL.M_MAGNITUDE + MIL.M_LOG_SCALE);// + MIL.M_MAGNITUDE + MIL.M_LOG_SCALE);

            float[,] array = new float[width, height];

            //MIL.MbufExport("d:\\realImage.bmp", MIL.M_BMP, realImage);

            MIL.MbufGet2d(realImage, 0, 0, width, height, array);

            MIL.MbufFree(trasformImage);
            MIL.MbufFree(realImage);

            return array;
        }

        public override void Add(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Add", "Source");
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Add", "Dest");

            MilGreyImage milGreySrcImage = srcImage as MilGreyImage;
            MilGreyImage milGreyDestImage = dstImage as MilGreyImage;

            MIL.MimArith(milGreySrcImage.Image, value, milGreyDestImage.Image, MIL.M_ADD_CONST + MIL.M_SATURATION);
        }

        public override void Subtract(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Add", "Source");
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Add", "Dest");

            MilGreyImage milGreySrcImage = srcImage as MilGreyImage;
            MilGreyImage milGreyDestImage = dstImage as MilGreyImage;

            MIL.MimArith(milGreySrcImage.Image, value, milGreyDestImage.Image, MIL.M_SUB_CONST + MIL.M_SATURATION);
        }

        public override void Mul(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Mul", "Source");
            MilImage.CheckGreyImage(dstImage, "MilImageProcessing.Mul", "Source");

            MilGreyImage milGreySrcImage = srcImage as MilGreyImage;
            MilGreyImage milGreyDestImage = dstImage as MilGreyImage;

            MIL.MimArith(milGreySrcImage.Image, value, milGreyDestImage.Image, MIL.M_MULT_CONST + MIL.M_SATURATION);
        }

        public override void Div(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Div", "Source");
            MilImage.CheckGreyImage(dstImage, "MilImageProcessing.Div", "Source");

            MilGreyImage milGreySrcImage = srcImage as MilGreyImage;
            MilGreyImage milGreyDestImage = dstImage as MilGreyImage;

            MIL.MimArith(milGreySrcImage.Image, value, milGreyDestImage.Image, MIL.M_DIV_CONST);
        }

        public override void Compare(AlgoImage image1, AlgoImage image2, AlgoImage dstImage)
        {
            MilImage.CheckGreyImage(image1, "MilImageProcessing.Compare", "Source");
            MilImage.CheckGreyImage(image2, "MilImageProcessing.Compare", "Source");
            MilImage.CheckGreyImage(dstImage, "MilImageProcessing.Compare", "Destination");

            MilGreyImage milGreyImage1 = image1 as MilGreyImage;
            MilGreyImage milGreyImage2 = image2 as MilGreyImage;
            MilGreyImage milDestGreyImage = dstImage as MilGreyImage;

            MIL.MimArith(milGreyImage1.Image, milGreyImage2.Image, milDestGreyImage.Image, MIL.M_GREATER_OR_EQUAL);
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Sobel", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.Sobel", "Destination");

            MilGreyImage milSrcGreyImage = srcImage as MilGreyImage;
            MilGreyImage milDestGreyImage = destImage as MilGreyImage;

            MIL.MimConvolve(milSrcGreyImage.Image, milDestGreyImage.Image, MIL.M_EDGE_DETECT_SOBEL_FAST);
        }

        public override void CustomEdge(AlgoImage srcImage, AlgoImage destImage, AlgoImage bufferX, AlgoImage bufferY, int length)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.CustomSobel", "Source");

            MilGreyImage milSrc = srcImage as MilGreyImage;
            MilGreyImage milBufferX = bufferX as MilGreyImage;
            MilGreyImage milBufferY = bufferY as MilGreyImage;
            MilGreyImage milDest = bufferY as MilGreyImage;
            MIL_ID xKernel = MIL.M_NULL;
            MIL_ID yKernel = MIL.M_NULL;

            int sumLenght = length * 2 + 1;

            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, sumLenght, 1, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref xKernel);
            MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, 1, sumLenght, 8 + MIL.M_SIGNED, MIL.M_KERNEL, ref yKernel);

            SByte[,] xKernelData = new sbyte[sumLenght, 1];
            SByte[,] yKernelData = new sbyte[1, sumLenght];

            xKernelData[0, 0] = -1;
            xKernelData[sumLenght - 1, 0] = 1;

            yKernelData[0, 0] = -1;
            yKernelData[0, sumLenght - 1] = 1;
            
            MIL.MbufPut(xKernel, xKernelData);
            MIL.MbufPut(yKernel, yKernelData);
            
            MIL.MimConvolve(milSrc.Image, milBufferX.Image, xKernel);
            MIL.MimConvolve(milSrc.Image, milBufferY.Image, yKernel);

            MIL.MimTranslate(milBufferX.Image, milBufferX.Image, length, 0, MIL.M_DEFAULT);
            MIL.MimTranslate(milBufferY.Image, milBufferY.Image, 0, length, MIL.M_DEFAULT);

            MIL.MimArith(milBufferX.Image, milBufferY.Image, milDest.Image, MIL.M_ADD);
        }

        public override void Clipping(AlgoImage srcImage, int minValue, int maxValue)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.Sobel", "Source");

            MilGreyImage milSrcGreyImage = srcImage as MilGreyImage;
            MIL.MimClip(milSrcGreyImage.Image, milSrcGreyImage.Image, MIL.M_OUT_RANGE, minValue, maxValue, MIL.M_NULL, 255);
        }

        public override void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.CustomBinarize", "Source");
            MilImage.CheckGreyImage(destImage, "MilImageProcessing.CustomBinarize", "Destination");

            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            if (inverse == false)
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_TRIANGLE_BISECTION_DARK + MIL.M_GREATER, 0, 255);
            else
                MIL.MimBinarize(milSrcImage.Image, milDestImage.Image, MIL.M_TRIANGLE_BISECTION_DARK + MIL.M_LESS, 0, 255);
        }

        public override void Flip(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;

            if (direction == Direction.Horizontal)
                MIL.MimFlip(milSrcImage.Image, milDestImage.Image, MIL.M_FLIP_HORIZONTAL, MIL.M_DEFAULT);
            else
                MIL.MimFlip(milSrcImage.Image, milDestImage.Image, MIL.M_FLIP_VERTICAL, MIL.M_DEFAULT);
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, float angle)
        {
            MilGreyImage milSrcImage = srcImage as MilGreyImage;
            MilGreyImage milDestImage = destImage as MilGreyImage;
            
            MIL.MimRotate(milSrcImage.Image, milDestImage.Image, angle, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_BILINEAR);
        }

        public override void DrawPolygon(AlgoImage srcImage, float[] xArray, float[] yArray, Color color, bool filled)
        {
            MilImage.CheckGreyImage(srcImage, "MilImageProcessing.DrawPolygon", "Polygon");

            MilImage milSrcImage = srcImage as MilGreyImage;
            if (milSrcImage == null)
                milSrcImage = srcImage as MilColorImage;

            MIL_ID milGraphicsContext = MIL.M_NULL;
            MIL.MgraAlloc(MIL.M_DEFAULT_HOST, ref milGraphicsContext);
            MIL.MgraColor(milGraphicsContext, MIL.M_RGB888(color));

            double[] xDoubleArray = xArray.Cast<double>().ToArray();
            double[] yDoubleArray = yArray.Cast<double>().ToArray();

            int count = Math.Min(xDoubleArray.Length, yDoubleArray.Length);

            if (filled == true)
                MIL.MgraLines(milGraphicsContext, milSrcImage.Image, count, xDoubleArray, yDoubleArray, MIL.M_NULL, MIL.M_NULL, MIL.M_POLYGON + MIL.M_FILLED);
            else
                MIL.MgraLines(milGraphicsContext, milSrcImage.Image, count, xDoubleArray, yDoubleArray, MIL.M_NULL, MIL.M_NULL, MIL.M_POLYGON);

            MIL.MgraFree(milGraphicsContext);
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            MilBlobRectList milBlobRectList1 = (MilBlobRectList)blobRectList1;
            MilBlobRectList milBlobRectList2 = (MilBlobRectList)blobRectList2;

            MilBlobRectList blobRectList = new MilBlobRectList();
            MilBlobObject blobResult = blobRectList.BlobResult;
            
            if (blobResult.Id == MIL.M_NULL)
                throw new AllocFailedException("[MilImageProcessing.Blob]");

            MIL.MblobMerge(milBlobRectList1.BlobResult.Id, milBlobRectList2.BlobResult.Id, blobResult.Id, MIL.M_DEFAULT);

            FilterBlob(blobResult, blobRectList, blobParam);

            return blobRectList;
        }

        public override BlobRectList EreseBorderBlobs(BlobRectList blobRectList, BlobParam blobParam)
        {
            MilBlobRectList milBlobRectList = blobRectList as MilBlobRectList;
            MilBlobRectList tempBlobRectList = new MilBlobRectList(milBlobRectList.BlobResult);

            MIL.MblobSelect(milBlobRectList.BlobResult.Id, MIL.M_DELETE, MIL.M_BLOB_TOUCHING_IMAGE_BORDERS, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL);
            
            GetBlobBox(milBlobRectList.BlobResult, tempBlobRectList, blobParam);

            return tempBlobRectList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.UI;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;
using Emgu.CV.Cuda;
using Emgu.CV.Util;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvImageProcessing : ImageProcessing
    {
        //public Emgu.CV.Cuda.Stream Stream { get => this.stream;}
        Emgu.CV.Cuda.Stream stream = null;

        public override bool StreamExist => this.stream != null;

        public OpenCvImageProcessing()
        {
            try
            {
                this.stream = new Emgu.CV.Cuda.Stream();
            }
            catch ( Emgu.CV.Util.CvException ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }
        }

        ~OpenCvImageProcessing()
        {
            Dispose();
        }

        public override bool WaitStream()
        {
            if (this.stream == null)
                return false;

            if (this.stream.Ptr == IntPtr.Zero)
                return false;

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            this.stream.WaitForCompletion();
            //System.Diagnostics.Debug.WriteLine(string.Format("OpenCvImageProcessing.WaitStream - {0}[ms]", sw.Elapsed.TotalMilliseconds));
            return true;
        }

        public override void Dispose()
        {
            WaitStream();
            this.stream?.Dispose();
            this.stream = null;
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            const int HIST_NUM_INTENSITIES = 256;
            //MIL_INT[]
            OpenCvImage openCvImage = (OpenCvImage)algoImage;
            if (openCvImage.IsCudaImage)
            {
                OpenCvCudaImage openCvCudaImage = (OpenCvCudaImage)openCvImage;
                openCvCudaImage.UpdateHostImage();

                OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
                openCvGreyImage.Image = openCvCudaImage.HostImage;
                long[] d = this.Histogram(openCvGreyImage);
                openCvGreyImage.Dispose();

                return d;
                //GpuMat hist = new GpuMat(1, HIST_NUM_INTENSITIES, DepthType.Cv32S, 1);
                //CudaInvoke.HistEven(openCvImage.InputArray, hist, HIST_NUM_INTENSITIES, 0, 255);

                //Mat hist2 = new Mat(1, HIST_NUM_INTENSITIES, DepthType.Cv32S, 1);
                //hist.Download(hist2);
                //long[] a = hist2.Data.Cast<long>().ToArray();
                //return a;
            }
            else
            {
                //Mat hist = new Mat();
                //VectorOfMat vectorOfMat = new VectorOfMat();
                //vectorOfMat.Push(openCvImage.InputArray);

                //CvInvoke.CalcHist(vectorOfMat, new int[] { 0 }, null, hist, new int[] { HIST_NUM_INTENSITIES }, new float[] { 0, 255 }, false);
                DenseHistogram histo = new DenseHistogram(256, new RangeF(0, 256));

                if (algoImage is OpenCvGreyImage)
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);
                else if (algoImage is OpenCvColorImage)
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);
                else if (algoImage is OpenCvDepthImage)
                    histo.Calculate(new Image<Gray, byte>[] { ((OpenCvGreyImage)algoImage).Image }, false, null);

                float[] h = histo.GetBinValues();
                long[] l = Array.ConvertAll<float, long>(h, f => (long)f);
                return l;
            }
        }

        // Auto Threshold
        public override double Binarize(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            OpenCvImage openCvImageSrc = srcImage as OpenCvImage;
            OpenCvImage openCvImageDsr = destImage as OpenCvImage;
            ThresholdType thresholdType = (inverse ? ThresholdType.BinaryInv : ThresholdType.Binary);
            if (openCvImageDsr.IsCudaImage)
            {
                CudaInvoke.Threshold(openCvImageSrc.InputArray, openCvImageDsr.OutputArray, 127, 255, thresholdType, this.stream);
            }
            else 
            {
                thresholdType |= ThresholdType.Otsu;
                CvInvoke.Threshold(openCvImageSrc.InputArray, openCvImageDsr.OutputArray, 127, 255, thresholdType);
            }
        }

        // Single Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            //OpenCvGreyImage openCvSrcImage = srcImage as OpenCvGreyImage;
            //OpenCvGreyImage openCvDestImage = destImage as OpenCvGreyImage;
            //openCvDestImage.Image = openCvSrcImage.Image.ThresholdBinary(new Gray(threshold), new Gray(255));

            OpenCvImage openCvImage = srcImage as OpenCvImage;
            OpenCvImage openCvImageDst = destImage as OpenCvImage;

            if (openCvImageDst.IsCudaImage)
                CudaInvoke.Threshold(openCvImage.InputArray, openCvImageDst.OutputArray, threshold, 255, inverse ? ThresholdType.BinaryInv : ThresholdType.Binary, this.stream);
            else
                CvInvoke.Threshold(openCvImage.InputArray, openCvImageDst.OutputArray, threshold, 255, inverse ? ThresholdType.BinaryInv : ThresholdType.Binary);
        }

        // Double Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            OpenCvGreyImage openCvSrcImage = srcImage as OpenCvGreyImage;
            OpenCvGreyImage openCvDestImage = destImage as OpenCvGreyImage;

            Image<Gray, Byte> lowerThresholdImage = openCvSrcImage.Image.ThresholdBinary(new Gray(thresholdLower), new Gray(128));
            Image<Gray, Byte> upperThresholdImage = openCvSrcImage.Image.ThresholdBinary(new Gray(thresholdUpper), new Gray(128));

            openCvDestImage.Image = lowerThresholdImage + upperThresholdImage;
        }

        public override void Morphology(AlgoImage srcImage, AlgoImage destImage, MorphologyType type, int[,] kernalXY, int repeatNum, bool isGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, int kernelSize = 3, bool useGray = false)
        {
            OpenCvImage openCvSrcImage = srcImage as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            Mat kernel = new Mat(3, 3, DepthType.Cv8U, 1);
            kernel.SetTo(new MCvScalar(255));

            if (openCvDstImage.IsCudaImage)
            {
                using (CudaMorphologyFilter filter = new CudaMorphologyFilter(MorphOp.Erode, DepthType.Cv8U, 1, kernel, new Point(-1, -1), numErode))
                    filter.Apply(openCvSrcImage.InputArray, openCvDstImage.OutputArray, this.stream);
            }
            else
            {
                CvInvoke.MorphologyEx(openCvSrcImage.InputArray, openCvDstImage.OutputArray,
                    MorphOp.Erode, kernel, new Point(-1, -1), numErode, BorderType.Default, new MCvScalar(0));
            }
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, int kernelSize = 3, bool useGray = false)
        {
            OpenCvImage openCvSrcImage = srcImage as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            Mat kernel = new Mat(kernelSize, kernelSize, DepthType.Cv8U, 1);
            kernel.SetTo(new MCvScalar(255));

            if (openCvDstImage.IsCudaImage)
            {
                using (CudaMorphologyFilter filter = new CudaMorphologyFilter(MorphOp.Dilate, DepthType.Cv8U, 1, kernel, new Point(-1, -1), numDilate))
                    filter.Apply(openCvSrcImage.InputArray, openCvDstImage.OutputArray, this.stream);
            }
            else
            {
                CvInvoke.MorphologyEx(openCvSrcImage.InputArray, openCvDstImage.OutputArray,
                    MorphOp.Dilate, kernel, new Point(-1, -1), numDilate, BorderType.Default, new MCvScalar(0));
            }
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray)
        {
            OpenCvImage openCvSrcImage = srcImage as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            Mat kernel = new Mat(3, 3, DepthType.Cv8U, 1);
            kernel.SetTo(new MCvScalar(255));

            if (openCvDstImage.IsCudaImage)
            {
                using (CudaMorphologyFilter filter = new CudaMorphologyFilter(MorphOp.Open, DepthType.Cv8U, 1, kernel, new Point(-1, -1), numOpen))
                    filter.Apply(openCvSrcImage.InputArray, openCvDstImage.OutputArray, this.stream);
            }
            else
            {
                CvInvoke.MorphologyEx(openCvSrcImage.InputArray, openCvDstImage.OutputArray,
                    MorphOp.Open, kernel, new Point(-1, -1), numOpen, BorderType.Default, new MCvScalar(0));
            }
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray)
        {
            OpenCvImage openCvSrcImage = srcImage as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            Mat kernel = new Mat(3, 3, DepthType.Cv8U, 1);
            kernel.SetTo(new MCvScalar(255));

            if (openCvDstImage.IsCudaImage)
            {
                using (CudaMorphologyFilter filter = new CudaMorphologyFilter(MorphOp.Close, DepthType.Cv8U, 1, kernel, new Point(-1, -1), numClose))
                    filter.Apply(openCvSrcImage.InputArray, openCvDstImage.OutputArray, this.stream);
            }
            else
            {
                CvInvoke.MorphologyEx(openCvSrcImage.InputArray, openCvDstImage.OutputArray,
                    MorphOp.Close, kernel, new Point(-1, -1), numClose, BorderType.Default, new MCvScalar(0));
            }
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void Count(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            if (algoImage is OpenCvGreyImage)
            {
                OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;

                numBlackPixel = 0;
                numWhitePixel = 0;
                numGreyPixel = 0;
                //Image<Gray, Byte> greyImage = openCvGreyImage.Image.InRange(new Gray(120), new Gray(140));
                foreach (byte value in openCvGreyImage.Image.Bytes)
                {
                    if (value == 0)
                        numBlackPixel++;
                    else if (value == 255)
                        numWhitePixel++;
                    else
                        numGreyPixel++;
                }
//                numGreyPixel = algoImage.CountNonzero()[0];
//                numWhitePixel = openCvGreyImage.Image.CountNonzero()[0] - numGreyPixel;
//#if DEBUG
//                string path = string.Format("{0}\\{1}.bmp", Configuration.TempFolder, "OpenCV");
//                openCvGreyImage.Image.Save(path);
//#endif

//                numBlackPixel = (greyImage.Width * greyImage.Height) - numWhitePixel - numGreyPixel;
            }
            else
            {
                OpenCvDepthImage openCvDepthImage = algoImage as OpenCvDepthImage;

                Image<Gray, byte> greyImage = openCvDepthImage.Image.InRange(new Gray(120), new Gray(140));
                numGreyPixel = greyImage.CountNonzero()[0];

                numWhitePixel = openCvDepthImage.Image.CountNonzero()[0] - numGreyPixel;

                numBlackPixel = (greyImage.Width * greyImage.Height) - numWhitePixel - numGreyPixel;
            }
        }

        private double length(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public override void Count(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            numWhitePixel = numGreyPixel = numBlackPixel = 0;

            Byte[,,] imageData = openCvGreyImage.Image.Data;
            Byte[,,] maskData = openCvMaskImage.Image.Data;
            int width = openCvGreyImage.Image.Width;
            int height = openCvGreyImage.Image.Height;

            int halfWidth = width / 2;
            int centerX = width / 2;
            int centerY = height / 2;

            double doubleWhitePixel = 0;
            double doubleGreyPixel = 0;
            double doubleBlackPixel = 0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (maskData[y, x, 0] > 0)
                    {
                        double weight = 1; // (halfWidth - length(x, y, centerX, centerY)) / halfWidth;
                        if (imageData[y, x, 0] > 200)
                            doubleWhitePixel += weight;
                        else if (imageData[y, x, 0] > 100)
                            doubleGreyPixel += weight;
                        else
                            doubleBlackPixel += weight;
                    }
                }
            }

            numBlackPixel = (int)doubleBlackPixel;
            numGreyPixel = (int)doubleGreyPixel;
            numWhitePixel = (int)doubleWhitePixel;
        }

        public override void Count2(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            Image<Gray, Byte> maskedImage = openCvGreyImage.Image.And(openCvMaskImage.Image);
            maskedImage.Save(Path.Combine(Configuration.TempFolder, "MaskedImage.bmp"));

            Count2(maskedImage, out numBlackPixel, out numGreyPixel, out numWhitePixel);

            numBlackPixel = openCvMaskImage.Image.CountNonzero()[0] - numWhitePixel;
        }

        public override void Count2(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
            Count2(openCvGreyImage.Image, out numBlackPixel, out numGreyPixel, out numWhitePixel);
        }

        private void Count2(Image<Gray, Byte> image, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            CvBlobs blobs = new CvBlobs();
            CvBlobDetector blobDetector = new CvBlobDetector();
            blobDetector.Detect(image, blobs);
            Image<Bgr, byte> blobRenderImage = blobDetector.DrawBlobs(image, blobs, CvBlobDetector.BlobRenderType.BoundingBox, 0.5);
            blobRenderImage.Save(Path.Combine(Configuration.TempFolder, "TestBlob.bmp"));

            numWhitePixel = numBlackPixel = numGreyPixel = 0;

            foreach (KeyValuePair<uint, CvBlob> blob in blobs)
            {
                //MemStorage storage = new MemStorage();

                List<Point> conturePointList = blob.Value.GetContour().ToList();
                Rectangle minRect = Rectangle.FromLTRB(
                    conturePointList.Min(f => f.X), conturePointList.Min(f => f.Y),
                    conturePointList.Max(f => f.X), conturePointList.Max(f => f.Y)
                    );
                SizeF blobSize = minRect.Size;
                float compactness = (blobSize.Width > blobSize.Height ? blobSize.Width / blobSize.Height : blobSize.Height / blobSize.Width);

                if (compactness < 2 && (((blob.Value.BoundingBox.Width < (image.Width * 0.3)) && (blob.Value.BoundingBox.Height < (image.Height * 0.3)))))
                {
                    numWhitePixel += blob.Value.Area;
                }
            }

            numBlackPixel = image.Width * image.Height - numWhitePixel;
//            Count(algoImage, out numBlackPixel, out numGreyPixel, out numWhitePixel);
        }

        public override Color GetColorAverage(AlgoImage algoImage, Rectangle rect)
        {
            OpenCvColorImage openCvColorImage = algoImage as OpenCvColorImage;
            openCvColorImage.Image.ROI = rect;
            Bgr bgrColor = openCvColorImage.Image.GetAverage();
            openCvColorImage.Image.ROI = Rectangle.Empty;

            return Color.FromArgb((int)bgrColor.Red, (int)bgrColor.Green, (int)bgrColor.Blue);
        }


        public override Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            OpenCvColorImage openCvColorImage = algoImage as OpenCvColorImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            Bgr bgrColor = openCvColorImage.Image.GetAverage(openCvMaskImage.Image);

            return Color.FromArgb((int)bgrColor.Red, (int)bgrColor.Green, (int)bgrColor.Blue);
        }

        public override float GetGreyAverage(AlgoImage algoImage)
        {
            OpenCvImage openCvImage = algoImage as OpenCvImage;

            MCvScalar mean = new MCvScalar(0);
            if (openCvImage.IsCudaImage)
            {
                MCvScalar stdDev = new MCvScalar(0);
                CudaInvoke.MeanStdDev(openCvImage.InputArray, ref mean, ref stdDev);
            }
            else
            {
                mean = CvInvoke.Mean(openCvImage.InputArray);
            }
            return (float)mean.V0;
            //if (algoImage is OpenCvDepthImage)
            //{
            //    OpenCvDepthImage openCvDepthImage = algoImage as OpenCvDepthImage;
            //    return (float)openCvDepthImage.Image.GetAverage().Intensity;
            //}
            //else
            //{
            //    OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
            //    return (float)openCvGreyImage.Image.GetAverage().Intensity;
            //}
        }

        public override float GetGreyAverage(AlgoImage greyImage, AlgoImage maskImage)
        {
            OpenCvGreyImage openCvGreyImage = greyImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            return (float)openCvGreyImage.Image.GetAverage(openCvMaskImage.Image).Intensity;
        }

        public override float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage)
        {
            OpenCvImage openCvImage = algoImage as OpenCvImage;
            OpenCvImage openCvMaskImage = maskImage as OpenCvImage;

            double minVal = 0, maxVal = 0;
            Point minLoc = Point.Empty, maxLoc = Point.Empty;
            if (openCvImage.IsCudaImage)
            {
                CudaInvoke.MinMaxLoc(openCvImage.InputArray, ref minVal, ref maxVal, ref minLoc, ref maxLoc, openCvMaskImage?.InputArray);
            }
            else
            {
                CvInvoke.MinMaxLoc(openCvImage.InputArray, ref minVal, ref maxVal, ref minLoc, ref maxLoc, openCvMaskImage?.InputArray);
            }
            return (float)maxVal;
        }

        public override float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage)
        {
            OpenCvImage openCvImage = algoImage as OpenCvImage;
            OpenCvImage openCvMaskImage = maskImage as OpenCvImage;

            double minVal = 0, maxVal = 0;
            Point minLoc = Point.Empty, maxLoc = Point.Empty;
            if (openCvImage.IsCudaImage)
            {
                CudaInvoke.MinMaxLoc(openCvImage.InputArray, ref minVal, ref maxVal, ref minLoc, ref maxLoc, openCvMaskImage?.InputArray);
            }
            else
            {
                CvInvoke.MinMaxLoc(openCvImage.InputArray, ref minVal, ref maxVal, ref minLoc, ref maxLoc, openCvMaskImage?.InputArray);
            }
            return (float)minVal;
        }

        public override float GetStdDev(AlgoImage algoImage)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;

            Byte[,,] imageData = openCvGreyImage.Image.Data;
            int width = openCvGreyImage.Image.Width;
            int height = openCvGreyImage.Image.Height;
            int size = width * height;

            float sumValue = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sumValue += imageData[y, x, 0];
                }
            }


            float avgValue = sumValue / size;

            double doubleSumValue = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    doubleSumValue += Math.Pow(avgValue - imageData[y, x, 0], 2);
                }
            }

            doubleSumValue /= size;

            return (float)Math.Sqrt(doubleSumValue);
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            Byte[,,] imageData = openCvGreyImage.Image.Data;
            Byte[,,] maskImageData = openCvMaskImage.Image.Data;
            int width = openCvGreyImage.Image.Width;
            int height = openCvGreyImage.Image.Height;
            int size = width * height;

            float sumValue = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (maskImageData[y, x, 0] > 0)
                        sumValue += imageData[y, x, 0];
                }
            }


            float avgValue = sumValue / size;

            double doubleSumValue = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (maskImageData[y, x, 0] > 0)
                        doubleSumValue += Math.Pow(avgValue - imageData[y, x, 0], 2);
                }
            }

            doubleSumValue /= size;

            return (float)Math.Sqrt(doubleSumValue);
        }

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            MCvScalar[] scalar = new MCvScalar[2];
            Point[] point = new Point[2];
            OpenCvImage openCvImage = algoImage as OpenCvImage;
            StatResult statResult = new StatResult();

            CvInvoke.MeanStdDev(openCvImage.InputArray, ref scalar[0], ref scalar[1]);
            statResult.average = scalar[0].V0;
            statResult.stdDev = scalar[1].V0;

            CvInvoke.MinMaxLoc(openCvImage.InputArray, ref statResult.min, ref statResult.max, ref point[0], ref point[1]);

            statResult.squareSum = 0; // (float)CvInvoke.Sum(openCvImage.GetOutputArray()).V0;
            statResult.count = openCvImage.Width * openCvImage.Height;

            return statResult;
        }

        public override int Projection(AlgoImage greyImage, ref float[] projData, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage algoImage, AlgoImage maskImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, Direction projectionDir, ProjectionType projectionType)
        {
            int width = greyImage.Width;
            int height = greyImage.Height;
            int nbEntries;

            Mat reduceMat;
            ReduceDimension reduceDimension;
            if (projectionDir == Direction.Horizontal)
            {
                nbEntries = width;
                reduceDimension = ReduceDimension.SingleRow;
                reduceMat = new Mat(1, nbEntries, DepthType.Cv32F, 1);
            }
            else
            {
                nbEntries = height;
                reduceDimension = ReduceDimension.SingleCol;
                reduceMat = new Mat(1, nbEntries, DepthType.Cv32F, 1);
            }

            ReduceType reduceType = (projectionType == ProjectionType.Mean) ? ReduceType.ReduceAvg : ReduceType.ReduceSum;
            if (greyImage is OpenCvGreyImage)
            {
                OpenCvGreyImage openCvGreyImage = greyImage as OpenCvGreyImage;
                CvInvoke.Reduce(openCvGreyImage.Image, reduceMat, reduceDimension, reduceType, DepthType.Cv32F);
            }
            else if (greyImage is OpenCvCudaImage)
            {
                OpenCvCudaImage openCvCudaImage = greyImage as OpenCvCudaImage;
                CudaInvoke.Reduce(openCvCudaImage.Image, reduceMat, reduceDimension, reduceType, DepthType.Cv32F);
            }

            float[] projData = new float[nbEntries];
            System.Runtime.InteropServices.Marshal.Copy(reduceMat.DataPointer, projData, 0, projData.Length);
            //Buffer.BlockCopy(reduceMat.Data, 0, projData, 0, projData.Length*sizeof(float));
            reduceMat.Dispose();

            return projData;
        }

        public override void Sobel(AlgoImage algoImage, int size = 3)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;

            Image<Gray, float> sobelXImage = openCvGreyImage.Image.Sobel(1, 0, 3);
            Image<Gray, float> sobelYImage = openCvGreyImage.Image.Sobel(0, 1, 3);
            Image<Gray, float> sobelXImage2 = sobelXImage.Mul(sobelXImage);
            Image<Gray, float> sobelYImage2 = sobelYImage.Mul(sobelYImage);
            Image<Gray, float> sobelSum = sobelXImage2.Add(sobelYImage2);
            Image<Gray, float> sobel = sobelXImage2.Clone();

            CvInvoke.Sqrt(sobelSum, sobelSum);

            sobel.Save(String.Format("{0}\\Sobel.bmp", Configuration.TempFolder));

            openCvGreyImage.Image = sobel.ConvertScale<Byte>(1, 0);

            sobelXImage.Dispose();
            sobelYImage.Dispose();
            sobelXImage2.Dispose();
            sobelYImage2.Dispose();
            sobelSum.Dispose();
            sobel.Dispose();
        }

        public override void And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            //OpenCvGreyImage openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            //OpenCvGreyImage openCvGreyImage2 = algoImage2 as OpenCvGreyImage;
            //OpenCvGreyImage openCvGreyImageDst = destImage as OpenCvGreyImage;

            //openCvGreyImageDst.Image = openCvGreyImage1.Image.And(openCvGreyImage2.Image);

            OpenCvImage openCvImage1 = algoImage1 as OpenCvImage;
            OpenCvImage openCvImage2 = algoImage2 as OpenCvImage;
            OpenCvImage openCvImageDst = destImage as OpenCvImage;

            if (algoImage1 is OpenCvCudaImage)
                CudaInvoke.BitwiseAnd(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray,null, this.stream);
            else
                CvInvoke.BitwiseAnd(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray);
        }

        public override void Not(AlgoImage algoImage, AlgoImage destImage)
        {
            OpenCvGreyImage openCvGreyImage1 = algoImage as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImage2 = destImage as OpenCvGreyImage;

            openCvGreyImage2.Image = openCvGreyImage1.Image.Not();
        }

        public double MatchShape(AlgoImage algoImage1, AlgoImage algoImage2)
        {
            OpenCvGreyImage openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

            IInputArray ptrImage1 = (IInputArray)openCvGreyImage1.Image.GetInputArray();
            IInputArray ptrImage2 = (IInputArray)openCvGreyImage2.Image.GetInputArray();

            return CvInvoke.MatchShapes(ptrImage1, ptrImage2, ContoursMatchType.I3, 0);
        }

        public void AdaptiveThreshold(AlgoImage algoImage1, AlgoImage algoImage2, double param1)
        {
            OpenCvGreyImage openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

            IInputArray ptrImage1 = (IInputArray)openCvGreyImage1.Image.GetInputArray();
            IOutputArray ptrImage2 = (IOutputArray)openCvGreyImage2.Image.GetOutputArray();

            CvInvoke.AdaptiveThreshold(ptrImage1, ptrImage2, 255,  AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, param1);

            //openCvGreyImage2.Image.Ptr = ptrImage2;
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            OpenCvGreyImage openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImage2 = algoImage2 as OpenCvGreyImage;

//            openCvGreyImage2.Image = openCvGreyImage1.Image.GoodFeaturesToTrack(Canny(dblParams[0], dblParams[1]);

            return 0;
        }

        public override void Canny(AlgoImage greyImage, double threshold, double thresholdLinking)
        {
            OpenCvGreyImage openCvGreyImage = greyImage as OpenCvGreyImage;
            openCvGreyImage.Image = openCvGreyImage.Image.Canny(threshold, thresholdLinking);
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            OpenCvGreyImage openCvGreyImage = greyImage as OpenCvGreyImage;
            
            PointF centerPt = new PointF(openCvGreyImage.Image.Width/2, openCvGreyImage.Image.Height/2);

            openCvGreyImage.Image = openCvGreyImage.Image.LogPolar(centerPt, 15, Inter.Linear, Warp.FillOutliers);
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask = null)
        {
            if (algoImage is OpenCvCudaImage)
            {
                BlobRectList blobRectList = new BlobRectList();
                return blobRectList;
            }
            else
            {
                BlobRectList blobRectList = new BlobRectList();
                OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;
                CvBlobs blobs = new CvBlobs();
                CvBlobDetector blobDetector = new CvBlobDetector();
                try
                {
                    blobDetector.Detect(openCvGreyImage.Image, blobs);
                }catch(OutOfMemoryException ex)
                {

                }

                foreach (CvBlob blob in blobs.Values.ToList())
                {
                    BlobRect blobRect = new BlobRect();

                    blobRect.Area = blob.Area;
                    blobRect.BoundingRect = blob.BoundingBox;
                    blobRect.CenterPt = blob.Centroid;
                    blobRect.CalcCenterOffset();
                    blobRectList.Append(blobRect);
                }

                return blobRectList;
            }
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect blobRect, DrawBlobOption drawBlobOption)
        {
            OpenCvGreyImage openCvGreyImage = algoImage as OpenCvGreyImage;

            if (blobRect == null)
            {
                List<BlobRect> list = blobRectList.GetList();
                foreach (BlobRect rect in list)
                {
                    CvInvoke.Rectangle(openCvGreyImage.Image,
              Rectangle.FromLTRB(
                  (int)rect.BoundingRect.X,
                  (int)rect.BoundingRect.Y,
                  (int)rect.BoundingRect.X + (int)rect.BoundingRect.Width,
                  (int)rect.BoundingRect.Y + (int)rect.BoundingRect.Height),
              new MCvScalar(127), 1, LineType.EightConnected, 0);
                }
            }
            else
            {
                CvInvoke.Rectangle(openCvGreyImage.Image,
                    Rectangle.FromLTRB(
                        (int)blobRect.BoundingRect.X,
                        (int)blobRect.BoundingRect.Y,
                        (int)blobRect.BoundingRect.X + (int)blobRect.BoundingRect.Width,
                        (int)blobRect.BoundingRect.Y + (int)blobRect.BoundingRect.Height),
                    new MCvScalar(127), 1, LineType.EightConnected, 0);
            }
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            OpenCvGreyImage openCvGreyImage = greyImage as OpenCvGreyImage;
            OpenCvGreyImage openCvMaskImage = maskImage as OpenCvGreyImage;

            Byte[, ,] imageData = openCvGreyImage.Image.Data;
            Byte[, ,] maskData = null;
            if (maskImage != null)
                maskData = openCvMaskImage.Image.Data;
            int width = openCvGreyImage.Image.Width;
            int height = openCvGreyImage.Image.Height;

            avgX = new float[width];
            avgY = new float[height];
            stdDevX = new float[width];
            stdDevY = new float[height];

            float[] squareSumX = new float[width];
            float[] squareSumY = new float[height];
            float[] sumX = new float[width];
            float[] sumY = new float[height];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (maskData == null || maskData[y, x, 0] > 0)
                    {
                        squareSumX[x] += imageData[y, x, 0] * imageData[y, x, 0];
                        squareSumY[y] += imageData[y, x, 0] * imageData[y, x, 0];
                        sumX[x] += imageData[y, x, 0];
                        sumY[y] += imageData[y, x, 0];
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                avgX[x] = sumX[x] / height;
                stdDevX[x] = (float)Math.Sqrt(squareSumX[x] / height - avgX[x] * avgX[x]);
            }

            for (int y = 0; y < height; y++)
            {
                avgY[y] = sumY[y] / width;
                stdDevY[y] = (float)Math.Sqrt(squareSumY[y] / width - avgY[y] * avgY[y]);
            }
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage)
        {
            if (srcImage is OpenCvGreyImage)
            {
                OpenCvGreyImage openCvGreyImageSrc = srcImage as OpenCvGreyImage;
                OpenCvGreyImage openCvGreyImageDst = destImage as OpenCvGreyImage;

                openCvGreyImageDst.Image = openCvGreyImageSrc.Image.SmoothBlur(3, 3);
            }
            else if(srcImage is OpenCvDepthImage)
            {
                OpenCvDepthImage openCvGreyImageSrc = srcImage as OpenCvDepthImage;
                OpenCvDepthImage openCvGreyImageDst = destImage as OpenCvDepthImage;

                openCvGreyImageDst.Image = openCvGreyImageSrc.Image.SmoothBlur(7, 7);
            }
            else if(srcImage is OpenCvCudaImage)
            {
                OpenCvCudaImage openCvCudaImageSrc = srcImage as OpenCvCudaImage;
                OpenCvCudaImage openCvCudaImageDst = destImage as OpenCvCudaImage;

                using (CudaBoxFilter filter = new CudaBoxFilter(DepthType.Cv8U, 1, DepthType.Cv8U, 1, new Size(3, 3), new Point(-1, -1)))
                {
                    filter.Apply(openCvCudaImageSrc.InputArray, openCvCudaImageDst.OutputArray, this.stream);
                }
            }
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            OpenCvImage openCvImage = algoImage as OpenCvImage;

            if (openCvImage is OpenCvCudaImage)
            {
                CudaInvoke.EqualizeHist(openCvImage.InputArray, openCvImage.OutputArray, this.stream);
            }
            else
            {
                CvInvoke.EqualizeHist(openCvImage.InputArray, openCvImage.OutputArray);
                //CvInvoke.CLAHE(openCvImage.GetInputArray(), 2, new Size(8, 8), openCvImage.GetOutputArray());
                
            }

        }

        public override void FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
        
        }

        public override void WeightedAdd(AlgoImage[] srcImages, AlgoImage dstImage)
        {
            double scale = 1.0 / srcImages.Length;

            int idx = Array.FindIndex(srcImages, f => f.Equals(dstImage));
            if(idx>0)
            {
                AlgoImage temp = srcImages[0];
                srcImages[0] = srcImages[idx];
                srcImages[idx] = temp;
            }

            System.Diagnostics.Debug.Assert(Array.Exists(srcImages,f=>f.Equals(dstImage))==false || srcImages[0].Equals(dstImage));
            if (dstImage is OpenCvCudaImage)
            {
                OpenCvCudaImage[] openCvSrcImages = Array.ConvertAll(srcImages, f => (f as OpenCvCudaImage));
                OpenCvCudaImage openCvDstImage = dstImage as OpenCvCudaImage;

                if (srcImages.Length == 1)
                {
                    dstImage.Copy(srcImages[0]);
                }
                else if (srcImages.Length > 1)
                {
                    //srcImages[0].Save(@"C:\temp\srcImages[0].bmp");
                    //srcImages[1].Save(@"C:\temp\srcImages[1].bmp");
                    //dstImage.Save(@"C:\temp\dstImage.bmp");
                    CudaInvoke.AddWeighted(openCvSrcImages[0].Image, scale, openCvSrcImages[1].Image, scale, 0, openCvDstImage.Image, DepthType.Default, this.stream);
                    //this.stream.WaitForCompletion();
                    //dstImage.Save(@"C:\temp\dstImage.bmp");
                    for (int i = 2; i < srcImages.Length; i++)
                        CudaInvoke.AddWeighted(openCvDstImage.Image, 1, openCvSrcImages[i].Image, scale, 0, openCvDstImage.Image, DepthType.Default, this.stream);
                }
            }
            else if (dstImage is OpenCvGreyImage)
            {
                OpenCvGreyImage[] openCvSrcImages = Array.ConvertAll(srcImages, f => (f as OpenCvGreyImage));
                OpenCvGreyImage openCvDstImage = dstImage as OpenCvGreyImage;

                Array.ForEach(openCvSrcImages, f =>
                {
                    CvInvoke.AddWeighted(f.Image, scale, openCvDstImage.Image, 1, 0, openCvDstImage.Image);
                });
                openCvDstImage.Save(@"d:\temp\tt.bmp");
            }
        }

        public override void Add(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            OpenCvImage openCvImage1 = image1 as OpenCvImage;
            OpenCvImage openCvImage2 = image2 as OpenCvImage;
            OpenCvImage openCvImageDst = destImage as OpenCvImage;

            if (image1 is OpenCvCudaImage)
            {
                CudaInvoke.Add(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray,null, DepthType.Default, this.stream);
            }
            else
            {
                CvInvoke.Add(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray); 
            }
        }

        public override void Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool isAbs = false)
        {
            OpenCvImage openCvImage1 = image1 as OpenCvImage;
            OpenCvImage openCvImage2 = image2 as OpenCvImage;
            OpenCvImage openCvImageDst = destImage as OpenCvImage;

            //openCvImage1.Save(@"C:\temp\openCvImage1.bmp");
            //openCvImage2.Save(@"C:\temp\openCvImage2.bmp");
            if (destImage is OpenCvCudaImage)
            {
                if (isAbs)
                { CudaInvoke.Absdiff(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray, this.stream); }
                else
                { CudaInvoke.Subtract(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray, null, DepthType.Default, this.stream); }
            }
            else
            {
                if (isAbs)
                { CvInvoke.AbsDiff(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray); }
                else
                { CvInvoke.Subtract(openCvImage1.InputArray, openCvImage2.InputArray, openCvImageDst.OutputArray); }
            }
            //openCvImageDst.Save(@"C:\temp\openCvImageDst.bmp");
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            if (srcImage is OpenCvGreyImage)
            {
                OpenCvGreyImage openCvGreyImage = srcImage as OpenCvGreyImage;

                if (destImage != null)
                    ((OpenCvGreyImage)destImage).Image = openCvGreyImage.Image.SmoothMedian(size);
                else
                    openCvGreyImage.Image = openCvGreyImage.Image.SmoothMedian(size);
            }
            else
            {
                OpenCvDepthImage openCvDepthImage = srcImage as OpenCvDepthImage;

                if (destImage != null)
                    ((OpenCvDepthImage)destImage).Image = openCvDepthImage.Image.SmoothMedian(size);
                else
                    openCvDepthImage.Image = openCvDepthImage.Image.SmoothMedian(5);
            }
        }

        public void Median33F(AlgoImage srcImage, AlgoImage destImage)
        {
            int W = srcImage.Width;
            int H = srcImage.Height;
            int i, j, m, n;

            OpenCvDepthImage openCvSrcDepthImage = srcImage as OpenCvDepthImage;
            OpenCvDepthImage openCvDestDepthImage = destImage as OpenCvDepthImage;

            float[,,] srcArray = openCvSrcDepthImage.Image.Data;
            float[,,] destArray = openCvDestDepthImage.Image.Data;

            float[] t = new float[9];
            float ftemp;

            int j0, j1, j2;

            for (j = 1; j < H - 1; j++)
            {
                for (i = 1; i < W - 1; i++)
                {
                    j0 = W * (j - 1);
                    j1 = W * (j + 0);
                    j2 = W * (j + 1);

                    if (srcArray[j1, i, 0] == 0)
                        continue;

                    t[0] = srcArray[j0, i - 1, 0]; t[1] = srcArray[j0, i, 0]; t[2] = srcArray[j0, i + 1, 0];
                    t[3] = srcArray[j1, i - 1, 0]; t[4] = srcArray[j1, i, 0]; t[5] = srcArray[j1, i + 1, 0];
                    t[6] = srcArray[j2, i - 1, 0]; t[7] = srcArray[j2, i, 0]; t[8] = srcArray[j2, i + 1, 0];

                    if (srcArray[j0, i - 1, 0] == 0) t[0] = t[4];
                    if (srcArray[j0, i + 0, 0] == 0) t[1] = t[4];
                    if (srcArray[j0, i + 1, 0] == 0) t[2] = t[4];
                    if (srcArray[j1, i - 1, 0] == 0) t[3] = t[4];

                    if (srcArray[j1, i + 1, 0] == 0) t[5] = t[4];
                    if (srcArray[j2, i - 1, 0] == 0) t[6] = t[4];
                    if (srcArray[j2, i + 0, 0] == 0) t[7] = t[4];
                    if (srcArray[j2, i + 1, 0] == 0) t[8] = t[4];

                    for (n = 8; n >= 4; n--)
                    {
                        for (m = 0; m < n; m++)
                        {
                            if (t[m] > t[m + 1])
                            {
                                ftemp = t[m];
                                t[m] = t[m + 1];
                                t[m + 1] = ftemp;
                            }
                        }
                    }
                    if (srcArray[j1, i, 0] != 0)
                        destArray[j1, i, 0] = t[4];
                }
            }

            openCvDestDepthImage.Image.Data = destArray;
        }

        public override void Clear(AlgoImage srcImage, byte value)
        {
            MCvScalar mCvScalar = new MCvScalar(value, value, value);
            OpenCvImage openCvImage = srcImage as OpenCvImage;
            if (openCvImage.IsCudaImage)
            {
                ((OpenCvCudaImage)openCvImage).Clear(value, this.stream);
            }
            else
            {
                openCvImage.Clear(value);
            }
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            AlgoImage subImage = srcImage.GetSubImage(rect);
            OpenCvImage openCvImage = subImage as OpenCvImage;
            if (openCvImage != null)
            {
                if (openCvImage.IsCudaImage)
                {
                    MCvScalar mCvScalar = new MCvScalar(value.B, value.G, value.R);
                    ((OpenCvCudaImage)openCvImage).Image.SetTo(mCvScalar, null, this.stream);
                }
                else if (openCvImage is OpenCvColorImage)
                {
                    Bgr bgr = new Bgr(value);
                    ((OpenCvColorImage)openCvImage).Image.SetValue(bgr);
                }
            }
            subImage.Dispose();
            //OpenCvColorImage openCvSrcImage = srcImage as OpenCvColorImage;
            //openCvSrcImage.Image.ROI = rect;
            //openCvSrcImage.Image.SetValue();
            //openCvSrcImage.Image.ROI = Rectangle.Empty;
        }

        public override void Clear(AlgoImage image, AlgoImage mask, Color value)
        {
            throw new NotImplementedException();
        }

        public override void Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            OpenCvGreyImage openCvGreyImage1 = algoImage1 as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImage2 = algoImage2 as OpenCvGreyImage;
            OpenCvGreyImage openCvGreyImageDst = destImage as OpenCvGreyImage;

            openCvGreyImageDst.Image = openCvGreyImage1.Image.Or(openCvGreyImage2.Image);
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            throw new NotImplementedException();
        }

        //int Mean33_Roi(RoiInfo roiInfo)
        //{
        //    int W = roiInfo.Region.Width;
        //    int H = roiInfo.Region.Height;
        //    int i, j;
        //    float fsum = 0;
        //    int index0, index1, index2;
        //    byte[] pNoise = roiInfo.AmplitudeBuffer;
        //    float[] pSrc = roiInfo.ZHeight;
        //    float[] pData = new float[W * H];
        //    int cnt = 0;
        //    pSrc.CopyTo(pData, 0);
        //    for (j = 1; j < H - 1; j++)
        //    {
        //        for (i = 1; i < W - 1; i++)
        //        {
        //            cnt = 0;
        //            index0 = (j - 1) * W + i;

        //            if (pNoise[index0] == 0) continue;

        //            if (pNoise[index0 - 1] > 0) { fsum = pData[index0 - 1]; cnt++; }
        //            if (pNoise[index0] > 0) { fsum += pData[index0]; cnt++; }
        //            if (pNoise[index0 + 1] > 0) { fsum += pData[index0 + 1]; cnt++; }

        //            index1 = j * W + i;
        //            if (pNoise[index1 - 1] > 0) { fsum += pData[index1 - 1]; cnt++; }
        //            if (pNoise[index1] > 0) { fsum += pData[index1]; cnt++; }
        //            if (pNoise[index1 + 1] > 0) { fsum += pData[index1 + 1]; cnt++; }


        //            index2 = (j + 1) * W + i;
        //            if (pNoise[index2 - 1] > 0) { fsum += pData[index2 - 1]; cnt++; }
        //            if (pNoise[index2] > 0) { fsum += pData[index2]; cnt++; }
        //            if (pNoise[index2 + 1] > 0) { fsum += pData[index2 + 1]; cnt++; }

        //            pSrc[index1] = fsum / cnt;
        //        }
        //    }
        //    return 0;
        //}

        public override void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled)
        {
            if (srcImage is OpenCvGreyImage)
            {
                OpenCvGreyImage openCvGreySrcImage = srcImage as OpenCvGreyImage;
                openCvGreySrcImage.Image.Draw(rectangle, new Gray(value), filled ? -1 : 2);
            }
            else if (srcImage is OpenCvColorImage)
            {
                int argb = (int)value;
                byte a = (byte)((argb >> 24 & 0xff));
                byte r = (byte)((argb >> 16) & 0xff);
                byte g = (byte)((argb >> 8) & 0xff);
                byte b = (byte)((argb >> 0) & 0xff);

                OpenCvColorImage openCvColorImage = srcImage as OpenCvColorImage;
                openCvColorImage.Image.Draw(rectangle, new Bgr(b, g, r), filled ? -1 : 2);
            }
            else if(srcImage is OpenCvCudaImage)
            {
                OpenCvCudaImage openCvCudaImage = srcImage as OpenCvCudaImage;
                MCvScalar mCvScalar = new MCvScalar(value);
                openCvCudaImage.Image.GetSubRect(rectangle).SetTo(mCvScalar,null, this.stream);
            }
        }

        public override void DrawRotateRact(AlgoImage srcImage, DynMvp.UI.RotatedRect rotateRect, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }
        public override void ResconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage)
        {
            throw new NotImplementedException();
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            throw new NotImplementedException();
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY)
        {
            OpenCvImage openCvSrcImage = srcImage as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            Size size = Size.Empty;
            if (scaleFactorX <= 0 || scaleFactorY <= 0)
            {
                scaleFactorX = 0;
                scaleFactorY = 0;
                size = destImage.Size;
            }

            if (srcImage is OpenCvCudaImage)
            { 
                CudaInvoke.Resize(openCvSrcImage.InputArray, openCvDstImage.OutputArray, size, scaleFactorX, scaleFactorY, Inter.Linear, this.stream);
            }
            else 
            {
                CvInvoke.Resize(openCvSrcImage.InputArray, openCvDstImage.OutputArray, size, scaleFactorX, scaleFactorY, Inter.Area);
            }
        }

        public override void Stitch(AlgoImage srcImage1, AlgoImage srcImage2, AlgoImage destImage, Direction direction)
        {
            OpenCvImage openCvSrcImage1 = srcImage1 as OpenCvImage;
            OpenCvImage openCvSrcImage2 = srcImage2 as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            int width = 0;
            int height = 0;
            switch (direction)
            {
                case Direction.Horizontal:
                    width = openCvSrcImage1.Width + openCvSrcImage2.Width;
                    height = Math.Max(openCvSrcImage1.Height, openCvSrcImage2.Height);
                    break;
                case Direction.Vertical:
                    width = Math.Max(openCvSrcImage1.Width, openCvSrcImage2.Width);
                    height = openCvSrcImage1.Height + openCvSrcImage2.Height;
                    break;
            }

            if (openCvSrcImage1 is OpenCvCudaImage)
            {
                OpenCvCudaImage openCvSrcCudaImage1 = srcImage1 as OpenCvCudaImage;
                OpenCvCudaImage openCvSrcCudaImage2 = srcImage2 as OpenCvCudaImage;
                OpenCvCudaImage openCvDstCudaImage = destImage as OpenCvCudaImage;

                OpenCvCudaImage[] srcImages = new OpenCvCudaImage[2] { openCvSrcCudaImage1, openCvSrcCudaImage2 };
                Rectangle copyWindowDstImage = Rectangle.Empty;
                for (int i = 0; i < 2; i++)
                {
                    OpenCvCudaImage srcImage = srcImages[i];

                    if (direction == Direction.Horizontal)
                        copyWindowDstImage = new Rectangle(copyWindowDstImage.Right, 0, srcImage.Width, height);
                    else if (direction == Direction.Vertical)
                        copyWindowDstImage = new Rectangle(0, copyWindowDstImage.Bottom, width, srcImage.Height);

                    OpenCvCudaImage subImage = (OpenCvCudaImage)openCvDstCudaImage.GetSubImage(copyWindowDstImage);
                    srcImage.Image.CopyTo(subImage.Image);
                    subImage.Dispose();
                }
            }
            else 
            {
                OpenCvGreyImage openCvSrcGrayImage1 = srcImage1 as OpenCvGreyImage;
                OpenCvGreyImage openCvSrcGrayImage2 = srcImage2 as OpenCvGreyImage;
                OpenCvGreyImage openCvDstGrayImage = destImage as OpenCvGreyImage;

                if (direction == Direction.Horizontal)
                    openCvDstGrayImage.Image.ROI = new Rectangle(0, 0, openCvSrcGrayImage1.Width, height);
                else if (direction == Direction.Vertical)
                    openCvDstGrayImage.Image.ROI = new Rectangle(0, 0, width, openCvSrcGrayImage1.Height);
                openCvSrcGrayImage1.Image.CopyTo(openCvDstGrayImage.Image);
                openCvDstGrayImage.Image.ROI = Rectangle.Empty;
                //openCvDstGrayImage.Save(@"d:\tt1.bmp", null);

                if (direction == Direction.Horizontal)
                    openCvDstGrayImage.Image.ROI = new Rectangle(0, openCvSrcGrayImage1.Width, openCvSrcGrayImage2.Width, height);
                else if (direction == Direction.Vertical)
                    openCvDstGrayImage.Image.ROI = new Rectangle(0, openCvSrcGrayImage1.Height, width, openCvSrcGrayImage2.Height);
                openCvSrcGrayImage2.Image.CopyTo(openCvDstGrayImage.Image);
                openCvDstGrayImage.Image.ROI = Rectangle.Empty;
                //openCvDstGrayImage.Save(@"d:\tt2.bmp", null);
            }
        }

        public override void Min(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            OpenCvImage openCvSrcImage1 = image1 as OpenCvImage;
            OpenCvImage openCvSrcImage2 = image2 as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            if (openCvDstImage is OpenCvCudaImage)
                CudaInvoke.Min(openCvSrcImage1.InputArray, openCvSrcImage2.InputArray, openCvDstImage.OutputArray, this.stream);
            else
                CvInvoke.Min(openCvSrcImage1.InputArray, openCvSrcImage2.InputArray, openCvDstImage.OutputArray);
        }

        public override void Max(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            OpenCvImage openCvSrcImage1 = image1 as OpenCvImage;
            OpenCvImage openCvSrcImage2 = image2 as OpenCvImage;
            OpenCvImage openCvDstImage = destImage as OpenCvImage;

            if (openCvDstImage is OpenCvCudaImage)
                CudaInvoke.Max(openCvSrcImage1.InputArray, openCvSrcImage2.InputArray, openCvDstImage.OutputArray, this.stream);
            else
                CvInvoke.Max(openCvSrcImage1.InputArray, openCvSrcImage2.InputArray, openCvDstImage.OutputArray);
        }

        public override float[,] FFT(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
        {
            OpenCvGreyImage openCvSrcGrayImage = srcImage as OpenCvGreyImage;
            OpenCvGreyImage openCvDstGrayImage = destImage as OpenCvGreyImage;

            //openCvSrcGrayImage.Save(@"D:\temp\Src.bmp");
            CvInvoke.AdaptiveThreshold(openCvSrcGrayImage.Image, openCvDstGrayImage.Image, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 7, 0);
            //openCvDstGrayImage.Save(@"D:\temp\Dst.bmp");
        }

        public override void Add(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            OpenCvGreyImage openCvSrcGrayImage = srcImage as OpenCvGreyImage;
            OpenCvGreyImage openCvDstGrayImage = dstImage as OpenCvGreyImage;

            openCvDstGrayImage.Image = openCvSrcGrayImage.Image.Add(new Gray(value));
        }

        public override void Subtract(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            OpenCvGreyImage openCvSrcGrayImage = srcImage as OpenCvGreyImage;
            OpenCvGreyImage openCvDstGrayImage = dstImage as OpenCvGreyImage;

            openCvDstGrayImage.Image = openCvSrcGrayImage.Image.SubR(new Gray(value));
        }

        public override void Mul(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            OpenCvGreyImage openCvSrcGrayImage = srcImage as OpenCvGreyImage;
            OpenCvGreyImage openCvDstGrayImage = dstImage as OpenCvGreyImage;

            openCvDstGrayImage.Image = openCvSrcGrayImage.Image.Mul(value);
        }

        public override void Div(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            throw new NotImplementedException();
        }

        public override void Compare(AlgoImage image1, AlgoImage image2, AlgoImage dstImage)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage)
        {
            if (srcImage is OpenCvCudaImage)
            {
                OpenCvCudaImage openCvImageSrc = srcImage as OpenCvCudaImage;
                OpenCvCudaImage openCvImageDst = destImage as OpenCvCudaImage;

                CudaImage<Gray, float> tempX = new CudaImage<Gray, float>(openCvImageDst.Image.Size);
                CudaImage<Gray, float> tempY = new CudaImage<Gray, float>(openCvImageDst.Image.Size);
                CudaImage<Gray, float> temp = new CudaImage<Gray, float>(openCvImageDst.Image.Size);

                using (CudaFilter filter = new CudaSobelFilter(openCvImageSrc.Image.Depth, openCvImageSrc.Image.NumberOfChannels, tempX.Depth, tempX.NumberOfChannels, 1, 0))
                    filter.Apply(openCvImageSrc.Image, tempX, this.stream);
                CudaInvoke.Abs(tempX, tempX, this.stream);

                using (CudaFilter filter = new CudaSobelFilter(openCvImageSrc.Image.Depth, openCvImageSrc.Image.NumberOfChannels, tempY.Depth, tempY.NumberOfChannels, 0, 1))
                    filter.Apply(openCvImageSrc.Image, tempY, this.stream);
                CudaInvoke.Abs(tempY, tempY, this.stream);
                
                CudaInvoke.AddWeighted(tempX, 0.5, tempY, 0.5, 0, temp, DepthType.Default, this.stream);
                openCvImageDst.Image = temp.Convert<Gray, byte>();

                temp.Dispose();
                tempY.Dispose();
                tempX.Dispose();
            }
            else
            {
                OpenCvImage openCvSrcGrayImage = srcImage as OpenCvImage;
                OpenCvImage openCvDstGrayImage = destImage as OpenCvImage;

                CvInvoke.Sobel(openCvSrcGrayImage.InputArray, openCvDstGrayImage.OutputArray, DepthType.Cv8U, 1, 1);
            }
            
            //openCvDstGrayImage.Image = openCvSrcGrayImage.Image.Sobel(value);
        }

        public override void Clipping(AlgoImage srcImage, int minValue, int maxValue)
        {
            throw new NotImplementedException();
        }

        public override void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void CustomEdge(AlgoImage srcImage, AlgoImage destImage, AlgoImage bufferX, AlgoImage bufferY, int length)
        {
            throw new NotImplementedException();
        }

        public override void Flip(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, float angle)
        {
            throw new NotImplementedException();
        }

        public override void DrawPolygon(AlgoImage srcImage, float[] xArray, float[] yArray, Color color, bool filled)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList EreseBorderBlobs(BlobRectList blobRectList, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void DrawLine(AlgoImage srcImage, PointF point1, PointF point2, double value)
        {
            throw new NotImplementedException();
        }
    }
}

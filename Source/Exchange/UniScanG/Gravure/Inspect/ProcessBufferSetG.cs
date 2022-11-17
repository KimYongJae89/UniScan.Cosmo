using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace UniScanG.Gravure.Inspect
{
    public abstract class ProcessBufferSetG : UniScanG.Inspect.ProcessBufferSet
    {
        public bool IsMultiLayer { get => isMultiLayer; }
        protected bool isMultiLayer = false;

        public float ScaleFactor { get => scaleFactor; }
        protected float scaleFactor = 1;

        public AlgoImage ScaledImage { get => scaledImage; }
        protected AlgoImage scaledImage = null;

        public AlgoImage CalculatorResult { get => calculatorResult; }
        protected AlgoImage calculatorResult = null;

        public AlgoImage DetectorInsp { get => detectorInsp; }
        protected AlgoImage detectorInsp = null;

        public AlgoImage DetectorBinal { get => detectorBinal; }
        protected AlgoImage detectorBinal = null;

        public ProcessBufferSetG(float scaleFactor, bool isMultiLayer, int width, int height) : base("", width, height)
        {
            this.isMultiLayer = isMultiLayer;
            this.scaleFactor = scaleFactor;
        }

        public abstract void PostCalculate();
        public abstract void PreCalculate(AlgoImage fullImage);
    }

    public class ProcessBufferSetG1 : ProcessBufferSetG
    {
        public AlgoImage CalculatorInsp { get => calculatorInsp; }
        protected AlgoImage calculatorInsp = null;

        public AlgoImage[] CalculatorTemp { get => calculatorTemp; }
        protected AlgoImage[] calculatorTemp = null;

        public ProcessBufferSetG1(float scaleFactor, bool isMultiLayer, int width, int height) : base(scaleFactor, isMultiLayer, width, height) { }

        public override void BuildBuffers()
        {
            ImagingLibrary calculatorLibType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).LibraryType;
            ImageType calculatorImgType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).ImageType;
            ImagingLibrary detectorLibType = AlgorithmBuilder.GetStrategy(Detector.TypeName).LibraryType;
            ImageType detectorImgType = AlgorithmBuilder.GetStrategy(Detector.TypeName).ImageType;
            bool isHeterogenous = (calculatorLibType != detectorLibType) || (calculatorImgType != detectorImgType);

            int bufferDepth = this.isMultiLayer ? 4 : 1;

            width = (int)(width * scaleFactor);
            height = (int)(height * scaleFactor);

            if (scaleFactor > 1 || isHeterogenous)
                bufferList.Add(calculatorInsp = ImageBuilder.Build(CalculatorBase.TypeName, width, height));

            if (scaleFactor < 1 && isHeterogenous)
                bufferList.Add(scaledImage = ImageBuilder.Build(Detector.TypeName, width, height));

            if (scaleFactor > 1 || isHeterogenous)
            {
                bufferList.Add(scaledImage = ImageBuilder.Build(detectorLibType, ImageType.Grey, width, height));
                bufferList.Add(calculatorInsp = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            }

            calculatorTemp = new AlgoImage[bufferDepth];
            for (int i = 0; i < bufferDepth; i++)
                bufferList.Add(calculatorTemp[i] = ImageBuilder.Build(CalculatorBase.TypeName, width, height));

            bufferList.Add(calculatorResult = ImageBuilder.Build(Detector.TypeName, width, height));

            if (isHeterogenous)
                bufferList.Add(detectorInsp = ImageBuilder.Build(Detector.TypeName, width, height));

            bufferList.Add(detectorBinal = ImageBuilder.Build(Detector.TypeName, width, height));

            //imageProcessing = new ImageProcessing[0];
        }
        
        public override void PreCalculate(AlgoImage fullImage)
        {
            Vision.AlgorithmCommon.GetInspImage(fullImage, this.scaledImage, this.calculatorInsp, this.scaleFactor);
        }

        public override void PostCalculate()
        {
            if (this.detectorInsp != null)
                DynMvp.Vision.ImageConverter.Convert(this.calculatorResult, this.detectorInsp);
        }
    }

    public class ProcessBufferSetG2 : ProcessBufferSetG
    {
        float previewScale = 0.1f;

        public AlgoImage PreviewBuffer { get => previewBuffer; }
        protected AlgoImage previewBuffer = null;

        ManualResetEvent isWorkDone = new ManualResetEvent(true);
        Task bitmapBuildTask = null;

        public override bool IsDone => isWorkDone.WaitOne(0);

        public ProcessBufferSetG2(float scaleFactor, float previewScale, bool isMultiLayer, int width, int height) : base(scaleFactor, isMultiLayer, width, height)
        {
            this.previewScale = previewScale;
        }

        public override void BuildBuffers()
        {
            ImagingLibrary calculatorLibType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).LibraryType;
            ImageType calculatorImgType = AlgorithmBuilder.GetStrategy(CalculatorBase.TypeName).ImageType;
            ImagingLibrary detectorLibType = AlgorithmBuilder.GetStrategy(Detector.TypeName).LibraryType;
            ImageType detectorImgType = AlgorithmBuilder.GetStrategy(Detector.TypeName).ImageType;
            bool isHeterogenous = (calculatorLibType != detectorLibType) || (calculatorImgType != detectorImgType);

            int bufferDepth = this.isMultiLayer ? 4 : 1;

            bufferList.Add(previewBuffer = ImageBuilder.Build(Detector.TypeName, (int)(width * previewScale), (int)(height * previewScale)));

            width = (int)(width * scaleFactor);
            height = (int)(height * scaleFactor);

            if (scaleFactor < 1 && isHeterogenous)
                bufferList.Add(scaledImage = ImageBuilder.Build(Detector.TypeName, width, height));
            bufferList.Add(calculatorResult = ImageBuilder.Build(Detector.TypeName, width, height));

            bufferList.Add(detectorBinal = ImageBuilder.Build(Detector.TypeName, width, height));
        }

        public override void PreCalculate(AlgoImage fullImage)
        {
            Vision.AlgorithmCommon.ScaleImage(fullImage, this.previewBuffer, this.previewScale);
            
            if (this.scaledImage != null)
                Vision.AlgorithmCommon.ScaleImage(fullImage, this.scaledImage, this.scaleFactor);
        }

        public void StartPreviewBitmapBuild(Size fullImageSize, CalculatorResult calculatorResult)
        {
            this.bitmapBuildTask = new Task(() =>
            {
                Rectangle previewBufferRect = new Rectangle(Point.Empty, this.previewBuffer.Size);
                Size roiSize = Size.Round(new SizeF(fullImageSize.Width * previewScale, fullImageSize.Height * previewScale));
                Rectangle roiRect = new Rectangle(Point.Empty, roiSize);
                roiRect.Intersect(previewBufferRect);

                AlgoImage previewChildBuffer = this.previewBuffer.Clip(roiRect);
                calculatorResult.PreviewImageD = BuildPreviewImageD(previewChildBuffer, null);
                previewChildBuffer.Dispose();

                //calculatorResult.PrevImage = BuildPreviewBitmap(previewBuffer, null);

                isWorkDone.Set();
            });

            isWorkDone.Reset();
            this.bitmapBuildTask.Start();
        }

        private ImageD BuildPreviewImageD(AlgoImage previewBuffer, DebugContext debugContext)
        {
            //return previewBuffer.ToImageD();

            int width = previewBuffer.Width;
            int height = previewBuffer.Height;
            int pitch = previewBuffer.Pitch;
            PixelFormat pixelFormat = previewBuffer.ImageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            int numBand = previewBuffer.ImageType == ImageType.Color ? 3 : 1;
            IntPtr imagePtr = previewBuffer.GetImagePtr();
            byte[] bytes = previewBuffer.GetByte();

            Image2D image2D = new Image2D(width, height, numBand, pitch, bytes);
            //image2D.ConvertFromDataPtr();
            return image2D;
        }

        private Bitmap BuildPreviewBitmap(AlgoImage previewBuffer, DebugContext debugContext)
        {
            int width = previewBuffer.Width;
            int height = previewBuffer.Height;
            int pitch = previewBuffer.Pitch;
            PixelFormat pixelFormat = previewBuffer.ImageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            IntPtr imagePtr = previewBuffer.GetImagePtr();
            Bitmap bitmap = new Bitmap(width, height, pitch, pixelFormat, imagePtr);

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            Bitmap previewBitmap = new Bitmap(stream);
            ColorPalette cp = previewBitmap.Palette;
            for (int i = 0; i < cp.Entries.Length; i++)
                cp.Entries[i] = Color.FromArgb(255, i, i, i);
            previewBitmap.Palette = cp;
            return previewBitmap;

            //{
            //    previewBitmap = previewBuffer.ToBitmap();
            //    Debug.WriteLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //}

            //{
            //    System.Windows.Media.Imaging.BitmapSource bitmapSource = resizeSheetImage.ToBitmapSource();
            //    sb.AppendLine(string.Format("ToBitmapSource: {0}", sw.Elapsed.TotalMilliseconds));
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
            //        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //        bmpBitmapEncoder.Save(stream);

            //        previewBitmap = new Bitmap(stream);
            //    }
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //    previewBitmap?.Save(@"C:\temp\previewBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //}

            //{
            //    ImageD previewImageD = resizeSheetImage.ToImageD();
            //    sb.AppendLine(string.Format("ToImageD: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewBitmap = previewImageD.ToBitmap();
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewImageD.Dispose();
            //}
        }

        public override void PostCalculate()
        {
            //if (this.detectorInsp != null)
            //    DynMvp.Vision.ImageConverter.Convert(this.calculatorResult, this.detectorInsp);
        }

        public override void WaitDone()
        {
            Stopwatch sw = Stopwatch.StartNew();
            this.isWorkDone.WaitOne();
            //Debug.WriteLine(string.Format("ProcessBufferSetG2::WaitDone {0}", sw.Elapsed.TotalMilliseconds));
        }
    }
}

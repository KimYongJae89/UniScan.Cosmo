using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DynMvp.Vision
{
    public enum ImageBandType
    {
        Luminance, Red, Green, Blue
    }

    public enum ImageFilterType
    {
        EdgeExtraction, AverageFilter, HistogramEqualization, Binarization, Erode, Dilate
    }

    public abstract class AlgoImage : IDisposable
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public object Tag = null;

        protected AlgoImage parentImage = null;
        public AlgoImage ParentImage
        {
            get { return parentImage; }
            set { parentImage = value; }
        }

        protected List<AlgoImage> subImageList = new List<AlgoImage>();
        public List<AlgoImage> SubImageList
        {
            get { return subImageList; }
        }
        
        private ImageType imageType;
        public ImageType ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }

        private ImagingLibrary libraryType;
        public ImagingLibrary LibraryType
        {
            get { return libraryType; }
            set { libraryType = value; }
        }

        //private byte[] byteData = null;

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public byte[] GetByte()
        {
            return CloneByte();
        }

        public void SetByte(byte[] data)
        {
            PutByte(data);
        }

        public bool HasChild
        {
            get { return this.subImageList.Count > 0; }
        }

        public void DisposeChild()
        {
            foreach (AlgoImage subImage in this.subImageList)
            {
                subImage.DisposeChild();
                subImage.Dispose();
            }
        }

        public virtual bool IsReady { get=>true; }
        public virtual void WaitReady() { }

        public abstract byte[] CloneByte();
        public abstract void PutByte(byte[] data);
        public abstract void PutByte(IntPtr ptr, int pitch);
        public abstract int Width { get; }
        public abstract void Dispose();
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public abstract int BitPerPixel { get; }
        public abstract AlgoImage Clone();
        public abstract ImageD ToImageD();
        public abstract AlgoImage Clip(Rectangle rectangle);
        public abstract void Save(string fileName);
        public void Save(DebugContext debugContext)
        {
            if (debugContext == null || debugContext.SaveDebugImage == false)
                return;

            Save(Path.Combine(debugContext.FullPath, this.name));
        }
        public void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext != null && debugContext.SaveDebugImage == false)
                return;

            string fullName = "";
            if (debugContext != null)
                fullName = Path.Combine(debugContext.FullPath, fileName);
            else
                fullName = fileName;

            Save(fullName);
        }

        public void Save(string fileName, SizeF scale, DebugContext debugContext)
        {
            if (debugContext != null && debugContext.SaveDebugImage == false)
                return ;

            ImageProcessing processing = AlgorithmBuilder.GetImageProcessing(this);
            int w = Math.Max((int)Math.Round(this.Width * scale.Width), 1);
            int h = Math.Max((int)Math.Round(this.Height * scale.Height), 1);
            AlgoImage resize = ImageBuilder.Build(this.libraryType, this.ImageType, w, h);
            processing.Resize(this, resize, -1, -1);
            resize.Save(fileName, debugContext);
            resize.Dispose();
        }
        public void Save(string fileName, float scale, DebugContext debugContext) {  Save(fileName, new SizeF(scale, scale), debugContext); }
        public abstract void Clear(byte initVal = 0);
        public abstract void Copy(AlgoImage srcImage, Rectangle srcRect);
        public abstract void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size);
        public void Copy(AlgoImage srcImage) { Copy(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height)); }
        protected abstract void GetSubImage(Rectangle rectangle, out AlgoImage dstImage);
        public AlgoImage GetSubImage(Rectangle rectangle)
        {
            //Rectangle adjestRect = Rectangle.Intersect(new Rectangle(Point.Empty, this.Size), rectangle);
            //if (rectangle != adjestRect)
            //    throw new ArgumentException("AlgoImage::GetSubImage - rectangle is invalid");

            AlgoImage subImage;
            GetSubImage(rectangle, out subImage);
            return subImage;
        }
        public abstract IntPtr GetImagePtr();

        List<ImageFilterType> filteredList = new List<ImageFilterType>();
        public List<ImageFilterType> FilteredList
        {
            get { return filteredList; }
            set { filteredList = value; }
        }

        public bool CheckFilterd(ImageFilterType imageFilterType)
        {
            return filteredList.Contains(imageFilterType);
        }

        public bool IsCompatible(string algorithmType)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            return IsCompatible(strategy);

        }

        public bool IsCompatible(AlgorithmStrategy strategy)
        {
            if (strategy == null)
                return false;

            return IsCompatible(strategy.LibraryType, strategy.ImageType);
        }

        public bool IsCompatible(AlgoImage algoImage)
        {
            return IsCompatible(algoImage.LibraryType, algoImage.ImageType);
        }

        public bool IsCompatible(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            return (this.LibraryType == imagingLibrary && this.ImageType == imageType);
        }

        public AlgoImage ConvertTo(AlgorithmStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException("[AlgoImage.ConvertTo] strategy is null");

            return ConvertTo(strategy.LibraryType, strategy.ImageType);
        }

        public AlgoImage ConvertTo(string algorithmType)
        {
            ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            ImageType imageType = ImageType.Grey;

            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            if (strategy != null)
            {
                libraryType = strategy.LibraryType;
                imageType = strategy.ImageType;
            }

            if(imageType == ImageType.Gpu)
                libraryType = ImagingLibrary.OpenCv;

            return ConvertTo(libraryType, imageType);
        }

        public AlgoImage ConvertTo(ImageType imageType)
        {
            return ConvertTo(this.libraryType, imageType);
        }

        public AlgoImage ConvertTo(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            return ImageConverter.Convert(this, imagingLibrary, imageType);

            //byte[] bytes = this.GetByte();

            //AlgoImage convertImage = null;
            //if (imageType == ImageType.Binary)
            //{
            //    convertImage = ImageBuilder.Build(imagingLibrary, imageType, this.Width, this.Height);
            //    if (imagingLibrary == ImagingLibrary.MatroxMIL)
            //    {
            //        AlgoImage milImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, this.Width, this.Height);
            //        milImage.PutByte(bytes);
            //        convertImage.Copy(milImage);
            //        milImage.Dispose();
            //    }
            //    else
            //        throw new NotImplementedException();
            //}
            //else
            //{
            //    int numBand = this.ImageType == ImageType.Color ? 3 : 1;
            //    Image2D image2D = new Image2D(this.Width, this.Height, numBand, this.Width, bytes);
            //    convertImage = ImageBuilder.Build(imagingLibrary, image2D, imageType);
            //}

            //return convertImage;
        }

        public Bitmap ToBitmap()
        {
            PixelFormat pixelFormat = this.imageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            Rectangle rectangle = new Rectangle(Point.Empty, this.Size);
            int length = Pitch * Height;
            IntPtr imagePtr = GetImagePtr();
            Bitmap bitmap = new Bitmap(Width, Height, Pitch, pixelFormat, imagePtr);

            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Bmp);
            memoryStream.Position = 0;
            Bitmap bitmap2 = new Bitmap(memoryStream);

            ColorPalette cp = bitmap2.Palette;
            for (int i = 0; i < cp.Entries.Length; i++)
                cp.Entries[i] = Color.FromArgb(255, i, i, i);
            bitmap2.Palette = cp;

            return bitmap2;
        }

        public BitmapSource ToBitmapSource()
        {
            BitmapSource bitmapSource = null;

            switch (this.imageType)
            {
                case ImageType.Grey:
                    bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, GetByte(), this.Pitch);
                    bitmapSource.Freeze();
                    break;
                case ImageType.Color:
                    bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Rgb24, null, GetByte(), this.Pitch);
                    bitmapSource.Freeze();
                    break;
            }

            return bitmapSource;
        }
    }
}

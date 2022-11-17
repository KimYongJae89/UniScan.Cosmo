using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Standard.DynMvp.Base;
using System.IO;

namespace Standard.DynMvp.Vision
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

        private AlgoImage parentImage = null;
        public AlgoImage ParentImage
        {
            get { return parentImage; }
            set { parentImage = value; }
        }

        private List<AlgoImage> subImageList = new List<AlgoImage>();
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

        private byte[] byteData = null;

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public byte[] GetByte()
        {
            if (byteData == null)
            {
                byteData = CloneByte();
            }
            return byteData;
        }

        public void SetByte(byte[] data)
        {
            byteData = data;
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

        public abstract byte[] CloneByte();
        public abstract void PutByte(byte[] data);
        public abstract int Width { get; }
        public abstract void Dispose();
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public abstract AlgoImage Clone();
        public abstract AlgoImage Clone(ImageType imageType);
        
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
            Rectangle adjestRect = Rectangle.Intersect(new Rectangle(Point.Empty, this.Size), rectangle);
            if (rectangle != adjestRect)
                throw new ArgumentException("AlgoImage::GetSubImage - rectangle is invalid");

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

        public AlgoImage ChangeImageType(string algorithmType)
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

            return ChangeImageType(libraryType, imageType);
        }

        public AlgoImage ChangeImageType(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            if (this.LibraryType == imagingLibrary && this.ImageType == imageType)
                return this.Clone();

            switch (imagingLibrary)
            {
                case ImagingLibrary.OpenCv:
                    return ConvertToOpenCvImage(imageType);
                case ImagingLibrary.EuresysOpenEVision:
                    return ConvertToEuresysImage(imageType);
                case ImagingLibrary.CognexVisionPro:
                    return ConvertToCognexImage(imageType);
                case ImagingLibrary.MatroxMIL:
                    return ConvertToMilImage(imageType);
                case ImagingLibrary.Halcon:
                    return ConvertToHalconImage(imageType);
                case ImagingLibrary.Custom:
                    return ConvertToCustomImage(imageType);
                default:
                    throw new InvalidTypeException();
            }
        }

        public virtual AlgoImage ConvertToCustomImage(ImageType imageType)
        {
            throw new NotImplementedException();
        }

        public virtual AlgoImage ConvertToHalconImage(ImageType imageType)
        {
            throw new NotImplementedException();
        }

        protected abstract AlgoImage Convert2GreyImage();
        protected abstract AlgoImage Convert2ColorImage();
        public virtual AlgoImage ConvertToMilImage(ImageType imageType)
        {
            if (this.imageType == imageType)
                return this.Clone();

            AlgoImage converted = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    converted = Convert2GreyImage();
                    break;
                case ImageType.Color:
                    converted = Convert2ColorImage();
                    break;
                case ImageType.Depth:
                    throw new NotImplementedException();
                    break;
                case ImageType.Gpu:
                    throw new NotImplementedException();
                    break;
            }

            return converted;
        }

        public virtual AlgoImage ConvertToCognexImage(ImageType imageType)
        {
            throw new NotImplementedException();
        }

        public virtual AlgoImage ConvertToEuresysImage(ImageType imageType)
        {
            throw new NotImplementedException();
        }

        public virtual AlgoImage ConvertToOpenCvImage(ImageType imageType)
        {
            throw new NotImplementedException();
        }

        //public BitmapSource ToBitmapSource()
        //{
        //    BitmapSource bitmapSource = null;

        //    switch (this.imageType)
        //    {
        //        case ImageType.Grey:
        //            bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, GetByte(), this.Width);
        //            bitmapSource.Freeze();
        //            break;
        //        case ImageType.Color:
        //            bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Rgb24, null, GetByte(), this.Width * 3);
        //            bitmapSource.Freeze();
        //            break;
        //    }

        //    return bitmapSource;
        //}
    }
}

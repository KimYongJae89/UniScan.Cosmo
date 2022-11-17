using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;
using DynMvp.Base;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DynMvp.Vision.Matrox;
using Emgu.CV.CvEnum;

namespace DynMvp.Vision.OpenCv
{
    public abstract class OpenCvImage : AlgoImage
    {
        public override void Dispose()
        {
            lock (this.SubImageList)
            {
                while (this.SubImageList.Count() > 0)
                {
                    AlgoImage algoImage = this.SubImageList.Last();
                    if (algoImage != null)
                        algoImage.Dispose();
                }
                this.SubImageList.Clear();
            }

            if (this.ParentImage != null)
            {
                lock (this.ParentImage.SubImageList)
                    this.ParentImage.SubImageList.Remove(this);
            }
            this.ParentImage = null;

            DisposeImageObject();
        }

        //public abstract void Copy(AlgoImage srcImage, Rectangle srcRect);

        //public abstract void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size);

        public abstract bool IsCudaImage { get; }
        public abstract IInputArray InputArray { get; }
        public abstract IOutputArray OutputArray { get; }
        public abstract IInputOutputArray InputOutputArray { get; }
        public abstract void DisposeImageObject();
    }

    public class OpenCvGreyImage : OpenCvImage
    {
        public override bool IsCudaImage { get => false; }
        public override IInputArray InputArray { get => this.image; }
        
        public override IOutputArray OutputArray { get => this.image; }

        public override IInputOutputArray InputOutputArray { get => this.image; }

        public override int BitPerPixel => this.image.Mat.ElementSize;
        public OpenCvGreyImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;
        }

        public OpenCvGreyImage(int width, int height) 
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;

            this.image = new Image<Gray, byte>(width, height);
        }

        public OpenCvGreyImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;

            if (convertPack.Ptr == IntPtr.Zero)
            {
                this.image = new Image<Gray, byte>(convertPack.Width, convertPack.Height);
                this.image.Bytes = convertPack.Bytes;
            }
            else
            {
                this.image = new Image<Gray, byte>(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
            }
        }

        public override void DisposeImageObject()
        {
            if (this.image != null)
                this.image.Dispose();
            this.image = null;
        }

        public override void Clear(byte initVal)
        {
            image.SetValue(new MCvScalar(initVal));
        }

        public override IntPtr GetImagePtr()
        {
            return this.image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvGreyImage cloneImage = new OpenCvGreyImage();
            cloneImage.Image = image.Clone();

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvGreyImage clipImage = new OpenCvGreyImage();
            clipImage.Image = image.Copy(rectangle);

            return clipImage;
        }

        private Image<Gray, Byte> image;
        public Image<Gray, Byte> Image
        {
            get { return image; }
            set { image = value; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Pitch
        {
            get { return image.Data.GetUpperBound(1) + 1; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public override byte[] CloneByte()
        {
            return image.Bytes;
        }

        public override void PutByte(byte[] data)
        {
            image.Bytes = data;
        }

        public override void PutByte(IntPtr srcPtr, int srcPitch)
        {
            if (srcPitch == this.Pitch)
            {
                int length = Width * Height;
                unsafe
                {
                    Buffer.MemoryCopy(srcPtr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(srcPitch, this.Pitch);
                IntPtr ptr = this.image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = srcPtr + (i * srcPitch);
                    IntPtr dst = ptr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(image);
        }
        
        public override void Save(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            image.Save(fileName);
            LogHelper.Debug(LoggerType.Operation, string.Format("OpenCvGreyImage::Save - {0}", fileName));
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            dstImage = new OpenCvGreyImage();
            OpenCvGreyImage openCvGreyImage = dstImage as OpenCvGreyImage;
            openCvGreyImage.image = this.image.GetSubRect(rectangle);

            openCvGreyImage.ParentImage = this;
            lock(this.SubImageList)
                this.SubImageList.Add(openCvGreyImage);
        }

        //public override AlgoImage ConvertToMilImage(ImageType imageType)
        //{
        //    AlgoImage algoImage;
        //    switch (ImageType)
        //    {
        //        case ImageType.Grey:
        //            MilGreyImage milGreyImage = new MilGreyImage(this.Width, this.Height, this.image.MIplImage.ImageData);
        //            algoImage = milGreyImage;
        //            break;
        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(algoImage);
        //    algoImage.ParentImage = this;

        //    return algoImage;
        //    ImageD imageD = this.ToImageD();
        //    return ImageBuilder.Build(ImagingLibrary.MatroxMIL, imageD, imageType);
        //}

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    if (imageType == ImageType.Grey)
        //        return this.Clone();

        //    if(imageType == ImageType.Color)
        //    {
        //        Image<Gray, byte> channel = this.image.Clone();
        //        OpenCvColorImage openCvColorImage = new OpenCvColorImage();
        //        openCvColorImage.Image = new Image<Bgr, byte>(new Image<Gray, byte>[3] { this.image, this.image, this.image });
        //        return openCvColorImage;
        //    }

        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    return this.Clone();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    Image<Gray, byte> channel = this.image.Clone();
        //    OpenCvColorImage openCvColorImage = new OpenCvColorImage();
        //    openCvColorImage.Image = new Image<Bgr, byte>(new Image<Gray, byte>[3] { this.image, this.image, this.image });
        //    return openCvColorImage;
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            Copy(srcImage, srcRect.Location, Point.Empty, srcRect.Size);
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvGreyImage openCvSrcImage = (OpenCvGreyImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            openCvSrcImage.image.ROI = srcRect;
            this.image.ROI = dstRect;

            CvInvoke.cvCopy(openCvSrcImage.image, this.image, IntPtr.Zero);

            openCvSrcImage.image.ROI = Rectangle.Empty;
            this.image.ROI = Rectangle.Empty;
        }
    }

    public class OpenCvDepthImage : OpenCvImage
    {
        public override bool IsCudaImage { get => false; }
        public override IInputArray InputArray { get => this.image; }

        public override IOutputArray OutputArray { get => this.image; }

        public override IInputOutputArray InputOutputArray { get => this.image; }

        public override int BitPerPixel => this.image.Mat.ElementSize;

        public OpenCvDepthImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;
        }

        public OpenCvDepthImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;

            this.image = new Image<Gray, float>(width, height);
        }

        public OpenCvDepthImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;

            if (convertPack.Ptr == IntPtr.Zero)
            {
                this.image = new Image<Gray, float>(convertPack.Width, convertPack.Height);
                this.image.Bytes = convertPack.Bytes;
            }
            else
            {
                this.image = new Image<Gray, float>(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
            }
        }

        public override void DisposeImageObject()
        {
            if (this.image != null)
                this.image.Dispose();
            this.image = null;

            if (pinnedArray != null)
                pinnedArray.Free();
        }
        

        public override void Clear(byte initVal)
        {
            image.SetValue(new MCvScalar(initVal));
        }

        public override IntPtr GetImagePtr()
        {
            return this.image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvDepthImage cloneImage = new OpenCvDepthImage();
            cloneImage.Image = image.Clone();

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvDepthImage clipImage = new OpenCvDepthImage();
            clipImage.Image = image.Copy(rectangle);

            return clipImage;
        }

        private Image<Gray, float> image;
        public Image<Gray, float> Image
        {
            get { return image; }
            set { image = value; }
        }

        GCHandle pinnedArray;
        public GCHandle PinnedArray
        {
            get { return pinnedArray; }
            set { pinnedArray = value; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Pitch
        {
            get { return image.Data.GetUpperBound(1) + 1; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public override byte[] CloneByte()
        {
            return image.Bytes;
        }

        public override void PutByte(byte[] data)
        {
            image.Bytes = data;
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            if (pitch == this.Pitch)
            {
                int length = this.Pitch * this.Height;
                unsafe
                {
                    Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(pitch, this.Pitch);
                IntPtr basePtr = this.image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = ptr + (i * pitch);
                    IntPtr dst = basePtr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        //public override void PutByte(IntPtr srcPtr, int srcPitch)
        //{
        //    int length = Math.Min(srcPitch, this.Pitch);
        //    IntPtr ptr = this.image.Mat.DataPointer;
        //    for (int i = 0; i < this.Height; i++)
        //    {
        //        IntPtr src = srcPtr + (i * srcPitch);
        //        IntPtr dst = ptr + (i * this.Pitch);
        //        unsafe
        //        {
        //            Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
        //        }
        //    }
        //}

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(image);
        }
        
        public override void Save(string fileName)
        {
            image.Save(fileName);
        }

        protected override void GetSubImage(Rectangle rectangle,  out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.GetChildImage] float data type is not support");
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            throw new NotImplementedException();
        }

        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class OpenCvColorImage : OpenCvImage
    {
        public override bool IsCudaImage { get => false; }
        public override IInputArray InputArray { get => this.image; }

        public override IOutputArray OutputArray { get => this.image; }

        public override IInputOutputArray InputOutputArray { get => this.image; }
        
        public override int BitPerPixel => this.image.Mat.ElementSize;

        public OpenCvColorImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;
        }

        public OpenCvColorImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;

            this.image = new Image<Bgr, byte>(width, height);
        }

        public OpenCvColorImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;

            if (convertPack.Ptr == IntPtr.Zero)
            {
                this.image = new Image<Bgr, byte>(convertPack.Width, convertPack.Height);
                this.image.Bytes = convertPack.Bytes;
            }
            else
            {
                this.image = new Image<Bgr, byte>(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
            }
        }

        public override void DisposeImageObject()
        {
            if (this.image != null)
                this.image.Dispose();
            this.image = null;
        }

        public override void Clear(byte initVal)
        {
            image.SetValue(new Bgr(initVal, initVal, initVal));
        }

        public override IntPtr GetImagePtr()
        {
            return this.image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvColorImage cloneImage = new OpenCvColorImage();
            cloneImage.Image = image.Clone();

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }
        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    AlgoImage cloneImage = null;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            cloneImage = this.Convert2GreyImage();
        //            break;
        //        case ImageType.Color:
        //            cloneImage = this.Clone();
        //            break;
        //    }
        //    if (cloneImage == null)
        //        throw new NotImplementedException();

        //    return cloneImage;
        //}

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvColorImage clipImage = new OpenCvColorImage();
            clipImage.Image = image.Copy(rectangle);

            clipImage.FilteredList = FilteredList;

            return clipImage;
        }

        private Image<Bgr, Byte> image;
        public Image<Bgr, Byte> Image
        {
            get { return image; }
            set { image = value; }
        }

        public override int Pitch
        {
            get { return image.Data.GetUpperBound(1) + 1; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public override byte[] CloneByte()
        {
            throw new NotImplementedException();
        }

        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            if (pitch == this.Pitch)
            {
                int length = this.Pitch * this.Height;
                unsafe
                {
                    Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(pitch, this.Pitch);
                IntPtr basePtr = this.image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = ptr + (i * pitch);
                    IntPtr dst = basePtr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(image);
        }

        public override void Save(string fileName)
        {
                image.Save(fileName);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            dstImage = new OpenCvColorImage();
            OpenCvColorImage openCvGreyImage = dstImage as OpenCvColorImage;
            openCvGreyImage.image = this.image.GetSubRect(rectangle);

            openCvGreyImage.ParentImage = this;
            lock (this.SubImageList)
                this.SubImageList.Add(openCvGreyImage);
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    Image<Bgr, byte> color = this.image.Clone();
        //    OpenCvGreyImage openCvGrayImage = new OpenCvGreyImage();
        //    openCvGrayImage.Image = new Image<Gray, byte>(color.Size);
        //    CvInvoke.CvtColor(color, openCvGrayImage.Image, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
        //    return openCvGrayImage;
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    return this.Clone();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvColorImage openCvSrcImage = (OpenCvColorImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            openCvSrcImage.image.ROI = srcRect;
            this.image.ROI = dstRect;

            CvInvoke.cvCopy(openCvSrcImage.image, this.image, IntPtr.Zero);

            openCvSrcImage.image.ROI = Rectangle.Empty;
            this.image.ROI = Rectangle.Empty;
        }
    }

    public class OpenCvCudaImage : OpenCvImage
    {
        public Emgu.CV.Cuda.Stream TransferStream { get => this.transferStream; }
        Emgu.CV.Cuda.Stream transferStream = new Emgu.CV.Cuda.Stream();

        public override bool IsCudaImage { get => true; }

        public override bool IsReady => transferStream.Completed;

        public CudaImage<Gray, byte> Image { get => image; set => image = value; }
        CudaImage<Gray, byte> image = null;

        public Image<Gray, byte> HostImage { get => hostImage;  }
        Image<Gray, byte> hostImage = null;

        public override IInputArray InputArray { get => this.image; }

        public override IOutputArray OutputArray { get => this.image; }

        public override IInputOutputArray InputOutputArray { get => this.image; }

        public override int BitPerPixel => this.hostImage.Mat.ElementSize;

        public OpenCvCudaImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;
        }

        public OpenCvCudaImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;

            Create(width, height);
        }

        public OpenCvCudaImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;

            if (convertPack.Ptr == IntPtr.Zero)
                Create(convertPack.Width, convertPack.Height, convertPack.Bytes);
            else
                Create(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
        }

        public override void WaitReady()
        {
            transferStream.WaitForCompletion();
        }

        public override void DisposeImageObject()
        {
            this.image?.Dispose();
            this.image = null;

            this.hostImage?.Dispose();
            this.hostImage = null;

            this.transferStream?.Dispose();
            this.transferStream = null;
        }

        public override int Height
        {
            get { return hostImage.Height; }
        }

        public override int Width
        {
            get { return hostImage.Width; }
        }

        public override int Pitch
        {
            get { return hostImage.Mat.Step; }
        }
        
        public override void Clear(byte initVal)
        {
            image.SetTo(new MCvScalar(initVal), null, transferStream);
        }

        public void Clear(byte initVal, Emgu.CV.Cuda.Stream stream)
        {
            image.SetTo(new MCvScalar(initVal), null, stream);

        }
        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvCudaImage clipImage = new OpenCvCudaImage();
            CudaImage<Gray, byte> temp = new CudaImage<Gray, byte>(this.image, new Range(rectangle.Top, rectangle.Bottom), new Range(rectangle.Left, rectangle.Right));

            clipImage.image = temp;
            return clipImage;
        }

        public override AlgoImage Clone()
        {
            OpenCvCudaImage newImage = new OpenCvCudaImage();
            newImage.image = (CudaImage<Gray, byte>)image.Clone(null);
            return newImage;
        }

        public override byte[] CloneByte()
        {
            UpdateHostImage();
            this.WaitReady();

            return this.hostImage.Bytes;
            byte[] bytes = new byte[image.Size.Width * image.Size.Height];

            GCHandle pinnedArray = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            IntPtr ptr = pinnedArray.AddrOfPinnedObject();

            Mat mat = new Mat(image.Size, image.Depth, image.NumberOfChannels, ptr, image.Size.Width);
            image.Download(mat);
            mat.Dispose();

            pinnedArray.Free();

            return bytes;
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            OpenCvCudaImage newImage = new OpenCvCudaImage(); 
            newImage.image = image.GetSubRect(rectangle);
            newImage.hostImage = hostImage.GetSubRect(rectangle);

            newImage.ParentImage = this;
            lock(this.SubImageList)
                this.SubImageList.Add(newImage);

            dstImage = newImage as OpenCvCudaImage;
        }

        public override IntPtr GetImagePtr()
        {
            UpdateHostImage();
            //transferStream.WaitForCompletion();
            return this.hostImage.Mat.DataPointer;
        }

        public override void PutByte(byte[] data)
        {
            this.hostImage.Bytes = data;
            image.Upload(this.hostImage, transferStream);
            transferStream.WaitForCompletion();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            Mat mat = hostImage.Mat;
            Image<Gray, byte> newHostImage = new Image<Gray, byte>(mat.Width, mat.Height, pitch, ptr);
            this.hostImage?.Dispose();
            this.hostImage = newHostImage;

            this.image.Upload(this.hostImage, transferStream);
            transferStream.WaitForCompletion();
        }

        public void Create(int width, int height)
        {
            this.hostImage = new Image<Gray, byte>(width, height);
            this.image = new CudaImage<Gray, byte>(height, width);
        }

        public void Create(int width, int height, byte[] data)
        {
            if (image != null)
                image.Dispose();

            //System.Diagnostics.Debug.Assert(data.Length == pitch * height);

            //GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            //IntPtr ptr = pinnedArray.AddrOfPinnedObject();

            //Create(width, height, pitch, ptr);

            //pinnedArray.Free();

            this.hostImage = new Image<Gray, byte>(width, height);
            Marshal.Copy(data, 0, this.hostImage.Mat.DataPointer, data.Length);
            //this.hostImage.Bytes = data;

            this.image = new CudaImage<Gray, byte>(this.hostImage);
        }

        public void Create(int width, int height, int pitch, IntPtr data)
        {
            if (this.image != null)
                this.image.Dispose();

            this.hostImage = new Image<Gray, byte>(width, height, pitch, data);
            this.image = new CudaImage<Gray, byte>(this.hostImage);
        }

        public void UpdateHostImage()
        {
            this.image.Download(this.hostImage.Mat, transferStream);

            //this.WaitTransferStream();
        }

        public override void Save(string fileName)
        {
            string dirName = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(dirName);

            UpdateHostImage();
            this.WaitReady();
            OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
            openCvGreyImage.Image = this.hostImage;
            openCvGreyImage.Save(fileName);
        }

        public override ImageD ToImageD()
        {
            Image<Gray, byte> openCvImage = this.image.ToImage();
            ImageD imageD = OpenCvImageBuilder.ConvertImage(openCvImage);
            openCvImage.Dispose();
            return imageD;
        }

        //public override AlgoImage ConvertToMilImage(ImageType imageType)
        //{
        //    AlgoImage openCvImage = this.ConvertToOpenCvImage(imageType);
        //    AlgoImage milImage = openCvImage.ConvertToMilImage(imageType);
        //    //openCvImage.Dispose();
        //    return milImage;

        //    //return ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.ToImageD(), imageType);
        //}

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    AlgoImage algoImage;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
        //            //openCvGreyImage.Image = new Image<Gray, byte>(this.Width, this.Height);
        //            //this.image.Download(openCvGreyImage.Image);
        //            openCvGreyImage.Image = this.image.ToImage();
        //            algoImage = openCvGreyImage;
        //            break;

        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(algoImage);
        //    algoImage.ParentImage = this;
        //    return algoImage;

        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            Copy(srcImage, srcRect.Location, Point.Empty, srcRect.Size);
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvCudaImage openCvSrcImage = (OpenCvCudaImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            CudaImage<Gray, byte> srcCudaImage = openCvSrcImage.image.ColRange(srcRect.Left, srcRect.Right).RowRange(srcRect.Top, srcRect.Bottom);
            CudaImage<Gray, byte> dstCudaImage = this.image.ColRange(dstRect.Left, dstRect.Right).RowRange(dstRect.Top, dstRect.Bottom);
            srcCudaImage.CopyTo(dstCudaImage, null, this.transferStream);
            //this.transferStream?.WaitForCompletion();
        }
    }
}

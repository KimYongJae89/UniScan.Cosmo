using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using Standard.DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using Standard.DynMvp.Devices;

namespace Standard.DynMvp.Vision.Matrox
{
    public abstract class MilImage : AlgoImage, MilObject
    {
        System.Diagnostics.StackTrace stackTrace;
              
        ~MilImage()
        {
            Dispose();
        }

        public void Free()
        {
            if (image != MIL.M_NULL)
            {
                MIL.MbufFree(image);
                image = MIL.M_NULL;
            }
        }

        public override void Dispose()
        {
            if (this.SubImageList.Count > 0)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("MilImage Dispose but Subimage is exist.\r\n{0}", stackTrace?.ToString()));
                LogHelper.Error(LoggerType.Inspection, string.Format("MilImage Dispose but Subimage is exist.\r\n{0}", new System.Diagnostics.StackTrace().ToString()));
                LogSubImageProp(this);
            }
            System.Diagnostics.Debug.Assert(this.SubImageList.Count == 0, "SubImage is Exist!!");
            MilObjectManager.Instance.ReleaseObject(this);

            if (this.ParentImage != null)
            {
                lock (this.ParentImage.SubImageList)
                    this.ParentImage.SubImageList.Remove(this);
            }

            this.stackTrace = null;
        }

        private void LogSubImageProp(AlgoImage algoImage, int detpth = 0)
        {
            string str = string.Format("{0} N:{1}, W:{2}, H{3}, T{4}", new string('\t', detpth), algoImage.Name, algoImage.Width, algoImage.Height, algoImage.Tag?.ToString());
            LogHelper.Error(LoggerType.Inspection, string.Format("SubImage.\r\n{0}", str));
            for (int i = 0; i < algoImage.SubImageList.Count; i++)
                LogSubImageProp(algoImage.SubImageList[i], detpth + 1);
        }

        public override void Clear(byte initVal)
        {
            MIL.MbufClear(image, initVal);
        }

        public override AlgoImage Clone()
        {
            return Clip(new Rectangle(0, 0, width, height));
        }

        protected MIL_ID image = MIL.M_NULL;
        public MIL_ID Image
        {
            get { return image; }
            set { image = value; }
        }

        protected int width;
        public override int Width
        {
            get { return width; }
        }

        protected int height;
        public override int Height
        {
            get { return height; }
        }

        public override int Pitch
        {
            get
            {
                if (image == MIL.M_NULL)
                    throw new InvalidObjectException("[MilImage.Pitch]");

                MIL_INT pitch = MIL.MbufInquire(image, MIL.M_PITCH);
                return (int)pitch;
            }
        }

        public static void CheckMilImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilImage milImage = algoImage as MilImage;
                if (milImage == null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be gray image", functionName, imageName));
            }
        }

        public static void CheckGreyImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilGreyImage milImage = algoImage as MilGreyImage;
                if (milImage==null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
            }
            catch ( InvalidCastException )
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be gray image", functionName, imageName));
            }
        }

        public static void CheckColorImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilColorImage milImage = algoImage as MilColorImage;
                if (milImage == null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be color image", functionName, imageName));
            }
        }

        public void Alloc(int width, int height)
        {
            Alloc(width, height, IntPtr.Zero, 0);
        }

        public abstract void Alloc(int width, int height, IntPtr dataPtr, int pitch);
        //public abstract void Copy(AlgoImage srcImage, Rectangle rectangle);
        public abstract void Put(byte[] userArrayPtr);
        public abstract void Get(byte[] userArrayPtr);
        public abstract void Put(float[] userArrayPtr);
        public abstract void Get(float[] userArrayPtr);
        
        public override void Save(string fileName)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilImage.Alloc]");

            string path = Path.GetDirectoryName(fileName);
            if (string.IsNullOrEmpty(path))
                return;

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            MIL_INT fileFormat = MIL.M_BMP;
            string extension = Path.GetExtension(fileName).ToLower();

            if (extension.Contains(".jpg") == true || extension.Contains(".jpeg") == true)
                fileFormat = MIL.M_JPEG_LOSSY;
            if (extension.Contains(".png") == true)
                fileFormat = MIL.M_PNG;

            MIL.MbufExport(fileName, fileFormat, image);   
        }
        
        public override byte[] CloneByte()
        {
            byte[] data = new byte[width * height];
            Get(data);

            return data;
        }

        public override void PutByte(byte[] data)
        {
            Put(data);
        }

        public override bool Equals(object obj)
        {
            if(obj is MilImage)
            {
                MilImage milImage = (MilImage)obj;
                return milImage.Image == this.image;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.image.GetHashCode();
        }

        public void AddTrace()
        {
#if DEBUG
            this.stackTrace = new System.Diagnostics.StackTrace();
#endif
        }

        public System.Diagnostics.StackTrace GetTrace()
        {
            return this.stackTrace;
        }

        protected void GetSubImage(Rectangle rectangle, MilImage milChildImage)
        {
            //Rectangle wholeRect = new Rectangle(0, 0, width, height);
            //rectangle.Intersect(wholeRect);

            //lock (MilImageProcessing.lockObject)
                milChildImage.image = MIL.MbufChild2d(image, (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, MIL.M_NULL);
           
            milChildImage.width = rectangle.Width;
            milChildImage.height = rectangle.Height;
            milChildImage.ParentImage = this;
        }
    }

    public class MilGreyImage : MilImage
    {
        public MilGreyImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;
        }

        public MilGreyImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;

            Alloc(width, height);
        }

        public MilGreyImage(int width, int height, IntPtr dataPtr, int pitch = 0)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;

            Alloc(width, height, dataPtr, pitch);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Alloc]");

            this.width = width;
            this.height = height;
            if (pitch == 0)
                pitch = width;
            pitch = (pitch + 3) / 4 * 4;

            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                if (MatroxHelper.UseNonPagedMem)
                    attribute += MIL.M_NON_PAGED;

                //lock (MilImageProcessing.lockObject)
                    image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, attribute, MIL.M_NULL);
            }
            else
            {
                //lock (MilImageProcessing.lockObject)
                    image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH, MIL.M_DEFAULT, (ulong)dataPtr, MIL.M_NULL);
            }

            //MilObjectManager.Instance.AddObject(this);
        }

        public override IntPtr GetImagePtr()
        {
            IntPtr ptr = MIL.MbufInquire(image, MIL.M_HOST_ADDRESS, MIL.M_NULL);

            return ptr;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilGreyImage cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override AlgoImage Clone(ImageType imageType)
        {
            AlgoImage cloneImage = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    cloneImage = this.Clone();
                    break;
                case ImageType.Color:
                    cloneImage = this.Convert2ColorImage();
                    break;
            }
            if (cloneImage == null)
                throw new NotImplementedException();

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            MilImage.CheckGreyImage(srcImage, "MilGreyImage.Copy", "Source");

            MilGreyImage milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcRect.Left, srcRect.Top, MIL.M_ALL_BAND, 0, 0, srcRect.Width, srcRect.Height);

            milSrcImage.FilteredList = FilteredList;
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            MilImage.CheckGreyImage(srcImage, "MilGreyImage.Copy", "Source");

            MilGreyImage milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcPt.X, srcPt.Y, MIL.M_ALL_BAND, dstPt.X, dstPt.Y, size.Width, size.Height);

            milSrcImage.FilteredList = FilteredList;
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Get]");
            
            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            MilGreyImage milChildImage = new MilGreyImage();
            GetSubImage(rectangle, milChildImage);
            dstImage = milChildImage;

            //dstImage = new MilGreyImage();
            //MilGreyImage milChildImage = dstImage as MilGreyImage;

            //Rectangle wholeRect = new Rectangle(0, 0, width, height);
            ////if (rectangle.IntersectsWith(wholeRect))
            //rectangle.Intersect(wholeRect);

            //milChildImage.image = MIL.MbufChild2d(image, (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, MIL.M_NULL);
            //MilObjectManager.Instance.AddObject(milChildImage);

            //milChildImage.width = rectangle.Width;
            //milChildImage.height = rectangle.Height;

            //milChildImage.ParentImage = this;
            MilObjectManager.Instance.AddObject(milChildImage);

            lock (this.SubImageList)
                this.SubImageList.Add(milChildImage);
        }

        public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        {
            IntPtr ptr = this.GetImagePtr();
            Image2D image2d = new Image2D(width, height, 1, Pitch, ptr);

            AlgoImage newImage;
            switch (imageType)
            {
                case ImageType.Grey:
                    newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
                    break;
                case ImageType.Gpu:
                    newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
                    //newImage = new OpenCvCudaImage();
                    //((OpenCvCudaImage)newImage).Create(this.width, this.height, ptr);
                    break;

                default:
                    throw new NotImplementedException();
            }

            this.SubImageList.Add(newImage);
            newImage.ParentImage = this;
            return newImage;
        }

        protected override AlgoImage Convert2GreyImage()
        {
            return this.Clone();
        }

        protected override AlgoImage Convert2ColorImage()
        {
            MilColorImage milColorImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, this.width, this.height) as MilColorImage;

            MIL.MbufCopy(this.image, milColorImage.Image);
            return milColorImage;
        }
    }

    public class MilDepthImage : MilImage
    {
        public MilDepthImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Depth;
        }

        public MilDepthImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Depth;

            Alloc(width, height);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilDepthImage.Alloc]");

            this.width = width;
            this.height = height;
            if (dataPtr == IntPtr.Zero)
                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            else
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, MIL.M_IMAGE + MIL.M_PROC, MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr.ToInt64(), MIL.M_NULL);

            if (image == MIL.M_NULL)
                throw new AllocFailedException("[MilDepthImage.Alloc]");

            //MilObjectManager.Instance.AddObject(this);
        }

        public override IntPtr GetImagePtr()
        {
            return MIL.MbufInquire(image, MIL.M_HOST_ADDRESS, MIL.M_NULL);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilGreyImage cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            MilImage.CheckGreyImage(srcImage, "MilDepthImage.Copy", "Source");

            MilGreyImage milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcRect.Left, srcRect.Top, MIL.M_ALL_BAND, 0, 0, srcRect.Width, srcRect.Height);

            FilteredList = milSrcImage.FilteredList;
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            MilImage.CheckGreyImage(srcImage, "MilGreyImage.Copy", "Source");

            MilGreyImage milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcPt.X, srcPt.Y, MIL.M_ALL_BAND, dstPt.X, dstPt.Y, size.Width, size.Height);

            FilteredList = milSrcImage.FilteredList;
        }

        public override void Put(byte[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilDepthImage.Put] Byte data type is not support.");
        }

        public override void Get(byte[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilDepthImage.Get] Byte data type is not support.");
        }

        public override void Put(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Get]");

            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        protected override AlgoImage Convert2GreyImage()
        {
            throw new NotImplementedException();
        }

        protected override AlgoImage Convert2ColorImage()
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Clone(ImageType imageType)
        {
            throw new NotImplementedException();
        }
    }

    public class MilColorImage : MilImage
    {
        public MilColorImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Color;
        }

        public MilColorImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Color;

            Alloc(width, height);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Alloc] Already Allocated.");

            this.width = width;
            this.height = height;
            if (dataPtr == IntPtr.Zero)
                image = MIL.MbufAllocColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            else
                image = MIL.MbufCreateColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_PITCH_BYTE, pitch,ref dataPtr, MIL.M_NULL);

            if (image == MIL.M_NULL)
                throw new AllocFailedException("[MilColorImage.Alloc]");

            //MilObjectManager.Instance.AddObject(this);
        }

        public override IntPtr GetImagePtr()
        {
            return MIL.MbufInquire(image, MIL.M_HOST_ADDRESS, MIL.M_NULL);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilGreyImage cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            return cloneImage;
        }

        private MIL_INT GetBand(ImageBandType imageBandType)
        {
            MIL_INT band;
            switch (imageBandType)
            {
                case ImageBandType.Red: band = MIL.M_RED; break;
                case ImageBandType.Blue: band = MIL.M_BLUE; break;
                case ImageBandType.Green: band = MIL.M_GREEN; break;
                default:
                    throw new ArgumentException("Invalid Image Band Type");
            }

            return band;
        }

        public MilImage Clone(ImageBandType imageBandType)
        {
            return Clone(new Rectangle(0, 0, width, height), imageBandType);
        }

        public override AlgoImage Clone(ImageType imageType)
        {
            AlgoImage cloneImage = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    cloneImage = this.Convert2GreyImage();
                    break;
                case ImageType.Color:
                    cloneImage = this.Clone();
                    break;
            }
            if (cloneImage == null)
                throw new NotImplementedException();

            return cloneImage;
        }

        public MilImage Clone(Rectangle rectangle, ImageBandType imageBandType)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Clone] Image is not allocated ");

            MilImage cloneImage;

            if (imageBandType == ImageBandType.Luminance)
            {
                cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
                MIL.MimConvert(image, cloneImage.Image, MIL.M_RGB_TO_L);
            }
            else
            {
                MIL_INT band = GetBand(imageBandType);
                cloneImage = new MilColorImage(rectangle.Width, rectangle.Height);
                MIL.MbufCopyColor2d(image, cloneImage.Image, band, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);
            }

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            MilImage.CheckColorImage(srcImage, "MilColorImage.Copy", "Source");

            MilColorImage milSrcImage = (MilColorImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcRect.Left, srcRect.Top, MIL.M_ALL_BAND, 0, 0, srcRect.Width, srcRect.Height);

            FilteredList = milSrcImage.FilteredList;
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            MilImage.CheckColorImage(srcImage, "MilColorImage.Copy", "Source");

            MilColorImage milSrcImage = (MilColorImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcPt.X, srcPt.Y, MIL.M_ALL_BAND, dstPt.X, dstPt.Y, size.Width, size.Height);

            FilteredList = milSrcImage.FilteredList;
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Put]");

            MIL.MbufPutColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Get]");

            MIL.MbufGetColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            MilColorImage milChildImage = new MilColorImage();
            GetSubImage(rectangle, milChildImage);
            dstImage = milChildImage;
        }

        protected override AlgoImage Convert2GreyImage()
        {
            MilGreyImage milImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, this.width, this.height) as MilGreyImage;

            MIL.MbufCopy(this.image, milImage.Image);
            return milImage;
        }

        protected override AlgoImage Convert2ColorImage()
        {
            return this.Clone();
        }
    }
}

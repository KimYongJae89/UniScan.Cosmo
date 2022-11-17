using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using DynMvp.Base;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvImageBuilder : ImageBuilder
    {
        CudaDeviceInfo cudaDeviceInfo = null;
        public OpenCvImageBuilder()
        {
            int device = -1;
            try
            {
                device = CudaInvoke.GetCudaEnabledDeviceCount();
                //int ii =  CudaInvoke.GetDevice();
            }
            catch (TypeInitializationException ex)
            {
                Exception exception = ex;
                do
                {
                    LogHelper.Error(LoggerType.Error, string.Format("OpenCvImageBuilder::OpenCvImageBuilder() - Exception {0}", exception.Message));
                    exception = exception.InnerException;
                } while (exception != null);
            }

            if (device <= 0)
                return;

            this.cudaDeviceInfo = new CudaDeviceInfo();
        }

        public override bool IsCudaAvailable()
        {
            return ((this.cudaDeviceInfo != null) && this.cudaDeviceInfo.IsCompatible);
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            AlgoImage algoImage = null;
            Bitmap bitmap = null;
            ImageD image2D = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    //bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    //image2D = Image2D.ToImage2D(bitmap);
                    //algoImage = BuildGreyImage(image2D, ImageBandType.Luminance);
                    algoImage = BuildGreyImage(width, height);
                    break;

                case ImageType.Color:
                    //bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    //image2D = Image2D.ToImage2D(bitmap);
                    //algoImage = BuildColorImage(image2D);
                    algoImage = BuildColorImage(width, height);
                    break;

                case ImageType.Depth:
                    throw new NotImplementedException();
                    break;

                case ImageType.Gpu:
                    //image2D = new Image2D(width, height, 1);
                    //algoImage = BuildCudaImage(image2D);
                    algoImage = BuildCudaImage(width, height);
                    break;

                default:
                    throw new InvalidTypeException();
            }

            return algoImage;
        }

        public override AlgoImage Build(ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgoImage algoImage = null;
            try
            {
                switch (imageType)
                {
                    case ImageType.Grey:
                        algoImage = BuildGreyImage(image, imageBand);
                        break;

                    case ImageType.Color:
                        algoImage = BuildColorImage(image);
                        break;

                    case ImageType.Depth:
                        algoImage = BuildDepthImage(image);
                        break;

                    case ImageType.Gpu:
                        algoImage = BuildCudaImage(image);
                        break;

                    default:
                        throw new InvalidTypeException(string.Format("Not Suppoerted ImageType {0}", imageType.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("OpenCvImageBuilder::Build Exception {0}", ex.Message));
            }
            return algoImage;
        }

        private AlgoImage BuildCudaImage(int width, int height)
        {
            OpenCvCudaImage openCvCudaImage = new OpenCvCudaImage();
            openCvCudaImage.Create(width, height);

            return openCvCudaImage;
        }

        private AlgoImage BuildCudaImage(ImageD image)
        {
            if ((image is Image2D == false))
                return null;

            OpenCvCudaImage openCvCudaImage = new OpenCvCudaImage();

            Image2D image2D = (Image2D)image;
            if (image2D.IsUseIntPtr())
            {
                openCvCudaImage.Create(image2D.Width, image2D.Height, image.Pitch, image2D.DataPtr);
            }
            else
            {
                byte[] data = image2D.ImageData.Data;
                switch (image2D.NumBand)
                {
                    case 1:
                        openCvCudaImage.Create(image.Width, image.Height, image2D.ImageData.Data);
                        break;
                    default:
                        throw new InvalidTypeException();
                }
            }
            return openCvCudaImage;
        }

        private AlgoImage BuildGreyImage(int width, int height)
        {
            OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
            openCvGreyImage.Image = new Image<Gray, byte>(width, height);

            return openCvGreyImage;
        }

        private AlgoImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            Image2D image2d = (Image2D)image;
            if (image.NumBand == 3)
            {
                OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
                Image<Bgr, Byte> colorImage = new Image<Bgr, byte>(image2d.Width, image2d.Height);
                byte[] imageBytes = colorImage.Bytes;

                int imagePitch = colorImage.Data.GetUpperBound(1) + 1;

                for (int y = 0; y < image2d.Height; y++)
                {
                    Array.Copy(image2d.Data, y * image2d.Width * 3, imageBytes, y * imagePitch * 3, image2d.Width * 3);
                }
                colorImage.Bytes = imageBytes;

                if (imageBand == ImageBandType.Luminance)
                {
                    openCvGreyImage.Image = colorImage.Convert<Gray, Byte>();
                }
                else
                {
                    openCvGreyImage.Image = new Image<Gray, Byte>(image2d.Width, image2d.Height);
                    IInputArray inputArray = (IInputArray)colorImage.GetInputArray();
                    IOutputArray outputArray = (IOutputArray)openCvGreyImage.Image.GetOutputArray();

                    switch (imageBand)
                    {
                        case ImageBandType.Red:
                            CvInvoke.Split(inputArray, outputArray);
                            break;
                        case ImageBandType.Green:
                            CvInvoke.Split(inputArray, null);
                            CvInvoke.Split(inputArray, outputArray);

                            break;
                        case ImageBandType.Blue:
                            CvInvoke.Split(inputArray, null);
                            CvInvoke.Split(inputArray, null);
                            CvInvoke.Split(inputArray, outputArray);
                            break;
                    }
                }

                return openCvGreyImage;
            }
            else
            {
                OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
                Image<Gray, Byte> cvImage = null;
                if (image2d.IsUseIntPtr())
                {
                    cvImage = new Image<Gray, Byte>(image2d.Width, image2d.Height, image2d.Pitch, image2d.DataPtr);
                }
                else
                {
                    cvImage = new Image<Gray, Byte>(image2d.Width, image2d.Height);
                    byte[] imageBytes = cvImage.Bytes;
                    int imagePitch = image2d.Pitch;// cvImage.Data.GetUpperBound(1) + 1;

                    //if (image2d.IsUseIntPtr())
                    //    image2d.ConverFromDataPtr();

                    for (int y = 0; y < image2d.Height; y++)
                    {
                        {
                            //Marshal.Copy(image2d.DataPtr /*+ y * imagePitch*/, imageBytes, y * imagePitch, imagePitch);
                        }

                        {
                            Array.Copy(
                                image2d.Data, y * imagePitch,
                                imageBytes, y * imagePitch,
                                imagePitch);
                        }
                    }

                    cvImage.Bytes = imageBytes;
                    if (image2d.Roi.IsEmpty == false)
                        cvImage.ROI = image2d.Roi;
                }
                openCvGreyImage.Image = cvImage;
                return openCvGreyImage;
            }
        }

        private AlgoImage BuildDepthImage(ImageD image)
        {
            OpenCvDepthImage openCvDepthImage = new OpenCvDepthImage();

            Image3D image3d = (Image3D)image;

            openCvDepthImage.Image = new Image<Gray, float>(image3d.Width, image3d.Height);

            for (int y=0; y<image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    openCvDepthImage.Image.Data[y, x, 0] = image3d.Data[y * image.Width + x];
                }
            }

            return openCvDepthImage;
        }

        private AlgoImage BuildColorImage(int width, int height)
        {
            OpenCvColorImage openCvColorImage = new OpenCvColorImage();
            openCvColorImage.Image = new Image<Bgr, byte>(width, height);
            return openCvColorImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            OpenCvColorImage openCvColorImage = new OpenCvColorImage();

            Image2D image2d = (Image2D)image;

            Image<Bgr, Byte> cvImage = new Image<Bgr, Byte>(image2d.Width, image2d.Height);
            byte[] imageBytes = cvImage.Bytes;

            int imagePitch = (cvImage.Data.GetUpperBound(1) + 1) * 3;
        
            for (int y = 0; y < image2d.Height; y++)
            {
                Array.Copy(image2d.Data, y * image2d.Pitch, imageBytes, y * imagePitch, image2d.Pitch);
            }
            cvImage.Bytes = imageBytes;
            cvImage.ROI = image2d.Roi;
            openCvColorImage.Image = cvImage;
            return openCvColorImage;
        }

        public static ImageD ConvertImage(Image<Gray, byte> cvImage)
        {
            if (cvImage == null)
                return null;

            //int width = cvImage.Width;
            //int height = cvImage.Height;
            //int band = cvImage.NumberOfChannels;
            //int pitch = cvImage.MIplImage.WidthStep * cvImage.NumberOfChannels;

            ImageD image = Image2D.ToImage2D(cvImage.Bitmap);
            //Image2D image = new Image2D();
            //image.Initialize(width, height, band, pitch, cvImage.MIplImage.ImageData);
            return image;
        }

        public static ImageD ConvertImage(Image<Gray, float> cvImage)
        {
            if (cvImage == null)
                return null;

            int width = cvImage.Width;
            int height = cvImage.Height;
            int imagePitch = cvImage.Data.GetUpperBound(1) + 1;

            Image3D image = new Image3D();
            image.Initialize(width, height, 1, imagePitch);

            image.SetData(cvImage.Bytes);

            return image;
        }

        public static ImageD ConvertImage(Image<Bgr, byte> cvImage)
        {
            if (cvImage == null)
                return null;

            int width = cvImage.Width;
            int height = cvImage.Height;
            int imagePitch = (cvImage.Data.GetUpperBound(1) + 1) * 3;

            Image2D image = Image2D.ToImage2D(cvImage.ToBitmap());

            //Image2D image = new Image2D();
            //image.Initialize(width, height, 3, 0);

            //image.SetData(cvImage.Bytes);

            return image;
        }

        public static ImageD ConvertImage(CudaImage<Gray, byte> cvImage)
        {
            if (cvImage == null)
                return null;

            int width = cvImage.Size.Width;
            int height = cvImage.Size.Height;
            int band = cvImage.NumberOfChannels;
            int imagePitch = ((width * cvImage.NumberOfChannels + 3) / 4) * 4;

            Image<Gray, byte> cvGrayImage = cvImage.ToImage();
            ImageD image = ConvertImage(cvGrayImage);

            //cvGrayImage.Save(@"d:\OCV.bmp");
            //image.SaveImage(@"d:\imageD.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return image;
        }

        //protected override AlgoImage ConvertFromOpenCv(AlgoImage algoImage, ImageType imageType)
        //{
        //    if (algoImage.ImageType == imageType)
        //        return algoImage.Clone();

        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromEuresysOpenEVision(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromCognexVisionPro(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromMatroxMIL(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromHalcon(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromCustom(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}
    }
    
}

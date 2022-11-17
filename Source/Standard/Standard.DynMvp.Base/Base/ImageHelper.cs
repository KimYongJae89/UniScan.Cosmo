using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace Standard.DynMvp.Base
{
    public class ImageHelper
    {
        private class SaveQueueData
        {
            string name;
            public string Name
            {
                get { return name; }
            }

            byte[] data;
            public byte[] Data
            {
                get { return data; }
            }

            public SaveQueueData(string name, byte[] data)
            {
                this.name = name; this.data = data;
            }
        }

        static object lockObject = new Object();

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        static List<SaveQueueData> saveQueueDataList = new List<SaveQueueData>();
        static Thread saveThread = new Thread(saveProc);

        private static void saveProc()
        {
            SaveQueueData curSaveData = null;
            while (true)
            {
                lock (saveQueueDataList)
                {
                    if (saveQueueDataList.Count > 0)
                    {
                        curSaveData = saveQueueDataList[0];
                        saveQueueDataList.RemoveAt(0);
                    }
                }

                if (curSaveData != null)
                {
                    try
                    {
                        File.WriteAllBytes(curSaveData.Name, curSaveData.Data);
                    }
                    catch (IOException)
                    {

                    }
                    catch (UnauthorizedAccessException)
                    {

                    }
                    curSaveData = null;
                }

                Thread.Sleep(10);
            }
        }

        public static string BitmapToBase64String(Bitmap image)
        {
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Bmp);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static string ImageToBase64String(Image image, ImageFormat imageFormat)
        {
            if (imageFormat == null)
                imageFormat = ImageFormat.Bmp;

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static Bitmap Base64StringToBitmap(string base64string)
        {
            if (string.IsNullOrEmpty(base64string))
                return null;

            byte[] imageByte = Convert.FromBase64String(base64string);

            MemoryStream memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            Bitmap bitmap = new Bitmap(memoryStream);

            return bitmap;
        }

        public static Image Base64StringToImage(string base64string, ImageFormat imageFormat)
        {
            if (base64string == "")
                return null;

            byte[] imageByte = Convert.FromBase64String(base64string);

            MemoryStream memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            Image image = Image.FromStream(memoryStream);

            return image;
        }

        public static Image LoadImage(string fileName)
        {
            try
            {
                if (File.Exists(fileName) == false)
                    return null;

                if (false/*Path.GetExtension(fileName) == ".bmp"*/)
                {
                    ImageD imageD = new Image2D();
                    imageD.LoadImage(fileName);
                    Bitmap bitmap = imageD.ToBitmap();
                    imageD.Dispose();
                    return bitmap;
                }
                else
                {
                    try
                    {
                        FileStream fs = new FileStream(fileName, FileMode.Open);
                        Image image = Image.FromStream(fs);
                        fs.Close();
                        //Image image = Image.FromFile(fileName);
                        return image;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::LoadImage - {0},{1}", fileName, ex.Message));
                        return null;
                    }
                }
            }
#if DEBUG==false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::LoadImage - {0}", ex.Message));
            }
#endif
            finally { }

            return null;
        }

        public static void SaveImage(Image image, string fileName)
        {
            ImageFormat imageFormat = ImageFormat.Bmp;
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".jpg" || extension == ".jpeg")
                imageFormat = ImageFormat.Jpeg;
            else if(extension == ".png")
                imageFormat = ImageFormat.Png;

            SaveImage(image, fileName, imageFormat);
        }

        public static void SaveImage(Image image, string fileName, ImageFormat imageFormat)
        {
            if (image == null)
                return;

            //LogHelper.Debug(LoggerType.Debug, string.Format("ImageHelper::SaveImage - {0}", fileName));
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                lock (image)
                    image.Save(memoryStream, imageFormat);
                memoryStream.Position = 0;

                byte[] imageByte = memoryStream.ToArray();
                //byte[] imageByte = new byte[memoryStream.Length];
                memoryStream.Close();
                memoryStream.Dispose();
                string dirName = Path.GetDirectoryName(fileName);
                if (Directory.Exists(dirName) == false)
                    Directory.CreateDirectory(dirName);

                Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
                stream.Write(imageByte, 0, imageByte.Length);
                stream.Flush();
                stream.Close();
                stream.Dispose();

                //LogHelper.Debug(LoggerType.Debug, "ImageHelper::SaveImage End");

                //memoryStream.Position = 0;
                //byte[] imageByte = memoryStream.ToArray();
                //memoryStream.Dispose();

                //Stream stream = File.OpenWrite(fileName);
                //image.Save(stream, imageFormat);

                //stream.Dispose();
            }
            //#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::SaveImage Error. {0}", ex.Message));
            }
//#endif
            finally { }
        }

        public static void StartSaveImage(Image image, string fileName, ImageFormat imageFormat)
        {
            if (image == null)
                return;

            string directoryName = Path.GetDirectoryName(fileName);
            if (Directory.Exists(directoryName) == false)
                Directory.CreateDirectory(directoryName);

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            memoryStream.Dispose();

            AddSaveQueueData(new SaveQueueData(fileName, imageByte));
        }

        private static void AddSaveQueueData(SaveQueueData saveQueueData)
        {
            lock (saveQueueDataList)
            {
                saveQueueDataList.Add(saveQueueData);
            }
            
            if (saveThread.IsAlive==false)
            //if (saveThread.ThreadState != System.Threading.ThreadState.Running)
                saveThread.Start();
        }

        public static void SaveImage(float[] floatData, int width, int height, string fileName)
        {
            int i, j, j0;
            byte[] byteData = new byte[width * height];

         //   float minValue = float.MaxValue;
	        //float mavValue = float.MinValue;

	        //for(j=0; j< height; j++)
	        //{
		       // j0 = j* width;
		       // for(i=0; i< width; i++)
		       // {
         //           minValue = Math.Min(floatData[j0 + i], minValue);
         //           mavValue = Math.Max(floatData[j0 + i], mavValue);
		       // }
         //   }

            float minValue = floatData.Min();
            float maxValue = floatData.Max();

            float diffValue = Math.Abs(maxValue - minValue);

	        for(j=0; j< height; j++)
	        {
		        j0 = j* width;
		        for(i=0; i< width; i++)
		        {
                    byteData[j0 + i] = (byte)(Math.Abs(floatData[j0 + i] - minValue) / diffValue * 255);
		        }
	        }

            SaveImage(byteData, width, height, fileName);
        }
        
        public static void SaveImage(byte[] byteData, int width, int height, string fileName)
        {
            int stride = width;
            if ((width % 4) != 0)
                stride = width + (4 - width % 4);//4바이트 배수가 아닐시..

            IntPtr imageBuffer = Marshal.AllocHGlobal(byteData.Length);
            Marshal.Copy(byteData, 0, imageBuffer, byteData.Length);

            Bitmap bitmap = new Bitmap(width, height, stride, PixelFormat.Format8bppIndexed, imageBuffer);

            ColorPalette colorPalette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = colorPalette;

            bitmap.Save(fileName);

            bitmap.Dispose();
            Marshal.FreeHGlobal(imageBuffer);
        }

        public static Bitmap Resize(Bitmap original, float scaleX, float scaleY)
        {
            //Debug.Assert(scaleX <= 0 || scaleY <= 0, "Destination is invalid");

            Bitmap resizeImage = new Bitmap((int)(original.Width * scaleX), (int)(original.Height * scaleY), PixelFormat.Format8bppIndexed);
            Rectangle resizeImageRect = new Rectangle(0, 0, resizeImage.Width, resizeImage.Height);
            BitmapData resizeBmpData = resizeImage.LockBits(resizeImageRect, ImageLockMode.ReadWrite, resizeImage.PixelFormat);
            IntPtr resizeImagePtr = resizeBmpData.Scan0;

            int resizeImageSize = resizeBmpData.Stride * resizeBmpData.Height;
            byte[] resizeImageBuffer = new byte[resizeImageSize];

            Rectangle originalImageRect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData originalBmpData = original.LockBits(originalImageRect, ImageLockMode.ReadWrite, resizeImage.PixelFormat);
            IntPtr originalImagePtr = originalBmpData.Scan0;

            int originalImageSize = originalBmpData.Stride * originalBmpData.Height;
            byte[] originalImageBuffer = new byte[originalImageSize];

            System.Runtime.InteropServices.Marshal.Copy(resizeImagePtr, resizeImageBuffer, 0, resizeImageSize);
            System.Runtime.InteropServices.Marshal.Copy(originalImagePtr, originalImageBuffer, 0, originalImageSize);

            float inverseScaleX = 1.0f / scaleX;
            float inverseScaleY = 1.0f / scaleY;

            for (int i = 0; i < resizeImage.Width; i++)
            {
                for (int j = 0; j < resizeImage.Height; j++)
                {
                    resizeImageBuffer[i + j * resizeBmpData.Stride] =
                        originalImageBuffer[(int)(i * inverseScaleX) + (int)(j * inverseScaleY) * originalBmpData.Stride];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resizeImageBuffer, 0, resizeImagePtr, resizeImageSize);

            resizeImage.UnlockBits(resizeBmpData);
            original.UnlockBits(resizeBmpData);

            System.Drawing.Imaging.ColorPalette cp = resizeImage.Palette;
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);
            resizeImage.Palette = cp;

            return resizeImage;
        }


        public static Bitmap ClipImage(Bitmap bitmap, RectangleF clipRect)
        {
            return ClipImage(bitmap, new Rectangle((int)clipRect.X, (int)clipRect.Y, (int)clipRect.Width, (int)clipRect.Height));
        }

        public static Bitmap ClipImage(Bitmap bitmap, Rectangle clipRect)
        {
            Rectangle bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            clipRect = Rectangle.Intersect(bmpRect, clipRect);
            if (bmpRect.Contains(clipRect) == false)
                return null;

            Bitmap cloneBitmap = bitmap.Clone(clipRect, bitmap.PixelFormat);

            return cloneBitmap;
        }
        
        public static Bitmap CloneImage(Bitmap bitmap)
        {
            Rectangle bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            return bitmap.Clone(bmpRect, bitmap.PixelFormat);
        }

        public static Image CloneImage(Image image)
        {
            return (Image)image.Clone();
        }

        public static void Add(Bitmap srcImage, int value)
        {
            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height; index++)
            {
                srcImageBuffer[index] = (byte)(srcImageBuffer[index] + value);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }

        public static Bitmap MakeColor(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    ////create the color object
                    //Color newColor = Color.FromArgb(originalColor, originalColor, originalColor);

                    ////set the new image's pixel to the grayscale version
                    //newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }
        
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            Rectangle rect = new Rectangle(Point.Empty, original.Size);
            Bitmap greyImage = new Bitmap(rect.Width, rect.Height, PixelFormat.Format8bppIndexed);
            
            BitmapData greyData = greyImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            BitmapData originalData = original.LockBits(rect, ImageLockMode.ReadOnly, original.PixelFormat);

            int length = greyData.Stride * greyData.Height;
            int originlength = originalData.Stride * originalData.Height;

            //Parallel.For(0, length, i =>
            //{
            //    CopyMemory(greyData.Scan0 + i, originalData.Scan0 + (i * 3), 1);
            //});

            byte[] greyBuffer = new byte[length];
            //Marshal.Copy(greyData.Scan0, greyBuffer, 0, length);

            byte[] originalBuffer = new byte[originlength];
            Marshal.Copy(originalData.Scan0, originalBuffer, 0, originlength);

            for (int y = 0; y < originalData.Height; y++)
            {
                int originalIndex = y * originalData.Stride;
                int greyIndex = y * greyData.Stride;
                for (int x = 2; x < originalData.Stride; x += 3)
                {
                    greyBuffer[greyIndex + (x / 3)] = (byte)((originalBuffer[originalIndex + x - 2] + originalBuffer[originalIndex + x - 1] + originalBuffer[originalIndex + x]) / 3);
                }
            }
            
            Marshal.Copy(greyBuffer, 0, greyData.Scan0, length);

            original.UnlockBits(originalData);
            greyImage.UnlockBits(greyData);
            
            return greyImage;
        }
        
        public static byte[] GetByte(Bitmap bitmap)
        {
            Rectangle bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bmpData = bitmap.LockBits(bitmapRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr bmpPtr = bmpData.Scan0;
            
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            int imageSize = bmpData.Stride * bmpData.Height;

            byte[] imageData = new byte[imageSize];

            Parallel.For(0, bitmap.Height, y =>
            {
                Marshal.Copy(bmpPtr + y * bmpData.Stride, imageData, y * bmpData.Stride, bmpData.Stride);
            });

            return imageData;
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, byte[] imageData)
        {
            Debug.Assert(imageData != null);

            if (pitch == 0)
            {
                pitch = width * numBand;
                if ((pitch % 4) != 0)
                    pitch = pitch + (4 - pitch % 4);//4바이트 배수가 아닐시..
            }

            //byte[] destImageData = new byte[stride * height];

            Bitmap bmpImage;
            System.Drawing.Imaging.BitmapData bmpData;
            
            if (numBand == 3)
            {
                //pitch = width * numBand;
                bmpImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmpData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                //pitch = bmpData.Stride;
            }
            else
            {
                bmpImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                System.Drawing.Imaging.ColorPalette cp = bmpImage.Palette;
                for (int i = 0; i < 256; i++)
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                bmpImage.Palette = cp;

                bmpData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            }

            //Parallel.For(0, height, y =>
            //{
            //    try
            //    {
            //        int offsetSrc = pitch * y;
            //        int offsetDst = bmpData.Stride * y;
            //        Marshal.Copy(imageData, offsetSrc, IntPtr.Add(bmpData.Scan0, offsetDst), pitch);
            //    }
            //    catch (/*ArgumentOutOfRangeException*/Exception ex)
            //    {
            //        LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::CreateBitmap {0}",ex.GetType().ToString(), ex.Message));
            //    }
            //    //Buffer.BlockCopy(imageData, pitch * y, destImageData, stride * y, pitch);
            //    //Array.Copy(imageData, pitch * y, destImageData, stride * y, pitch);
            //});

            try //수정된 코드
            {
                Parallel.For(0, height, y =>
                {
                    int offsetSrc = pitch * y;
                    int offsetDst = bmpData.Stride * y;
                    Marshal.Copy(imageData, offsetSrc, IntPtr.Add(bmpData.Scan0, offsetDst), pitch);
                });
            }
            catch (/*ArgumentOutOfRangeException*/Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::CreateBitmap {0}", ex.GetType().ToString(), ex.Message));
            }

            //System.Runtime.InteropServices.Marshal.Copy(destImageData, 0, bmpData.Scan0, destImageData.Length);

            bmpImage.UnlockBits(bmpData);
            //Array.Resize(ref destImageData, 0);
            //bmpImage.Save(@"d:\temp\bbb.bmp");
            return bmpImage;
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, IntPtr dataPtr)
        {
            Debug.Assert(dataPtr != null);

            int stride = width * numBand;
            if ((stride % 4) != 0)
                stride = stride + (4 - stride % 4);//4바이트 배수가 아닐시..
            
            Bitmap bmpImage;

            if (numBand == 3)
            {
                bmpImage = new Bitmap(width, height, stride,System.Drawing.Imaging.PixelFormat.Format24bppRgb, dataPtr);
            }
            else
            {
                if (pitch == stride)
                    bmpImage = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, dataPtr);
                else
                {
                    bmpImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    //bmpImage.Save(@"d:\tt.bmp");
                    BitmapData bitmapData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    //Parallel.For(0, bmpImage.Height, i =>
                    for(int i=0; i< bmpImage.Height; i++)
                    {
                        int srcIndex = i * pitch;
                        int dstIndex = i * stride;
                        CopyMemory(dataPtr + srcIndex, bitmapData.Scan0 + dstIndex, (uint)pitch);
                    }
                    //);
                    bmpImage.UnlockBits(bitmapData);
                }

                System.Drawing.Imaging.ColorPalette cp = bmpImage.Palette;
                for (int i = 0; i < 256; i++)
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                bmpImage.Palette = cp;
            }

            return bmpImage;
        }

        public static byte[] ConvertByteBuffer(float[] floatData)
        {
            byte[] byteData = new byte[floatData.Count()];

            float fmin = floatData.Min();
            float fmax = floatData.Max();
            float fdiff = Math.Abs(fmax - fmin);

            if (fdiff > 0)
            {
                Parallel.For(0, floatData.Count(), index =>
                {
                    byteData[index] = Convert.ToByte(Math.Abs(floatData[index] - fmin) / fdiff * 255);
                });
            }
            else
            {
                Array.Clear(byteData, 0, byteData.Length);
            }

            return byteData;
        }

        public static void Clear(Bitmap srcImage, byte value)
        {
            if (srcImage == null)
                return;

            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];
            Array.Clear(srcImageBuffer, 0, srcImageBuffer.Length);
            //System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            if (value != 0)
            {
                for (int yIndex = 0; yIndex < srcImage.Height; yIndex++)
                {
                    for (int xIndex = 0; xIndex < srcImage.Width; xIndex++)
                    {
                        int index = yIndex * srcBmpData.Stride + xIndex;
                        srcImageBuffer[index] = value;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            if (srcImage.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = srcImage.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
            }
        }

        public static void Not(Bitmap srcImage)
        {
            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height * srcImage.Width; index++)
            {
                srcImageBuffer[index] = (byte)(255 - srcImageBuffer[index]);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }
    }
}

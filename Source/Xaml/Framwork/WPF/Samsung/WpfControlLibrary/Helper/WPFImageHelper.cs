﻿using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UniScanWPF.Helper
{
    public class WPFImageHelper : DynMvp.Base.ImageHelper
    {
        public static PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            PixelFormat wpfPixelFormat = PixelFormats.Gray8;

            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    wpfPixelFormat = PixelFormats.Gray8;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    wpfPixelFormat = PixelFormats.Rgb24;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    wpfPixelFormat = PixelFormats.Bgr32;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                    wpfPixelFormat = PixelFormats.Bgra32;
                    break;
            }

            return wpfPixelFormat;
        }

        public static BitmapSource ConvertImage(System.Drawing.Bitmap image)
        {
            var bitmapData = image.LockBits(
            new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);

            var bitmapSource = BitmapSource.Create(
               bitmapData.Width, bitmapData.Height, 96, 96, ConvertPixelFormat(image.PixelFormat), null,
               bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            image.UnlockBits(bitmapData);

            bitmapSource.Freeze();

            return bitmapSource;
        }

        public static void SaveBitmapSource(string filePath, BitmapSource bitmapSource)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();

                string[] split = filePath.Split('.');
                switch(split.Last().ToString().ToUpper())
                {
                    case "JPG":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case "PNG":
                        encoder = new PngBitmapEncoder();
                        break;
                }

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }

        public static BitmapSource LoadBitmapSource(string filePath)
        {
            if (File.Exists(filePath) == false)
                return null;

            try
            {
                BitmapImage bitmapImage = new BitmapImage();// = new BitmapImage(new Uri(filePath));
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.UriSource = new Uri(filePath);
                bitmapImage.EndInit();

                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("WPFImageHelper::{0}", ex.Message));
                return null;
            }
        }
    }
}

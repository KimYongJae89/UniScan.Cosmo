using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF.Base.Helpers
{
    public class ImageHelper
    {
        public static Bitmap BitmapSourceToBitmap(ImageSource source)
        {
            //convert image format
            var src = new System.Windows.Media.Imaging.FormatConvertedBitmap();
            src.BeginInit();
            src.Source = (BitmapSource)source;
            src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            src.EndInit();

            //copy to bitmap
            Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);

            return bitmap;
        }
    }
}

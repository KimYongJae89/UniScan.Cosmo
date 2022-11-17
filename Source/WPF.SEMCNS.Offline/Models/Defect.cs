using DynMvp.Vision;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF.SEMCNS.Offline.Models
{
    public enum DefectType
    {
        PInHole, Dust
    }

    public class Defect
    {
        float _resolution;
        public float Resolution { get => _resolution; }

        BlobRect _blobRect;
        public BlobRect BlobRect { get => _blobRect; }

        Rectangle _inflateRect;
        public Rectangle InflateRect { get => _inflateRect; }

        DefectType _defectType;
        public DefectType DefectType { get => _defectType; }

        ImageSource _image;

        [JsonIgnore]
        public ImageSource Image { get => _image; }

        float _difference;
        public float Difference { get => _difference; }

        [JsonIgnore]
        public double Area { get => _blobRect.Area * (_resolution * _resolution) / 1000000; }
        [JsonIgnore]
        public Rectangle Region { get => Rectangle.Round(_blobRect.BoundingRect); }
        [JsonIgnore]
        public float MajorAxis { get => _blobRect.MaxFeretDiameter * _resolution; }
        [JsonIgnore]
        public float MinorAxis { get => _blobRect.MinFeretDiameter * _resolution; }
        [JsonIgnore]
        public float[] RotateX { get => _blobRect.RotateXArray; }
        [JsonIgnore]
        public float[] RotateY { get => _blobRect.RotateYArray; }

        [JsonIgnore]
        public double SelectedX { get => _image.Width / 2 - _blobRect.BoundingRect.Width / 2; }
        [JsonIgnore]
        public double SelectedY { get => _image.Height / 2 - _blobRect.BoundingRect.Height / 2; }
        [JsonIgnore]
        public double PixelWidth { get => _blobRect.BoundingRect.Width; }
        [JsonIgnore]
        public double PixelHeight { get => _blobRect.BoundingRect.Height; }

        [JsonIgnore]
        public double X { get => _blobRect.BoundingRect.X; }
        [JsonIgnore]
        public double Y { get => _blobRect.BoundingRect.Y; }

        public Defect(BlobRect blobRect, Rectangle inflateRect, ImageSource image, DefectType defectType, float difference, float resolution)
        {
            _blobRect = blobRect;

            _inflateRect = inflateRect;

            _image = image;

            _defectType = defectType;

            _resolution = resolution;

            _difference = difference;
        }

        public void SetImage(ImageSource imageSource)
        {
            _image = new CroppedBitmap(imageSource as BitmapSource, new Int32Rect(_inflateRect.X, _inflateRect.Y, _inflateRect.Width, _inflateRect.Height));
        }
    }
}

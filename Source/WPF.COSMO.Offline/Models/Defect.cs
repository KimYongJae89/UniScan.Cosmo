using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Models
{
    public enum SpecUnit
    {
        um, mm
    }

    public class UnitItem : Attribute
    {
        private SpecUnit _unit;
        public SpecUnit Unit => _unit;

        public UnitItem(SpecUnit unit)
        {
            _unit = unit;
        }
    }

    public interface IDirectionDefect
    {
        ScanDirection ScanDirection { get; set; }
    }

    public interface IDistanceDefect
    {
        double Distance { get; set; }
    }

    public class Defect
    {
        [JsonIgnore]
        public ImageSource Image { get; set; }

        public string ImagePath { get; set; }

        public byte Min { get; set; }
        public byte Max { get; set; }
        public double Mean { get; set; }

        public double Major { get; set; }
        public double Minor { get; set; }

        public double Angle { get; set; }

        public System.Windows.Point[] Points { get; set;}

        public uint pixelArea { get; set; }

        public PointF CenterPt { get; set; }

        public int Index { get; set; }

        [JsonIgnore]
        public double Area => pixelArea / AxisGrabService.Settings.Resolution / AxisGrabService.Settings.Resolution;

        private static double GetUnitValue(double value)
        {
            var attributes = Attribute.GetCustomAttributes(value.GetType());

            foreach (Attribute attribute in attributes)
            {
                if (attribute is UnitItem)
                {
                    var item = attribute as UnitItem;
                    switch (item.Unit)
                    {
                        case SpecUnit.um:
                            return value;
                        case SpecUnit.mm:
                            return value * 1000;
                    }
                }
            }

            return value;
        }

        public Defect()
        {

        }
        
        protected Defect(int index, BitmapSource _image, double[] _rotateXArray, double[] _rotateYArray, uint _pixelArea, PointF _centerPt, 
            byte min, byte max, double mean, double major, double minor, double angle)
        {
            Index = index;
            Image = _image;
            Points = new System.Windows.Point[4];
            for (int i = 0; i < 4; i++)
                Points[i] = (new System.Windows.Point(_rotateXArray[i], _rotateYArray[i]));
            
            pixelArea = _pixelArea;
            CenterPt = _centerPt;

            Min = min;
            Max = max;
            Mean = mean;

            Major = major;
            Minor = minor;
            Angle = angle;
        }
    }

    public class InnerDefect : Defect, IDistanceDefect
    {
        double _distance;
        public double Distance { get => _distance; set => _distance = value; }

        public InnerDefect()
        {

        }

        public InnerDefect(int index, BitmapSource image, double[] rotateXArray, double[] rotateYArray, uint pixelArea, PointF centerPt, byte min, byte max, double mean, double major, double minor, double angle, double distance)
            : base(index, image, rotateXArray, rotateYArray, pixelArea, centerPt, min, max, mean, major, minor, angle)
        {
            _distance = distance;
        }
    }

    public class SectionDefect : Defect, IDirectionDefect, IDistanceDefect
    {
        ScanDirection _scanDirection;
        public ScanDirection ScanDirection { get => _scanDirection; set => _scanDirection = value; }

        double _distance;
        public double Distance { get => _distance; set => _distance = value; }

        public SectionDefect()
        {

        }

        public SectionDefect(int index, ScanDirection scanDirection, BitmapSource image, double[] rotateXArray, double[] rotateYArray, uint pixelArea, PointF centerPt, byte min, byte max, double mean, double major, double minor, double angle, double distance)
            : base(index, image, rotateXArray, rotateYArray, pixelArea, centerPt, min, max, mean, major, minor, angle)
        {
            _scanDirection = scanDirection;
            _distance = distance;
        }
    }

    public class EdgeDefect : Defect, IDirectionDefect
    {
        //fake
        [JsonIgnore]
        public double Distance { get; set; }

        ScanDirection _scanDirection;
        public ScanDirection ScanDirection { get => _scanDirection; set => _scanDirection = value; }

        public EdgeDefect()
        {

        }

        public EdgeDefect(int index, ScanDirection scanDirection, BitmapSource image, double[] rotateXArray, double[] rotateYArray, uint pixelArea, PointF centerPt, byte min, byte max, double mean, double major, double minor, double angle)
            : base(index, image, rotateXArray, rotateYArray, pixelArea, centerPt, min, max, mean, major, minor, angle)
        {
            _scanDirection = scanDirection;
        }
    }
}

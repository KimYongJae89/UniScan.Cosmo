using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP.SEMCNS.Models
{
    public class Defect
    {
        //Pixel
        uint _area;
        uint _width;
        uint _height;

        Rect _region;
        public Rect Region { get => _region; }

        WriteableBitmap _image;

        //UM
        double _resolution;

        //Gray
        double _difference;

        //Real
        public double Area { get => _area * (_resolution * _resolution); }
        public double Width { get => _width * _resolution; }
        public double Height { get => _height * _resolution; }
        public double Difference { get => _difference; }
        public WriteableBitmap Image { get => _image; }

        public Defect(WriteableBitmap image, uint area, uint width, uint height, Rect region, double resolution, double difference)
        {
            _image = image;

            _area = area;
            _width = width;
            _height = height;
           
            _region = region;

            _resolution = resolution;

            _difference = difference;
        }
    }
}

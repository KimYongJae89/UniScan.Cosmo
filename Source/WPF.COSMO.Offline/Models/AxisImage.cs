using DynMvp.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF.COSMO.Offline.Models
{
    public class AxisImageSource
    {
        public double X { get; set; }
        public double Y { get; set; }

        [JsonIgnore]
        public ImageSource ImageSource { get; set; }

        public AxisImageSource()
        {

        }

        public AxisImageSource(Point axisPosition, ImageSource imageSource)
        {
            X = axisPosition.X;
            Y = axisPosition.Y;

            ImageSource = imageSource;
        }
    }

    public class AxisImage
    {
        double _x;
        double _y;

        public double X { get => _x; }
        public double Y { get => _y; }

        byte[] _data;
        public byte[] Data { get => _data; }

        public AxisImage(byte[] data, double x, double y)
        {
            _data = data;

            _x = x;
            _y = y;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Base.Models
{
    public class LineModel
    {
        public double _accuracy;
        public double Accuracy => _accuracy;

        double _gradient;
        public double Gradient => _gradient;

        double _centerX;
        public double CenterX => _centerX;

        double _centerY;
        public double CenterY => _centerY;

        public double GetX(double yPos)
        {
            return CenterX + ((yPos - CenterY) / Gradient);
        }

        public double GetY(double xPos)
        {
            return CenterY + ((xPos - CenterX) * Gradient);
        }

        public LineModel(double radian, double centerX, double centerY)
        {
            _gradient = radian;
            _centerX = centerX;
            _centerY = centerY;
        }

        public LineModel(double accuracy, double radian, double centerX, double centerY)
        {
            _accuracy = accuracy;
            _gradient = radian;
            _centerX = centerX;
            _centerY = centerY;
        }

        public LineModel GetParallelLine(double dist)
        {
            var radian = Math.Atan(_gradient);

            var x = _gradient > 0 ? Math.Sin(radian) * dist : -Math.Sin(radian) * dist;
            var y = _gradient > 0 ? Math.Cos(radian) * dist : -Math.Cos(radian) * dist;

            return new LineModel(_accuracy, _gradient, _centerX + x, _centerY - y);
        }

        public PointF GetIntersectPoint(LineModel lineModel)
        {
            double b1 = CenterY - (CenterX * Gradient);
            double b2 = lineModel.CenterY - (lineModel.CenterX * lineModel.Gradient);
            
            double x = (b2 - b1) / (_gradient - lineModel.Gradient);
            double y = x * _gradient + b1;

            return new PointF((float)x, (float)y);
        }

        public double GetDistance(PointF point)
        {
            //결과 같음 - 빗변 * 높이
            //double xDist = Math.Abs(GetX(point.Y) - point.X);
            //double yDist = Math.Abs(GetY(point.X) - point.Y);

            //double hypotenuse = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            //double dist = xDist * yDist / hypotenuse;

            double lower = Math.Sqrt(Math.Pow(_gradient, 2) + 1);
            double dist = Math.Abs(((point.X - CenterX) * Gradient) + CenterY - point.Y) / lower;

            return dist;
        }

        public PointF GetDistPt(double dist)
        {
            var radian = Math.Atan(_gradient);

            var x = CenterX + (_gradient > 0 ? Math.Cos(radian) * dist : -Math.Cos(radian) * dist);
            var y = CenterY + (_gradient > 0 ? Math.Sin(radian) * dist : -Math.Sin(radian) * dist);

            return new PointF((float)x, (float)y);
        }
    }
}

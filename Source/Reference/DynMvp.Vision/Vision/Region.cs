using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Vision
{
    public interface Region
    {
    }

    public class RectangleRegion : Region
    {
        RectangleF rectangle;
        public float X
        {
            get { return rectangle.X; }
            set { rectangle.X = value; }
        }
        public float Y
        {
            get { return rectangle.Y; }
            set { rectangle.Y = value; }
        }
        public float Width
        {
            get { return rectangle.Width; }
            set { rectangle.Width = value; }
        }
        public float Height
        {
            get { return rectangle.Height; }
            set { rectangle.Height = value; }
        }

        float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}

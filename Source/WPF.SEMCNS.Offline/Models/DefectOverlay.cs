using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF.SEMCNS.Offline.Models
{
    public class DefectLine
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
    }
    
    public class DefectOverlay
    {
        public Rectangle Rect { get; set; }
        public DefectLine[] Lines { get; set; }
        public PointCollection PointCollection { get; set; }
    }
}

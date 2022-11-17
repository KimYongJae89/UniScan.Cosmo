using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanG.Temp
{
    public partial class DefectImageViewer : Form
    {
        const float calibrationValue = 10;

        public DefectImageViewer()
        {
            InitializeComponent();
        }

        public void UpdateDefectInfo(Image image, float x, float y, float width, float height)
        {
            defectImage.Image = image;
            defectX.Text = String.Format("{0:.0} mm", x.ToString());
            defectY.Text = String.Format("{0:.0} mm", y.ToString());
            defectWidth.Text = String.Format("{0:0} um", width.ToString());
            defectHeight.Text = String.Format("{0:0} um", height.ToString());
        }
    }
}

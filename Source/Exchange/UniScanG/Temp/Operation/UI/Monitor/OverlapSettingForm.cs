using DynMvp.Data.UI;
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
    public partial class OverlapSettingForm : Form
    {
        DrawBox leftImage = new DrawBox();
        DrawBox rightImage = new DrawBox();
        DrawBox overlapImage = new DrawBox();

        int overlapAreaPx = 0;

        bool onUpdate = false;

        public OverlapSettingForm()
        {
            InitializeComponent();

            leftImage.Dock = DockStyle.Fill;
            this.panelImageLeft.Controls.Add(leftImage);

            rightImage.Dock = DockStyle.Fill;
            this.panelImageRight.Controls.Add(rightImage);

            overlapImage.Dock = DockStyle.Fill;
            this.panelImageOverlap.Controls.Add(overlapImage);

            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = 17824;
            hScrollBar1.SmallChange = 10;
            hScrollBar1.LargeChange = 100;

            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 17824;

        }

        public void Init(Image left, Image right, MonitorInfo monitorInfo)
        {
            leftImage.UpdateImage(left);
            rightImage.UpdateImage(right);
            overlapAreaPx = monitorInfo.OverlapAreaPx;
        }

        private void OverlapSettingForm_Load(object sender, EventArgs e)
        {
            UpdateOverlapImage();
        }

        private void UpdateData()
        {
            onUpdate = true;

            hScrollBar1.Value = overlapAreaPx;
            numericUpDown1.Value = overlapAreaPx;

            onUpdate = false;
        }

        private void UpdateOverlapImage()
        {
            throw new NotImplementedException();
        }
    }
}

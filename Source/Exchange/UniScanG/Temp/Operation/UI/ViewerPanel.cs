using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;

namespace UniScanG.Temp
{
    public partial class ViewerPanel : UserControl
    {
        DrawBox drawBox = null;

        public ViewerPanel()
        {
            InitializeComponent();

            this.drawBox = new DrawBox();

            this.panelCamera.Controls.Add(this.drawBox);

            this.drawBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.drawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawBox.Location = new System.Drawing.Point(3, 3);
            this.drawBox.Name = "DrawBox";
            this.drawBox.Size = new System.Drawing.Size(409, 523);
            this.drawBox.TabIndex = 8;
            this.drawBox.TabStop = false;
            this.drawBox.Enable = false;
            this.drawBox.CoordScaleX = 0.1f;
            this.drawBox.CoordScaleY = 0.1f;

            //this.drawBox.MouseMoved += DrawBoxMouseMoved;
            //this.drawBox.pictureBox.KeyUp += PictureBoxMouseKeyUped;
        }

        public void SetHeaderTitle(string title)
        {
            labelHeaderTitle.Text = title;
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.UI;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.UI;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Settings.UI;

namespace UniScan.Monitor.Settings.Monitor.UI
{
    public partial class InspectorFovPanel : UserControl
    {
        bool onUpdateData = false;

        InspectorInfo inspectorInfo = null;
        public InspectorInfo InspectorInfo
        {
            get { return inspectorInfo; }
        }

        Image2D image;
        public Image2D Image
        {
            get { return image; }
        }

        CanvasPanel canvasPanel = new CanvasPanel();

        public InspectorFovPanel(InspectorInfo inspectorInfo)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.inspectorInfo = inspectorInfo;

            canvasPanel.DragMode = DragMode.Pan;
            canvasPanel.ShowCenterGuide = false;
            
            canvasPanel.Dock = DockStyle.Fill;
            panelImage.Controls.Add(canvasPanel);
        }

        ~InspectorFovPanel()
        {
            Release();
        }

        public delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            if (onUpdateData == true)
                return;

            onUpdateData = true;

            offsetX.Value = (Decimal)inspectorInfo.Fov.X;
            offsetY.Value = (Decimal)inspectorInfo.Fov.Y;
            width.Value = (Decimal)inspectorInfo.Fov.Width;
            height.Value = (Decimal)inspectorInfo.Fov.Height;

            UpdateRegion();

            onUpdateData = false;
        }

        private void UpdateRegion()
        {
            canvasPanel.TempFigures.Clear();

            int fidgureWidth = (int)(5);
            canvasPanel.TempFigures.AddFigure(new RectangleFigure(inspectorInfo.Fov, new Pen(Color.Green, fidgureWidth)));

            canvasPanel.Invalidate(true);
        }

        public void UpdateRegion(Rectangle rect)
        {
            canvasPanel.TempFigures.Clear();
            int fidgureWidth = (int)(5);
            canvasPanel.TempFigures.AddFigure(new RectangleFigure(rect, new Pen(Color.Red, fidgureWidth)));
            
            canvasPanel.Invalidate(true);
        }

        private void Release()
        {
            if (image != null)
                image.Dispose();
        }
        
        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path.Combine(this.inspectorInfo.Path, "Model");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Release();

                image = new Image2D(dlg.FileName);
                Bitmap bitmap = null;
                if (image.Height > 30120)
                    bitmap = image.ClipImage(new Rectangle(0, 0, image.Width, 30120)).ToBitmap();
                else
                    bitmap = image.ToBitmap();
                canvasPanel.UpdateImage(bitmap);

                UpdateData();
            }
        }

        private void offsetX_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            inspectorInfo.Fov = new RectangleF((float)offsetX.Value, inspectorInfo.Fov.Y, inspectorInfo.Fov.Width, InspectorInfo.Fov.Height);

            UpdateRegion();
        }

        private void offsetY_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            inspectorInfo.Fov = new RectangleF(inspectorInfo.Fov.X, (float)offsetY.Value, inspectorInfo.Fov.Width, InspectorInfo.Fov.Height);

            UpdateRegion();
        }

        private void width_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            inspectorInfo.Fov = new RectangleF(inspectorInfo.Fov.X, inspectorInfo.Fov.Y, (float)width.Value, InspectorInfo.Fov.Height);

            UpdateRegion();
        }

        private void height_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            inspectorInfo.Fov = new RectangleF(inspectorInfo.Fov.X, inspectorInfo.Fov.Y, inspectorInfo.Fov.Width, (float)height.Value);

            UpdateRegion();
        }
    }
}

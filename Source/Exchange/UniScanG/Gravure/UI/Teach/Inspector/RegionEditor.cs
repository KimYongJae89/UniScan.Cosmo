using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Data;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public delegate void ApplyAllDelegate(RegionInfoG regionInfo);
    public partial class RegionEditor : UserControl
    {
        public RegionInfoG RegionInfo
        {
            get { return this.regionInfo; }
            set {
                this.regionInfo = value;
                this.regionInfoWork = (RegionInfoG)value.Clone();
            }
        }

        public ImageD TrainImage { get { return trainImage; } set { trainImage = value; } }
        public ImageD MajorPatternImage { get { return majorPatternImage; } set { majorPatternImage = value; } }

        public Direction SplitLineDirection
        {
            get { return this.splitLineDirection; }
            set { this.splitLineDirection = value; }
        }

        public ApplyAllDelegate ApplyAll
        {
            get { return this.ApplyAllDelegate; }
            set { this.ApplyAllDelegate = value; }
        }

        ImageD trainImage;
        ImageD majorPatternImage;
        RegionInfoG regionInfo = null;
        RegionInfoG regionInfoWork = null;
        Direction splitLineDirection;
        ApplyAllDelegate ApplyAllDelegate = null;

        CanvasPanel canvasPanel = null;

        public RegionEditor(ImageD trainImage, ImageD majorPatternImage)
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.DragMode = DragMode.Pan;
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.MouseClicked = canvasPanel_MouseClicked;
            this.panelImage.Controls.Add(this.canvasPanel);

            this.trainImage = trainImage;
            this.majorPatternImage = majorPatternImage;
        }

        private void canvasPanel_MouseClicked(PointF point, ref bool processingCancelled)
        {
            for (int y = 0; y < this.regionInfoWork.AdjPatRegionList.GetLength(0); y++)
            {
                for (int x = 0; x < this.regionInfoWork.AdjPatRegionList.GetLength(1); x++)
                {
                    Rectangle rect = this.regionInfoWork.AdjPatRegionList[y, x];
                    if (rect.Contains(Point.Round(point)))
                    {
                        Point pt = new Point(x, y);
                        int idx = this.regionInfoWork.DontcareLocationList.FindIndex(f => f.Equals(pt));

                        if (idx<0)
                            this.regionInfoWork.DontcareLocationList.Add(pt);
                        else
                            this.regionInfoWork.DontcareLocationList.RemoveAt(idx);

                        break;
                    }
                }
            }
            UpdateFigure();
        }

        private void RegionEditor_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = this.regionInfoWork.AdjPatRegionList.GetLength((int)this.splitLineDirection);

            UpdateData();
            UpdateImage();
            UpdateFigure();

            if (SystemManager.Instance().CurrentModel != null)
                SystemManager.Instance().CurrentModel.Modified = true;
        }

        private ImageD GetCurrentImage()
        {
            if (showTrainImage.Checked)
                return this.trainImage;
            else if (showPatternImage.Checked)
                return this.majorPatternImage;

            return null;
        }

        private void UpdateImage()
        {
            ImageD currentImageD = GetCurrentImage();
            Bitmap bitmap = currentImageD?.ToBitmap();
            this.canvasPanel.UpdateImage(bitmap);
            //bitmap?.Dispose();
        }

        private void UpdateData()
        {
            numericUpDown1.Value = this.regionInfoWork.LinePair;
            oddEvenPair.Checked = this.regionInfoWork.OddEvenPair;
        }

        private void UpdateFigure()
        {
            this.canvasPanel.WorkingFigures.Clear();

            Figure figure = regionInfoWork.GetFigure();
            this.canvasPanel.WorkingFigures.AddFigure(figure);
        }

        private void RegionEditor_SizeChanged(object sender, EventArgs e)
        {
            this.canvasPanel.ZoomFit();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (this.regionInfoWork.LinePair != (int)numericUpDown1.Value)
            {
                this.regionInfoWork.LinePair = (int)numericUpDown1.Value;
                this.regionInfoWork.BuildInspRegion(this.trainImage, this.majorPatternImage, this.splitLineDirection);
                UpdateFigure();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Apply();
            MessageForm.Show(null, "Apply Success");
        }

        private void Apply()
        {
            //this.regionInfo.Dispose();
            
            this.regionInfo = (RegionInfoG)this.regionInfoWork.Clone();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
            MessageForm.Show(null, "Reset Success");
            UpdateData();
            UpdateFigure();
        }

        private void Reset()
        {
            this.regionInfoWork = (RegionInfoG)this.regionInfo.Clone();
        }

        private void buttonApplyAll_Click(object sender, EventArgs e)
        {
            Apply();
            ApplyAllDelegate(this.regionInfo);
            MessageForm.Show(null, "Apply All Success");
        }

        private void Image_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "BITMAP(*.bmp) Files|*.bmp|JPEG(*.jpg) Files|*.jpg";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                this.canvasPanel.Image.Save(dlg.FileName);
            }
        }

        private void loadImage_Click(object sender, EventArgs e)
        {
            ImageD currentImage = GetCurrentImage();
            if (currentImage == null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BITMAP(*.bmp) Files|*.bmp|JPEG(*.jpg) Files|*.jpg";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            ImageD loadImage = new Image2D();
            loadImage.LoadImage(dlg.FileName);

            Rectangle currentImageRect = new Rectangle(Point.Empty, currentImage.Size);
            Rectangle loadImageRect = new Rectangle(Point.Empty, loadImage.Size);
            AdjustSize(ref currentImageRect, ref loadImageRect);

            currentImage.Clear();
            currentImage.CopyFrom(loadImage, loadImageRect, loadImage.Pitch, currentImageRect.Location);

            UpdateImage();
        }

        private void AdjustSize(ref Rectangle rect1, ref Rectangle rect2)
        {
            PointF center1 = DrawingHelper.CenterPoint(rect1);
            PointF center2 = DrawingHelper.CenterPoint(rect2);
            Size minSize = new Size(Math.Min(rect1.Width, rect2.Width), Math.Min(rect1.Height, rect2.Height));

            rect1 = Rectangle.Round(DrawingHelper.FromCenterSize(center1, minSize));
            rect2 = Rectangle.Round(DrawingHelper.FromCenterSize(center2, minSize));
        }

        private void oddEvenPair_CheckedChanged(object sender, EventArgs e)
        {
            this.regionInfoWork.OddEvenPair = oddEvenPair.Checked;
        }

    }
}

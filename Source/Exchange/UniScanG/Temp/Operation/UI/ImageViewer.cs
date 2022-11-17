//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using DynMvp.InspData;
//using DynMvp.UI;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace UniScanG.Temp
//{
//    //public delegate void ImageViewerCloseDelegate();
//    public partial class ImageViewer : Form
//    {
//        InspectionResult inspectionResult = null;

//        DrawBox camView;
//        public DrawBox CamView
//        {
//            get { return camView; }
//        }

//        private const int padding = 3;
//        DefectImageViewer defectImageViewer = new DefectImageViewer();
//        //public ImageViewerCloseDelegate imageViewerCloseDelegate;
//        public ImageViewer()
//        {
//            InitializeComponent();

//            camView = new DrawBox();

//            this.camViewPanel.ClientArea.Controls.Add(this.camView);

//            this.camView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.camView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.camView.Location = new System.Drawing.Point(3, 3);
//            this.camView.Name = "camView";
//            this.camView.Size = new System.Drawing.Size(409, 523);
//            this.camView.TabIndex = 8;
//            this.camView.TabStop = false;
//            this.camView.Enable = false;
//            this.camView.MouseClicked += CamView_MouseClicked;
//            this.camView.MouseMoved += CamView_MouseMoved;
//            this.camView.pictureBox.MouseLeave += PictureBox_MouseLeaveClient;
//            lastDefectView.RowTemplate.Height = lastDefectView.Height / 5;
//        }

//        private void CamView_MouseMoved(DrawBox senderView, Point movedPos, Image image, MouseEventArgs e, ref bool processingCancelled)
//        {
//            if (lastDefectView.SelectedRows.Count == 0)
//                return;
            
//            const float validBound = 500;
//            float minDistance = float.MaxValue;
//            int minIndex = 0;
//            int index = -1;
//            Data.SheetCheckerSubResult minDistanceSheetCheckerSubResult = null;
//            foreach (DataGridViewRow dataGridViewRow in lastDefectView.Rows)
//            {
//                Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)(dataGridViewRow.Tag);
//                PointF centerPt = new PointF(sheetCheckerSubResult.DefectBlob.CenterPt.X, sheetCheckerSubResult.DefectBlob.CenterPt.Y);

//                float lenght = MathHelper.GetLength(centerPt, movedPos);
//                if (lenght < minDistance)
//                {
//                    minDistance = lenght;
//                    minIndex = dataGridViewRow.Index;
//                    minDistanceSheetCheckerSubResult = sheetCheckerSubResult;
//                }

//                index++;
//            }

//            if (minDistance < validBound)
//            {
//                defectImageViewer.UpdateDefectInfo(minDistanceSheetCheckerSubResult.Image, (int)minDistanceSheetCheckerSubResult.X,
//                        (int)minDistanceSheetCheckerSubResult.Y, (int)minDistanceSheetCheckerSubResult.Width,
//                        (int)minDistanceSheetCheckerSubResult.Height);

//                Point location = PointToScreen(new Point(e.X + splitContainer1.Location.X + 10, e.Y + splitContainer1.Location.Y + 10));

//                defectImageViewer.Location = location;
//                defectImageViewer.Show();
//            }
//            else
//            {
//                defectImageViewer.Hide();
//            }
//            //TextFigure textFigure = new TextFigure() 
//        }

//        private void CamView_MouseClicked(DrawBox senderView, Point clickPos, ref bool processingCancelled)
//        {
//            if (lastDefectView.SelectedRows.Count > 0)
//            {
//                const float validBound = 500;

//                Operation.Data.SheetCheckerSubResult sheetCheckerSubResult = (Operation.Data.SheetCheckerSubResult)lastDefectView.Rows[lastDefectView.SelectedRows[0].Index].Tag;
//                PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.DefectBlob.BoundingRect);

//                float length = MathHelper.GetLength(centerPt, clickPos);
//                if (length < validBound)
//                {
//                    camView.pictureBox.Width = camView.pictureBox.Image.Width;// / sheetCheckerSubResult.Bitmap.Width;
//                    camView.pictureBox.Height = camView.pictureBox.Image.Height;// / sheetCheckerSubResult.Bitmap.Height;

//                    Rectangle resultRect = sheetCheckerSubResult.ResultRect.ToRectangle();
//                    resultRect.Inflate(25, 25);

//                    HScrollProperties hScroll = camView.HorizontalScroll;
//                    VScrollProperties vScroll = camView.VerticalScroll;
//                    hScroll.Value = (int)MathHelper.Bound(centerPt.X - camView.Size.Width / 2, hScroll.Minimum, hScroll.Maximum);
//                    vScroll.Value = (int)MathHelper.Bound(centerPt.Y - camView.Size.Height / 2, vScroll.Minimum, vScroll.Maximum);
//                    camView.DrawEllipse(resultRect);
//                    camView.ZoomScale = new SizeF(1,1);
//                    //teachBox.DrawCrossLine(centerPt);

//                    //defectPicture.Image = sheetCheckerSubResult.Bitmap;
//                }
//            }
//        }

//        private void buttonZoomIn_Click(object sender, EventArgs e)
//        {
//            camView.ZoomIn();
//        }

//        private void buttonZoomOut_Click(object sender, EventArgs e)
//        {
//            camView.ZoomOut();
//        }

//        private void buttonZoomFit_Click(object sender, EventArgs e)
//        {
//            camView.ZoomFit();
//            ShowResult();
//        }

//        private void ShowResult()
//        {
//            camView.TempFigureGroup.Clear();

//            if (inspectionResult != null)
//            {
//                inspectionResult.AppendResultFigures(camView.TempFigureGroup);
//            }

//            camView.Update();
//            camView.Invalidate(false);
//        }

//        private void ImageViewer_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            //if (imageViewerCloseDelegate != null)
//            //imageViewerCloseDelegate();
//            e.Cancel = true;
//            Hide();
//        }

//        public void DefectUpdate(int seqNum, InspectionResult inspectionResult)
//        {
//            this.inspectionResult = inspectionResult;
//            List<AlgorithmResult> subResultList = ((VisionProbeResult)(inspectionResult.ProbeResultList[0])).AlgorithmResult.SubResultList;
//            foreach (Data.SheetCheckerSubResult subResult in subResultList)
//            {
//                if (subResult == null)
//                    continue;
//                if (subResult == null)
//                    continue;

//                string defectType = null;
//                switch (subResult.DefectType)
//                {
//                    case Data.SheetDefectType.BlackDefect:
//                        defectType = "전극";
//                        break;
//                    case Data.SheetDefectType.WhiteDefect:
//                        defectType = "성형";
//                        break;
//                }

//                int rowIndex = 0;

//                if (lastDefectView.Rows.Count == 0)
//                    rowIndex = lastDefectView.Rows.Add(defectType, null, subResult.Image);
//                else
//                    lastDefectView.Rows.Insert(0, defectType, null, subResult.Image);
                
//                lastDefectView.Rows[0].Tag = subResult;
//            }

//            index.Text = String.Format("Index : {0}", seqNum);
//            total.Text = String.Format("Total : {0}", subResultList.Count);
//        }

//        public void Clear()
//        {
//            lastDefectView.Rows.Clear();
//        }

//        private void lastDefectView_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (lastDefectView.SelectedRows.Count > 0)
//            {
//                Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView.Rows[lastDefectView.SelectedRows[0].Index].Tag;
//                PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//                Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//                        (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//                resultRect.Inflate(500, 500);
//                camView.TempFigureGroup.Clear();
//                camView.DrawEllipse(resultRect);
//            }
//        }

//        private void lastDefectView_Paint(object sender, PaintEventArgs e)
//        {
//            Brush defectBrush = new SolidBrush(Color.Black);
//            Font font = new Font("Arial", 12);
//            StringFormat stringFormat = new StringFormat();
//            stringFormat.Alignment = StringAlignment.Near;
//            stringFormat.LineAlignment = StringAlignment.Near;

//            Brush typeBrush = new SolidBrush(Color.Blue);

//            for (int i = 0; i < lastDefectView.Rows.Count; i++)
//            {
//                if (lastDefectView.Rows[i].Cells[0].Displayed == true)
//                {
//                    Rectangle cellRect = lastDefectView.GetCellDisplayRectangle(1, i, false);
//                    Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView.Rows[i].Tag;
//                    if (sheetCheckerSubResult != null)
//                    {
//                        e.Graphics.DrawString(sheetCheckerSubResult.Message, font, defectBrush, new Point(cellRect.Left + padding, cellRect.Top + padding * 10), stringFormat);

//                        Color color = new Color();
//                        switch (sheetCheckerSubResult.DefectType)
//                        {
//                            case Data.SheetDefectType.BlackDefect:
//                                color = Color.Red;
//                                break;
//                            case Data.SheetDefectType.WhiteDefect:
//                                color = Color.Yellow;
//                                break;
//                        }

//                        lastDefectView.Rows[i].Cells[0].Style.BackColor = color;
//                    }
                        
//                    if (string.IsNullOrEmpty(sheetCheckerSubResult.ShortResultMessage) == false)
//                        e.Graphics.DrawString(sheetCheckerSubResult.ShortResultMessage, font, typeBrush, new Point(cellRect.Left + padding, cellRect.Bottom - padding * 12), stringFormat);
//                }
//            }
//        }

//        private void PictureBox_MouseLeaveClient(object sender, EventArgs e)
//        {
//            defectImageViewer.Hide();
//        }

//        private void ImageViewer_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}

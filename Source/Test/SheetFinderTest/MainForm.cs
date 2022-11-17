using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Inspect;
using UniScanG.Vision;

namespace SheetFinderTest
{
    public delegate void PracticeGrabbedDelegate(ImageD foundImage, Size size,int id, object tag);
    public partial class MainForm : Form
    {
        Practice practice = null;
        CanvasPanel canvasPanelF = new CanvasPanel();
        CanvasPanel canvasPanelS = new CanvasPanel();
        Image2D image2D = new Image2D();
        int boundSizeHalf = -1;

        public MainForm()
        {
            InitializeComponent();

            canvasPanelF = new CanvasPanel();
            canvasPanelF.Dock = DockStyle.Fill;
            panel2.Controls.Add(canvasPanelF);

            canvasPanelS = new CanvasPanel();
            canvasPanelS.Dock = DockStyle.Fill;
            panel3.Controls.Add(canvasPanelS);

            frameHeight.Minimum = 0;
            frameHeight.Maximum = int.MaxValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (practice == null)
            {
                ClearResult();

                practice = new Practice();
                practice.FrameGrabbed = practice_FrameGrabbed;
                practice.SheetFound = practice_SheetFound;

                SheetFinderV2Param param = new SheetFinderV2Param { ProjectionBinalizeMul = (float)this.thresMul.Value, BlankLengthMul = (float)this.lengthMul.Value };
                practice.Start(this.image2D, param, (int)boundarySize.Value, (int)frameHeight.Value);
            }
            else
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() =>
                {
                    practice.Stop();
                });
                practice = null;
            }
        }

        private void practice_FrameGrabbed(ImageD foundImage, Size size, int frameId, object tag)
        {
            Bitmap bitmap = foundImage.Resize(0.1f).ToBitmap();
            //Directory.CreateDirectory(@"d:\temp\");
            //bitmap.Save(string.Format(@"d:\temp\Frame{0}.bmp", frameId));
            canvasPanelF.UpdateImage(bitmap);
            canvasPanelF.ZoomFit();
        }

        private void practice_SheetFound(ImageD foundImage, Size size,int sheetNo, object tag)
        {
            //canvasPanelS.UpdateImage(foundImage.ToBitmap());
            //canvasPanelS.ZoomFit();
            AddResult(foundImage, size, sheetNo, tag);
        }


        private delegate void ClearResultDelegate();
        private void ClearResult()
        {
            if(InvokeRequired)
            {
                Invoke(new ClearResultDelegate(ClearResult));
                return;
            }
            dataGridView.Rows.Clear();
        }

        private void AddResult(ImageD imageD, Size size, int sheetId,object tag)
        {
            if (InvokeRequired)
            {
                Invoke(new PracticeGrabbedDelegate(AddResult), imageD, size,sheetId, tag);
                return;
            }
            
            List<int> list = tag as List<int>;
            string joinString = string.Join(", ", list.ToArray());

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView);
            row.Cells[0].Value = sheetId;
            row.Cells[1].Value = size.Height;
            row.Cells[2].Value = joinString;
            //row.Tag = imageD.ToBitmap();

            dataGridView.Rows.Add(row);
            
            Bitmap bitmap = imageD.ToBitmap();
            canvasPanelS.UpdateImage(bitmap);
            canvasPanelS.ZoomFit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BITMAP(*.BMP)|*.bmp|ALL(*.*|*.*)";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    this.image2D.LoadImage(dlg.FileName);
                    Bitmap bitmap = null;
                    if (this.image2D.Height < 30120)
                        bitmap = this.image2D.ToBitmap();
                    else
                        bitmap = this.image2D.ClipImage(new Rectangle(0, 0, this.image2D.Width, 30120)).ToBitmap();

                    canvasPanelF.UpdateImage(bitmap);
                    canvasPanelF.ZoomFit();
                    canvasPanelF.Invalidate();
                });

                frameHeight.Value = this.image2D.Height;
            }
        }
        
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            boundSizeHalf = (int)boundarySize.Value;
        }
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            canvasPanelF.ZoomFit();
            canvasPanelS.ZoomFit();
        }
    }
}

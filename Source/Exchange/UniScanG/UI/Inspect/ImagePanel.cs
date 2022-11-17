using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.UI;
using UniScanG.Screen.Vision.Detector;
using UniScanG.UI.Etc;

namespace UniScanG.UI.Inspect
{
    public partial class ImagePanel : UserControl, IMultiLanguageSupport
    {
        CanvasPanel canvasPanel;

        ContextInfoForm contextInfoForm = new ContextInfoForm();

        public ImagePanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.DragMode = DragMode.Pan;
            canvasPanel.FigureFocused = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            canvasPanel.MouseClicked = CanvasPanel_MouseClicked;
            image.Controls.Add(canvasPanel);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelImage.Text = StringManager.GetString(this.GetType().FullName, labelImage.Text);
        }

        public void UpdateResult(Bitmap image, List<DataGridViewRow> dataGridViewRowList)
        {
            canvasPanel.WorkingFigures.Clear();

            float ratio = SystemTypeSettings.Instance().ResizeRatio;
            if (dataGridViewRowList != null)
            {
                foreach (DataGridViewRow row in dataGridViewRowList)
                {
                    Data.SheetSubResult subResult = (Data.SheetSubResult)row.Tag;
                    canvasPanel.WorkingFigures.AddFigure(subResult.GetFigure(10, ratio));
                }
            }

            if (image != null)
                canvasPanel.UpdateImage(image);
            canvasPanel.Invalidate();
        }


        private void CanvasPanel_MouseClicked(PointF point, ref bool processingCancelled)
        {
            canvasPanel.ZoomFit();
        }

    }
}

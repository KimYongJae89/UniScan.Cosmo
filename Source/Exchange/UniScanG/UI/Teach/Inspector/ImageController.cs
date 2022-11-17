using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Screen.Vision;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;
using UniScanG.Vision;

namespace UniScanG.UI.Teach.Inspector
{
    public delegate void OnDefectTpyeSelectChangedDelegate(DefectType defectType);
    public interface IDefectTypeFilter {
        void SetDefectTpyeSelectChanged(OnDefectTpyeSelectChangedDelegate onDefectTpyeSelectChanged);
    }
    public interface IDefectLegend { }

    public partial class ImageController : UserControl, IModellerControl, IMultiLanguageSupport, IModelListener
    {
        public const int RescaleFactor = 2;
        ModellerPageExtender modellerPageExtender;
        CanvasPanel canvasPanel;

        ContextInfoForm contextInfoForm = new ContextInfoForm();

        DefectType selectedDefectType = DefectType.Total;
        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();

        public ImageController(IDefectTypeFilter defectTypeFilterPanel, IDefectLegend defectLegendPanel )
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            canvasPanel = new CanvasPanel();
            canvasPanel.TabIndex = 0;
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.DragMode = DragMode.Pan;
            canvasPanel.MouseClicked = contextInfoForm.CanvasPanel_MouseClicked;
            canvasPanel.FigureClicked = contextInfoForm.CanvasPanel_FigureFocused;
            //canvasPanel.FigureFocused = contextInfoForm.CanvasPanel_FigureFocused;
            //canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            
            imageContanier.Controls.Add(canvasPanel);

            Control defectTypeFilterPanelControl = defectTypeFilterPanel as Control;
            if (defectTypeFilterPanelControl != null)
            {
                defectTypeFilterPanel.SetDefectTpyeSelectChanged(OnDefectTpyeSelectChanged);
                defectTypeFilterPanelControl.Dock = DockStyle.Fill;
                this.panelDefectType.Controls.Add(defectTypeFilterPanelControl);
            }

            Control defectLegendPanelControl = defectLegendPanel as Control;
            if (defectLegendPanelControl != null)
            {
                defectLegendPanelControl.Dock = DockStyle.Fill;
                this.panelDefectLegend.Controls.Add(defectLegendPanelControl);
            }

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        private void OnDefectTpyeSelectChanged(DefectType defectType)
        {
            selectedDefectType = defectType;
            ShowResult();
        }

        public void SetModellerExtender(ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender;

            this.modellerPageExtender.UpdateImage = UpdateImage;
            this.modellerPageExtender.UpdateFigure = UpdateFigure;
            this.modellerPageExtender.UpdateZoom = UpdateZoom;
            
            this.modellerPageExtender.UpdateSheetResult = UpdateSheetResult;
            this.modellerPageExtender.ExportData = ExportData; 
        }
        
        public void UpdateImage(ImageD grabImage, bool zoomFit)
        {
            Bitmap bitmap = null;

            if (grabImage != null)
            {
                //UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
                //if (model!=null && model.Divider>1)
                //{
                //    grabImage = grabImage.Resize(1f / SystemManager.Instance().CurrentModel.Divider);
                //}
                if (RescaleFactor != 1)
                {
                    AlgoImage grabAlgoImage = null;
                    AlgoImage rescaleAlgoImage = null;
                    try
                    {
                        grabAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabImage, ImageType.Grey);
                        ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(grabAlgoImage);

                        rescaleAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey,
                            new Size(grabImage.Width / RescaleFactor, grabImage.Height / RescaleFactor));
                        ip.Resize(grabAlgoImage, rescaleAlgoImage);
                        grabImage = rescaleAlgoImage.ToImageD();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Operation, string.Format("ImageController::UpdateImage - {0}", ex.Message));
                    }
                    finally
                    {
                        grabAlgoImage?.Dispose();
                        rescaleAlgoImage?.Dispose();
                    }
                }

                int size = grabImage.Width * grabImage.Height;
                if (size >= 536870912)// 2GB / 4
                {
                    int newHeight = (int)(2147483648 / 4 / grabImage.Width);
                    bitmap = grabImage.ClipImage(new Rectangle(0, 0, grabImage.Width, newHeight)).ToBitmap();
                }
                else
                {
                    bitmap = grabImage.ToBitmap();
                }
            }

            canvasPanel.WorkingFigures.Clear();
            canvasPanel.BackgroundFigures.Clear();
            canvasPanel.TempFigures.Clear();

            zoomFit |= (canvasPanel.Image == null);
            canvasPanel.UpdateImage(bitmap);
            if (zoomFit)
                canvasPanel.ZoomFit();

            if (this.modellerPageExtender.ImageUpdated != null)
                this.modellerPageExtender.ImageUpdated(bitmap);

            //bitmap?.Dispose();
        }

        private void buttonZoomIn_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomIn();
        }

        private void buttonZoomOut_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomOut();
        }

        private void buttonZoomFit_Click(object sender, System.EventArgs e)
        {
            canvasPanel.ZoomFit();
        }

        private void buttonDeleteFigure_Click(object sender, System.EventArgs e)
        {
            //canvasPanel.ClearFigure();

            //canvasPanel..Save("d:\\1.bmp");
        }
        
        private void UpdateFigure(Figure figure)
        {
            canvasPanel.TempFigures.Clear();
            canvasPanel.TempFigures.AddFigure(figure);
            canvasPanel.Invalidate(false);
        }

        private void UpdateZoom(Rectangle viewPort)
        {
            int scaleFactor = 1;
            if (SystemManager.Instance().CurrentModel != null)
                scaleFactor = SystemManager.Instance().CurrentModel.ScaleFactor;
            
            int l = viewPort.Left * scaleFactor;
            int t = viewPort.Top * scaleFactor;
            int r = viewPort.Right * scaleFactor;
            int b = viewPort.Bottom * scaleFactor;
            canvasPanel.ZoomRange(Rectangle.Inflate(Rectangle.FromLTRB(l, t, r, b), 100, 100));

            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }

        delegate void UpdateSheetResultDelegate(SheetResult sheetResult);
        private void UpdateSheetResult(SheetResult sheetResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateSheetResultDelegate(UpdateSheetResult), sheetResult);
                return;
            }
            
            dataGridViewRowList.Clear();
            if (sheetResult == null)
                return;

            inspectTime.Text = sheetResult.SpandTime.ToString("ss\\.fff");

            foreach (SheetSubResult sheetSubResult in sheetResult.SheetSubResultList)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = StringManager.GetString(this.GetType().FullName, sheetSubResult.GetDefectType().ToString()) };
                typeCell.Style.ForeColor = sheetSubResult.GetColor();
                typeCell.Style.BackColor= sheetSubResult.GetBgColor();
                typeCell.ToolTipText = sheetSubResult.GetDefectTypeDiscription();
                row.Cells.Add(typeCell);
                    
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = sheetSubResult.ToString()});
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = sheetSubResult.Image };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                row.Cells.Add(imageCell);
                row.Height = defectList.Height / 5;
                
                row.Tag = sheetSubResult;

                dataGridViewRowList.Add(row);
            }

            ShowResult();
        }

        bool onUpdate = false;
        private void ShowResult()
        {
            defectList.Rows.Clear();
            //if (dataGridViewRowList.Count == 0)
            //    return;

            SimpleProgressForm lodingForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Filtering"));

            List<DataGridViewRow> tempDataGridViewRowList = new List<DataGridViewRow>();
            lodingForm.Show(() =>
            {
                canvasPanel.WorkingFigures.Clear();
                
                foreach (DataGridViewRow dataGridViewRow in dataGridViewRowList)
                {
                    SheetSubResult sheetSubResult = (SheetSubResult)dataGridViewRow.Tag;
                    if (selectedDefectType == DefectType.Total || sheetSubResult.GetDefectType() == selectedDefectType)
                    {
                        if (useSize.Checked == true)
                        {
                            if (sheetSubResult.RealLength >= (float)sizeMin.Value && sheetSubResult.RealLength <= (float)sizeMax.Value)
                                tempDataGridViewRowList.Add(dataGridViewRow);
                        }
                        else
                        {
                            tempDataGridViewRowList.Add(dataGridViewRow);
                        }
                    }
                    //tempDataGridViewRowList.Add(dataGridViewRow);
                }
                
                foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
                {
                    SheetSubResult sheetSubResult = (SheetSubResult)dataGridViewRow.Tag;
                    canvasPanel.WorkingFigures.AddFigure(sheetSubResult.GetFigure(2,1f/RescaleFactor));
                }
            });

            totalDefectNum.Text = tempDataGridViewRowList.Count.ToString();
            onUpdate = true;
            defectList.Rows.Clear();
            defectList.Rows.AddRange(tempDataGridViewRowList.ToArray());
            defectList.ClearSelection();
            onUpdate = false;

            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }
        
        private void ExportData()
        {
            if (defectList.Rows.Count <= 0)
                return;

            string folderPath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, DateTime.Now.ToString("yy-MM-dd hh-mm-ss"));

            if (Directory.Exists(folderPath) == true)
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch(Exception e)
                {
                    Directory.Delete(folderPath, true);
                }
                //System.Threading.Thread.Sleep(1000);
            }
                
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "Data.csv");
            StringBuilder sb = new StringBuilder();

            int index = 0;
            //foreach (DataGridViewRow row in defectList.Rows)
            //{
            //    SheetSubResult sheetSubResult =  (SheetSubResult)row.Tag;
            //    sb.AppendFormat("{1}\t{0}", index, sheetSubResult.ToExportString());
            //    sb.AppendLine();
            //    string imagePath = Path.Combine(folderPath, string.Format("{0}.bmp", index));
            //    index++;
            //    ImageHelper.SaveImage(sheetSubResult.Image, imagePath);
            //}

            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(folderPath);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            //StringManager.UpdateString(this.GetType().FullName, labelSheetAttack);
            //StringManager.UpdateString(this.GetType().FullName, labelPoleLine);
            //StringManager.UpdateString(this.GetType().FullName, labelPoleCircle);
            //StringManager.UpdateString(this.GetType().FullName, labelDielectric);
            //StringManager.UpdateString(this.GetType().FullName, labelPinHole);
            //StringManager.UpdateString(this.GetType().FullName, labelShape);
            //StringManager.UpdateString(this.GetType().FullName, labelInspectTime);
            //StringManager.UpdateString(this.GetType().FullName, labelTotalDefectNum);
            //StringManager.UpdateString(this.GetType().FullName, typeSheetAttack);

            //StringManager.UpdateString(this.GetType().FullName, labelType);
            //StringManager.UpdateString(this.GetType().FullName, typePoleCircle);
            //StringManager.UpdateString(this.GetType().FullName, typePoleLine);
            //StringManager.UpdateString(this.GetType().FullName, typeDielectric);
            //StringManager.UpdateString(this.GetType().FullName, typePinHole);
            //StringManager.UpdateString(this.GetType().FullName, typeShape);

            //StringManager.UpdateString(this.GetType().FullName, labelSize);
            //StringManager.UpdateString(this.GetType().FullName, labelMin);
            //StringManager.UpdateString(this.GetType().FullName, labelMax);

            //for (int i = 0; i < defectList.ColumnCount; i++)
            //    defectList.Columns[i].HeaderText = StringManager.GetString(this.GetType().FullName, defectList.Columns[i].HeaderText);
        }
        
        private void sizeMin_ValueChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void sizeMax_ValueChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void useSize_CheckedChanged(object sender, EventArgs e)
        {
            sizeMin.Enabled = useSize.Checked;
            sizeMax.Enabled = useSize.Checked;

            ShowResult();
        }

        delegate void ModelChangedDelegate();
        public void ModelChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ModelChangedDelegate(ModelChanged));
                return;
            }

            canvasPanel.WorkingFigures.Clear();
            defectList.Rows.Clear();
        }

        public void ModelTeachDone(int camId) { }
        public void ModelRefreshed() { }

        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (defectList.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = defectList.SelectedRows[0];
            SheetSubResult sheetSubResult = (SheetSubResult)row.Tag;

            //canvasPanel.ZoomRange(Rectangle.Inflate(sheetSubResult.Region, 100, 100));
            int l = sheetSubResult.Region.Left / RescaleFactor;
            int t = sheetSubResult.Region.Top / RescaleFactor;
            int r = sheetSubResult.Region.Right / RescaleFactor;
            int b = sheetSubResult.Region.Bottom / RescaleFactor;
            canvasPanel.ZoomRange(Rectangle.Inflate(Rectangle.FromLTRB(l, t, r, b), 100, 100));

            canvasPanel.TempFigures.Clear();
            //canvasPanel.TempFigures.AddFigure(sheetSubResult.GetFigure(70));
            canvasPanel.Invalidate(false);
            canvasPanel.Update();
        }

        private void defectList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==2)
            {
                DataGridViewRow row = defectList.Rows[e.RowIndex];
                SheetSubResult ssr = row.Tag as SheetSubResult;
                if (ssr == null)
                    return;

                DataGridViewImageCell cell = row.Cells[2] as DataGridViewImageCell;
                if (cell == null)
                    return;

                if (cell.Value == ssr.BufImage)
                    cell.Value = ssr.Image;
                else
                    cell.Value = ssr.BufImage;
            }
        }
    }
}
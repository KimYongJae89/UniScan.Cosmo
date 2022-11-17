using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Settings;
using UniScanG.UI;
using static UniScanG.UI.Inspect.InspectPage;

namespace UniScanG.Gravure.UI.Inspect
{
    public partial class DefectPanel : UserControl, IInspectDefectPanel, IMultiLanguageSupport, IModelListener
    {
        bool blockUpdate = true;
        List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
        List<CheckBox> checkBoxCamList = new List<CheckBox>();

        DefectType selectedDefectType = DefectType.Total;

        UpdateResultDelegate UpdateResultDelegate;
        List<DataGridViewRow> dataSource = new List<DataGridViewRow>();
        object lockObject = new object();
        List<DataGridViewRow> filteredDataSource = null;

        public bool BlockUpdate
        {
            get { return blockUpdate; }
            set { blockUpdate = value; }
        }

        public void AddDelegate(UpdateResultDelegate UpdateResultDelegate)
        {
            this.UpdateResultDelegate = UpdateResultDelegate;
        }

        public DefectPanel()
        {
            InitializeComponent();

            this.TabIndex = 0;
            this.Dock = DockStyle.Right;
            
            this.defectList.RowTemplate.Height = (this.defectList.Height - this.defectList.ColumnHeadersHeight) / 8;

            this.maxViewCount.Value = 100;

            InitCheckBoxCam();
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            StringManager.AddListener(this);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);
        }

        void InitCheckBoxCam()
        {
            ExchangeOperator exchangeOperator = SystemManager.Instance().ExchangeOperator;
            if (exchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f=>f.Info.ClientIndex==0);
                for (int i = inspectorList.Count - 1; i >= 0; i--)
                {
                    CheckBox checkBoxNewCam = new CheckBox();
                    checkBoxNewCam.Appearance = checkBoxCam.Appearance;
                    checkBoxNewCam.AutoSize = checkBoxCam.AutoSize;
                    checkBoxNewCam.Checked = checkBoxCam.Checked;
                    checkBoxNewCam.Dock = checkBoxCam.Dock;
                    checkBoxNewCam.FlatAppearance.BorderSize = checkBoxCam.FlatAppearance.BorderSize;
                    checkBoxNewCam.FlatAppearance.CheckedBackColor = checkBoxCam.FlatAppearance.CheckedBackColor;
                    checkBoxNewCam.FlatAppearance.MouseDownBackColor = checkBoxCam.FlatAppearance.MouseDownBackColor;
                    checkBoxNewCam.FlatAppearance.MouseOverBackColor = checkBoxCam.FlatAppearance.MouseOverBackColor;
                    checkBoxNewCam.FlatStyle = checkBoxCam.FlatStyle;
                    checkBoxNewCam.Margin = checkBoxCam.Margin;
                    checkBoxNewCam.Name = "checkBoxCam" + (inspectorList[i].Info.CamIndex + 1).ToString();
                    checkBoxNewCam.Size = checkBoxCam.Size;
                    checkBoxNewCam.TabIndex = 0;
                    checkBoxNewCam.Text = (inspectorList[i].Info.CamIndex + 1).ToString();
                    checkBoxNewCam.TextAlign = checkBoxCam.TextAlign;
                    checkBoxNewCam.UseVisualStyleBackColor = checkBoxCam.UseVisualStyleBackColor;

                    checkBoxNewCam.Tag = inspectorList[i];
                    checkBoxCamList.Add(checkBoxNewCam);
                    panelSelectCam.Controls.Add(checkBoxNewCam);
                }
            }
        }

        public void Reset()
        {
            inspectionNo = -1;
            ClearGridData();
            Bitmap defaultImage = null;
            string imagePath = SystemManager.Instance().ModelManager?.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription);
            if (System.IO.File.Exists(imagePath))
                defaultImage = (Bitmap)ImageHelper.LoadImage(imagePath);

            UpdateResultDelegate?.Invoke(defaultImage, null);

            //UpdateResultDelegate?.Invoke(null, null);
        }

        /// <summary>
        ///  마지막에 Display된 시트 번호.
        /// </summary>
        int inspectionNo = -1;
        public void InspectDone(InspectionResult inspectionResult)
        {
            //UpdateGrid(0);
            inspectionNo = -1;
            if (inspectionResult.Judgment == Judgment.Skip)
                return;

            int curSheetNo = int.Parse(inspectionResult.InspectionNo);
            if (inspectionNo > curSheetNo)
                return;
            inspectionNo = curSheetNo;

            if (inspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
                InspectDoneInspector(inspectionResult);
            else if(inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName))
                InspectDoneMonitor(inspectionResult);
        }

        private void InspectDoneMonitor(InspectionResult inspectionResult)
        {
            SheetResult sheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as SheetResult;
            if (sheetResult == null)
                return;
            
            if (this.blockUpdate)
                return;

            bool ok = System.Threading.Monitor.TryEnter(this.lockObject);
            if (ok)
            {
                BuildGridData(sheetResult, inspectionResult.InspectionNo);
                UpdateGrid();

                Bitmap prevImage = sheetResult.PrevImage;
                if (UpdateResultDelegate != null)
                    UpdateResultDelegate(prevImage, filteredDataSource);
                
                System.Threading.Monitor.Exit(this.lockObject);
            }
        }

        private void InspectDoneInspector(InspectionResult inspectionResult)
        {
            SheetResult sheetResult = inspectionResult.AlgorithmResultLDic[Detector.TypeName] as SheetResult;
            if (sheetResult == null)
                return;
            
            if (this.blockUpdate)
                return;

            bool ok = System.Threading.Monitor.TryEnter(this.lockObject);
            if (ok)
            {
                BuildGridData(sheetResult, inspectionResult.InspectionNo);
                UpdateGrid();

                if (UpdateResultDelegate != null)
                {
                    Bitmap prevImage = (Bitmap)(inspectionResult.AlgorithmResultLDic[CalculatorBase.TypeName] as SheetResult)?.PrevImage;
                    if (prevImage == null)
                        prevImage = SystemManager.Instance().CurrentModel.GetPreviewImage();

                    Bitmap prevImageClone = null;
                    lock(prevImage)
                        prevImageClone = (Bitmap)(prevImage.Clone());

                    UpdateResultDelegate(prevImageClone, filteredDataSource);
                    //prevImageClone?.Dispose();
                }

                System.Threading.Monitor.Exit(this.lockObject);
            }
        }
        
        private void ClearGridData()
        {
            this.UpdateGrid(0);
            this.dataSource.Clear();
            this.filteredDataSource?.Clear();
        }

        private delegate void UpdateGridDelegate(int rowCount = -1);
        private void UpdateGrid(int rowCount = -1)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new UpdateGridDelegate(UpdateGrid), rowCount);
            //    return;
            //}

            if (filteredDataSource != null)
            {
                if (rowCount < 0)
                    rowCount = this.filteredDataSource.Count;
                ShowResult(new List<DataGridViewRow>(this.filteredDataSource.GetRange(0, rowCount)));
            }
            //    defectList.RowCount = rowCount;
            //    totalDefect.Text = rowCount.ToString();
        }

        private string lastBuildedInspNo = "";
        private void BuildGridData(SheetResult sheetResult, string inspectionNo)
        {
            int maxViewCount = (int)this.maxViewCount.Value;
            lock (dataSource)
            {
                if(maxViewCount<0)
                    dataSource.Clear();

                foreach (Data.SheetSubResult sheetSubResult in sheetResult.SheetSubResultList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    row.CreateCells(this.defectList);

                    // Sheet No
                    row.Cells[0].Value = inspectionNo;

                    // Cam No
                    row.Cells[1].Value = sheetSubResult.CamIndex + 1;

                    // Defect Type
                    row.Cells[2].Value = StringManager.GetString(this.GetType().FullName, sheetSubResult.GetDefectType().ToString());
                    row.Cells[2].Style.ForeColor = sheetSubResult.GetColor();
                    row.Cells[2].Style.BackColor = sheetSubResult.GetBgColor();
                    row.Cells[2].ToolTipText = sheetSubResult.GetDefectTypeDiscription();

                    // Position
                    PointF realCenter = DrawingHelper.CenterPoint(sheetSubResult.RealRegion);
                    row.Cells[3].Value = string.Format("X{0:0.0} / Y{1:0.0}", realCenter.X / 1000, realCenter.Y / 1000);

                    // Size
                    row.Cells[4].Value = string.Format("W{0:0.0} / H{1:0.0}", sheetSubResult.RealRegion.Width, sheetSubResult.RealRegion.Height);

                    // Info
                    row.Cells[5].Value = string.Format("{0}", sheetSubResult.SubtractValueMax); // sheetSubResult.ToString();

                    // Image
                    Bitmap image = sheetSubResult.Image;
                    if (image != null)
                    {
                        lock (sheetSubResult.Image)
                            ((DataGridViewImageCell)row.Cells[6]).Value = sheetSubResult.Image.Clone();
                        ((DataGridViewImageCell)row.Cells[6]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    }

                    row.Height = defectList.Height / 8;
                    row.Tag = sheetSubResult;

                    dataSource.Add(row);

                    if (maxViewCount>=0 && dataSource.Count > maxViewCount)
                        dataSource.RemoveRange(0, dataSource.Count - maxViewCount);
                }

                defectList.RowTemplate.Height = defectList.Height / 12;
                filteredDataSource = GetFilteredList(dataSource);
                lastBuildedInspNo = inspectionNo;
            }
        }


        private void defectList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //return;
            //DataGridView dataGridView = sender as DataGridView;
            //if (dataGridView != null)
            //{
            //    dataGridView.Rows[e.RowIndex].Height = dataGridView.Height / 8;
            //}
            lock (this.lockObject)
                e.Value = filteredDataSource[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private delegate void ShowResultDelegate(List<DataGridViewRow> filteredDataGridViewRowList);
        private void ShowResult(List<DataGridViewRow> filteredDataGridViewRowList)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ShowResultDelegate(ShowResult), filteredDataGridViewRowList);
                return;
            }

            lock (defectList)
            {
                int rowIndex = defectList.FirstDisplayedScrollingRowIndex;

                defectList.Rows.Clear();
                filteredDataGridViewRowList.Reverse();

                defectList.Rows.AddRange(filteredDataGridViewRowList.ToArray());
                defectList.ClearSelection();

                if (rowIndex >= 0 && defectList.Rows.Count > rowIndex)
                    defectList.FirstDisplayedScrollingRowIndex = rowIndex;

                // 요기요
                if (filteredDataSource.Count > 0)
                {
                    this.totalDefect.Text = filteredDataSource.Count(f => (string)f.Cells[0].Value == lastBuildedInspNo).ToString();
                }
                else
                {
                    this.totalDefect.Text = "0";
                }
            }
        }

        private List<DataGridViewRow> GetFilteredList(List<DataGridViewRow> tempDataGridViewRowList)
        {
            List<DataGridViewRow> filteredDataGridViewRowList = new List<DataGridViewRow>();

            lock (tempDataGridViewRowList)
            {
                foreach (DataGridViewRow dataGridViewRow in tempDataGridViewRowList)
                {
                    Data.SheetSubResult sheetSubResult = (Data.SheetSubResult)dataGridViewRow.Tag;

                    if (checkBoxCamList.Count > 0 && checkBoxCamList[sheetSubResult.CamIndex].Checked == false)
                        continue;

                    DefectType defectType = sheetSubResult.GetDefectType();
                    if (defectType == DefectType.Unknown)
                        continue;

                    if (selectedDefectType == DefectType.Total || defectType == selectedDefectType)
                    {
                        if (useSize.Checked == true)
                        {
                            if (sheetSubResult.RealLength >= (float)sizeMin.Value && sheetSubResult.RealLength <= (float)sizeMax.Value)
                                filteredDataGridViewRowList.Add(dataGridViewRow);
                        }
                        else
                        {
                            filteredDataGridViewRowList.Add(dataGridViewRow);
                        }
                    }
                }
            }

            return filteredDataGridViewRowList;
        }

        private void ChangeFilter()
        {
            if (dataGridViewRowList.Count == 0)
                return;

            lock (defectList)
                defectList.Rows.Clear();

            SimpleProgressForm lodingForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Filtering"));
            
            List<DataGridViewRow> filteredDataGridViewRowList = new List<DataGridViewRow>();
            lodingForm.Show(() =>
            {
                lock (dataGridViewRowList)
                    filteredDataGridViewRowList = GetFilteredList(dataGridViewRowList);
            });

            ShowResult(filteredDataGridViewRowList);
            if (UpdateResultDelegate != null)
                UpdateResultDelegate(null, GetFilteredList(dataSource));
        }

        void IMultiLanguageSupport.UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void ModelChanged() { }

        public void ModelTeachDone(int camId) { }

        public void ModelRefreshed() { }

        private void useSize_CheckedChanged(object sender, System.EventArgs e)
        {
            sizeMin.Enabled = useSize.Checked;
            sizeMax.Enabled = useSize.Checked;

            ChangeFilter();
        }

        private void total_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Total;

            ChangeFilter();
        }

        private void sheetAttack_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.SheetAttack;

            ChangeFilter();
        }

        private void noprint_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Noprint;

            ChangeFilter();
        }

        private void dielectric_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Dielectric;

            ChangeFilter();
        }

        private void poleLine_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.PoleLine;

            ChangeFilter();
        }

        private void poleCircle_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.PoleCircle;

            ChangeFilter();
        }

        private void pinHole_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.PinHole;

            ChangeFilter();
        }

        private void shape_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            selectedDefectType = DefectType.Shape;

            ChangeFilter();
        }

        private void sizeMin_ValueChanged(object sender, System.EventArgs e)
        {
            ChangeFilter();
        }

        private void sizeMax_ValueChanged(object sender, System.EventArgs e)
        {
            ChangeFilter();
        }

        private void defectList_SelectionChanged(object sender, System.EventArgs e)
        {
            //((DataGridView)sender).ClearSelection();
        }

        private void DefectPanel_Load(object sender, EventArgs e)
        {
            Color color = Color.Empty;

            Data.ColorTable.UpdateControlColor(this.pinhole, DefectType.PinHole);
            Data.ColorTable.UpdateControlColor(this.noprint, DefectType.Noprint);
            Data.ColorTable.UpdateControlColor(this.sheetAttack, DefectType.SheetAttack);
            Data.ColorTable.UpdateControlColor(this.dielectric, DefectType.Dielectric);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.UI.Etc;
using UniScanG.UI.Report;

namespace UniScanG.Gravure.UI.Report
{
    public partial class ReportPanel : UserControl, IReportPanel, IUserHandlerListener, IMultiLanguageSupport
    {
        List<CheckBox> checkBoxCamList = new List<CheckBox>();
        List<FileInfo> teachInfoList = new List<FileInfo>();
        List<DataGridViewRow> refSheetDataList = new List<DataGridViewRow>();

        bool onUpdateData = false;
        DefectType defectType = DefectType.Total;

        SingleSheetResultPanel singleSheetResultPanel;
        MultiSheetResultPanel multiSheetResultPanel;

        Data.ProductionG selProduction = null;
        public ReportPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            InitCheckBoxCam();

            this.singleSheetResultPanel = new SingleSheetResultPanel();
            this.singleSheetResultPanel.Dock = DockStyle.Fill;
            this.singleSheetResultPanel.Hide();
            this.panelResult.Controls.Add(singleSheetResultPanel);

            this.multiSheetResultPanel = new MultiSheetResultPanel();
            this.multiSheetResultPanel.Dock = DockStyle.Fill;
            this.multiSheetResultPanel.Hide();
            this.panelResult.Controls.Add(multiSheetResultPanel);

            UserHandler.Instance().AddListener(this);
        }

        public void UserChanged()
        {
            User user = UserHandler.Instance().CurrentUser;
            //layoutAdvance.Visible = user.SuperAccount;
        }

        void InitCheckBoxCam()
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f=>f.Info.ClientIndex<=0);
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
                    checkBoxNewCam.CheckedChanged += CamButton_CheckedChanged;
                    checkBoxNewCam.Tag = inspectorList[i];
                    checkBoxCamList.Add(checkBoxNewCam);
                    panelSelectCam.Controls.Add(checkBoxNewCam);
                }
                checkBoxCamList.Reverse();
            }
        }
         
        private void CamButton_CheckedChanged(object sender, System.EventArgs e)
        {
            ShowResult();
        }


        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Search(DynMvp.Data.ProductionBase production)
        {
            onUpdateData = true;
            sheetList.Tag = production;

            selProduction = production as Data.ProductionG;
            if (selProduction == null)
                return;

            List<string> resultPathList = new List<string>();
            string resultPath = selProduction.GetResultPath();

            productionModelName.Text = selProduction.Name;
            productionLotName.Text = selProduction.LotNo;
            productionStartTime.Text = selProduction.StartTime.ToString("yy-MM-dd HH:mm");
            productionEndTime.Text = selProduction.LastUpdateTime.ToString("yy-MM-dd HH:mm");
            productionTargetSpd.Text = selProduction.LineSpeedMpm.ToString("0.0");

            UpdateDefectInfo();

            teachInfoList.Clear();
            refSheetDataList.Clear();

            ReportProgressForm loadingForm = new ReportProgressForm("Result Loading");
            loadingForm.Show(resultPath, refSheetDataList, teachInfoList);

            // Update Sheet Length Data
            Label[] labels = new Label[] { infoHeight1, infoHeight2, infoHeight3 };
            Array.ForEach(labels, f => f.Text = "");

            List<float> sheetHeightList = refSheetDataList.ConvertAll(f => (f.Tag as MergeSheetResult).SheetSize.Height);
            sheetHeightList.Sort();
            int count = sheetHeightList.Count;
            int count10 = sheetHeightList.Count / 10;
            if (count10 > 0)
            {
                int[] indexs = new int[] { 0, count10, count - count10, count };
                for (int i = 0; i < 3; i++)
                {
                    int src = indexs[i];
                    int dst = indexs[i + 1];
                    int len = dst - src;
                    List<float> list = sheetHeightList.GetRange(src, len);
                    if (list.Count > 0)
                        labels[i].Text = list.Average().ToString("F3");
                }
            }
            onUpdateData = false;

            ShowResult();
        }

        private void UpdateDefectInfo()
        {
            Gravure.Data.ProductionG productionG = (Gravure.Data.ProductionG)sheetList.Tag;

            if (productionG != null)
            {
                if (sheetRadioButton.Checked == true)
                {
                    sheetAttackNum.Text = string.Format("{0} / {1}", productionG.SheetAttackPatternNum, productionG.Total);
                    noPrintNum.Text = string.Format("{0} / {1}", productionG.NoPrintPatternNum, productionG.Total);
                    dielectricNum.Text = string.Format("{0} / {1}", productionG.DielectricPatternNum, productionG.Total);
                    pinHoleNum.Text = string.Format("{0} / {1}", productionG.PinHolePatternNum, productionG.Total);
                }
                else if (patternRadioButton.Checked == true)
                {
                    sheetAttackNum.Text = productionG.SheetAttackNum.ToString();
                    noPrintNum.Text = productionG.NoPrintNum.ToString();
                    dielectricNum.Text = productionG.DielectricNum.ToString();
                    pinHoleNum.Text = productionG.PinHoleNum.ToString();
                }
            }
        }

        private List<DataGridViewRow> Filtering()
        {
            List<DataGridViewRow> tempList = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in refSheetDataList)
            {
                MergeSheetResult sheetResult = (MergeSheetResult)row.Tag;
                List<SheetSubResult> sheetSubResultList = new List<SheetSubResult>(sheetResult.SheetSubResultList);

                // 카메라 번호 필터
                sheetSubResultList.RemoveAll(f =>
                {
                    if (f.CamIndex >= checkBoxCamList.Count)
                        return true;
                    return checkBoxCamList[f.CamIndex].Checked == false;
                    });

                // 타입 필터
                if (defectType != DefectType.Total)
                    sheetSubResultList.RemoveAll(f => f.GetDefectType() != defectType);

                // 크기 필터
                if (useSize.Checked == true)
                {
                    sheetSubResultList.RemoveAll(f =>
                    Math.Max(f.RealRegion.Width, f.RealRegion.Height) > (float)sizeMax.Value ||
                    Math.Min(f.RealRegion.Width, f.RealRegion.Height) < (float)sizeMin.Value);
                }

                MergeSheetResult tempResult = new MergeSheetResult(sheetResult.Index, sheetResult.resultPath, false);
                tempResult.SheetSize = sheetResult.SheetSize;
                tempResult.SheetSubResultList.AddRange(sheetSubResultList);

                DataGridViewRow dataGridViewRow = new DataGridViewRow();

                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell() { Value = tempResult.Index + 1 };
                DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = tempResult.SheetSubResultList.Count };
                if (tempResult.SheetSubResultList.Count > 0)
                {
                    if (ngFilter.Checked == false)
                        continue;

                    qtyCell.Style.BackColor = Color.Red;
                }
                else
                {
                    if (okFilter.Checked == false)
                        continue;

                    qtyCell.Style.BackColor = Color.LightGreen;
                }


                dataGridViewRow.Cells.Add(nameCell);
                dataGridViewRow.Cells.Add(qtyCell);

                dataGridViewRow.Tag = tempResult;

                tempList.Add(dataGridViewRow);
            }

            return tempList;
        }

        private void ShowResult()
        {
            onUpdateData = true;

            List<DataGridViewRow> tempResultList = null;
            float ngNum = 0;
            
            SimpleProgressForm loadingForm = new SimpleProgressForm();
            loadingForm.Show(new Action(() =>
            {
                tempResultList = Filtering();
                //foreach (DataGridViewRow row in tempResultList)
                //{
                //    MergeSheetResult result = (MergeSheetResult)row.Tag;
                //    if (result.IsNG == true)
                //        ngNum++;
                //}
                ngNum = tempResultList.Count(f => ((MergeSheetResult)f.Tag).IsNG);
            }));

            //if (tempResultList == null || tempResultList.Count == 0)
            //{
            //    Clear();
            //    return;
            //}

            int totalcount = tempResultList.Count;
            int ngCount = tempResultList.Count(f => (f.Tag as MergeSheetResult).IsNG);

            sheetTotal.Text = totalcount.ToString();
            sheetNG.Text = ngCount.ToString();
            sheetRatio.Text = string.Format("{0:F2} %", ngCount * 100.0f / totalcount);


            //float shMin = refSheetDataList.ConvertAll(f=>f.Tag)

            sheetList.Rows.Clear();
            sheetList.Rows.AddRange(tempResultList.ToArray());
            sheetList.Sort(sheetList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            
            onUpdateData = false;

            SelectSheet();
        }
        
        public void Clear()
        {
            sheetTotal.Text = string.Empty;
            sheetNG.Text = string.Empty;
            sheetRatio.Text = string.Empty;
            sheetList.Rows.Clear();

            this.singleSheetResultPanel.Clear();
            this.singleSheetResultPanel.Hide();

            this.multiSheetResultPanel.Clear();
            multiSheetResultPanel.Hide();
        }

        private void SelectSheet()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            List<MergeSheetResult> mergeSheetResultList = new List<MergeSheetResult>();
            foreach(DataGridViewRow row in sheetList.SelectedRows)
            {
                MergeSheetResult mergeSheetResult = (MergeSheetResult)row.Tag;
                mergeSheetResultList.Add(mergeSheetResult);
            }

            if (mergeSheetResultList.Count == 0)
            {
                this.singleSheetResultPanel.Hide();
                this.multiSheetResultPanel.Hide();
            }
            //else if (mergeSheetResultList.Count == 1)
            //{
            //    this.multiSheetResultPanel.Hide();
            //    this.singleSheetResultPanel.Show();
            //    this.singleSheetResultPanel.SelectSheet(mergeSheetResultList[0]);
            //}
            else
            {
                this.singleSheetResultPanel.Hide();
                this.multiSheetResultPanel.Show();
                this.multiSheetResultPanel.SelectSheet(mergeSheetResultList);
            }

            onUpdateData = false;

            //SelectDefect();
        }

        private void openFolder_Click(object sender, EventArgs e)
        {

        }

        public void Initialize() { }
        
        private void sheetList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdateData)
                return;

            SelectSheet();
        }

        private void total_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Total;
            ShowResult();
        }

        private void sheetAttack_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.SheetAttack;
            ShowResult();
        }

        private void poleLine_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PoleLine;

            ShowResult();
        }

        private void poleCircle_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PoleCircle;

            ShowResult();
        }

        private void dielectric_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Dielectric;

            ShowResult();
        }

        private void pinHole_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.PinHole;

            ShowResult();
        }

        private void shape_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Shape;

            ShowResult();
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

        private void okFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void ngFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void noPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
                return;

            defectType = DefectType.Noprint;

            ShowResult();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            sheetList.SelectAll();
        }

        private void buttonDeselect_Click(object sender, EventArgs e)
        {
            sheetList.ClearSelection();
        }

        private void collect_Click(object sender, EventArgs e)
        {
            //IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            //List<InspectorObj> inspectorObjList = server.GetInspectorList();
            //int inspectorCnt = inspectorObjList.Count;
            //int camCnt = inspectorObjList.FindAll(f => f.Info.ClientIndex <= 0).Count;
            //string[] selecetedPath = new string[inspectorCnt];

            //for (int i=0; i<4; i++)
            //{
            //    FolderBrowserDialog dlg = new FolderBrowserDialog();
            //    if (i > 0)
            //    {
            //        string selectedPath2 = selecetedPath[i - 1].Replace(inspectorObjList[i - 1].Info.Path, inspectorObjList[i].Info.Path);
            //        dlg.SelectedPath = selectedPath2;
            //        if (Directory.Exists(selectedPath2))
            //            selecetedPath[i] = dlg.SelectedPath;
            //    }

            //    if (string.IsNullOrEmpty(selecetedPath[i]))
            //    {
            //        dlg.SelectedPath = Path.Combine(inspectorObjList[i].Info.Path, "Result");
            //        if (dlg.ShowDialog() == DialogResult.Cancel)
            //            return;
            //        selecetedPath[i] = dlg.SelectedPath;
            //    }
            //}

            //List<Tuple<int,string>>[] sheetList = new List<Tuple<int, string>>[camCnt];
            //for (int i = 0; i < camCnt; i++)
            //    sheetList[i] = new List<Tuple<int, string>>();

            //SimpleProgressForm loadingForm = new SimpleProgressForm("Loading");
            //loadingForm.Show(() =>
            //{
            //    Parallel.For(0, inspectorCnt, i =>
            //     {
            //         int camIdx = inspectorObjList[i].Info.CamIndex;
            //         string[] subResultPaths = Directory.GetDirectories(selecetedPath[i]);
            //         Array.ForEach(subResultPaths, f =>
            //         {
            //             string sheetName = Path.GetFileName(f);
            //             int sheetNo;
            //             bool ok = int.TryParse(sheetName, out sheetNo);
            //             if (ok)
            //             {
            //                 lock (sheetList[camIdx])
            //                     sheetList[camIdx].Add(new Tuple<int, string>(sheetNo, f));
            //             }
            //         });
            //     });
            //});

            //Array.ForEach(sheetList, f => f.Sort());
            //int[] idxPointer = new int[camCnt];

            //while (true)
            //{
            //    int sheetNo = sheetList[0][idxPointer[0]].Item1;
            //    int minSheetIdx = -1;
            //    for (int camIdx = 1; camIdx < camCnt; camIdx++)
            //    {
            //        int idx = idxPointer[camIdx];
            //        int sheetNo2 =  sheetList[camIdx][idx].Item1;
            //        if(sheetNo == sheetNo2)
            //        {
            //            GOGOGOGOGO();
            //        }
            //        else
            //        {
            //            int incIdx = sheetNo < sheetNo2 ? 0 : 1;
            //            idxPointer[incIdx]++;
            //        }
            //    }

            //    if(Array)
            //}
            
        }

        private void buttonExportSheet_Click(object sender, EventArgs e)
        {
            if (sheetList.SelectedRows.Count == 0)
            {
                MessageForm.Show(null, StringManager.GetString("No Sheet Selected"));
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string path = Path.Combine(dlg.SelectedPath, string.Format("Export_{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            Directory.CreateDirectory(path);

            List<MergeSheetResult> mergeSheetResultList = new List<MergeSheetResult>();
            foreach (DataGridViewRow row in sheetList.SelectedRows)
                mergeSheetResultList.Add(row.Tag as MergeSheetResult);

            mergeSheetResultList.RemoveAll(f => f == null);

            ExportSheetDefectData(path, mergeSheetResultList);

            Process.Start(path);
        }

        private void ExportSheetDefectData(string path, List<MergeSheetResult> exportTargetList)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() =>
            {
                foreach (MergeSheetResult mergeSheetResult in exportTargetList)
                {
                    string sheetPath = Path.Combine(path, mergeSheetResult.Index.ToString());
                    mergeSheetResult.Export(sheetPath, cts.Token);
                    string report = Path.Combine(sheetPath, "Report");
                    Directory.CreateDirectory(report);
                    List<SheetSubResult> list = mergeSheetResult.SheetSubResultList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        Bitmap newBitmap = new Bitmap(list[i].Image);
                        ImageHelper.SaveImage(newBitmap, Path.Combine(report, string.Format("I{0}_W{1}_H{2}.jpg", i, list[i].RealRegion.Width, list[i].RealRegion.Height)));
                        newBitmap.Dispose();
                    }
                }
            }, cts);
        }

        private void buttonExportLength_Click(object sender, EventArgs e)
        {
            if (this.refSheetDataList.Count==0)
            {
                MessageForm.Show(null, StringManager.GetString("There is no Data to Export"));
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = string.Format("{0}_{1}_{2}.csv", selProduction.GetModelDescription().Name, selProduction.LotNo, selProduction.StartTime.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "CSV file (*.csv)|*.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;

            Dictionary<int, float> sheetLengthDic = refSheetDataList.ToDictionary(f => (f.Tag as MergeSheetResult).Index, f => (f.Tag as MergeSheetResult).SheetSize.Height);
            ExportSheetLengthData(filePath, sheetLengthDic);
        }

        private void ExportSheetLengthData(string filePath, Dictionary<int, float> sheetLengthDic)
        {
            StringBuilder sb = new StringBuilder();

            //if(sheetLengthDic.Count>10)
            //{
            //    List<float> sheetLenghtList = sheetLengthDic.Values.ToList();
            //    sheetLenghtList.Sort();
            //    int count10 = sheetLenghtList.Count / 10;
            //    int count80 = sheetLenghtList.Count - 2 * count10;
            //    float upper10 = sheetLenghtList.GetRange(0, count10).Average();
            //    float lower10 = sheetLenghtList.GetRange(sheetLenghtList.Count - count10, count10).Average();
            //    float middle80 = sheetLenghtList.GetRange(count10, count80).Average();
            //    sb.AppendLine(string.Format("Upper 10%,{0}", upper10));
            //    sb.AppendLine(string.Format("Middle 80%,{0}", middle80));
            //    sb.AppendLine(string.Format("Lower 10%,{0}", lower10));
            //}

            sb.AppendLine("SheetNo,Length");
            foreach (KeyValuePair<int, float> pair in sheetLengthDic)
                sb.AppendLine(string.Format("{0},{1}", pair.Key, pair.Value));

            File.WriteAllText(filePath, sb.ToString());
        }

        private void sheetList_Click(object sender, EventArgs e)
        {
            SelectSheet();
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPEG File(*.jpg)|*.jpg";
            dlg.FileName = (selProduction == null) ?
                string.Format("Capture_{0}.jpg", DateTime.Now.ToString("yyyyMMdd_HHmmss")) :
                string.Format("M{0}_L{1}.jpg", this.selProduction.Name, this.selProduction.LotNo);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            this.Update();
            Rectangle rect = RectangleToScreen(this.DisplayRectangle);
            Task.Factory.StartNew(new Action<object>(Capture), new object[] { rect, dlg.FileName});
        }

        private void Capture(object args)
        {
            Thread.Sleep(500);

            object[] array = (object[])args;
            Bitmap bmp = ImageHelper.Capture((Rectangle)array[0]);
            ImageHelper.SaveImage(bmp, (string)array[1]);
            //Process.Start((string)array[2]);
        }

        private void patternRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sheetRadioButton.Checked == true)
                UpdateDefectInfo();
        }

        private void totalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (patternRadioButton.Checked == true)
                UpdateDefectInfo();
        }

        private void ReportPanel_Load(object sender, EventArgs e)
        {
            Data.ColorTable.UpdateControlColor(this.pinhole, DefectType.PinHole);
            Data.ColorTable.UpdateControlColor(this.noprint, DefectType.Noprint);
            Data.ColorTable.UpdateControlColor(this.sheetAttack, DefectType.SheetAttack);
            Data.ColorTable.UpdateControlColor(this.dielectric, DefectType.Dielectric);
        }

        private void buttonInfoDetail_Click(object sender, EventArgs e)
        {
            if(this.teachInfoList.Count>0)
            {
                //UserControl control1 = new Teach.Inspector.ParamController(new Vision.Trainer.TrainerParam(), new Vision.SheetFinder.SheetBase.SheetFinderV2Param(), new Vision.Calculator.CalculatorParam(), new Vision.Detector.DetectorParam(), new Vision.Watcher.WatcherParam() );
                //UserControl control2 = new Teach.Inspector.ParamController(new Vision.Trainer.TrainerParam(), new Vision.SheetFinder.SheetBase.SheetFinderV2Param(), new Vision.Calculator.CalculatorParam(), new Vision.Detector.DetectorParam(), new Vision.Watcher.WatcherParam() );
                //control1.Dock = DockStyle.Fill;
                //control2.Dock = DockStyle.Fill;

                //Form form = new Form();
                //form.Controls.Add(control1);
                //form.Controls.Add(control2);
                //form.AutoSize = true;
                //form.StartPosition = FormStartPosition.CenterParent;

                //form.ShowDialog(this);
            }
        }

    }
}

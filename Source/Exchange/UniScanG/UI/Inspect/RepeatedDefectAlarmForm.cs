using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniScanG.Data;
using DynMvp.UI;
using UniScan.Common;
using UniScanG.Gravure.MachineIF;
using DynMvp.Base;
using UniScanG.Gravure.Settings;

namespace UniScanG.UI.Inspect
{
    public partial class RepeatedDefectAlarmForm : Form, IModelListener, IMultiLanguageSupport
    {
        CanvasPanel canvasPanel = new CanvasPanel();
        RepeatedDefectItemList repeatedDefectItemList = new RepeatedDefectItemList();
        List<RepeatedDefectItem> alarmedData = new List<RepeatedDefectItem>();
        public bool IsAlarmState
        {
            get
            {
                lock (alarmedData)
                    return alarmedData.Exists(f => f.IsAlarmState);
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public RepeatedDefectAlarmForm()
        {
            InitializeComponent();

            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.DragMode = DragMode.Select;
            canvasPanel.NoneClickMode = true;
            panelMain.Controls.Add(canvasPanel);

            //((DataGridViewImageCell)row.Cells[6]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.dataGridView1.RowTemplate.Height = this.dataGridView1.Height / 12;
            //this.dataGridView1.RowTemplate.DefaultCellStyle.

            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            this.updateTimer.Start();

            StringManager.AddListener(this);
        }

        public void InspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName) == false)
                return;
            
            SheetResult sheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as SheetResult;
            if (sheetResult == null)
                return;

            repeatedDefectItemList.AddResult(sheetResult, true);
            List<RepeatedDefectItem> newData = repeatedDefectItemList.GetAlarmData().FindAll(f => f.IsNotified == false);
            newData.ForEach(f => f.IsNotified = true);
            AddAlarm(newData);
        }

        public void Clear()
        {
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, "0");
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, "0");

            this.canvasPanel.WorkingFigures.Clear();
            this.dataGridView1.RowCount = 0;

            lock (this.alarmedData)
                this.alarmedData.Clear();

            lock (this.repeatedDefectItemList)
                repeatedDefectItemList.Clear();
        }


        private delegate void ShowDataDelegate(List<RepeatedDefectItem> newData);
        private void AddAlarm(List<RepeatedDefectItem> newData)
        {
            if (newData.Count > 0)
            {
                lock (this.alarmedData)
                {
                    this.alarmedData.AddRange(newData);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (alarmedData != null)
            {
                lock (alarmedData)
                {
                    foreach (RepeatedDefectItem repeatedDefectItem in alarmedData)
                    {
                        repeatedDefectItem.IsAlarmCleared = true;
                    }
                }
            }
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, "0");
            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, "0");
            this.Hide();
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (alarmedData == null)
                return;

            lock (alarmedData)
            {
                if (e.RowIndex >= alarmedData.Count)
                    return;

                List<SheetSubResult> sheetSubResult = alarmedData[e.RowIndex].SheetSubResultList.FindAll(f => f != null);
                if (sheetSubResult.Count == 0)
                {
                    e.Value = null;
                    return;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = e.RowIndex.ToString();
                        break;
                    case 1:
                        e.Value = sheetSubResult[0].Image;
                        break;
                    case 2:
                        e.Value = sheetSubResult[0].GetDefectType().ToString();
                        break;
                    case 3:
                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    sb.AppendLine(string.Format("W: {0:0.00}", sheetSubResult.Average(f => f.RealRegion.Width)));
                        //    sb.AppendLine(string.Format("H: {0:0.00}", sheetSubResult.Average(f => f.RealRegion.Height)));
                        //    e.Value = sb.ToString();
                        //}
                        e.Value = string.Format("W: {0:0.0}\r\nH: {1:0.0}", sheetSubResult.Average(f => f.RealRegion.Width), sheetSubResult.Average(f => f.RealRegion.Height));
                        break;
                    case 4:
                        e.Value = alarmedData[e.RowIndex].RepeatRatio.ToString("0.00");
                        break;
                    case 5:
                        e.Value = alarmedData[e.RowIndex].ContinueRatio.ToString("0.00");
                        break;
                }
            }
        }

        private void UpdateImage()
        {
            Bitmap prevImg = SystemManager.Instance().ModelManager.GetPreviewImage(SystemManager.Instance().CurrentModel.ModelDescription);
            canvasPanel.UpdateImage(prevImg);
            canvasPanel.ZoomFit();
        }

        public void ModelChanged()
        {
            UpdateImage();
        }

        public void ModelTeachDone(int camId)
        {
            if (camId < 0)
                UpdateImage();
        }
        public void ModelRefreshed() { }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            canvasPanel.WorkingFigures.Clear();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                RepeatedDefectItem item = this.alarmedData[row.Index];
                SheetSubResult president = item.SheetSubResultList.Find(f => f != null);
                Figure figure = president?.GetFigure(100);
                figure.Scale(0.1f, 0.1f);
                canvasPanel.WorkingFigures.AddFigure(figure);
            }
            canvasPanel.ZoomFit();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            lock (this.alarmedData)
            {
                alarmedData.RemoveAll(f => f.SheetSubResultList.FindAll(g => g != null).Count == 0);
                try
                {
                    dataGridView1.RowCount = this.alarmedData.Count;
                    List<RepeatedDefectItem> notid = alarmedData.FindAll(f => f.IsAlarmCleared == false);
                    if (notid.Count > 0)
                    {
                        if (this.Visible == false)
                        {
                            this.Show();

                            bool includePinhole = notid.Exists(f => f.SheetSubResultList.First().GetDefectType() == DefectType.PinHole);
                            if (includePinhole)
                                SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, "1");

                            bool includeNoprint = notid.Exists(f => f.SheetSubResultList.First().GetDefectType() == DefectType.Noprint);
                            if (includeNoprint)
                                SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, "1");
                        }
                    }

                    if (this.Visible)
                        this.dataGridView1.Invalidate();
                }
                catch (Exception) { }
                finally { }
            }
        }
    }
}

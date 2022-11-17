using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using UniScanG.UI.Etc;
using UniScanG.Data;
using DynMvp.Base;
using UniScan.Common.Settings;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.UI.Touch;
using System.Diagnostics;

namespace UniScanG.Gravure.UI.Report
{
    public partial class MultiSheetResultPanel : UserControl, IMultiLanguageSupport
    {
        private delegate void SingleSheetChartUpdateDelegate(List<MergeSheetResult> mergeSheetResult);
        private delegate void MultiSheetChartUpdateDelegate(RepeatedDefectItem repeatedDefectItem);
        private delegate void UpdateListDelegate();

        ContextInfoForm contextInfoForm;
        CanvasPanel canvasPanel;
        RepeatedDefectItemList repeatedDefectItemList;
        List<MergeSheetResult> selectedSheetList;
        
        Series seriesPinhole;
        Series seriesNoprint;
        Series seriesSheetAttack;
        Series seriesDielectric;
        Series seriesHeight;
        CheckBox[] legendCheckBoxs;
        Series[] serieses;

        bool defectSelected = false;
        bool onUpdate= false;

        public MultiSheetResultPanel()
        {
            InitializeComponent();

            onUpdate = true;

            seriesPinhole = new Series
            {
                Name = DefectType.PinHole.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.PinHole),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                ChartType = SeriesChartType.Line
            };

            seriesNoprint = new Series
            {
                Name = DefectType.Noprint.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Noprint),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                ChartType = SeriesChartType.Line
            };

            seriesSheetAttack = new Series
            {
                Name = DefectType.SheetAttack.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.SheetAttack),
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dot,
                ChartType = SeriesChartType.Line
            };

            seriesDielectric = new Series
            {
                Name = DefectType.Dielectric.ToString(),
                Color = Gravure.Data.ColorTable.GetColor(DefectType.Dielectric),
                BorderWidth = 2,
                BorderDashStyle= ChartDashStyle.Dot,
                ChartType = SeriesChartType.Area
            };

            seriesHeight = new Series
            {
                Name = "Height",
                Color = Color.Red,
                BorderWidth = 3,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartType = SeriesChartType.Line,
                YAxisType = AxisType.Secondary
            };

            Series seriesSelection = new Series
            {
                Name = "Selection",
                Color = Color.Black,
                BorderWidth = 3,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartType = SeriesChartType.Column
            };

            this.chart1.ChartAreas[0].BackColor = Color.Gray;
            this.chart1.Series.Add(seriesPinhole);
            this.chart1.Series.Add(seriesNoprint);
            this.chart1.Series.Add(seriesSheetAttack);
            this.chart1.Series.Add(seriesDielectric);
            this.chart1.Series.Add(seriesSelection);
            this.chart1.Series.Add(seriesHeight);

            //checkPinhole.Text = StringManager.GetString(DefectType.PinHole.ToString());
            //checkNoprint.Text = StringManager.GetString(DefectType.Noprint.ToString());
            //checkSheetattack.Text = StringManager.GetString(DefectType.SheetAttack.ToString());
            //checkDielectric.Text = StringManager.GetString(DefectType.Dielectric.ToString());

            this.legendCheckBoxs = new CheckBox[] { checkPinhole, checkNoprint, checkSheetattack, checkDielectric, checkHeight };
            this.serieses = new Series[] { seriesPinhole, seriesNoprint, seriesSheetAttack, seriesDielectric, seriesHeight };

            this.contextInfoForm = new ContextInfoForm();

            this.repeatedDefectItemList = new RepeatedDefectItemList();

            canvasPanel = new CanvasPanel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.TabIndex = 0;
            canvasPanel.ShowCenterGuide = false;
            canvasPanel.DragMode = DragMode.Pan;
            imagePanel.Controls.Add(canvasPanel);

            canvasPanel.FigureFocused = contextInfoForm.CanvasPanel_FigureFocused;
            canvasPanel.MouseLeaved = contextInfoForm.CanvasPanel_MouseLeaved;
            canvasPanel.FigureClicked = CanvasPanel_FigureClicked;
            canvasPanel.MouseClicked = CanvasPanel_MouseClicked;

            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            chart1.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().ToString(), chart1.ChartAreas[0].AxisX.Title);
            chart1.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().ToString(), chart1.ChartAreas[0].AxisY.Title);
            chart1.ChartAreas[0].AxisY2.Title = StringManager.GetString(this.GetType().ToString(), chart1.ChartAreas[0].AxisY2.Title);
            StringManager.UpdateString(this);
        }

        private void CanvasPanel_MouseClicked(PointF point, ref bool processingCancelled)
        {
            if (defectSelected == false)
                return;

            this.defectSelected = false;
            SelectDefect();
            processingCancelled = false;
        }

        private void CanvasPanel_FigureClicked(Figure figure)
        {
            RepeatedDefectItem item = figure.Tag as RepeatedDefectItem;
            if (item == null)
                return;

            List<RepeatedDefectItem> itemList = new List<RepeatedDefectItem>();
            itemList.Add(item);

            SelectDefect(itemList);
        }

        public void Clear()
        {
            defectImage.Image = null;
            canvasPanel.WorkingFigures.Clear();
            canvasPanel.UpdateImage(null);
            ClearChartData();
        }

        public bool SelectSheet(List<MergeSheetResult> resultList)
        {
            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource(); 
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                // Update Image
                MergeSheetResult lastResult = resultList.Last();
                lastResult.ImportPrevImage();
                if (lastResult.PrevImage != null)
                {
                    canvasPanel.UpdateImage(lastResult.PrevImage);

                }
                else
                {
                    canvasPanel.UpdateImage(lastResult.PrevImage);
                }
                canvasPanel.WorkingFigures.Clear();
                canvasPanel.ZoomFit();

                this.selectedSheetList = resultList.OrderBy(f => f.Index).ToList();

                // Make RepeatedDefect
                this.repeatedDefectItemList.Clear();
                for (int i = 0; i < resultList.Count; i++)
                {
                    if (tokenSource.IsCancellationRequested == true)
                        break;

                    MergeSheetResult result = resultList[i];
                    repeatedDefectItemList.AddResult(result, false);
                }

                UpdateFigure(null);
                UpdateDefectList();
                UpdateChart(selectedSheetList);
            }, tokenSource);
            
            //SelectDefect();

            
            return true;
        }

        private void UpdateDefectList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateListDelegate(UpdateDefectList));
                return;
            }

            this.onUpdate = true;

            defectList.Rows.Clear();
            repeatedDefectItemList.Sort();
            List<DataGridViewRow> newRowList = new List<DataGridViewRow>();
            if (selectedSheetList.Count == 1)
            {
                MergeSheetResult mergeSheetResult = selectedSheetList[0];
                for (int i = 0; i < mergeSheetResult.SheetSubResultList.Count; i++)
                {
                    Data.SheetSubResult subResult = mergeSheetResult.SheetSubResultList[i] as Data.SheetSubResult;
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(defectList);
                    newRow.Cells[0].Value = i + 1;
                    newRow.Cells[1].Value = StringManager.GetString(subResult.GetDefectType().ToString());
                    newRow.Cells[1].ToolTipText = subResult.GetDefectTypeDiscription();

                    newRow.Cells[2].Value = 1;
                    newRow.Tag = subResult;

                    newRowList.Add(newRow);
                }
            }
            else
            {
                for (int i = 0; i < repeatedDefectItemList.Count; i++)
                {
                    SheetSubResult lastResult = repeatedDefectItemList[i].SheetSubResultList.First(f => f != null);
                    if (lastResult == null)
                        continue;

                    DefectType defectType = lastResult.GetDefectType();

                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(defectList);
                    newRow.Cells[0].Value = i + 1;
                    newRow.Cells[1].Value = StringManager.GetString(defectType.ToString());
                    newRow.Cells[2].Value = repeatedDefectItemList[i].ValidItemCount;
                    newRow.Tag = repeatedDefectItemList[i];
                    newRowList.Add(newRow);
                }
            }

            defectList.Rows.AddRange(newRowList.ToArray());
            defectList.ClearSelection();

            this.onUpdate = false;

            //defectList.Rows[0].Selected = true;
        }

        private void UpdateChart(List<MergeSheetResult> mergeSheetResultList)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SingleSheetChartUpdateDelegate(UpdateChart), mergeSheetResultList);
                return;
            }

            ClearChartData();

            DefectType[] defectTypes = new DefectType[] { DefectType.PinHole, DefectType.Noprint, DefectType.SheetAttack, DefectType.Dielectric };
            if (mergeSheetResultList.Count == 1)
            {
                MergeSheetResult mergeSheetResult = mergeSheetResultList[0];
                int sheetNo = mergeSheetResult.Index + 1;

                for (int i = 0; i < 4; i++)
                {
                    List<SheetSubResult> defectList = mergeSheetResult.SheetSubResultList.FindAll(f => f.GetDefectType() == defectTypes[i]);
                    int count = 0; float width = 0; float height = 0;
                    if (defectList.Count > 0)
                    {
                        count = defectList.Count;
                        width = defectList.Average(f => f.RealRegion.Width);
                        height = defectList.Average(f => f.RealRegion.Height);
                    }

                    this.serieses[i].Points.AddXY(sheetNo, count);
                    this.serieses[i].ChartType = SeriesChartType.Column;

                    this.legendCheckBoxs[i].Text = string.Format("{0}\r\nW{1:0.0} / H{2:0.0}", StringManager.GetString(defectTypes[i].ToString()), width, height);
                }
                SetChartAxisXRange(sheetNo - 1, sheetNo + 1);

                seriesHeight.Points.AddXY(sheetNo, mergeSheetResult.SheetSize.Height);
                seriesHeight.ChartType = SeriesChartType.Column;
                chart1.ChartAreas[0].AxisY2.Maximum = mergeSheetResult.SheetSize.Height * 1.1f;
                chart1.ChartAreas[0].AxisY2.Minimum = 0;

            }
            else
            {
                List<Tuple<int, float, float>>[] defectTotal = new List<Tuple<int, float, float>>[4];
                for (int i = 0; i < selectedSheetList.Count; i++)
                {
                    MergeSheetResult msr = selectedSheetList[i];
                    AddChartData(msr.Index, msr.SheetSubResultList, msr.SheetSize.Height);

                    for (int j = 0; j < 4; j++)
                    {
                        if (defectTotal[j] == null)
                            defectTotal[j] = new List<Tuple<int, float, float>>();

                        List<SheetSubResult> defectList = msr.SheetSubResultList.FindAll(f => f.GetDefectType() == defectTypes[j]);
                        int count = defectList.Count;
                        float widthSum = defectList.Sum(f => f.RealRegion.Width);
                        float heightSum = defectList.Sum(f => f.RealRegion.Height);

                        defectTotal[j].Add(new Tuple<int, float, float>(count, widthSum, heightSum));
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    float averageWidth = 0; float averageHeight = 0;
                    int totalCount = defectTotal[i].Sum(f => f.Item1);
                    if (totalCount > 0)
                    {
                        averageWidth = defectTotal[i].Sum(f => f.Item2) / totalCount;
                        averageHeight = defectTotal[i].Sum(f => f.Item3) / totalCount;
                    }
                    this.legendCheckBoxs[i].Text = string.Format("{0}\r\nW{1:0.0} / H{2:0.0}", StringManager.GetString(defectTypes[i].ToString()), averageWidth, averageHeight);

                    this.serieses[i].ChartType = SeriesChartType.Line;
                }
                int xMin = selectedSheetList.Min(f => f.Index);
                int xMax = selectedSheetList.Max(f => f.Index);
                SetChartAxisXRange(xMin, xMax);

                this.seriesHeight.ChartType = SeriesChartType.Line;
                double y2Max = this.seriesHeight.Points.Max(f => f.YValues.Max() * 1.001f);
                double y2Min = this.seriesHeight.Points.Min(f => f.YValues.Min() * 0.999f);
                chart1.ChartAreas[0].AxisY2.Maximum = Math.Max(y2Max, y2Min);
                chart1.ChartAreas[0].AxisY2.Minimum = Math.Min(y2Max, y2Min);
                chart1.ChartAreas[0].AxisY2.LabelStyle.Format = "{F2}";
            }
        }

        private void UpdateChart(RepeatedDefectItem repeatedDefectItem)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MultiSheetChartUpdateDelegate(UpdateChart), repeatedDefectItem);
                return;
            }

            ClearChartData();

            // Selected Defect Trand
            AddChartData(repeatedDefectItem.SheetSubResultList);


            for (int i = 0; i < this.serieses.Length; i++)
                this.serieses[i].ChartType = SeriesChartType.Line;

            this.chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 10;
            this.chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
        }

        private void ClearChartData()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].Points.Clear();
        }

        private void AddChartData(int sheetNo, List<SheetSubResult> sheetSubResultList, float sheetHeight)
        {
            int pinhold = 0, noprint = 0, sheetAttack = 0, dielectric = 0;
            sheetSubResultList.ForEach(f =>
            {
                DefectType defectType = f.GetDefectType();
                switch (defectType)
                {
                    case DefectType.SheetAttack:
                        sheetAttack++;
                        break;
                    case DefectType.Noprint:
                        noprint++;
                        break;
                    case DefectType.Dielectric:
                        dielectric++;
                        break;
                    case DefectType.PinHole:
                        pinhold++;
                        break;

                }
            });

            this.seriesPinhole.Points.AddXY(sheetNo, pinhold);
            this.seriesNoprint.Points.AddXY(sheetNo, noprint);
            this.seriesSheetAttack.Points.AddXY(sheetNo, sheetAttack);
            this.seriesDielectric.Points.AddXY(sheetNo, dielectric);

            this.seriesHeight.Points.AddXY(sheetNo, sheetHeight);
        }

        private void AddChartData(List<SheetSubResult> sheetSubResultList)
        {
            int x = selectedSheetList.Count;
            for (int i = 0; i < sheetSubResultList.Count; i++)
            {
                x--;
                int y = sheetSubResultList[i] != null ? 1 : 0;
                chart1.Series["Selection"].Points.AddXY(x, y);
            }

            //if (x > 0)
            //  chart1.Series["Selection"].Points.AddXY(x - 1, 0);

            //SetChartAxisXRange(x, resultList.Count);

            int xMin = 0;
            int xMax = selectedSheetList.Count;
            SetChartAxisXRange(xMin, xMax);
        }

        public List<SheetSubResult> GetDefectList()
        {
            List<SheetSubResult> imageList = new List<SheetSubResult>();
            foreach (DataGridViewRow row in this.defectList.Rows)
            {
                if(row.Tag is SheetSubResult)
                {
                    SheetSubResult ssr = row.Tag as SheetSubResult;
                    imageList.Add(ssr);
                }
                else if(row.Tag is RepeatedDefectItem)
                {
                    RepeatedDefectItem rdi = row.Tag as RepeatedDefectItem;
                    //SheetSubResult ssr = rdi.SheetSubResultList.Find(f => f.Image != null);
                    //imageList.Add(ssr);
                }
            }

            return imageList;
        }

        private void SetChartAxisXRange(int min, int max)
        {
            double curMax = chart1.ChartAreas[0].AxisX.Maximum;
            double curMin = chart1.ChartAreas[0].AxisX.Minimum;

            if(curMax < min)
            {
                chart1.ChartAreas[0].AxisX.Maximum = max;
                chart1.ChartAreas[0].AxisX.Minimum = min;
            }
            else //if(curMin < max)
            {
                chart1.ChartAreas[0].AxisX.Minimum = min;
                chart1.ChartAreas[0].AxisX.Maximum = max;
            }

        }

        private void UpdateFigure(List<RepeatedDefectItem> itemList)
        {
            this.canvasPanel.WorkingFigures.Clear();

            if (itemList == null || itemList.Count == 0)
            {
                for (int i = 0; i < repeatedDefectItemList.Count; i++)
                    AddFigure(repeatedDefectItemList[i]);
            }
            else
            {
                itemList.ForEach(f => AddFigure(f));
            }
            this.canvasPanel.Invalidate();
        }

        private void AddFigure(RepeatedDefectItem item)
        {
            //if (item.RepeatRatio > 01)
            {
                float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
                Figure figure = item.SheetSubResultList.First(f => f != null)?.GetFigure(50, resizeRatio);
                if (figure != null)
                {
                    figure.Tag = item;
                    this.canvasPanel.WorkingFigures.AddFigure(figure);
                }
            }
        }

        private void SelectDefect(List<SheetSubResult> sheetSubResultList)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            Bitmap bitmap = null;
            Bitmap bitmapB = null;
            string sizeW = "", sizeH = "", sizeV = "";

            canvasPanel.WorkingFigures.Clear();
            foreach (SheetSubResult sheetSubResult in sheetSubResultList)
            {
                if (sheetSubResult != null)
                    canvasPanel.WorkingFigures.AddFigure(sheetSubResult.GetFigure(50, resizeRatio));
            }
            canvasPanel.Invalidate();

            if (sheetSubResultList.Count == 1)
            {
                if (sheetSubResultList.Exists(f => f.Image != null) == true)
                {
                    Data.SheetSubResult firstSheetSubResult = sheetSubResultList.First(f => f.Image != null) as Data.SheetSubResult;
                    if (firstSheetSubResult != null)
                    {
                        bitmap = firstSheetSubResult?.Image;
                        bitmapB = firstSheetSubResult?.BufImage;
                        sizeW = string.Format("{0:0.0}", firstSheetSubResult.RealRegion.Width);
                        sizeH = string.Format("{0:0.0}", firstSheetSubResult.RealRegion.Height);
                        if(firstSheetSubResult is Data.SheetSubResult)
                            sizeV = string.Format("{0}",firstSheetSubResult.SubtractValueMax);
                    }
                }
            }
            defectImage.Image = bitmap;
            defectImage.Tag = bitmapB;
            defectSizeW.Text = sizeW;
            defectSizeH.Text = sizeH;
            defectSizeV.Text = sizeV;
        }

        private void SelectDefect(List<RepeatedDefectItem> itemList)
        {
            onUpdate = true;

            Bitmap bitmap = null, bitmapB=null;
            string sizeW = "", sizeH = "",sizeV = ""; ;

            ClearChartData();
            if (itemList.Count == 1)
            {
                Data.SheetSubResult lastResult = itemList[0].SheetSubResultList.First(f => f != null) as Data.SheetSubResult;
                if (lastResult != null)
                {
                    bitmap = lastResult.Image;
                    bitmapB = lastResult.BufImage;

                    sizeW = string.Format("{0:F1}", lastResult.RealRegion.Width);
                    sizeH = string.Format("{0:F1}", lastResult.RealRegion.Height);
                    sizeV = string.Format("{0}", lastResult.SubtractValueMax);
                }
                UpdateChart(itemList[0]);
            }

            UpdateFigure(itemList);

            defectImage.Image = bitmap;
            defectImage.Tag= bitmapB;
            defectSizeW.Text = sizeW;
            defectSizeH.Text = sizeH;
            defectSizeV.Text = sizeV;

            this.defectSelected = (itemList == null || itemList.Count > 0);

            onUpdate = false;
        }

        private void SelectDefect()
        {
            onUpdate = true;

            List<SheetSubResult> itemList = new List<SheetSubResult>();
            List<RepeatedDefectItem> itemList2 = new List<RepeatedDefectItem>();
            for (int i = 0; i < defectList.SelectedRows.Count; i++)
            {
                itemList.Add(defectList.SelectedRows[i].Tag as SheetSubResult);
                itemList2.Add(defectList.SelectedRows[i].Tag as RepeatedDefectItem);
            }
            itemList.RemoveAll(f => f == null);
            itemList2.RemoveAll(f => f == null);
            Debug.Assert(itemList.Count == 0 || itemList2.Count == 0);
            if (itemList.Count == 0)
                SelectDefect(itemList2);
            else
                SelectDefect(itemList);


            //if (this.selectedSheetList.Count == 1)
            //{
            //    List<SheetSubResult> itemList = new List<SheetSubResult>();
            //    for (int i = 0; i < defectList.SelectedRows.Count; i++)
            //        itemList.Add(defectList.SelectedRows[i].Tag as SheetSubResult);
            //    itemList.RemoveAll(f => f == null);
            //    SelectDefect(itemList);
            //}
            //else
            //{
            //    List<RepeatedDefectItem> itemList = new List<RepeatedDefectItem>();
            //    for (int i = 0; i < defectList.SelectedRows.Count; i++)
            //        itemList.Add(defectList.SelectedRows[i].Tag as RepeatedDefectItem);
            //    itemList.RemoveAll(f => f == null);
            //    SelectDefect(itemList);
            //}

            onUpdate = false;
        }
        private void defectList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (defectList.SelectedRows.Count == 0)
                return;

            if (this.selectedSheetList.Count == 1)
            {
                List<SheetSubResult> itemList = new List<SheetSubResult>();
                for (int i = 0; i < defectList.SelectedRows.Count; i++)
                    itemList.Add(defectList.SelectedRows[i].Tag as SheetSubResult);
                itemList.RemoveAll(f => f == null);
                SelectDefect(itemList);
            }
            else
            {
                List<RepeatedDefectItem> itemList = new List<RepeatedDefectItem>();
                for (int i = 0; i < defectList.SelectedRows.Count; i++)
                    itemList.Add(defectList.SelectedRows[i].Tag as RepeatedDefectItem);
                itemList.RemoveAll(f => f == null);
                SelectDefect(itemList);
            }
        }

        private void check_CheckedChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            for (int i = 0; i < legendCheckBoxs.Length; i++)
                EnableSeries(i, this.legendCheckBoxs[i].Checked);

            //seriesPinHole.Enabled = checkPinhole.Checked;
            //seriesNoprint.Enabled = checkNoprint.Checked;
            //seriesSheetAttack.Enabled = checkSheetattack.Checked;
            //seriesDielectric.Enabled = checkDielectric.Checked;
        }

        private void EnableSeries(int i, bool visible)
        {
            Series target = this.serieses[i];
            if(visible)
            {
                if (chart1.Series.Contains(target) == false)
                    chart1.Series.Add(target);
            }else
            {
                if (chart1.Series.Contains(target))
                    chart1.Series.Remove(target);
            }
        }

        private void MultiSheetResultPanel_Load(object sender, EventArgs e)
        {
            checkPinhole.Checked = seriesPinhole.Enabled;
            checkNoprint.Checked = seriesNoprint.Enabled;
            checkSheetattack.Checked = seriesSheetAttack.Enabled;
            checkDielectric.Checked = seriesDielectric.Enabled;
            checkHeight.Checked = seriesHeight.Enabled;


            Gravure.Data.ColorTable.UpdateControlColor(checkPinhole, DefectType.PinHole);
            Gravure.Data.ColorTable.UpdateControlColor(checkNoprint, DefectType.Noprint);
            Gravure.Data.ColorTable.UpdateControlColor(checkSheetattack, DefectType.SheetAttack);
            Gravure.Data.ColorTable.UpdateControlColor(checkDielectric, DefectType.Dielectric);
            Gravure.Data.ColorTable.UpdateControlColor(checkHeight, Color.Red, Color.Empty);
        }

        private void defectImage_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = defectImage.Image as Bitmap;
            defectImage.Image = defectImage.Tag as Bitmap;
            defectImage.Tag = bitmap;
        }
    }
}

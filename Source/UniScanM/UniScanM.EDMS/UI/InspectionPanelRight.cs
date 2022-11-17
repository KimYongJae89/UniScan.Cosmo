using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.Devices.Light;
using UniScanM.EDMS.Operation;
using UniScanM.EDMS.Data;
using DynMvp.InspData;
using UniScanM.EDMS.MachineIF;
using UniScanM.Data;
using UniScanM.Operation;
using UniScanM.EDMS.Settings;

namespace UniScanM.EDMS.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        private CanvasPanel canvasPanel = null;

        public InspectionPanelRight()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.DragMode = DragMode.Pan;
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.SizeChanged += DrawBox_SizeChanged;
            this.canvasPanel.NoneClickMode = true;
            this.canvasPanel.BackColor = Color.CornflowerBlue;

            panelImage.Controls.Add(this.canvasPanel);
            
            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);

            EDMSSettings.Instance().AdditionalSettingChangedDelegate += this.UpdateParamControl;
        }
        
        private void DrawBox_SizeChanged(object sender, EventArgs e)
        {
            //UpdateModelData();
            canvasPanel.ZoomFit();
        }

        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            if (SystemState.Instance().GetOpState() != OpState.Idle)
            {
                if (myInspectionResult.IsWaitState)
                {
                    labelState.ForeColor = Color.Black;
                    labelState.BackColor = Color.Yellow;
                    labelState.Text = string.Format("{0} - {1}[m]", StringManager.GetString(this.GetType().FullName, "Wait"), myInspectionResult.RemainWaitDist);
                }
                else if (myInspectionResult.IsZeroingState)
                {
                    labelState.ForeColor = Color.Black;
                    labelState.BackColor = Color.Yellow;
                    labelState.Text = StringManager.GetString(this.GetType().FullName, "Zeroing");
                }
                else
                {
                    labelState.ForeColor = Color.White;
                    labelState.BackColor = Color.Green;
                    labelState.Text = StringManager.GetString(this.GetType().FullName, "Measure");
                    switch (myInspectionResult.Judgment)
                    {
                        case Judgment.Accept:
                        case Judgment.FalseReject:
                            break;
                        case Judgment.Reject:
                            break;
                        case Judgment.Skip:
                            break;
                        case Judgment.Warn:
                            break;
                    }
                }

                progressBarZeroing.Value = (int)(myInspectionResult.ZeroingNum / 10.0 * 100.0);
            }

            Bitmap bitmap = null;
            lock (myInspectionResult.DisplayBitmap)
                bitmap = (Bitmap)myInspectionResult.DisplayBitmap.Clone();

            Pen rulerLinePenV = new Pen(Color.Green, 100);
            Pen rulerLinePenH = new Pen(Color.Green, 2);

            FigureGroup figureGroup = new FigureGroup();
            PointF verticalOriginStartPt = new PointF(3 * 1000 / 5, 0);
            PointF verticalOriginEndPt = new PointF(3 * 1000 / 5, bitmap.Height);
            figureGroup.AddFigure(new LineFigure(verticalOriginStartPt, verticalOriginEndPt, rulerLinePenV));

            float rulerHeight = bitmap.Height != 0 ? bitmap.Height - (bitmap.Height / 4) : 100;
            PointF rulerStartPt = new PointF(0, rulerHeight);
            PointF rulerEndPt = new PointF(bitmap.Width, rulerHeight);
            figureGroup.AddFigure(new LineFigure(rulerStartPt, rulerEndPt, rulerLinePenH));

            for (int gridIndex = 0; gridIndex < bitmap.Width; gridIndex += 200)
            {
                PointF gridStartPt = new PointF(gridIndex, rulerHeight - 200);
                PointF gridEndPt = new PointF(gridIndex, rulerHeight);
                figureGroup.AddFigure(new LineFigure(gridStartPt, gridEndPt, rulerLinePenH));
            }

            double[] pixelPosition = myInspectionResult.EdgePositionResult;
            
            Pen arrowPen = new Pen(Color.Red, 2);
            arrowPen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            Pen linePen = new Pen(Color.Yellow, 2);
            

            PointF filmVerticalStartPt = new PointF((float)pixelPosition[0], 0);
            PointF filmVerticalEndPt = new PointF((float)pixelPosition[0], bitmap.Height);
            PointF coatingVerticalStartPt = new PointF((float)pixelPosition[1], 0);
            PointF coatingVerticalEndPt = new PointF((float)pixelPosition[1], bitmap.Height);
            PointF printingVerticalStartPt = new PointF((float)pixelPosition[2], 0);
            PointF printingVerticalEndPt = new PointF((float)pixelPosition[2], bitmap.Height);

            if (EDMSSettings.Instance().IsFrontPosition)
            {
                verticalOriginStartPt = new PointF(bitmap.Width - 3 * 1000 / 5, 0);
                verticalOriginEndPt = new PointF(bitmap.Width - 3 * 1000 / 5, bitmap.Height);

                filmVerticalStartPt = new PointF((float)(bitmap.Width - pixelPosition[0]), 0);
                filmVerticalEndPt = new PointF((float)(bitmap.Width - pixelPosition[0]), bitmap.Height);
                coatingVerticalStartPt = new PointF((float)(bitmap.Width - pixelPosition[1]), 0);
                coatingVerticalEndPt = new PointF((float)(bitmap.Width - pixelPosition[1]), bitmap.Height);
                printingVerticalStartPt = new PointF((float)(bitmap.Width - pixelPosition[2]), 0);
                printingVerticalEndPt = new PointF((float)(bitmap.Width - pixelPosition[2]), bitmap.Height);
            }

            /*
            figureGroup.AddFigure(new LineFigure(filmStartPt, filmEndPt, arrowPen));          0
            figureGroup.AddFigure(new LineFigure(coatingFilmStartPt, coatingFilmEndPt, arrowPen));
            figureGroup.AddFigure(new LineFigure(printingCoatingStartPt, printingCoatingEndPt, arrowPen));
            */

            figureGroup.AddFigure(new LineFigure(filmVerticalStartPt, filmVerticalEndPt, linePen));
            figureGroup.AddFigure(new LineFigure(coatingVerticalStartPt, coatingVerticalEndPt, linePen));
            figureGroup.AddFigure(new LineFigure(printingVerticalStartPt, printingVerticalEndPt, linePen));
                               
            canvasPanel.ClearFigure();
            canvasPanel.WorkingFigures.AddFigure(figureGroup);
            
            UpdateData(myInspectionResult.TotalEdgePositionResult, bitmap);
        }

        private delegate void UpdateDataDelegate(double[] position, Bitmap bitmap);
        private void UpdateData(double[] position, Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData), position, bitmap);
                return;
            }

            UiHelper.SuspendDrawing(labelFilmValue);
            UiHelper.SuspendDrawing(labelCoatingValue);
            UiHelper.SuspendDrawing(labelPrintingValue);
            UiHelper.SuspendDrawing(labelPrintingPos);

            double[] datas = new double[3];
            datas[0] = position[(int)Data.DataType.FilmEdge];
            datas[1] = Math.Max(0, position[(int)Data.DataType.Coating_Film]);
            datas[2] = Math.Max(0, position[(int)Data.DataType.Printing_Coating]);
            double posPrint = Array.TrueForAll(datas, f => f > 0) ? datas.Sum() : 0;

            labelFilmValue.Text = String.Format("{0:0.000}", datas[0]);
            labelCoatingValue.Text = String.Format("{0:0.000}", datas[1]);
            labelPrintingValue.Text = String.Format("{0:0.000}", datas[2]);
            labelPrintingPos.Text = String.Format("{0:0.000}", posPrint);

            UiHelper.ResumeDrawing(labelFilmValue);
            UiHelper.ResumeDrawing(labelCoatingValue);
            UiHelper.ResumeDrawing(labelPrintingValue);
            UiHelper.ResumeDrawing(labelPrintingPos);

            bool zoomFitRequred = canvasPanel.Image == null;
            canvasPanel.UpdateImage(bitmap);
            
            if (zoomFitRequred == true)
                canvasPanel.ZoomFit();
                                                                                                                     
            canvasPanel.Invalidate();
        }                                                   

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        bool onUpdate = false;
        private void UpdateModelData()
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                onUpdate = true;
                backLightValue.Value = currentModel.LightParamSet.LightParamList[0].LightValue.Value[0];
                frontLightValue.Value = currentModel.LightParamSet.LightParamList[0].LightValue.Value[1];
                numericUpDownFilmThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).FilmThreshold;
                numericUpDownCoatingThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).CoatingThreshold;
                numericUpDownPrintingThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).PrintingThreshold;
                onUpdate = false;
            }
        }

        private void lightValue_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                currentModel.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)backLightValue.Value;
                currentModel.LightParamSet.LightParamList[0].LightValue.Value[1] = (int)frontLightValue.Value;
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);

                SystemManager.Instance().InspectRunner.PostEnterWaitInspection();

                //float lineSpd = (float)SystemManager.Instance().InspectStarter.GetLineSpeed();
                //float baseLineSpd = Settings.EDMSSettings.Instance().LightBaseSpeed;

                //LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
                //LightValue lightValue = (LightValue)SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Clone();
                //if (lineSpd > 0 && baseLineSpd > 0)
                //{
                //    float mul = lineSpd / baseLineSpd;
                //    for (int i = 0; i < lightValue.Value.Length; i++)
                //    {
                //        int newValue = (int)Math.Round(lightValue.Value[i] * mul);
                //        newValue = Math.Min(newValue, lightCtrl.GetMaxLightLevel());
                //        newValue = Math.Max(newValue, 0);
                //        lightValue.Value[i] = newValue;
                //    }
                //}
                //lightCtrl.TurnOn(lightValue);
            }
        }

        private void numericUpDownFilmThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                ((EDMSParam)currentModel.InspectParam).FilmThreshold = (double)numericUpDownFilmThreshold.Value;
            }
        }

        private void numericUpDownCoatingThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                ((EDMSParam)currentModel.InspectParam).CoatingThreshold = (double)numericUpDownCoatingThreshold.Value;
            }
        }

        private void numericUpDownPrintingThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                ((EDMSParam)currentModel.InspectParam).PrintingThreshold = (double)numericUpDownPrintingThreshold.Value;
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        public void Initialize() { }

        public delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if(InvokeRequired)
            {
                this.Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            canvasPanel.ClearFigure();
            canvasPanel.UpdateImage(null);
            canvasPanel.Invalidate();

            labelCoatingValue.Text = "0";
            labelPrintingValue.Text = "0";
            labelPrintingPos.Text = "0";
        }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(DynMvp.Data.Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        private void buttonPLCTeset_Click(object sender, EventArgs e)
        {

         
        }

        private void buttonStateReset_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ResetState();
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

            ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();
        }

        void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");

            groupThresHold.Enabled = !flag;
            buttonSave.Enabled = (!flag) && (EDMS.Settings.EDMSSettings.Instance().AutoLight == false);
            groupLightParameter.Enabled = (!flag) && (EDMS.Settings.EDMSSettings.Instance().AutoLight == false);
        }

        private void InspectionPanelRight_Load(object sender, EventArgs e)
        {
            UpdateModelData();
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                //public delegate void (OpState curOpState, OpState prevOpState);
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            if (curOpState == OpState.Idle)
            {
                labelState.ForeColor = SystemColors.ControlText;
                labelState.BackColor = SystemColors.Control;
                labelState.Text = "";
                progressBarZeroing.Value = 0;
                return;
            }
        }
    }
}

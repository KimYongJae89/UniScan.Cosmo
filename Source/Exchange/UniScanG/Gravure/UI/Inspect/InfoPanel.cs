using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.MachineIF;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector;
using UniScanG.UI;

namespace UniScanG.Gravure.UI.Inspect
{
    public partial class InfoPanel : UserControl, IInspectStateListener, IOpStateListener,IMultiLanguageSupport, IModelListener
    {
        List<float> sheetHeightList = new List<float>();

        public InfoPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);
            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);

            Clear();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void InspectDoneDelegate(InspectionResult inspectionResult);
        public void InspectDone(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InspectDoneDelegate(InspectDone), inspectionResult);
                return;
            }

            if (inspectionResult.Judgment == Judgment.Skip)
                return;

            float height = 0;
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName))
                height = (inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as MergeSheetResult).SheetSize.Height;
            else if (inspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
                height = (inspectionResult.AlgorithmResultLDic[Detector.TypeName] as DetectorResult).SheetSize.Height;

            if (height > 0)
            {
                this.sheetHeightList.Add(height);
                this.sheetHeightList.Sort();
            }

            UpdateData();

            TimeSpan ts = new TimeSpan();
            foreach (KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
                ts += pair.Value.SpandTime;
            //ts += inspectionResult.ExportTime;

            processTime.Text = string.Format("{0} s", ts.ToString("ss\\.fff"));
        }
        
        private void UpdateDefectInfo()
        {
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG == null)
                return;

            if (sheetRadioButton.Checked == true)
            {
                sheetAttack.Text = productionG.SheetAttackPatternNum.ToString();
                noPrint.Text = productionG.NoPrintPatternNum.ToString();
                dielectric.Text = productionG.DielectricPatternNum.ToString();
                pinHole.Text = productionG.PinHolePatternNum.ToString();
            }
            else if (patternRadioButton.Checked == true)
            {
                sheetAttack.Text = productionG.SheetAttackNum.ToString();
                noPrint.Text = productionG.NoPrintNum.ToString();
                dielectric.Text = productionG.DielectricNum.ToString();
                pinHole.Text = productionG.PinHoleNum.ToString();
            }
        }

        delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG == null)
                return;

            lotNo.Text = productionG.LotNo;
            startTime.Text = productionG.StartTime.ToString("HH:mm");
            endTime.Text = DateTime.Now.ToString("HH:mm");

            productionTotal.Text = productionG.Done.ToString();
            productionNG.Text = productionG.Ng.ToString();
            productionRatio.Text = string.Format("{0:0.0} %", productionG.NgRatio);

            if (this.sheetHeightList.Count > 10)
            {
                int sp1 = this.sheetHeightList.Count / 10;
                int sp2 = this.sheetHeightList.Count - sp1;

                float lower = this.sheetHeightList.GetRange(0, sp1).Average();
                float middle = this.sheetHeightList.GetRange(sp1, sp2 - sp1).Average();
                float upper = this.sheetHeightList.GetRange(sp2, sp1).Average();

                productionHeightMax.Text = upper.ToString("F2");
                productionHeightAverage.Text = middle.ToString("F2");
                productionHeightMin.Text = lower.ToString("F2");

                AdditionalSettings additionalSettings = (AdditionalSettings)AdditionalSettings.Instance();
                if (additionalSettings.SheetLengthAlarm.Use)
                {
                    bool alarm = (this.sheetHeightList.Count > additionalSettings.SheetLengthAlarm.Count) &&
                        (middle - lower > additionalSettings.SheetLengthAlarm.Value) &&
                        (upper - middle > additionalSettings.SheetLengthAlarm.Value);
                    if (alarm)
                        SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_SHTLEN, "1");

                }
            }
            else
            {
                productionHeightMax.Text = "";
                productionHeightAverage.Text = "";
                productionHeightMin.Text = "";
            }

            todayQuntity.Text = string.Format("{0} Roll(s)", SystemManager.Instance().ProductionManager.GetTodayCount());

            UpdateDefectInfo();
        }

        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new InspectStateChangedDelegate(InspectStateChanged), curInspectState);
                return;
            }
            
            switch (curInspectState)
            {
                case UniEye.Base.Data.InspectState.Run:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Run;
                    break;
                case UniEye.Base.Data.InspectState.Wait:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
                case UniEye.Base.Data.InspectState.Done:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
            }
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
            switch (curOpState)
            {
                case OpState.Idle:
                    status.Appearance.BackColor = Colors.Idle;
                    break;
                case OpState.Wait:
                    status.Appearance.BackColor = Colors.Wait;
                    this.sheetHeightList.Clear();
                    break;
                case OpState.Alarm:
                    status.Appearance.BackColor = Colors.Alarm;
                    break;
                case OpState.Inspect:
                    InspectStateChanged(SystemState.Instance().InspectState);
                    //UpdateData();
                    break;
            }
            UpdateData();
        }

        private void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Clear));
                return;
            }

            SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_SHTLEN, "0");
            this.sheetHeightList.Clear();

            lotNo.Text = string.Empty;
            startTime.Text = string.Empty;

            processTime.Text = string.Empty;

            productionTotal.Text = string.Empty;
            productionNG.Text = string.Empty;
            productionRatio.Text = string.Empty;

            sheetAttack.Text = string.Empty;
            noPrint.Text = string.Empty;
            dielectric.Text = string.Empty;
            pinHole.Text = string.Empty;

            productionHeightMax.Text = string.Empty;
            productionHeightAverage.Text = string.Empty;
            productionHeightMin.Text = string.Empty;
        }

        public void ModelChanged()
        {
            Clear();
        }
        
        public void ModelTeachDone(int camId) { }
        public void ModelRefreshed() { }

        Thread updateThread = null;
        int bufferCount = 0, usageCount = 0;
        float bufferLoad = 0, presentSpeed = 0, targetSpeed = 0;
        bool dataUpdated = false;
        private void bufferUsageUpdateTimer_Tick(object sender, EventArgs e)
        {
            bufferUsageUpdateTimer.Interval = 500;

            if (dataUpdated)
            {
                progressSpd.Text = string.Format("{0:0.0} / {1:0.0}", presentSpeed, targetSpeed);

                bufferUsage.Maximum = bufferCount;
                bufferUsage.Value = usageCount;
                bufferUsage.Text = string.Format("{0:0.0}% ({1}/{2})", bufferLoad, usageCount, bufferCount);
                dataUpdated = false;
            }
        }

        private void InfoPanel_Load(object sender, EventArgs e)
        {
            bufferUsageUpdateTimer.Start();

            this.updateThread = new Thread(() =>
             {
                 while (true)
                 {
                     Thread.Sleep(100);
                     if (dataUpdated)
                         continue; 

                     InspectRunnerInspectorG inspectRunnerInspectorG = SystemManager.Instance().InspectRunner as InspectRunnerInspectorG;
                     if (inspectRunnerInspectorG != null)
                     {
                         bufferCount = inspectRunnerInspectorG.ProcessBufferManager.Count;
                         usageCount = inspectRunnerInspectorG.ProcessBufferManager.UsingCount;
                         bufferLoad = (bufferCount == 0) ? 0 : usageCount * 100.0f / bufferCount;
                     }

                     InspectRunnerMonitorG inspectRunnerMonitorG = SystemManager.Instance().InspectRunner as InspectRunnerMonitorG;
                     if (inspectRunnerMonitorG != null)
                     {
                         bufferCount = InspectRunnerMonitorG.MAX_BUFFER_SIZE;
                         usageCount = inspectRunnerMonitorG.SheetCombinerTaskList.Count;
                         bufferLoad = (bufferCount == 0) ? 0 : usageCount * 100.0f / bufferCount;
                     }

                     ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
                     targetSpeed = productionG == null ? 0 : productionG.LineSpeedMpm;
                     presentSpeed = 0;
                     MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList?.GetProtocol(UniScanGMachineIfCommon.GET_PRESENT_SPEED);
                     MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
                     if (responce != null && responce.IsGood)
                         presentSpeed = Convert.ToInt32(responce.ReciveData, 16) / 10.0f;
                     dataUpdated = true;
                 }
            });

            this.updateThread.IsBackground = true;
            this.updateThread.Start();
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
    }
}
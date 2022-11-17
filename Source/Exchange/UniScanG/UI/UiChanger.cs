using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanG.Settings;
using UniScanG.UI.Model;
using System.Windows.Forms;
using UniScan.Common.UI;
using UniScan.Common.Settings.UI;
using UniScan.Common;
using System;
using System.Collections.Generic;
using UniScanG.UI.Teach.Monitor;
using UniScan.Common.Data;
using UniScanG.Screen.UI;
using UniScan.Common.Util;
using UniScan.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.UI.Inspect;
using System.Drawing;
using System.IO;

namespace UniScanG.UI
{
    public interface IModelImagePanel
    {
        void SetPreview(ModelDescription modelDescription);
    }

    public interface IVncContainer
    {
        void ProcessStarted(IVncControl startVncViewer);
        void ProcessExited();
    }

    public delegate void UpdateResultDelegate(Bitmap image, List<DataGridViewRow> dataGridViewRowList);

    public interface IInspectDefectPanel
    {
        void AddDelegate(UpdateResultDelegate UpdateResultDelegate);
        bool BlockUpdate { get; set; }
        void Reset();
    }

    public abstract class MonitorUiChanger : UiChanger
    {
        public abstract Control CreateTeachSettingPanel();
        
        public override IMainTabPage CreateTeachPage()
        {
            return new UI.Teach.Monitor.TeachPage();
        }

        public override IMainTabPage CreateInspectPage()
        {
            return new UI.Inspect.InspectPage();
        }
        
        public abstract List<Control> GetTeachButtons(IVncContainer teachPage);

        public List<Control> GetReportButtons(IVncContainer reportPage)
        {
            List<Control> controlList = new List<Control>();
            
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_REPORT, inspector, reportPage.ProcessStarted, reportPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }

        public List<Control> GetInspectButtons(IVncContainer inspectPage)
        {
            List<Control> controlList = new List<Control>();
            
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_INSPECT, inspector, inspectPage.ProcessStarted, inspectPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }

        public override List<IStatusStripPanel> GetStatusStrip()
        {
            List<IStatusStripPanel> statusStripList = new List<IStatusStripPanel>();

            // Right align
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj info in server.GetInspectorList())
            {
                InspectorStatusStripPanel statusStripPanel = new InspectorStatusStripPanel(info);
                statusStripPanel.Dock = DockStyle.Right;
                statusStripList.Add(statusStripPanel);
            }

            // Left align
            List<DriveInfo> driveInfoList = DynMvp.Data.DataCopier.GetTargetDriveInfoList();
            foreach (DriveInfo driveInfo in driveInfoList)
            {
                VolumePanel volumePanel = new VolumePanel(driveInfo);
                volumePanel.Dock = DockStyle.Left;
                statusStripList.Add(volumePanel);
            }

            return statusStripList;
        }
    }

    public abstract class InspectorUiChanger : UiChanger
    {
        public override IMainTabPage CreateInspectPage()
        {
            this.inspect = new UI.Inspect.InspectPage();
            return this.inspect;
        }

        public override ISettingPage CreateSettingPage()
        {
            this.setting = null;
            return (ISettingPage)this.setting;
        }

        public override List<IStatusStripPanel> GetStatusStrip()
        {
            return new List<IStatusStripPanel>();
        }

        public abstract IModellerControl CreateImageController();
        public abstract IModellerControl CreateParamController();

        public abstract IModellerControl CreateTeachToolBar();
    }

    public abstract class UiChanger : UniEye.Base.UI.UiChanger
    {
        public IMainForm MainForm { get => mainForm; }
        public IMainTabPage InspectControl { get => inspect; }
        public IMainTabPage ModelControl { get => model; }
        public IMainTabPage TeachControl { get => teach; }
        public IMainTabPage SettingControl { get => setting; }
        public IMainTabPage ReportControl { get => report; }

        public IMainTabPage LogControl { get => report; }

        protected IUiControlPanel uiControlPanel;

        protected IMainForm mainForm = null;
        protected IMainTabPage inspect = null;
        protected IMainTabPage model = null;
        protected IMainTabPage teach = null;
        protected IMainTabPage report = null;
        protected IMainTabPage setting = null;
        protected IMainTabPage log = null;


        public override IMainForm CreateMainForm()
        {
            this.mainForm = new MonitorMainform(uiControlPanel, "");
            return this.mainForm;
        }

        public IMainTabPage CreateLogPage()
        {
            this.log = new UniScanG.UI.Log.LogPage();
            return this.log;
        }

        //public override IUiControlPanel CreateUiControlPanel()
        //{
        //    if (uiControlPanel == null)
        //    {
        //        this.inspect = CreateInspectPage();
        //        this.model = CreateModelPage();
        //        this.teach = CreateTeachPage();
        //        this.setting = CreateSettingPage();
        //        this.report = CreateReportPage();

        //        uiControlPanel = new MainTabPanel((Control)inspect, (Control)model, (Control)teach, (Control)setting, (Control)report);
        //    }

        //    return uiControlPanel;
        //}

        public IMainTabPage CreateModelPage()
        {
            this.model = new UI.Model.ModelPage();
            return this.model;
        }

        public IMainTabPage CreateReportPage()
        {
            this.report = new UI.Report.ReportPage();
            return this.report;
        }

        public abstract IMainTabPage CreateInspectPage();
        public abstract Control CreateDefectInfoPanel();
        public abstract IInspectDefectPanel CreateDefectPanel();

        public abstract IMainTabPage CreateTeachPage();

        public abstract List<IStatusStripPanel> GetStatusStrip();

        public override IReportPanel CreateReportPanel() { return null; }
        public override void EnableTargetParamControls(TargetParamControl targetParamControl) { }
        public override void SetupVisionParamControl(VisionParamControl visionParamControl) { }
        public override void ChangeModellerMenu(ModellerPage modellerPage) { }
        public override string[] GetProbeNames() { return null; }
        public override string[] GetStepTypeNames() { return null; }
        public override IDefectReportPanel CreateDefectReportPanel() { return null; }
        public override void BuildAdditionalAlgorithmTypeMenu(ModellerPage modellerPage, ToolStripItemCollection dropDownItems) { }
    }}

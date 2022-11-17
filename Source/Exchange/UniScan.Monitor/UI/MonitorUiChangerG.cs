using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanG.Settings;
using UniScanG.UI.Model;
using System.Windows.Forms;
using UniScan.Common;
using System;
using UniScanG.UI;
using System.Collections.Generic;
using UniScan.Common.Data;
using UniScanG.UI.Teach.Monitor;
using UniScan.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScan.Common.Util;
using UniScanG.Gravure.UI.Teach;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Gravure.UI.Teach.Monitor;
using UniScan.Common.Settings;
using UniScanG.Gravure.UI;
using UniScan.Monitor.Device.Gravure.UI;
using UniScan.Monitor.Settings.Monitor;
using System.IO;
using DynMvp.Data;
using UniScan.Monitor.Device.Gravure.Laser;

namespace UniScan.Monitor.UI
{
    public class MonitorUiChangerG : MonitorUiChanger
    {
        public MonitorUiChangerG()
        {
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            if (uiControlPanel == null)
            {
                this.inspect = CreateInspectPage();
                this.model = CreateModelPage();
                this.teach = CreateTeachPage();
                this.report = CreateReportPage();
                this.setting = CreateSettingPage();
                this.log = CreateLogPage();

                uiControlPanel = new MainTabPanel((Control)inspect, (Control)model, (Control)teach, (Control)report, (Control)setting, (Control)log);
            }

            return uiControlPanel;
        }

        public override Control CreateDefectInfoPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.DefectPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanG.Gravure.UI.Setting.SettingPage();
        }

        public override List<Control> GetTeachButtons(IVncContainer teachPage)
        {
            List<Control> controlList = new List<Control>();
            
            controlList.Add(new LightSettingButton());

            if(MonitorSystemSettings.Instance().UseLaserBurner)
                controlList.Add(new MachineSettingButton());

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                VncCamButton vncCamButton = new VncCamButton(ExchangeCommand.V_TEACH, inspector, teachPage.ProcessStarted, teachPage.ProcessExited);
                controlList.Add(vncCamButton);
            }

            return controlList;
        }

        public override IReportPanel CreateReportPanel()
        {
            return new UniScanG.Gravure.UI.Report.ReportPanel();
        }

        public override Control CreateTeachSettingPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel.AutoSize = true;
            //panel.Dock = DockStyle.Fill;
            panel.ColumnCount = 1;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            panel.RowCount = 2;
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0);
            for (int i = 0; i < inspectorObjList.Count; i++)
            {
                SettingPanel settingPanel = new SettingPanel(i);
                panel.Controls.Add(settingPanel, 0, i);
            }
            return panel;
        }

        public override List<IStatusStripPanel> GetStatusStrip()
        {
            List<IStatusStripPanel> statusStripList = base.GetStatusStrip();
            if (MonitorSystemSettings.Instance().UseLaserBurner)
            {
                HanbitLaser hanbitLaser = ((Device.Gravure.DeviceController)SystemManager.Instance().DeviceController).HanbitLaser;
                statusStripList.Add(new LaserStatusStripPanel(hanbitLaser) { Dock = DockStyle.Right });
            }

            return statusStripList;
        }
    }
}

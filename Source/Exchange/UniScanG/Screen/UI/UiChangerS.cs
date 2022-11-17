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
using UniScanG.Screen.UI.Teach.Monitor;
using UniScanG.Screen.UI.Inspect;
using UniScanG.Screen.UI.Teach.Inspector;
using UniScan.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.Screen.UI.Teach;
using UniScan.Common.Util;

namespace UniScanG.Screen.UI
{
    public class MonitorUiChangerS : MonitorUiChanger
    {
        public override IUiControlPanel CreateUiControlPanel()
        {
            if (uiControlPanel == null)
            {
                this.inspect = CreateInspectPage();
                this.model = CreateModelPage();
                this.teach = CreateTeachPage();
                this.setting = CreateSettingPage();
                this.report = CreateReportPage();

                uiControlPanel = new MainTabPanel((Control)inspect, (Control)model, (Control)teach, (Control)setting, (Control)report);
            }

            return uiControlPanel;
        }

        public override Control CreateDefectInfoPanel()
        {
            return new InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new DefectPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new Screen.Settings.Monitor.UI.SettingPage();
        }
        
        public override List<Control> GetTeachButtons(IVncContainer teachPage)
        {
            List<Control> controlList = new List<Control>();

            controlList.Add(new LightSettingButton());

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
            return new Screen.UI.Report.ReportPanel();
        }

        public override Control CreateTeachSettingPanel()
        {
            return new Teach.Monitor.SettingPanel();
        }
    }

    public class InspectorUiChangerS : InspectorUiChanger
    {
        public override Control CreateDefectInfoPanel()
        {
            return new InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new DefectPanel();
        }
        
        public override IModellerControl CreateImageController()
        {
            IDefectTypeFilter defectTypeFilter = new DefectTypeFilterPanel();
            IDefectLegend defectLegend = new DefectLegentPanel();
            return new ImageController(defectTypeFilter, defectLegend);
        }

        public override UniEye.Base.UI.ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtenderS();
        }

        public override IModellerControl CreateParamController()
        {
            return new ParamController();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new Screen.Settings.Monitor.UI.SettingPage();
        }
        public override IMainTabPage CreateTeachPage()
        {
            return new UniScanG.UI.Teach.Inspector.TeachPage();
        }

        public override IModellerControl CreateTeachToolBar()
        {
            return new TeachToolBar();
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            throw new NotImplementedException();
        }
    }
}

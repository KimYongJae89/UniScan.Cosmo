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

namespace UniScan.Inspector.UI
{   public class InspectorUiChangerG : InspectorUiChanger
    {
        public override Control CreateDefectInfoPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.InfoPanel();
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.DefectPanel();
        }
        
        public override IModellerControl CreateImageController()
        {
            IDefectTypeFilter defectTypeFilter = new DefectTypeFilterPanel();
            IDefectLegend defectLegend = new DefectLegendPanel();
            return new ImageController(defectTypeFilter, defectLegend);
        }

        public override UniEye.Base.UI.ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtenderG();
        }

        public override IModellerControl CreateParamController()
        {
            return new UniScanG.Gravure.UI.Teach.Inspector.ParamController();
        }

        public override ISettingPage CreateSettingPage()
        {
            this.setting= new UniScanG.Gravure.UI.Setting.SettingPage();
            return (ISettingPage)this.setting;
        }

        public override IMainTabPage CreateTeachPage()
        {
            this.teach = new UniScanG.UI.Teach.Inspector.TeachPage();
            return this.teach;
        }

        public override IModellerControl CreateTeachToolBar()
        {
            return new TeachToolBarG();
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            throw new NotImplementedException();
        }
    }
}

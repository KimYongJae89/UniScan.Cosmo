using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Device.Serial;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Exchange;
using UniScan.Common.Settings;
using UniScan.Common.Settings.UI;
using UniScan.Common.UI;
using UniScan.Monitor.Exchange;
using UniScan.Monitor.Settings.Monitor.UI;
using UniScan.Monitor.UI;
using UniScanG.Data;
using UniScanG.Gravure.MachineIF;

namespace UniScan.Monitor
{
    public class MonitorConfigHelper : Common.ConfigHelper
    {
        static MonitorConfigHelper _instance;
        public static MonitorConfigHelper Instance()
        {
            if (_instance == null)
                _instance = new MonitorConfigHelper();

            return _instance;
        }
        
        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new MonitorSystemSettingPanel());
        }

        Form mainForm = null;
        public Form MainForm { get => mainForm; }

        public override Form GetMainForm()
        {
            IUiControlPanel uiControlPanel = SystemManager.Instance().UiChanger.CreateUiControlPanel();
            SystemManager.Instance().UiController.SetUiControlPanel(uiControlPanel);
            switch (SystemTypeSettings.Instance().SystemType)
            {
                case SystemType.Screen:
                    this.mainForm = new UniScanG.UI.MonitorMainform(uiControlPanel, "MLCC Screen Print Inspector");
                    break;
                case SystemType.Gravure:
                    this.mainForm = new UniScanG.UI.MonitorMainform(uiControlPanel, "MLCC Gravure Print Inspector");
                    break;
                case SystemType.Film: 
                    break;
            }

            return this.mainForm;
        }

        public override void BuildSystemManager()
        {
            SystemManager systemManager = null;
            
            switch (SystemTypeSettings.Instance().SystemType)
            {
                case SystemType.Screen:
                    systemManager = CreateScreenSystemManager();
                    break;
                case SystemType.Gravure:
                    systemManager = CreateGravureSystemManager();
                    break;
                case SystemType.Film:
                    break;
            }

            SystemManager.SetInstance(systemManager);
            //SystemManager.Instance().ProductionManager.Load(PathSettings.Instance().Result);
            systemManager.ExchangeOperator = new MonitorOperator();
            systemManager.UiController = new MonitorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);

        }

        private SystemManager CreateScreenSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerS());
            SystemManager systemManager = new UniScanG.Screen.MonitorSystemManagerS();

            systemManager.Init(new UniScanG.Data.Model.ModelManager(), new UniScanG.Screen.UI.MonitorUiChangerS(), new UniScanG.Screen.AlgorithmArchiver(),
                new UniScanG.Screen.Device.DeviceBox(new UniScanG.Screen.Device.PortMap()), new UniEye.Base.Device.DeviceController(), new UniScanG.Screen.Data.ProductionManagerS(PathSettings.Instance().Result));

            SheetCombiner.SetCollector(new UniScanG.Screen.Data.ResultCollector());
            return systemManager;
        }

        private SystemManager CreateGravureSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerG());
            SystemManager systemManager = new UniScanG.Gravure.MonitorSystemManagerG();
            MachineIfProtocolList machineIfProtocolList = new UniScanGMachineIfProtocolList(new Type[] { typeof(UniScanGMachineIfCommon) });

            systemManager.Init(new UniScanG.Data.Model.ModelManager(), new MonitorUiChangerG(), new UniScanG.Gravure.AlgorithmArchiver(),
                new UniScanG.Gravure.Device.DeviceBox(new UniScan.Monitor.Device.Gravure.PortMap()), new UniScan.Monitor.Device.Gravure.DeviceController(), new UniScanG.Gravure.Data.ProductionManagerG(PathSettings.Instance().Result), machineIfProtocolList);
            
            SheetCombiner.SetCollector(new UniScanG.Gravure.Data.ResultCollector());
            return systemManager;
        }
    }
}

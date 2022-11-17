using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
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
using UniEye.Base.Settings.UI;
using UniScan.Common;
using UniScan.Common.Exchange;
using UniScan.Common.Settings;
using UniScan.Common.Settings.UI;
using UniScan.Inspector.Settings.Inspector;
using UniScan.Inspector.Settings.Inspector.UI;
using UniScan.Inspector.UI;
using UniScanG.Data;

namespace UniScan.Inspector
{
    public class InspectorConfigHelper : Common.ConfigHelper
    {
        static InspectorConfigHelper _instance;
        public static InspectorConfigHelper Instance()
        {
            if (_instance == null)
                _instance = new InspectorConfigHelper();

            return _instance;
        }

        public override UniEye.Base.Settings.UI.ICustomConfigPage GetCustomConfigPage()
        {
            return new SystemTypeSettingPanel(new InspectorSystemSettingPanel());
        }

        public override Form GetMainForm()
        {
            Form form = null;
            switch (SystemTypeSettings.Instance().SystemType)
            {
                case SystemType.Screen:
                case SystemType.Gravure:
                    form = new UniScanG.UI.InspectorMainForm();
                    break;
                case SystemType.Film:
                    break;
            }

            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.C_CONNECTED, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
            return form;
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
                    throw new NotImplementedException();
            }
            SystemManager.SetInstance(systemManager);

            //SystemManager.Instance().ProductionManager.Load(PathSettings.Instance().Result);

            systemManager.ExchangeOperator = new InspectorOperator();
            systemManager.UiController = new InspectorUiController();
        }

        public override void InitializeSystemManager()
        {
            SystemManager systemManager = SystemManager.Instance();
            systemManager.ModelManager.Init(PathSettings.Instance().Model);

            systemManager.InitalizeModellerPageExtender();
        }

        private SystemManager CreateScreenSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerS());
            SystemManager systemManager = new UniScanG.Screen.InspectorSystemManagerS();

            systemManager.Init(new UniScanG.Data.Model.ModelManager(), new UniScanG.Screen.UI.InspectorUiChangerS(), new UniScanG.Screen.AlgorithmArchiver(),
                new UniScan.Common.Device.DeviceBox(new UniEye.Base.Device.PortMap()), new UniEye.Base.Device.DeviceController(), new UniScanG.Screen.Data.ProductionManagerS(PathSettings.Instance().Result));

            return systemManager;
        }

        private SystemManager CreateGravureSystemManager()
        {
            AddressManager.SetInstance(new AddressManagerG());
            SystemManager systemManager = new UniScanG.Gravure.InspectorSystemManagerG();

            systemManager.Init(new UniScanG.Data.Model.ModelManager(), new InspectorUiChangerG(), new UniScanG.Gravure.AlgorithmArchiver(),
                new UniScanG.Gravure.Device.DeviceBox(new UniScan.Inspector.Device.Gravure.PortMap()), new UniScan.Inspector.Device.Gravure.DeviceController(), new UniScanG.Gravure.Data.ProductionManagerG(PathSettings.Instance().Result));

            return systemManager;
        }
    }
}

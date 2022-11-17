// custom
using System.Collections.Generic;
using System.IO;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScan.Common;
using UniScan.Common.Settings;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.UI;
using UniScanG.UI.Teach;

namespace UniScanG
{
    public abstract class SystemManager : UniScan.Common.SystemManager
    {
        public new UnitBaseInspectRunner InspectRunner { get => (UnitBaseInspectRunner)inspectRunner; }

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new UiChanger UiChanger
        {
            get { return (UiChanger)uiChanger; }
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
            set { currentModel = value; }
        }

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public override UniScan.Common.Data.ModelManager CreateModelManager()
        {
            return new ModelManager();
        }
    }
}

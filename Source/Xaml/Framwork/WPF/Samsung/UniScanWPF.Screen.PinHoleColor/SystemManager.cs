using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Color.Settings;
using UniScanWPF.Screen.PinHoleColor.Color.UI;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Settings;
using UniScanWPF.UI;

namespace UniScanWPF.Screen.PinHoleColor
{
    public delegate void InspectedDelegate(InspectResult inspectResult);

    interface IInspectedListner
    {
        void Inspected(InspectResult inspectResult);
    }

    public class SystemManager : UniScanWPF.SystemManager
    {
        private List<IInspectedListner> inspectedListnerList = new List<IInspectedListner>();
        
        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
            set { currentModel = value; }
        }

        public new MultipleProductionManager ProductionManager
        {
            get { return (MultipleProductionManager)productionManager; }
            set { productionManager = value; }
        }

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new Inspect.InspectRunner();
        }

        public override void InitializeDataExporter()
        {

        }

        public override void LoadAdditialSettings()
        {
            PinHoleSettings.Instance().Load();
            ColorSettings.Instance().Load();
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }

        public string[] GetDetectorParamType()
        {
            return new string[] { typeof(UniScanWPF.Screen.PinHoleColor.PinHole.Inspect.PinHoleDetectorParam).ToString(), typeof(UniScanWPF.Screen.PinHoleColor.Color.Inspect.ColorDetector).ToString() };
        }

        internal void AddInspectedIListner(IInspectedListner listner)
        {
            lock (inspectedListnerList)
                inspectedListnerList.Add(listner);
        }

        internal void Inspected(InspectResult inspectResult)
        {
            if (inspectResult.DetectorResult is Color.Inspect.ColorDetectorResult)
            {
                //if (inspectResult.Judgment == DynMvp.InspData.Judgment.Reject)
                //{
                //    deviceBox.DigitalIoHandler.WriteOutput(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutColor), true);
                //    Thread.Sleep(100);
                //    deviceBox.DigitalIoHandler.WriteOutput(deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutColor), false);
                //}

                ResultExportManager.Instance().ExportResult(inspectResult);
            }

            inspectedListnerList.ForEach(listner => listner.Inspected(inspectResult));
        }
    }
}
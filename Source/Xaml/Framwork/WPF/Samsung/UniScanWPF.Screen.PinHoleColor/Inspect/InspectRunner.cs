using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    internal class InspectRunner : UniEye.Base.Inspect.DirectTriggerInspectRunner
    {
        List<InspectSet> inspectSetList = new List<InspectSet>();
        Thread syncThread;
        CancellationTokenSource source;

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().GetOpState() != OpState.Idle)
                return false;

            string modelName = "";

            foreach (string str in Enum.GetNames(typeof(PortMap.ModelPortName)))
            {
                PortMap.ModelPortName modelPortName = (PortMap.ModelPortName)Enum.Parse(typeof(PortMap.ModelPortName), str);
                IoPort port = SystemManager.Instance().DeviceBox.PortMap.GetInPort(modelPortName);
                modelName += SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(port) == true ? "1" : "0";
            }

            SystemManager.Instance().CurrentModel = SystemManager.Instance().ModelManager.PreSetList.Find(model => model.Name == modelName);
            if (SystemManager.Instance().CurrentModel == null)
                return false;

            string productionName = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, productionName);
            SystemManager.Instance().ProductionManager.Save();

            inspectSetList.Clear();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                DetectorParam param = SystemManager.Instance().CurrentModel.DeviceDictionary[imageDevice];

                InspectSet inspectSet = new InspectSet(imageDevice, Detector.Create(param), param, Inspected);
                inspectSetList.Add(inspectSet);
                BufferManager.Instance().AddInspectSet(inspectSet);

                imageDevice.ImageGrabbed = ImageGrabbed;
            }

            BufferManager.Instance().Connect();

            source = new CancellationTokenSource();
            syncThread = new Thread(ThreadProc);
            syncThread.IsBackground = true;
            syncThread.Priority = ThreadPriority.Highest;
            syncThread.Start();

            inspectSetList.ForEach(set => set.Start());
            ResultExportManager.Instance().Start();

            InspectResult.Reset();
            ResultCombiner.Instance().Reset();
            
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRun), true);

            imageDeviceHandler.GrabMulti();

            SystemState.Instance().SetWait();

            return true;
        }

        public void ThreadProc()
        {
            bool run = false;

            while (source.IsCancellationRequested == false)
            {
                Thread.Sleep(1000);

                if (run == true)
                {
                    //if (inspectSetList.All(set => set.ThreadRun == false))
                    //{
                    //    SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutSync), true);
                    //    Thread.Sleep(100);
                    //    SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutSync), false);
                    //    SystemManager.Instance().ProductionManager.Save();

                    //    run = false;
                    //}
                }
                else
                {
                    run = inspectSetList.All(set => set.ThreadRun == true);
                }
            }
        }

        public override void ExitWaitInspection()
        {
            source.Cancel();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.Stop();

            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed = null;

            inspectSetList.ForEach(set => set.Stop());
            inspectSetList.Clear();
            BufferManager.Instance().Clear();

            ResultCombiner.Instance().Reset();

            ResultExportManager.Instance().Stop();

            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutRun), false);

            SystemManager.Instance().ProductionManager.Save();

            SystemState.Instance().SetIdle();
        }

        private void Inspected(InspectResult inspectResult)
        {
            inspectResult.EndTime = DateTime.Now;
            
            SystemManager.Instance().Inspected(inspectResult);
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            InspectSet inspectSet = inspectSetList.Find(set => set.TargetDevice == imageDevice);

            if (inspectSet == null)
            {
                LogHelper.Debug(LoggerType.Debug, "Not found inspect set.");
                return;
            }

            InspectResult inspectResult = new InspectResult(imageDevice);
            inspectSet.Enqueue(new Tuple<InspectResult, IntPtr>(inspectResult, ptr));
        }
    }
}
using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI; 
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanG.Inspect;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision;
using System.Collections.Generic;
using UniScanG.Vision;
using UniScan.Common.Exchange;
using UniScanG.Screen.Data;
using DynMvp.Authentication;
using UniScanG.Data;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using System.Threading.Tasks;
using DynMvp.Device.Serial;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;

namespace UniScanG.Screen.Inspect
{
    internal class InspectRunnerMonitorS : UniScanG.Inspect.InspectRunner
    {
        bool stopThread = true;
        Thread thread = null;
        Dictionary<int, List<Tuple<string, string>>> resultDic = new Dictionary<int, List<Tuple<string, string>>>();
        List<InspectorObj> InspectorList = new List<InspectorObj>();

        bool onInspect = false;

        List<InspectionResult> InspectionResultList = new List<InspectionResult>();

        public object SystemTypeManager { get; private set; }

        public InspectRunnerMonitorS() : base()
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            InspectorList = server.GetInspectorList();
            
            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);
        }

        protected override void SetupUnitManager() { }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().OnInspectOrWait == true)
                return false;
            
            if (SystemManager.Instance().CurrentModel == null || SystemManager.Instance().ProductionManager.CurProduction == null)
                return false;
            
            if (false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            //resultDic.Clear();

            foreach (InspectorObj inspector in InspectorList)
                resultDic.Add(inspector.Info.CamIndex, new List<Tuple<string, string>>());

            stopThread = false;
            thread = new Thread(InspectionResultTask);
            thread.Start();
            
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Start"));
            simpleProgressForm.Show(() =>
            {
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_START, SystemManager.Instance().ProductionManager.CurProduction.LotNo);

                bool startDone = false;
                while (startDone == false)
                {
                    startDone = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.InspectState == InspectState.Done)
                        {
                            startDone = false;
                            break;
                        }
                    }
                }
            });

            SystemState.Instance().SetWait();

            // DIO #0에서 Falling Edge 검출
            if (MessageForm.Show(null, "Is Virtual Run?", MessageFormType.YesNo) == DialogResult.No)
            {
                IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(UniScanG.Screen.Device.PortMap.IoPortName.InMachineRun);
                if (ioPort != null)
                    WaitFall(ioPort);
            }

            SystemState.Instance().SetInspect();

            LogHelper.Info(LoggerType.Inspection, string.Format("Start - Model : {0}, Lot : {1}", SystemManager.Instance().CurrentModel.Name, SystemManager.Instance().ProductionManager.CurProduction.LotNo));

            return PostEnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            UpdateSerialEncoder(true);
            UpdateLightCtrl(true);

            return true;
        }

        private void WaitFall(IoPort ioPort)
        {
            bool val1 = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioPort);
            bool val2 = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioPort);
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                while ((val1 == true && val2 == false) == false)   // High -> Low
                {
                    Thread.Sleep(10);
                    val1 = val2;
                    val2 = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioPort);
                }
            });
        }

        private void UpdateSerialEncoder(bool enable)
        {
            SerialDeviceHandler sdh = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialDevice sd = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder);
            if (sd != null)
            {
                SerialEncoder se = (SerialEncoder)sd;
                string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.EN, enable ? "1" : "0");
            }
        }

        private void UpdateLightCtrl(bool enable)
        {
            LightCtrlHandler lch = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            if (lch.Count > 0)
            {
                SerialLightCtrl serialLightCtrl = lch.GetLightCtrl(0) as SerialLightCtrl;
                if (serialLightCtrl != null)
                {
                    serialLightCtrl.LightSerialPort.WritePacket(enable ? "UDIO1\r\n" : "UDIO0\r\n");
                    if (enable)
                        serialLightCtrl.TurnOn();
                    else
                        serialLightCtrl.TurnOff();
                }
            }
        }

        public override void PreExitWaitInspection()
        {
            // Encoder/Ligth Disable
            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);
        }

        public override void ExitWaitInspection()
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Stop"));
            simpleProgressForm.Show(() =>
            {
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_STOP);

                bool stopDone = false;
                while (stopDone == false)
                {
                    stopDone = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.OpState != OpState.Idle)
                        {
                            stopDone = false;
                            break;
                        }
                    }

                    if (onInspect == true)
                        stopDone = false;
                }

                PreExitWaitInspection();

                stopThread = true;
                resultDic.Clear();

                SystemState.Instance().SetIdle();

                UniScanG.Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as UniScanG.Data.Production;
                LogHelper.Info(LoggerType.Inspection, string.Format("Stop - Total {0}", production == null ? -1 : production.Total));
            });
        }

        public override void EnterPauseInspection()
        {
            SystemState.Instance().Pause = true;
        }

        public override void InspectDone(UnitInspectItem unitInspectItem)
        {
            SystemState.Instance().SetInspectState(UniEye.Base.Data.InspectState.Done);
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            if (SystemState.Instance().GetOpState() == OpState.Idle)
                return;

            lock (InspectionResultList)
                InspectionResultList.Add(inspectionResult);
        }

        public void InspectionResultTask()
        {
            while (stopThread == false)
            {
                if (InspectionResultList.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                lock (InspectionResultList)
                {
                    foreach (InspectionResult result in InspectionResultList)
                    {
                        int camIndex = Convert.ToInt32(result.GetExtraResult("Cam"));
                        string inspectionNo = (string)result.GetExtraResult("No");
                        string inspectionTime = (string)result.GetExtraResult("Time");

                        if (resultDic.Count != 0)
                            resultDic[camIndex].Add(new Tuple<string, string>(inspectionNo, inspectionTime));
                    }

                    InspectionResultList.Clear();
                }

                if (resultDic.Count == 0)
                    continue;
                
                Tuple<string, string> foundedT = null;
                foreach (Tuple<string, string> tuple in resultDic[0])
                {
                    bool existResult = true;
                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.Info.CamIndex != 0)
                        {
                            if (resultDic[inspector.Info.CamIndex].Find(t => t.Item1 == tuple.Item1) == null)
                            {
                                existResult = false;
                                break;
                            }
                        }
                    }

                    if (existResult == false)
                        continue;

                    foundedT = tuple;
                }

                if (foundedT == null)
                    continue;

                onInspect = true;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                MergeSheetResult mergeSheetResult = new MergeSheetResult(int.Parse(foundedT.Item1),SheetCombiner.CombineResult(foundedT));

                InspectionResult inspectionResult = BuildInspectionResult();
                inspectionResult.AlgorithmResultLDic.Add(SheetInspector.TypeName, mergeSheetResult);
                inspectionResult.InspectionNo = foundedT.Item1;
                inspectionResult.InspectionTime = mergeSheetResult.SpandTime;

                UniScanG.Data.Production production = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
                production.Update(mergeSheetResult);
                SystemManager.Instance().ProductionManager.Save(PathSettings.Instance().Result);

                SystemManager.Instance().ExportData(inspectionResult);

                stopwatch.Stop();
                inspectionResult.ExportTime = stopwatch.Elapsed;

                LogHelper.Info(LoggerType.Inspection, string.Format("Sheet No : {0}, Spend time : {1} ms", inspectionResult.InspectionNo, inspectionResult.InspectionTime.ToString("ss\\.fff")));
                InspectDone(inspectionResult);

                //SystemManager.Instance().ExportData(inspectionResult);

                if (resultDic.Count == 0)
                    return;

                foreach (InspectorObj inspector in InspectorList)
                    resultDic[inspector.Info.CamIndex].RemoveAll(t => t.Item1 == foundedT.Item1);

                onInspect = false;
            }
        }
    }
}

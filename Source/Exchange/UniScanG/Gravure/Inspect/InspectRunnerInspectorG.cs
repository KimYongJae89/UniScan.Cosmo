using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
//using DynMvp.Data;
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
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Temp;
using UniScan.Common;
//using DynMvp.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using UniScanG.Gravure.UI.Inspect.Inspector;
using UniScanG.Gravure.Vision;
using System.Text;
using UniScanG.Gravure.Vision.Watcher;
using System.Xml;

namespace UniScanG.Gravure.Inspect
{
    internal class InspectRunnerInspectorG : UniScanG.Inspect.InspectRunner
    {
        //Gravure.Data.ResultCollector collector;

        public InspectRunnerInspectorG() : base()
        {
            this.grabProcesser = new SheetGrabProcesserG();
            this.grabProcesser.StartInspectionDelegate += grabProcesser_StartInspection;

            this.inspectObserver = new InspectObserver();
            //this.collector = new Data.ResultCollector();
        }

        ~InspectRunnerInspectorG()
        {
        }

        protected ImageDevice GetImageDevice(int deviceIndex)
        {
            return SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::EnterWaitInspection");
            if (SystemManager.Instance().CurrentModel == null)
                return false;

            Model model = SystemManager.Instance().CurrentModel;
            if (SystemManager.Instance().CurrentModel.IsTaught() == false)
            {
                SystemState.Instance().SetAlarm();
                MessageForm.Show(null, StringManager.GetString("There is no data or teach state is invalid."));
                SystemState.Instance().SetIdle();
                return false;
            }

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;

            // 현재 티칭 값 저장
            if (SaveTeachParam() == false)
                return false;

            // Create Buffer
            SimpleProgressForm form = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready"));
            bool isMultiLayerBuffer = Settings.AdditionalSettings.Instance().IsMultiLayerBuffer;
            float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;
            bool readyOk = false;
            form.Show(() =>
            {
                CalculatorBase calculator = (CalculatorBase)AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
                Detector detector = (Detector)AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);

                CalculatorParam calculatorParam = calculator.Param as CalculatorParam;
                DetectorParam detectorParam = detector.Param as DetectorParam;

                int bufferSize = AlgorithmSetting.Instance().InspBufferCount;
                for (int i = 0; i < bufferSize; i++)
                {
                    ProcessBufferSetG processBufferSetG = calculator.CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, calculatorParam.SheetSize.Width, (int)(calculatorParam.SheetSize.Height * 1.05));
                    processBufferSetG.BuildBuffers();
                    this.processBufferManager.AddProcessBufferSet(processBufferSetG, 1);
                }

                // Prepare Algorithm
                calculator.PrepareInspection();
                detector.PrepareInspection();

                // Init unit
                SetupUnitManager();
                readyOk = true;
            });

            if (readyOk == false)
            {
                SystemState.Instance().SetAlarm();
                MessageForm.Show(null, StringManager.GetString("Buffer Initialize Fail."));
                SystemState.Instance().SetIdle();
                return false;
            }

            this.inspectObserver.Clear();

            SystemState.Instance().SetWait();
            return true;
        }

        public override bool PostEnterWaitInspection()
        {
            Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as Settings.AdditionalSettings;

            if (this.inspectUnitManager.IsRunning() == false)
                return false;

            // 카메라 및 GrabProcesser 초기화
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed += this.grabProcesser.ImageGrabbed;
                imageDevice.SetTriggerMode(TriggerMode.Hardware);
                if (additionalSettings.UseAsyncMode)
                {
                    imageDevice.SetTriggerMode(TriggerMode.Software);
                    imageDevice.SetAcquisitionLineRate(additionalSettings.AsyncGrabHz);
                }
            }

            ((SheetGrabProcesserG)this.grabProcesser).Algorithm = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            ((SheetGrabProcesserG)this.grabProcesser).SetFoundSheetCount();
            ((SheetGrabProcesserG)this.grabProcesser).Start();

            // 0번 카메라는 2초 후에 시작
            int sleepTimeMs = SystemManager.Instance().ExchangeOperator.GetClientIndex() == 0 ? 2000 : 0;
            Thread.Sleep(sleepTimeMs);

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            SystemState.Instance().SetInspect();
            return true;
        }

        private bool SaveTeachParam()
        {
            string curResultPath = SystemManager.Instance().ProductionManager.CurProduction?.GetResultPath();
            if (string.IsNullOrEmpty(curResultPath))
                return false;

            Directory.CreateDirectory(curResultPath);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement element = xmlDoc.CreateElement("Element");
            xmlDoc.AppendChild(element);

            XmlElement sheetFinderElement = xmlDoc.CreateElement(SheetFinderBase.TypeName);
            element.AppendChild(sheetFinderElement);
            AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName).Param.SaveParam(sheetFinderElement);

            XmlElement trainerElement = xmlDoc.CreateElement(Trainer.TypeName);
            element.AppendChild(trainerElement);
            AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName).Param.SaveParam(trainerElement);

            XmlElement calcElement = xmlDoc.CreateElement(CalculatorBase.TypeName);
            element.AppendChild(calcElement);
            AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName).Param.SaveParam(calcElement);

            XmlElement detectElement = xmlDoc.CreateElement(Detector.TypeName);
            element.AppendChild(detectElement);
            AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName).Param.SaveParam(detectElement);

            xmlDoc.Save(Path.Combine(curResultPath, "TeachData.xml"));

            return true;
        }

        private void grabProcesser_StartInspection(ImageDevice imageDevice, IntPtr ptr)
        {
            // GrabProcesser에서 시트 1장을 그랩함.
            SheetImageSet sheetImageSet = ((SheetGrabProcesserG)this.grabProcesser).GetLastSheetImageSet();

            InspectionResult inspectionResult = BuildInspectionResult();
            //InspectionResult inspectionResult = BuildInspectionResult(sheetImageSet.SheetNo.ToString());

            // Client Index 확인
            int camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            int clientIndex = SystemManager.Instance().ExchangeOperator.GetClientIndex();
            int inspectionNo = int.Parse(inspectionResult.InspectionNo);
            this.inspectObserver.AddData(0, 0, inspectionNo);

            bool skipRun = (clientIndex >= 0 && inspectionNo % 2 != clientIndex);

            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp);

            
            ProcessBufferSetG bufferSet = this.processBufferManager.Request(imageDevice) as ProcessBufferSetG;
            SheetInspectParam inspectParam = new SheetInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext);
            inspectParam.AlgoImage = sheetImageSet;
            inspectParam.ProcessBufferSet = bufferSet;

            InspectionOption inspectionOption = new InspectionOption(imageDevice);
            UnitInspectItem unitInspectItem = new UnitInspectItem(inspectParam, inspectionResult, inspectionOption);

            if (skipRun || bufferSet == null)
            {
                inspectionResult.Judgment = Judgment.Skip;
            }
            else
            {
                bufferSet.PreCalculate(sheetImageSet);
            }

            bool runOk = this.inspectUnitManager.StartInspect(unitInspectItem);
            if (runOk)
            {
                SystemState.Instance().SetInspectState(InspectState.Run);
            }
            else
            {
                LogHelper.Debug(LoggerType.Debug, "InspectRunner::grabProcesser_StartInspection - Fail");
                unitInspectItem.Dispose();
                if (bufferSet != null)
                    this.processBufferManager.Return(bufferSet);
            }
        }

        protected override void SetupUnitManager()
        {
            this.inspectUnitManager.Dispose();

            // calculator
            Algorithm calculateAlgorithm = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
            if (calculateAlgorithm != null)
            {
                InspectUnit calculateInspectUnit = new InspectUnit(CalculatorBase.TypeName, calculateAlgorithm);
                calculateInspectUnit.UnitInspected = CalculateInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(calculateInspectUnit);
            }

            // detector
            Algorithm detectAlgorithm = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);
            if (detectAlgorithm != null)
            {
                InspectUnit detectInspectUnit = new InspectUnit(Detector.TypeName, detectAlgorithm);
                detectInspectUnit.UnitInspected = DetectInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(detectInspectUnit);
            }

            // Watcher
            Algorithm watchAlgorithm = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName);
            if (watchAlgorithm != null)
            {
                InspectUnit watchInspectUnit = new InspectUnit(Watcher.TypeName, watchAlgorithm);
                watchInspectUnit.UnitInspected = WatchInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(watchInspectUnit);
            }

            inspectUnitManager.AllUnitInspected = inspectUnitManager_AllUnitInspected;
            this.inspectUnitManager.Run();
        }

        private void WatchInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
        }

        private void DetectInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
        }

        private void CalculateInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
        }

        CancellationTokenSource exportCancellationTokenSource;
        private void inspectUnitManager_AllUnitInspected(UnitInspectItem unitInspectItem)
        {
            LogHelper.Debug(LoggerType.Debug, "InspectRunnerInspectorG::inspectUnitManager_AllUnitInspected");
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectItem.AlgorithmInspectParam;
            sheetAlgorithmInspectParam.Dispose();

            // 버퍼 반환
            ProcessBufferSet processBufferSet = sheetAlgorithmInspectParam.ProcessBufferSet;
            processBufferSet?.WaitDone();
            processBufferManager.Return(processBufferSet);

            UniScanG.Data.Production productionG = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;

            // 카운트 업데이트
            if (unitInspectItem.InspectionResult.Judgment == Judgment.Skip)
            {
                productionG.Update(null);
            }
            else
            {
                SheetResult seetResult = (SheetResult)unitInspectItem.InspectionResult.AlgorithmResultLDic[Detector.TypeName];
                productionG.Update(seetResult);

                SystemManager.Instance().DeviceController.OnProductInspected(unitInspectItem.InspectionResult);

                //exportCancellationTokenSource?.Cancel();
                exportCancellationTokenSource = new CancellationTokenSource();
                Task.Run(() =>
                {   
                    // 저장
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    SystemManager.Instance().ExportData(unitInspectItem.InspectionResult, exportCancellationTokenSource);
                    sw.Stop();
                    unitInspectItem.InspectionResult.ExportTime = sw.Elapsed;

                    // 서버에 통보
                    IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
                    clientExchangeOperator.SendInspectDone(unitInspectItem.InspectionResult.InspectionNo, productionG.StartTime.ToString("yy-MM-dd"));
                });

                this.inspectObserver.AddData(1, 0, int.Parse(unitInspectItem.InspectionResult.InspectionNo));
            }

            //StringBuilder infoStringBuilder = new StringBuilder();
            //infoStringBuilder.Append(string.Format("Sheet,{0}", unitInspectItem.InspectionResult.InspectionNo));
            //infoStringBuilder.Append(string.Format(",Result,{0}", unitInspectItem.InspectionResult.Judgment.ToString()));
            //if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(CalculatorBase.TypeName))
            //    infoStringBuilder.Append(string.Format(",Calculator,{0}", unitInspectItem.InspectionResult.AlgorithmResultLDic[CalculatorBase.TypeName].SpandTime.ToString("ss\\.fff")));
            //if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
            //    infoStringBuilder.Append(string.Format(",Detector,{0}", unitInspectItem.InspectionResult.AlgorithmResultLDic[Detector.TypeName].SpandTime.ToString("ss\\.fff")));
            //LogHelper.Info(LoggerType.Inspection, infoStringBuilder.ToString());

            InspectDone(unitInspectItem.InspectionResult);
            if (this.inspectUnitManager.IsBusy() == false)
                SystemState.Instance().SetInspectState(InspectState.Wait);

        }

        public override void PreExitWaitInspection()
        {
            SystemManager.Instance().DeviceController.OnExitWaitInspection();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
        }

        public override void ExitWaitInspection()
        {
            PreExitWaitInspection();
            LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed -= grabProcesser.ImageGrabbed;

            LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - START EXIT");
            SimpleProgressForm form = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Wait"));
            form.Show(() =>
            {
                LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - grabProcesser.Stop");
                this.grabProcesser.Stop();

                LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - inspectUnitManager.Stop");
                this.inspectUnitManager.Stop();
                LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - inspectUnitManager.Dispose");
                this.inspectUnitManager.Dispose();

                LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - processBufferManager.Dispose");
                this.processBufferManager.Dispose();
                LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - grabProcesser.Dispose");
                this.grabProcesser.Dispose();
            });

            LogHelper.Debug(LoggerType.Debug, "InspectRunner::ExitWaitInspection - END EXIT");
            SystemManager.Instance().ProductionManager.Save();

            // Clear Algorithm
            AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName).ClearInspection();
            AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName).ClearInspection();

            SystemState.Instance().SetInspectState(InspectState.Wait);
            SystemState.Instance().SetIdle();
        }

        private void WaitInspectionDone()//not used
        {
            do
            {
                UpdateSystemState();
                //Thread.Sleep(10);
                Application.DoEvents();
                OpState opState = SystemState.Instance().GetOpState();
                if (opState == OpState.Wait || opState == OpState.Idle)
                    break;
            } while (true);
        }

        private void UpdateSystemState()
        {
            //if (this.grabProcesser != null)
            //    SystemManager.Instance().MainForm.UpdateControl("0", this.grabProcesser.GetBufferCount().ToString());

            //if (unitManager != null)
            //{
            //    for (int i = 0; i < this.unitManager.Count; i++)
            //        SystemManager.Instance().MainForm.UpdateControl((i + 1).ToString(), this.unitManager.GetBufferCount(i).ToString());
            //}

            //if (SystemState.Instance().GetOpState() == OpState.Idle)
            //    return;

            //if (unitManager != null && unitManager.IsBusy())
            //    SystemState.Instance().SetInspectState(InspectState.Run);
            //else
            //    SystemState.Instance().SetWait();

        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            base.Inspect(imageDevice, ptr, inspectionResult, inspectionOption);
        }
        
        public override void Dispose()
        {
            grabProcesser.Dispose();
        }

        public override void InspectDone(UnitInspectItem unitInspectItem)
        {
            base.InspectDone(unitInspectItem);

            //LogHelper.Debug(LoggerType.Inspection, "InspectionThread_InspectionFinished");

            //inspectionResult.InspectionEndTime = DateTime.Now;

            //if (targetGroup == null)
            //{
            //    int inspectionStep = UniScanGSettings.Instance().InspectorInfo.CamIndex;
            //    targetGroup = SystemManager.Instance().CurrentModel.GetInspectionStep(inspectionStep).GetTargetGroup(0);
            //}

            //if (inspectionResult == null)
            //    inspectionResult = this.inspectionResult;

            //base.InspectionThread_TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult);

            ////UpdateSystemState();
        }
    }
}

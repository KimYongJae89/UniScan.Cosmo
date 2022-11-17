using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
//using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using System.IO;
//using UniScanM.Data;
using System.Threading;
using UniEye.Base.Device;
using UniScanM.Algorithm;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI.Touch;
using UniEye.Base;
//using DynMvp.InspData;
using UniEye.Base.Inspect;
using UniScanM.StillImage.State;
using DynMvp.Data;
using UniScanM.MachineIF;
using UniEye.Base.MachineInterface;
using UniScanM.StillImage.Settings;
//using UniScanM.Settings;
using System.Windows.Forms;
using UniScanM.StillImage.Data;

namespace UniScanM.StillImage.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        private struct LastGrabbedIamgeDevice
        {
            public ImageDevice imageDevice;
            public IntPtr ptr;
        }

        LastGrabbedIamgeDevice lastGrabbedIamgeDevice;
        //EncoderInspectStarter inspectStarter = null; //시트 속도 감지후 시작 or 정지.
        ThreadHandler runningThreadHandler = null;  // 검사시 While 돌릴 쓰레드.
        
        public InspectRunner() : base()
        {
            lastGrabbedIamgeDevice = new LastGrabbedIamgeDevice();

            //inspectStarter = new EncoderInspectStarter();
            //inspectStarter.OnStartInspection = EnterWaitInspection;
            //inspectStarter.OnStopInspection = ExitWaitInspection;

            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting+= OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;
        }

        private void OnLotChanged()
        {
            //SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        private void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override void Dispose()
        {
            base.Dispose();

            //inspectStarter.Stop();
            //inspectStarter.Dispose();
            //ExitWaitInspection();
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");

            if (runningThreadHandler != null)
                return false;

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;


            StillImageSettings stillImageSettings = StillImageSettings.Instance();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed += ImageGrabbed;

            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);

            //if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
            //{
            //    // Get Model Name
            //    if(additionalSettings.ModelAutoChange)
            //        ChangeModel();
            //}

            //if (SystemManager.Instance().CurrentModel == null)
            //{
            //    ErrorSection errorSection = ErrorSection.Environment;
            //    ErrorSubSection errorSubSection = ErrorSubSection.CommonReason;
            //    ErrorManager.Instance().Report((int)errorSection, (int)errorSubSection, ErrorLevel.Error, errorSection.ToString(), "Model", "There is no selected Model");
            //    //MessageForm.Show(null, "There is no selected Model");

            //    return false;
            //}

            // Load Lot No
            if (SystemManager.Instance().InspectStarter is EncoderInspectStarter)
                ChangeLotNo();

            SystemState.Instance().SetWait();

            bool userNotify =  SystemManager.Instance().InspectStarter.StartMode != StartMode.Auto;
            CheckOrigin(userNotify);

            return PostEnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");

            StillImageSettings additionalSettings = StillImageSettings.Instance();
            InspectRunnerExtender inspectRunnerExtender = this.InspectRunnerExtender as InspectRunnerExtender;

            if (additionalSettings.AsyncMode)
            {
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Software);
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetAquationLineRate(additionalSettings.AsyncGrabHz);
            }

            inspectProcesser = new LightTuneState();

            runningThreadHandler = new ThreadHandler("InspectRunner", new Thread(RunningThreadProc), false);
            requestStop = false;
            runningThreadHandler.Start();

            SystemState.Instance().SetInspect();
            SystemState.Instance().SetInspectState(InspectState.Run);

            return true;
        }

        private void ChangeLotNo()
        {
            string model = SystemManager.Instance().CurrentModel.Name;
            string worker = SystemManager.Instance().InspectStarter.GetWorker();
            string lotNo = SystemManager.Instance().InspectStarter.GetLotNo();
            string paste = SystemManager.Instance().InspectStarter.GetPaste();
            string mode = SystemManager.Instance().InspectStarter.StartMode.ToString();
            int rewinderSite = SystemManager.Instance().InspectStarter.GetRewinderSite();
            int position = SystemManager.Instance().InspectStarter.GetPosition();

            if (string.IsNullOrEmpty(lotNo))
                lotNo = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);
            SystemManager.Instance().ProductionManager.LotChange(model, worker, lotNo, paste, mode, rewinderSite);
            SystemManager.Instance().ProductionManager.CurProduction.StartPosition = position;

            SystemManager.Instance().InspectStarter?.OnLotChanged();
        }

        private void CheckOrigin(bool userQuary)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return;

            if (axisHandler.IsHomeDone())
                return;

            bool needHome = true;
            if (userQuary)
                needHome = (MessageForm.Show(null, "Origin Move?", "UniScan", MessageFormType.YesNo, 5000, DialogResult.Yes) == DialogResult.Yes);

            if (needHome)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                SimpleProgressForm form = new SimpleProgressForm("Origin");
                form.Show(() =>
                {
                    Task homeTask = axisHandler.StartHomeMove();
                    homeTask.Wait(100);
                    axisHandler.WaitHomeDone(cancellationTokenSource);
                }, cancellationTokenSource);
            }
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();

            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                SystemState.Instance().SetInspectState(InspectState.Stop);
            else
                SystemState.Instance().SetInspectState(InspectState.Ready);
        }


        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            lastGrabbedIamgeDevice.imageDevice = imageDevice;
            lastGrabbedIamgeDevice.ptr = ptr;
        }

        private void RunningThreadProc()
        {
            SystemState.Instance().SetInspect();

            while (runningThreadHandler.RequestStop == false)
            {
                UniscanState curState = (UniscanState)inspectProcesser;

                // Grab
                LogHelper.Debug(LoggerType.Operation, "Grab Start");
                if (Grab() == false)
                {
                    if (requestStop == false)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)ErrorSubSection.CommonReason,
                       ErrorLevel.Fatal, "Grab Fail", "Grabber", "Grab Timeout", "Check the Grabber and Camera");
                    }
                    runningThreadHandler.RequestStop = true;
                    continue;
                }
                LogHelper.Debug(LoggerType.Operation, "Grab Done");

                DynMvp.InspData.InspectionResult inspectionResult = BuildInspectionResult();
                try
                {
                    if (inspectionResult == null)
                        continue;

                    ImageDevice imageDevice = this.lastGrabbedIamgeDevice.imageDevice;
                    if (imageDevice == null)
                        continue;

                    IntPtr ptr = this.lastGrabbedIamgeDevice.ptr;
                    InspectionOption inspectionOption = new InspectionOption(imageDevice);
              
                    this.Inspect(imageDevice, ptr, inspectionResult, inspectionOption);
                }
#if DEBUG == false
            catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                inspectionResult.SetDefect();
            }
#endif
                finally { }

                //GC.Collect();
                //GC.WaitForFullGCComplete();
            }

            SystemState.Instance().SetIdle();

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
            SystemManager.Instance().DeviceController.RobotStage.StopMove();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");

            AlgoImage algoImage = null;
            AlgoImage adjustImage = null;
            UniscanState curState = (UniscanState)this.inspectProcesser;
            ImageD imageD = null;
            try
            {
                imageD = imageDevice.GetGrabbedImage(ptr);
                Debug.Assert(imageD != null);

                //if (imageDevice.IsBinningVirtical())
                //    imageD = ((UniScanM.Operation.InspectRunnerExtender)inspectRunnerExtender).GetBinningImage(imageD);

                algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);

                Debug.Assert(algoImage != null);

                adjustImage = GetAdjustImage(algoImage);
                inspectRunnerExtender.OnPreInspection();

                SystemState.Instance().SetInspectState(curState.InspectState);
                this.inspectProcesser.Process(adjustImage, inspectionResult, inspectionOption);

                inspectRunnerExtender.OnPostInspection();

                if (curState.IsSyncState)
                    ProductInspected(inspectionResult);
            }
            finally
            {
                if (curState.IsSyncState)
                    adjustImage?.Dispose();
                algoImage?.Dispose();
                //imageD?.Dispose();
                //SystemState.Instance().SetInspectState(InspectState.Wait);
                //SystemState.Instance().SetWait();
            }
        }
        
        private bool Grab()
        {
            Debug.WriteLine("InspectRunner::Grab Start");
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            //if (imageDeviceHandler.IsGrabDone() == false)

            //    return false;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                CameraVirtual cameraVirtual = imageDevice as CameraVirtual;
                if (cameraVirtual != null)
                    cameraVirtual.SetStepLight(((UniscanState)this.inspectProcesser).ImageSequnece, 0);
                //cameraVirtual.SetStepLight(0, 0);
            }

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Clone();
            lightCtrlHandler.TurnOn(lightValue);

            Thread.Sleep(100);

            imageDeviceHandler.GrabOnce();
            bool ok = imageDeviceHandler.WaitGrabDone(-1);
            imageDeviceHandler.Stop();
            lightCtrlHandler.TurnOff();
            Debug.WriteLine("InspectRunner::Grab End");
            return ok;
        }

        private bool requestStop = false;
        public override void PreExitWaitInspection()
        {
            requestStop = true;
            base.PreExitWaitInspection();

            if (runningThreadHandler != null)
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() => runningThreadHandler.Stop());

                runningThreadHandler = null;
            }
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            // Update UI
            InspectionResult stillImageinspectionResult = (InspectionResult)inspectionResult;
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);

            if (stillImageinspectionResult.SheetRectInFrame.IsEmpty == false)
            {
                double sheetLength = stillImageinspectionResult.SheetRectInFrame.Height * SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height / 1000.0;
                SystemManager.Instance().MainForm.SettingPage.UpdateControl("SheetLength", sheetLength);
            }
            
            if (((UniscanState)inspectProcesser).IsTeachState == false)
            {
                //UniScanM.Data.Production p = SystemManager.Instance().ProductionManager.CurProduction;

                UniScanM.Data.Production production = this.UpdateProduction(stillImageinspectionResult);
                SystemManager.Instance().ExportData(inspectionResult);
            }

            this.inspectProcesser = ((UniscanState)inspectProcesser).GetNextState(inspectionResult);
            //if (((UniscanState)inspectProcesser).Initialized == false)
                //((UniscanState)inspectProcesser).Initialize();
        }
        
        private AlgoImage GetAdjustImage(AlgoImage grabbedImage)
        {
            AlgoImage adjustImage = null;
            // Histogram EQ
            if (false)
            {
                adjustImage = ImageBuilder.Build(grabbedImage);
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grabbedImage);
                imageProcessing.Mul(grabbedImage, adjustImage, 10);
                //imageProcessing.HistogramEqualization(clipImage);
                //clipImage.Save(Path.Combine(imageSavePath, "HistoEQ", iamgeSaveName));
            }
            else
            {
                adjustImage = grabbedImage.Clone();
            }
            return adjustImage;
        }
    }
}

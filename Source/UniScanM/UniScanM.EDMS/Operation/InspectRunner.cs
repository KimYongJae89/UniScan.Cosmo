using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
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
using DynMvp.InspData;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;
using DynMvp.Data;
using UniScanM.EDMS.State;

namespace UniScanM.EDMS.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        bool resetZeroing;

        private struct LastGrabbedIamgeDevice
        {
            public ImageDevice imageDevice;
            public IntPtr ptr;
        }
        LastGrabbedIamgeDevice lastGrabbedIamgeDevice;
        float asyncGrabExpUs = -1;
        ThreadHandler runningThreadHandler = null;  // 검사시 While 돌릴 쓰레드.

        public float AsyncGrabExpUs
        {
            get { return asyncGrabExpUs; }
            set { asyncGrabExpUs = value; }
        }
        
        public InspectRunner() : base()
        {
            lastGrabbedIamgeDevice = new LastGrabbedIamgeDevice();

            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting += OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            //ExitWaitInspection();
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");
            if (SystemManager.Instance().CurrentModel == null)
                return false;

            //Model model = SystemManager.Instance().CurrentModel;
            if (SystemManager.Instance().CurrentModel.IsTaught() == false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;
            
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed += ImageGrabbed;
                imageDevice.SetExposureTime(50);
            }

            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            SystemState.Instance().SetInspect();
            //SystemState.Instance().SetInspectState(InspectState.Run);

            bool ok = PostEnterWaitInspection();

            if (ok)
            {
                inspectProcesser = new InspectionState();
                runningThreadHandler = new ThreadHandler("InspectRunner", new Thread(RunningThreadProc), false);
                runningThreadHandler.Start();
            }
            return ok;
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");
            bool autoLight = Settings.EDMSSettings.Instance().AutoLight;

            float lineSpd = (float)SystemManager.Instance().InspectStarter.GetLineSpeed();
            bool good = true;
            LightValue lightValue = (LightValue)SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;

            if (lineSpd > 0)
            {
                ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

                SystemManager.Instance().DeviceBox.CameraCalibrationList.ForEach(f =>
                {
                    float hz = 80E3f;
                    if (autoLight)
                        hz = lineSpd / (6 * f.PelSize.Height) * 1E5f;
                    good &= imageDeviceHandler[f.CameraIndex].SetAcquisitionLineRate(hz);
                });

                if (autoLight)
                    lightValue = GetAutoLightValue2(lineSpd);
            }

            LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
            lightCtrl.TurnOn(lightValue);

            return true;
        }

        private LightValue GetAutoLightValue(float lineSpd)
        {
            // 80kHz일 때 조명값 50,150
            // spd => back, top
            // 5 => 50, 80
            // 10 => 55, 130
            // 20 => 60, 180
            // 40 => 65, 230
            // 80 => 70, 255
            int[] value = new int[2];
            if (lineSpd < 5)
            {
                value[0] = (int)Math.Round((lineSpd - 5) * 10 + 50);
                value[1] = (int)Math.Round((lineSpd - 5) * 10 + 80);
            }
            else
            {
                double x = Math.Log(lineSpd / 5, 2);
                value[0] = (int)Math.Min(255, Math.Round(5 * x + 50));
                value[1] = (int)Math.Min(255, Math.Round(50 * x + 80));
            }

            value[0] += Settings.EDMSSettings.Instance().AutoLightOffsetBottom;
            value[1] += Settings.EDMSSettings.Instance().AutoLightOffsetTop;

            return new LightValue(value);
        }


        private LightValue GetAutoLightValue2(float lineSpd)
        {
            int[] value = new int[2];

            float s0, x00, y00;
            float s1, x10, y10;
            if (lineSpd <= 2)
            {
                s0 = 1; x00 = 1; y00 = 3;
                s1 = 0; x10 = 1; y10 = 5;
            }
            else if (lineSpd <= 5)
            {
                s0 = 1f / 3f; x00 = 2; y00 = 4;
                s1 = 5f / 3f; x10 = 2; y10 = 5;
            }
            else
            {
                s0 = 1f / 5f; x00 = 5; y00 = 5;
                s1 = 5f / 5f; x10 = 5; y10 = 10;
            }

            value[0] = (int)Math.Round(s0 * (lineSpd - x00) + y00);
            value[1] = (int)Math.Round(s1 * (lineSpd - x10) + y10);


            value[0] += Settings.EDMSSettings.Instance().AutoLightOffsetBottom;
            value[1] += Settings.EDMSSettings.Instance().AutoLightOffsetTop;

            return new LightValue(value);

        }
        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            lastGrabbedIamgeDevice.imageDevice = imageDevice;
            lastGrabbedIamgeDevice.ptr = ptr;
        }

        private void RunningThreadProc()
        {
            while (runningThreadHandler.RequestStop == false)
            {
                //SystemState.Instance().SetInspect();
                // Grab
                if (Grab() == false)
                {
                    Thread.Sleep(500);
                    if (runningThreadHandler.RequestStop == false)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)ErrorSubSection.CommonReason,
                        ErrorLevel.Fatal, "Grab Fail", "Grabber", "Grab Timeout", "Check the Grabber and Camera");
                        runningThreadHandler.RequestStop = true;
                    }
                    continue;
                }
                
                DynMvp.InspData.InspectionResult inspectionResult = BuildInspectionResult();
                try
                {
                    if (inspectionResult == null)
                        continue;

                    ImageDevice imageDevice = this.lastGrabbedIamgeDevice.imageDevice;
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
                finally
                {
                    int sleepTimeMs = (int)Math.Round(1000f / Settings.EDMSSettings.Instance().MaxMeasCountPerSec);
                    if (inspectionResult != null)
                        sleepTimeMs = (int)Math.Max(0, sleepTimeMs - inspectionResult.InspectionTime.TotalMilliseconds);

                    Thread.Sleep(sleepTimeMs);
                }

                //SystemState.Instance().SetIdle();
                //GC.Collect();
                //GC.WaitForFullGCComplete();

            }

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
        }


        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");

            AlgoImage algoImage = null;
            try
            {
                ImageD imageD = imageDevice.GetGrabbedImage(ptr);

                if (imageDevice.IsBinningVirtical())
                    imageD = ((UniScanM.Operation.InspectRunnerExtender)inspectRunnerExtender).GetBinningImage(imageD);

                algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);

                Debug.Assert(algoImage != null);
                inspectRunnerExtender.OnPreInspection();

                Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;

                if (resetZeroing == true)
                {
                    edmsResult.ResetZeroing = this.resetZeroing;
                    resetZeroing = false;
                }

                this.inspectProcesser.Process(algoImage, inspectionResult, inspectionOption);
         
                ProductInspected(inspectionResult);
            }
            finally
            {
                algoImage?.Dispose();
                SystemState.Instance().SetInspectState(InspectState.Wait);
            }
        }
        
        private bool Grab()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.GrabOnce();
            bool ok = imageDeviceHandler.WaitGrabDone(5000);
            //imageDeviceHandler.Stop();

            return ok;
        }

        public override void ResetState()
        {
            resetZeroing = true;
        }

        public override void PreExitWaitInspection()
        {
            base.PreExitWaitInspection();

            if (runningThreadHandler != null && runningThreadHandler.IsRunning)
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() => runningThreadHandler?.Stop());
            }
            runningThreadHandler = null;
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
        }

        public override void ExitWaitInspection()
        {
            PreExitWaitInspection();
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ExitWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed -= ImageGrabbed;

            SystemManager.Instance().ProductionManager.Save();

            SystemState.Instance().SetIdle();
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult edmsInspectionResult = (Data.InspectionResult)inspectionResult;

            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);

            bool isWaitState = edmsInspectionResult.IsWaitState;
            bool isMeasureState = edmsInspectionResult.IsMeasureState;
            if (isMeasureState)
            {
                if (inspectionResult.Judgment != Judgment.Skip)
                {
                    UniScanM.Data.Production production = this.UpdateProduction(edmsInspectionResult);
                    lock (production)
                    {
                        double value = Math.Abs(edmsInspectionResult.TotalEdgePositionResult[(int)Data.DataType.Printing_FilmEdge_0]);
                        production.Value = (float)Math.Max(production.Value, value);
                    }
                }
                SystemManager.Instance().ExportData(inspectionResult);
            }
        }

        public void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public void OnLotChanged()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }
    }
}

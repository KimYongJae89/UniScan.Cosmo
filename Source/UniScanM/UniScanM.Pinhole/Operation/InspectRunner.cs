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
using UniScanM.Data;
using System.Threading;
using UniEye.Base.Device;
using UniScanM.Algorithm;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI.Touch;
using UniEye.Base;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using System.Drawing.Imaging;
using UniScanM.Pinhole.Algorithm;
using UniScanM.Pinhole.Data;
using DynMvp.Devices.Dio;
using UniScanM.Pinhole.UI.MenuPanel;
using UniScanM.Pinhole.Settings;
using UniScanM.UI.MenuPage.AutoTune;
using System.Windows.Forms;
using UniScanM.Operation;

namespace UniScanM.Pinhole.Operation
{
    public delegate bool EnterWaitInspectionDelegate();
    public delegate void ExitWaitInspectionDelegate();

    public delegate void UpdateRealTimeImage(Bitmap bitmap, int deviceIndex);

    public class InspectRunner : UniEye.Base.Inspect.DirectTriggerInspectRunner
    {
        float asyncGrabExpUs = -1;
        

        bool onReject = false;
        bool onRejectProcess = false;
        PinholeChecker pinholeChecker = new PinholeChecker();
        IoPort resultNg;
        DigitalIoHandler digitalIoHandler;
        //int prepareSectionIndex = 0;  // 어디에 쓰는 물건인고?

        public UpdateRealTimeImage UpdateRealTimeImage;

        GrabbedImageList grabbedImageList = new GrabbedImageList();

        object[] lockObject;
        IntPtr[] nextPtr = new IntPtr[2];
        int [] nextSectionIndex = new int[2] {-1, -1};

        CancellationTokenSource ctsInspTask = new CancellationTokenSource();

        public float AsyncGrabExpUs
        {
            get { return asyncGrabExpUs; }
            set { asyncGrabExpUs = value; }
        }
        
        public InspectRunner() : base()
        {
            PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;
            resultNg = portMap.OutResultNg;
            digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            int count = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;
            this.lockObject = new object[count];
            this.nextPtr = new IntPtr[count];
            this.nextSectionIndex = new int[count];
            for (int i = 0; i < count; i++)
                this.lockObject[i] = new object();
            
            this.Reset();

            SystemManager.Instance().InspectStarter.OnStartInspection = EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection = ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting = OnRewinderCut;
            SystemManager.Instance().InspectStarter.OnLotChanged = OnLotChanged;
        }

        private void OnLotChanged()
        {
            this.Reset();
            //Data.Production production = (Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            //int lastSection = production == null ? -1 : Math.Max(production.Section1, production.Section2);
            //for (int i = 0; i < sectionIndex.Length; i++)
            //    sectionIndex[i] = lastSection;
        }

        private void OnRewinderCut()
        {
            //for(int i = 0; i < 2; i++)
            //{
            //    sectionIndex[i] = -1;
            //}
            SystemManager.Instance().MainForm?.InspectPage?.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override void Dispose()
        {
            //ExitWaitInspection();
            base.Dispose();
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");

            //string lotNo = SystemManager.Instance().InspectStarter.GetLotNo();
            //if (string.IsNullOrEmpty(lotNo))
            //    lotNo = "Unnown";

            //SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);

            SystemState.Instance().SetWait();
            Reset();
            SetCameraLineSpeed();

            //SystemManager.Instance().MainForm?.InspectPage.InspectionPanelList[0].EnterWaitInspection();  // 화면 초기화 필요시 넣을 것

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            float lineSpeed = (float)SystemManager.Instance().InspectStarter.GetLineSpeed();

            for (int i = 0; i <this.lockObject.Length; i++)
            {
                lock (this.lockObject[i])
                {
                    nextPtr[i] = IntPtr.Zero;

                    Camera camera = (Camera)imageDeviceHandler.GetImageDevice(i);
                    StartTask(camera, i);
                }
            }

            return PostEnterWaitInspection();
        }

        Task StartTask(Camera camera, int camIndex)
        {
            
            return Task.Run(() =>
            {
                while (SystemState.Instance().GetOpState() !=  OpState.Idle)
                {
                    IntPtr ptr = IntPtr.Zero;
                    int sectionId = -1;
                    if (nextPtr[camIndex] != IntPtr.Zero)
                    {
                        lock (this.lockObject[camIndex])
                        {
                            sectionId = nextSectionIndex[camIndex];
                            ptr = nextPtr[camIndex];
                            nextPtr[camIndex] = IntPtr.Zero;
                        }
                    }
                 
                    if (ptr != IntPtr.Zero)
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        LogHelper.Debug(LoggerType.Inspection, string.Format("Start InspectFilm - ({0}/{1})", camIndex, sectionId));

                        Data.InspectionResult inspResult = CreateInspectionResult(camIndex, sectionId);
                        Inspect(camera, ptr, inspResult, null);

                        LogHelper.Debug(LoggerType.Inspection, string.Format("End InspectFilm - ({0}/{1}) / Time = {2}", camIndex, sectionId, sw.ElapsedMilliseconds));
                    }


                    Thread.Sleep(1);
                }
            });
        }

        void SetCameraLineSpeed()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            float lineSpeed = (float)SystemManager.Instance().InspectStarter.GetLineSpeed();
            for (int i = 0; i < 2; i++)
            {
                Camera camera = (Camera)imageDeviceHandler.GetImageDevice(i);
                if (camera != null)
                {
                    float pixelResoultion = (float)PinholeSettings.Instance().PixelResolution / 1000.0f;

                    if (lineSpeed == 0)
                        lineSpeed = 120;                    

                    float Hz = ((lineSpeed / 60)  * 1000) / pixelResoultion;

                    bool set = camera.SetAcquisitionLineRate(Hz);//7092.1985(100m/min)
                }
            }
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            TimeSpan idltTimeSpan = TimeSpan.MaxValue;

            if (((InspectRunnerExtender)inspectRunnerExtender).EndTime.HasValue)
                idltTimeSpan = DateTime.Now - ((InspectRunnerExtender)inspectRunnerExtender).EndTime.Value;

            SystemState.Instance().SetInspect();
            ((InspectRunnerExtender)inspectRunnerExtender).StartTime = DateTime.Now;
            ((InspectRunnerExtender)inspectRunnerExtender).EndTime = null;
            
            bool tooLongIdle = idltTimeSpan.TotalMinutes > 10;
            //SystemState.Instance().SetWait();
            SystemState.Instance().SetInspectState(InspectState.Tune);
            Application.DoEvents();

            //AutoTuneForm autoTuneForm = new AutoTuneForm(TuneDone, AutoTuneType.Otsu, null, true);
            //if(autoTuneForm.ShowDialog() == DialogResult.Cancel)
            //{
            //    return false;
            //}
            //SystemState.Instance().SetInspectState(InspectState.Ready);
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);

            Thread.Sleep(100);

            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                imageDevice.ImageGrabbed = ImageGrabbed;
                //if (imageList != null)
                //    pinholeChecker.AutoSet(imageDevice.Index, imageList[imageDevice.Index]);
            }
            SystemState.Instance().SetInspectState(InspectState.Run);

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            Thread.Sleep(200);

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            return true;
        }
        
        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            lock (this.lockObject[imageDevice.Index])
            {
                if (nextPtr[imageDevice.Index] != IntPtr.Zero)
                    LogHelper.Debug(LoggerType.Error, string.Format("nextPtr fault {0}, section {1}", imageDevice.Index, nextSectionIndex[imageDevice.Index]));

                nextSectionIndex[imageDevice.Index]++;
                nextPtr[imageDevice.Index] = ptr;
            }
            //LogHelper.Debug(LoggerType.Device, string.Format("nextPtr suc {0}, section {1}", imageDevice.Index, ptr));
        }

        public void RewinderCut()
        {
            
        }

        
        private void SendError(int sectionIndex)
        {
            if (onReject)
                return;
            onReject = true;
            //prepareSectionIndex = sectionIndex;
            Task.Run(() =>
            {
                onRejectProcess = true;

                digitalIoHandler.WriteOutput(resultNg, true);
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                
                Thread.Sleep(50);

                //onReject = false;
                digitalIoHandler.WriteOutput(resultNg, false);
                onReject = false;
                onRejectProcess = false;
            });
        }

        private void SendReset(int sectionIndex)
        {
            if (onReject == false )
                return;
            if (onRejectProcess)
                return;
            //if (prepareSectionIndex <= sectionIndex)
            //{
            //    prepareSectionIndex = sectionIndex;
            //    return;
            //}
                
            //두개의 동시 싱크 맞추기는 불가능에 가까움
            Task.Run(() =>
            {
                digitalIoHandler.WriteOutput(resultNg, false);
                onReject = false;
            });
        }

        private bool Grab()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                if (imageDevice is CameraVirtual)
                {
                    CameraVirtual cameraVirtual = (CameraVirtual)imageDevice;
                }
            }

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);
            imageDeviceHandler.GrabOnce();
            bool ok = imageDeviceHandler.WaitGrabDone(5000);
            imageDeviceHandler.Stop();
            lightCtrlHandler.TurnOff();
            return ok;
        }

        public override void PreExitWaitInspection()
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::PreExitWaitInspection");

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            if (inspectRunnerExtender != null)
                ((InspectRunnerExtender)inspectRunnerExtender).EndTime = DateTime.Now;
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();
            onReject = false;

            //PreExitWaitInspection();
            //LogHelper.Debug(LoggerType.Function, "InspectRunner::ExitWaitInspection");

            //ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            //imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            //foreach (ImageDevice imageDevice in imageDeviceHandler)
            //    imageDevice.ImageGrabbed -= ImageGrabbed;

            //onReject = false;
            ////SystemState.Instance().SetIdle();
            //SystemState.Instance().SetInspectState(InspectState.Stop);
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::ProductInspected");

            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            UniScanM.Pinhole.Data.InspectionResult pinholeInspectionResult = (UniScanM.Pinhole.Data.InspectionResult)inspectionResult;

            //UniScanM.Pinhole.Data.Production p = (UniScanM.Pinhole.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.GetProduction(pinholeInspectionResult);
            lock (production)
            {
                production.Update(pinholeInspectionResult);
            }

            UniScanM.Data.InspectionResult sheetInspectionResult = (UniScanM.Data.InspectionResult)inspectionResult;
            SystemManager.Instance().MainForm?.InspectPage.ProductInspected(sheetInspectionResult);
            SystemManager.Instance().ExportData(inspectionResult);
        }

        public void SetupAutoTune()
        {

        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            SystemState.Instance().SetInspectState(InspectState.Run);
            ImageD grabbedImage = imageDevice.GetGrabbedImage(ptr);

            int procIndex = imageDevice.Index;

            Debug.Assert(grabbedImage != null);

            Data.InspectionResult sheetInspResult = (Data.InspectionResult)inspectionResult;
            try
            {
                pinholeChecker.Inspect(grabbedImage, sheetInspResult);
            }
            catch (Exception ex)
            {                                                 
                LogHelper.Debug(LoggerType.Inspection, ex.Message);
                LogHelper.Debug(LoggerType.Inspection, ex.StackTrace);
            }
            if (PinholeSettings.Instance().UseReject)
            {
                if (inspectionResult.Judgment != Judgment.Accept && OperationOption.Instance().OnTune == false)
                {
                    SendError(sheetInspResult.SectionIndex);
                }
            }

            ProductInspected(sheetInspResult);
            SystemState.Instance().SetInspectState(InspectState.Done);
        }

        private void Reset()
        {
            //prepareSectionIndex = 0;
            Data.Production production = (Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            int lastSection = production == null ? -1 : Math.Max(production.Section1, production.Section2);
            for (int i = 0; i < nextSectionIndex.Length; i++)
                nextSectionIndex[i] = lastSection;
            digitalIoHandler.WriteOutput(resultNg, false); // 검사 시작시 IO를 리셋한다.
        }

        internal void ToggleAutoTuneMode()
        {

        }

        private Data.InspectionResult CreateInspectionResult(int deviceIndex, int sectionIndex)
        {
            Data.InspectionResult inspectionResult = this.inspectRunnerExtender.BuildInspectionResult() as Data.InspectionResult;

            inspectionResult.SectionIndex = sectionIndex;
            inspectionResult.DeviceIndex = deviceIndex;

            return inspectionResult;
        }

        string GetResultPath(string modelName, DateTime startTime, string lotNo, int deviceIndex)
        {
            string autoManual = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto ? "Auto" : "Manual";
            string result = Path.Combine(PathSettings.Instance().Result, startTime.ToString("yyyyMMdd"), modelName, autoManual, lotNo);
            return result;
        }

        void SetLineSpeed()
        {
            float lineSpeed = GetLineSpeed();
            for(int i = 0; i < 2; i++)
            {
                ImageDevice cam = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(i);
            }
        }

        float GetLineSpeed()
        {
            float sheetSpeed = 10;
            float resolution = 0.122f;
            float result = ((sheetSpeed / 60) * 1000) / resolution;
            return result;
        }

        public void DemoInspect()
        {

        }
    }

    public class GrabbedImage
    {
        int index = 0;
        Bitmap bitmap;

        public int Index { get => index; set => index = value; }
        public Bitmap Bitmap { get => bitmap;}

        public GrabbedImage(int index, Bitmap bitmap)
        {
            this.index = index;
            this.bitmap = bitmap;
        }
    }

    public class GrabbedImageList : List<GrabbedImage>
    {

    }
}


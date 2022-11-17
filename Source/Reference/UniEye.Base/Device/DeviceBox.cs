using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.Daq;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;
using DynMvp.Devices.Comm;
using DynMvp.Device.Serial;
using UniEye.Base.MachineInterface;
using DynMvp.UI.Touch;

namespace UniEye.Base.Device
{
    public class DeviceBox
    {
        PortMapBase portMap = null;
        public PortMapBase PortMap
        {
            get { return portMap; }
            set { portMap = value; }
        }

        DigitalIoHandler digitalIoHandler = null;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
        }

        MotionList motionList = null;
        public MotionList MotionList
        {
            get { return motionList; }
        }

        AxisConfiguration axisConfiguration = null;
        public AxisConfiguration AxisConfiguration
        {
            get { return axisConfiguration; }
        }

        GrabberList grabberList = null;
        public GrabberList GrabberList
        {
            get { return grabberList; }
        }

        ImageDeviceHandler imageDeviceHandler = null;
        public ImageDeviceHandler ImageDeviceHandler
        {
            get { return imageDeviceHandler; }
        }

        List<Calibration> cameraCalibrationList = new List<Calibration>();
        public List<Calibration> CameraCalibrationList
        {
            get { return cameraCalibrationList; }
        }

        LightCtrlHandler lightCtrlHandler = new LightCtrlHandler();
        public LightCtrlHandler LightCtrlHandler
        {
            get { return lightCtrlHandler; }
        }

        SerialDeviceHandler serialDeviceHandler = new SerialDeviceHandler();
        public SerialDeviceHandler SerialDeviceHandler
        {
            get { return serialDeviceHandler; }
        }

        private ImageAcquisition imageAcquisition;

        private IReportProgress reportProgress = null;
        public IReportProgress ReportProgress
        {
            get { return reportProgress; }
            set { reportProgress = value; }
        }

        private List<DaqChannel> daqChannelList;
        public List<DaqChannel> DaqChannelList
        {
            get { return daqChannelList; }
        }

        protected MachineIf machineIf;
        public MachineIf MachineIf
        {
            get { return machineIf; }
        }

        protected List<MachineIf> machineIfList = new List<MachineIf>();
        public List<MachineIf> MachineIfList
        {
            get { return machineIfList; }
        }


        public DeviceBox(PortMap portMap)
        {
            this.portMap = portMap;
            this.portMap.Load();
        }

        private void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
                reportProgress.ReportProgress(percentage, StringManager.GetString(message));
        }

        public ImageAcquisition GetImageAcquisition()
        {
            if (imageAcquisition == null)
            {
                imageAcquisition = new ImageAcquisition();
                imageAcquisition.Initialize(imageDeviceHandler, lightCtrlHandler, MachineSettings.Instance().NumLightType, 24);
            }

            return imageAcquisition;
        }

        public void InitializeCameraAndLight()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - InitializeCameraAndLightMachine");

            MachineSettings machineSettings = MachineSettings.Instance();
            bool isVirtual = machineSettings.VirtualMode;

            grabberList = new GrabberList();
            grabberList.Initialize(machineSettings.GrabberInfoList, isVirtual);
            InitializeCamera();

            DoReportProgress(reportProgress, 35, "Initialize Light Ctrl");
            InitializeLightCtrl(machineSettings.LightCtrlInfoList, isVirtual);
        }

        public void Initialize(IReportProgress reportProgress)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - Initialize Machine");

            MachineSettings machineSettings = MachineSettings.Instance();
            bool isVirtual = machineSettings.VirtualMode;

            DoReportProgress(reportProgress, 15, "Initialize Grabber");

            grabberList = new GrabberList();
            grabberList.Initialize(machineSettings.GrabberInfoList, isVirtual);
            InitializeCamera();

            DoReportProgress(reportProgress, 20, "Initialize Motion");
            InitializeMotion(machineSettings.MotionInfoList, isVirtual);

            DoReportProgress(reportProgress, 25, "Initialize Digital IO");
            InitializeDigitalIo(machineSettings.DigitalIoInfoList, isVirtual);

            portMap = portMap;// SystemManager.Instance().CreatePortMap();
            //portMap = new PortMap();
            //portMap.SetupPorts();

            DoReportProgress(reportProgress, 35, "Initialize Light Ctrl");
            InitializeLightCtrl(machineSettings.LightCtrlInfoList, isVirtual);

            DoReportProgress(reportProgress, 50, "Initialize DAQ Device");
            InitializeDaqDevice(machineSettings.DaqChannelPropertyList, isVirtual);

            DoReportProgress(reportProgress, 60, "Initialize Serial Device");
            InitializeSerialDevice(machineSettings.SerialDeviceInfoList, isVirtual);

            DoReportProgress(reportProgress, 70, "Initialize MachineIF");
            InitializeMachineIF(machineSettings.MachineIfSetting, isVirtual);
            //InitializeMachineIFList(machineSettings.MachineIfSettingList, isVirtual);

            PostInitialize();
        }

        public virtual void PostInitialize()
        {
            
        }

        public void InitializeMachineIFList(List<MachineIfSetting> machineIfSettingList, bool isVirtual)
        {
            foreach(MachineIfSetting tempMachineIfSetting in machineIfSettingList)
            {
                MachineIf tempMachineIf = MachineIf.Create(tempMachineIfSetting, isVirtual);
                if(tempMachineIf != null)
                {
                    MachineIfList.Add(tempMachineIf);
                }
            }
            foreach(MachineIf tempMachineIf in MachineIfList)
            {
                tempMachineIf.Initialize();
                MachineIfExecuter[] machineIfExecuters = SystemManager.Instance().CreateMachineIfExecuter();
                tempMachineIf.AddExecuters(machineIfExecuters);
                tempMachineIf.Start();
            }

        }

        public void InitializeMachineIF(MachineIfSetting machineIfSetting, bool isVirtual)
        {
            machineIf = MachineIf.Create(machineIfSetting, isVirtual);
            if (machineIf != null)
            {
                machineIf.Initialize();
                MachineIfExecuter[] machineIfExecuters = SystemManager.Instance().CreateMachineIfExecuter();
                machineIf.AddExecuters(machineIfExecuters);
                machineIf.Start();
            }
        }

        private void InitializeSerialDevice(SerialDeviceInfoList serialDeviceInfoList, bool isVirtual)
        {
            foreach (SerialDeviceInfo serialDeviceInfo in serialDeviceInfoList)
            {
                SerialDevice serialDevice = serialDeviceInfo.BuildSerialDevice(isVirtual);
                if (serialDevice == null)
                {
                    string message = string.Format("Serial Device \'{0}\' Create Failed", serialDeviceInfo.DeviceName);
                    LogHelper.Error(LoggerType.Debug, message);
                    ErrorManager.Instance().Report((int)ErrorSection.Initialize, (int)CommonError.FailToInitialize, ErrorLevel.Error, ErrorSection.Machine.ToString(), CommonError.FailToInitialize.ToString(), message, "", "");
                    continue;
                }

                if (serialDevice.Initialize(/*serialDeviceInfo, machineSettings.VirtualMode*/) == false)
                {
                    string message = string.Format("Serial Device \'{0}\' Initialize Failed", serialDeviceInfo.DeviceName);
                    LogHelper.Error(LoggerType.Debug, message);
                    ErrorManager.Instance().Report((int)ErrorSection.Initialize, (int)CommonError.FailToInitialize, ErrorLevel.Error, ErrorSection.Machine.ToString(), CommonError.FailToInitialize.ToString(), message, "", "");

                    serialDevice = serialDeviceInfo.BuildSerialDevice(true);
                    serialDevice.Initialize(/*serialDeviceInfo, true*/);
                }
                serialDeviceHandler.Add(serialDevice);
            }
        }

        private void SaveCameraConfiguration(CameraConfiguration cameraConfiguration, string grabberName)
        {
            string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabberName);
            cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private CameraConfiguration LoadCameraConfiguration(Grabber grabber)
        {
            CameraConfiguration cameraConfiguration = new CameraConfiguration();
            cameraConfiguration.RequiredCameras = grabber.NumCamera;

            string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabber.Name);
            cameraConfiguration.LoadCameraConfiguration(filePath);

            return cameraConfiguration;
        }

        private void InitializeCamera()
        {
            imageDeviceHandler = new ImageDeviceHandler();

            AddCamera();

            if (OperationSettings.Instance().ImagingLibrary != ImagingLibrary.Custom)
                LoadCameraCalibration();

            imageDeviceHandler.SetTriggerDelay(TimeSettings.Instance().TriggerDelayMs);
        }

        private void AddCamera()
        {
            int index = 0;

            foreach (Grabber grabber in grabberList)
            {
                CameraConfiguration cameraConfiguration = LoadCameraConfiguration(grabber);
                if (cameraConfiguration.RequiredCameras != grabber.NumCamera)
                    MessageForm.Show(null, "Please Check Camera Settings");

                foreach (CameraInfo cameraInfo in cameraConfiguration)
                {
                    Camera camera = CreateCamera(grabber);
                    grabber.UpdateCameraInfo(cameraInfo);

                    if (string.IsNullOrEmpty(cameraInfo.VirtualImagePath) || Directory.Exists(cameraInfo.VirtualImagePath)==false)
                    {
                        MessageForm.Show(null, "VirtualImagePath is Incorrect");
                        cameraInfo.VirtualImagePath = PathSettings.Instance().VirtualImage;
                    }

                    cameraInfo.Index = index++;

                    imageDeviceHandler.AddCamera(camera, cameraInfo);
                }
                //imageDeviceHandler.AddCamera(grabber, cameraConfiguration);
                SaveCameraConfiguration(cameraConfiguration, grabber.Name);
            }
        }

        public virtual Camera CreateCamera(Grabber grabber)
        {
            return grabber.CreateCamera();
        }

        private void LoadCameraCalibration()
        {
            foreach (Camera camera in imageDeviceHandler)
            {
                string datFileName = String.Format(@"{0}\Calibration{1}.xml", PathSettings.Instance().Config, camera.Index);
                string gridFileName = String.Format(@"{0}\Calibration{1}.dat", PathSettings.Instance().Config, camera.Index);

                Calibration calibration = AlgorithmBuilder.CreateCalibration();

                if (calibration != null)
                {
                    calibration.Initialize(camera.Index, datFileName, gridFileName);
                    cameraCalibrationList.Add(calibration);

                    camera.UpdateFovSize(calibration.PelSize);
                    calibration.UpdatePelSize(camera.ImageSize.Width, camera.ImageSize.Height);
                }
            }
        }

        private void InitializeMotion(MotionInfoList motionInfoList, bool isVirtual)
        {
            motionList = new MotionList();
            motionList.Initialize(motionInfoList, isVirtual);

            axisConfiguration = new AxisConfiguration();

            if (motionList.Count > 0)
            {
                if (axisConfiguration.LoadConfiguration(motionList) == false)
                {
                    axisConfiguration.Initialize(motionList);
                }
            }
        }

        public void InitAxisConfiguration(List<string> axisHandlerNames)
        {
            axisConfiguration.SetupAxisHandler(axisHandlerNames.ToArray());

        }

        private void InitializeDigitalIo(DigitalIoInfoList digitalIoInfoList, bool isVirtual)
        {
            digitalIoHandler = new DigitalIoHandler();

            IDigitalIo digitalIo;
            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                if (isVirtual)
                {
                    DigitalIoInfo digitalIoInfo2 = new DigitalIoInfo(DigitalIoType.Virtual, digitalIoInfo.Index,
                        digitalIoInfo.NumInPortGroup, 0, digitalIoInfo.NumInPort,
                        digitalIoInfo.NumOutPortGroup, 0, digitalIoInfo.NumOutPort);

                    digitalIo = DigitalIoFactory.Create(digitalIoInfo2);
                }
                else
                {
                    if (DigitalIoFactory.IsSlaveDevice(digitalIoInfo.Type))
                    {
                        digitalIo = (IDigitalIo)motionList.GetMotion(digitalIoInfo.Type.ToString());

                        if (digitalIo.Initialize(digitalIoInfo) == false)
                        {
                            ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                                ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Fail to initialize Digital I/O. {0}", digitalIoInfo.Type.ToString()));
                            digitalIo.UpdateState(DeviceState.Error, "DigitalIo is invalid.");
                            continue;
                        }
                        else
                        {
                            digitalIo = (IDigitalIo)motionList.GetMotion(digitalIoInfo.Type.ToString());
                            //digitalIoHandler.Add((IDigitalIo)motionList.GetMotion(digitalIoInfo.Type.ToString()));
                        }
                    }
                    else
                    {
                        digitalIo = DigitalIoFactory.Create(digitalIoInfo);
                    }
                }

                if (digitalIo != null)
                {
                    digitalIoHandler.Add(digitalIo);
                }
            }

            InitializeDigitalIo();
        }

        protected virtual void InitializeDigitalIo() { }

        private void InitializeLightCtrl(LightCtrlInfoList lightCtrlInfoList, bool isVirtual)
        {
            foreach (LightCtrlInfo lightCtrlInfo in lightCtrlInfoList)
            {
                LightCtrl lightCtrl = LightCtrlFactory.Create(lightCtrlInfo, digitalIoHandler, isVirtual);

                if (lightCtrl != null)
                {
                    if (lightCtrlInfo.Type == LightCtrlType.IO)
                    {
                        IoLightCtrlInfo ioLightCtrlInfo = (IoLightCtrlInfo)lightCtrlInfo;
                        portMap.GetIoLightPorts(ioLightCtrlInfo.LightCtrlIoPortList);
                    }

                    lightCtrl.LightStableTimeMs = TimeSettings.Instance().LightStableTimeMs;

                    lightCtrlHandler.AddLightCtrl(lightCtrl);
                }
            }

            LightSettings.Instance().Load();
        }

        private void InitializeDaqDevice(DaqChannelPropertyList daqChannelPropertyList, bool isVirtual)
        {
            daqChannelList = new List<DaqChannel>();

            foreach (DaqChannelProperty daqChannelProperty in daqChannelPropertyList)
            {
                DaqChannel daqChannel = DaqChannelManager.Instance().CreateDaqChannel(daqChannelProperty.DaqChannelType, "Daq Channel", isVirtual);

                if (daqChannel != null)
                {
                    daqChannel.Initialize(daqChannelProperty);
                    daqChannelList.Add(daqChannel);

                    DeviceManager.Instance().AddDevice(daqChannel);
                }
            }
        }

        public void Release()
        {
            foreach (Calibration calibration in cameraCalibrationList)
                calibration.Dispose();
            cameraCalibrationList.Clear();

            if (imageDeviceHandler != null)
                imageDeviceHandler.Release();

            if (grabberList != null)
                grabberList.Release();

            if (digitalIoHandler != null)
                digitalIoHandler.Release();

            if (motionList != null)
                motionList.Release();

            if (serialDeviceHandler != null)
                serialDeviceHandler.Release();

            if (lightCtrlHandler != null)
                lightCtrlHandler.Release();

            if (machineIf != null)
            {
                machineIf.Stop();
                machineIf.Release();
                machineIf = null;
            }
        }
    }
}
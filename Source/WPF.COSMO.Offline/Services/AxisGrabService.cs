using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.Base.Converters;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Converters;
using WPF.COSMO.Offline.Models;
using static WPF.COSMO.Offline.Models.PortMap;

namespace WPF.COSMO.Offline.Services
{
    public class ServiceSettings
    {
        //Common
        ObservableCollection<AxisGrabInfo> _axisGrabInfoList = new ObservableCollection<AxisGrabInfo>();
        public ObservableCollection<AxisGrabInfo> AxisGrabInfoList
        {
            get
            {
                if (AxisGrabService.IsSingleMode)
                    return new ObservableCollection<AxisGrabInfo>() { _axisGrabInfoList.OrderByDescending(info => info.OffsetX).First() };

                return _axisGrabInfoList;
            }
            set => _axisGrabInfoList = value;
        }

        public double Resolution { get; set; } = 3.5;

        public AxisHandler AxisHandler { get; set; }

        public int AxisIndex { get; set; } = 2;

        public int MoveVelocity { get; set; } = 100000;
        public int GrabVelocity { get; set; } = 25000;
        public int IonizerVelocity { get; set; } = 50000;

        public int IonizerY { get; set; }
        
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int CenterY => (MaxY - MinY) / 2 + MinY;

        //Calibration
        public int CalibrationScanLength { get; set; } = 10000;
        public double CalibrationRatio { get; set; } = 0.75;

        public int IgnoreLength = 2;

        //Align
        public double AlignRatioX { get; set; } = 0.1;
        public double AlignRatioY { get; set; } = 0.1;
        public double AlignDistTheshold { get; set; } = 15;
        public int AlignScanLength { get; set; } = 100000;

        public double AlignDegreeThreshold { get; set; } = 2;

        public double ImageResizeRatio { get; set; } = 0.25;

        public double RealScale => Resolution / ImageResizeRatio;
        
        public int LineCameraLightValue { get; set; } = 200;
        public int MicroscopeLightValue { get; set; } = 200;

        public byte LineEstimateNum { get; set; } = 3;

        public int RegionBinAddValue { get; set; } = 5;

        public int EdgeMaxDist { get; set; } = 500;
        public int EdgeDilate { get; set; } = 20;
        public int DefectClose { get; set; } = 2;
        
        public int AverageLength { get; set; } = 50;

        public byte BinaryMinValue { get; set; } = 6;

        public ServiceSettings()
        {
            AxisHandler = MotionService.Handlers.First();
        }
    }

    public delegate void EmptyDelegate();
    public delegate void AxisImageGrabbedDelegate(AxisGrabInfo info, AxisImage axisImage);

    public static class AxisGrabService
    {
        public static bool IsSingleMode { get; set; }

        const string _key = "AxisGrab";

        public static EmptyDelegate Initialized;

        public static AxisImageGrabbedDelegate AxisImageGrabbed;

        private static Dictionary<ImageDevice, ConcurrentQueue<IntPtr>> imageDevicePtrQueue = new Dictionary<ImageDevice, ConcurrentQueue<IntPtr>>();
        private static Dictionary<ImageDevice, ManualResetEvent> triggerResetEventDictionary = new Dictionary<ImageDevice, ManualResetEvent>();
        private static Dictionary<ImageDevice, CudaImage> preProcessDictionary = new Dictionary<ImageDevice, CudaImage>();

        private static Dictionary<ImageDevice, double> _positionDictionary = new Dictionary<ImageDevice, double>();

        private static CancellationToken _token;
        private static Model_COSMO _model;

        private static ServiceSettings _settings;
        public static ServiceSettings Settings => _settings;

        static int _grabOffset { get; set; } = 5000;

        [JsonIgnore]
        public static int NextY { get; set; }

        public static bool CalibrationMode { get; set; }

        public static async Task InitializeAsync()
        {
            await LoadAxisGrabInfos();
            await SaveAxisGrabInfos();
        }

        public static void Initialize(Model_COSMO model, CancellationToken token)
        {
            _model = model;
            _token = token;
            
            Release();

            int index = 0;
            CudaMethods.CUDA_INITIALIZE(ref index);

            foreach (var info in _settings.AxisGrabInfoList)
            {
                info.ImageDevice.ImageGrabbed += ImageGrabbed;
                imageDevicePtrQueue[info.ImageDevice] = new ConcurrentQueue<IntPtr>();
                triggerResetEventDictionary[info.ImageDevice] = new ManualResetEvent(false);

                CudaImage srcImage = new CudaDepthImage<byte>();
                srcImage.Alloc(info.ImageDevice.ImageSize.Width, info.ImageDevice.ImageSize.Height);
                preProcessDictionary[info.ImageDevice] = srcImage;

                info.ImageDevice.GrabMulti();
            }

            if (Initialized != null)
                Initialized();
        }

        public static void Release()
        {
            ErrorManager.Instance().ResetAlarm();

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

            CudaMethods.CUDA_RELEASE();

            foreach (var info in _settings.AxisGrabInfoList)
            {
                info.ImageDevice.ImageGrabbed -= ImageGrabbed;
                imageDevicePtrQueue[info.ImageDevice] = null;
                if (preProcessDictionary.ContainsKey(info.ImageDevice))
                {
                    preProcessDictionary[info.ImageDevice]?.Dispose();
                    preProcessDictionary[info.ImageDevice] = null;
                }
            }
        }
        
        private static void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (triggerResetEventDictionary[imageDevice].WaitOne(0) == false)
                return;

            imageDevicePtrQueue[imageDevice].Enqueue(ptr);

            var founded = _settings.AxisGrabInfoList.First(info => info.ImageDevice == imageDevice);

            int x = founded.OffsetX + founded.NextX;
            int y = founded.OffsetY;

            lock (founded)
            {
                y += (int)_positionDictionary[imageDevice];
                _positionDictionary[imageDevice] += founded.ImageHeight * _settings.Resolution;
            }

            int width = imageDevice.ImageSize.Width;
            int height = imageDevice.ImageSize.Height;
            int pitch = imageDevice.ImagePitch;

            byte[] data = new byte[width * height];

            for (int src = 0, dest = 0; src < pitch * height; src += pitch, dest += width)
                Marshal.Copy(ptr + src, data, dest, width);

            Task.Run(async () =>
            {
                if (CalibrationMode == false)
                {
                    if (_model.CalibrationDataMap.ContainsKey(founded.Name))
                    {
                        lock (preProcessDictionary[imageDevice])
                        {
                            preProcessDictionary[imageDevice].SetByte(data);
                            CudaMethods.CUDA_MATH_MUL(preProcessDictionary[imageDevice].ImageID, _model.CalibrationDataMap[founded.Name]);
                            data = preProcessDictionary[imageDevice].GetByte();
                        }
                    }
                }

                if (AxisImageGrabbed != null)
                    AxisImageGrabbed(founded, new AxisImage(data,x, y));

                while (imageDevicePtrQueue[imageDevice].TryDequeue(out IntPtr temp) == false)
                {
                    await Task.Delay(100);
                }
            });
        }

        public static void AddAxisDevice()
        {
            if (SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count == 0)
                return;
            
            _settings.AxisGrabInfoList.Add(new AxisGrabInfo(SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList.First()));
        }

        public static async Task<bool> IonizerMoveNextPos()
        {
            var axisHandler = _settings.AxisHandler;
            var axisPosition = axisHandler.GetActualPos();
            axisHandler.SetPosition(axisPosition);

            foreach (var info in _settings.AxisGrabInfoList)
                axisPosition[info.AxisIndex] = info.NextX;

            axisPosition[_settings.AxisIndex] = NextY;

            axisHandler.StartMultipleMove(axisPosition, _settings.IonizerVelocity);

            return await MotionService.WaitMoveDone(_token);
        }

        public static async Task<bool> MoveNextPos()
        {
            var axisHandler = _settings.AxisHandler;

            var axisPosition = axisHandler.GetActualPos();
            
            axisHandler.SetPosition(axisPosition);
            
            foreach (var info in _settings.AxisGrabInfoList)
            {
                axisPosition[info.AxisIndex] = info.NextX;
            }

            axisPosition[_settings.AxisIndex] = NextY;
            
            axisHandler.StartMultipleMove(axisPosition, _settings.MoveVelocity);
            
            return await MotionService.WaitMoveDone(_token);
        }

        public static async Task<bool> MoveNextPos(AxisGrabInfo info)
        {
            var axisHandler = _settings.AxisHandler;
            var axisPosition = axisHandler.GetActualPos();
            axisHandler.SetPosition(axisPosition);
            
            axisPosition[info.AxisIndex] = info.NextX;
            axisPosition[_settings.AxisIndex] = NextY;

            axisHandler.StartMultipleMove(axisPosition, _settings.MoveVelocity);

            return await MotionService.WaitMoveDone(_token);
        }

        public static async Task<bool> GrabNextPos()
        {
            var axisHandler = _settings.AxisHandler;
            var axisPosition = axisHandler.GetActualPos();
            axisHandler.SetPosition(axisPosition);

            int[] valueArray = new int[SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLight];

            foreach (var info in _settings.AxisGrabInfoList)
            {
                _positionDictionary[info.ImageDevice] = axisPosition[_settings.AxisIndex];
                foreach (var lightIndex in info.LightIndexList)
                    valueArray[lightIndex.Value] = Settings.LineCameraLightValue;
            }

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(new LightValue(valueArray));

            axisPosition[_settings.AxisIndex] -= _grabOffset;

            axisHandler.StartMultipleMove(axisPosition, _settings.GrabVelocity);
            if (await MotionService.WaitMoveDone(_token) == false)
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                return false;
            }
            
            foreach (var info in _settings.AxisGrabInfoList)
            {
                triggerResetEventDictionary[info.ImageDevice].Set();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, true);

                axisPosition[info.AxisIndex] = info.NextX;
            }

            axisHandler.StartCmp("Y", (int)(axisPosition[_settings.AxisIndex] + _grabOffset), (float)_settings.Resolution * 2, true);

            axisPosition[_settings.AxisIndex] = NextY;

            axisHandler.StartMultipleMove(axisPosition, _settings.GrabVelocity);
            var result = await MotionService.WaitMoveDone(_token);

            foreach (var info in _settings.AxisGrabInfoList)
            {
                triggerResetEventDictionary[info.ImageDevice].Reset();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, false);
            }
            
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            if (result == false)
                return false;

            while (imageDevicePtrQueue.Values.Any(value => value != null && value.Count > 0))
            {
                await Task.Delay(100);

                if (_token.IsCancellationRequested)
                    return false;
            }

            return true;
        }

        public static async Task<bool> GrabNextPos(AxisGrabInfo info)
        {
            var axisHandler = _settings.AxisHandler;
            var axisPosition = axisHandler.GetActualPos();
            axisHandler.SetPosition(axisPosition);

            // Light
            int[] valueArray = new int[SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLight];
            foreach (var lightIndex in info.LightIndexList)
                valueArray[lightIndex.Value] = Settings.LineCameraLightValue;

            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, true);
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(new LightValue(valueArray));

            _positionDictionary[info.ImageDevice] = (int)axisPosition[_settings.AxisIndex];

            axisPosition[_settings.AxisIndex] -= _grabOffset;

            axisHandler.StartMultipleMove(axisPosition, _settings.GrabVelocity);
            if (await MotionService.WaitMoveDone(_token) == false)
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, false);

                return false;
            }

            triggerResetEventDictionary[info.ImageDevice].Set();

            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, true);

            axisPosition[info.AxisIndex] = info.NextX;

            axisHandler.StartCmp("Y", (int)(axisPosition[_settings.AxisIndex] + _grabOffset), (float)_settings.Resolution * 2, true);

            axisPosition[_settings.AxisIndex] = NextY;

            axisHandler.StartMultipleMove(axisPosition, _settings.GrabVelocity);
            var result = await MotionService.WaitMoveDone(_token);

            triggerResetEventDictionary[info.ImageDevice].Reset();
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, false);
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(info.TriggerPort, false);

            if (result == false)
                return false;

            while (imageDevicePtrQueue[info.ImageDevice].Count > 0)
            {
                await Task.Delay(100);

                if (_token.IsCancellationRequested)
                    return false;
            }

            return true;
        }

        public static async Task SaveAxisGrabInfos()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync(_key, _settings, new ImageDeviceJsonConverter(), new AxisHandlerJsonConverter(), new IoPortJsonConverter());
        }

        public static async Task LoadAxisGrabInfos()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            _settings = await directoryInfo.ReadAsync<ServiceSettings>(_key, new ImageDeviceJsonConverter(), new AxisHandlerJsonConverter(), new IoPortJsonConverter()) ?? new ServiceSettings();
        }

        public static bool CheckDoorLock()
        {
            var doorPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutDoor);
            var inDoorPort1 = SystemManager.Instance().DeviceBox.PortMap.GetInPort(InPortName.InDoor1);
            var inDoorPort2 = SystemManager.Instance().DeviceBox.PortMap.GetInPort(InPortName.InDoor2);

            if (SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(inDoorPort1)
                || SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(inDoorPort2)
                || SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(doorPort) == false)
            {
                return false;
            }

            return true;
        }

    }
}

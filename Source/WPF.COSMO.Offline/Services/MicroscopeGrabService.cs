using DynMvp.Device;
using DynMvp.Device.AutoFocus;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.Base.Converters;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Services
{
    public class MicroscopeServiceSettings
    {
        public AxisHandler AxisHandler { get; set; }

        public double FocusValue { get; set; }
        public int LightValue { get; set; } = 200;
        public int MoveVelocity { get; set; } = 100000;
        public double Exposure { get; set; } = 3000;

        public int XAxisIndex { get; set; } = 1;
        public int YAxisIndex { get; set; } = 2;
        public int LightIndex { get; set; } = 5;

        public int OffsetX { get; set; } = -867820;
        public int OffsetY { get; set; } = -88497;
        
        public MicroscopeServiceSettings()
        {
            AxisHandler = Base.Services.MotionService.Handlers.First();
        }
    }

    public enum MicroscopeState
    {
        Live, Once, AutoFocus, Idle
    }

    public static class MicroscopeGrabService
    {
        public static ImageDeviceEventDelegate MicroscopeGrabbed;

        const string _key = "MicroscopeGrab";

        private static CameraPylon microscopeCamera;

        private static MicroscopeServiceSettings settings;
        public static MicroscopeServiceSettings Settings => settings;

        private static MicroscopeState state = MicroscopeState.Idle;
        public static MicroscopeState State => state;

        //------------ Auto Focus
        static AutoFocus AutoFocus { get; set; }
        static OptoTune OptoTune { get; set; }
        static OptoTuneFocusDriver OptoTuneFocusDriver { get; set; }

        public static bool? IsConnected => OptoTune?.IsConnected();

        public static async Task<bool> InitializeOptotune()
        {
            return await Task.Run(() =>
            {
                var deviceList = SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList;
                microscopeCamera = deviceList.Find(x => x is CameraPylon) as CameraPylon;
                if (microscopeCamera == null)
                    return false;

                if (ConnectOptoTune() == false)
                    return false;
                
                var size = microscopeCamera.ImageSize;
                var rect = new System.Drawing.Rectangle(size.Width / 8 * 3, size.Height / 8 * 3, size.Width / 4, size.Height / 4);

                AutoFocus.SetFocusRegion(rect);

                return true;
            });
        }

        public static void InitializeCameara()
        {
            state = MicroscopeState.Idle;

            SetExposure(settings.Exposure);
            TurnOnLight(settings.LightValue);

            microscopeCamera.ImageGrabbed += ImageGrabbed;

            microscopeCamera.Reverse(true, true);
            microscopeCamera.SetTriggerMode(TriggerMode.Software);
            microscopeCamera.GrabMulti(-1);
        }

        public static async Task ReleaseCameara()
        {
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            microscopeCamera.ImageGrabbed -= ImageGrabbed;
            microscopeCamera.Stop();

            await SaveMicroscopeServiceSettings();
        }

        public static void ReleaseOptotune()
        {
            DisconnectOptoTune();
        }


        private static bool ConnectOptoTune()
        {
            // AutoFocus
            OptoTune = new OptoTuneLAN();

            if (OptoTune.Connect())
            {
                OptoTuneFocusDriver = new OptoTuneFocusDriver();
                OptoTuneFocusDriver.Initialize(OptoTune, new OptoTuneFocusDriverSetting()
                {
                    MinFocusCurrent = OptoTune.GetMinCurrent(),
                    MaxFocusCurrent = OptoTune.GetMaxCurrent(),
                    CoarseCurrentStep = 50,
                    MiddleCurrentStep = 15,
                    FineCurrentStep = 1
                });

                AutoFocus = new AutoFocus();
                AutoFocus.Initialize(OptoTuneFocusDriver, new AutoFocusSetting()
                {
                    OptimizeMethod = AutoFocusSetting.OptimizationMethod.CoarseToFine,
                    FindMethod = AutoFocusSetting.CalculateMethod.Variance,
                });

                return true;
            }

            return false;
        }

        private static void DisconnectOptoTune()
        {
            // AutoFocus
            if (OptoTune != null)
                OptoTune.Disconnect();
        }

        public static void TurnOnLight(int lightValue)
        {
            int[] valueArray = new int[SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLight];
            
            valueArray[settings.LightIndex] = lightValue;

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(new LightValue(valueArray));
        }

        public static void SetExposure(double exposure)
        {
            Settings.Exposure = exposure;
            microscopeCamera?.SetExposureTime(Convert.ToSingle(exposure));
        }
        
        public static void Grab()
        {
            if (state != MicroscopeState.Idle)
                return;

            state = MicroscopeState.Once;

            microscopeCamera.SetTriggerMode(true);
            microscopeCamera.ExcuteTriggerSoftware();
        }

        public static void LiveGrab()
        {
            if (state != MicroscopeState.Idle)
                return;

            state = MicroscopeState.Live;
            microscopeCamera.SetTriggerMode(false);
        }

        public static void AutoFocusGrab()
        {
            if (state == MicroscopeState.Once || state == MicroscopeState.Live)
                return;

            if (state == MicroscopeState.Idle)
            {
                state = MicroscopeState.AutoFocus;
                microscopeCamera.SetTriggerMode(true);

                AutoFocus.SetReady();
            }

            microscopeCamera.ExcuteTriggerSoftware();
        }

        public static void StopGrab()
        {
            if (state == MicroscopeState.Idle)
                return;

            microscopeCamera.SetTriggerMode(true);
            state = MicroscopeState.Idle;
        }

        private static void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (state == MicroscopeState.Once)
                state = MicroscopeState.Idle;
            
            MicroscopeGrabbed?.Invoke(imageDevice, ptr);
        }

        public static void StepMoveX(float distanceUM)
        {
            Settings.AxisHandler[Settings.XAxisIndex].RelativeMove(distanceUM, new MovingParam() { MaxVelocity = settings.MoveVelocity * 1000 });
        }

        public static void StepMoveY(float distanceUM)
        {
            Settings.AxisHandler[Settings.YAxisIndex].RelativeMove(distanceUM, new MovingParam() { MaxVelocity = settings.MoveVelocity * 1000 });
        }

        public static void JogMoveX(bool isNagativeMove = false)
        {
            Settings.AxisHandler[Settings.XAxisIndex].ContinuousMove(new MovingParam() { MaxVelocity = settings.MoveVelocity * 1000 }, isNagativeMove);
        }
        
        public static void JogMoveY(bool isNagativeMove = false)
        {
            Settings.AxisHandler[Settings.YAxisIndex].ContinuousMove(new MovingParam() { MaxVelocity = settings.MoveVelocity * 1000 }, isNagativeMove);
        }

        public static void StopMove()
        {
            Settings.AxisHandler.StopMove();
        }

        public static bool CalculateAutoFocus(Bitmap bitmap)
        {
            if (OptoTune != null && OptoTune.IsConnected())
            {
                bool result = AutoFocus.Action(bitmap);

                settings.FocusValue = OptoTune.GetCurrent();

                if (result)
                {
                    SetFocusValue(settings.FocusValue);
                    state = MicroscopeState.Idle;
                }

                return result;
            }

            return true;
        }

        public static void SetFocusValue(double value)
        {
            if (OptoTune != null && OptoTune.IsConnected())
                OptoTune.SetCurrent(value);
        }

        public static double GetFocusValue()
        {
            if (OptoTune != null && OptoTune.IsConnected())
                return OptoTune.GetCurrent();

            return 0;
        }

        public static double GetMinCurrent()
        {
            if (OptoTune != null && OptoTune.IsConnected())
            {
                return OptoTune.GetMinCurrent();
            }

            return 0;
        }

        public static double GetMaxCurrent()
        {
            if (OptoTune != null && OptoTune.IsConnected())
            {
                return OptoTune.GetMaxCurrent();
            }
                

            return 0;
        }

        public static async Task SaveMicroscopeServiceSettings()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync(_key, settings, new ImageDeviceJsonConverter(), new AxisHandlerJsonConverter(), new IoPortJsonConverter());
        }

        public static async Task LoadMicroscopeServiceSettings()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            settings = await directoryInfo.ReadAsync<MicroscopeServiceSettings>(_key, new ImageDeviceJsonConverter(), new AxisHandlerJsonConverter(), new IoPortJsonConverter()) ?? new MicroscopeServiceSettings();
        }

        public static async Task<bool> MoveDefectPos(Defect defect)
        {
            var info = AxisGrabService.Settings.AxisGrabInfoList.First(i => i.AxisIndex == MicroscopeGrabService.Settings.XAxisIndex);
            info.NextX = (int)defect.CenterPt.X + MicroscopeGrabService.Settings.OffsetX;
            AxisGrabService.NextY = (int)defect.CenterPt.Y + MicroscopeGrabService.Settings.OffsetY;

            if (await AxisGrabService.MoveNextPos(info) == false)
                return false;

            return true;
        }
    }
}

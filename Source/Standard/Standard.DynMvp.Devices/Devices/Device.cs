using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Standard.DynMvp.Base;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Base.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Standard.DynMvp.Devices.ImageDevices;
using Standard.DynMvp.Devices.MotionController;
using Standard.DynMvp.Devices.LightController;
using Standard.DynMvp.Devices.ImageDevices.Virtual;
using Standard.DynMvp.Devices.ImageDevices.MultiCam;
using Standard.DynMvp.Devices.ImageDevices.GenTL;

namespace Standard.DynMvp.Devices
{
    public enum DeviceType
    {
        FrameGrabber, MotionController, DigitalIo, LightController, DaqChannel, Camera, DepthScanner, BarcodeReader, BarcodePrinter, Serial
    }

    public enum DeviceState
    {
        Idle, Ready, Warning, Error
    }

    public class DeivceInfoConverter : JsonCreationConverter<DeviceInfo>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public class CameraInfoConverter : JsonCreationConverter<CameraInfo>
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }

            protected override CameraInfo Create(Type objectType, JObject jObject)
            {
                GrabberType grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), jObject["GrabberType"].ToString());
                var name = jObject["Name"].ToString();

                switch (grabberType)
                {
                    case GrabberType.Virtual:
                        return new CameraInfoVirtual(name);
                    case GrabberType.MultiCam:
                        return new CameraInfoMultiCam(name);
                    case GrabberType.GenTL:
                        return new CameraInfoGenTL(name);
                }

                return null;
            }
        }

        protected override DeviceInfo Create(Type objectType, JObject jObject)
        {
            DeviceType deviceType = (DeviceType)Enum.Parse(typeof(DeviceType), jObject["DeviceType"].ToString());
            var name = jObject["Name"].ToString();

            switch (deviceType)
            {
                case DeviceType.FrameGrabber:
                    GrabberType grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), jObject["GrabberType"].ToString());
                    return new GrabberInfo(name, grabberType, 0);
                case DeviceType.MotionController:
                    MotionType motionType = (MotionType)Enum.Parse(typeof(MotionType), jObject["MotionType"].ToString());
                    return new MotionInfo(name, motionType);
                case DeviceType.DigitalIo:
                    break;
                case DeviceType.LightController:
                    LightControllerType lightControllerType = (LightControllerType)Enum.Parse(typeof(LightControllerType), jObject["LightControllerType"].ToString());
                    return new LightControllerInfo(name, lightControllerType);
                case DeviceType.DaqChannel:
                    break;
                case DeviceType.Camera:
                    grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), jObject["GrabberType"].ToString());
                    switch (grabberType)
                    {
                        case GrabberType.MultiCam:
                            return new CameraInfoMultiCam(name);
                        case GrabberType.GenTL:
                            return new CameraInfoGenTL(name);
                    }
                    return new CameraInfoVirtual(name);
                case DeviceType.DepthScanner:
                    break;
                case DeviceType.BarcodeReader:
                    break;
                case DeviceType.BarcodePrinter:
                    break;
                case DeviceType.Serial:
                    break;
            }

            return null;
        }
    }

    public abstract class DeviceInfo
    {
        DeviceType _deviceType;
        string _name;

        public DeviceType DeviceType { get => _deviceType; }
        public string Name { get => _name; set => _name = value; }

        public static ICommand DefaultAddCommand;
        public static ICommand DefaultRemoveCommand;

        public ICommand AddCommand { get => DefaultAddCommand; }
        public ICommand RemoveCommand { get => DefaultRemoveCommand; }

        protected ObservableCollection<SettingData> _settings;

        [JsonIgnore]
        public ObservableCollection<SettingData> Settings { get => _settings; }

        protected DeviceInfo(DeviceType deviceType, string name)
        {
            _name = name;
            _deviceType = deviceType;
            _settings = SettingDataAttribute.GetProperties(this);
        }
    }

    public abstract class Device : IDisposable
    {
        bool disposedValue = false;

        DeviceInfo _deviceInfo;
        public DeviceInfo DeviceInfo { get => _deviceInfo; }

        DeviceState _deviceState;
        public DeviceState DeviceState { get => _deviceState; }

        protected abstract void Release();

        protected Device(DeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
            _deviceState = DeviceState.Idle;
        }

        public void UpdateState(DeviceState deviceState)
        {
            _deviceState = deviceState;
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    Release();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~Device() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            Dispose(true);
             GC.SuppressFinalize(this);
        }
    }
}

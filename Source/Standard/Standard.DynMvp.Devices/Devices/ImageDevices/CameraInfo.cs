using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices.ImageDevices;
using Standard.DynMvp.Devices.ImageDevices.GenTL;
using Standard.DynMvp.Devices.ImageDevices.MultiCam;
using Standard.DynMvp.Devices.ImageDevices.Virtual;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Standard.DynMvp.Devices.ImageDevices
{
    public abstract class CameraInfo : DeviceInfo
    {
        GrabberType _grabberType;

        [SettingData(SettingDataType.Numeric)]
        public uint ImageWidth { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint ImageHeight { get; set; }

        [SettingData(SettingDataType.Enum)]
        public RotateFlipType RotateFlipType { get; set; }

        [SettingData(SettingDataType.Enum)]
        public TriggerType TriggerType { get; set; }

        [SettingData(SettingDataType.Enum)]
        public TriggerMode TriggerMode { get; set; }
        public GrabberType GrabberType { get => _grabberType; }

        public CameraInfo(string name, GrabberType grabberType) : base(DeviceType.Camera, name)
        {
            _grabberType = grabberType;
        }

        public static CameraInfo CreateInstance(string name, GrabberType grabberType)
        {
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
}

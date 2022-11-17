using Newtonsoft.Json;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices.Helpers;
using Standard.DynMvp.Devices.ImageDevices.GenTL;
using Standard.DynMvp.Devices.ImageDevices.MultiCam;
using Standard.DynMvp.Devices.ImageDevices.Virtual;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Standard.DynMvp.Devices.ImageDevices
{
    public enum GrabberType
    {
        Virtual, MultiCam, GenTL
    }

    public class GrabberInfo : DeviceInfo
    {
        GrabberType _grabberType;
        ObservableCollection<CameraInfo> _cameraInfos = new ObservableCollection<CameraInfo>();
        public GrabberType GrabberType { get => _grabberType; }

        [DeviceData]
        public ObservableCollection<CameraInfo> CameraInfos { get => _cameraInfos; }

        [JsonIgnore]
        public IEnumerable<SettingData> CameraSettings { get => SettingDataAttribute.GetProperties(this); }
        
        public GrabberInfo(string name, GrabberType grabberType, int numCamera = 1) : base(DeviceType.FrameGrabber, name)
        {
            _grabberType = grabberType;

            for (int index = 0; index < numCamera; index++)
            {
                _cameraInfos.Add(CreateCameraInfo(grabberType, index));
            }
        }

        public static CameraInfo CreateCameraInfo(GrabberType grabberType, int index)
        {
            switch (grabberType)
            {
                case GrabberType.MultiCam:
                    return new CameraInfoMultiCam(CreateName(grabberType, index));
                case GrabberType.GenTL:
                    return new CameraInfoGenTL(CreateName(grabberType, index));
            }

            return new CameraInfoVirtual(CreateName(grabberType, index));
        }

        private static string CreateName(GrabberType grabberType, int index)
        {
            return string.Format("{0}-{1}", grabberType, index);
        }

        public void CreateCamera()
        {
            _cameraInfos.Add(CreateCameraInfo(_grabberType, _cameraInfos.Count));
        }
    }
}

using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.DynMvp.Devices.ImageDevices.MultiCam
{
    public class CameraInfoMultiCam : CameraInfo
    {
        [SettingData(SettingDataType.Enum)]
        public EuresysBoardType BoardType { get; set; }

        [SettingData(SettingDataType.Enum)]
        public EuresysImagingType ImagingType { get; set; }

        [SettingData(SettingDataType.Enum)]
        public EuresysCameraType CameraType { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint BoardId { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint ConnectorId { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint SurfaceNum { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint PageLength { get; set; }

        public CameraInfoMultiCam(string name) : base(name, GrabberType.MultiCam)
        {

        }

        public CameraInfoMultiCam(string name, GrabberType grabberType, EuresysBoardType boardType, uint boardId, uint connectorId, EuresysCameraType cameraType, uint surfaceNum, uint pageLength) : base(name, grabberType)
        {
            BoardType = boardType;
            BoardId = boardId;
            ConnectorId = connectorId;
            CameraType = cameraType;
            SurfaceNum = surfaceNum;
            PageLength = pageLength;
        }
    }
}

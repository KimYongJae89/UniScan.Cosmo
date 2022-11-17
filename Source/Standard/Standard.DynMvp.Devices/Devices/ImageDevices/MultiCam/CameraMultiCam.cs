using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using Standard.DynMvp.Base;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Euresys.MultiCam;
using Euresys;

namespace Standard.DynMvp.Devices.ImageDevices.MultiCam
{
    public enum EuresysBoardType
    {
        GrabLink_Value, GrabLink_Base, GrabLink_DualBase, GrabLink_Full, Picolo
    }

    public enum EuresysImagingType
    {
        AREA, LINE //LINE 확인 O, AREA는 확인 X
    }

    public enum EuresysCameraType
    {
        RaL12288_66km
    }

    class McSurface
    {
        uint handle;
        byte[] surfaceData;
        GCHandle pinnedArray;
        IntPtr dataPtr;

        public void Create(int width, int height, int pitch)
        {
            MC.Create(MC.DEFAULT_SURFACE_HANDLE, out handle);
            MC.SetParam(handle, "SurfaceSize", width*height);
            MC.SetParam(handle, "SurfacePitch", pitch);

            surfaceData = new byte[pitch * height];
            GCHandle pinnedArray = GCHandle.Alloc(surfaceData, GCHandleType.Pinned);
            dataPtr = pinnedArray.AddrOfPinnedObject();

            MC.SetParam(handle, "SurfaceAddr", dataPtr);
        }

        public void Release()
        {
            pinnedArray.Free();
        }
    }

    public class CameraMultiCam : Camera
    {
        UInt32 channel;

        public CameraMultiCam(CameraInfo cameraInfo) : base(cameraInfo)
        {

        }

        protected override void Initialize()
        {
            CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)DeviceInfo;
        
            SetBoardTopology();

            MC.Create("CHANNEL", out channel);
            MC.SetParam(channel, "DriverIndex", cameraInfoMultiCam.BoardId);
            string connectorString = GetConnectorString();

            MC.SetParam(channel, "Connector", connectorString);

            string camFile = GetCamFile();
            MC.SetParam(channel, "CamFile", camFile);

            InitializeDevice();

            var multiCamCallback = new MC.CALLBACK(MultiCamCallback);

            MC.RegisterCallback(channel, multiCamCallback, channel);

            MC.SetParam(channel, MC.SignalEnable + MC.SIG_END_EXPOSURE, "ON");
            MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_FILLED, "ON");
            MC.SetParam(channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");

            MC.SetParam(channel, "SurfaceCount", cameraInfoMultiCam.SurfaceNum);

            MC.GetParam(channel, "ImageSizeX", out Int32 width);
            MC.GetParam(channel, "ImageSizeY", out Int32 height);
            MC.GetParam(channel, "BufferPitch", out Int32 bufferPitch);
            MC.GetParam(channel, "ImagePlaneCount", out Int32 planeCount);
                
            MC.SetParam(channel, "ChannelState", "ACTIVE");
            MC.SetParam(channel, "ChannelState", "IDLE");
        }

        private void InitializeDevice()
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;

            switch (cameraInfoMultiCam.ImagingType)
            {
                case EuresysImagingType.AREA:
                    MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                    MC.SetParam(channel, "NextTrigMode", "REPEAT");
                    MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                    break;
                case EuresysImagingType.LINE:
                    MC.SetParam(channel, "AcquisitionMode", "LONGPAGE");
                    MC.SetParam(channel, "TrigMode", "HARD");

                    MC.SetParam(channel, "LineCaptureMode", "ALL");
                    MC.SetParam(channel, "LineTrigCtl", "DIFF");
                    MC.SetParam(channel, "LineTrigEdge", "RISING_A");
                    MC.SetParam(channel, "LineTrigFilter", "OFF");
                    MC.SetParam(channel, "LineTrigLine", "DIN1");

                    MC.SetParam(channel, "RateDivisionFactor", "1");
                    MC.SetParam(channel, "LineRateMode", "PULSE");
                    MC.SetParam(channel, "PageLength_Ln", cameraInfoMultiCam.PageLength);
                    MC.SetParam(channel, "SeqLength_Ln", MC.INDETERMINATE);

                    MC.SetParam(channel, "EndTrigMode", "HARD");
                    MC.SetParam(channel, "EndTrigCtl", "ISO");
                    MC.SetParam(channel, "EndTrigEdge", "GOLOW");
                    MC.SetParam(channel, "EndTrigEffect", "PRECEDINGLINE");
                    MC.SetParam(channel, "EndTrigFilter", "OFF");
                    MC.SetParam(channel, "EndTrigLine", "IIN1");
                    MC.SetParam(channel, "EndTrigFollowingLinesCount", "ONELINE");

                    MC.SetParam(channel, "TrigCtl", "ISO");
                    MC.SetParam(channel, "TrigDelay_Pls", "0");
                    MC.SetParam(channel, "TrigEdge", "GOHIGH");
                    MC.SetParam(channel, "TrigFilter", "OFF");
                    MC.SetParam(channel, "TrigLine", "IIN1");

                    MC.SetParam(channel, "EndPageDelay_Ln", "0");
                    MC.SetParam(channel, "PageDelay_Ln", "0");

                    break;
            }
        }

        private void SetBoardTopology()
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.Picolo)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "1_01_2");
            }
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.GrabLink_Full)
            {
                MC.SetParam(MC.BOARD + cameraInfoMultiCam.BoardId, "BoardTopology", "MONO_DECA");
            }
        }

        private string GetCamFile()
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;
            if (cameraInfoMultiCam.BoardType == EuresysBoardType.Picolo)
            {
                return "NTSC";
            }
            else
            {
                switch (cameraInfoMultiCam.CameraType)
                {
                    case EuresysCameraType.RaL12288_66km:
                        return "raL12288-66km_L12288RG";
                }
            }

            return "";
        }

        private string GetConnectorString()
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;
            switch (cameraInfoMultiCam.BoardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_Full:
                    return "M";
                case EuresysBoardType.GrabLink_DualBase:
                    if (cameraInfoMultiCam.ConnectorId == 0)
                        return "A";
                    else
                        return "B";
                case EuresysBoardType.Picolo:
                    return String.Format("VID{0}", cameraInfoMultiCam.ConnectorId + 1);
            }

            return "";
        }

        private string GetTriggerLineName(int triggerChannel)
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;
            switch (cameraInfoMultiCam.BoardType)
            {
                case EuresysBoardType.GrabLink_Base:
                case EuresysBoardType.GrabLink_DualBase:
                case EuresysBoardType.GrabLink_Full:
                    string[] grabLinkTriggerNameList = new string[] { "NOM", "DIN1", "DIN2", "IIN1", "IIN2", "IIN3", "IIN4" };
                    return grabLinkTriggerNameList[triggerChannel];
            }

            return "NOM";
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            var cameraInfoMultiCam = DeviceInfo as CameraInfoMultiCam;
            try
            {
                if (triggerMode == TriggerMode.Software)
                {
                    // Choose the way the first acquisition is triggered
                    switch (cameraInfoMultiCam.ImagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "IMMEDIATE");
                            MC.SetParam(channel, "ChannelState", "IDLE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "ChannelState", "IDLE");
                            break;
                    }
                }
                else
                {
                    switch (cameraInfoMultiCam.ImagingType)
                    {
                        case EuresysImagingType.AREA:
                            MC.SetParam(channel, "TrigMode", "HARD");

                            //MC.SetParam(channel, "TrigLine", GetTriggerLineName(triggerChannel));        // Norminal

                            if (triggerType == TriggerType.FallingEdge)
                                MC.SetParam(channel, "TrigEdge", "GOLOW");
                            else
                                MC.SetParam(channel, "TrigEdge", "GOHIGH");

                            MC.SetParam(channel, "TrigFilter", "ON");

                            MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);

                            // Parameter valid only for Grablink Full, DualBase, Base
                            MC.SetParam(channel, "TrigCtl", "ISO");
                            //MC.SetParam(channel, "ChannelState", "ACTIVE");
                            break;
                        case EuresysImagingType.LINE:
                            MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);

                            MC.SetParam(channel, "TrigMode", "HARD");
                            MC.SetParam(channel, "TrigLine", "IIN1");    
                            MC.SetParam(channel, "TrigEdge", "GOHIGH");
                            MC.SetParam(channel, "TrigFilter", "ON");
                            MC.SetParam(channel, "TrigCtl", "ISO");

                            MC.SetParam(channel, "EndTrigMode", "HARD");
                            MC.SetParam(channel, "EndTrigCtl", "ISO");
                            MC.SetParam(channel, "EndTrigEdge", "GOLOW");
                            MC.SetParam(channel, "EndTrigFilter", "ON");
                            MC.SetParam(channel, "EndTrigLine", "IIN1");
                            MC.SetParam(channel, "ImageFlipX", "ON");

                            MC.SetParam(channel, "AcqTimeout_ms", "-1");

                            //MC.SetParam(channel, "ChannelState", "ACTIVE");
                            break;
                    }

                }
            }
            catch (MultiCamException e)
            {
                //LogHelper.Error("MultiCam Exception : " + exc.Message);
            }
        }

        protected override void Release()
        {
            MC.Delete(channel);
        }
        
        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabSingle");

            try
            {
                SetupGrab(1);

                MC.SetParam(channel, "ChannelState", "ACTIVE");

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (MultiCamException exc)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + exc.Message);
            }
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - GrabContinuous");

            try
            {
                SetupGrab(grabCount);

                if (grabCount == CONTINUOUS)
                    MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                else
                    MC.SetParam(channel, "SeqLength_Fr", grabCount);

                MC.SetParam(channel, "ChannelState", "ACTIVE");

                LogHelper.Debug(LoggerType.Grab, "CameraMulticam - Channel Activated");
            }
            catch (MultiCamException exc)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + exc.Message);
            }
        }

        private void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case MC.SIG_END_EXPOSURE:
                    ProcessingEndExposureCallback();
                    break;
                case MC.SIG_SURFACE_FILLED:
                    ProcessingSurfaceFilledCallback(signalInfo);
                    break;
                
                case MC.SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new MultiCamException("Unknown signal");
            }
        }

        private void ProcessingStartExposureCallback()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingStartExposureCallback");
        }

        private void ProcessingEndExposureCallback()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingEndExposureCallback");
        }

        private void ProcessingSurfaceFilledCallback(MC.SIGNALINFO signalInfo)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMulticam - ProcessingSurfaceFilledCallback");
            
            CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)DeviceInfo;

            UInt32 currentChannel = (UInt32)signalInfo.Context;
            
            try
            {
                MC.GetParam(signalInfo.SignalInfo, "SurfaceAddr", out IntPtr bufferAddress);
                ImageGrabbDone(bufferAddress);
            }
            catch (MultiCamException e)
            {
                LogHelper.Error(LoggerType.Error, "MultiCam Exception : " + e.Message);
            }
        }

        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            LogHelper.Error(LoggerType.Error, "Acquisition Failure, Channel State: IDLE");
        }
        
        public override void Stop()
        {
            MC.SetParam(channel, "ChannelState", "IDLE");
        }
    }
}

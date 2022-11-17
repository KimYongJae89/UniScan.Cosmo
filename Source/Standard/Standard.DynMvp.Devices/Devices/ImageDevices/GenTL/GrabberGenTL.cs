using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

using Standard.DynMvp.Base;

namespace Standard.DynMvp.Devices.ImageDevices.GenTL
{
    public class CameraInfoGenTL : CameraInfo
    {
        public enum EClientType { Master, Slave }
        public enum EScanDirectionType { Forward, Reverse }
        public enum EAnalogGain { X1, X2, X3, X4 }
        public enum ELineInputToolSource { DIN11, TTLIO11 }
        uint frameNum;

        [CategoryAttribute("CameraInfoGenTL"), DescriptionAttribute("Frame Buffer Count")]
        public uint FrameNum
        {
            get { return frameNum; }
            set { frameNum = value; }
        }

        uint scanLength;
        [Category("CameraInfoGenTL"), Description("Image Length")]
        public uint ScanLength
        {
            get { return scanLength; }
            set { scanLength = value; }
        }

        bool useMilBuffer;
        [Category("CameraInfoGenTL"), Description("Alloc in MIL Non-paged Area")]
        public bool UseMilBuffer
        {
            get { return useMilBuffer; }
            set { useMilBuffer = value; }
        }

        EClientType clientType;
        [Category("CameraInfoGenTL"), Description("DF Client Type")]
        public EClientType ClientType
        {
            get { return clientType; }
            set { clientType = value; }
        }

        EScanDirectionType directionType;
        [Category("CameraInfoGenTL"), Description("Camera Scan Direction")]
        public EScanDirectionType DirectionType
        {
            get { return directionType; }
            set { directionType = value; }
        }

        uint offsetX = 0;
        [Category("CameraInfoGenTL"), Description("Image Offset X > 0")]
        public uint OffsetX
        {
            get { return offsetX; }
            set { offsetX = value; }
        }

        float digitalGain = 1.0f;
        [Category("CameraInfoGenTL"), Description("Degital Gain")]
        public float DigitalGain
        {
            get { return digitalGain; }
            set { digitalGain = value; }
        }

        EAnalogGain analogGain = EAnalogGain.X1;
        [Category("CameraInfoGenTL"), Description("Analog Gain")]
        public EAnalogGain AnalogGain
        {
            get { return analogGain; }
            set { analogGain = value; }
        }

        bool binningVertical = false;
        [Category("CameraInfoGenTL"), Description("Binning Vertical")]
        public bool BinningVertical
        {
            get { return binningVertical; }
            set { binningVertical = value; }
        }

        float triggerRescalerRate = 4.0f;
        [Category("CameraInfoGenTL"), Description("TriggerRescalerRate")]
        public float TriggerRescalerRate
        {
            get { return triggerRescalerRate; }
            set { triggerRescalerRate = value; }
        }

        bool triggerRescalerMode = true;
        [Category("CameraInfoGenTL"), Description("Trigger Rescaler Mode")]
        public bool TriggerRescalerMode
        {
            get { return triggerRescalerMode; }
            set { triggerRescalerMode = value; }
        }

        ELineInputToolSource lineInputToolSource = ELineInputToolSource.DIN11;
        [Category("CameraInfoGenTL"), Description("LineInput Tool Source")]
        public ELineInputToolSource LineInputToolSource
        {
            get { return lineInputToolSource; }
            set { lineInputToolSource = value; }
        }


        public CameraInfoGenTL(string name) : base(name, GrabberType.GenTL)
        {

        }
    }

    public class GrabberGenTL : Grabber
    {
        public GrabberGenTL(GrabberInfo grabberInfo) : base(grabberInfo)
        {

        }

        protected override void Release()
        {

        }

        protected override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraGenTL(cameraInfo);
        }

        protected override bool Initialize()
        {
            return true;
        }
    }
}

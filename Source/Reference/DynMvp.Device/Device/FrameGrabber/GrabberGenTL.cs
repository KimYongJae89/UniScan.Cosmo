using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;

namespace DynMvp.Devices.FrameGrabber
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


        public CameraInfoGenTL()
        {
            GrabberType = GrabberType.GenTL;
            this.UseNativeBuffering = true;
        }

        public CameraInfoGenTL(int width, int height , uint scanLength, uint frameNum, EClientType clientType, EScanDirectionType directionType, bool useMilBuffer=false)
        {
            this.GrabberType = GrabberType.GenTL;

            this.Width = width;
            this.Height = height;
            this.frameNum = frameNum;
            this.scanLength = scanLength;
            this.clientType = clientType;
            this.useMilBuffer = useMilBuffer;
            this.directionType = directionType;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            frameNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "FrameNum", "1"));
            scanLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ScanLength", "0"));
            offsetX = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "OffsetX", "0"));
            binningVertical = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "BinningVertical", "false"));
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "ClientType", EClientType.Master.ToString()), out clientType);
            useMilBuffer = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "UseMilBuffer", "false"));
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "DirectionType", EScanDirectionType.Forward.ToString()), out directionType);

            Enum.TryParse(XmlHelper.GetValue(cameraElement, "AnalogGain", this.analogGain.ToString()), out this.analogGain);
            float.TryParse(XmlHelper.GetValue(cameraElement, "DigitalGain", this.digitalGain.ToString()), out this.digitalGain);
            float.TryParse(XmlHelper.GetValue(cameraElement, "TriggerRescalerRate", this.triggerRescalerRate.ToString()), out this.triggerRescalerRate);
            //triggerRescalerRate = XmlHelper.GetValue(cameraElement, "", triggerRescalerRate);


            triggerRescalerMode = XmlHelper.GetValue(cameraElement, "TriggerRescalerMode", triggerRescalerMode);
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "LineInputToolSource", ELineInputToolSource.DIN11.ToString()), out this.lineInputToolSource);
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "FrameNum", frameNum.ToString());
            XmlHelper.SetValue(cameraElement, "ScanLength", scanLength.ToString());
            XmlHelper.SetValue(cameraElement, "OffsetX", offsetX.ToString());
            XmlHelper.SetValue(cameraElement, "BinningVertical", binningVertical.ToString());
            XmlHelper.SetValue(cameraElement, "ClientType", clientType.ToString());
            XmlHelper.SetValue(cameraElement, "UseMilBuffer", useMilBuffer.ToString());
            XmlHelper.SetValue(cameraElement, "DirectionType", directionType.ToString());
            XmlHelper.SetValue(cameraElement, "DigitalGain", digitalGain.ToString());
            XmlHelper.SetValue(cameraElement, "AnalogGain", analogGain.ToString());
            XmlHelper.SetValue(cameraElement, "TriggerRescalerRate", triggerRescalerRate.ToString());
            XmlHelper.SetValue(cameraElement, "TriggerRescalerMode", triggerRescalerMode.ToString());
            XmlHelper.SetValue(cameraElement, "LineInputToolSource", lineInputToolSource.ToString());
        }
    }

    public class GrabberGenTL : Grabber
    {
        static int cntOpenDriver = 0;

        public GrabberGenTL(string name) : base(GrabberType.GenTL, name)
        {
      
        }

        public override Camera CreateCamera()
        {
            return new CameraGenTL();
           
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            GenTLCameraListForm form = new GenTLCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");
            
            cntOpenDriver++;
            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
          
        }
    }
}

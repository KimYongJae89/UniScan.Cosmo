using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfo
    {
        int index;
        [Category("CameraInfo"), Description("Index"), ReadOnly(true)]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        GrabberType grabberType;
        [Category("CameraInfo"), Description("Grabber Type"), ReadOnly(true)]
        public GrabberType GrabberType
        {
            get { return grabberType; }
            set { grabberType = value; }
        }

        bool enabled = true;
        [Category("CameraInfo"), Description("Use Camera")]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        int width = 1000;
        [Category("CameraInfo"), Description("Image Width")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        int height = 1000;
        [Category("CameraInfo"), Description("Image Height")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        bool mirrorX;
        [Category("CameraInfo"), Description("Image Mirror X")]
        public bool MirrorX
        {
            get { return mirrorX; }
            set { mirrorX = value; }
        }

        bool mirrorY;
        [Category("CameraInfo"), Description("Image Mirror Y")]
        public bool MirrorY
        {
            get { return mirrorY; }
            set { mirrorY = value; }
        }

        PixelFormat pixelFormat = PixelFormat.Format8bppIndexed;
        [Category("CameraInfo"), Description("Pixel Format")]
        public PixelFormat PixelFormat
        {
            get { return pixelFormat; }
            set { pixelFormat = value; }
        }

        private bool bayerCamera;
        [Category("CameraInfo"), Description("?????")]
        public bool BayerCamera
        {
            get { return bayerCamera; }
            set { bayerCamera = value; }
        }

        float[] whiteBalanceCoefficient;
        [Category("CameraInfo"), Description("Manual PRNU")]
        public float[] WhiteBalanceCoefficient
        {
            get { return whiteBalanceCoefficient; }
            set { whiteBalanceCoefficient = value; }
        }

        BayerType bayerType;
        [Category("CameraInfo"), Description("?????")]
        internal BayerType BayerType
        {
            get { return bayerType; }
            set { bayerType = value; }
        }

        private RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        [Category("CameraInfo"), Description("Image Rotation")]
        public RotateFlipType RotateFlipType
        {
            get { return rotateFlipType; }
            set { rotateFlipType = value; }
        }

        bool useNativeBuffering;
        [Category("CameraInfo"), Description("Use image pointer instead of data")]
        public bool UseNativeBuffering
        {
            get { return useNativeBuffering; }
            set { useNativeBuffering = value; }
        }

        string virtualImagePath = "";
        [Category("CameraInfo"), Description("Virtual Image Path")]
        [System.ComponentModel.Editor(
            typeof(System.Windows.Forms.Design.FolderNameEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public string VirtualImagePath
        {
            get { return virtualImagePath; }
            set { virtualImagePath = value; }
        }

        public CameraInfo()
        {
            grabberType = GrabberType.Virtual;
        }

        public static CameraInfo Create(GrabberType grabberType)
        {
            CameraInfo cameraInfo;
            switch (grabberType)
            {
                case GrabberType.MultiCam:
                    cameraInfo = new CameraInfoMultiCam(); break;
                case GrabberType.Pylon:
                    cameraInfo = new CameraInfoPylon(); break;
                case GrabberType.MIL:
                    cameraInfo = new CameraInfoMil(); break;
                case GrabberType.GenTL:
                    cameraInfo = new CameraInfoGenTL(); break;
                case GrabberType.Virtual:
                default:
                    cameraInfo = new CameraInfo();   break;
            }
            
            return cameraInfo;
        }

        public virtual void LoadXml(XmlElement cameraElement)
        {
            grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(cameraElement, "Type", "Pylon"));
            enabled = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "Enabled", "True"));
            width = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "Width", "1000"));
            height = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "Height", "1000"));
            bayerCamera = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "BayerCamera", "False"));
            pixelFormat = (PixelFormat)Enum.Parse(typeof(PixelFormat), XmlHelper.GetValue(cameraElement, "PixelFormat", PixelFormat.Format8bppIndexed.ToString()));
            mirrorX = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "MirrorX", "False"));
            mirrorY = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "MirrorY", "False"));
            rotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), XmlHelper.GetValue(cameraElement, "RotateFlipType", "RotateNoneFlipNone"));
            useNativeBuffering = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "UseNativeBuffering", this.useNativeBuffering.ToString()));
            virtualImagePath = XmlHelper.GetValue(cameraElement, "VirtualImagePath", "");
        }

        public virtual void SaveXml(XmlElement cameraElement)
        {
            XmlHelper.SetValue(cameraElement, "Type", grabberType.ToString());
            XmlHelper.SetValue(cameraElement, "Enabled", enabled.ToString());
            XmlHelper.SetValue(cameraElement, "Width", width.ToString());
            XmlHelper.SetValue(cameraElement, "Height", height.ToString());
            XmlHelper.SetValue(cameraElement, "BayerCamera", bayerCamera.ToString());
            XmlHelper.SetValue(cameraElement, "PixelFormat", pixelFormat.ToString());
            XmlHelper.SetValue(cameraElement, "MirrorX", mirrorX.ToString());
            XmlHelper.SetValue(cameraElement, "MirrorY", mirrorY.ToString());
            XmlHelper.SetValue(cameraElement, "RotateFlipType", rotateFlipType.ToString());
            XmlHelper.SetValue(cameraElement, "UseNativeBuffering", useNativeBuffering.ToString());
            XmlHelper.SetValue(cameraElement, "VirtualImagePath",  virtualImagePath?.ToString());
        }
    }

    public class CameraConfiguration
    {
        int requiredCameras;
        public int RequiredCameras
        {
            get { return requiredCameras; }
            set { requiredCameras = value; }
        }

        public IEnumerator<CameraInfo> GetEnumerator()
        {
            return cameraInfoList.GetEnumerator();
        }

        List<CameraInfo> cameraInfoList = new List<CameraInfo>();
        public List<CameraInfo> CameraInfoList
        {
            get { return cameraInfoList; }
        }
        
        public void Clear()
        {
            cameraInfoList.Clear();
        }

        public void AddCameraInfo(CameraInfo cameraInfo)
        {
            cameraInfo.Index = cameraInfoList.Count();
            cameraInfo.Enabled = true;
            cameraInfoList.Add(cameraInfo);
        }

        public void LoadCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Camera Configuration");

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            int index = 0;

            XmlElement cameraListElement = xmlDocument.DocumentElement;
            foreach (XmlElement cameraElement in cameraListElement)
            {
                if (cameraElement.Name == "Camera")
                {
                    GrabberType grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(cameraElement, "Type", "Pylon"));

                    CameraInfo cameraInfo = CameraInfo.Create(grabberType);
                    cameraInfo.LoadXml(cameraElement);

                    cameraInfoList.Add(cameraInfo);
                }
            }
        }

        public void SaveCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Camera Configuration");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement cameraListElement = xmlDocument.CreateElement("", "CameraList", "");
            xmlDocument.AppendChild(cameraListElement);

            foreach (CameraInfo cameraInfo in cameraInfoList)
            {
                XmlElement cameraElement = xmlDocument.CreateElement("", "Camera", "");
                cameraListElement.AppendChild(cameraElement);

                cameraInfo.SaveXml(cameraElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}

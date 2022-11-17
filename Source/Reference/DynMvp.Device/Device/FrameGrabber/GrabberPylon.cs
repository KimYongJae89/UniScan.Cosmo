using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;

using PylonC.NET;
using PylonC.NETSupportLibrary;
using System.Windows.Forms;
using DynMvp.UI;
using System.ComponentModel;

namespace DynMvp.Devices.FrameGrabber
{
    public enum TrigerSourceType
    {
        Software, Line1, Line2, Line3, FrequencyConverter
    }

    public class CameraInfoPylon : CameraInfo
    {
        string deviceUserId;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Device ID")]
        public string DeviceUserId
        {
            get { return deviceUserId; }
            set { deviceUserId = value; }
        }

        bool updateDeviceFeature;
        [CategoryAttribute("CameraInfoPylon"),DisplayNameAttribute("Update Device Feature"), DescriptionAttribute("Ignore ImageSize Settings in Pylon Viewer")]
        public bool UpdateDeviceFeature
        {
            get { return updateDeviceFeature; }
            set { updateDeviceFeature = value; }
        }

        string ipAddress;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("IP Address")]
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        string serialNo;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Serial No")]
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        uint deviceIndex;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Device Index")]
        public uint DeviceIndex
        {
          get { return deviceIndex; }
          set { deviceIndex = value; }
        }

        string modelName;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Model Name")]
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        bool useLineTrigger;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Use Line Trigger")]
        public bool UseLineTrigger
        {
            get { return useLineTrigger; }
            set { useLineTrigger = value; }
        }

        TrigerSourceType lineTriggerSourceType;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Line Trigger Source Type")]
        public TrigerSourceType LineTriggerSourceType
        {
            get { return lineTriggerSourceType; }
            set { lineTriggerSourceType = value; }
        }

        bool useFrameTrigger;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Use Frame Trigger")]
        public bool UseFrameTrigger
        {
            get { return useFrameTrigger; }
            set { useFrameTrigger = value; }
        }

        TrigerSourceType frameTriggerSourceType;
        [CategoryAttribute("CameraInfoPylon"), DescriptionAttribute("Frame Trigger Source Type")]
        public TrigerSourceType FrameTriggerSourceType
        {
            get { return frameTriggerSourceType; }
            set { frameTriggerSourceType = value; }
        }

        public CameraInfoPylon()
        {
            this.GrabberType = GrabberType.Pylon;

            this.deviceUserId = "";
            this.ipAddress = "";
            this.serialNo = "";
            this.deviceIndex = 0;
            this.modelName = "";

            this.useFrameTrigger = false;
            this.frameTriggerSourceType = TrigerSourceType.Line2;
        }

        public CameraInfoPylon(string deviceUserId, string ipAddress, string serialNo)
        {
            this.GrabberType = GrabberType.Pylon;

            this.ipAddress = ipAddress;
            this.serialNo = serialNo;
            this.deviceUserId = deviceUserId;
            this.deviceIndex = 0;
            this.modelName = "";
            this.updateDeviceFeature = false;

            this.useLineTrigger = false;
            this.lineTriggerSourceType = TrigerSourceType.Line1;
            this.useFrameTrigger = false;
            this.frameTriggerSourceType = TrigerSourceType.Line2;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            deviceUserId = XmlHelper.GetValue(cameraElement, "DeviceUserId", "");
            ipAddress = XmlHelper.GetValue(cameraElement, "IpAddress", "");
            serialNo = XmlHelper.GetValue(cameraElement, "SerialNo", "");
            modelName = XmlHelper.GetValue(cameraElement, "ModelName", "");
            updateDeviceFeature = XmlHelper.GetValue(cameraElement, "UpdateDeviceFeature", updateDeviceFeature);

            useLineTrigger = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "UseLineTrigger", "false"));
            lineTriggerSourceType = (TrigerSourceType)Enum.Parse(typeof(TrigerSourceType), XmlHelper.GetValue(cameraElement, "LineTriggerSourceType", "Line2"));

            useFrameTrigger = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "UseFrameTrigger", "false"));
            frameTriggerSourceType = (TrigerSourceType)Enum.Parse(typeof(TrigerSourceType), XmlHelper.GetValue(cameraElement, "FrameTriggerSourceType", "Line2"));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "DeviceUserId", deviceUserId);
            XmlHelper.SetValue(cameraElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(cameraElement, "SerialNo", serialNo);
            XmlHelper.SetValue(cameraElement, "ModelName", modelName);
            XmlHelper.SetValue(cameraElement, "UpdateDeviceFeature", updateDeviceFeature);

            XmlHelper.SetValue(cameraElement, "UseFrameTrigger", useFrameTrigger);
            XmlHelper.SetValue(cameraElement, "FrameTriggerSourceType", frameTriggerSourceType.ToString());
        }
    }

    class InvalidDeviceListException : ApplicationException
    {
    }

    class InvalidIpAddressException : ApplicationException
    {
    }

    public class GrabberPylon : Grabber
    {
        List<DeviceEnumerator.Device> deviceList;

        public GrabberPylon(string name) : base(GrabberType.Pylon, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraPylon();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            PylonCameraListForm form = new PylonCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == DialogResult.OK;
        }

        public static void GetFeature(string featureFullString, out string deviceUserId, out string ipAddress, out string serialNo, out string modelName)
        {
            deviceUserId = "";
            ipAddress = "";
            serialNo = "";
            modelName = "";

            string[] features = featureFullString.Split('\n');
            foreach(string feature in features)
            {
                string[] tokens = feature.Split(':');

                string keyName = tokens[0].Replace(" ", "");
                if (keyName == "SerialNumber")
                {
                    serialNo = tokens[1].Trim();
                }
                else if (keyName == "IpAddress")
                {
                    ipAddress = tokens[1].Trim();
                }
                else if (keyName == "UserDefinedName")
                {
                    deviceUserId = tokens[1].Trim();
                }
                else if (keyName == "ModelName")
                {
                    modelName  = tokens[1].Trim();
                }
            }
        }

        private DeviceEnumerator.Device GetDevice(CameraInfoPylon cameraInfo)
        {
            if (deviceList == null)
                throw new InvalidDeviceListException();

            string deviceUserId, ipAddress, serialNo, modelName;

            foreach (DeviceEnumerator.Device device in deviceList)
            {
                GetFeature(device.Tooltip, out deviceUserId, out ipAddress, out serialNo, out modelName);

                if (string.IsNullOrEmpty(deviceUserId) == false && cameraInfo.DeviceUserId == deviceUserId)
                    return device;
                else if (string.IsNullOrEmpty(ipAddress) == false && cameraInfo.IpAddress == ipAddress)
                    return device;
                else if (string.IsNullOrEmpty(serialNo) == false && cameraInfo.SerialNo == serialNo)
                    return device;
            }

            return null;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000");

            Pylon.Initialize();

            deviceList = DeviceEnumerator.EnumerateDevices();

            return true;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylon) == false)
                return;

            CameraInfoPylon cameraInfoPylon = (CameraInfoPylon)cameraInfo;
            try
            {
                DeviceEnumerator.Device pylonDevice = GetDevice(cameraInfoPylon);
                if (pylonDevice == null)
                {
                    string messge = String.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                    MessageBox.Show(messge);
                    LogHelper.Error(LoggerType.Error, messge);

                    cameraInfoPylon.DeviceIndex = 0;
                    cameraInfoPylon.Enabled = false;

                    return;
                }

                cameraInfoPylon.DeviceIndex = pylonDevice.Index;
            }
            catch (InvalidIpAddressException)
            {
                string messge = String.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                MessageBox.Show(messge);
                LogHelper.Error(LoggerType.Error, messge);

                cameraInfoPylon.DeviceIndex = 0;
                cameraInfoPylon.Enabled = false;
            }                    
        }

        public override void Release()
        {
            base.Release();
            Pylon.Terminate();
        }
    }
}

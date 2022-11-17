using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using UniEye.Base.Settings;
using UniScan.Common.Data;

namespace UniScan.Common.Settings
{
    public class DriveInfoComparer : EqualityComparer<DriveInfo>
    {
        public override bool Equals(DriveInfo x, DriveInfo y)
        {
            return x.RootDirectory.Name == y.RootDirectory.Name;
        }

        public override int GetHashCode(DriveInfo obj)
        {
            return base.GetHashCode();
        }
    }

    public class SystemTypeSettings
    {
        static SystemTypeSettings _instance;
        public static SystemTypeSettings Instance()
        {
            if (_instance == null)
                _instance = new SystemTypeSettings();

            return _instance;
        }
        
        SystemType systemType;
        public SystemType SystemType
        {
            get { return systemType; }
            set { systemType = value; }
        }
        
        float resizeRatio = 0.1f;
        public float ResizeRatio
        {
            get { return resizeRatio; }
            set { resizeRatio = value; }
        }

        RectangleF monitorFov = new RectangleF();
        public RectangleF MonitorFov
        {
            get { return monitorFov; }
            set { monitorFov = value; }
        }
        
        public SystemTypeSettings()
        {
            //foreach (DriveInfo info in DriveInfo.GetDrives())
            //    driveInfoList.Add(info, false);
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemTypeConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlElement = xmlDocument["Config"];
            
            systemType = (SystemType)Enum.Parse(typeof(SystemType), XmlHelper.GetValue(configXmlElement, "SystemType", "Screen"));
            resizeRatio = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "ResizeRatio", "1"));
            //isStandAlone = XmlHelper.GetValue(configXmlElement, "IsStandAlone", isStandAlone);

            float fovX = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovX", "0"));
            float fovY = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovY", "0"));
            float fovW = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovWidth", "0"));
            float fovH = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovHeight", "0"));
            monitorFov = new RectangleF(fovX, fovY, fovW, fovH);
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemTypeConfig.xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlElement);

            XmlHelper.SetValue(configXmlElement, "SystemType", systemType.ToString());
            XmlHelper.SetValue(configXmlElement, "ResizeRatio", resizeRatio.ToString());
            //XmlHelper.SetValue(configXmlElement, "IsStandAlone", isStandAlone.ToString());

            XmlHelper.SetValue(configXmlElement, "FovX", monitorFov.X.ToString());
            XmlHelper.SetValue(configXmlElement, "FovY", monitorFov.Y.ToString());
            XmlHelper.SetValue(configXmlElement, "FovWidth", monitorFov.Width.ToString());
            XmlHelper.SetValue(configXmlElement, "FovHeight", monitorFov.Height.ToString());
            
            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}

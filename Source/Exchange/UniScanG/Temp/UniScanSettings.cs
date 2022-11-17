using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using DynMvp.Data;
using UniEye.Base.Settings;
using DynMvp.UI.Touch;

using System.Data;
using DynMvp.Devices.Comm;

namespace UniScanG.Temp
{
    public enum SaveDebugData { None,Text,Image,Both}
    public class UniScanGSettings
    {
        static UniScanGSettings _instance;
        static string configFileName;

        public static UniScanGSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new UniScanGSettings();
            }

            return _instance;
        }

        TcpIpInfo umxTcpIpInfo = new TcpIpInfo();
        public TcpIpInfo UmxTcpIpInfo
        {
            get { return umxTcpIpInfo; }
            set { umxTcpIpInfo = value; }
        }

        List<InspectorInfo> clientInfoList = new List<InspectorInfo>();
        public List<InspectorInfo> ClientInfoList
        {
            get { return clientInfoList; }
            set { clientInfoList = value; }
        }

        string vncPath;
        public string VncPath
        {
            get { return vncPath; }
            set { vncPath = value; }
        }
        
        int maskThH = 150;
        public int MaskThH
        {
            get { return maskThH; }
            set { maskThH = value; }
        }

        int maskThV = 150;
        public int MaskThV
        {
            get { return maskThV; }
            set { maskThV = value; }
        }

        float startXPosition;
        public float StartXPosition
        {
            get { return startXPosition; }
            set { startXPosition = value; }
        }

        float startYPosition;
        public float StartYPosition
        {
            get { return startYPosition; }
            set { startYPosition = value; }
        }

        float fovX = 100;
        public float FovX
        {
            get { return fovX; }
            set { fovX = value; }
        }

        //float sheetHeight = 220;
        //public float SheetHeight
        //{
        //    get { return sheetHeight; }
        //    set { sheetHeight = value; }
        //}

        int saturationRange = 10;
        public int SaturationRange
        {
            get { return saturationRange; }
            set { saturationRange = value; }
        }

        int circleRadius = 400;
        public int CircleRadius
        {
            get { return circleRadius; }
            set { circleRadius = value; }
        }

        float patternGroupTheshold = 15;
        public float PatternGroupTheshold
        {
            get { return patternGroupTheshold; }
            set { patternGroupTheshold = value; }
        }

        int maxPattern = 10000;
        public int MaxPattern
        {
            get { return maxPattern; }
            set { maxPattern = value; }
        }

        private bool asyncMode = false;
        public bool AsyncMode
        {
            get { return asyncMode; }
            set { asyncMode = value; }
        }

        private int bufferSize = 10;
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        string saveImageDebugDataPath = "";
        public string SaveImageDebugDataPath
        {
            get { return saveImageDebugDataPath; }
            set { saveImageDebugDataPath = value; }
        }

        private SaveDebugData saveImageDebugData = SaveDebugData.None;
        public SaveDebugData SaveImageDebugData
        {
            get { return saveImageDebugData; }
            set { saveImageDebugData = value; }
        }

        private SaveDebugData saveFiducialDebugData = SaveDebugData.None;
        public SaveDebugData SaveFiducialDebugData
        {
            get { return saveFiducialDebugData; }
            set { saveFiducialDebugData = value; }
        }

        private SaveDebugData saveInspectionDebugData = SaveDebugData.None;
        public SaveDebugData SaveInspectionDebugData
        {
            get { return saveInspectionDebugData; }
            set { saveInspectionDebugData = value; }
        }

        MonitorInfo monitorInfo = new MonitorInfo();
        public MonitorInfo MonitorInfo
        {
            get { return monitorInfo; }
            set { monitorInfo = value; }
        }

        InspectorInfo inspectorInfo = new InspectorInfo();
        public InspectorInfo InspectorInfo
        {
            get { return inspectorInfo; }
            set { inspectorInfo = value; }
        }
        

        public UniScanGSettings()
        {

        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\customConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Config"];
            
            umxTcpIpInfo.Load(configXmlDocument, "UmxTcpIpInfo");

            //systemType = (SystemType)Enum.Parse(typeof(SystemType), XmlHelper.GetValue(configXmlDocument, "SystemType", "Inspector"));
            umxTcpIpInfo.Load(configXmlDocument, "UmxTcpIpInfo");

            foreach (XmlElement clientInfoElement in configXmlDocument)
            {
                if (clientInfoElement.Name == "ClientInfo")
                {
                    InspectorInfo clientInfo = new InspectorInfo();
                    clientInfo.Load(clientInfoElement);
                    clientInfoList.Add(clientInfo);
                }
            }

            foreach (XmlElement monitorInfoElement in configXmlDocument)
            {
                if (monitorInfoElement.Name == "MonitorInfo")
                {
                    monitorInfo = new MonitorInfo();
                    monitorInfo.Load(monitorInfoElement);
                }
            }

            foreach (XmlElement inspectorInfoElement in configXmlDocument)
            {
                if (inspectorInfoElement.Name == "InspectorInfo")
                {
                    inspectorInfo = new InspectorInfo();
                    inspectorInfo.Load(inspectorInfoElement);
                }
            }

            vncPath = XmlHelper.GetValue(configXmlDocument, "VncPath", "");
            
            asyncMode = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "AsyncMode", "false"));
            bufferSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "BufferSize", "10"));

            saveImageDebugData = (SaveDebugData)Enum.Parse(typeof(SaveDebugData), XmlHelper.GetValue(configXmlDocument, "SaveImageDebugData", "None"));
            saveFiducialDebugData = (SaveDebugData)Enum.Parse(typeof(SaveDebugData), XmlHelper.GetValue(configXmlDocument, "SaveFiducialDebugData", "None"));
            saveInspectionDebugData = (SaveDebugData)Enum.Parse(typeof(SaveDebugData), XmlHelper.GetValue(configXmlDocument, "SaveInspectionDebugData", "None"));
            saveImageDebugDataPath = XmlHelper.GetValue(configXmlDocument, "SaveImageDebugDataPath", "");

            startXPosition = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "StartXPosition", "0"));
            startYPosition = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "StartYPosition", "0"));
            fovX = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "FovX", "100"));
            maskThH = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaskThH", "100"));
            maskThV = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaskThV", "100"));

            patternGroupTheshold = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "PatternGroupTheshold", "15"));
            
            saturationRange = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SaturationRange", "20"));
            circleRadius = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "CircleRadius", "400"));

            maxPattern = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxPattern", "10000"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\customConfig.xml", PathSettings.Instance().Config);

            XmlDocument configXmlDocument = new XmlDocument();

            XmlElement configElement = configXmlDocument.CreateElement("", "Config", "");
            configXmlDocument.AppendChild(configElement);

            //XmlDocument machineXmlDocument = new XmlDocument();
            //XmlElement machineElement = configXmlDocument.CreateElement("", "Machine", "");
            //machineXmlDocument.AppendChild(machineElement);
            
            
            XmlElement chartOptionElement = configElement.OwnerDocument.CreateElement("", "ChartOption", "");
            configElement.AppendChild(chartOptionElement);

            XmlElement sheetOptionElement = configElement.OwnerDocument.CreateElement("", "SheetOption", "");
            configElement.AppendChild(sheetOptionElement);
            
            //XmlHelper.SetValue(configElement, "SystemType", systemType.ToString());
            umxTcpIpInfo.Save(configElement, "UmxTcpIpInfo");
            
            foreach (InspectorInfo clientInfo in clientInfoList)
            {
                XmlElement clientInfoElement = configElement.OwnerDocument.CreateElement("", "ClientInfo", "");
                configElement.AppendChild(clientInfoElement);
                clientInfo.Save(clientInfoElement);
            }

            XmlElement monitorInfoElement = configElement.OwnerDocument.CreateElement("", "MonitorInfo", "");
            configElement.AppendChild(monitorInfoElement);
            monitorInfo.Save(monitorInfoElement);

            XmlElement inspectorInfoElement = configElement.OwnerDocument.CreateElement("", "InspectorInfo", "");
            configElement.AppendChild(inspectorInfoElement);
            inspectorInfo.Save(inspectorInfoElement);

            XmlHelper.SetValue(configElement, "VncPath", vncPath);

            XmlHelper.SetValue(configElement, "AsyncMode", asyncMode.ToString());
            XmlHelper.SetValue(configElement, "BufferSize", bufferSize.ToString());
            XmlHelper.SetValue(configElement, "SaveImageDebugData", saveImageDebugData.ToString());
            XmlHelper.SetValue(configElement, "SaveImageDebugDataPath", saveImageDebugDataPath.ToString());
            XmlHelper.SetValue(configElement, "SaveFiducialDebugData", saveFiducialDebugData.ToString());
            XmlHelper.SetValue(configElement, "SaveInspectionDebugData", saveInspectionDebugData.ToString());

            XmlHelper.SetValue(configElement, "StartXPosition", startXPosition.ToString());
            XmlHelper.SetValue(configElement, "StartYPosition", startYPosition.ToString());
            XmlHelper.SetValue(configElement, "FovX", fovX.ToString());
            XmlHelper.SetValue(configElement, "MaskThH", maskThH.ToString());
            XmlHelper.SetValue(configElement, "MaskThV", maskThV.ToString());

            XmlHelper.SetValue(configElement, "PatternGroupTheshold", patternGroupTheshold.ToString());
            XmlHelper.SetValue(configElement, "SaturationRange", saturationRange.ToString());

            XmlHelper.SetValue(configElement, "CircleRadius", circleRadius.ToString());

            XmlHelper.SetValue(configElement, "MaxPattern", maxPattern.ToString());
            XmlHelper.Save(configXmlDocument, configFileName);
        }
    }

    public enum GrabberType
    {
        Master, Slave
    }

    public class MonitorInfo
    {
        string version = "2.0";
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        string ipAddress = "";
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        int overlapAreaPx = 0;
        public int OverlapAreaPx
        {
            get { return overlapAreaPx; }
            set { overlapAreaPx = value; }
        }

        public void Load(XmlElement xmlElement)
        {
            version = XmlHelper.GetValue(xmlElement, "Version", version.ToString());
            ipAddress = XmlHelper.GetValue(xmlElement, "IpAddress", "");
            overlapAreaPx =Convert.ToInt32(XmlHelper.GetValue(xmlElement, "OverlapAreaPx", "0"));
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Version", version);
            XmlHelper.SetValue(xmlElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(xmlElement, "OverlapAreaPx", overlapAreaPx.ToString());
        }
    }

    public class InspectorInfo
    {
        string ipAddress = "";
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        string path = "";
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        GrabberType grabberType;
        public GrabberType GrabberType
        {
            get { return grabberType; }
            set { grabberType = value; }
        }

        int camIndex = 0;
        public int CamIndex
        {
            get { return camIndex; }
            set { camIndex = value; }
        }

        int clientIndex = 0;
        public int ClientIndex
        {
            get { return clientIndex; }
            set { clientIndex = value; }
        }

        bool standAlone = false;
        public bool StandAlone
        {
            get { return standAlone; }
            set { standAlone = value; }
        }

        string password = "";
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public void Load(XmlElement xmlElement)
        {
            ipAddress = XmlHelper.GetValue(xmlElement, "IpAddress", "");
            path = XmlHelper.GetValue(xmlElement, "Path", "");
            clientIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ClientIndex", "0"));
            camIndex = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CamIndex", "0"));
            standAlone = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "StandAlone", standAlone.ToString()));
            password = XmlHelper.GetValue(xmlElement, "Password", "");
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(xmlElement, "Path", path);
            XmlHelper.SetValue(xmlElement, "ClientIndex", clientIndex.ToString());
            XmlHelper.SetValue(xmlElement, "CamIndex", camIndex.ToString());
            XmlHelper.SetValue(xmlElement, "StandAlone", standAlone.ToString());
            XmlHelper.SetValue(xmlElement, "Password", password);
        }
    }
    
    public class SamsungElectroTransferSettings
    {
        static SamsungElectroTransferSettings _instance;

        public static SamsungElectroTransferSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new SamsungElectroTransferSettings();
            }

            return _instance;
        }
        
        int storingDays = 21;
        public int StoringDays
        {
            get { return storingDays; }
            set { storingDays = value; }
        }

        public void Load(string path = null)
        {
            string configFileName = null;

            if (path == null)
                configFileName = String.Format(@"{0}\transferConfig.xml", PathSettings.Instance().Config);
            else
                configFileName = String.Format(@"{0}\transferConfig.xml", path); ;

            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Config"];
            storingDays = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "StoringDays", "21"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\transferConfig.xml", PathSettings.Instance().Config);

            XmlDocument configXmlDocument = new XmlDocument();

            XmlElement configElement = configXmlDocument.CreateElement("", "Config", "");
            configXmlDocument.AppendChild(configElement);

            XmlHelper.SetValue(configElement, "StoringDays", storingDays.ToString());

            XmlHelper.Save(configXmlDocument, configFileName);
        }
    }
}

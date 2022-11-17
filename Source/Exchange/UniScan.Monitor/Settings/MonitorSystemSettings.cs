using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScan.Common.Data;
using UniScan.Common.Exchange;

namespace UniScan.Monitor.Settings.Monitor
{
    public class MonitorSystemSettings
    {
        TcpIpMachineIfSettings serverSetting;
        public TcpIpMachineIfSettings ServerSetting
        {
            get { return serverSetting; }
            set { serverSetting = value; }
        }

        static MonitorSystemSettings _instance;
        public static MonitorSystemSettings Instance()
        {
            if (_instance == null)
                _instance = new MonitorSystemSettings();

            return _instance;
        }

        List<InspectorInfo> inspectorInfoList = new List<InspectorInfo>();
        public List<InspectorInfo> InspectorInfoList
        {
            get { return inspectorInfoList; }
            set { inspectorInfoList = value; }
        }

        string vncPath;
        public string VncPath
        {
            get { return vncPath; }
            set { vncPath = value; }
        }

        bool localExchangeMode;
        public bool LocalExchangeMode
        {
            get { return localExchangeMode; }
            set { localExchangeMode = value; }
        }

        bool useTestbedStage = false;
        public bool UseTestbedStage { get => useTestbedStage; set => useTestbedStage = value; }

        bool useLaserBurner = false;
        public bool UseLaserBurner { get => this.useLaserBurner; set => this.useLaserBurner = value; }

        public MonitorSystemSettings()
        {
            serverSetting = new TcpIpMachineIfSettings(MachineIfType.TcpServer);
            //serverSetting.TcpIpInfo = new TcpIpInfo(AddressManager.Instance().GetMonitorAddress(), 6000);

            ExchangeProtocolList exchangeProtocolList = new ExchangeProtocolList(typeof(ExchangeCommand));
            exchangeProtocolList.Initialize(MachineIfType.TcpServer);

            serverSetting.MachineIfProtocolList = exchangeProtocolList;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemConfig(Monitor).xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlElement = xmlDocument["Config"];
            
            vncPath = XmlHelper.GetValue(configXmlElement, "VncPath", "");
            localExchangeMode = Convert.ToBoolean(XmlHelper.GetValue(configXmlElement, "LocalExchangeMode", localExchangeMode.ToString()));
            this.useTestbedStage = XmlHelper.GetValue(configXmlElement, "UseTestbedStage", this.useTestbedStage);
            this.useLaserBurner = XmlHelper.GetValue(configXmlElement, "UseLaserBurner", this.useLaserBurner);

            foreach (XmlElement inspectorInfoElement in configXmlElement)
            {
                if (inspectorInfoElement.Name == "InspectorInfo")
                {
                    InspectorInfo inspectorInfo = new InspectorInfo();
                    inspectorInfo.Load(inspectorInfoElement);
                    inspectorInfoList.Add(inspectorInfo);
                }
            }

            XmlElement serverXmlElement = configXmlElement["Server"];
            serverSetting.Load(serverXmlElement);
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemConfig(Monitor).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();
            
            XmlElement configXmlElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlElement);
            
            XmlHelper.SetValue(configXmlElement, "VncPath", vncPath);
            XmlHelper.SetValue(configXmlElement, "LocalExchangeMode", localExchangeMode.ToString());
            XmlHelper.SetValue(configXmlElement, "UseTestbedStage", this.useTestbedStage.ToString());
            XmlHelper.SetValue(configXmlElement, "UseLaserBurner", this.useLaserBurner.ToString());

            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
            {
                XmlElement inspectorInfoElement = xmlDocument.CreateElement("", "InspectorInfo", "");
                configXmlElement.AppendChild(inspectorInfoElement);

                inspectorInfo.Save(inspectorInfoElement);
            }

            XmlElement serverXmlElement = xmlDocument.CreateElement("", "Server", "");
            configXmlElement.AppendChild(serverXmlElement);
            serverSetting.Save(serverXmlElement);

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}

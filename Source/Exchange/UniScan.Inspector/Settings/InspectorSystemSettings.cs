using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScan.Common.Data;
using UniScan.Common.Exchange;

namespace UniScan.Inspector.Settings.Inspector
{
    public class InspectorSystemSettings
    {
        TcpIpMachineIfSettings clientSetting = new TcpIpMachineIfSettings(MachineIfType.TcpClient);
        public TcpIpMachineIfSettings ClientSetting
        {
            get { return clientSetting; }
            set { clientSetting = value; }
        }

        TcpIpMachineIfSettings serverSetting = new TcpIpMachineIfSettings(MachineIfType.TcpServer);
        public TcpIpMachineIfSettings ServerSetting
        {
            get { return serverSetting; }
            set { serverSetting = value; }
        }

        static InspectorSystemSettings _instance;
        public static InspectorSystemSettings Instance()
        {
            if (_instance == null)
                _instance = new InspectorSystemSettings();

            return _instance;
        }

        List<InspectorInfo> slaveInfoList = new List<InspectorInfo>();
        public List<InspectorInfo> SlaveInfoList
        {
            get { return slaveInfoList; }
            set { slaveInfoList = value; }
        }

        int camIndex;
        public int CamIndex
        {
            get { return camIndex; }
            set { camIndex = value; }
        }

        int clientIndex;
        public int ClientIndex
        {
            get { return clientIndex; }
            set { clientIndex = value; }
        }

        bool localExchangeMode;
        public bool LocalExchangeMode
        {
            get { return localExchangeMode; }
            set { localExchangeMode = value; }
        }

        public InspectorSystemSettings()
        {
            ExchangeProtocolList exchangeProtocolList = new ExchangeProtocolList(typeof(ExchangeCommand));
            exchangeProtocolList.Initialize(MachineIfType.TcpClient);

            clientSetting.MachineIfProtocolList = exchangeProtocolList;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlDocument = xmlDocument["Config"];

            XmlElement clientXmlElement = configXmlDocument["Client"];
            clientSetting.Load(clientXmlElement);

            XmlElement serverXmlElement = configXmlDocument["Server"];
            serverSetting.Load(serverXmlElement);

            camIndex = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "CamIndex", "0"));
            clientIndex = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "ClientIndex", "0"));
            localExchangeMode = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "LocalExchangeMode", localExchangeMode.ToString()));

            foreach (XmlElement slaveInfoElement in configXmlDocument)
            {
                if (slaveInfoElement.Name == "SlaveInfo")
                {
                    InspectorInfo slaveInfo = new InspectorInfo();
                    slaveInfo.Load(slaveInfoElement);
                    slaveInfoList.Add(slaveInfo);
                }
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemConfig.xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();
            
            XmlElement configXmlDocument = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlDocument);

            XmlElement clientXmlElement = xmlDocument.CreateElement("", "Client", "");
            configXmlDocument.AppendChild(clientXmlElement);
            clientSetting.Save(clientXmlElement);

            XmlElement serverXmlElement = xmlDocument.CreateElement("", "Server", "");
            configXmlDocument.AppendChild(serverXmlElement);
            serverSetting.Save(serverXmlElement);

            XmlHelper.SetValue(configXmlDocument, "CamIndex", camIndex.ToString());
            XmlHelper.SetValue(configXmlDocument, "ClientIndex", clientIndex.ToString());
            XmlHelper.SetValue(configXmlDocument, "LocalExchangeMode", localExchangeMode.ToString());
            
            XmlElement monitoringElement = xmlDocument.CreateElement("", "Monitor", "");
            configXmlDocument.AppendChild(monitoringElement);

            foreach (InspectorInfo slaveInfo in slaveInfoList)
            {
                XmlElement slaveInfoElement = xmlDocument.CreateElement("", "SlaveInfo", "");
                configXmlDocument.AppendChild(slaveInfoElement);

                slaveInfo.Save(slaveInfoElement);
            }

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}

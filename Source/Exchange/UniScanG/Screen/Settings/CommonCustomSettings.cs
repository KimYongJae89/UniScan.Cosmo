using DynMvp.Base;
using System;
using System.IO;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanG.Screen.Settings
{
    internal class CommonCustomSettings
    {
        static CommonCustomSettings _instance;
        public static CommonCustomSettings Instance()
        {
            if (_instance == null)
                _instance = new CommonCustomSettings();

            return _instance;
        }

        int samePointRadius = 20;
        public int SamePointRadius
        {
            get { return samePointRadius; }
            set { samePointRadius = value; }
        }
        
        float poleCompactness;
        public float PoleCompactness
        {
            get { return poleCompactness; }
            set { poleCompactness = value; }
        }

        float dielectricCompactness;
        public float DielectricCompactness
        {
            get { return dielectricCompactness; }
            set { dielectricCompactness = value; }
        }

        public void Load(string path = null)
        {
            string configFileName = null;

            if (path == null)
                configFileName = String.Format(@"{0}\{1}", "CustomConfig.xml", PathSettings.Instance().Config);
            else
                configFileName = String.Format(@"{0}\{1}", "CustomConfig.xml", path); ;
            
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Config"];

            poleCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "PoleCompactness", "3"));
            dielectricCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "DielectricCompactness", "1"));
            
            samePointRadius = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SamePointRadius", "20"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\CustomConfig.xml", PathSettings.Instance().Config);

            XmlDocument configXmlDocument = new XmlDocument();

            XmlElement configElement = configXmlDocument.CreateElement("", "Config", "");
            configXmlDocument.AppendChild(configElement);
            
            XmlHelper.SetValue(configElement, "PoleCompactness", poleCompactness.ToString());
            XmlHelper.SetValue(configElement, "DielectricCompactness", dielectricCompactness.ToString());
            
            XmlHelper.SetValue(configElement, "SamePointRadius", samePointRadius.ToString());

            XmlHelper.Save(configXmlDocument, configFileName);
        }
    }
}

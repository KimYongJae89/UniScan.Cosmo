using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Settings
{
    public class DeveloperSettings : ISavableObj
    {
        static DeveloperSettings _instance;

        string fileName;

        float resolution;
        int backwardOffset;
        int moveOffset;
        int bufferWidth;
        int bufferHeight;
        int scanNum;

        public float Resolution { get => resolution; }
        public int BackwardOffset { get => backwardOffset; }
        public int MoveOffset { get => moveOffset; }
        public int BufferWidth { get => bufferWidth; }
        public int BufferHeight { get => bufferHeight; }
        public int ScanNum { get => scanNum; }

        public static DeveloperSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeveloperSettings();
                    _instance.Load();
                }

                return _instance;
            }
        }
        
        private DeveloperSettings()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Developer");

            resolution = 5.0f;
            backwardOffset = 45;
            bufferWidth = 12280;
            bufferHeight = 100000;

            scanNum = 9;
        }

        public void Load()
        {
            if (File.Exists(fileName) == false)
            {
                Save();
                return;
            }

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement settingsElement = xmlDocument["Settings"];
            if (settingsElement == null)
                return;

            resolution = Convert.ToSingle(XmlHelper.GetValue(settingsElement, "Resolution", "5"));
            backwardOffset = Convert.ToInt32(XmlHelper.GetValue(settingsElement, "BackwardOffset", "45"));
            moveOffset = Convert.ToInt32(XmlHelper.GetValue(settingsElement, "MoveOffset", "2000"));
            bufferWidth = Convert.ToInt32(XmlHelper.GetValue(settingsElement, "BufferWidth", "12280"));
            bufferHeight = Convert.ToInt32(XmlHelper.GetValue(settingsElement, "BufferHeight", "12280"));
            scanNum = Convert.ToInt32(XmlHelper.GetValue(settingsElement, "ScanNum", "9"));
        }

        public void Save(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.fileName;

            if (Directory.Exists(fileName) == false)
                Directory.CreateDirectory(fileName);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement settingsElement = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(settingsElement);
            
            XmlHelper.SetValue(settingsElement, "Resolution", resolution.ToString());
            XmlHelper.SetValue(settingsElement, "BackwardOffset", backwardOffset.ToString());
            XmlHelper.SetValue(settingsElement, "MoveOffset", moveOffset.ToString());
            XmlHelper.SetValue(settingsElement, "BufferWidth", bufferWidth.ToString());
            XmlHelper.SetValue(settingsElement, "BufferHeight", bufferHeight.ToString());
            XmlHelper.SetValue(settingsElement, "ScanNum", scanNum.ToString());

            xmlDocument.Save(fileName);
        }
    }
}

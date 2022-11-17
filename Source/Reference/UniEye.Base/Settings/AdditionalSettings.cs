using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.Settings
{
    public delegate void AdditionalSettingChangedDelegate();

    public class AdditionalSettings
    {
        public AdditionalSettingChangedDelegate AdditionalSettingChangedDelegate = null;

        protected static AdditionalSettings instance = null;
        //protected string fileName = String.Format(@"{0}\AdditionalSettings.xml", PathSettings.Instance().Config);
        protected string fileName = @"AdditionalSettings.xml";

        protected AdditionalSettings()        {        }

        public static AdditionalSettings Instance()
        {
            return instance;
        }

        public static void CreateInstance()
        {
            if (instance == null)
                instance = new AdditionalSettings();
        }

        public void Load()
        {
            bool ok = false;
            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(Path.Combine(PathSettings.Instance().Config, fileName));
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Additional"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
            }
            finally
            {
                if (ok == false)
                    Save();
            }
        }

        public virtual void Load(XmlElement xmlElement) { }

        public void Save()
        {
            if (Directory.Exists(PathSettings.Instance().Config) == false)
                Directory.CreateDirectory(PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Additional");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(Path.Combine(PathSettings.Instance().Config, fileName));

            PostSave();
        }

        public virtual void Save(XmlElement xmlElement) { }


        protected virtual void PostSave()        {        }
    }
}

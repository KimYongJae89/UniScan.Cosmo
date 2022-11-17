using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.Settings
{
    public enum EAutoStartMethod { Encoder, PLC }
    public enum EInspectionMode { Inspect, Monitor }
    public enum EOperationMode { Sequencial, Random }
    public abstract class UniScanMSettings : UniEye.Base.Settings.AdditionalSettings
    {
        float maximumLineSpeed = 100.0f;

        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Maximum Line Speed [m/m]")]
        public float MaximumLineSpeed
        {
            get { return maximumLineSpeed; }
            set { maximumLineSpeed = value; }
        }

        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Save Debug Image")]
        public bool SaveDebugImage
        {
            get { return OperationSettings.Instance().SaveDebugImage; }
            set { OperationSettings.Instance().SaveDebugImage = value; }
        }

        public static UniScanMSettings Instance()
        {
            return instance as UniScanMSettings;
        }

        public override void Save(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "MaximumLineSpeed", maximumLineSpeed.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            maximumLineSpeed = XmlHelper.GetValue(xmlElement, "MaximumLineSpeed", maximumLineSpeed);
        }
    }
}

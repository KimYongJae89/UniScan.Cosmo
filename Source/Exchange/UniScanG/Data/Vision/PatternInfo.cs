using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Data.Vision
{
    public class PatternInfo : BlobRect
    {
        int waistRatio;

        public int WaistRatio
        {
            get { return waistRatio; }
            set { waistRatio = value; }
        }

        public PatternInfo() : base()
        {

        }

        public PatternInfo(BlobRect f)
        {
            this.Copy(f);
            this.waistRatio = 0;
        }

        public PatternInfo Clone()
        {
            PatternInfo clone = new PatternInfo();
            clone.Copy(this);
            return clone;
        }

        public void Copy(PatternInfo srcPatternInfo)
        {
            base.Copy(srcPatternInfo);
            this.waistRatio = srcPatternInfo.waistRatio;
        }

        public override void LoadXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement[key];
                if (sumElement != null)
                    LoadXml(sumElement);
                return;
            }

            base.LoadXml(xmlElement);
            this.waistRatio = XmlHelper.GetValue(xmlElement, "WaistRatio", this.waistRatio);
        }

        public override void SaveXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(sumElement);
                SaveXml(sumElement);
                return;
            }

            base.SaveXml(xmlElement);
            XmlHelper.SetValue(xmlElement, "WaistRatio", this.waistRatio);
        }
    }
}

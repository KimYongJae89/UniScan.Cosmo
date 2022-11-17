using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Gravure.Vision.Detector;

namespace UniScanG.Gravure.Settings
{
    public abstract class SettingElement
    {
        public abstract void Save(XmlElement xmlElement);
        public void Save(XmlElement xmlElement, string subElementName = "")
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subElementName);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            Save(xmlElement);
        }

        public abstract bool Load(XmlElement xmlElement);

        public bool Load(XmlElement xmlElement, string subElementName = "")
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement[subElementName];
                return Load(subElement);
            }

            if (xmlElement == null)
                return false;

            return Load(xmlElement);
        }
    }

    public abstract class AlarmSettingElement : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Use"), LocalizedDescriptionAttributeUniScanG("Use Alaram")]
        public bool Use { get => use; set => use = value; }
        protected bool use;

        [LocalizedDisplayNameAttributeUniScanG("Min Sheet Count"), LocalizedDescriptionAttributeUniScanG("Minimum Sheet Count")]
        public int Count { get => count; set => count = value; }
        protected int count;

        public AlarmSettingElement(bool use, int count)
        {
            this.use = use;
            this.count = count;
        }

        public override void Save(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return ;

            XmlHelper.SetValue(xmlElement, "Use", use.ToString());
            XmlHelper.SetValue(xmlElement, "Count", count.ToString());
        }


        public override bool Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return false;

            this.use = XmlHelper.GetValue(xmlElement, "Use", use);
            this.count = XmlHelper.GetValue(xmlElement, "Count", count);

            return true;
        }
    }

    public class RatioAlarmSettingElement : AlarmSettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Min NG Ratio"), LocalizedDescriptionAttributeUniScanG("Minimum NG Ratio")]
        public double Ratio { get => ratio; set => ratio = value; }
        double ratio;

        public RatioAlarmSettingElement(bool use, int count, double ratio) : base(use, count)
        {
            this.ratio = ratio;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "Ratio", ratio.ToString());
        }

        public override bool Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return false;

            this.ratio = XmlHelper.GetValue(xmlElement, "Ratio", ratio);
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}[sheets] / {2:F2}[%]", this.use ? "Use" : "Unuse", count, ratio);
        }
    }

    public class ValueAlarmSettingElement : AlarmSettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Min Difference Value"), LocalizedDescriptionAttributeUniScanG("Minimum Difference Value")]
        public double Value { get => value; set => this.value = value; }
        double value;

        string unit;
        public ValueAlarmSettingElement(bool use, int count, double value, string unit) : base(use, count)
        {
            this.value = value;
            this.unit = unit;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "Value", value.ToString());
            XmlHelper.SetValue(xmlElement, "Unit", unit);
        }

        public override bool Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return false;

            this.value = XmlHelper.GetValue(xmlElement, "Value", value);
            this.unit = XmlHelper.GetValue(xmlElement, "Unit", unit);
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}[sheets] / {2:F2}[{3}]", this.use ? "Use" : "Unuse", count, value, unit);
        }
    }

    public class LaserSettingElement : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Use Laser"), LocalizedDescriptionAttributeUniScanG("Use Laser")]
        public bool Use { get => use; set => use = value; }
        bool use;

        [LocalizedDisplayNameAttributeUniScanG("Laser Distance[m]"), LocalizedDescriptionAttributeUniScanG("Laser Distance[m]")]
        public double DistanceM { get => distanceM; set => distanceM = value; }
        double distanceM;

        [LocalizedDisplayNameAttributeUniScanG("Safe Distance[m]"), LocalizedDescriptionAttributeUniScanG("Safe Distance[m]")]
        public double SafeDistanceM { get => safeDistanceM; set => safeDistanceM = value; }
        double safeDistanceM;

        [LocalizedDisplayNameAttributeUniScanG("Min Pinhole"), LocalizedDescriptionAttributeUniScanG("Min Pinhole")]
        public int Pinhole { get => pinhole; set => pinhole = value; }
        int pinhole;

        [LocalizedDisplayNameAttributeUniScanG("Min Noprint"), LocalizedDescriptionAttributeUniScanG("Min Noprint")]
        public int Noprint { get => noprint; set => noprint = value; }
        int noprint;
        [LocalizedDisplayNameAttributeUniScanG("Min Coating"), LocalizedDescriptionAttributeUniScanG("Min Coating")]
        public int Coating { get => coating; set => coating = value; }
        int coating;

        [LocalizedDisplayNameAttributeUniScanG("Min Sheetattack"), LocalizedDescriptionAttributeUniScanG("Min Sheetattack")]
        public int Sheetattack { get => sheetattack; set => sheetattack = value; }
        int sheetattack;

        public LaserSettingElement(bool use, double laserDistanceM, double safeDistanceM,
            int pinhole, int noprint, int coating, int sheetattack) : base()
        {
            this.use = use; 
            this.DistanceM = laserDistanceM; 
            this.safeDistanceM = safeDistanceM; 

            this.pinhole = pinhole; 
            this.noprint = noprint; 
            this.coating = coating; 
            this.sheetattack = sheetattack;
        }

        public override void Save(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "Use", use.ToString());
            XmlHelper.SetValue(xmlElement, "DistanceM", DistanceM.ToString());
            XmlHelper.SetValue(xmlElement, "SafeDistanceM", safeDistanceM.ToString());

            XmlHelper.SetValue(xmlElement, "Pinhole", pinhole.ToString());
            XmlHelper.SetValue(xmlElement, "Noprint", noprint.ToString());
            XmlHelper.SetValue(xmlElement, "Coating", coating.ToString());
            XmlHelper.SetValue(xmlElement, "Sheetattack", sheetattack.ToString());
        }

        public override bool Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return false;

            this.use = XmlHelper.GetValue(xmlElement, "Use", use);
            this.DistanceM = XmlHelper.GetValue(xmlElement, "DistanceM", DistanceM);
            this.safeDistanceM = XmlHelper.GetValue(xmlElement, "SafeDistanceM", safeDistanceM);

            this.pinhole = XmlHelper.GetValue(xmlElement, "Pinhole", pinhole);
            this.noprint = XmlHelper.GetValue(xmlElement, "Noprint", noprint);
            this.coating = XmlHelper.GetValue(xmlElement, "Coating", coating);
            this.sheetattack = XmlHelper.GetValue(xmlElement, "Sheetattack", sheetattack);

            return true;
        }

        public override string ToString()
        {
            //return string.Format("Pinhole {0} / Noprint {1} / Coating {2} / Sheetattack {3}", this.pinhole, this.noprint, this.coating, this.sheetattack);
            return this.use ? "Use" : "Unuse";
        }

        public bool CheckCondition(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (this.use == false)
                return false;

            AlgorithmResult algorithmResult = null;
            inspectionResult.AlgorithmResultLDic.TryGetValue(Detector.TypeName, out algorithmResult);
            DetectorResult detectorResult = algorithmResult as DetectorResult;
            if (detectorResult==null)
                return false;

            //int pinhole, noprint, coating, sheetattack;
            int[] items = new int[4] { 0, 0, 0, 0 };
            detectorResult.SheetSubResultList.ForEach(f =>
            {
                switch (f.GetDefectType())
                {
                    case UniScanG.Data.DefectType.PinHole: items[0]++; break;
                    case UniScanG.Data.DefectType.Noprint: items[1]++; break;
                    case UniScanG.Data.DefectType.Dielectric: items[2]++; break;
                    case UniScanG.Data.DefectType.SheetAttack: items[3]++; break;
                }
            });

            return this.pinhole < items[0] || this.noprint < items[1] || this.coating < items[2] || this.sheetattack < items[3];
        }
    }

}

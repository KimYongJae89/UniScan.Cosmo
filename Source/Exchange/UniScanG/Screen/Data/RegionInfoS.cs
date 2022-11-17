using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Drawing;
using System.Xml;

namespace UniScanG.Screen.Data
{
    public class RegionInfoS : UniScanG.Data.Vision.RegionInfo
    {
        int meanValue;
        public int MeanValue
        {
            get { return meanValue; }
            set { meanValue = value; }
        }

        int poleValue;
        public int PoleValue
        {
            get { return poleValue; }
            set { poleValue = value; }
        }

        int dielectricValue;
        public int DielectricValue
        {
            get { return dielectricValue; }
            set { dielectricValue = value; }
        }

        public override UniScanG.Data.Vision.RegionInfo Clone()
        {
            RegionInfoS clone = new RegionInfoS();
            clone.Copy(this);
            return clone;
        }

        public override void Copy(UniScanG.Data.Vision.RegionInfo srcRegionInfo)
        {
            base.Copy(srcRegionInfo);

            RegionInfoS regionInfo = srcRegionInfo as RegionInfoS;

            this.region = regionInfo.region;
            this.meanValue = regionInfo.meanValue;
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
            
            XmlHelper.SetValue(algorithmElement, "MeanValue", meanValue.ToString());
            XmlHelper.SetValue(algorithmElement, "PoleValue", poleValue.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricValue", dielectricValue.ToString());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);
            
            meanValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MeanValue", "0"));
            poleValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "PoleValue", "0"));
            dielectricValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DielectricValue", "0"));
        }

        public override Figure GetFigure()
        {
            return new RectangleFigure(region, new Pen(Color.LightBlue, 50));
        }
    }
}

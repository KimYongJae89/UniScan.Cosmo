using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Drawing;
using System.Xml;

namespace UniScanG.Data.Vision
{
    public abstract class RegionInfo
    {
        protected bool use = true;
        public bool Use
        {
            get { return this.use; }
            set { this.use = value; }
        }

        protected Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        public RegionInfo()
        {
            this.use = false;
            this.region = Rectangle.Empty;
        }

        public RegionInfo(Rectangle region)
        {
            this.use = true;
            this.region = region;
        }

        public abstract RegionInfo Clone();

        public virtual void Copy(RegionInfo srcRegionInfo)
        {
            this.use = srcRegionInfo.use;
            this.region = srcRegionInfo.region;
        }
    
        public virtual void Dispose() { }

        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "Use", use.ToString());
            XmlHelper.SetValue(algorithmElement, "X", region.X.ToString());
            XmlHelper.SetValue(algorithmElement, "Y", region.Y.ToString());
            XmlHelper.SetValue(algorithmElement, "Width", region.Width.ToString());
            XmlHelper.SetValue(algorithmElement, "Height", region.Height.ToString());
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            use = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "Use", use.ToString()));
            int x = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "X", "0"));
            int y = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Y", "0"));
            int width = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Width", "0"));
            int height = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Height", "0"));

            region = new Rectangle(x, y, width, height);
        }

        public abstract Figure GetFigure();
    }
}

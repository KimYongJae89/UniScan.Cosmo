using DynMvp.Base;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UniScanG.Gravure.Data;

namespace UniScanG.Data.Model
{
    public class Model : UniScan.Common.Data.Model
    {
        public int ScaleFactor
        {
            get { return scaleFactor; }
            set { scaleFactor = value; }
        }

        private int scaleFactor = 1;

        public float ScaleFactorF { get => 1f / this.scaleFactor;
        }
        public Model()
        {

        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            XmlHelper.SetValue(xmlElement, "ScaleFactor", this.scaleFactor);
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            this.scaleFactor = XmlHelper.GetValue(xmlElement, "ScaleFactor", this.scaleFactor);
        }

        public new ModelDescription ModelDescription
        {
            get { return (ModelDescription)modelDescription; }
        }

        public Bitmap GetPreviewImage()
        {
            return SystemManager.Instance().ModelManager.GetPreviewImage((ModelDescription)modelDescription);
        }
    }
}
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Data;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class MultipleProduction : UniEye.Base.Data.Production
    {
        Production pinHoleProduction;
        Production colorProduction;

        public Production PinHoleProduction { get => pinHoleProduction; set => pinHoleProduction = value; }
        public Production ColorProduction { get => colorProduction; set => colorProduction = value; }

        object[] brushes;
        public object[] Brushes
        {
            get
            {
                return brushes;
            }
        }
        
        public MultipleProduction(string name = "", string lotNo = "") : base(name, lotNo)
        {
            pinHoleProduction = new Production(name, lotNo);
            colorProduction = new Production(name, lotNo);

            brushes = new object[Name.Length];

            for (int i = 0; i < Name.Length; i++)
            {
                if (Name[i] == '0')
                    brushes[i] = App.Current.Resources["RedBrush"];
                else
                    brushes[i] = App.Current.Resources["GreenBrush"];
            }
        }

        public MultipleProduction(XmlElement productionElement) : base(productionElement)
        {
            brushes = new object[Name.Length];
            for (int i = 0; i < Name.Length; i++)
            {
                if (Name[i] == '0')
                    brushes[i] = App.Current.Resources["RedBrush"];
                else
                    brushes[i] = App.Current.Resources["GreenBrush"];
            }
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            pinHoleProduction = new Production(Name, LotNo);
            XmlElement pinHoleElement = productionElement["PinHole"];
            if (pinHoleElement != null)
                pinHoleProduction.Load(pinHoleElement);

            colorProduction = new Production(Name, LotNo);
            XmlElement colorElement = productionElement["Color"];
            if (colorElement != null)
                colorProduction.Load(colorElement);
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlElement pinHoleElement = productionElement.OwnerDocument.CreateElement("", "PinHole", "");
            productionElement.AppendChild(pinHoleElement);
            pinHoleProduction.Save(pinHoleElement);

            XmlElement colorElement = productionElement.OwnerDocument.CreateElement("", "Color", "");
            productionElement.AppendChild(colorElement);
            colorProduction.Save(colorElement);
        }

        public override void Reset()
        {
            base.Reset();
            colorProduction.Reset();
            pinHoleProduction.Reset();
        }
    }
}

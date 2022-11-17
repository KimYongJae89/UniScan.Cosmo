using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace UniScanWPF.Table.Inspect
{
    public enum ReferencePos {None=-1, LT, RT, RB, LB};
    public class MarginMeasurePoint
    {
        ReferencePos referencePos;
        Point point;

        public Point Point { get => point; set => point = value; }
        public ReferencePos ReferencePos { get => referencePos; set => referencePos = value; }

        public MarginMeasurePoint()
        {
            this.referencePos = ReferencePos.None;
            this.point = new Point(0,0);
        }

        public MarginMeasurePoint(ReferencePos referencePos, Point point)
        {
            this.referencePos = referencePos;
            this.point = point;
        }

        internal void Save(XmlElement xmlElement, string key="")
        {
            if(string.IsNullOrEmpty(key)==false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "ReferencePos", referencePos.ToString());
            XmlHelper.SetValue(xmlElement, "Point.X", point.X);
            XmlHelper.SetValue(xmlElement, "Point.Y", point.Y );
        }

        internal static MarginMeasurePoint Load(XmlElement xmlElement)
        {
            MarginMeasurePoint marginMeasurePoint = new MarginMeasurePoint();
            marginMeasurePoint.referencePos = (ReferencePos)XmlHelper.GetValue(xmlElement, "ReferencePos", ReferencePos.None);
            marginMeasurePoint.point.X = XmlHelper.GetValue(xmlElement, "Point.X", 0.0);
            marginMeasurePoint.point.Y = XmlHelper.GetValue(xmlElement, "Point.Y", 0.0);
            return marginMeasurePoint;
        }
    }


}

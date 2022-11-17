using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Data
{
    public enum PinHoleDefectType
    {
        PinHole, Dust
    }

    public class PinHoleDefect : Defect
    {
        PinHoleDefectType type;
        float avgValue;
        public PinHoleDefectType Type { get => type; }
        public string TypeName { get => type.ToString(); }
        public string AvgValue { get => string.Format("{0:0.00}", avgValue); }

        public PinHoleDefect(int index, float avgValue, BitmapSource image, System.Drawing.Rectangle rectangle, PinHoleDefectType type)
        {
            this.index = index;
            this.avgValue = avgValue;
            this.image = image;
            this.rectangle = rectangle;
            this.type = type;
        }

        public PinHoleDefect(string resultPath, int index, System.Drawing.Rectangle rectangle, XmlElement defectElement)
        {
            this.resultPath = resultPath;
            this.index = index;
            this.rectangle = rectangle;

            this.type = (PinHoleDefectType)Enum.Parse(typeof(PinHoleDefectType), XmlHelper.GetValue(defectElement, "Type", type.ToString()));
            this.avgValue = Convert.ToSingle(XmlHelper.GetValue(defectElement, "AvgValue", "0"));
        }

        public override void ExportResult(string resultPath, XmlElement defectElement)
        {
            base.ExportResult(resultPath, defectElement);

            XmlHelper.SetValue(defectElement, "Type", type.ToString());
            XmlHelper.SetValue(defectElement, "AvgValue", avgValue.ToString());
        }
    }
}

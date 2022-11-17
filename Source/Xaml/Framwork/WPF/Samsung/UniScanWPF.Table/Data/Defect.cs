using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Data
{
    public enum DefectType { Pattern, Margin, Shape }

    public abstract class Defect: IResultObject
    {
        DefectType defectType;
        BitmapSource image;
        BlobRect defectBlob;
        protected abstract float GetDiffValue();

        public float Length { get => defectBlob.RotateWidth * DeveloperSettings.Instance.Resolution; }
        public BlobRect DefectBlob { get => defectBlob; }
        public BitmapSource Image { get => image; }
        public DefectType DefectType { get => defectType; }
        public float DiffValue { get => GetDiffValue(); }

        public Enum ResultObjectType { get => this.defectType; }

        protected Defect(DefectType defectType, BlobRect defectBlob, BitmapSource image)
        {
            this.defectType = defectType;
            this.defectBlob = defectBlob;
            this.image = image;
        }

        protected Defect(DefectType defectType, BitmapSource image, XmlElement defectElement)
        {
            this.defectType = defectType;
            this.image = image;

            BlobRect defectBlob = new BlobRect();
            defectBlob.RotateXArray = new float[4];
            defectBlob.RotateYArray = new float[4];

            defectBlob.RotateWidth = XmlHelper.GetValue(defectElement, "RotateWidth", 0.0f);
            this.defectBlob = defectBlob;

            for (int i = 0; i < 4; i++)
            {
                defectBlob.RotateXArray[i] = XmlHelper.GetValue(defectElement, string.Format("X{0}", i), 0.0f);
                defectBlob.RotateYArray[i] = XmlHelper.GetValue(defectElement, string.Format("Y{0}", i), 0.0f);
            }

            defectBlob.BoundingRect = System.Drawing.RectangleF.FromLTRB(
                defectBlob.RotateXArray.Min(),
                defectBlob.RotateYArray.Min(),
                defectBlob.RotateXArray.Max(),
                defectBlob.RotateYArray.Max());
        }

        public static IResultObject CreateDefect(BitmapSource image, XmlElement defectElement)
        {
            IResultObject defect = null;
            string typeString = XmlHelper.GetValue(defectElement, "Type", "");
            if (Enum.IsDefined(typeof(DefectType), typeString))
            {
                DefectType type = (DefectType)Enum.Parse(typeof(DefectType), typeString);

                switch (type)
                {
                    case DefectType.Pattern:
                        defect = new PatternDefect(image, defectElement);
                        break;
                    case DefectType.Margin:
                        defect = new MarginDefect(image, defectElement);
                        break;
                    case DefectType.Shape:
                        defect = new ShapeDefect(image, defectElement);
                        break;
                    default:
                        return null;
                }
            }else if(Enum.IsDefined(typeof(MeasureType), typeString))
            {
                MeasureType type = (MeasureType)Enum.Parse(typeof(MeasureType), typeString);
                switch (type)
                {
                    case MeasureType.Length:
                        defect = new LengthMeasure(defectElement);
                        break;
                    case MeasureType.Extra:
                        defect = new ExtraMeasure(defectElement);
                        break;
                    case MeasureType.Meander:
                        defect = new MeanderMeasure(defectElement);
                        break;
                }
            }
            return defect;
        }

        public abstract System.Windows.Media.SolidColorBrush GetBrush();
        public static System.Windows.Media.SolidColorBrush GetBrush(DefectType defectType)
        {
            System.Windows.Media.SolidColorBrush brush = null;
            switch (defectType)
            {
                case DefectType.Pattern:
                    brush = System.Windows.Media.Brushes.Red;
                    break;
                case DefectType.Margin:
                    brush = System.Windows.Media.Brushes.Blue;
                    break;
                case DefectType.Shape:
                    brush = System.Windows.Media.Brushes.Yellow;
                    break;
            }
            return brush;
        }
        public Rect GetRect(double scale)
        {
            return new System.Windows.Rect(this.defectBlob.BoundingRect.X / scale, this.defectBlob.BoundingRect.Y / scale,
                this.defectBlob.BoundingRect.Width / scale, this.defectBlob.BoundingRect.Height / scale);
        }

        public Point[] GetPoints(double scale)
        {
            return new Point[]
                 { new Point(this.defectBlob.RotateXArray[0] / scale, this.defectBlob.RotateYArray[0] / scale),
                        new Point(this.defectBlob.RotateXArray[1] / scale, this.defectBlob.RotateYArray[1] / scale),
                        new Point(this.defectBlob.RotateXArray[2] / scale, this.defectBlob.RotateYArray[2] / scale),
                 new Point(this.defectBlob.RotateXArray[3] / scale, this.defectBlob.RotateYArray[3] / scale)};
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Type", this.DefectType.ToString());
            XmlHelper.SetValue(xmlElement, "RotateWidth", this.DefectBlob.RotateWidth.ToString());
            XmlHelper.SetValue(xmlElement, "Diff", this.DiffValue);
        }

        public BitmapSource GetBitmapSource()
        {
            return this.image;
        }
    }

    public class PatternDefect : Defect
    {
        float diffValue;
        public PatternDefect(BlobRect defectBlob, BitmapSource image, float diffValue) : base(DefectType.Pattern, defectBlob, image)
        {
            this.diffValue = diffValue;
        }

        public PatternDefect(BitmapSource image, XmlElement defectElement) : base(DefectType.Pattern, image, defectElement)
        {
            diffValue = XmlHelper.GetValue(defectElement, "Diff", 0.0f);
        }

        protected override float GetDiffValue()
        {
            return diffValue;
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Pattern);
        }
    }

    public class MarginDefect : Defect
    {
        float diffValue;
        public MarginDefect(BlobRect defectBlob, BitmapSource image, float diffValue) : base(DefectType.Margin, defectBlob, image)
        {
            this.diffValue = diffValue;
        }

        public MarginDefect(BitmapSource image, XmlElement defectElement) : base(DefectType.Margin, image, defectElement)
        {
            diffValue = XmlHelper.GetValue(defectElement, "Diff", 0.0f);
        }

        protected override float GetDiffValue()
        {
            return diffValue;
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Margin);
        }
    }

    public class ShapeDefect : Defect
    {
        List<Tuple<PatternFeature, float>> minDiffList;

        public ShapeDefect(BlobRect defectBlob, BitmapSource image, List<Tuple<PatternFeature, float>> minDiffList) : base(DefectType.Shape, defectBlob, image)
        {
            this.minDiffList = minDiffList;
        }

        public ShapeDefect(BitmapSource image, XmlElement defectElement) : base(DefectType.Shape, image, defectElement)
        {
            minDiffList = new List<Tuple<PatternFeature, float>>();
            minDiffList.Add(new Tuple<PatternFeature, float>(PatternFeature.MajorLength, XmlHelper.GetValue(defectElement, "Diff", 0.0f)));
        }

        protected override float GetDiffValue()
        {
            return minDiffList.Max(diff => diff.Item2) * DeveloperSettings.Instance.Resolution;
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Defect.GetBrush(DefectType.Shape);
        }
    }

}

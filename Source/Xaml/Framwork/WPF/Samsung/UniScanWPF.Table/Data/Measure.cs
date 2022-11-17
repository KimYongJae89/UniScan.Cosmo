using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Data
{
    public enum MeasureType { Length, Extra, Meander }

    public abstract class Measure : IResultObject
    {
        MeasureType measureType;
        public MeasureType MeasureType { get => measureType;}
        public Measure(MeasureType measureType)
        {
            this.measureType = measureType;
        }

        public abstract System.Windows.Media.SolidColorBrush GetBrush();
        public static System.Windows.Media.SolidColorBrush GetBrush(MeasureType measureType)
        {
            System.Windows.Media.SolidColorBrush brush = null;
            switch (measureType)
            {
                case MeasureType.Length:
                    brush = System.Windows.Media.Brushes.Green;
                    break;
                case MeasureType.Extra:
                    brush = System.Windows.Media.Brushes.Brown;
                    break;
                case MeasureType.Meander:
                    brush = System.Windows.Media.Brushes.Brown;
                    break;
            }
            return brush;
        }

        
        public abstract Rect GetRect(double scale);
        public abstract Point[] GetPoints(double scale);

        public Enum ResultObjectType { get => this.measureType; }

        public abstract void Save(XmlElement xmlElement);

        public BitmapSource GetBitmapSource()
        {
            return null;
        }
    }

    // 시트 길이 측정 결과
    public class LengthMeasure : Measure
    {
        Direction direction;
        Point srcPt;
        Point dstPt;
        double lengthMm;
        bool isValid;

        public Point SrcPt { get => srcPt; set => srcPt = value; }
        public Point DstPt { get => dstPt; set => dstPt = value; }
        public double LengthMm { get => lengthMm; set => lengthMm = value; }
        public bool IsValid { get => isValid; set => isValid = value; }

        public Direction Direction { get => direction; }

        public LengthMeasure(Direction direction, System.Drawing.PointF srcPt, System.Drawing.PointF dstPt, double lengthMm, bool isValid) : base(MeasureType.Length)
        {
            this.direction = direction;
            this.srcPt = new Point(srcPt.X, srcPt.Y);
            this.dstPt = new Point(dstPt.X, dstPt.Y);
            this.lengthMm = lengthMm;
            this.isValid = isValid;
        }

        public LengthMeasure(XmlElement xmlElement) : base(MeasureType.Length)
        {
            this.direction = (Direction)XmlHelper.GetValue(xmlElement, "Direction", this.direction);
            this.srcPt.X = XmlHelper.GetValue(xmlElement, "SrcPt.X", this.srcPt.X);
            this.srcPt.Y = XmlHelper.GetValue(xmlElement, "SrcPt.Y", this.srcPt.Y);
            this.dstPt.X = XmlHelper.GetValue(xmlElement, "DstPt.X", this.dstPt.X);
            this.dstPt.Y = XmlHelper.GetValue(xmlElement, "DstPt.Y", this.dstPt.Y);
            this.lengthMm = XmlHelper.GetValue(xmlElement, "LengthMm", this.lengthMm);
            this.isValid = XmlHelper.GetValue(xmlElement, "IsValid", this.isValid);
        }

        public override SolidColorBrush GetBrush()
        {
            return Measure.GetBrush(MeasureType.Length);
        }

        public override Point[] GetPoints(double scale)
        {
            //Rect rect = GetRect(scale);
            //return new Point[]
            //{
            //    new Point(rect.Left,rect.Top),
            //    new Point(rect.Right,rect.Top),
            //    new Point(rect.Right,rect.Bottom),
            //    new Point(rect.Left,rect.Bottom)
            //};
            return new Point[] { srcPt, dstPt };
        }

        public override Rect GetRect(double scale)
        {
            Rect rect = new Rect(srcPt, dstPt);
            if (rect.Width > rect.Height)
                rect.Inflate(0, 10);
            else
                rect.Inflate(10, 0);
            return rect;
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Type", this.MeasureType.ToString());
            XmlHelper.SetValue(xmlElement, "Direction", this.direction.ToString());
            XmlHelper.SetValue(xmlElement, "SrcPt.X", this.srcPt.X);
            XmlHelper.SetValue(xmlElement, "SrcPt.Y", this.srcPt.Y);
            XmlHelper.SetValue(xmlElement, "DstPt.X", this.dstPt.X);
            XmlHelper.SetValue(xmlElement, "DstPt.Y", this.dstPt.Y);
            XmlHelper.SetValue(xmlElement, "LengthMm", this.lengthMm);
            XmlHelper.SetValue(xmlElement, "IsValid", this.isValid);
        }
    }

    // 패턴-패턴간 거리 측정 결과
    public class ExtraMeasure : Measure
    {
        ReferencePos referencePos;
        System.Drawing.RectangleF dispRect;
        float[] values;

        public System.Drawing.RectangleF DispRect { get => dispRect; }
        public float[] Value { get => values; }

        public ReferencePos ReferencePos { get => this.referencePos; }
        public float Width { get => values.Length == 4 ? Math.Max(values[0], values[2]) : 0; }
        public float Height { get => values.Length == 4 ? Math.Max(values[1], values[3]) : 0; }

        public ExtraMeasure(ReferencePos referencePos, System.Drawing.RectangleF dispRect, float[] values) : base(MeasureType.Extra)
        {
            this.referencePos = referencePos;
            this.dispRect = dispRect;
            this.values = values;
        }

        public ExtraMeasure(XmlElement xmlElement) : base(MeasureType.Extra)
        {
            referencePos = (ReferencePos)XmlHelper.GetValue(xmlElement, "ReferencePos", ReferencePos.None);
            dispRect.X = XmlHelper.GetValue(xmlElement, "DispRect.X", dispRect.X);
            dispRect.Y = XmlHelper.GetValue(xmlElement, "DispRect.Y", dispRect.Y);
            dispRect.Width = XmlHelper.GetValue(xmlElement, "DispRect.W", dispRect.Width);
            dispRect.Height = XmlHelper.GetValue(xmlElement, "DispRect.H", dispRect.Height);

            XmlNodeList xmlNodeList=  xmlElement.GetElementsByTagName("Value");
            List<float> valueList = new List<float>();
            foreach (XmlElement subElement in xmlNodeList)
                valueList.Add(XmlHelper.GetValue(subElement, "", 0));
            this.values = valueList.ToArray();
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Measure.GetBrush(MeasureType.Extra);
        }

        public override Rect GetRect(double scale)
        {
            return new Rect(
                this.dispRect.X / scale,
                this.dispRect.Y / scale,
                this.dispRect.Width / scale,
                this.dispRect.Height / scale);
        }

        public override Point[] GetPoints(double scale)
        {
            Rect rect = GetRect(scale);
            return new Point[]
            {
                new Point(rect.Left,rect.Top),
                new Point(rect.Right,rect.Top),
                new Point(rect.Right,rect.Bottom),
                new Point(rect.Left,rect.Bottom)
            };
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Type", this.MeasureType.ToString());

            XmlHelper.SetValue(xmlElement, "ReferencePos", referencePos.ToString());
            XmlHelper.SetValue(xmlElement, "DispRect.X", dispRect.X);
            XmlHelper.SetValue(xmlElement, "DispRect.Y", dispRect.Y);
            XmlHelper.SetValue(xmlElement, "DispRect.W", dispRect.Width);
            XmlHelper.SetValue(xmlElement, "DispRect.H", dispRect.Height);

            Array.ForEach(values, f => XmlHelper.SetValue(xmlElement, "Value", f.ToString()));
        }
    }

    // 사행 측정 결과
    public class MeanderMeasure : Measure
    {
        float measurePosY;
        float sheetPos;
        float coatPos;
        float printPos;

        float[] rotateXArray;
        float[] rotateYArray;

        public float MeasurePos { get => measurePosY; }
        public float SheetPos { get => sheetPos; }
        public float CoatPos { get => coatPos; }
        public float PrintPos { get => printPos; }
        public float SheetPrintDistMm { get => (printPos - sheetPos) * DeveloperSettings.Instance.Resolution / 1000; }
        public float CoatPrintDistMm { get => (printPos - coatPos) * DeveloperSettings.Instance.Resolution / 1000; }

        public MeanderMeasure(float measurePosY, float sheetPos, float coatPos, float printPos) : base(MeasureType.Meander)
        {
            this.measurePosY = measurePosY;
            this.sheetPos = sheetPos;
            this.coatPos = coatPos;
            this.printPos = printPos;

            this.rotateXArray = new float[2] { sheetPos, printPos };
            this.rotateYArray = new float[2] { measurePosY, measurePosY };
        }

        public MeanderMeasure(XmlElement xmlElement) : base(MeasureType.Meander)
        {
            this.measurePosY = XmlHelper.GetValue(xmlElement, "MeasurePosY", this.measurePosY);
            this.sheetPos = XmlHelper.GetValue(xmlElement, "SheetPos", this.sheetPos);
            this.coatPos = XmlHelper.GetValue(xmlElement, "CoatPos", this.coatPos);
            this.printPos = XmlHelper.GetValue(xmlElement, "PrintPos", this.printPos);

            this.rotateXArray = new float[4];
            this.rotateYArray = new float[4];
            for (int i = 0; i < 4; i++)
            {
                rotateXArray[i] = XmlHelper.GetValue(xmlElement, string.Format("X{0}", i), 0.0f);
                rotateYArray[i] = XmlHelper.GetValue(xmlElement, string.Format("Y{0}", i), 0.0f);
            }
        }

        public override System.Windows.Media.SolidColorBrush GetBrush()
        {
            return Measure.GetBrush(MeasureType.Meander);
        }

        public override Rect GetRect(double scale)
        {
            return new Rect(new Point(rotateXArray[0] / scale, rotateYArray[0] / scale),
                new Point(rotateXArray[1] / scale, rotateYArray[1] / scale));
        }

        public override Point[] GetPoints(double scale)
        {
            if (rotateXArray.Length == 2)
            {
                return new Point[]
                {
                new Point(rotateXArray[0]/ scale,(rotateYArray[0])/ scale),
                new Point(rotateXArray[0]/ scale,(rotateYArray[0])/ scale),
                new Point(rotateXArray[1]/ scale,(rotateYArray[1])/ scale),
                new Point(rotateXArray[1]/ scale,(rotateYArray[1])/ scale)
                };
            }
            else
            {
                return new Point[]
                                {
                new Point(rotateXArray[0]/ scale,(rotateYArray[0])/ scale),
                new Point(rotateXArray[1]/ scale,(rotateYArray[1])/ scale),
                new Point(rotateXArray[2]/ scale,(rotateYArray[2])/ scale),
                new Point(rotateXArray[3]/ scale,(rotateYArray[3])/ scale)
                                };
            }
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Type", this.MeasureType.ToString());
            XmlHelper.SetValue(xmlElement, "MeasurePosY", this.measurePosY);
            XmlHelper.SetValue(xmlElement, "SheetPos", this.sheetPos);
            XmlHelper.SetValue(xmlElement, "CoatPos", this.coatPos);
            XmlHelper.SetValue(xmlElement, "PrintPos", this.printPos);
        }
    }
}

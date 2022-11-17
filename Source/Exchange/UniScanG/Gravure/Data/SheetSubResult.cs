using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data;
using UniScanG.Data.Vision;
using UniScanG.Screen.Vision;
using UniScanG.Vision;

namespace UniScanG.Gravure.Data
{
    public enum PositionType { Pole, Dielectric }
    public enum ValueType { Bright, Dark }
    public enum ShapeType { Circular, Linear }

    public static class ColorTable
    {
        public static Color GetBgColor(DefectType defectType)
        {
            Color color = Color.Empty;

            switch (defectType)
            {
                case DefectType.Noprint:
                    color = Color.DarkBlue;
                    break;
                case DefectType.PinHole:
                    color = Color.DarkBlue;
                    break;
            }

            return color;
        }

        public static Color GetColor(DefectType defectType)
        {
            Color color = Color.Empty;

            switch (defectType)
            {
                case DefectType.Unknown:
                    color = Color.Black;
                    break;
                case DefectType.SheetAttack:
                    color = Color.Brown;
                    break;
                case DefectType.Noprint:
                    color = Color.Yellow;
                    break;
                case DefectType.Dielectric:
                    color = Color.Blue;
                    break;
                case DefectType.PinHole:
                    color = Color.Cyan;
                    break;
                case DefectType.Total:
                    color = Color.Red;
                    break;
            }

            return color;
        }

        public static void UpdateControlColor(Control control, DefectType defectType)
        {
            Color fColor = Gravure.Data.ColorTable.GetColor(defectType);
            Color bColor = Gravure.Data.ColorTable.GetBgColor(defectType);
            UpdateControlColor(control, fColor, bColor);
        }

        public static void UpdateControlColor(Control checkBox, Color fColor, Color bColor)
        {
            if (fColor.IsEmpty == false)
                checkBox.ForeColor = fColor;

            if (bColor.IsEmpty == false)
                checkBox.BackColor = bColor;
        }
    }
    public class SheetSubResult : UniScanG.Data.SheetSubResult
    {
        private enum ExportOrderBase { CamIndex, ImageIndex, DefectTypePos, DefectTypeValue, DefectTypeShape, X, RealX, Y, RealY, W, RealW, H, RealH, MAX_COUNT } //13
        private enum ExportOrder { Base, DiffValueLow, DiffValueHigh, Compactness, SubtractValueMin, SubtractValueMax, FillRate, MAX_COUNT} //4
        
        bool isValid = true;
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        RectangleF avgIntensity;
        public RectangleF AvgIntensity
        {
            get { return avgIntensity; }
            set { avgIntensity = value; }
        }

        int subtractValueMin = -1;
        public int SubtractValueMin
        {
            get { return subtractValueMin; }
            set { subtractValueMin = value; }
        }

        int subtractValueMax = -1;
        public int SubtractValueMax
        {
            get { return subtractValueMax; }
            set { subtractValueMax = value; }
        }

        PositionType positionType;
        public PositionType PositionType
        {
            get { return positionType; }
            set { positionType = value; }
        }

        ValueType valueType;
        public ValueType ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }

        ShapeType shapeType;
        public ShapeType ShapeType
        {
            get { return shapeType; }
            set { shapeType = value; }
        }

        public SheetSubResult():base()
        {

        }

        public void Offset(Point point, int scaleFactor, SizeF pelSize)
        {
            PointF realOffset = new PointF(point.X * pelSize.Width*1E-3f, point.Y * pelSize.Height * 1E-3f);
            resultRect.Offset(point);

            region.Offset(point);
            realCenterPos = PointF.Add(realCenterPos, new SizeF(realOffset));
            realRegion.Offset(realOffset);
        }

        public override string GetExportHeader()
        {
            StringBuilder sb = new StringBuilder();
            List<string> headerItemList = new List<string>();

            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base:
                        headerItemList.Add(string.Format("{0},", GetExportBaseHeader()));
                        break;
                    case ExportOrder.MAX_COUNT:
                        break;
                    default:
                        headerItemList.Add(string.Format("{0},", e.ToString()));
                        break;
                }
            }

            return string.Join(",",headerItemList.ToArray()).Trim(',');
        }
        private string GetExportBaseHeader()
        {
            StringBuilder sb = new StringBuilder();
            List<string> headerItemList = new List<string>();

            Array array = Enum.GetValues(typeof(ExportOrderBase));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrderBase.MAX_COUNT:
                        break;
                    default:
                        headerItemList.Add(string.Format("{0},", e.ToString()));
                        break;
                }
            }

            return string.Join(",", headerItemList.ToArray()).Trim(',');
        }


        protected string ToBaseExportString()
        {
            string[] exportArray = new string[(int)ExportOrderBase.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrderBase));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrderBase.CamIndex: exportArray[(int)ExportOrderBase.CamIndex] = string.Format("{0}", camIndex.ToString()); break;
                    case ExportOrderBase.ImageIndex: exportArray[(int)ExportOrderBase.ImageIndex] = string.Format("{0}", index.ToString()); break;
                    case ExportOrderBase.DefectTypePos: exportArray[(int)ExportOrderBase.DefectTypePos] = string.Format("{0}", positionType.ToString()); break;
                    case ExportOrderBase.DefectTypeValue: exportArray[(int)ExportOrderBase.DefectTypeValue] = string.Format("{0}", valueType.ToString()); break;
                    case ExportOrderBase.DefectTypeShape: exportArray[(int)ExportOrderBase.DefectTypeShape] = string.Format("{0}", shapeType.ToString()); break;
                    case ExportOrderBase.X: exportArray[(int)ExportOrderBase.X] = string.Format("{0}", region.X); break;
                    case ExportOrderBase.RealX: exportArray[(int)ExportOrderBase.RealX] = string.Format("{0}", realRegion.X / 1000f); break;
                    case ExportOrderBase.Y: exportArray[(int)ExportOrderBase.Y] = string.Format("{0}", region.Y); break;
                    case ExportOrderBase.RealY: exportArray[(int)ExportOrderBase.RealY] = string.Format("{0}", realRegion.Y / 1000f); break;
                    case ExportOrderBase.W: exportArray[(int)ExportOrderBase.W] = string.Format("{0}", region.Width); break;
                    case ExportOrderBase.RealW: exportArray[(int)ExportOrderBase.RealW] = string.Format("{0}", realRegion.Width); break;
                    case ExportOrderBase.H: exportArray[(int)ExportOrderBase.H] = string.Format("{0}", region.Height); break;
                    case ExportOrderBase.RealH: exportArray[(int)ExportOrderBase.RealH] = string.Format("{0}", realRegion.Height); break;
                }
            }
            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        protected bool FromBaseExportData(string line)
        {
            List<string> resultList = new List<string>();
            resultList.AddRange(line.Split(new char[] { '\t', ',' }));
            resultList.RemoveAll(s => s == "");
            if (resultList.Count < (int)ExportOrderBase.MAX_COUNT)
                return false;

            try
            {
                camIndex = Convert.ToInt32(resultList[(int)ExportOrderBase.CamIndex]);
                index = Convert.ToInt32(resultList[(int)ExportOrderBase.ImageIndex]);
                positionType = (PositionType)Enum.Parse(typeof(PositionType), resultList[(int)ExportOrderBase.DefectTypePos]);
                valueType = (ValueType)Enum.Parse(typeof(ValueType), resultList[(int)ExportOrderBase.DefectTypeValue]);
                shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), resultList[(int)ExportOrderBase.DefectTypeShape]);
                region.X = Convert.ToInt32(resultList[(int)ExportOrderBase.X]);
                realRegion.X = Convert.ToSingle(resultList[(int)ExportOrderBase.RealX]) * 1000.0f;
                region.Y = Convert.ToInt32(resultList[(int)ExportOrderBase.Y]);
                realRegion.Y = Convert.ToSingle(resultList[(int)ExportOrderBase.RealY]) * 1000.0f;
                region.Width = Convert.ToInt32(resultList[(int)ExportOrderBase.W]);
                realRegion.Width = Convert.ToSingle(resultList[(int)ExportOrderBase.RealW]);
                region.Height = Convert.ToInt32(resultList[(int)ExportOrderBase.H]);
                realRegion.Height = Convert.ToSingle(resultList[(int)ExportOrderBase.RealH]);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public override string ToExportData()
        {
            string[] exportArray = new string[(int)ExportOrder.MAX_COUNT];
            Array array = Enum.GetValues(typeof(ExportOrder));
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base: exportArray[(int)ExportOrder.Base] = string.Format("{0}", ToBaseExportString()); break;
                    case ExportOrder.DiffValueLow: exportArray[(int)ExportOrder.DiffValueLow] = string.Format("{0}", lowerDiffValue); break;
                    case ExportOrder.DiffValueHigh: exportArray[(int)ExportOrder.DiffValueHigh] = string.Format("{0}", upperDiffValue); break;
                    case ExportOrder.Compactness: exportArray[(int)ExportOrder.Compactness] = string.Format("{0}", compactness); break;
                    case ExportOrder.SubtractValueMin: exportArray[(int)ExportOrder.SubtractValueMin] = string.Format("{0}", subtractValueMin); break;
                    case ExportOrder.SubtractValueMax: exportArray[(int)ExportOrder.SubtractValueMax] = string.Format("{0}", subtractValueMax); break;
                    case ExportOrder.FillRate: exportArray[(int)ExportOrder.FillRate] = string.Format("{0}", fillRate); break;
                }
            }

            string ss = string.Join(",", exportArray).Trim(',');
            return ss;
        }

        public override bool FromExportData(string line)
        {
            int baseCount = (int)ExportOrderBase.MAX_COUNT - 1;

            List<string> resultList = new List<string>();
            resultList.AddRange(line.Split(new char[] { '\t', ',' }));
            resultList.RemoveAll(s => string.IsNullOrEmpty(s));
            if (resultList.Count < (int)ExportOrderBase.MAX_COUNT + (int)ExportOrder.MAX_COUNT - 2)
                return false;

            Array array = Enum.GetValues(typeof(ExportOrder));
            bool ok = true;
            foreach (Enum e in array)
            {
                switch (e)
                {
                    case ExportOrder.Base: ok = FromBaseExportData(line); break;
                    case ExportOrder.DiffValueLow: lowerDiffValue = Convert.ToSingle(resultList[baseCount + (int)ExportOrder.DiffValueLow]); break;
                    case ExportOrder.DiffValueHigh: upperDiffValue = Convert.ToSingle(resultList[baseCount + (int)ExportOrder.DiffValueHigh]); break;
                    case ExportOrder.Compactness: compactness = Convert.ToSingle(resultList[baseCount + (int)ExportOrder.Compactness]); break;
                    case ExportOrder.SubtractValueMin:
                        if (resultList.Count > baseCount + (int)ExportOrder.SubtractValueMin)
                            subtractValueMin = (int)Convert.ToSingle(resultList[baseCount + (int)ExportOrder.SubtractValueMin]);
                        break;
                    case ExportOrder.SubtractValueMax:
                        if (resultList.Count > baseCount + (int)ExportOrder.SubtractValueMax)
                            subtractValueMax = (int)Convert.ToSingle(resultList[baseCount + (int)ExportOrder.SubtractValueMax]);
                        break;
                    case ExportOrder.FillRate:
                        if (resultList.Count > baseCount + (int)ExportOrder.FillRate)
                            fillRate = Convert.ToSingle(resultList[baseCount + (int)ExportOrder.FillRate]);
                        break;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(this.GetDefectTypeName());
            sb.AppendLine(string.Format("POS: {0:F2} / {1:F2} [mm]", realRegion.X / 1000.0f, realRegion.Y / 1000.0f));
            sb.AppendLine(string.Format("SIZE: {0:F1} / {1:F1} [um]", realRegion.Width, realRegion.Height));
            if (avgIntensity.IsEmpty == false)
            {
                sb.AppendLine(string.Format("IL,IR: {0:0.00}, {1:0.00}[lv]", avgIntensity.Left, avgIntensity.Right));
                sb.AppendLine(string.Format("IT,IB: {0:0.00}, {1:0.00}[lv]", avgIntensity.Top, avgIntensity.Bottom));
            }

            sb.AppendLine(string.Format("V: {0:F0} / {1:F0}[lv]", subtractValueMin, subtractValueMax));
            sb.AppendLine(string.Format("R: {0:F2}[%]", fillRate));
            //if (Math.Abs(lowerDiffValue) > lowerTh)
            //    message += string.Format("L : {0:0.00} ", lowerDiffValue);

            //if (Math.Abs(upperDiffValue) > upperTh)
            //    message += string.Format("U : {0:0.00} ", upperDiffValue);

            //message += string.Format("C : {0:0.00} ", compactness);
            return sb.ToString();
            return message;
        }

        public override DefectType GetDefectType()
        {
            //public enum PositionType { Pole, Dielectric }
            //public enum ValueType { Bright, Dark }
            //public enum ShapeType { Circular, Linear }

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(positionType.ToString());
            //sb.AppendLine(valueType.ToString());
            //return sb.ToString();

            float maxRect = Math.Max(this.region.Width, this.region.Height);
            float minRect = Math.Min(this.region.Width, this.region.Height);
            if (this.isValid == false)
                return DefectType.Unknown;

            int idx = 0;
            idx |= (int)positionType << 2;
            idx |= (int)valueType << 1;
            idx |= (int)shapeType << 0;
            DefectType defectType = DefectType.Unknown;
            switch (idx)
            {
                case 0: //000. 전극-명-원
                case 1: //001. 전극-명-선
                    defectType = DefectType.Noprint;
                    break;
                case 2: //010. 전극-암-원
                case 3: //011. 전극-암-선
                    defectType = DefectType.SheetAttack;
                    break;
                case 4: //100. 성형-명-원
                case 5: //101. 성형-명-선
                    defectType = DefectType.Dielectric;
                    break;
                case 6: //110. 성형-암-원
                case 7: //111. 성형-암-선
                    defectType = DefectType.PinHole;
                    //"Pinhole";
                    break;
            }

            return defectType;
        }

        public override string GetDefectTypeDiscription()
        {
            return string.Format("{0},{1},{2}", positionType, shapeType, valueType);
        }

        public override void ImportImage()
        {
            this.image = Image;
            this.bufImage = BufImage;
        }

        public override Color GetColor()
        {
            return ColorTable.GetColor(this.GetDefectType());
        }

        public override Color GetBgColor()
        {
            return ColorTable.GetBgColor(this.GetDefectType());
        }
    }
}

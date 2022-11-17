using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Data.Vision;
using UniScanG.Screen.Vision;
using UniScanG.Vision;

namespace UniScanG.Screen.Data
{
    public static class ColorTable
    {
        public static Color GetColor(DefectType defectType)
        {
            switch (defectType)
            {
                case DefectType.SheetAttack:
                    return Color.Maroon;
                case DefectType.PoleLine:
                    return Color.Red;
                case DefectType.PoleCircle:
                    return Color.OrangeRed;
                case DefectType.Dielectric:
                    return Color.Blue;
                case DefectType.PinHole:
                    return Color.DarkMagenta;
                case DefectType.Shape:
                    return Color.DarkGreen;
            }
            return Color.Black;
        }
    }

    public class SheetSubResult : UniScanG.Data.SheetSubResult
    {
        int lowerTh;
        int upperTh;

        Bitmap binaryImage;
        public Bitmap BinaryImage
        {
            get { return binaryImage; }
            set { binaryImage = value; }
        }

        DefectType defectType;
        public DefectType DefectType
        {
            get { return defectType; }
            set { defectType = value; }
        }

        public void SetThreshold(int lowerTh, int upperTh)
        {
            this.lowerTh = lowerTh;
            this.upperTh = upperTh;
        }

        protected string ToBaseExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t", camIndex.ToString(), index.ToString(), defectType.ToString());
            stringBuilder.AppendFormat("{0}\t{1}\t", region.X, region.X * AlgorithmSetting.Instance().XPixelCal / 1000.0f);
            stringBuilder.AppendFormat("{0}\t{1}\t", region.Y, region.Y * AlgorithmSetting.Instance().YPixelCal / 1000.0f);
            stringBuilder.AppendFormat("{0}\t{1}\t", region.Width, region.Width * AlgorithmSetting.Instance().XPixelCal / 1000.0f);
            stringBuilder.AppendFormat("{0}\t{1}", region.Height, region.Height * AlgorithmSetting.Instance().YPixelCal / 1000.0f);

            return stringBuilder.ToString();
        }

        public static DefectType GetDefectType(string line)
        {
            return (DefectType)Enum.Parse(typeof(DefectType), line);
        }

        protected bool FromBaseExportData(string line)
        {
            List<string> resultList = new List<string>();
            resultList.AddRange(line.Split('\t'));
            resultList.RemoveAll(s => s == "");
            if (resultList.Count < 9)
                return false;
            try
            {
                camIndex = Convert.ToInt32(resultList[0]);
                index = Convert.ToInt32(resultList[1]);
                defectType = (DefectType)Enum.Parse(typeof(DefectType), resultList[2]);
                region.X = Convert.ToInt32(resultList[3]);
                realRegion.X = Convert.ToSingle(resultList[4]);
                region.Y = Convert.ToInt32(resultList[5]);
                realRegion.Y = Convert.ToSingle(resultList[6]);
                region.Width = Convert.ToInt32(resultList[7]);
                realRegion.Width = Convert.ToSingle(resultList[8]);
                region.Height = Convert.ToInt32(resultList[9]);
                realRegion.Height = Convert.ToSingle(resultList[10]);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public override string GetExportHeader()
        {
            return "";
        }

        public override string ToExportData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\t", ToBaseExportString());
            stringBuilder.AppendFormat("{0:0.00}\t", lowerDiffValue);
            stringBuilder.AppendFormat("{0:0.00}\t", upperDiffValue);
            stringBuilder.AppendFormat("{0:0.00}\t", compactness);

            return stringBuilder.ToString();
        }

        public override bool FromExportData(string line)
        {
            bool ok = FromBaseExportData(line);
            if (ok == false)
                return false;

            List<string> resultList = new List<string>();
            resultList.AddRange(line.Split('\t'));
            resultList.RemoveAll(s => s == "");
            resultList = resultList.Skip(11).ToList();

            if (resultList.Count < 3)
                return false;

            try
            {
                lowerDiffValue = Convert.ToSingle(resultList[0]);
                upperDiffValue = Convert.ToSingle(resultList[1]);
                compactness = Convert.ToSingle(resultList[2]);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            string message = "";
            message += string.Format("X : {0}mm  \nY : {1}mm\n W : {2}um, H : {3}um\n", region.X * AlgorithmSetting.Instance().XPixelCal / 1000.0f, region.Y * AlgorithmSetting.Instance().YPixelCal / 1000.0f, region.Width * AlgorithmSetting.Instance().XPixelCal, region.Height * AlgorithmSetting.Instance().YPixelCal);

            if (Math.Abs(lowerDiffValue) > lowerTh)
                message += string.Format("L : {0:0.00} ", lowerDiffValue);

            if (Math.Abs(upperDiffValue) > upperTh)
                message += string.Format("U : {0:0.00} ", upperDiffValue);

            message += string.Format("C : {0:0.00} ", compactness);

            return message;
        }

        public override DefectType GetDefectType()
        {
            return defectType;
        }

        public override string GetDefectTypeDiscription()
        {
            return defectType.ToString();
        }

        public override void ImportImage()
        {
            
        }

        public override Color GetColor()
        {
            return ColorTable.GetColor(this.GetDefectType());
        }
        public override Color GetBgColor()
        {
            return SystemColors.Control;
        }
    }

    public class ShapeDiffValue
    {
        float diffTol;
        float heightDiffTol;

        public float SumDiff
        {
            get { return Math.Abs(areaDiff) + Math.Abs(widthDiff) + Math.Abs(heightDiff) + Math.Abs(centerXDiff) + Math.Abs(centerYDiff); }
        }
        
        SheetPattern similarPattern;
        public SheetPattern SimilarPattern
        {
            get { return similarPattern; }
            set { similarPattern = value; }
        }
        
        float areaDiff;
        public float AreaDiff
        {
            get { return areaDiff; }
            set { areaDiff = value; }
        }

        float widthDiff;
        public float WidthDiff
        {
            get { return widthDiff; }
            set { widthDiff = value; }
        }

        float heightDiff;
        public float HeightDiff
        {
            get { return heightDiff; }
            set { heightDiff = value; }
        }

        float centerXDiff;
        public float CenterXDiff
        {
            get { return centerXDiff; }
            set { centerXDiff = value; }
        }

        float centerYDiff;
        public float CenterYDiff
        {
            get { return centerYDiff; }
            set { centerYDiff = value; }
        }
        
        public ShapeDiffValue(bool init)
        {
            if (init == true)
            {
                areaDiff = float.MaxValue;
                widthDiff = float.MaxValue;
                heightDiff = float.MaxValue;
                centerXDiff = float.MaxValue;
                centerYDiff = float.MaxValue;
            }
        }

        public void SetTolerance(float diffTol, float heightDiffTol = -1)
        {
            this.diffTol = diffTol;
            this.heightDiffTol = heightDiffTol;
        }

        public string ToExportString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0:0.00}\t", areaDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", widthDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", heightDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", centerXDiff);
            stringBuilder.AppendFormat("{0:0.00}\t", centerYDiff);

            return stringBuilder.ToString();
        }

        public void FromExportData(List<string> resultList)
        {
            if (resultList.Count < 5)
                return;

            areaDiff = Convert.ToSingle(resultList[0]);
            widthDiff = Convert.ToSingle(resultList[1]);
            heightDiff = Convert.ToSingle(resultList[2]);
            centerXDiff = Convert.ToSingle(resultList[3]);
            centerYDiff = Convert.ToSingle(resultList[4]);
        }

        public override string ToString()
        {
            string message = "";

            if (Math.Abs(areaDiff) > diffTol)
                message += string.Format("A : {0:0.00} ", areaDiff);

            if (Math.Abs(widthDiff) > diffTol)
                message += string.Format("W : {0:0.00} ", widthDiff);


            if (heightDiffTol == -1)
            {
                if (Math.Abs(heightDiff) > diffTol)
                    message += string.Format("H : {0:0.00} ", heightDiff);
            }
            else
            {
                if (Math.Abs(heightDiff) > heightDiffTol)
                    message += string.Format("H : {0:0.00} ", heightDiff);
            }

            if (Math.Abs(centerXDiff) > diffTol)
                message += string.Format("Cx : {0:0.00} ", centerXDiff);

            if (Math.Abs(centerYDiff) > diffTol)
                message += string.Format("Cy : {0:0.00} ", centerYDiff);

            return message;
        }

        public bool IsDefect()
        {
            if (Math.Abs(areaDiff) > diffTol)
                return true;

            if (Math.Abs(widthDiff) > diffTol)
                return true;
                

            if (heightDiffTol == -1)
            {
                if (Math.Abs(heightDiff) > diffTol)
                    return true;
            }
            else
            {
                if (Math.Abs(heightDiff) > heightDiffTol)
                    return true;
            }

            if (Math.Abs(centerXDiff) > diffTol)
                return true;

            if (Math.Abs(centerYDiff) > diffTol)
                return true;

            return false;
        }
    }

    public class ShapeResult : SheetSubResult
    {
        public ShapeResult()
        {
            DefectType = DefectType.Shape;
        }

        ShapeDiffValue shapeDiffValue;
        public ShapeDiffValue ShapeDiffValue
        {
            get { return shapeDiffValue; }
            set { shapeDiffValue = value; }
        }

        public override string ToString()
        {
            string message = "";
            message += string.Format("X : {0}mm  Y : {1}mm\n W : {2}um, H : {3}um\n", Region.X * AlgorithmSetting.Instance().XPixelCal / 1000.0f, Region.Y * AlgorithmSetting.Instance().YPixelCal / 1000.0f, Region.Width * AlgorithmSetting.Instance().XPixelCal, Region.Height * AlgorithmSetting.Instance().YPixelCal);
            message += shapeDiffValue.ToString();

            return message;
        }

        public override string GetExportHeader()
        {
            return "";
        }

        public override string ToExportData()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0}\t", ToBaseExportString());
            stringBuilder.AppendFormat("{0}\t", shapeDiffValue.ToExportString());
            
            return stringBuilder.ToString();
        }

        public override bool FromExportData(string line)
        {
            bool ok = FromBaseExportData(line);
            if (ok == false)
                return false;
            
            List<string> resultList = new List<string>();
            resultList.AddRange(line.Split('\t'));
            resultList.RemoveAll(s => s == "");
            resultList = resultList.Skip(11).ToList();

            if (resultList.Count < 5)
                return false;

            shapeDiffValue = new ShapeDiffValue(false);
            shapeDiffValue.FromExportData(resultList);
            return true;
        }
    }
}

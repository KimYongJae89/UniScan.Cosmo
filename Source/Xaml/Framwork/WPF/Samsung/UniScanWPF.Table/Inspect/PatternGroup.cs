using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;

namespace UniScanWPF.Table.Inspect
{
    public enum PatternFeature
    {
        Area, MajorLength, MinorLength, SawToothArea
    }

    public class PatternGroup
    {
        float diagonal;
        public float Diagonal
        {
            get { return diagonal; }
        }

        int count;
        public int Count
        {
            get { return count; }
        }

        float averageArea;
        public float AverageArea
        {
            get { return averageArea; }
        }

        float averageMajorLength;
        public float AverageMajorLength
        {
            get { return averageMajorLength; }
        }

        float averageMinorLength;
        public float AverageMinorLength
        {
            get { return averageMinorLength; }
        }

        float averageSawToothLength;
        public float AverageSawToothLength
        {
            get { return (float)Math.Sqrt(averageSawToothLength); }
        }

        float averageRotateAngle;
        public float AverageRotateAngle
        {
            get { return averageRotateAngle; }
        }

        List<BlobRect> patternList = new List<BlobRect>();
        public List<BlobRect> PatternList
        {
            get { return patternList; }
        }
        
        public float SumArea
        {
            get { return averageArea * count; }
        }

        BitmapSource refImage;
        public BitmapSource RefImage { get => refImage; set => refImage = value; }

        public BlobRect GetAverageBlobRect()
        {
            return patternList[patternList.Count / 2];
        }

        public void AddPattern(BlobRect pattern)
        {
            patternList.Add(pattern);
            Calc();
        }

        public void AddPattern(List<BlobRect> patternList)
        {
            this.patternList.AddRange(patternList.ToArray());
        }
        
        private void Calc()
        {
            count = patternList.Count;

            if (patternList.Count == 1)
            {
                averageArea = patternList[0].Area;
                diagonal = (float)Math.Sqrt(averageArea);
                averageMajorLength = patternList[0].RotateWidth;
                averageMinorLength = patternList[0].RotateHeight;

                averageSawToothLength = patternList[0].SawToothArea;

                averageRotateAngle = patternList[0].RotateAngle;
                return;
            }

            averageArea = averageArea * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Area * (1.0f / patternList.Count);
            diagonal = (float)Math.Sqrt(averageArea);
            
            averageMajorLength = averageMajorLength * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateWidth * (1.0f / patternList.Count);
            averageMinorLength = averageMinorLength * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateHeight * (1.0f / patternList.Count);
            averageSawToothLength = averageSawToothLength * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().SawToothArea * (1.0f / patternList.Count);
            averageRotateAngle = averageRotateAngle * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateAngle * (1.0f / patternList.Count);
        }

        public void Clear()
        {
            patternList.Clear();
        }

        public FigureGroup CreateFigureGroup()
        {
            FigureGroup figureGroup = new FigureGroup();
            foreach (BlobRect blobRect in patternList)
                figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Yellow)));

            return figureGroup;
        }

        public float GetDiffValue(PatternFeature feature, BlobRect blobRect)
        {
            switch (feature)
            {
                case PatternFeature.Area:
                    return Math.Abs(diagonal - (float)Math.Sqrt(blobRect.Area));
                case PatternFeature.MajorLength:
                    return Math.Abs(averageMajorLength - blobRect.RotateWidth);
                case PatternFeature.MinorLength:
                    return Math.Abs(averageMinorLength - blobRect.RotateHeight);
                case PatternFeature.SawToothArea:
                    return 0;// (float)Math.Abs(Math.Sqrt(averageSawToothLength) - Math.Sqrt(blobRect.SawToothArea));
            }

            return 0;
        }

        private float GetDiffValue(PatternFeature feature, PatternGroup patternGroup, BlobRect blobRect)
        {
            float diffValue = 0;

            switch (feature)
            {
                case PatternFeature.Area:
                    diffValue = Math.Abs(patternGroup.diagonal - (float)Math.Sqrt(blobRect.Area));
                    break;
                case PatternFeature.MajorLength:
                    diffValue = Math.Abs(patternGroup.averageMajorLength - blobRect.RotateWidth);
                    break;
                case PatternFeature.MinorLength:
                    diffValue = Math.Abs(patternGroup.averageMinorLength - blobRect.RotateHeight);
                    break;
                case PatternFeature.SawToothArea:
                    diffValue = 0;//(float)Math.Abs(Math.Sqrt(patternGroup.averageSawToothLength) - Math.Sqrt(blobRect.SawToothArea));
                    break;
            }

            return diffValue;
        }

        public bool IsContain(Rectangle region)
        {
            foreach (BlobRect blobRect in  patternList)
            {
                if (region.Contains(Point.Round(blobRect.CenterPt)) == true)
                    return true;
            }

            return false;
        }

        private List<BlobRect> GetSortedList(PatternFeature feature, PatternGroup patternGroup)
        {
            List<BlobRect> sortedList = null;

            switch (feature)
            {
                case PatternFeature.Area:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.Area).ToList();
                    break;
                case PatternFeature.MajorLength:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.RotateWidth).ToList();
                    break;
                case PatternFeature.MinorLength:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.RotateHeight).ToList();
                    break;
                case PatternFeature.SawToothArea:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.SawToothArea).ToList();
                    break;
            }

            return sortedList;
        }

        private List<PatternGroup> DevideSubGroup(PatternFeature feature, PatternGroup patternGroup, float diffTol)
        {
            List<BlobRect> sortedList = GetSortedList(feature, patternGroup);

            List<PatternGroup> subPatternGroupList = new List<PatternGroup>();
            PatternGroup subPatternGroup = new PatternGroup();

            foreach (BlobRect blobRect in sortedList)
            {
                if (subPatternGroup.count == 0)
                {
                    subPatternGroup.AddPattern(blobRect);
                }
                else
                {
                    float diffValue = GetDiffValue(feature, subPatternGroup, blobRect);
                    if (diffValue <= diffTol)
                    {
                        subPatternGroup.AddPattern(blobRect);
                    }
                    else
                    {
                        if (subPatternGroup.patternList.Count > 0)
                            subPatternGroupList.Add(subPatternGroup);

                        subPatternGroup = new PatternGroup();
                        subPatternGroup.AddPattern(blobRect);
                    }
                }
            }

            if (subPatternGroup.patternList.Count > 0)
                subPatternGroupList.Add(subPatternGroup);

            return subPatternGroupList;
        }

        public List<PatternGroup> DivideSubGroup(float diffTol)
        {
            List<PatternGroup> patternGroupList = new List<PatternGroup>() { this };

            foreach (Enum feature in Enum.GetValues(typeof(PatternFeature)))
            {
                List<PatternGroup> temp = new List<PatternGroup>();
                foreach (PatternGroup patternGroup in patternGroupList)
                    temp.AddRange(DevideSubGroup((PatternFeature)feature, patternGroup, diffTol));

                temp = temp.OrderByDescending(x => x.count).ToList();
                patternGroupList = temp;
            }

            return patternGroupList;
        }

        public void Load(string path, int index, XmlElement xmlElement)
        {
            string imagePath = Path.Combine(path, string.Format("Image{0}.bmp", index));

            refImage = WPFImageHelper.LoadBitmapSource(imagePath);
            count = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Count", "0"));
            diagonal = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "Diagonal", "0"));
            averageArea = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageArea", "0"));
            averageMajorLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageMajorLength", "0"));
            averageMinorLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageMinorLength", "0"));
            averageSawToothLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageSawToothLength", "0"));
            averageRotateAngle = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageRotateAngle", "0"));
        }

        public void Save(string path, int index, XmlElement xmlElement)
        {
            if (refImage != null)
            {
                string imagePath = Path.Combine(path, string.Format("Image{0}.bmp", index));
                WPFImageHelper.SaveBitmapSource(imagePath, refImage);
            }
            XmlHelper.SetValue(xmlElement, "Count", count.ToString());
            XmlHelper.SetValue(xmlElement, "Diagonal", diagonal.ToString());
            XmlHelper.SetValue(xmlElement, "AverageArea", averageArea.ToString());
            XmlHelper.SetValue(xmlElement, "AverageMajorLength", averageMajorLength.ToString());
            XmlHelper.SetValue(xmlElement, "AverageMinorLength", averageMinorLength.ToString());
            XmlHelper.SetValue(xmlElement, "AverageSawToothLength", averageSawToothLength.ToString());
            XmlHelper.SetValue(xmlElement, "AverageRotateAngle", averageRotateAngle.ToString());
        }
    }
}

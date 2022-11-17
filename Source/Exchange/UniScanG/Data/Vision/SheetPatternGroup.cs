using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Data.Vision
{
    public enum PatternFeature
    {
        Area, Width, Height, Waist, CenterX, CenterY, CenterWidth
    }

    public class SheetPatternGroup
    {
        bool use;
        public bool Use
        {
            get { return use; }
            set { use = value; }
        }

        ImageD masterImage = null;
        public ImageD MasterImage
        {
            get { return masterImage; }
            set { masterImage = value; }
        }

        float countRatio;
        public float CountRatio
        {
            get { return countRatio; }
            set { countRatio = value; }
        }

        float diagonal;
        public float Diagonal
        {
            get { return diagonal; }
            set { diagonal = value; }
        }

        float averageArea;
        public float AverageArea
        {
            get { return averageArea; }
            set { averageArea = value; }
        }

        float averageCenterOffsetX;
        public float AverageCenterOffsetX
        {
            get { return averageCenterOffsetX; }
            set { averageCenterOffsetX = value; }
        }

        float averageCenterOffsetY;
        public float AverageCenterOffsetY
        {
            get { return averageCenterOffsetY; }
            set { averageCenterOffsetY = value; }
        }

        float averageWidth;
        public float AverageWidth
        {
            get { return averageWidth; }
            set { averageWidth = value; }
        }

        float averageHeight;
        public float AverageHeight
        {
            get { return averageHeight; }
            set { averageHeight = value; }
        }

        float averageWaist;
        public float AverageWaist
        {
            get { return averageWaist; }
            set { averageWaist = value; }
        }

        float averageAreaRatio;
        public float AverageAreaRatio
        {
            get { return averageAreaRatio; }
            set { averageAreaRatio = value; }
        }

        List<PatternInfo> patternList = new List<PatternInfo>();
        public List<PatternInfo> PatternList
        {
            get { return patternList; }
        }

        public int NumPattern
        {
            get { return patternList.Count; }
        }

        public SheetPatternGroup()
        {
        }

        public SheetPatternGroup(List<PatternInfo> patternList)
        {
            this.patternList.AddRange(patternList);
            Calc();
        }

        public SheetPatternGroup Clone()
        {
            List<PatternInfo> blobRectList = new List<PatternInfo>();
            this.patternList.ForEach(f => blobRectList.Add(f.Clone()));
            
            SheetPatternGroup newSheetPatternGroup = new SheetPatternGroup(blobRectList);
            return newSheetPatternGroup;
        }

        public BlobRect GetAverageBlobRect()
        {
            BlobRect blobRect = new BlobRect();
            blobRect.Area = averageArea;
            blobRect.BoundingRect = patternList[patternList.Count / 2].BoundingRect;
            blobRect.CenterOffset = new PointF(averageCenterOffsetX, averageCenterOffsetY);

            blobRect.CenterPt = patternList[patternList.Count / 2].CenterPt;

            return blobRect;
        }

        public void AddPattern(PatternInfo pattern)
        {
            patternList.Add(pattern);
            Calc();
        }

        public void AddPattern(List<PatternInfo> patternList)
        {
            this.patternList.AddRange(patternList.ToArray());
            Calc();
        }

        public void Merge(SheetPatternGroup sheetPatternGroup)
        {
            this.patternList.AddRange(sheetPatternGroup.patternList);
            Calc();
        }

        private void Calc()
        {
            if (patternList.Count == 1)
            {
                BlobRect blobRect = patternList[0];
                averageArea = patternList[0].Area;
                diagonal = (float)Math.Sqrt(averageArea);
                averageCenterOffsetX = patternList[0].CenterOffset.X;
                averageCenterOffsetY = patternList[0].CenterOffset.Y;
                averageWidth = patternList[0].BoundingRect.Width;
                averageHeight = patternList[0].BoundingRect.Height;
                averageWaist = patternList[0].WaistRatio;
                averageAreaRatio = (blobRect.Area / (blobRect.BoundingRect.Width * blobRect.BoundingRect.Height))*100;
                return;
            }

            averageArea = averageArea * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Area * (1.0f / patternList.Count);
            diagonal = (float)Math.Sqrt(averageArea);
            averageCenterOffsetX = averageCenterOffsetX * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.X * (1.0f / (float)patternList.Count);
            averageCenterOffsetY = averageCenterOffsetY * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.Y * (1.0f / (float)patternList.Count);
            averageWidth = averageWidth * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().BoundingRect.Width * (1.0f / patternList.Count);
            averageHeight = averageHeight * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().BoundingRect.Height * (1.0f / patternList.Count);
            averageWaist = averageWaist * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().WaistRatio * (1.0f / patternList.Count);
            averageAreaRatio = averageAreaRatio * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().AreaRatio * (1.0f / patternList.Count);
        }

        public FigureGroup CreateFigureGroup()
        {
            FigureGroup figureGroup = new FigureGroup();
            foreach (BlobRect blobRect in patternList)
                figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Yellow)));

            return figureGroup;
        }
        
        public float GetDiffValue(PatternFeature feature, PatternInfo blobRect)
        {
            switch (feature)
            {
                case PatternFeature.Area:
                    return diagonal - (float)Math.Sqrt(blobRect.Area);
                case PatternFeature.Width:
                    return averageWidth - blobRect.BoundingRect.Width;
                case PatternFeature.Height:
                    return averageHeight - blobRect.BoundingRect.Height;
                case PatternFeature.Waist:
                    return averageWaist - blobRect.WaistRatio;
                case PatternFeature.CenterX:
                    return averageCenterOffsetX - blobRect.CenterOffset.X;
                case PatternFeature.CenterY:
                    return averageCenterOffsetY - blobRect.CenterOffset.Y;
                case PatternFeature.CenterWidth:
                    return averageAreaRatio - blobRect.AreaRatio;
            }

            return 0;
        }

        private float GetDiffValue(PatternFeature feature, SheetPatternGroup patternGroup, PatternInfo blobRect)
        {
            float diffValue = 0;

            switch (feature)
            {
                case PatternFeature.Area:
                    diffValue = Math.Abs((float)Math.Sqrt(patternGroup.averageArea) - (float)Math.Sqrt(blobRect.Area));
                    break;
                case PatternFeature.Width:
                    diffValue = Math.Abs(patternGroup.averageWidth - blobRect.BoundingRect.Width);
                    break;
                case PatternFeature.Height:
                    diffValue = Math.Abs(patternGroup.averageHeight - blobRect.BoundingRect.Height);
                    break;
                case PatternFeature.Waist:
                    diffValue = Math.Abs(patternGroup.averageWaist - blobRect.WaistRatio);
                    break;
                case PatternFeature.CenterX:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetX - blobRect.CenterOffset.X);
                    break;
                case PatternFeature.CenterY:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetY - blobRect.CenterOffset.Y);
                    break;
                case PatternFeature.CenterWidth:
                    diffValue = Math.Abs(patternGroup.averageAreaRatio - blobRect.AreaRatio);
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

        private List<PatternInfo> GetSortedList(PatternFeature feature, SheetPatternGroup patternGroup)
        {
            List<PatternInfo> sortedList = null;

            switch (feature)
            {
                case PatternFeature.Area:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.Area).ToList();
                    break;
                case PatternFeature.Width:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.BoundingRect.Width).ToList();
                    break;
                case PatternFeature.Height:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.BoundingRect.Height).ToList();
                    break;
                case PatternFeature.Waist:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.WaistRatio).ToList();
                    break;
                case PatternFeature.CenterX:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.X).ToList();
                    break;
                case PatternFeature.CenterY:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.Y).ToList();
                    break;
                case PatternFeature.CenterWidth:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.AreaRatio).ToList();
                    break;
            }

            return sortedList;
        }

        private List<SheetPatternGroup> DevideSubGroup(PatternFeature feature, SheetPatternGroup patternGroup, float diffTol)
        {
            List<PatternInfo> sortedList = GetSortedList(feature, patternGroup);

            List<SheetPatternGroup> subPatternGroupList = new List<SheetPatternGroup>();
            SheetPatternGroup subPatternGroup = new SheetPatternGroup();

            //sortedList.ForEach(f =>
            //{
            //    float areaRatio = (f.Area / (f.BoundingRect.Width * f.BoundingRect.Height)) * 100;
            //    System.Diagnostics.Debug.WriteLine(areaRatio);
            //});

            foreach (PatternInfo blobRect in sortedList)
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

                    subPatternGroup = new SheetPatternGroup();
                    subPatternGroup.AddPattern(blobRect);
                }
            }

            if (subPatternGroup.patternList.Count > 0)
                subPatternGroupList.Add(subPatternGroup);

            return subPatternGroupList;
        }

        public List<SheetPatternGroup> DivideSubGroup(float diffTol)
        {
            List<SheetPatternGroup> patternGroupList = new List<SheetPatternGroup>();

            List<SheetPatternGroup> areaPatternGroupList = DevideSubGroup(PatternFeature.Area, this, diffTol);
            foreach (SheetPatternGroup areaPatternGroup in areaPatternGroupList)
            {
                List<SheetPatternGroup> heightPatternGroupList = DevideSubGroup(PatternFeature.Height, areaPatternGroup, diffTol);
                foreach (SheetPatternGroup heightPatternGroup in heightPatternGroupList)
                {
                    List<SheetPatternGroup> widthPatternGroupList = DevideSubGroup(PatternFeature.Width, heightPatternGroup, diffTol);
                    foreach (SheetPatternGroup widthPatternGroup in widthPatternGroupList)
                    {
                        List<SheetPatternGroup> waistPatternGroupList = DevideSubGroup(PatternFeature.Waist, widthPatternGroup, diffTol / 2.0f);
                        foreach (SheetPatternGroup waistPatternGroup in waistPatternGroupList)
                        {
                            List<SheetPatternGroup> centerXPatternGroupList = DevideSubGroup(PatternFeature.CenterX, waistPatternGroup, diffTol / 2.0f);
                            foreach (SheetPatternGroup centerXPatternGroup in centerXPatternGroupList)
                            {
                                List<SheetPatternGroup> centerYPatternGroupList = DevideSubGroup(PatternFeature.CenterY, centerXPatternGroup, diffTol / 2.0f);
                                patternGroupList.AddRange(centerYPatternGroupList);
                                //foreach (SheetPatternGroup centerWidthPatternGroup in centerYPatternGroupList)
                                //{
                                //    List<SheetPatternGroup> minFeretPatternGroupList = DevideSubGroup(PatternFeature.CenterWidth, centerWidthPatternGroup, diffTol/4);
                                //    patternGroupList.AddRange(minFeretPatternGroupList);
                                //}
                            }
                        }
                    }
                }
            }

            return patternGroupList;
        }

        public void UpdateMaterImage(AlgoImage trainImage)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, trainImage.Size);
            int width = (int)Math.Ceiling(this.patternList.Max(f => f.BoundingRect.Width) * 1.1f);
            int heigth = (int)Math.Ceiling(this.patternList.Max(f => f.BoundingRect.Height) * 1.1f);

            AlgoImage masterAlgoImage = ImageBuilder.Build(trainImage.LibraryType, trainImage.ImageType, width, heigth);
            masterAlgoImage.Clear();
            List<AlgoImage> subAlgoImageList = new List<AlgoImage>();

            for (int i = 0; i < this.patternList.Count; i++)
            {
                BlobRect pattern = this.patternList[i];
                Point pattternCenter = Point.Round(DrawingHelper.CenterPoint(pattern.BoundingRect));
                Rectangle patternRect = DrawingHelper.FromCenterSize(pattternCenter, masterAlgoImage.Size);
                Rectangle adjustPatternRect = Rectangle.Intersect(patternRect, imageRect);
                if (adjustPatternRect == patternRect)
                {
                    AlgoImage subPatternImage = trainImage.GetSubImage(adjustPatternRect);
                    subAlgoImageList.Add(subPatternImage);
                }
            }

            if (subAlgoImageList.Count > 0)
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(masterAlgoImage);
                ip.WeightedAdd(subAlgoImageList.ToArray(), masterAlgoImage);
            }
            this.masterImage = masterAlgoImage?.ToImageD();

            subAlgoImageList.ForEach(f => f.Dispose());
            masterAlgoImage?.Dispose();
        }

        public string GetInfoText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} [%]", StringManager.GetString("Ratio")));
            sb.AppendLine((this.countRatio * 100).ToString("0.00"));

            //sb.AppendLine(string.Format("{0} [px^2]", StringManager.GetString("Area")));
            //sb.AppendLine((this.averageArea * 100).ToString("0.00"));

            sb.AppendLine(string.Format("{0} [px]", StringManager.GetString("Width")));
            sb.AppendLine(this.averageWidth.ToString("0.00"));

            sb.AppendLine(string.Format("{0} [px]", StringManager.GetString("Heigth")));
            sb.AppendLine(this.averageHeight.ToString("0.00"));
            
            return sb.ToString();
        }

        public void LoadParam(XmlElement paramElement)
        {
            use = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "Use", use.ToString()));
            countRatio = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CountRatio", countRatio.ToString()));
            averageArea = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageArea", "0"));
            averageCenterOffsetX = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageCenterOffsetX", "0"));
            averageCenterOffsetY = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageCenterOffsetY", "0"));
            averageWidth = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageWidth", "0"));
            averageHeight = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageHeight", "0"));
            diagonal = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Diagonal", "0"));

            string imageString;
            imageString = XmlHelper.GetValue(paramElement, "MasterImage", "");
            if(string.IsNullOrEmpty(imageString)==false)
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
                masterImage = Image2D.ToImage2D(bitmap);
                bitmap.Dispose();
            }

            XmlNodeList xmlNodeList = paramElement.GetElementsByTagName("BlobRect");
            foreach (XmlElement subElement in xmlNodeList)
            {
                PatternInfo blobRect = new PatternInfo();
                blobRect.LoadXml(subElement);
                patternList.Add(blobRect);
            }
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Use", use.ToString());
            XmlHelper.SetValue(paramElement, "CountRatio", countRatio.ToString());
            XmlHelper.SetValue(paramElement, "AverageArea", averageArea.ToString());
            XmlHelper.SetValue(paramElement, "AverageCenterOffsetX", averageCenterOffsetX.ToString());
            XmlHelper.SetValue(paramElement, "AverageCenterOffsetY", averageCenterOffsetY.ToString());
            XmlHelper.SetValue(paramElement, "AverageWidth", averageWidth.ToString());
            XmlHelper.SetValue(paramElement, "AverageHeight", averageHeight.ToString());
            XmlHelper.SetValue(paramElement, "Diagonal", diagonal.ToString());

            if (this.masterImage != null)
            {
                Bitmap bitmap = masterImage.ToBitmap();
                string imageString = ImageHelper.BitmapToBase64String(bitmap);
                XmlHelper.SetValue(paramElement, "MasterImage", imageString.ToString());
                bitmap.Dispose();
            }

            foreach (PatternInfo blobRect in patternList)
            {
                blobRect.SaveXml(paramElement, "BlobRect");
            }
        }
    }
}

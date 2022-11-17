using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.Data
{
    public class PatternRegionElement
    {
        public Rectangle PatternRect { get => patternRect; set => patternRect = value; }
        public Rectangle AdjustpatternRect { get => adjustpatternRect; set => adjustpatternRect = value; }

        Rectangle patternRect;
        Rectangle adjustpatternRect;

        public PatternRegionElement(Rectangle patternRect, Rectangle adjustpatternRect)
        {
            this.patternRect = patternRect;
            this.adjustpatternRect = adjustpatternRect;
        }        
    }

    //public class InspRegionElementGroup
    //{
    //    int index;
    //    public int Index { get => this.index; }

    //    Rectangle rectangle;
    //    public Rectangle Rectangle { get => rectangle; }
    //    List<InspRegionElement> inspRegionElementList = new List<InspRegionElement>();
    //    public List<InspRegionElement> InspRegionElementList { get => this.inspRegionElementList; }

    //    public InspRegionElementGroup(int index, List<InspRegionElement> inspRegionElements)
    //    {
    //        this.index = index;
    //        this.inspRegionElementList.AddRange(inspRegionElements);
    //        if (inspRegionElements.Count > 0)
    //        {
    //            rectangle = inspRegionElements[0].Rectangle;
    //            inspRegionElements.ForEach(f => rectangle = Rectangle.Union(rectangle, f.Rectangle));
    //        }
    //    }

    //    public void Clear()
    //    {
    //        this.inspRegionElementList.Clear();
    //    }

    //    public void Save(XmlElement xmlElement, string key=null)
    //    {
    //        if (string.IsNullOrEmpty(key) == false)
    //        {
    //            XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
    //            xmlElement.AppendChild(subElement);
    //            Save(subElement);
    //            return;
    //        }

    //        XmlHelper.SetValue(xmlElement, "Index", this.index);
    //        this.inspRegionElementList.ForEach(f => f.Save(xmlElement, "InspElement"));
    //    }

    //    public static InspRegionElementGroup Load(XmlElement xmlElement)
    //    {
    //        int index = XmlHelper.GetValue(xmlElement, "Index", -1);

    //        List<InspRegionElement> inspRegionElementList = new List<InspRegionElement>();
    //        XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("InspElement");
    //        foreach(XmlElement subElement in xmlNodeList)
    //        {
    //            inspRegionElementList.Add(InspRegionElement.Load(subElement));
    //        }

    //        return new InspRegionElementGroup(index, inspRegionElementList);
    //    }
    //}

    public class InspectElement
    {
        public int Index { get => this.index; }
        public Rectangle Rectangle { get => rectangle;}
        public int OffsetYBase { get => offsetYBase; set => offsetYBase = value; }
        public int OffsetXBase { get => offsetXBase; set => offsetXBase = value; }
        public Rectangle[] DontcareRects { get => dontcareRects; }
        public bool IsDontcare { get => dontcareRects.Length>0; }

        int index;
        Rectangle rectangle = Rectangle.Empty;
        int offsetYBase = -1;
        int offsetXBase = -1;    // 검사 시작때마다 새로 설정함.
        Rectangle[] dontcareRects = null;

        public InspectElement(int index, Rectangle rectangle, int offsetXBase, int offsetYBase, Rectangle[] dontcareRects)
        {
            this.index = index;
            this.rectangle = rectangle;
            this.offsetXBase = offsetXBase;
            this.offsetYBase = offsetYBase;
            this.dontcareRects = dontcareRects;
        }

        public void Inflate(Size size)
        {
            this.rectangle.Inflate(size);
            for (int i = 0; i < this.dontcareRects.Length; i++)
                this.dontcareRects[i].Inflate(size);
        }

        public void Add(Size size)
        {
            this.rectangle.Size = Size.Add(this.rectangle.Size, size);
            for (int i = 0; i < this.dontcareRects.Length; i++)
                this.dontcareRects[i].Size = Size.Add(this.dontcareRects[i].Size, size);
        }

        public void Offset(Point point)
        {
            this.rectangle.Offset(point);
        }

        public void Clear()
        {
            this.index = -1;
            this.rectangle = Rectangle.Empty;
            this.offsetXBase = -1;
            this.offsetYBase = -1;
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Index", this.index);
            XmlHelper.SetValue(xmlElement, "Rectangle", this.rectangle);
            XmlHelper.SetValue(xmlElement, "OffsetXBase", this.offsetXBase);
            XmlHelper.SetValue(xmlElement, "OffsetYBase", this.offsetYBase);

            XmlHelper.SetValue(xmlElement, "DontcareRects", this.dontcareRects);
        }

        public static InspectElement Load(XmlElement xmlElement)
        {
            int index = XmlHelper.GetValue(xmlElement, "Index", -1);

            Rectangle rectangle = Rectangle.Empty;
            XmlHelper.GetValue(xmlElement, "Rectangle", ref rectangle);

            int offsetXBase = XmlHelper.GetValue(xmlElement, "OffsetXBase", -1);
            int offsetYBase = XmlHelper.GetValue(xmlElement, "OffsetYBase", -1);

            Rectangle[] dontcareRects = null;
            XmlHelper.GetValue(xmlElement, "DontcareRects", ref dontcareRects);

            return new InspectElement(index, rectangle, offsetXBase, offsetYBase, dontcareRects);
        }

        internal static int GetOffsetValue(AlgoImage algoImage)
        {
            SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            if (sheerFinder == null)
                return -1;
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            int foundPos = -1;
            if (algoImage.Width > algoImage.Height)
                foundPos = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 10);
            else
                foundPos = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 10);

            //subImage.Save(@"d:\temp\tt.bmp");

            return foundPos;
        }

        public void Merge(InspectElement element)
        {
            Point offset = Point.Subtract(element.rectangle.Location, new Size(this.rectangle.Location));

            this.rectangle = Rectangle.Union(this.rectangle, element.rectangle);
            
            this.offsetXBase = (this.offsetXBase + element.offsetXBase) / 2;
            this.offsetYBase = (this.offsetYBase + element.offsetYBase) / 2;
        }
    }

    public class RegionInfoG : UniScanG.Data.Vision.RegionInfo
    {
        public int LinePair
        {
            get { return this.linePair; }
            set { this.linePair = value; }
        }

        public bool OddEvenPair
        {
            get { return this.oddEvenPair; }
            set { this.oddEvenPair = value; }
        }

        public bool AdvancedPair
        {
            get { return this.advancedPair; }
            set { this.advancedPair = value; }
        }

        public ImageD Thumbnail
        {
            get { return this.thumbnail; }
        }

        public Rectangle[,] AdjPatRegionList
        {
            get { return this.adjPatRegionList; }
        }

        public Rectangle[,] PatRegionList
        {
            get { return this.patRegionList; }
        }

        public List<InspectElement> InspectElementList
        {
            get { return this.inspectElementList; }
        }

        public List<Point> DontcareLocationList
        {
            get { return this.dontcareLocationList; }
        }

        public Direction SplitLineDirection
        {
            get { return this.splitLineDirection;}
        }

        public float PoleAvg
        {
            get { return this.poleAvg; }
        }

        public float DielectricAvg
        {
            get { return this.dielectricAvg; }
        }

        int linePair = 1;
        bool oddEvenPair = false;
        bool advancedPair = false;
        ImageD thumbnail = null;
        //ImageD patternImage = null;

        Rectangle[,] adjPatRegionList = null;
        Rectangle[,] patRegionList = null;
        List<InspectElement> inspectElementList = null;
        List<Point> dontcareLocationList = null;
        //float imageScale = 1.0f;
        float poleAvg;
        float dielectricAvg;
        Direction splitLineDirection;

        Pen regionPen;
        Pen patRegionInsidePen;
        Pen patRegionOutsidePen;
        Pen inspRegionPen;
        Pen skipRegionPen;
        Brush skipRegionBrush;

        private RegionInfoG() : base()
        {
            this.use = true;
            this.linePair = 1;
            this.oddEvenPair = false;
            //this.trainImage = null;
            //this.patternImage = null;
            this.adjPatRegionList = new Rectangle[0, 0];
            this.patRegionList = new Rectangle[0, 0];
            this.inspectElementList = new List<InspectElement>();
            this.dontcareLocationList = new List<Point>();
            this.poleAvg = -1;
            this.dielectricAvg = -1;
            this.splitLineDirection = Direction.Vertical;

            InitializePen();
        }

        public RegionInfoG(ImageD thumbnail, Rectangle region, Rectangle[,] subRegionList, Rectangle[,] adjustSubRegionList, float poleAvg, float dielectricAvg) : base(region)
        {
            this.patRegionList = subRegionList;
            this.adjPatRegionList = adjustSubRegionList;
            this.inspectElementList = new List<InspectElement>();
            this.dontcareLocationList = new List<Point>();
            //this.trainImage = trainImage.Resize(imageScale, imageScale);
            this.thumbnail = thumbnail;
            //this.imageScale = imageScale;
            this.poleAvg = poleAvg;
            this.dielectricAvg = dielectricAvg;

            InitializePen();
        }

        private void InitializePen()
        {
            this.regionPen = new Pen(Color.Yellow, 5);

            this.patRegionInsidePen = new Pen(Color.LightBlue, 2);
            this.patRegionInsidePen.DashPattern = new float[] { 5, 3 };

            this.patRegionOutsidePen = new Pen(Color.CadetBlue, 1);

            this.inspRegionPen = new Pen(Color.DarkOrange, 5);

            this.skipRegionPen = new Pen(Color.Red, 2);
            this.skipRegionBrush = new SolidBrush(Color.FromArgb(128, Color.Red));
        }
        
        internal void BuildInspRegion(ImageD trainImageD, ImageD majorPatternImageD, Direction splitLineDirection, bool oddEvenPair)
        {
            this.oddEvenPair = oddEvenPair;
            BuildInspRegion(trainImageD, majorPatternImageD, splitLineDirection);
        }

        internal void BuildInspRegion(ImageD trainImageD, ImageD majorPatternImageD, Direction splitLineDirection)
        {
            // 기본 검사 줄 생성(가로/세로)
            this.inspectElementList.Clear();
            List<InspectElement> inspElementList = new List<InspectElement>();
            switch (splitLineDirection)
            {
                case Direction.Horizontal:
                    {
                        int loopY = this.adjPatRegionList.GetLength(0);
                        for (int y = 0; y < loopY; y++)
                        {
                            Rectangle unionRect = this.adjPatRegionList[y, 0];
                            int loopX = this.adjPatRegionList.GetLength(1);
                            for (int x = 0; x < loopX; x++)
                            {
                                Rectangle rect = this.adjPatRegionList[y, x];
                                if (rect.IsEmpty == false)
                                    unionRect = Rectangle.Union(unionRect, rect);
                            }
                            int inflate = Math.Min(unionRect.X, trainImageD.Width - unionRect.Right);
                            unionRect.Inflate(Math.Min(inflate, 15), 0);

                            List<Rectangle> dontcareRectList= this.dontcareLocationList.FindAll(f => f.Y == y).ConvertAll(f =>
                            {
                                Rectangle rect = this.adjPatRegionList[f.Y, f.X];
                                rect.Offset(-unionRect.X, 0);
                                return Rectangle.FromLTRB(rect.Left, 0, rect.Right, unionRect.Bottom);
                            });
                            inspElementList.Add(new InspectElement(y, unionRect, -1, -1, dontcareRectList.ToArray()));
                        }
                    }
                    break;

                case Direction.Vertical:
                    {
                        int loopX = this.adjPatRegionList.GetLength(1);
                        for (int x = 0; x < loopX; x++)
                        {
                            Rectangle unionRect = this.adjPatRegionList[0, x];
                            int loopY = this.adjPatRegionList.GetLength(0);
                            for (int y = 0; y < loopY; y++)
                            {
                                Rectangle rect = this.adjPatRegionList[y, x];
                                if (rect.IsEmpty == false)
                                    unionRect = Rectangle.Union(unionRect, rect);
                            }
                            int inflate = Math.Min(unionRect.Y, trainImageD.Height - unionRect.Bottom);
                            unionRect.Inflate(0, Math.Min(inflate, 16));

                     

                            List<Rectangle> dontcareRectList = this.dontcareLocationList.FindAll(f=>f.X == x).ConvertAll(f =>
                            {
                                Rectangle rect = this.adjPatRegionList[f.Y, f.X];
                                rect.Offset(0, -unionRect.Y);
                                return Rectangle.FromLTRB(0, rect.Top, unionRect.Width, rect.Bottom);
                            });
                            inspElementList.Add(new InspectElement(x, unionRect, -1, -1, dontcareRectList.ToArray()));
                        }
                    }
                    break;
            }

            // 크기 정렬 (같은 크기, 패턴 위치 동일하게)
            // 가로 길이를 4의 배수로 맞춤.
            Size inspRegionSize = Size.Round(new SizeF((float)inspElementList.Max(f => f.Rectangle.Width), (float)inspElementList.Max(f => f.Rectangle.Height)));
            inspRegionSize.Width = ((inspRegionSize.Width + 3) / 4) * 4;            
            
            inspElementList.ForEach(f =>
            {
                Size diff1 = new Size((inspRegionSize.Width - f.Rectangle.Width) / 2, (inspRegionSize.Height - f.Rectangle.Height) / 2);
                Size diff2 = new Size((inspRegionSize.Width - f.Rectangle.Width) % 2, (inspRegionSize.Height - f.Rectangle.Height) % 2);

                f.Inflate(diff1);
                f.Add(diff2);
            });

            // 상관함수로 오프셋 정렬
            AlgoImage trainImage = null;
            AlgoImage majorPatternImage = null;
            try
            {
                trainImage = ImageBuilder.Build(Trainer.TypeName, trainImageD, ImageType.Grey);
                majorPatternImage = ImageBuilder.Build(Trainer.TypeName, majorPatternImageD, ImageType.Grey);
                
                Rectangle imageRect = new Rectangle(Point.Empty, majorPatternImage.Size);

                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(majorPatternImage);
                Rectangle baseRect = inspElementList[inspElementList.Count / 2].Rectangle;
                if (Rectangle.Intersect(imageRect, baseRect) != baseRect)
                {
                    this.inspectElementList.Clear();
                    return;
                }
                AlgoImage subImageBase = majorPatternImage.GetSubImage(baseRect);
                Direction projDirection = splitLineDirection == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
                float[] projData = ip.Projection(subImageBase, projDirection, ProjectionType.Mean);

                for (int i = 0; i < this.inspectElementList.Count; i++)
                {
                    InspectElement inspRegionElement = inspElementList[i];
                    Rectangle intersectRect = Rectangle.Intersect(new Rectangle(Point.Empty, majorPatternImage.Size), inspRegionElement.Rectangle);
                    if (intersectRect == inspRegionElement.Rectangle)
                    {
                        //subRect.Inflate(0, -baseRect.Height / 4);
                        AlgoImage subTrainImage = trainImage.GetSubImage(inspRegionElement.Rectangle);
                        AlgoImage subPatternImage = majorPatternImage.GetSubImage(inspRegionElement.Rectangle);
                        float[] projData2 = ip.Projection(subPatternImage, projDirection, ProjectionType.Mean);
                        int offset = (int)Math.Round(AlgorithmCommon.Correlation(projData, projData2));
                        Point pt = Point.Empty;
                        switch (splitLineDirection)
                        {
                            case Direction.Horizontal:
                                pt.Y = -offset;
                                break;
                            case Direction.Vertical:
                                pt.X = -offset;
                                break;
                        }
                        inspRegionElement.Offset(pt);
                        subTrainImage.Dispose();
                        subPatternImage.Dispose();

                        intersectRect = Rectangle.Intersect(new Rectangle(Point.Empty, majorPatternImage.Size), inspRegionElement.Rectangle);
                        if (intersectRect == inspRegionElement.Rectangle)
                        {
                            Rectangle rectangle = inspRegionElement.Rectangle;
                            Rectangle lineImagerect = Rectangle.FromLTRB(rectangle.Left, 0, rectangle.Right, rectangle.Height);
                            AlgoImage lineImage = trainImage.GetSubImage(lineImagerect);
                            //lineImage.Save(@"d:\temp\lineImage.bmp");
                            inspElementList[i].OffsetYBase = InspectElement.GetOffsetValue(lineImage);
                            lineImage.Dispose();
                        }
                        else
                        {
                            inspElementList[i].Clear();
                        }
                    }
                    else
                    {
                        inspElementList[i].Clear();
                    }
                    ////검증
                    //AlgoImage subAlgoImage2 = algoImage.GetSubImage(subRect);
                    //float[] projData3 = ip.Projection(subAlgoImage2, projDirection, ProjectionType.Mean);
                    //int offset2 = (int)Math.Round(AlgorithmCommon.Correlation(projData, projData3));
                    //subAlgoImage2.Dispose();
                    //if(offset2 != 0)
                    //{
                    //    LogHelper.Debug(LoggerType.Operation, "RegionInfoG::BuildInspRegion offset2 is not 0");
                    //}

                }
                subImageBase.Dispose();
                inspElementList.RemoveAll(f => f.Rectangle.IsEmpty);
                if (inspElementList.Count <= 3)
                    return;

                // 검사영역에서 빠진 부분(갭)이 없도록 영역을 넓힘
                List<int> gap = new List<int>();
                switch (splitLineDirection)
                {
                    case Direction.Horizontal:
                        inspElementList.Aggregate((f, g) => { gap.Add(g.Rectangle.Top - f.Rectangle.Bottom); return g; });
                        break;
                    case Direction.Vertical:
                        inspElementList.Aggregate((f, g) => { gap.Add(g.Rectangle.Left - f.Rectangle.Right); return g; });
                        break;
                }

                int maxGap = gap.Max();
                if (maxGap > 0)
                {
                    int inflate = (int)Math.Ceiling(maxGap / 2.0);
                    for (int i = 0; i < this.inspectElementList.Count; i++)
                    {
                        switch (splitLineDirection)
                        {
                            case Direction.Horizontal:
                                inspElementList[i].Inflate(new Size(0, inflate));
                                break;
                            case Direction.Vertical:
                                inspElementList[i].Inflate(new Size(inflate, 0));
                                break;
                        }
                    }
                }

                Rectangle regionRect = new Rectangle(Point.Empty, this.region.Size);
                inspElementList.RemoveAll(f => f.Rectangle != Rectangle.Intersect(f.Rectangle, regionRect));
                Debug.Assert(inspElementList.All(f => f.Rectangle.Width % 4 == 0));


                // 주어진 줄 단위로 검사영역 묶기
                //List<InspectElement> newInspectElementList = new List<InspectElement>();

                //if (linePair < 1)
                //    linePair = 1;

                //this.splitLineDirection = splitLineDirection;
                //while (inspElementList.Count >= linePair)
                //{
                //    List<InspectElement> mergeItemList = inspElementList.GetRange(0, this.linePair);
                //    inspElementList.RemoveRange(0, this.linePair);

                //    newInspectElementList.Add(new InspectElement(newInspectElementList.Count, mergeItemList));
                //}

                this.inspectElementList = inspElementList;
            }
            finally
            {
                trainImage?.Dispose();
                majorPatternImage?.Dispose();
            }
        }
        
        public bool AlignX(AlgoImage regionAlgoImage)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, regionAlgoImage.Size);
            Size patSize = new Size(this.patRegionList.GetLength(1), this.patRegionList.GetLength(0));
            int centerPatPos = patSize.Height / 2;

            Rectangle rect = this.patRegionList[centerPatPos, 0];
            for (int i = 1; i < patSize.Width; i++)
                rect = Rectangle.Union(rect, this.patRegionList[centerPatPos, i]);
            rect.Intersect(imageRect);

            AlgoImage processImage = regionAlgoImage.GetSubImage(rect);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(processImage);
            float[] data = ip.Projection(processImage, Direction.Horizontal, ProjectionType.Mean);
            List<Point> hillList = AlgorithmCommon.FindHill(data, data.Average());
            if (patSize.Width != hillList.Count)
                return false;

            return true;
        }

        public override void Dispose()
        {
            base.Dispose();

            //trainImage?.Dispose();
            //trainImage = null;

            //patternImage?.Dispose();
            //patternImage = null;

            this.inspectElementList = null;
            this.patRegionList = null;
            this.dontcareLocationList = null;
        }

        public static RegionInfoG Load(XmlElement xmlElement)
        {
            RegionInfoG regionInfoG = new RegionInfoG();
            regionInfoG.LoadParam(xmlElement);
            return regionInfoG;
        }

        public override UniScanG.Data.Vision.RegionInfo Clone()
        {
            RegionInfoG clone = new RegionInfoG();
            clone.Copy(this);
            return clone;
        }

        public override void Copy(UniScanG.Data.Vision.RegionInfo srcRegionInfo)
        {
            base.Copy(srcRegionInfo);

            RegionInfoG regionInfo = srcRegionInfo as RegionInfoG;

            this.region = regionInfo.region;
            //this.trainImage = regionInfo.trainImage.Clone();
            //this.patternImage = regionInfo.patternImage.Clone();
            this.linePair = regionInfo.linePair;
            this.oddEvenPair = regionInfo.oddEvenPair;

            this.patRegionList = new Rectangle[regionInfo.patRegionList.GetLength(0), regionInfo.patRegionList.GetLength(1)];
            Array.Copy(regionInfo.patRegionList, this.patRegionList, this.patRegionList.Length);

            this.adjPatRegionList = new Rectangle[regionInfo.adjPatRegionList.GetLength(0), regionInfo.adjPatRegionList.GetLength(1)];
            Array.Copy(regionInfo.adjPatRegionList, this.adjPatRegionList, this.adjPatRegionList.Length);

            this.inspectElementList = new List<InspectElement>(regionInfo.inspectElementList);
            this.dontcareLocationList = new List<Point>(regionInfo.dontcareLocationList);

            //this.imageScale = regionInfo.imageScale;
            this.poleAvg = regionInfo.poleAvg;
            this.dielectricAvg = regionInfo.dielectricAvg;

            this.thumbnail = regionInfo.thumbnail.Clone();
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //if (this.trainImage != null)
            //{
            //    Bitmap bitmap = trainImage.ToBitmap();
            //    string imageString = ImageHelper.BitmapToBase64String(bitmap);
            //    bitmap.Dispose();
            //    XmlHelper.SetValue(algorithmElement, "TrainImage", imageString);
            //}

            if (this.thumbnail != null)
            {
                Bitmap bitmap = thumbnail.ToBitmap();
                string imageString = ImageHelper.BitmapToBase64String(bitmap);
                bitmap.Dispose();
                XmlHelper.SetValue(algorithmElement, "Thumbnail", imageString);
            }

            XmlHelper.SetValue(algorithmElement, "LinePair", this.linePair.ToString());
            XmlHelper.SetValue(algorithmElement, "OddEvenPair", this.oddEvenPair.ToString());
            XmlHelper.SetValue(algorithmElement, "PatRegionList", this.patRegionList);
            XmlHelper.SetValue(algorithmElement, "AdjPatRegionList", this.adjPatRegionList);

            XmlElement inspRegionListElement = algorithmElement.OwnerDocument.CreateElement("InspRegionList2");
            algorithmElement.AppendChild(inspRegionListElement);
            //this.inspRegionList.ForEach(f => f.Save(inspRegionListElement, "InspRegion"));
            foreach (InspectElement inspRegion in this.inspectElementList)
            {
                inspRegion.Save(inspRegionListElement, "InspRegion");
            }

            XmlHelper.SetValue(algorithmElement, "DontCareLocationList", this.dontcareLocationList.ToArray());

            //XmlHelper.SetValue(algorithmElement, "ImageScale", imageScale.ToString());
            XmlHelper.SetValue(algorithmElement, "PoleAvg", poleAvg.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricAvg", dielectricAvg.ToString());
            XmlHelper.SetValue(algorithmElement, "SplitLineDirection", splitLineDirection.ToString());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            string imageString;

            //imageString = XmlHelper.GetValue(algorithmElement, "TrainImage", "");
            //if (string.IsNullOrEmpty(imageString) == false)
            //{
            //    Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
            //    trainImage = Image2D.ToImage2D(bitmap);
            //}

            imageString = XmlHelper.GetValue(algorithmElement, "Thumbnail", "");
            if (string.IsNullOrEmpty(imageString) == false)
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
                thumbnail = Image2D.ToImage2D(bitmap);
            }

            this.linePair = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "LinePair", this.linePair.ToString()));
            this.oddEvenPair = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "OddEvenPair", this.oddEvenPair.ToString()));
            XmlHelper.GetValue(algorithmElement, "PatRegionList", ref this.patRegionList);
            XmlHelper.GetValue(algorithmElement, "AdjPatRegionList", ref this.adjPatRegionList);

            this.inspectElementList = new List<InspectElement>();
            bool isRecentModel = XmlHelper.Exist(algorithmElement, "InspRegionList2");
            if (isRecentModel == false)
                return;

            XmlElement inspRegionListElement = algorithmElement["InspRegionList2"];
            XmlNodeList xmlNodeList = inspRegionListElement.SelectNodes("InspRegion");
            foreach (XmlElement xmlElement in xmlNodeList)
            {
                InspectElement inspRegion = InspectElement.Load(xmlElement);
                inspectElementList.Add(inspRegion);
            }

            Point[] points = null;
            if (XmlHelper.GetValue(algorithmElement, "DontCareLocationList", ref points) == false)
                XmlHelper.GetValue(algorithmElement, "SkipRegions", ref points);
            this.dontcareLocationList = new List<Point>(points);

            //imageScale = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ImageScale", imageScale.ToString()));
            poleAvg = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "PoleAvg", "-1"));
            dielectricAvg = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "DielectricAvg", "-1"));
            splitLineDirection = (Direction)Enum.Parse(typeof(Direction), XmlHelper.GetValue(algorithmElement, "SplitLineDirection", this.splitLineDirection.ToString()));
        }

        public override Figure GetFigure()
        {
            FigureGroup figureGroup = new FigureGroup();
            figureGroup.AddFigure(new RectangleFigure(new Rectangle(Point.Empty, region.Size), regionPen));

            foreach (InspectElement inspRegion in this.inspectElementList)
            {
                    figureGroup.AddFigure(new RectangleFigure(inspRegion.Rectangle, inspRegionPen));
            }

            foreach (Rectangle adjPatRegion in this.adjPatRegionList)
            {
                figureGroup.AddFigure(new RectangleFigure(adjPatRegion, patRegionOutsidePen));
            }

            foreach (Rectangle patRegion in this.patRegionList)
            {
                figureGroup.AddFigure(new RectangleFigure(patRegion, patRegionInsidePen));
            }

            foreach (Point skipRegion in this.dontcareLocationList)
            {
                Rectangle rectangle = this.adjPatRegionList[skipRegion.Y, skipRegion.X];
                FigureGroup fg = new FigureGroup();
                fg.AddFigure(new RectangleFigure(rectangle, skipRegionPen, skipRegionBrush));
                fg.AddFigure(new LineFigure(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom), skipRegionPen));
                fg.AddFigure(new LineFigure(new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom), skipRegionPen));
                figureGroup.AddFigure(fg);
            }

            return figureGroup;
        }

        public void SetSkipRegion(AlgoImage regionPatternImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionPatternImage);
            Rectangle imageRect = new Rectangle(Point.Empty, regionPatternImage.Size);

            this.dontcareLocationList.Clear();
            for (int i = 0; i < this.patRegionList.GetLength(0); i++)
            {
                for (int j = 0; j < this.patRegionList.GetLength(1); j++)
                {
                    Rectangle subRect = this.patRegionList[i, j];
                    //if (subRect.Width < 0 || subRect.Height < 0 || Rectangle.Intersect(subRect, imageRect) != subRect)
                    //    this.skipRegions.Add(new Point(j, i));

                    AlgoImage subAlgoImage = regionPatternImage.GetSubImage(subRect);
                    int blk, gry, wht;
                    ip.Count(subAlgoImage, out blk, out gry, out wht);
                    float rate = wht * 1.0f / (wht + gry + blk);
                    if (rate < 0.1)
                        this.dontcareLocationList.Add(new Point(j, i));
                    subAlgoImage.Dispose();
                }
            }
            this.dontcareLocationList.RemoveAll(f => f.Y >= this.adjPatRegionList.GetLength(0) || f.X >= this.adjPatRegionList.GetLength(1));
        }

        public ImageD BuildSkippedImage()
        {
            //Image2D skippedImage = new Image2D(this.region.Width, this.region.Height, 1);

            //for (int y = 0; y < this.patRegionList.GetLength(0); y++)
            //{
            //    for (int x = 0; x < this.patRegionList.GetLength(1); x++)
            //    {
            //        Rectangle patRegion = this.patRegionList[y, x];
            //        bool isSkipped = this.skipPoints.Exists(f => f.X == x && f.Y == y);
            //        if (isSkipped)
            //            continue;

            //        Image2D subSkippedImage = new Image2D(patRegion.Width, patRegion.Height, 1);
            //        subSkippedImage.Clear();
            //        skippedImage.CopyFrom();

            //    }
            //}
            return null;
        }

        public Rectangle GetPatRect()
        {
            Rectangle patRect = this.patRegionList[0, 0];
            foreach (Rectangle patRegion in this.patRegionList)
                patRect = Rectangle.Union(patRect, patRegion);

            return patRect;
        }

        public string GetInfoString()
        {
            return string.Format("{0} / {1}", this.inspectElementList.Count, this.linePair);
        }
    }
}

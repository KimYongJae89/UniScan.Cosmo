using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Calculator
{
    internal class InspectRegion
    {
        public Rectangle Rectangle { get => rectangle; }
        Rectangle rectangle;

        public AlgoImage AlgoImage { get => algoImage; }
        AlgoImage algoImage;

        public bool IsBuilded{ get => isBuilded; }
        bool isBuilded;

        AlgoImage resultImage;

        public InspectLineSet[] InspectLineSets { get => inspectLineSets; }
        InspectLineSet[] inspectLineSets;

        public InspectRegion(Rectangle rectangle, InspectLineSet[] inspectLineSets)
        {
            this.rectangle = rectangle;
            this.inspectLineSets = inspectLineSets;

            this.algoImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
            this.resultImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);

            Array.ForEach(this.inspectLineSets, f => f.Init(this.algoImage, this.resultImage));
        }

        public void Build(AlgoImage inspImage, AlgoImage resultImage, Point offset)
        {
            Rectangle ofsRect = this.rectangle;
            ofsRect.Offset(offset.X, offset.Y);

            Rectangle imageRect = new Rectangle(Point.Empty, inspImage.Size);
            Rectangle adjustRect = Rectangle.Intersect(imageRect, ofsRect);

            if (adjustRect != ofsRect)
            {
                isBuilded = false;
                return;
            }

            if (this.algoImage.IsCompatible(inspImage))
            {
                this.algoImage.Copy(inspImage, adjustRect);
            }
            else
            {
                AlgoImage subImage = inspImage.GetSubImage(adjustRect);
                DynMvp.Vision.ImageConverter.Convert(subImage, this.algoImage);
                subImage.Dispose();
            }
            Array.ForEach(this.inspectLineSets, f => f.Build(this.algoImage));

            isBuilded = true;
        }

        public void GetResult(AlgoImage resultImage, Point offset)
        {
            //resultImage.Save(@"C:\temp\resultImage1.bmp");

            //this.resultImage.Save(@"C:\temp\resultImage2.bmp");
            //Array.ForEach(this.inspectLineSets, f => f.GetResult(this.resultImage));
            //this.resultImage.Save(@"C:\temp\resultImage3.bmp");

            Rectangle imageRect = new Rectangle(Point.Empty, resultImage.Size);
            Rectangle ofsRect = this.rectangle;
            ofsRect.Offset(offset.X, offset.Y);
            ofsRect.Intersect(imageRect);
            resultImage.Copy(this.resultImage, Point.Empty, ofsRect.Location, ofsRect.Size);
            //resultImage.Save(@"C:\temp\resultImage4.bmp");
        }

        public void Release()
        {
            Array.ForEach(this.inspectLineSets, f => f.Release());

            this.algoImage.Clear();
            this.resultImage.Clear();
        }

        public void Dispose()
        {
            Array.ForEach(inspectLineSets, f => f.Dispose());

            this.algoImage.Dispose();
            this.resultImage.Dispose();

        }
    }

    internal class InspectLineSet
    {
        public InspectLine[] InspectLines { get => inspectLines; }
        InspectLine[] inspectLines;

        public InspectEdge InspectEdge { get => inspectEdge; }
        InspectEdge inspectEdge;

        public ImageProcessing ImageProcessing { get => imageProcessing; }
        ImageProcessing imageProcessing;

        public InspectLineSet(InspectLine[] inspectLines, ImageProcessing imageProcessing)
        {
            this.inspectLines = inspectLines;
            this.inspectEdge = (InspectEdge)Array.Find(inspectLines, f => f is InspectEdge);
            this.imageProcessing = imageProcessing;
        }

        internal void Init(AlgoImage inspImage, AlgoImage resultImage)
        {
            Array.ForEach(this.inspectLines,
                f => f.Init(inspImage, resultImage));
        }

        internal InspectLine[] Build(AlgoImage inspImage)
        {
            Array.ForEach(this.inspectLines,
                f => f.Build(inspImage, this.imageProcessing));

            //Parallel.ForEach(this.inspectLines, new ParallelOptions { MaxDegreeOfParallelism = inspectLines.Length },
            //    f => f.Build(inspImage, bufImage, resultImage, this.imageProcessing));

            return inspectLines;
        }

        internal void GetResult(AlgoImage resultImage)
        {
            Array.ForEach(this.inspectLines, f => resultImage.Copy(f.ResultImage, Point.Empty, f.Rectangle.Location, f.Rectangle.Size));
        }

        internal void Release()
        {
            this.imageProcessing.WaitStream();
            Array.ForEach(this.inspectLines, f => f.Release());
        }

        public void Dispose()
        {
            this.imageProcessing.WaitStream();
            Array.ForEach(inspectLines, f => f.Dispose());
        }

    }

    internal class InspectLine
    {
        public Rectangle Rectangle { get => rectangle; }
        protected Rectangle rectangle;

        public Rectangle[] IgnoreArea { get => ignoreArea; }
        protected Rectangle[] ignoreArea;

        public InspectLine PrevInspectLine { get => prevLineElement; }
        protected InspectLine prevLineElement;

        public InspectLine NextInspectLine { get => nextLineElement; }
        protected InspectLine nextLineElement;

        public bool IsLinked { get => (this.prevLineElement != null && this.nextLineElement != null); }

        public bool IsDoncare { get => this.ignoreArea.Length > 0; }
        
        public AlgoImage AlgoImage { get => algoImage; }
        protected AlgoImage algoImage = null;

        public AlgoImage[] BufImage { get => bufImage; }
        protected AlgoImage[] bufImage = null;

        public AlgoImage ResultImage { get => resultImage; }
        protected AlgoImage resultImage = null;

        public InspectLine(Rectangle rectangle, Rectangle[] ignoreArea)
        {
            this.rectangle = rectangle;
            this.ignoreArea = ignoreArea;

            //this.algoImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);

            this.bufImage = new AlgoImage[1];
            for (int i = 0; i < this.bufImage.Length; i++)
                this.bufImage[i] = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);

            //this.resultImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
        }

        public void Link(InspectLine prev, InspectLine next, CalculatorParam.EIgnoreMethod ignoreMethod)
        {
            this.prevLineElement = prev;
            this.nextLineElement = next;

            if (ignoreMethod == CalculatorParam.EIgnoreMethod.Neighborhood)
            {
                List<Rectangle> list = new List<Rectangle>();
                list.AddRange(this.ignoreArea);
                list.AddRange(prev.ignoreArea);
                list.AddRange(next.ignoreArea);
                List<Rectangle> orderedList = list.OrderBy(f => DynMvp.Base.MathHelper.GetLength(Point.Empty, f.Location)).ToList();
                List<Rectangle> ignoreList = new List<Rectangle>();
                while (orderedList.Count > 0)
                {
                    List<Rectangle> intersectRectList = orderedList.FindAll(f => orderedList[0].IntersectsWith(f));
                    orderedList.RemoveAll(f => intersectRectList.Contains(f));

                    Rectangle rectangle = intersectRectList[0];
                    intersectRectList.ForEach(f => rectangle = Rectangle.Union(rectangle, f));
                    ignoreList.Add(rectangle);
                }
                this.ignoreArea = ignoreList.ToArray();
            }
        }

        public void Init(AlgoImage algoImage,AlgoImage resultImage)
        {
            this.algoImage = algoImage.GetSubImage(this.rectangle);
            this.resultImage = resultImage.GetSubImage(this.rectangle);
        }

        public virtual void Build(AlgoImage algoImage, ImageProcessing imageProcessing)
        {
            //this.algoImage.Copy(algoImage, rectangle.Location, Point.Empty, rectangle.Size);
        }

        public virtual void Release()
        {
            //this.algoImage?.Clear();

            //this.resultImage?.Clear();

            if (this.bufImage != null)
                Array.ForEach(this.bufImage, f => f?.Clear());
        }

        public virtual void Dispose()
        {
            this.algoImage?.Dispose();

            this.resultImage?.Dispose();

            if (this.bufImage != null)
                Array.ForEach(this.bufImage, f => f?.Dispose());
        }
    }

    internal class InspectEdge : InspectLine
    {
        public CalculatorParam.EEdgeFindMethod EdgeFindMethod { get => edgeFindMethod; }
        CalculatorParam.EEdgeFindMethod edgeFindMethod;

        public int Width { get => width; }
        int width;

        public int Value { get => value; }
        int value;

        public AlgoImage EdgeImage { get => edgeImage; }
        AlgoImage edgeImage = null;

        public AlgoImage TempImage { get => tempImage; }
        AlgoImage tempImage = null;

        public bool IsBuilded { get => isBuilded; }
        bool isBuilded;

        public InspectEdge(InspectLine inspectLine, EdgeParam edgeParam)
            : base(inspectLine.Rectangle, inspectLine.IgnoreArea)
        {
            Init(edgeParam.EdgeFindMethod, edgeParam.EdgeWidth, edgeParam.EdgeValue);
        }

        public InspectEdge(Rectangle rectangle, Rectangle[] ignoreArea, EdgeParam edgeParam)
            : base(rectangle, ignoreArea)
        {
            Init(edgeParam.EdgeFindMethod, edgeParam.EdgeWidth, edgeParam.EdgeValue);
        }

        private void Init(CalculatorParam.EEdgeFindMethod edgeFindMethod, int width, int value)
        {
            this.edgeFindMethod = edgeFindMethod;
            this.width = width;
            this.value = value;

            Size size = rectangle.Size;

            if (edgeFindMethod == CalculatorParam.EEdgeFindMethod.Soble)
                this.tempImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
            this.edgeImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);

            this.isBuilded = false;
        }

        public override void Build(AlgoImage algoImage, ImageProcessing imageProcessing)
        {
            base.Build(algoImage, imageProcessing);
            
            if (this.edgeFindMethod == CalculatorParam.EEdgeFindMethod.Projection)
                this.isBuilded = GetEdgeImagePJ(this.algoImage, imageProcessing, null);
            else
                this.isBuilded = GetEdgeImageSoble(this.algoImage, imageProcessing, null);
        }

        private bool GetEdgeImagePJ(AlgoImage lineImage, ImageProcessing ip, DebugContext debugContext)
        {
            if (this.width == 0 || this.value == 0)
                return false;

            AlgoImage edgeImage = this.edgeImage;
            //Debug.Assert(baseEdgeImage.Size == baseLineImage.Size);
            //baseLineImage.Save("BaseLineImage.bmp", debugContext);
            //baseLineImage.Save(@"d:\temp\BaseLineImage.bmp");

            Size drawSize = Size.Empty;
            Rectangle[] vertSkipRect = new Rectangle[2];

            float[] dataH = ip.Projection(lineImage, Direction.Horizontal, ProjectionType.Mean);
            //List<Point> hillListH = AlgorithmCommon.FindHill(dataH, -1, true,debugContext);
            List<Point> hillListH = AlgorithmCommon.FindHill2(dataH, 5, debugContext);
            drawSize = new Size(this.width * 3, lineImage.Height);
            if (hillListH.Count > 0)
            {
                int maxWidth = hillListH.Max(f => f.Y - f.X);
                Point foundHill = hillListH.Find(f => (f.Y - f.X) == maxWidth);
                Rectangle srcEdgeRect = new Rectangle(new Point(foundHill.X - 2 * this.width, 0), drawSize);
                ip.DrawRect(edgeImage, srcEdgeRect, this.value, true);
                vertSkipRect[0] = new Rectangle(foundHill.X - 3, 0, 5, lineImage.Height);

                Rectangle dstEdgeRect = new Rectangle(new Point(foundHill.Y - this.width, 0), drawSize);
                ip.DrawRect(edgeImage, dstEdgeRect, this.value, true);
                vertSkipRect[1] = new Rectangle(foundHill.Y - 2, 0, 5, lineImage.Height);
            }

            float[] dataV = ip.Projection(lineImage, Direction.Vertical, ProjectionType.Mean);
            List<Point> hillListV = AlgorithmCommon.FindHill(dataV, -1, true);
            drawSize = new Size(lineImage.Width, this.width * 3);
            foreach (Point hill in hillListV)
            {
                Rectangle srcEdgeRect = new Rectangle(new Point(0, hill.X - this.width), drawSize);
                ip.DrawRect(edgeImage, srcEdgeRect, this.value, true);
                ip.DrawRect(edgeImage, new Rectangle(0, hill.X - 3, lineImage.Width, 5), 255, true);

                Rectangle dstEdgeRect = new Rectangle(new Point(0, hill.Y - 2 * this.width), drawSize);
                ip.DrawRect(edgeImage, dstEdgeRect, this.value, true);
                ip.DrawRect(edgeImage, new Rectangle(0, hill.Y - 3, lineImage.Width, 5), 255, true);
            }

            Array.ForEach(vertSkipRect, f => ip.DrawRect(edgeImage, f, 255, true));

            //baseEdgeImage.Save("BaseEdgeImage.bmp", debugContext);
            //edgeImage.Save(@"d:\temp\BaseEdgeImage.bmp");
            return true;
        }
        
        private bool GetEdgeImageSoble(AlgoImage lineImage, ImageProcessing ip, DebugContext debugContext)
        {
            AlgoImage edgeImage = this.edgeImage;
            AlgoImage tempImage = this.tempImage;

            if (this.width == 0 || this.value == 0)
                return false;

            ip.Average(lineImage, edgeImage);
            //tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-1Average.bmp");

            ip.Sobel(edgeImage, tempImage);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-2Sobel.bmp");

            ip.Binarize(tempImage, edgeImage);
            //tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-3Binarize.bmp");

            ip.Close(edgeImage, tempImage, 2);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-4Close.bmp");

            ip.Dilate(tempImage, edgeImage, this.width);
            //tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-5Dilate1.bmp");

            //ip.Dilate(edgeImage, tempImage, this.width);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-5Dilate2.bmp");

            //ip.WeightedAdd(new AlgoImage[] { edgeImage, tempImage }, edgeImage);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-6WeightAdd.bmp");

            return true;
        }

        public override void Release()
        {
            base.Release();

            this.isBuilded = false;
            this.edgeImage.Clear();
            this.tempImage?.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.tempImage?.Dispose();
            this.tempImage = null;

            this.edgeImage?.Dispose();
            this.edgeImage = null;
        }

    }

    internal class CalculatorV2Extender
    {
        public InspectRegion[] Train(CalculatorParam calculatorParam)
        {
            LineSetParam lineSetParam = new LineSetParam(calculatorParam.AdaptivePairing, calculatorParam.BoundaryPairStep, calculatorParam.IgnoreMethod, calculatorParam.IgnoreSideLine);
            EdgeParam edgeParam = new EdgeParam(calculatorParam.EdgeValue, calculatorParam.EdgeWidth, calculatorParam.EdgeFindMethod);

            List<RegionInfoG> inspectRegionInfoG = calculatorParam.RegionInfoList.FindAll(f => f.Use);
            int length = inspectRegionInfoG.Count;
            InspectRegion[] inspectRegions = new InspectRegion[length];

            for (int i = 0; i < length; i++)
            {
                RegionInfoG regionInfoG = inspectRegionInfoG[i];
                if (regionInfoG.Use == false)
                    continue;

                InspectRegion inspectRegion = GetInspectRegion(regionInfoG, lineSetParam, edgeParam);
                inspectRegions[i] = inspectRegion;
            }

            return inspectRegions;
        }

        private InspectRegion GetInspectRegion(RegionInfoG regionInfoG, LineSetParam lineSetParam, EdgeParam edgeParam)
        {
            List<InspectElement>[] inspectElementLists = null;
            if (regionInfoG.OddEvenPair)
            {
                inspectElementLists = new List<InspectElement>[2];
                inspectElementLists[0] = regionInfoG.InspectElementList.Where((f, i) => i % 2 == 0).ToList();
                inspectElementLists[1] = regionInfoG.InspectElementList.Where((f, i) => i % 2 == 1).ToList();
            }
            else
            {
                inspectElementLists = new List<InspectElement>[1];
                inspectElementLists[0].AddRange(regionInfoG.InspectElementList);
            }

            InspectLineSet[] inspectLineSets = GetInspectLineSet(inspectElementLists, lineSetParam, edgeParam);
            InspectRegion inspectRegion = new InspectRegion(regionInfoG.Region, inspectLineSets);
            return inspectRegion;
        }

        private InspectLineSet[] GetInspectLineSet(List<InspectElement>[] inspectElementLists, LineSetParam lineSetParam, EdgeParam edgeParam)
        {
            List<InspectLineSet> inspectLineSetList = new List<InspectLineSet>();
            Array.ForEach(inspectElementLists, inspectElementList =>
            {
                InspectLine[] inspectLines = GetInspectLines(inspectElementList, lineSetParam, edgeParam);
                InspectEdge inspectEdge = (InspectEdge)Array.Find(inspectLines, f => f is InspectEdge);
                ImageProcessing imageProcessing = ImageProcessing.Create(CalculatorBase.TypeName);

                InspectLineSet inspectLineSet = new InspectLineSet(inspectLines, imageProcessing);
                inspectLineSetList.Add(inspectLineSet);
            });

            return inspectLineSetList.ToArray();
        }

        private InspectLine[] GetInspectLines(List<InspectElement> inspectElementList, LineSetParam lineSetParam, EdgeParam edgeParam)
        {
            int src = lineSetParam.IgnoreSideLine ? 1 : 0;
            int dst = inspectElementList.Count - (lineSetParam.IgnoreSideLine ? 2 : 1);
            int cent = (src + dst) / 2 + 1;

            List<InspectLine> inspectLineList = new List<InspectLine>();
            for (int i = src; i <= dst; i++)
            {
                InspectElement inspectElement = inspectElementList[i];
                if (i == cent)
                    inspectLineList.Add(new InspectEdge(inspectElement.Rectangle, inspectElement.DontcareRects, edgeParam));
                else
                    inspectLineList.Add(new InspectLine(inspectElement.Rectangle, inspectElement.DontcareRects));
            }

            int min = 0, max = inspectLineList.Count - 1;
            List<InspectLine> careLineElementList = inspectLineList.FindAll(f => f.IsDoncare == false);
            int minCare = 0, maxCare = careLineElementList.Count - 1;
            for (int i = 0; i <= max; i++)
            {
                int curIdx = i;
                int prevIdx, nextIdx;
                if (!lineSetParam.AdaptivePairing || inspectLineList[i].IsDoncare)
                {
                    prevIdx = (i != min) ? i - 1 : i + lineSetParam.BoundaryPairStep;
                    nextIdx = (i != max) ? i + 1 : i - lineSetParam.BoundaryPairStep;
                }
                else
                {
                    int careIdx = careLineElementList.IndexOf(inspectLineList[curIdx]);
                    int carePrevIdx = (careIdx != minCare) ? careIdx - 1 : careIdx + lineSetParam.BoundaryPairStep;
                    int careNextIdx = (careIdx != maxCare) ? careIdx + 1 : careIdx - lineSetParam.BoundaryPairStep;

                    prevIdx = inspectLineList.IndexOf(careLineElementList[carePrevIdx]);
                    nextIdx = inspectLineList.IndexOf(careLineElementList[careNextIdx]);
                }
                inspectLineList[i].Link(inspectLineList[prevIdx], inspectLineList[nextIdx], lineSetParam.IgnoreMethod);
            }

            inspectLineList.RemoveAll(f => f.IsLinked == false);
            return inspectLineList.ToArray();
        }
    }
}
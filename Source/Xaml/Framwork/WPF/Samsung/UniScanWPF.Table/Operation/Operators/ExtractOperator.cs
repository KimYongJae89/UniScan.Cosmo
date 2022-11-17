using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class ExtractOperator : Operator
    {
        ExtractOperatorSettings settings;
        
        List<Task> taskList;   

        public ExtractOperatorSettings Settings { get => settings; }

        public ExtractOperator()
        {
            settings = new ExtractOperatorSettings();

            taskList = new List<Task>();
        }

        public override bool Initialize(ResultKey resultKey, CancellationTokenSource cancellationTokenSource)
        {
            taskList.Clear();

            return base.Initialize(resultKey, cancellationTokenSource);
        }

        public void StartExtract(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Scan)
                return;

            this.OperatorState = OperatorState.Run;

            Task task = Task.Factory.StartNew(() =>
            {
                SystemManager.Instance().OperatorProcessed(Extract((ScanOperatorResult)operatorResult));
            }, cancellationTokenSource.Token);

            lock (taskList)
                taskList.Add(task);
        }

        public void WaitExtract()
        {
            Task.WaitAll(taskList.ToArray());
            
            taskList.Clear();

            SystemManager.Instance().OperatorCompleted(new ExtractOperatorResult(resultKey, null, "Completed"));
        }

        private BlobRectList MultiRegionBlob(AlgoImage processBuffer, ImageProcessing imageProcessing, BlobParam blobParam)
        {
            bool eraseBorderBlob = blobParam.EraseBorderBlobs;
            if (eraseBorderBlob)
                blobParam.EraseBorderBlobs = false;

            Rectangle interestRect = new Rectangle(0, 0, processBuffer.Width, processBuffer.Height);

            int blobSectionCount = 10;
            RectangleF[] blobSectionRectArray = new RectangleF[blobSectionCount];

            int sectionHeight = interestRect.Height / blobSectionCount;
            for (int i = 0; i < blobSectionCount; i++)
            {
                blobSectionRectArray[i].Location = new PointF(interestRect.Left, 0 + (i * sectionHeight));
                blobSectionRectArray[i].Size = new SizeF(interestRect.Width, sectionHeight);
                if (i == blobSectionCount - 1)
                    blobSectionRectArray[i].Size = new SizeF(interestRect.Width, interestRect.Height - (i * sectionHeight));
            }

            List<Tuple<Rectangle, BlobRectList>> blobRectTupleList = new List<Tuple<Rectangle, BlobRectList>>();
            Parallel.ForEach(blobSectionRectArray, sectionRect =>
            {
                Rectangle rect = Rectangle.Truncate(sectionRect);
                AlgoImage inspectProcessImage = processBuffer.GetSubImage(rect);

                lock (blobRectTupleList)
                    blobRectTupleList.Add(new Tuple<Rectangle, BlobRectList>(rect, imageProcessing.Blob(inspectProcessImage, blobParam)));

                inspectProcessImage.Dispose();
            });
            
            blobRectTupleList = blobRectTupleList.OrderByDescending(tuple => tuple.Item1.Y).Reverse().ToList();

            Tuple<Rectangle, BlobRectList> blobTuple = blobRectTupleList.Aggregate((prev, next) =>
            {
                BlobRectList blobRectList = imageProcessing.BlobMerge(prev.Item2, next.Item2, blobParam);

                BufferManager.Instance().AddDispoableObj(prev.Item2);
                BufferManager.Instance().AddDispoableObj(next.Item2);

                return new Tuple<Rectangle, BlobRectList>(Rectangle.Union(prev.Item1, next.Item1), blobRectList);
            });

            BlobRectList mergeBlobRectList = blobTuple.Item2;
            
            if (eraseBorderBlob)
            {
                mergeBlobRectList = imageProcessing.EreseBorderBlobs(blobTuple.Item2, blobParam);
                blobParam.EraseBorderBlobs = true;
            }
            
            return mergeBlobRectList;
         }

        private ExtractOperatorResult Extract(ScanOperatorResult scanOperatorResult)
        {
            string debugContextSubPath = string.Format("ExtractOperator_{0}", scanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);
            
            //try
            //{
            AlgoImage topAlgoImage = scanOperatorResult.TopLightImage;
            int topBinValue = SystemManager.Instance().CurrentModel.BinarizeValueTop;
            AlgoImage backAlgoImage = scanOperatorResult.BackLightImage;
            int backBinValue = (int)(SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue * 0.4);
            topAlgoImage.Save(@"topAlgoImage.bmp", debugContext);

            AlgoImage sheetBuffer = BufferManager.Instance().GetSheetBuffer(scanOperatorResult.FlowPosition);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(topAlgoImage);

            int width = scanOperatorResult.TopLightImage.Width;
            int height = scanOperatorResult.TopLightImage.Height;

            // 이진화 영상
            imageProcessing.Binarize(backAlgoImage, sheetBuffer, backBinValue, true);

            imageProcessing.FillHoles(sheetBuffer, sheetBuffer);

            AlgoImage maskBuffer = BufferManager.Instance().GetMaskBuffer(scanOperatorResult.FlowPosition);
                
            imageProcessing.Binarize(topAlgoImage, maskBuffer, topBinValue, true);
            imageProcessing.Dilate(maskBuffer, maskBuffer, (int)(settings.MaxMarginLength / DeveloperSettings.Instance.Resolution));
            imageProcessing.FillHoles(maskBuffer, maskBuffer);
            imageProcessing.Erode(maskBuffer, (int)(settings.MaxMarginLength / DeveloperSettings.Instance.Resolution) - 5);

            imageProcessing.And(maskBuffer, sheetBuffer, sheetBuffer);

            BlobParam sheetBlobParam = new BlobParam();
            sheetBlobParam.SelectBorderBlobs = true;
            sheetBlobParam.SelectLabelValue = true;
            BlobRectList sheetBlobRectList = imageProcessing.Blob(sheetBuffer, sheetBlobParam);
            if (sheetBlobRectList.GetList().Count == 0)
            {
                sheetBlobRectList.Dispose();
                return new ExtractOperatorResult(resultKey, scanOperatorResult, "No Sheet exist");
            }

            BlobRect maxAreaBlob = sheetBlobRectList.GetMaxAreaBlob();
            float areaRatio = (maxAreaBlob.Area * 100f) / (sheetBuffer.Width * sheetBuffer.Height);
            if (areaRatio < 1.0f)
            {
                sheetBlobRectList.Dispose();
                return new ExtractOperatorResult(resultKey, scanOperatorResult, "No Sheet exist");
            }

            imageProcessing.Clear(sheetBuffer, 0);
            imageProcessing.DrawBlob(sheetBuffer, sheetBlobRectList, maxAreaBlob, new DrawBlobOption() { SelectBlob = true });
            sheetBlobRectList.Dispose();

            BlobParam blobParam = new BlobParam();
            blobParam.SelectArea = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectBoundingRect = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectLabelValue = true;
            blobParam.SelectCenterPt = true;
            blobParam.RotateWidthMin = settings.MinPatternLength / DeveloperSettings.Instance.Resolution;
            if (settings.UseMaxPatternLength)
                blobParam.RotateWidthMax = settings.MaxPatternLength / DeveloperSettings.Instance.Resolution;

            imageProcessing.Binarize(topAlgoImage, maskBuffer, topBinValue, true);
            imageProcessing.FillHoles(maskBuffer, maskBuffer);
            imageProcessing.And(maskBuffer, sheetBuffer, maskBuffer);

            BlobRectList blobList = imageProcessing.Blob(maskBuffer, blobParam);// MultiRegionBlob(maskBuffer, imageProcessing, blobParam);
            imageProcessing.Clear(maskBuffer, 0);
            imageProcessing.DrawBlob(maskBuffer, blobList, null, new DrawBlobOption() { SelectBlob = true });
            BufferManager.Instance().AddDispoableObj(blobList);
            
            List <BlobRect> blobRectList = blobList.GetList();
            if (blobRectList.Count == 0)
                return new ExtractOperatorResult(resultKey, scanOperatorResult, "No Pattern exist");

            RectangleF sheetRect = blobRectList[0].BoundingRect;

            blobRectList.ForEach(f => sheetRect = RectangleF.Union(sheetRect, f.BoundingRect));

            Point[] vertexPoints = GetVertexPoints(blobRectList, debugContext);

            LogHelper.Debug(LoggerType.Inspection, string.Format("FlowPosition {0} SheetRect is {1}", scanOperatorResult.FlowPosition, sheetRect));

            AlgoImage resizeMaskBuffer = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size((int)(width * resizeRatio), (int)(height * resizeRatio)));
            imageProcessing.Resize(maskBuffer, resizeMaskBuffer, resizeRatio);
            BitmapSource maskBufferBitmap = resizeMaskBuffer.ToBitmapSource();
            resizeMaskBuffer.Dispose();
     
            AlgoImage resizeSheetBuffer = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size((int)(width * resizeRatio), (int)(height * resizeRatio)));
            imageProcessing.Resize(sheetBuffer, resizeSheetBuffer, resizeRatio);
            BitmapSource sheetBufferBitmap = resizeSheetBuffer.ToBitmapSource();
            resizeSheetBuffer.Dispose();
                
            double sheetHeightMm = maxAreaBlob.BoundingRect.Height * DeveloperSettings.Instance.Resolution / 1000.0;
            int patternCount = blobRectList.Count;
            
            return new ExtractOperatorResult(resultKey, scanOperatorResult,
                sheetBuffer, maskBuffer, sheetBufferBitmap, maskBufferBitmap,
                Rectangle.Round(sheetRect),
                blobRectList, vertexPoints);
        }

        private Point[] GetVertexPoints(List<BlobRect> blobRectList, DebugContext debugContext)
        {
            if (blobRectList.Count == 0)
                return null;

            // 블랍의 평균 크기
            double meanArea = blobRectList.Average(f => f.Area);
            if (meanArea < 100)
                return null;

            Size meanSize = new Size()
            {
                Width = (int)Math.Round(blobRectList.Average(f => f.BoundingRect.Width)),
                Height = (int)Math.Round(blobRectList.Average(f => f.BoundingRect.Height))
            };

            // 중심과 가장 가까이 있는 블랍을 가져온다.
            Point center = new Point()
            {
                X = (int)Math.Round(blobRectList.Average(f => f.CenterPt.X)),
                Y = (int)Math.Round(blobRectList.Average(f => f.CenterPt.Y))
            };
            List<Tuple<BlobRect, double>> tupleList = blobRectList.ConvertAll(f => new Tuple<BlobRect, double>(f, Math.Sqrt(Math.Pow(f.CenterPt.X - center.X, 2) + Math.Pow(f.CenterPt.Y - center.Y, 2))));
            tupleList.Sort((f, g) => f.Item2.CompareTo(g.Item2));
            BlobRect centerBlobRect = tupleList.First().Item1;

            // Rect간 거리가 ratio 이하인 블랍을 Union,
            float ratio = 0.8f;
            List<BlobRect> validBlobRectList = new List<BlobRect>();
            Rectangle rectangle = Rectangle.Round(centerBlobRect.BoundingRect);
            tupleList.ForEach(f =>
            {
                Rectangle blobRect = Rectangle.Round(f.Item1.BoundingRect);
                bool union = rectangle.IntersectsWith(blobRect);
                if (union == false)
                {
                    float diffL = Math.Max(0, rectangle.Left - blobRect.Right);
                    float diffT = Math.Max(0, rectangle.Top - blobRect.Bottom);
                    float diffR = Math.Max(0, blobRect.Left - rectangle.Right);
                    float diffB = Math.Max(0, blobRect.Top - rectangle.Bottom);

                    union = Math.Max(diffL, diffR) < meanSize.Width * ratio && Math.Max(diffT, diffB) < meanSize.Height * ratio;
                }

                if (union)
                {
                    rectangle = Rectangle.Union(rectangle, Rectangle.Round(f.Item1.BoundingRect));
                    validBlobRectList.Add(f.Item1);
                }

            });

            // 네 방향 꼭지점에 위치한 BlobRect를 찾는다.
            PointF[] refPt = new PointF[] {
                    new PointF(rectangle.Left, rectangle.Top),
                    new PointF(rectangle.Right, rectangle.Top),
                    new PointF(rectangle.Right, rectangle.Bottom),
                    new PointF(rectangle.Left, rectangle.Bottom)
                };

            Point[] vertexPoints = new Point[4];
            for (int i = 0; i < refPt.Length; i++)
            {
                BlobRect vertexBlobRect = validBlobRectList.OrderBy(f =>
                {
                    PointF pt1 = refPt[i];
                    PointF pt2 = DrawingHelper.GetPoints(f.BoundingRect, 0)[i];
                    return MathHelper.GetLength(pt1, pt2);
                }).FirstOrDefault();
                vertexPoints[i] = Point.Round(DrawingHelper.GetPoints(vertexBlobRect.BoundingRect, 0)[i]);
            }
            return vertexPoints;
        }
    }

    public class ExtractOperatorResult : OperatorResult
    {
        ScanOperatorResult scanOperatorResult;
        List<BlobRect> blobRectList;
        AlgoImage sheetBuffer;
        AlgoImage maskBuffer;
        BitmapSource sheetBufferBitmap;
        BitmapSource maskBufferBitmap;

        Rectangle sheetRect;
        Point[] vertexPoints;

        public List<BlobRect> BlobRectList { get => blobRectList; }
        public ScanOperatorResult ScanOperatorResult { get => scanOperatorResult; }
        public AlgoImage SheetBuffer { get => sheetBuffer; }
        public AlgoImage MaskBuffer { get => maskBuffer; }
        public BitmapSource SheetBufferBitmap { get => sheetBufferBitmap; }
        public BitmapSource MaskBufferBitmap { get => maskBufferBitmap; }

        public Rectangle SheetRect { get => sheetRect; }
        public Point[] VertexPoints { get => vertexPoints; }

        public int PatternCount { get => blobRectList == null ? 0 : blobRectList.Count; }

        public ExtractOperatorResult(ResultKey resultKey, ScanOperatorResult scanOperatorResult, 
            AlgoImage sheetBuffer, AlgoImage maskBuffer, BitmapSource sheetBufferBitmap, BitmapSource maskBufferBitmap,
            Rectangle sheetRect, List<BlobRect> blobRectList, Point[] vertexPoints)
            : base(ResultType.Extract, resultKey, null)
        {
            this.scanOperatorResult = scanOperatorResult;
            this.sheetBuffer = sheetBuffer;
            this.maskBuffer = maskBuffer;
            this.sheetBufferBitmap = sheetBufferBitmap;
            this.maskBufferBitmap = maskBufferBitmap;
            this.sheetRect = sheetRect;
            this.blobRectList = blobRectList;
            this.vertexPoints = vertexPoints;
        }

        public ExtractOperatorResult(ResultKey resultKey, ScanOperatorResult scanOperatorResult, string exeptionMessage) : base(ResultType.Extract, resultKey, exeptionMessage)
        {
            this.scanOperatorResult = scanOperatorResult;
        }
    }

    public class ExtractOperatorSettings : OperatorSettings
    {
        int maxCount = 50000;
        bool useMaxPatternLength = false;
        float minPatternLength = 100;
        float maxPatternLength = 10000;
        int maxMarginLength = 500;
       
        
        [CatecoryAttribute("Extract"), NameAttribute("Max Count")]
        public int MaxCount { get => maxCount; set => maxCount = value; }

        [CatecoryAttribute("Extract"), NameAttribute("Use Max Pattern Length")]
        public bool UseMaxPatternLength { get => useMaxPatternLength; set => useMaxPatternLength = value; }
        
        [CatecoryAttribute("Extract"), NameAttribute("Min Pattern Length")]
        public float MinPatternLength { get => minPatternLength; set => minPatternLength = value; }
        
        [CatecoryAttribute("Extract"), NameAttribute("Max Pattern Length")]
        public float MaxPatternLength { get => maxPatternLength; set => maxPatternLength = value; }

        [CatecoryAttribute("Extract"), NameAttribute("Max Margin Length")]
        public int MaxMarginLength { get => maxMarginLength; set => maxMarginLength = value; }

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Extract");
        }

        public override void Load(XmlElement xmlElement)
        {
            maxCount = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "MaxCount", "500"));
            minPatternLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinPatternLength", "500"));
            maxPatternLength = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MaxPatternLength", "10000"));
            useMaxPatternLength = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UseMaxPatternLength", "false"));
            maxMarginLength = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "MaxMarginLength", "10000"));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "MaxCount", maxCount.ToString());
            XmlHelper.SetValue(xmlElement, "MinPatternLength", minPatternLength.ToString());
            XmlHelper.SetValue(xmlElement, "MaxPatternLength", maxPatternLength.ToString());
            XmlHelper.SetValue(xmlElement, "UseMaxPatternLength", useMaxPatternLength.ToString());
            XmlHelper.SetValue(xmlElement, "MaxMarginLength", maxMarginLength.ToString());
        }
    }
}

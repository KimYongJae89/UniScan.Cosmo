using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.COSMO.Offline.Models;
using WPF.Base.Extensions;
using System.IO;
using DynMvp.Devices;
using System.Runtime.InteropServices;
using DynMvp.Vision.Matrox;
using WPF.COSMO.Offline.Controls;
using WPF.Base.Models;
using System.Collections;
using System.Windows.Media;
using Newtonsoft.Json;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls.ViewModel;
using DynMvp.Base;

namespace WPF.COSMO.Offline.Services
{
    public enum InspectSequence
    {
        Edge, Section, Inner
    }

    public class InspectResult
    {
        public List<AxisImageSource> SourceImageList = new List<AxisImageSource>();
        public List<ScanResult> ScanResultList { get; set; } = new List<ScanResult>();
        public List<EstimatedLine> Lines { get; set; } = new List<EstimatedLine>();

        public InspectResult()
        {

        }

        public InspectResult(IEnumerable<EstimatedLine> lines)
        {
            Lines.AddRange(lines);
        }
    }
    
    public class ScanResult
    {
        public int ImageNum { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public double ResizeRatio { get; set; }

        public System.Windows.Point AxisPosition { get; set; }
        public List<Defect> Defects { get; set; }

        public double Avg { get; set; }

        public ScanResult()
        {

        }

        public ScanResult(int imageWidth, int imageHeight, double resizeRatio, System.Windows.Point axisPosition, int imageNum, List<Defect> defects)
        {
            ResizeRatio = resizeRatio;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            AxisPosition = axisPosition;
            ImageNum = imageNum;
            Defects = defects;
            double sum = 0;
            foreach(Defect defect in Defects)
            {
                sum += defect.CenterPt.X;
            }
            Avg = sum / Defects.Count;
        }

        public ImageSource GetDefectImage()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                foreach (var defect in Defects)
                {
                    SolidColorBrush brush = new SolidColorBrush(defect is EdgeDefect ? Colors.Blue : Colors.Red);

                    StreamGeometry streamGeometry = new StreamGeometry();

                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        var converted = Array.ConvertAll(defect.Points, point => new System.Windows.Point((int)(point.X * AxisGrabService.Settings.ImageResizeRatio + 0.5), (int)(point.Y * AxisGrabService.Settings.ImageResizeRatio + 0.5)));

                        geometryContext.BeginFigure(converted[0], true, true);
                        geometryContext.PolyLineTo(converted, true, true);
                    }

                    drawingContext.DrawGeometry(brush, new System.Windows.Media.Pen(brush, 1), streamGeometry);
                }
            }

            int resizeWidth = (int)(ImageWidth * ResizeRatio);
            int resizeHeight = (int)(ImageHeight * ImageNum * ResizeRatio);

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(resizeWidth, resizeHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);

            renderTargetBitmap.Freeze();

            return renderTargetBitmap;
        }
    }

    public class InspectProcess
    {
        public Dictionary<double, ProcessUnit> InnerUnitDictionary { get; set; } = new Dictionary<double, ProcessUnit>();
        public Dictionary<AxisGrabInfo, Tuple<ProcessUnit, ProcessUnit>> SideUnitDictionary { get; set; } = new Dictionary<AxisGrabInfo, Tuple<ProcessUnit, ProcessUnit>>();
        public Dictionary<AxisGrabInfo, ObservableTuple<int, int, int, int>> BufferDictionary { get; set; } = new Dictionary<AxisGrabInfo, ObservableTuple<int, int, int, int>>();
        public ObservableTuple<bool, bool, bool, bool> EventTuple { get; set; } = new ObservableTuple<bool, bool, bool, bool>(false, false, false, false);

        public InspectProcess()
        {

            foreach (var position in SectionService.Settings.Selected.InspectPositionList)
                InnerUnitDictionary.Add(position, new ProcessUnit());

            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
            {
                SideUnitDictionary.Add(info, new Tuple<ProcessUnit, ProcessUnit>(new ProcessUnit(), new ProcessUnit()));
                BufferDictionary.Add(info, new ObservableTuple<int, int, int, int>(0, 0, 0, 0));
            }
        }
    }
    
    public delegate void InspectedDelegate(ScanResult scanResult);
    public delegate void FilmGrabbedDelegate(AxisImageSource axisImageSource);
    public delegate void DrawDefectsDoneDelegate(AxisImageSource axisImageSource);

    public static class InspectService
    {
        private static int defectIndex;

        public static FilmGrabbedDelegate FilmGrabbed;
        public static DrawDefectsDoneDelegate DrawDefectsDone;

        public static InspectedDelegate Inspected;
        public static EmptyDelegate InspectDone;

        public static double EdgeIgnoreLength { get; set; }
        
        static Dictionary<AxisGrabInfo, ManualResetEvent> _enableResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();
        
        static Dictionary<AxisGrabInfo, ManualResetEvent> _runResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();

        static Dictionary<AxisGrabInfo, ConcurrentQueue<AxisImage>> _validQueueDictionary = new Dictionary<AxisGrabInfo, ConcurrentQueue<AxisImage>>();
        static Dictionary<AxisGrabInfo, ManualResetEvent> _validResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();

        static Dictionary<AxisGrabInfo, ConcurrentQueue<AxisImage>> _edgeBlobQueueDictionary = new Dictionary<AxisGrabInfo, ConcurrentQueue<AxisImage>>();
        static Dictionary<AxisGrabInfo, ManualResetEvent> _edgeBlobResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();
        
        static Dictionary<AxisGrabInfo, ConcurrentQueue<Tuple<AxisImage, byte[]>>> _edgeSubBlobQueueDictionary = new Dictionary<AxisGrabInfo, ConcurrentQueue<Tuple<AxisImage, byte[]>>>();
        static Dictionary<AxisGrabInfo, ManualResetEvent> _edgeSubBlobResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();
        
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeSourceBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeLabelBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _lineMaskBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeSubSourceBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeSubLabelBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeSubRegionBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();


        static Dictionary<AxisGrabInfo, ConcurrentQueue<Tuple<AxisImage, byte[]>>> _innerBlobDictionary = new Dictionary<AxisGrabInfo, ConcurrentQueue<Tuple<AxisImage, byte[]>>>();
        static Dictionary<AxisGrabInfo, ManualResetEvent> _innerBlobResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();

        static Dictionary<AxisGrabInfo, ConcurrentDictionary<InspectSequence, ConcurrentQueue<Tuple<AxisImage, BlobRectList>>>> _mergeBlobDictionary = new Dictionary<AxisGrabInfo, ConcurrentDictionary<InspectSequence, ConcurrentQueue<Tuple<AxisImage, BlobRectList>>>>();
        
        static Dictionary<AxisGrabInfo, AlgoImage> _innerSourceBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _innerLabelBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _innerRegionBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();
        static Dictionary<AxisGrabInfo, AlgoImage> _edgeRegionBufferDictionary = new Dictionary<AxisGrabInfo, AlgoImage>();

        static Dictionary<AxisGrabInfo, Point> _startPositionDictionary = new Dictionary<AxisGrabInfo, Point>();
        static Dictionary<AxisGrabInfo, Point> _endPositionDictionary = new Dictionary<AxisGrabInfo, Point>();

        static Dictionary<AxisGrabInfo, ManualResetEvent> _positionResetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();

        static Dictionary<AxisGrabInfo, Tuple<ManualResetEvent, ManualResetEvent, ManualResetEvent, ManualResetEvent>> _endResetEventDictionary = new Dictionary<AxisGrabInfo, Tuple<ManualResetEvent, ManualResetEvent, ManualResetEvent, ManualResetEvent>>();
        
        static InspectProcess _inspectProcess;
        static CancellationToken _token;

        static InspectSequence _inspectSequence;

        static InspectResult _inspectResult;
        public static InspectResult InspectResult => _inspectResult;

        static bool IsRunningValid => _validQueueDictionary.Values.Any(queue => queue.Count > 0) || _validResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0));
        static bool IsRunningEdgeMain => _edgeBlobQueueDictionary.Values.Any(queue => queue.Count > 0) || _edgeBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0));
        static bool IsRunningEdgeSub => _edgeSubBlobQueueDictionary.Values.Any(queue => queue.Count > 0) || _edgeSubBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0));
        static bool IsRunningInner => _innerBlobDictionary.Values.Any(queue => queue.Count > 0) || _innerBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0));

        static Model_COSMO _model;

        public static Task InitializeAsync(Model_COSMO model, InspectProcess inspectProcess, CancellationToken token)
        {
            return Task.Run(() =>
            {
                _model = model;

                Release();

                _inspectResult = new InspectResult(AlignService.Lines);

                _token = token;
                _inspectProcess = inspectProcess;

                _inspectSequence = InspectSequence.Edge;

                foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
                {
                    info.Initialize();
                    
                    _runResetEventDictionary.Add(info, new ManualResetEvent(false));

                    _enableResetEventDictionary.Add(info, new ManualResetEvent(false));

                    _validQueueDictionary.Add(info, new ConcurrentQueue<AxisImage>());
                    //_tempStackDictionary.Add(info, new Stack<AxisImage>());
                    //_tempQueueDictionary.Add(info, new Queue<AxisImage>());
                    _validResetEventDictionary.Add(info, new ManualResetEvent(false));
                    
                    _edgeSourceBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _edgeLabelBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _lineMaskBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _edgeSubSourceBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _edgeSubLabelBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _edgeSubRegionBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));

                    _edgeRegionBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));

                    _innerSourceBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _innerLabelBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));
                    _innerRegionBufferDictionary.Add(info, ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(info.ImageWidth, info.ImageHeight)));


                    _innerBlobDictionary.Add(info, new ConcurrentQueue<Tuple<AxisImage, byte[]>>());
                    _innerBlobResetEventDictionary.Add(info, new ManualResetEvent(false));

                    _edgeBlobQueueDictionary.Add(info, new ConcurrentQueue<AxisImage>());
                    _edgeBlobResetEventDictionary.Add(info, new ManualResetEvent(false));

                    _edgeSubBlobQueueDictionary.Add(info, new ConcurrentQueue<Tuple<AxisImage, byte[]>>());
                    _edgeSubBlobResetEventDictionary.Add(info, new ManualResetEvent(false));
                    
                    _startPositionDictionary.Add(info, Point.Empty);
                    _endPositionDictionary.Add(info, Point.Empty);

                    _mergeBlobDictionary.Add(info, new ConcurrentDictionary<InspectSequence, ConcurrentQueue<Tuple<AxisImage, BlobRectList>>>());
                    _mergeBlobDictionary[info].TryAdd(InspectSequence.Edge, new ConcurrentQueue<Tuple<AxisImage, BlobRectList>>());
                    _mergeBlobDictionary[info].TryAdd(InspectSequence.Inner, new ConcurrentQueue<Tuple<AxisImage, BlobRectList>>());
                    
                    _positionResetEventDictionary.Add(info, new ManualResetEvent(false));

                    _endResetEventDictionary.Add(info, new Tuple<ManualResetEvent, ManualResetEvent, ManualResetEvent, ManualResetEvent>(new ManualResetEvent(false), new ManualResetEvent(false), new ManualResetEvent(false), new ManualResetEvent(false)));

                    _runResetEventDictionary[info].Set();
                }

                foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
                {
                    StartValidTask(info);
                    StartEdgeDefectTask(info);
                    StartInnerDefectTask(info);
                }

                AxisGrabService.AxisImageGrabbed += AxisImageGrabbed;
            });
        }

        private static void Reset()
        {
            foreach (var resetEvent in _innerBlobResetEventDictionary.Values)
                resetEvent.Reset();

            foreach (var resetEvent in _validResetEventDictionary.Values)
                resetEvent.Reset();

            foreach (var resetEvent in _edgeBlobResetEventDictionary.Values)
                resetEvent.Reset();

            foreach (var resetEvent in _edgeSubBlobResetEventDictionary.Values)
                resetEvent.Reset();
        }

        private static void Set()
        {
            foreach (var resetEvent in _validResetEventDictionary.Values)
                resetEvent.Set();

            foreach (var resetEvent in _edgeBlobResetEventDictionary.Values)
                resetEvent.Set();

            foreach (var resetEvent in _edgeSubBlobResetEventDictionary.Values)
                resetEvent.Set();

            foreach (var resetEvent in _innerBlobResetEventDictionary.Values)
                resetEvent.Set();
        }

        public static void Release()
        {
            Set();

            defectIndex = 0;

            _inspectResult = null;

            _enableResetEventDictionary.Clear();
            
            foreach (var buffer in _edgeSourceBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _edgeLabelBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _lineMaskBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _edgeSubLabelBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _edgeSubSourceBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _edgeSubRegionBufferDictionary.Values)
                buffer.Dispose();

            
            foreach (var buffer in _edgeRegionBufferDictionary.Values)
                buffer.Dispose();

            _edgeSourceBufferDictionary.Clear();
            _edgeLabelBufferDictionary.Clear();
            _lineMaskBufferDictionary.Clear();
            _edgeSubLabelBufferDictionary.Clear();
            _edgeSubSourceBufferDictionary.Clear();
            _edgeRegionBufferDictionary.Clear();
            _edgeSubRegionBufferDictionary.Clear();
            //_tempStackDictionary.Clear();
            //_tempQueueDictionary.Clear();

            foreach (var buffer in _innerSourceBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _innerLabelBufferDictionary.Values)
                buffer.Dispose();
            foreach (var buffer in _innerRegionBufferDictionary.Values)
                buffer.Dispose();

            _innerSourceBufferDictionary.Clear();
            _innerLabelBufferDictionary.Clear();
            _innerRegionBufferDictionary.Clear();

            _runResetEventDictionary.Clear();
            
            _validQueueDictionary.Clear();
            _validResetEventDictionary.Clear();
            
            _innerBlobDictionary.Clear();
            _innerBlobResetEventDictionary.Clear();

            _edgeBlobQueueDictionary.Clear();
            _edgeBlobResetEventDictionary.Clear();

            _edgeSubBlobQueueDictionary.Clear();
            _edgeSubBlobResetEventDictionary.Clear();
            
            _startPositionDictionary.Clear();
            _endPositionDictionary.Clear();

            _positionResetEventDictionary.Clear();

            _mergeBlobDictionary.Clear();

            _endResetEventDictionary.Clear();

            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed;
        }

        private static async Task Cancel()
        {
            foreach (var resetEvent in _runResetEventDictionary.Values)
                resetEvent.Reset();

            Set();

            WaitEndTask();

            foreach (var resetEvent in _runResetEventDictionary.Values)
                resetEvent.Set();

            await WaitAll();

            foreach (var dictionary in _mergeBlobDictionary.Values)
            {
                foreach (var queue in dictionary.Values)
                {
                    while (queue.IsEmpty == false)
                    {
                        if (queue.TryDequeue(out Tuple<AxisImage, BlobRectList> temp))
                            temp.Item2.Dispose();
                    }
                }
            }
        }

        private static void WaitEndTask()
        {
            foreach (var tuple in _endResetEventDictionary.Values)
            {
                tuple.Item1.WaitOne();
                tuple.Item2.WaitOne();
                tuple.Item3.WaitOne();
                tuple.Item4.WaitOne();
            }
        }

        private static async Task WaitResetEvent()
        {
            while (_validResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)) 
            || _edgeBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0))
            || _edgeSubBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0))
            || _innerBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)))
            {
                if (_validResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)))
                {

                }
                if (_edgeBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)))
                {

                }
                if (_edgeSubBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)))
                {

                }

                if (_innerBlobResetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0)))
                {

                }

                await Task.Delay(500);
            }
        }

        private static async Task WaitAll()
        {
            while (IsRunningValid || IsRunningEdgeMain || IsRunningEdgeSub || IsRunningInner)
            {
                _inspectProcess.EventTuple.Item1 = IsRunningValid;
                _inspectProcess.EventTuple.Item2 = IsRunningEdgeMain;
                _inspectProcess.EventTuple.Item3 = IsRunningEdgeSub;
                _inspectProcess.EventTuple.Item4 = IsRunningInner;

                await Task.Delay(500);
            }

            _inspectProcess.EventTuple.Item1 = IsRunningValid;
            _inspectProcess.EventTuple.Item2 = IsRunningEdgeMain;
            _inspectProcess.EventTuple.Item3 = IsRunningEdgeSub;
            _inspectProcess.EventTuple.Item4 = IsRunningInner;
        }

        public static async Task<bool> InspectionAsync()
        {
            _inspectSequence = InspectSequence.Edge;

            var settings = AxisGrabService.Settings;

            foreach (var resetEvent in _runResetEventDictionary.Values)
                resetEvent.Set();
            
            double maxDist = SectionService.Settings.Selected.DefectDistanceList.Max();

            var rightInfo = settings.AxisGrabInfoList.OrderByDescending(info => info.OffsetX).First();
            int offset = 1000;


            double startY = double.MaxValue;
            double endY = 0;

            foreach (var info in settings.AxisGrabInfoList)
            {
                var length = SectionService.Settings.Selected.InspectScanLength * 1000;

                var a = info.RightLineModel.GetDistPt(-length / 2).Y;
                var b = info.LeftLineModel.GetDistPt(-length / 2).Y;

                startY = Math.Min(startY, Math.Min(info.RightLineModel.GetDistPt(-length / 2).Y, info.LeftLineModel.GetDistPt(-length / 2).Y));
                endY = Math.Max(endY, Math.Max(info.RightLineModel.GetDistPt(length / 2).Y, info.LeftLineModel.GetDistPt(length / 2).Y));
            }

            AxisGrabService.NextY = (int)(startY - offset);

            foreach (var process in _inspectProcess.SideUnitDictionary.Values)
            {
                process.Item1.Processing = true;
                process.Item2.Processing = true;
            }

            if (await AxisGrabService.MoveNextPos() == false)
            {
                foreach (var process in _inspectProcess.SideUnitDictionary.Values)
                {
                    process.Item1.Fail = true;
                    process.Item2.Fail = true;
                }

                await Cancel();
                return false;
            }

            var temp1 = (endY - startY + offset) % (settings.Resolution * rightInfo.ImageHeight);
            AxisGrabService.NextY = (int)(endY + temp1 + (settings.Resolution * rightInfo.ImageHeight / 2));

            if (await AxisGrabService.GrabNextPos() == false)
            {
                foreach (var process in _inspectProcess.SideUnitDictionary.Values)
                {
                    process.Item1.Fail = false;
                    process.Item2.Fail = false;
                }

                return false;
            }
            
            while (IsRunningValid || IsRunningEdgeMain || IsRunningEdgeSub)
            {
                _inspectProcess.EventTuple.Item1 = IsRunningValid;
                _inspectProcess.EventTuple.Item2 = IsRunningEdgeMain;
                _inspectProcess.EventTuple.Item3 = IsRunningEdgeSub;

                await Task.Delay(500);
            }

            _inspectProcess.EventTuple.Item1 = IsRunningValid;
            _inspectProcess.EventTuple.Item2 = IsRunningEdgeMain;
            _inspectProcess.EventTuple.Item3 = IsRunningEdgeSub;

            if (_token.IsCancellationRequested)
            {
                foreach (var info in settings.AxisGrabInfoList)
                {
                    _inspectProcess.SideUnitDictionary[info].Item1.Fail = true;
                    _inspectProcess.SideUnitDictionary[info].Item2.Fail = true;
                }

                await Cancel();

                return false;
            }

            foreach (var info in settings.AxisGrabInfoList)
            {
                await BlobMergeAsync(info);
                _inspectProcess.SideUnitDictionary[info].Item1.Success = true;
            }
            
            _inspectSequence = InspectSequence.Section;

            await WaitAll();
            
            if (_token.IsCancellationRequested)
            {
                foreach (var info in settings.AxisGrabInfoList)
                    _inspectProcess.SideUnitDictionary[info].Item2.Fail = true;

                return false;
            }

            foreach (var info in settings.AxisGrabInfoList)
            {
                await BlobMergeAsync(info);
                _inspectProcess.SideUnitDictionary[info].Item2.Success = true;
            }

            ///////////////////////////
            //await Cancel();

            //AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed;

            //if (InspectDone != null)
            //    InspectDone();

            //return true;

            ///////////////////////////

            _inspectSequence = InspectSequence.Inner;
            
            var leftInfo = settings.AxisGrabInfoList.OrderByDescending(info => info.OffsetX).Last();
            leftInfo.NextX = 0;
            

            foreach (var position in SectionService.Settings.Selected.InspectPositionList)
            {
                _inspectProcess.InnerUnitDictionary[position].Processing = true;

                rightInfo.LeftLineModel = rightInfo.LineModel.GetParallelLine((-position - (maxDist / 2)) * 1000);
                rightInfo.RightLineModel = rightInfo.LineModel.GetParallelLine((-position + (maxDist / 2)) * 1000);


                PointF leftStartPt = rightInfo.LeftLineModel.GetDistPt(-SectionService.Settings.Selected.InspectScanLength * 1000 / 2);
                PointF leftEndPt = rightInfo.LeftLineModel.GetDistPt(SectionService.Settings.Selected.InspectScanLength * 1000 / 2);

                PointF rightStartPt = rightInfo.RightLineModel.GetDistPt(-SectionService.Settings.Selected.InspectScanLength * 1000 / 2);
                PointF rightEndPt = rightInfo.RightLineModel.GetDistPt(SectionService.Settings.Selected.InspectScanLength * 1000 / 2);

                float minX = Math.Min(leftStartPt.X, leftEndPt.X);
                float maxX = Math.Max(rightStartPt.X, rightEndPt.X);
                float minY = Math.Min(leftStartPt.Y, rightStartPt.Y);
                float maxY = Math.Max(leftEndPt.Y, rightEndPt.Y);

                rightInfo.NextX = (int)((minX + maxX) / 2 - (rightInfo.ImageWidth * settings.Resolution) / 2) - rightInfo.OffsetX;

                var temp = (maxY - minY + offset) % (settings.Resolution * rightInfo.ImageHeight);
                AxisGrabService.NextY = (int)(minY - offset);
                if (await AxisGrabService.MoveNextPos() == false)
                {
                    _inspectProcess.InnerUnitDictionary[position].Fail = true;
                    await Cancel();
                    return false;
                }
                
                AxisGrabService.NextY = (int)(maxY + temp + (settings.Resolution * rightInfo.ImageHeight / 2));
                if (await AxisGrabService.GrabNextPos(rightInfo) == false)
                {
                    _inspectProcess.InnerUnitDictionary[position].Fail = true;
                    await Cancel();
                    return false;
                }

                await WaitAll();

                if (_token.IsCancellationRequested)
                {
                    _inspectProcess.InnerUnitDictionary[position].Fail = true;
                    await Cancel();
                    return false;
                }

                await BlobMergeAsync(rightInfo);
                _inspectProcess.InnerUnitDictionary[position].Success = true;
            }

            await Cancel();

            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed;

            int index = 1;

            if (InspectDone != null)
            {


                ////각 scan별 평균 위치 확인 후 left, right, center 순으로 index 매기기
                ////for(int i = 0; i < _inspectResult.ScanResultList.Count; i++)
                ////{
                ////    LogHelper.Debug(LoggerType.Machine, string.Format("LHJ {0}", _inspectResult.ScanResultList[i].AxisPosition.X));
                ////}

                //List<ScanResult> tempList = _inspectResult.ScanResultList.OrderBy(x => x.AxisPosition.X).ToList();

                //List<ScanResult> realList = tempList;

                //ScanResult scanResult = realList[realList.Count - 1];
                //realList[realList.Count - 1] = realList[realList.Count / 2];
                //realList[realList.Count / 2] = scanResult;

                //scanResult = realList[realList.Count / 2];
                //realList[realList.Count / 2] = realList[(realList.Count / 2) + 1];
                //realList[(realList.Count / 2) + 1] = scanResult;

                //for (int i = 0; i < _inspectResult.ScanResultList.Count; i++)
                //{
                //    LogHelper.Debug(LoggerType.Machine, string.Format("LHJ {0}", realList[i].AxisPosition.X));
                //}

                //foreach (ScanResult result in realList)
                //{
                //    List<Defect> defectList = result.Defects.OrderBy(x => x.CenterPt.X).ToList();
                //    foreach (Defect defect in defectList)
                //    {
                //        defect.Index = index;
                //        index++;
                //    }

                //    result.Defects = defectList;
                //}

                //_inspectResult.ScanResultList = realList;

                ////LogHelper.Debug(LoggerType.Machine, string.Format("LHJ {0}", realList.Count));
                ///







                //List<ScanResult> scanResults = _inspectResult.ScanResultList.OrderBy(x => x.AxisPosition.X).ToList();

                //List<ScanResult> scanResults2 = new List<ScanResult>();

                //_inspectResult.ScanResultList.Clear();

                //if (AxisGrabService.IsSingleMode == false)
                //{
                //    _inspectResult.ScanResultList.Add(scanResults[0]);
                //    _inspectResult.ScanResultList.Add(scanResults[1]);
                //    _inspectResult.ScanResultList.Add(scanResults[5]);
                //    _inspectResult.ScanResultList.Add(scanResults[6]);
                //    _inspectResult.ScanResultList.Add(scanResults[2]);
                //    _inspectResult.ScanResultList.Add(scanResults[3]);
                //    _inspectResult.ScanResultList.Add(scanResults[4]);
                //}
                //else
                //{
                //    _inspectResult.ScanResultList.Add(scanResults[3]);
                //    _inspectResult.ScanResultList.Add(scanResults[4]);
                //    _inspectResult.ScanResultList.Add(scanResults[0]);
                //    _inspectResult.ScanResultList.Add(scanResults[1]);
                //    _inspectResult.ScanResultList.Add(scanResults[2]);
                //}

                //int defectIndex = 1;
                //foreach (ScanResult scanResult in _inspectResult.ScanResultList)
                //{
                //    foreach (Defect defect in scanResult.Defects)
                //    {
                //        defect.Index = defectIndex;
                //        defectIndex++;
                //    }
                //}


                lock (_inspectResult.ScanResultList)
                {
                    if (AxisGrabService.IsSingleMode)
                    {
                        int defectIndex = 1;
                        foreach (Defect defect in _inspectResult.ScanResultList[0].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[1].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[4].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[3].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[2].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }
                    }
                    else
                    {
                        int defectIndex = 1;
                        foreach (Defect defect in _inspectResult.ScanResultList[0].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[2].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[1].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[3].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[4].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[5].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }

                        foreach (Defect defect in _inspectResult.ScanResultList[6].Defects)
                        {
                            defect.Index = defectIndex;
                            defectIndex++;
                        }
                    }
                }
                //int defectIndex = 1;
                //foreach (ScanResult scanResult in _inspectResult.ScanResultList)
                //{
                //    foreach (Defect defect in scanResult.Defects)
                //    {
                //        defect.Index = defectIndex;
                //        defectIndex++;
                //    }
                //}





                InspectDone();
            }

            return true;
        }

        private static void AxisImageGrabbed(object sender, AxisImage axisImage)
        {
            var info = sender as AxisGrabInfo;
            
            double ratio = (double)axisImage.Data.Count(d => d > AlignService.ThresholdDictionary[info]) / axisImage.Data.Length;

            if (ratio < 0.05)
            {
                if (_positionResetEventDictionary[info].WaitOne(0))
                {
                    if (_endPositionDictionary[info] == null)
                        _endPositionDictionary[info] = new Point((int)axisImage.X, (int)axisImage.Y);
                    _positionResetEventDictionary[info].Reset();
                }

                return;
            }

            if (_positionResetEventDictionary[info].WaitOne(0) == false)
            {
                if (_startPositionDictionary[info] == null)
                    _startPositionDictionary[info] = new Point((int)axisImage.X, (int)axisImage.Y);
                _positionResetEventDictionary[info].Set();
            }

            if (_positionResetEventDictionary[info].WaitOne(0) == false)
                return;

            _validQueueDictionary[info].Enqueue(axisImage);
            _validResetEventDictionary[info].Set();
        }

        private static void StartValidTask(AxisGrabInfo info)
        {
            var settings = AxisGrabService.Settings;

            var runResetEvent = _runResetEventDictionary[info];

            var validQueue = _validQueueDictionary[info];
            var validResetEvent = _validResetEventDictionary[info];

            var edgeBlobQueue = _edgeBlobQueueDictionary[info];
            var edgeBlobResetEvent = _edgeBlobResetEventDictionary[info];

            var innerBlobQueue = _innerBlobDictionary[info];
            var innerBlobResetEvent = _innerBlobResetEventDictionary[info];

            var width = info.ImageWidth;
            var height = info.ImageHeight;

            Task.Factory.StartNew(() =>
            {
                while (runResetEvent.WaitOne(0))
                {
                    if (validQueue.IsEmpty)
                    {
                        lock (validResetEvent)
                            validResetEvent.Reset();
                    }

                    validResetEvent.WaitOne();

                    if (validQueue.TryDequeue(out AxisImage temp))
                    {
                        byte[] resizeData = new byte[(int)(width * height * settings.ImageResizeRatio)];

                        int mul = (int)(1.0 / settings.ImageResizeRatio);

                        int resizeWidth = (int)(width * settings.ImageResizeRatio);
                        int resizeHeight = (int)(height * settings.ImageResizeRatio);
                        
                        for (int yIndex = 0, destIndex = 0; yIndex < resizeHeight; yIndex++)
                        {
                            for (int xIndex = 0, srcindex = yIndex * width * mul; xIndex < resizeWidth; xIndex++, srcindex += mul, destIndex++)
                            {
                                resizeData[destIndex] = temp.Data[srcindex];
                            }
                        }

                        BitmapSource image = BitmapSource.Create(resizeWidth, resizeHeight,
                                                    96, 96, System.Windows.Media.PixelFormats.Gray8, null,
                                                    resizeData, resizeWidth);

                        image.Freeze();
                        var sourceImage = new AxisImageSource(new System.Windows.Point(temp.X, temp.Y), image);
                        if (FilmGrabbed != null)
                            FilmGrabbed(sourceImage);

                        lock (_inspectResult.SourceImageList)
                            _inspectResult.SourceImageList.Add(sourceImage);

                        switch (_inspectSequence)
                        {
                            case InspectSequence.Edge:
                            case InspectSequence.Section:
                                if (runResetEvent.WaitOne(0))
                                {
                                    edgeBlobQueue.Enqueue(temp);
                                    edgeBlobResetEvent.Set();
                                }
                                break;
                            case InspectSequence.Inner:
                                if (runResetEvent.WaitOne(0))
                                {
                                    innerBlobQueue.Enqueue(new Tuple<AxisImage, byte[]>(temp, null));
                                    innerBlobResetEvent.Set();
                                }
                                break;
                        }

                        _inspectProcess.BufferDictionary[info].Item1 = validQueue.Count;
                    }
                }

                _endResetEventDictionary[info].Item1.Set();

                runResetEvent.WaitOne();

                validResetEvent.Reset();

            }, TaskCreationOptions.LongRunning);
        }

        static void StartEdgeDefectTask(AxisGrabInfo info)
        {
            var settings = AxisGrabService.Settings;

            var runResetEvent = _runResetEventDictionary[info];

            var edgeBlobQueue = _edgeBlobQueueDictionary[info];
            var edgeBlobResetEvent = _edgeBlobResetEventDictionary[info];

            var edgeSubBlobQueue = _edgeSubBlobQueueDictionary[info];
            var edgeSubBlobResetEvent = _edgeSubBlobResetEventDictionary[info];

            var lineMaskBuffer = _lineMaskBufferDictionary[info];

            var edgeSrcBuffer = _edgeSourceBufferDictionary[info];
            var edgeLabelBuffer = _edgeLabelBufferDictionary[info];
            var edgeRegionBuffer = _edgeRegionBufferDictionary[info];

            var edgeSubSrcBuffer = _edgeSubSourceBufferDictionary[info];
            var edgeSubLabelBuffer = _edgeSubLabelBufferDictionary[info];
            var edgeSubRegionBuffer = _edgeSubRegionBufferDictionary[info];


            var innerBlobQueue = _innerBlobDictionary[info];
            var innerBlobResetEvent = _innerBlobResetEventDictionary[info];
            MilImageProcessing imageProcessing = new MilImageProcessing();

            double outerMinLength = Model_COSMO.Param.EdgeOuterMinLengthUM / settings.Resolution;
            double innerMinLength = Model_COSMO.Param.EdgeInnerMinLengthUM / settings.Resolution;

            BlobParam edgeBlobParam = new BlobParam();
            edgeBlobParam.SelectLabelValue = true;
            edgeBlobParam.SelectRotateRect = true;

            BlobParam edgeSubBlobParam = new BlobParam();

            BlobParam defectBlobParam = new BlobParam();
            defectBlobParam.SelectCenterPt = true;
            defectBlobParam.SelectMinValue = true;
            defectBlobParam.SelectMeanValue = true;
            defectBlobParam.SelectMaxValue = true;
            defectBlobParam.SelectRotateRect = true;

            var sourceRegion = new Rectangle(0, 0, info.ImageWidth, info.ImageHeight);

            BlobParam regionBlobParam = new BlobParam();
            regionBlobParam.SelectLabelValue = true;

            DrawBlobOption drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;

            int width = info.ImageWidth;
            int height = info.ImageHeight;
            
            LineModel lineModelEdge = null;
            LineModel lineModelMaxDist = null;
            switch (info.ScanDirection)
            {
                case ScanDirection.LeftToRight:
                    lineModelEdge = info.LeftLineModel;
                    lineModelMaxDist = info.LeftLineModel.GetParallelLine(settings.EdgeMaxDist);
                    break;
                case ScanDirection.RightToLeft:
                    lineModelEdge = info.RightLineModel;
                    lineModelMaxDist = info.RightLineModel.GetParallelLine(-settings.EdgeMaxDist);
                    break;
            }
            

            //int edgeBinValue = Model_COSMO.CalibrationValue + Model_COSMO.Param.EdgeBinarizeValue;
            int innerBinValue = Model_COSMO.CalibrationValue + (int)Math.Round(Model_COSMO.Param.InnerBinarizeValue * CalibrationService.CalibrationValueDictionary[info]);

            innerBinValue = innerBinValue > 255 ? 255 : Model_COSMO.CalibrationValue + innerBinValue;
            innerBinValue = innerBinValue < Model_COSMO.CalibrationValue + settings.BinaryMinValue ? Model_COSMO.CalibrationValue + settings.BinaryMinValue : Model_COSMO.CalibrationValue + innerBinValue;

            Task.Factory.StartNew(() =>
            {
                while (runResetEvent.WaitOne(0))
                {
                    if (edgeBlobQueue.IsEmpty)
                    {
                        lock (edgeBlobResetEvent)
                            edgeBlobResetEvent.Reset();
                    }

                    edgeBlobResetEvent.WaitOne();

                    if (edgeBlobQueue.TryDequeue(out AxisImage axisImage))
                    {
                        var startX = (lineModelEdge.GetX((int)axisImage.Y) - axisImage.X) / settings.Resolution;
                        var endX = (lineModelEdge.GetX((int)(axisImage.Y + (height * settings.Resolution - 1))) - axisImage.X) / settings.Resolution;

                        lineMaskBuffer.Clear();

                        imageProcessing.DrawLine(lineMaskBuffer, new PointF((float)(startX), 0), new PointF((float)(endX), height - 1), 255);
                        imageProcessing.Dilate(lineMaskBuffer, settings.EdgeDilate);

                        edgeSrcBuffer.PutByte(axisImage.Data);
                        
                        imageProcessing.Binarize(edgeSrcBuffer, edgeLabelBuffer, (int)Math.Round(AlignService.ThresholdDictionary[info]));

                        var regionBlobRectList = imageProcessing.Blob(edgeLabelBuffer, regionBlobParam);

                        edgeRegionBuffer.Clear();
                        imageProcessing.DrawBlob(edgeRegionBuffer, regionBlobRectList, regionBlobRectList.GetMaxAreaBlob(), drawBlobOption);
                        regionBlobRectList.Dispose();

                        //imageProcessing.FillHoles(edgeRegionBuffer, edgeRegionBuffer);

                        //var lineBlobRectList = imageProcessing.Blob(edgeLabelBuffer, regionBlobParam);
                        //lineBlobRectList.Dispose();

                        //var lineRegion = Rectangle.Round(lineBlobRectList.GetMaxAreaBlob().BoundingRect);
                        //var edgeSrcSubBuffer = edgeSrcBuffer.GetSubImage(lineRegion);
                        //var edgeLabelSubBuffer = edgeLabelBuffer.GetSubImage(lineRegion);
                        //var edgeRegionSubBuffer = edgeRegionBuffer.GetSubImage(lineRegion);

                        //imageProcessing.Binarize(edgeSrcSubBuffer, edgeLabelSubBuffer, Model_COSMO.CalibrationValue - settings.RegionBinAddValue);

                        //var lineRegionBlobRectList = imageProcessing.Blob(edgeLabelSubBuffer, regionBlobParam);

                        //edgeLabelSubBuffer.Clear();
                        //imageProcessing.DrawBlob(edgeLabelSubBuffer, lineRegionBlobRectList, lineRegionBlobRectList.GetMaxAreaBlob(), drawBlobOption);
                        //lineBlobRectList.Dispose();

                        //imageProcessing.FillHoles(edgeLabelSubBuffer, edgeLabelSubBuffer);
                        //imageProcessing.And(edgeRegionSubBuffer, edgeLabelSubBuffer, edgeRegionSubBuffer);

                        //edgeSrcSubBuffer.Dispose();
                        //edgeLabelSubBuffer.Dispose();
                        //edgeRegionSubBuffer.Dispose();

                        if (runResetEvent.WaitOne(0))
                        {
                            edgeSubBlobQueue.Enqueue(new Tuple<AxisImage, byte[]>(axisImage, edgeRegionBuffer.GetByte()));
                            edgeSubBlobResetEvent.Set();

                            imageProcessing.Subtract(edgeRegionBuffer, lineMaskBuffer, edgeRegionBuffer);

                            innerBlobQueue.Enqueue(new Tuple<AxisImage, byte[]>(axisImage, edgeRegionBuffer.GetByte()));
                            innerBlobResetEvent.Set();
                        }

                        _inspectProcess.BufferDictionary[info].Item2 = edgeBlobQueue.Count;
                    }
                }

                _endResetEventDictionary[info].Item2.Set();

                runResetEvent.WaitOne();

                while (!edgeBlobQueue.IsEmpty)
                    edgeBlobQueue.TryDequeue(out AxisImage temp);

                edgeBlobResetEvent.Reset();

            }, TaskCreationOptions.LongRunning);

            var mergeBlobEdgeQueue = _mergeBlobDictionary[info][InspectSequence.Edge];

            Task.Factory.StartNew(() =>
            {
                while (runResetEvent.WaitOne(0))
                {
                    if (edgeSubBlobQueue.IsEmpty)
                    {
                        lock (edgeSubBlobResetEvent)
                            edgeSubBlobResetEvent.Reset();
                    }

                    edgeSubBlobResetEvent.WaitOne();

                    if (edgeSubBlobQueue.TryDequeue(out var tuple))
                    {
                        var regionData = tuple.Item2;
                        edgeSubRegionBuffer.SetByte(regionData);

                        double[] outerArray = new double[edgeSubLabelBuffer.Height];
                        
                        for (int y = 0; y < edgeSubLabelBuffer.Height; y++)
                        {
                            switch (info.ScanDirection)
                            {
                                case ScanDirection.LeftToRight:
                                    for (int x = 0, index = y * width; x < edgeSubLabelBuffer.Width; x++, index++)
                                    {
                                        if (regionData[index] > 0)
                                        {
                                            outerArray[y] = x;
                                            break;
                                        }
                                    }
                                    break;
                                case ScanDirection.RightToLeft:
                                    for (int x = edgeSubLabelBuffer.Width - 1, index = y * width + edgeSubLabelBuffer.Width - 1; x >= 0; x--, index--)
                                    {
                                        if (regionData[index] > 0)
                                        {
                                            outerArray[y] = x;
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }

                        Array.Clear(regionData, 0, regionData.Length);

                        for (int y = 0; y < outerArray.Length; y++)
                        {
                            int yStartIndex = Math.Max(0, y - settings.AverageLength);
                            int yEndIndex = Math.Min(outerArray.Length - 1, y + settings.AverageLength);

                            if (outerArray.Skip(yStartIndex).Take(yEndIndex - yStartIndex).Where(outer => outer > 0).Count() == 0)
                                continue;

                            double avgOuter = outerArray.Skip(yStartIndex).Take(yEndIndex - yStartIndex).Where(outer => outer > 0).Average();

                            switch (info.ScanDirection)
                            {
                                case ScanDirection.LeftToRight:
                                    if (outerArray[y] < avgOuter - outerMinLength)
                                    {
                                        int index = y * edgeSubLabelBuffer.Width;
                                        for (int i = (int)outerArray[y]; i < avgOuter; i++)
                                        {
                                            regionData[index + i] = 255;
                                        }
                                    }
                                    break;
                                case ScanDirection.RightToLeft:
                                    if (outerArray[y] > avgOuter + outerMinLength)
                                    {
                                        int index = y * edgeSubLabelBuffer.Width;
                                        for (int i = (int)Math.Round(avgOuter); i < (int)outerArray[y]; i++)
                                        {
                                            regionData[index + i] = 255;
                                        }
                                    }
                                    break;
                            }
                            
                        }

                        edgeSubLabelBuffer.SetByte(regionData);

                        imageProcessing.And(edgeSubLabelBuffer, edgeSubRegionBuffer, edgeSubLabelBuffer);

                        imageProcessing.Dilate(edgeSubLabelBuffer, settings.DefectClose);
                        imageProcessing.Erode(edgeSubLabelBuffer, settings.DefectClose);

                        edgeSubSrcBuffer.SetByte(tuple.Item1.Data);
                        var blobRectList = imageProcessing.Blob(edgeSubLabelBuffer, defectBlobParam, edgeSubSrcBuffer);
                        
                        mergeBlobEdgeQueue.Enqueue(new Tuple<AxisImage, BlobRectList>(tuple.Item1, blobRectList));

                        _inspectProcess.BufferDictionary[info].Item3 = edgeSubBlobQueue.Count;
                    }
                }

                _endResetEventDictionary[info].Item3.Set();
                runResetEvent.WaitOne();

                while (!edgeSubBlobQueue.IsEmpty)
                    edgeSubBlobQueue.TryDequeue(out var temp);

                edgeSubBlobResetEvent.Reset();
            }, TaskCreationOptions.LongRunning);
        }

        static void StartInnerDefectTask(AxisGrabInfo info)
        {
            var mergeBlobInnerQueue = _mergeBlobDictionary[info][InspectSequence.Inner];

            var settings = AxisGrabService.Settings;

            var runResetEvent = _runResetEventDictionary[info];

            var innerBlobQueue = _innerBlobDictionary[info];
            var innerBlobResetEvent = _innerBlobResetEventDictionary[info];

            var innerBlobSrcBuffer = _innerSourceBufferDictionary[info];
            var innerBlobLabelBuffer = _innerLabelBufferDictionary[info];
            var innerRegionBuffer = _innerRegionBufferDictionary[info];
            
            MilImageProcessing imageProcessing = new MilImageProcessing();

            double minLength = Model_COSMO.Param.InnerMinLengthUM / settings.Resolution;

            BlobParam regionBlobParam = new BlobParam();
            regionBlobParam.SelectLabelValue = true;
           
            BlobParam innerDefectBlobParam = new BlobParam();
            innerDefectBlobParam.SelectCenterPt = true;
            innerDefectBlobParam.SelectMinValue = true;
            innerDefectBlobParam.SelectMeanValue = true;
            innerDefectBlobParam.SelectMaxValue = true;
            innerDefectBlobParam.SelectRotateRect = true;

            DrawBlobOption drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;

            var sourceRegion = new Rectangle(0, 0, info.ImageWidth, info.ImageHeight);

            int innerBinValue = Model_COSMO.CalibrationValue + (int)Math.Round(Model_COSMO.Param.InnerBinarizeValue * CalibrationService.CalibrationValueDictionary[info]);

            innerBinValue = innerBinValue > 255 ? 255 : Model_COSMO.CalibrationValue + innerBinValue;
            innerBinValue = innerBinValue < Model_COSMO.CalibrationValue + settings.BinaryMinValue ? Model_COSMO.CalibrationValue + settings.BinaryMinValue : Model_COSMO.CalibrationValue + innerBinValue;

            int width = info.ImageWidth;
            int height = info.ImageHeight;
            Task.Factory.StartNew(() =>
            {
                while (runResetEvent.WaitOne(0))
                {
                    if (innerBlobQueue.IsEmpty)
                    {
                        lock (innerBlobResetEvent)
                            innerBlobResetEvent.Reset();
                    }

                    innerBlobResetEvent.WaitOne();

                    if (innerBlobQueue.TryDequeue(out Tuple<AxisImage, byte[]> tuple))
                    {
                        var axisImage = tuple.Item1;
                        var regionData = tuple.Item2;

                        innerBlobSrcBuffer.PutByte(axisImage.Data);
                        imageProcessing.Binarize(innerBlobSrcBuffer, innerBlobLabelBuffer, innerBinValue);
                        
                        if (regionData != null)
                        {
                            innerRegionBuffer.PutByte(regionData);
                            imageProcessing.And(innerBlobLabelBuffer, innerRegionBuffer, innerBlobLabelBuffer);
                        }
                        
                        imageProcessing.Dilate(innerBlobLabelBuffer, settings.DefectClose);
                        imageProcessing.Erode(innerBlobLabelBuffer, settings.DefectClose);

                        BlobRectList innerDefectRectList = imageProcessing.Blob(innerBlobLabelBuffer, innerDefectBlobParam, innerBlobSrcBuffer);

                        mergeBlobInnerQueue.Enqueue(new Tuple<AxisImage, BlobRectList>(axisImage, innerDefectRectList));

                        _inspectProcess.BufferDictionary[info].Item4 = innerBlobQueue.Count;
                    }
                }

                _endResetEventDictionary[info].Item4.Set();

                runResetEvent.WaitOne();

                while (!innerBlobQueue.IsEmpty)
                    innerBlobQueue.TryDequeue(out Tuple<AxisImage, byte[]> temp);

                innerBlobResetEvent.Reset();

            }, TaskCreationOptions.LongRunning);
        }

        static async Task BlobMergeAsync(AxisGrabInfo info)
        {
            double maxDist = SectionService.Settings.Selected.DefectDistanceList.Max();

            await Task.Run(() =>
            {
                var settings = AxisGrabService.Settings;

                MilImageProcessing imageProcessing = new MilImageProcessing();

                var mergeBlobEdgeQueue = _mergeBlobDictionary[info][InspectSequence.Edge];
                var mergeBlobInnerQueue = _mergeBlobDictionary[info][InspectSequence.Inner];

                BlobParam mergeBlobParam = new BlobParam();
                mergeBlobParam.SelectCenterPt = true;
                mergeBlobParam.SelectMinValue = true;
                mergeBlobParam.SelectMeanValue = true;
                mergeBlobParam.SelectMaxValue = true;
                mergeBlobParam.SelectRotateRect = true;

                Tuple<AxisImage, BlobRectList>[] results = null;

                switch (_inspectSequence)
                {
                    case InspectSequence.Edge:
                        results = mergeBlobEdgeQueue.OrderBy(result => result.Item1.Y).ToArray();
                        while (!mergeBlobEdgeQueue.IsEmpty)
                            mergeBlobEdgeQueue.TryDequeue(out Tuple<AxisImage, BlobRectList> temp);
                        
                        break;
                    case InspectSequence.Section:
                    case InspectSequence.Inner:
                        results = mergeBlobInnerQueue.OrderBy(result => result.Item1.Y).ToArray();
                        while (!mergeBlobInnerQueue.IsEmpty)
                            mergeBlobInnerQueue.TryDequeue(out Tuple<AxisImage, BlobRectList> temp);

                        mergeBlobParam.RotateWidthMin = Model_COSMO.Param.InnerMinLengthUM / settings.Resolution;
                        break;
                }

                BlobRectList prevBlobRectList = null;
                foreach (var result in results)
                {
                    if (prevBlobRectList == null)
                    {
                        prevBlobRectList = result.Item2;
                        continue;
                    }

                    var temp = imageProcessing.BlobMerge(prevBlobRectList, result.Item2, mergeBlobParam);
                    prevBlobRectList.Dispose();
                    result.Item2.Dispose();

                    prevBlobRectList = temp;
                }

                if (prevBlobRectList == null)
                    return;

                prevBlobRectList.Dispose();

                int width = info.ImageWidth;
                int height = results.Count() * info.ImageHeight;

                AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(width, height));

                int index = 0;
                foreach (var result in results)
                {
                    AlgoImage subImage = algoImage.GetSubImage(new Rectangle(0, index * info.ImageHeight, info.ImageWidth, info.ImageHeight));
                    subImage.PutByte(result.Item1.Data);
                    index++;
                    subImage.Dispose();
                }

                List<Defect> defectList = new List<Defect>();

                var xPos = results.Average(result => result.Item1.X);
            
                foreach (BlobRect blobRect in prevBlobRectList)
                {
                    switch (_inspectSequence)
                    {
                        case InspectSequence.Edge:
                            switch (info.ScanDirection)
                            {
                                case ScanDirection.LeftToRight:
                                    if (blobRect.MeanValue < Model_COSMO.CalibrationValue + (_model.GetWeightValue(info) * Model_COSMO.Param.LeftEdgeBinarizeValue))
                                        continue;
                                    break;
                                case ScanDirection.RightToLeft:
                                    if (blobRect.MeanValue < Model_COSMO.CalibrationValue + (_model.GetWeightValue(info) * Model_COSMO.Param.RightEdgeBinarizeValue))
                                        continue;
                                    break;
                            }
                            break;
                        case InspectSequence.Section:
                            break;
                        case InspectSequence.Inner:
                            break;
                    }
                    if (blobRect.RotateWidth == settings.Resolution)
                        continue;

                    int yIndex = (int)(blobRect.CenterPt.Y / info.ImageHeight);

                    //var yPos = results[yIndex].Item1.Y + (blobRect.CenterPt.Y % info.ImageHeight);

                    double[] xArray = Array.ConvertAll(blobRect.RotateXArray, x => (double)x);
                    double[] yArray = Array.ConvertAll(blobRect.RotateYArray, y => (double)y);

                    float centerX = (float)(blobRect.CenterPt.X * settings.Resolution + xPos);
                    float centerY = (float)(results[yIndex].Item1.Y + (blobRect.CenterPt.Y % info.ImageHeight) * settings.Resolution);

                    if (info.TopLineModel.GetY(centerX) > centerY)
                        continue;

                    if (info.BottomLineModel.GetY(centerX) < centerY)
                        continue;

                    switch (_inspectSequence)
                    {
                        case InspectSequence.Edge:
                            defectList.Add(new EdgeDefect(++defectIndex, info.ScanDirection, DrawDefect(algoImage, blobRect), 
                                            xArray, yArray, (uint)blobRect.Area, new PointF(centerX, centerY),
                                            (byte)blobRect.MinValue, (byte)blobRect.MaxValue, blobRect.MeanValue,
                                            blobRect.RotateWidth * settings.Resolution, blobRect.RotateHeight * settings.Resolution,
                                            blobRect.RotateAngle));
                            break;
                        case InspectSequence.Section:
                            if (info.LeftLineModel.GetX(centerY) > centerX)
                                continue;

                            if (info.RightLineModel.GetX(centerY) < centerX)
                                continue;

                            double dist = info.LineModel.GetDistance(new PointF((float)centerX, (float)centerY)) / 1000.0;
                            if (dist > maxDist)
                                continue;

                            defectList.Add(new SectionDefect(++defectIndex, info.ScanDirection, DrawDefect(algoImage, blobRect),
                                            xArray, yArray, (uint)blobRect.Area, new PointF(centerX, centerY),
                                            (byte)blobRect.MinValue, (byte)blobRect.MaxValue, blobRect.MeanValue,
                                            blobRect.RotateWidth * settings.Resolution, blobRect.RotateHeight * settings.Resolution,
                                            blobRect.RotateAngle, dist));
                            break;
                        case InspectSequence.Inner:

                            if (info.LeftLineModel.GetX(centerY) > centerX)
                                continue;

                            if (info.RightLineModel.GetX(centerY) < centerX)
                                continue;

                            defectList.Add(new InnerDefect(++defectIndex, DrawDefect(algoImage, blobRect), 
                                            xArray, yArray, (uint)blobRect.Area, new PointF(centerX, centerY),
                                            (byte)blobRect.MinValue, (byte)blobRect.MaxValue, blobRect.MeanValue,
                                            blobRect.RotateWidth * settings.Resolution, blobRect.RotateHeight * settings.Resolution,
                                            blobRect.RotateAngle, info.LineModel.GetDistance(new PointF((float)centerX, (float)centerY)) / 1000.0));
                            break;
                    }
                }

                algoImage.Dispose();

                ScanResult scanResult = new ScanResult(info.ImageWidth, info.ImageHeight, settings.ImageResizeRatio,
                    new System.Windows.Point((int)results.Min(result => result.Item1.X), (int)results.Min(result => result.Item1.Y)), results.Count(), defectList);

                var defectImage = scanResult.GetDefectImage();

                if (DrawDefectsDone != null)
                    DrawDefectsDone(new AxisImageSource(scanResult.AxisPosition, defectImage));

                lock (_inspectResult.ScanResultList)
                    _inspectResult.ScanResultList.Add(scanResult);

                if (Inspected != null)
                    Inspected(scanResult);
            });
        }

        static BitmapSource DrawDefect(AlgoImage algoImage, BlobRect blobRect)
        {
            Rectangle sourceRegion = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            var infalteRegion = new Rectangle(
                        (int)blobRect.BoundingRect.X, (int)blobRect.BoundingRect.Y,
                        (int)blobRect.BoundingRect.Width, (int)blobRect.BoundingRect.Height);

            infalteRegion.Inflate(50, 50);
            infalteRegion.Intersect(sourceRegion);

            var defectImage = algoImage.GetSubImage(infalteRegion);

            BitmapSource defectBitmap = BitmapSource.Create(infalteRegion.Width, infalteRegion.Height,
                                       96, 96, System.Windows.Media.PixelFormats.Gray8, null,
                                       defectImage.GetByte(), infalteRegion.Width);
            defectBitmap.Freeze();
            defectImage.Dispose();

            //double[] xArray = Array.ConvertAll(blobRect.RotateXArray, x => (double)x);
            //double[] yArray = Array.ConvertAll(blobRect.RotateYArray, y => (double)y);

            //var visual = new DrawingVisual();
            //using (DrawingContext drawingContext = visual.RenderOpen())
            //{
            //    SolidColorBrush brush = new SolidColorBrush(_inspectMode == InspectMode.Edge ? Colors.Blue : Colors.Red);

            //    StreamGeometry streamGeometry = new StreamGeometry();

            //    System.Windows.Point[] points = new System.Windows.Point[4];
            //    for (int i = 0; i < 4; i++)
            //        points[i] = new System.Windows.Point(xArray[i] - infalteRegion.X, yArray[i] - infalteRegion.Y);

            //    using (StreamGeometryContext geometryContext = streamGeometry.Open())
            //    {
            //        geometryContext.BeginFigure(points[0], true, true);
            //        geometryContext.PolyLineTo(points, true, true);
            //    }

            //    brush.Opacity = 0.25;

            //    drawingContext.DrawImage(defectBitmap, new System.Windows.Rect(0, 0, defectBitmap.Width, defectBitmap.Height));
            //    drawingContext.DrawGeometry(brush, new System.Windows.Media.Pen(brush, 1), streamGeometry);
            //}

            //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(infalteRegion.Width, infalteRegion.Height, 96, 96, PixelFormats.Pbgra32);
            //renderTargetBitmap.Render(visual);

            //renderTargetBitmap.Freeze();

            return defectBitmap;
        }

        

        static void FloodFill(ref byte[] data, int index, int width, int height, byte target, byte replace)
        {
            if (target == replace)
                return;

            if (data[index] != target)
                return;

            data[index] = replace;

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(index);

            int size = width * height;

            while (queue.Count != 0)
            {
                int n = queue.Dequeue();

                int min = n - (n % width);
                int max = min + width - 1;

                int north = n - width;
                int south = n + width;
                int west = n - 1;
                int east = n + 1;

                if (west >= min && data[west] == target)
                {
                    data[west] = replace;
                    queue.Enqueue(west);
                }

                if (east <= max && data[east] == target)
                {
                    data[east] = replace;
                    queue.Enqueue(east);
                }

                if (north >= 0 && data[north] == target)
                {
                    data[north] = replace;
                    queue.Enqueue(north);
                }

                if (south < size && data[south] == target)
                {
                    data[south] = replace;
                    queue.Enqueue(south);
                }
            }
        }
    }
}

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.ViewModels;
using Direction = DynMvp.Vision.Direction;

namespace WPF.COSMO.Offline.Services
{
    public class AlignProcess : Observable
    {
        public Dictionary<AxisGrabInfo, Tuple<ProcessUnit, ProcessUnit, ProcessUnit>> ProcessUnitDictionary { get; set; } = new Dictionary<AxisGrabInfo, Tuple<ProcessUnit, ProcessUnit, ProcessUnit>>(); 

        public bool IsError => Error != null;

        string _error;
        public string Error
        {
            get => _error;
            set
            {
                Set(ref _error, value);
                OnPropertyChanged("IsError");
            }
        }

        double _accuracy;
        public double Accuracy
        {
            get => _accuracy;
            set
            {
                Set(ref _accuracy, value);
            }
        }

        double _degree;
        public double Degree
        {
            get => _degree;
            set
            {
                Set(ref _degree, value);
            }
        }

        public AlignProcess()
        {
            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
                ProcessUnitDictionary.Add(info, new Tuple<ProcessUnit, ProcessUnit, ProcessUnit>(new ProcessUnit(), new ProcessUnit(), new ProcessUnit()));
        }
    }

    public class EstimatedLine
    {
        public String Name { get; set; }
        public System.Drawing.PointF StartPt { get; set; }
        public System.Drawing.PointF EndPt { get; set; }

        public EstimatedLine(string name, float startX, float endX, float startY, float endY)
        {
            Name = name;
            StartPt = new System.Drawing.PointF(startX, startY);
            EndPt = new System.Drawing.PointF(endX, endY);
        }
    }

    public delegate void EstimatedDelegate(IEnumerable<EstimatedLine> lines);

    public static class AlignService
    {
        public static EstimatedDelegate Estimated;

        static CancellationToken _token;
        
        static Dictionary<AxisGrabInfo, int> _xPositionDictionary = new Dictionary<AxisGrabInfo, int>();

        static Dictionary<AxisGrabInfo, List<AxisImage>> _alignDataDictionary = new Dictionary<AxisGrabInfo, List<AxisImage>>();
        static Dictionary<AxisGrabInfo, ManualResetEvent> _resetEventDictionary = new Dictionary<AxisGrabInfo, ManualResetEvent>();
        static Dictionary<AxisGrabInfo, LineModel> _lineDictionary = new Dictionary<AxisGrabInfo, LineModel>();
        static Dictionary<AxisGrabInfo, Tuple<CudaImage, CudaImage, MilImage>> _bufferDictionary = new Dictionary<AxisGrabInfo, Tuple<CudaImage, CudaImage, MilImage>>();
        
        static AlignProcess _alignProcess;

        static List<EstimatedLine> _lines = new List<EstimatedLine>();
        public static IEnumerable<EstimatedLine> Lines => _lines;

        public static Dictionary<AxisGrabInfo, double> ThresholdDictionary { get; set; } = new Dictionary<AxisGrabInfo, double>();

        public static void Initialize(AlignProcess alignProcess, CancellationToken token)
        {
            Release();
            
            _alignProcess = alignProcess;

            _token = token;
            
            AxisGrabService.AxisImageGrabbed += AxisImageGrabbed_X;

            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
            {
                _xPositionDictionary.Add(info, 0);

                _alignDataDictionary.Add(info, new List<AxisImage>());
                _resetEventDictionary.Add(info, new ManualResetEvent(false));
                _lineDictionary.Add(info, null);
                ThresholdDictionary.Add(info, 0);
                _bufferDictionary.Add(info, new Tuple<CudaImage, CudaImage, MilImage>(new CudaDepthImage<byte>(), new CudaDepthImage<byte>(), new MilGreyImage()));
            }
        }
        
        public static void Release()
        {
            _lines.Clear();

            _xPositionDictionary.Clear();

            _lineDictionary.Clear();
            _alignDataDictionary.Clear();
            _resetEventDictionary.Clear();
            ThresholdDictionary.Clear();

            foreach (var buffers in _bufferDictionary.Values)
            {
                buffers.Item1.Dispose();
                buffers.Item2.Dispose();
                buffers.Item3.Dispose();
            }

            _bufferDictionary.Clear();

            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed_Gradient;
            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed_X;
        }
        
        public static async Task<bool> AlignAsync()
        {
            var settings = AxisGrabService.Settings;

            var scanLength = settings.AxisGrabInfoList.Max(info => info.ImageDevice.ImageSize.Height * settings.Resolution) * 1.99;

            foreach (var info in settings.AxisGrabInfoList)
                info.NextX = info.ScanDirection == ScanDirection.LeftToRight ? info.MinX : info.MaxX;

            foreach (var unit in _alignProcess.ProcessUnitDictionary.Values)
                unit.Item1.Processing = true;

            while (_resetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0) == false) && _token.IsCancellationRequested == false)
            {
                AxisGrabService.NextY = settings.CenterY;

                if (await AxisGrabService.MoveNextPos() == false)
                {
                    foreach (var pair in _resetEventDictionary)
                    {
                        if (pair.Value.WaitOne(0) == false)
                            _alignProcess.ProcessUnitDictionary[pair.Key].Item1.Fail = true;
                    }

                    _alignProcess.Error = TranslationHelper.Instance.Translate("Align_Find_Error");

                    return false;
                }

                AxisGrabService.NextY = settings.CenterY + (int)scanLength;

                if (await AxisGrabService.GrabNextPos() == false)
                {
                    foreach (var pair in _resetEventDictionary)
                    {
                        if (pair.Value.WaitOne(0) == false)
                            _alignProcess.ProcessUnitDictionary[pair.Key].Item1.Fail = true;
                    }

                    _alignProcess.Error = TranslationHelper.Instance.Translate("Align_Find_Error");

                    return false;
                }
                
                foreach (var pair in _resetEventDictionary)
                {
                    var info = pair.Key;
                    var resetEvent = pair.Value;

                    var scanWidth = info.ImageDevice.ImageSize.Width * settings.Resolution;

                    var nextLength = info.ScanDirection == ScanDirection.LeftToRight ? scanWidth / 2 : -scanWidth / 2;
                    
                    if (resetEvent.WaitOne(0) == false)
                    {
                        if (info.NextX + nextLength <= info.MaxX || info.NextX + nextLength >= info.MinX)
                            info.NextX += (int)nextLength;
                    }
                    else
                    {
                        _alignProcess.ProcessUnitDictionary[pair.Key].Item1.Success = true;
                    }
                }
            }
        
            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed_X;

            if (_resetEventDictionary.Values.Any(resetEvent => resetEvent.WaitOne(0) == false))
            {
                foreach (var pair in _resetEventDictionary)
                {
                    if (pair.Value.WaitOne(0) == false)
                        _alignProcess.ProcessUnitDictionary[pair.Key].Item1.Fail = true;
                }

                _alignProcess.Error = TranslationHelper.Instance.Translate("Align_Find_Error");

                return false;
            }

            if (_token.IsCancellationRequested)
                return false;

            foreach (var unit in _alignProcess.ProcessUnitDictionary.Values)
                unit.Item2.Processing = true;

            foreach (var info in settings.AxisGrabInfoList)
                info.NextX = _xPositionDictionary[info];

            AxisGrabService.NextY = settings.CenterY - (settings.AlignScanLength / 2);

            if (await AxisGrabService.MoveNextPos() == false)
                return false;

            AxisGrabService.NextY = settings.CenterY + (settings.AlignScanLength / 2);
            AxisGrabService.AxisImageGrabbed += AxisImageGrabbed_Gradient;

            if (await AxisGrabService.GrabNextPos() == false)
            {
                foreach (var unit in _alignProcess.ProcessUnitDictionary.Values)
                    unit.Item2.Fail = true;

                return false;
            }
            
            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed_Gradient;

            foreach (var unit in _alignProcess.ProcessUnitDictionary.Values)
                unit.Item2.Success = true;

            foreach (var info in settings.AxisGrabInfoList)
            {
                var dataList = _alignDataDictionary[info];

                int width = info.ImageWidth;
                int height = dataList.Count * info.ImageHeight;

                _bufferDictionary[info].Item1.Alloc(width, height);
                _bufferDictionary[info].Item2.Alloc(width, height);
                _bufferDictionary[info].Item3.Alloc(width, height);
            }

            foreach (var info in settings.AxisGrabInfoList)
            {
                _alignProcess.ProcessUnitDictionary[info].Item3.Processing = true;
                
                if (await GetAlignModel(info) == false)
                {
                    _alignProcess.ProcessUnitDictionary[info].Item3.Fail = true;
                    _alignProcess.Error = TranslationHelper.Instance.Translate("Aline_Line_Error");
                    return false;
                }
            }
            

            if (_token.IsCancellationRequested)
                return false;

            var rightInfo = settings.AxisGrabInfoList.OrderByDescending(info => info.OffsetX).First();
            //var maxAccuracy = _lineDictionary.Values.Max(line => line.Accuracy);
            
            //double bestGradient = _lineDictionary[rightInfo].Gradient;//_lineDictionary.Values.First(line => line.Accuracy == maxAccuracy).Gradient;
            //double bestGradient = _lineDictionary.Values.Average(line => line.Gradient);//_lineDictionary.Values.First(line => line.Accuracy == maxAccuracy).Gradient;

            foreach (var info in settings.AxisGrabInfoList)
            {
                var scanWidth = info.ImageDevice.ImageSize.Width * settings.Resolution;
                double axisCenterX = _xPositionDictionary[info] + _lineDictionary[info].CenterX ;
                double axisCenterY = settings.CenterY - (settings.AlignScanLength / 2) + _lineDictionary[info].CenterY;

                LineModel lineModel = new LineModel(_lineDictionary[info].Accuracy, _lineDictionary[info].Gradient, axisCenterX + info.OffsetX, axisCenterY + info.LineOffset);
                
                _alignProcess.ProcessUnitDictionary[info].Item3.Success = true;
                info.LineModel = lineModel;
            }

            List<PointF> intersectPointList = new List<PointF>();

            var intersectRadian = -1.0 / _lineDictionary.Values.Average(line => line.Gradient);
            if (intersectRadian == 0)
                intersectRadian = double.MaxValue;
            
            foreach (var info in settings.AxisGrabInfoList)
            {
                var intersectLine = new LineModel(intersectRadian, info.LineModel.CenterX, info.LineModel.CenterY);
                if (AxisGrabService.IsSingleMode == false)
                {
                    var another = settings.AxisGrabInfoList.First(i => i != info);
                    intersectPointList.Add(another.LineModel.GetIntersectPoint(intersectLine));
                }
                else
                {
                    var dist = SectionService.Settings.Selected.DefectDistanceList.Max();
                    var parallelLine = info.LineModel.GetParallelLine(-dist * 1000);
                    intersectPointList.Add(parallelLine.GetIntersectPoint(intersectLine));
                }
            }

            PointF centerPt = new PointF(intersectPointList.Average(point => point.X), intersectPointList.Average(point => point.Y));

            LineModel centerLine = new LineModel(intersectRadian, centerPt.X, centerPt.Y);


            double maxDist = SectionService.Settings.Selected.DefectDistanceList.Max();
            
            //var rightInfo = settings.AxisGrabInfoList.OrderByDescending(info => info.OffsetX).First();

            foreach (var info in settings.AxisGrabInfoList)
            {
                Point intersectPt = Point.Round(info.LineModel.GetIntersectPoint(centerLine));
                info.LineModel = new LineModel(info.LineModel.Accuracy, info.LineModel.Gradient, intersectPt.X, intersectPt.Y);
                
                var length = SectionService.Settings.Selected.InspectScanLength * 1000;

                PointF startPt = info.LineModel.GetDistPt(-length / 2);
                PointF endPt = info.LineModel.GetDistPt(length / 2);

                _lines.Add(new EstimatedLine("Edge", startPt.X, endPt.X, startPt.Y, endPt.Y));

                foreach (var dist in SectionService.Settings.Selected.DefectDistanceList)
                {
                    var parallelLine = info.LineModel.GetParallelLine(info.ScanDirection == ScanDirection.LeftToRight ? dist * 1000 : -dist * 1000);

                    PointF sPt = parallelLine.GetDistPt(-length / 2);
                    PointF ePt = parallelLine.GetDistPt(length / 2);

                    _lines.Add(new EstimatedLine(dist.ToString(), sPt.X, ePt.X, sPt.Y, ePt.Y));
                }

                switch (info.ScanDirection)
                {
                    case ScanDirection.LeftToRight:
                        info.LeftLineModel = new LineModel(info.LineModel.Gradient, info.LineModel.CenterX, info.LineModel.CenterY);
                        info.RightLineModel = info.LineModel.GetParallelLine(maxDist * 1000);
                        break;
                    case ScanDirection.RightToLeft:
                        info.RightLineModel = new LineModel(info.LineModel.Gradient, info.LineModel.CenterX, info.LineModel.CenterY);
                        info.LeftLineModel = info.LineModel.GetParallelLine(-maxDist * 1000);
                        break;
                }

                var intersectGradient = -1.0 / info.LineModel.Gradient;
                info.TopLineModel = new LineModel(-1.0 / info.LineModel.Gradient, startPt.X, startPt.Y);
                info.BottomLineModel = new LineModel(-1.0 / info.LineModel.Gradient, endPt.X, endPt.Y);

                LineModel anotherLineModel = null;// 
                if (AxisGrabService.IsSingleMode == false)
                {
                    anotherLineModel = settings.AxisGrabInfoList.First(another => another != info).LineModel;
                }
                else
                {
                    anotherLineModel = info.LineModel.GetParallelLine(-maxDist * 1000);
                }
                
                var interPt1 = anotherLineModel.GetIntersectPoint(info.TopLineModel);
                var interPt2 = anotherLineModel.GetIntersectPoint(info.BottomLineModel);

                _lines.Add(new EstimatedLine(string.Empty, startPt.X, interPt1.X, startPt.Y, interPt1.Y));
                _lines.Add(new EstimatedLine(string.Empty, endPt.X, interPt2.X, endPt.Y, interPt2.Y));

                if (info == rightInfo)
                {
                    foreach (var position in SectionService.Settings.Selected.InspectPositionList)
                    {
                        var leftLine = rightInfo.LineModel.GetParallelLine((-position + (maxDist / 2)) * 1000);
                        var rightLine = rightInfo.LineModel.GetParallelLine((-position - (maxDist / 2)) * 1000);
                        
                        PointF leftStartPt = leftLine.GetDistPt(-SectionService.Settings.Selected.InspectScanLength * 1000 / 2);
                        PointF leftEndPt = leftLine.GetDistPt(SectionService.Settings.Selected.InspectScanLength * 1000 / 2);

                        PointF rightStartPt = rightLine.GetDistPt(-SectionService.Settings.Selected.InspectScanLength * 1000 / 2);
                        PointF rightEndPt = rightLine.GetDistPt(SectionService.Settings.Selected.InspectScanLength * 1000 / 2);

                        _lines.Add(new EstimatedLine(string.Empty, leftStartPt.X, leftEndPt.X, leftStartPt.Y, leftEndPt.Y));
                        _lines.Add(new EstimatedLine(string.Empty, rightStartPt.X, rightEndPt.X, rightStartPt.Y, rightEndPt.Y));
                    }
                }
            }

            if (Estimated != null)
                Estimated(Lines);

            foreach (var info in settings.AxisGrabInfoList)
            {
                double angle = Math.Atan(_lineDictionary[info].Gradient) * (180.0 / Math.PI);
                if (Math.Abs(angle - 90.0) > settings.AlignDegreeThreshold &&
                    Math.Abs(angle + 90.0) > settings.AlignDegreeThreshold)
                {
                    _alignProcess.ProcessUnitDictionary[info].Item3.Fail = true;
                    _alignProcess.Error = string.Format(TranslationHelper.Instance.Translate("Align_Degree_Error"), angle, 90.0 - settings.AlignDegreeThreshold, 90.0 + settings.AlignDegreeThreshold);
                    return false;
                }
            }

            if (_token.IsCancellationRequested)
                return false;

            return true;
        }

        private static void AxisImageGrabbed_X(object sender, AxisImage e)
        {
            var info = sender as AxisGrabInfo;
            
            var ratio = e.Data.Count(d => d > Model_COSMO.CalibrationValue - 2) / (double)e.Data.Length;
            string str = string.Format("ratio : {0}, (double)e.Data.Length : {1} - LHJ", ratio, (double)e.Data.Length);
            
            if (ratio > AxisGrabService.Settings.AlignRatioX)
            {
                int pos = FindEdge(Direction.Horizontal, e.Data, (int)info.ImageWidth, (int)info.ImageHeight);
                
                _xPositionDictionary[info] = info.NextX - (int)((info.ImageWidth / 2 - pos) * AxisGrabService.Settings.Resolution);
                
                _resetEventDictionary[info].Set();
            }
        }

        private static void AxisImageGrabbed_Gradient(object sender, AxisImage axisImage)
        {
            var info = sender as AxisGrabInfo;
            
            lock (_alignDataDictionary[info])
            {
                _alignDataDictionary[info].Add(axisImage);
            }
        }

        private static Task<bool> GetAlignModel(AxisGrabInfo info)
        {
            return Task.Run(() =>
            {
                try
                {
                    var imageList = _alignDataDictionary[info].OrderBy(axisImage => axisImage.Y).ToList();

                    byte[] data = new byte[imageList.Count * info.ImageWidth * info.ImageHeight];

                    int size = info.ImageWidth * info.ImageHeight;

                    for (int i = 0, index = 0; i < imageList.Count; i++, index += size)
                        Array.Copy(imageList[i].Data, 0, data, index, size);

                    int width = info.ImageWidth;
                    int height = data.Length / width;

                    var srcImage = _bufferDictionary[info].Item1;
                    var destImage = _bufferDictionary[info].Item2;

                    srcImage.SetByte(data);
                    ThresholdDictionary[info] = GetThValue(data); 

                    if (CudaMethods.CUDA_BINARIZE_UPPER(srcImage.ImageID, srcImage.ImageID, (int)Math.Round(ThresholdDictionary[info])) == false)
                        return false;
                    
                    var milImage = _bufferDictionary[info].Item3;
                    milImage.SetByte(srcImage.CloneByte());

                    MilImageProcessing imageProcessing = new MilImageProcessing();
                    imageProcessing.FillHoles(milImage, milImage);
                    srcImage.SetByte(milImage.CloneByte());
                    //info.EdgeThValue = (int)Math.Round(GetThValue(data, value));

                    if (CudaMethods.CUDA_MORPHOLOGY_ERODE(srcImage.ImageID, destImage.ImageID, 1) == false)
                        return false;

                    if (CudaMethods.CUDA_MATH_XOR(srcImage.ImageID, destImage.ImageID, destImage.ImageID) == false)
                        return false;

                    var lineData = destImage.CloneByte();

                    srcImage.Dispose();
                    destImage.Dispose();

                    var indexList = FindLine(info.ScanDirection, width, height, lineData);

                    if (indexList == null)
                        return false;

                    double[] xIndexs = new double[indexList.Count];
                    double[] yIndexs = new double[indexList.Count];

                    var resolution = AxisGrabService.Settings.Resolution;

                    for (int i = 0; i < indexList.Count; i++)
                    {
                        xIndexs[i] = indexList[i] % width * resolution;
                        yIndexs[i] = indexList[i] / width * resolution;
                    }

                    double bestCost = 0;
                    double bestGradient = 0;
                    double bestXCenter = 0;
                    double bestYCenter = 0;

                    for (int i = 0; i < AxisGrabService.Settings.LineEstimateNum; i++)
                    {
                        double cost = 0;
                        double gradient = 0;
                        double xCenter = 0;
                        double yCenter = 0;

                        CudaMethods.CUDA_RANSAC(width, height, xIndexs, yIndexs, indexList.Count, ref cost, ref gradient, ref xCenter, ref yCenter, AxisGrabService.Settings.AlignDistTheshold);

                        if (cost > bestCost)
                        {
                            bestCost = cost;
                            bestGradient = gradient;
                            bestXCenter = xCenter;
                            bestYCenter = yCenter;
                        }
                    }

                    if (bestGradient > 10000)
                        bestGradient = 10000;

                    if (bestGradient < -10000)
                        bestGradient = -10000;

                    _lineDictionary[info] = new LineModel(bestCost / indexList.Count, bestGradient, bestXCenter, bestYCenter);

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        private static List<int> FindLine(ScanDirection scanDirection, int width, int height, byte[] data)
        {
            const double lineRatio = 0.75;

            List<int> startIndexs = new List<int>();
            List<int> endIndexs = new List<int>();

            switch (scanDirection)
            {
                case ScanDirection.LeftToRight:
                    for (int i = 0; i < width; i++)
                    {
                        if (data[i] > 0)
                        {
                            if (i == 0)
                                startIndexs.Add(i);
                            else if (data[i - 1] == 0)
                                startIndexs.Add(i);
                        }
                    }
                    break;
                case ScanDirection.RightToLeft:
                    for (int i = width - 1; i >= 0; i--)
                    {
                        if (data[i] > 0)
                        {
                            if (i == width - 1)
                                startIndexs.Add(i);
                            else if (data[i + 1] == 0)
                                startIndexs.Add(i);
                        }
                    }
                    break;
            }

            switch (scanDirection)
            {
                case ScanDirection.LeftToRight:
                    for (int i = (height - 1) * width; i < height * width - 1; i++)
                    {
                        if (data[i] > 0)
                        {
                            if (i == 0)
                                endIndexs.Add(i);
                            else if (data[i - 1] == 0)
                                endIndexs.Add(i);
                        }
                    }
                    break;
                case ScanDirection.RightToLeft:
                    for (int i = height * width - 1; i >= (height - 1) * width; i--)
                    {
                        if (data[i] > 0)
                        {
                            if (i == height * width - 1)
                                endIndexs.Add(i);
                            else if (data[i + 1] == 0)
                                endIndexs.Add(i);
                        }
                    }
                    break;
            }

            int topHalfIndex = width / 2;
            int bottomHalfIndex = ((height - 1) * width) + (width / 2);

            startIndexs = startIndexs.OrderBy(index => Math.Abs(index - topHalfIndex)).ToList();
            endIndexs = endIndexs.OrderBy(index => Math.Abs(index - bottomHalfIndex)).ToList();

            List<Tuple<int, List<int>>> indexsList = new List<Tuple<int, List<int>>>();

            foreach (int startIndex in startIndexs)
            //Parallel.ForEach(startIndexs, startIndex =>
            {
                Stack<int> indexs = new Stack<int>();
                Stack<int> stack = new Stack<int>();
                stack.Push(startIndex);

                while (stack.Count != 0)
                {
                    if (_token.IsCancellationRequested)
                        break;

                    int index = stack.Pop();

                    indexs.Push(index);

                    int x = index % width;
                    int y = index / width;

                    if (y > height * lineRatio)
                        break;

                    int indexer = index - width - 1;
                    if (x > 0 && y > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (y > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (x < width - 1 && y > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer = index - 1;

                    if (x > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer += 2;

                    if (x < width - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer = index + width - 1;

                    if (x > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (x < width - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }
                }

                if (indexs.Count > 0)
                {
                    int maxY = indexs.Max(index => index / width);
                    if (maxY > height * lineRatio)
                        return indexs.ToList();

                    lock (indexsList)
                        indexsList.Add(new Tuple<int, List<int>>(maxY, indexs.ToList()));
                }
            }//);

            if (indexsList.Count != 0)
            {
                int maxY = indexsList.Max(tuple => tuple.Item1);
                if (maxY > height * lineRatio)
                    return indexsList.Find(tuple => tuple.Item1 == maxY).Item2;
            }

            foreach (var endIndex in endIndexs)
            //Parallel.ForEach(endIndexs, endIndex =>
            {
                Stack<int> indexs = new Stack<int>();
                Stack<int> stack = new Stack<int>();
                stack.Push(endIndex);

                while (stack.Count != 0)
                {
                    if (_token.IsCancellationRequested)
                        break;

                    int index = stack.Pop();

                    indexs.Push(index);

                    int x = index % width;
                    int y = index / width;
                    
                    if (y < height * 0.25)
                        break;

                    int indexer = index - width - 1;
                    if (x > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (x < width - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer = index - 1;

                    if (x > 0 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer += 2;

                    if (x < width - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer = index + width - 1;

                    if (x > 0 && y < height - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (y < height - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }

                    indexer++;

                    if (x < width - 1 && y < height - 1 && data[indexer] > 0)
                    {
                        if (!indexs.Contains(indexer))
                            stack.Push(indexer);
                    }
                }

                if (indexs.Count > 0)
                {
                    int minY = indexs.Max(index => index / width);

                    lock (indexsList)
                        indexsList.Add(new Tuple<int, List<int>>(height - minY - 1, indexs.ToList()));
                }
            }

            if (indexsList.Count == 0)
                return null;

            var max = indexsList.Max(tuple => tuple.Item1);
            var maxList = indexsList.Find(tuple => tuple.Item1 == max).Item2;

            return maxList;
        }

        public static int FindEdge(Direction direction, byte[] data, int width, int height)
        {
            var profile = new double[width];
            int major = direction == Direction.Horizontal ? width : height;
            int minor = direction == Direction.Horizontal ? height : width;

            for (int minorIndex = 0; minorIndex < minor; minorIndex++)
            {
                for (int majorIndex = 0, index = minorIndex * major; majorIndex < major; majorIndex++)
                    profile[majorIndex] += data[index++];
            }

            for (int majorIndex = 0; majorIndex < major; majorIndex++)
                profile[majorIndex] /= minor;

            int inflate = 1000;
            int maxIndex = 0;
            double maxSum = 0;

            double[] temp = new double[inflate];
            for (int majorIndex = 0; majorIndex < major - inflate; majorIndex++)
            {
                Array.Copy(profile, majorIndex, temp, 0, inflate);
                var avg = temp.Average();
                double sum = 0;

                foreach (var val in temp)
                    sum += Math.Abs(val - avg);

                if (sum > maxSum)
                {
                    maxIndex = majorIndex;
                    maxSum = sum;
                }
            }

            return maxIndex + inflate / 2;
        }

        static double GetThValue(byte[] data)
        {
            double tolerance = 0.1f;

            long count = data.Length;

            double mean = 0;
            double sum = 0;

            long[] histogram = new long[256];
            for (int i = 0; i < count; i++)
                histogram[data[i]]++;

            for (int ih = 0 + 1; ih < 256; ih++)
                sum += ih * histogram[ih];

            mean = sum / count;

            double sumBack;
            double sumObj; 
            double numBack;
            double numObj; 

            double meanBack;
            double meanObj;

            double newThreshold = mean;
            double oldThreshold = 0;
            double threshold = 0;

            do
            {
                oldThreshold = newThreshold;
                threshold = oldThreshold + 0.5f;
                
                numBack = 0;
                for (int ih = 0; ih <= (int)threshold; ih++)
                    numBack += histogram[ih];

                if (numBack == 0)
                {
                    newThreshold += tolerance * 2;
                    continue;
                }

                sumBack = 0;
                for (int ih = 0; ih <= (int)threshold; ih++)
                {
                    sumBack += ih * histogram[ih];
                }

                meanBack = (numBack == 0 ? 0 : (sumBack / numBack));

                sumObj = 0;
                numObj = 0;
                for (int ih = (int)threshold + 1; ih < 256; ih++)
                {
                    sumObj += ih * histogram[ih];
                    numObj += histogram[ih];
                }

                meanObj = (numObj == 0 ? 0 : (sumObj / numObj));
                
                double logDiff = Math.Log(meanBack) - Math.Log(meanObj);
                newThreshold = (meanBack - meanObj) / logDiff;
            }
            while (Math.Abs(newThreshold - oldThreshold) > tolerance);

            return threshold;
        }

        static double GetThValue(byte[] data, double thValue)
        {
            int count = 0;
            long[] histogram = new long[256];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= thValue)
                {
                    histogram[data[i]]++;
                    count++;
                }
            }
                

            double sum = 0;
            for (int i = 1; i < 256; ++i)
                sum += i * histogram[i];
                
            double sumB = 0;
            double wB = 0;
            double wF = 0;
            double mB;
            double mF;
            double max = 0.0;
            double between = 0.0;
            double threshold1 = 0.0;
            double threshold2 = 0.0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0)
                    continue;
                wF = count - wB;
                if (wF == 0)
                    break;
                sumB += i * histogram[i];
                mB = sumB / wB;
                mF = (sum - sumB) / wF;
                between = wB * wF * (mB - mF) * (mB - mF);
                if (between >= max)
                {
                    threshold1 = i;
                    if (between > max)
                        threshold2 = i;

                    max = between;
                }
            }

            return (float)((threshold1 + threshold2) / 2.0F);
        }
    }
}

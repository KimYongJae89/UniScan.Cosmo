using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Inspect;
using UniScanG.Vision;

namespace UniScanG.Gravure.Inspect
{
    public class Fiducial
    {
        private AlgoImage algoImage;
        private Rectangle rectangle;
        private DateTime grabbedDateTime;

        public AlgoImage AlgoImage
        {
            get { return algoImage; }
            set { algoImage = value; }
        }

        public Rectangle Rectnagle
        {
            get { return this.rectangle; }
        }

        public DateTime GrabbedDateTime
        {
            get { return this.grabbedDateTime; }
            set { this.grabbedDateTime = value; }
        }

        public bool IsValid
        {
            get { return this.rectangle.IsEmpty == false; }
        }

        public Fiducial(AlgoImage algoImage, Rectangle rectangle)
        {
            this.algoImage = algoImage;
            this.rectangle = rectangle;
            if(algoImage.Tag is DateTime)
                this.grabbedDateTime = (DateTime)algoImage.Tag;
        }

        public bool IntersectWith(Fiducial fiducial, int marginX = 0, int marginY = 0)
        {
            if (this.algoImage != fiducial.algoImage)
                return false;

            if (this.rectangle.IsEmpty || fiducial.rectangle.IsEmpty)
                return true;

            Rectangle rectA = this.rectangle;
            Rectangle rectB = fiducial.rectangle;
            rectA.Inflate(marginX, marginY);
            rectB.Inflate(marginX, marginY);
            return rectA.IntersectsWith(rectB);
        }

        public int GetMidLine()
        {
            return (this.rectangle.Top + this.rectangle.Bottom) / 2;
        }
    }

    public class SheetGrabProcesserG : GrabProcesserG
    {
        private Dictionary<IntPtr, AlgoImage> algoImageBuffer = null;
        private List<int> foundFidHeight;
        private ThreadHandler runningThreadHandler;
        private List<Task> delegateTaskList = null;

        //private SheetImageSet tempSheetImageSet = null;
        private int averageFoundFidHeight = 0;
        private int boundSearchHeight2 = 0;   // 이미지 접합 부분에서 검사할 높이
        private int imageArrivedCount = 0;
        private int foundSheetCount = 0;
        private int startSearchLine = 500;// 첫 이미지의 위 500라인은 SKIP

        // Fiducial 찾은 후 다음 Fiducial 검색 시작점까지 여유
        public const int SkipLines = 500;

        private SheetFinderBase algorithm = null;
        public SheetFinderBase Algorithm
        {
            get { return algorithm; }
            set
            {
                algorithm = value;
                boundSearchHeight2 = algorithm.GetBoundSize();
            }
        }

        bool isTestMode = false;
        string foundTextPath = "";
        string frameImagePath = "";
        string sheetImagePath = "";

        public bool SetTestMode(bool isTestMode, string testModePath)
        {
            this.isTestMode = isTestMode;
            this.foundTextPath = Path.Combine(testModePath, "Frame", "Result.txt");
            this.frameImagePath = Path.Combine(testModePath, "Frame");
            this.sheetImagePath = Path.Combine(testModePath, "Sheet");

            try
            {
                if (Directory.Exists(frameImagePath))
                    FileHelper.ClearFolder(frameImagePath);

                if (Directory.Exists(sheetImagePath))
                    FileHelper.ClearFolder(sheetImagePath);

                while (Directory.Exists(frameImagePath) == false)
                {
                    Directory.CreateDirectory(frameImagePath);
                    Thread.Sleep(100);
                }

                while (Directory.Exists(sheetImagePath) == false)
                {
                    Directory.CreateDirectory(sheetImagePath);
                    Thread.Sleep(100);
                }
            }
            catch (IOException ex)
            {
                MessageForm.Show(null, ex.Message);
                return false;
            }
            return true;
        }

        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy || grabbedImageQueue.Count > 0; }
        }

        private ManualResetEvent onImageGrabbed = new ManualResetEvent(false);

        /// <summary>
        /// 시트 이미지 완성. 후단에서 가져가지 않음.
        /// </summary>
        private List<SheetImageSet> sheetImageSetList = new List<SheetImageSet>();

        /// <summary>
        /// 시트 이미지 완성. 후단에서 가져감. Dispose되지 않음.
        /// </summary>
        private List<SheetImageSet> sheetImageSetList2 = new List<SheetImageSet>();

        private DateTime lastGrabDoneTime = DateTime.Now;

        private DebugContext debugContext = null;

        public SheetGrabProcesserG()
        {
            this.algoImageBuffer = new Dictionary<IntPtr, AlgoImage>();
            this.delegateTaskList = new List<Task>();
            this.foundFidHeight = new List<int>();
        }

        ~SheetGrabProcesserG()
        {
            Dispose();
        }

        public void Initialize(AlgorithmParam param)
        {
            this.algorithm = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
        }

        public void SetFoundSheetCount(int foundSheetCount =0)
        {
            this.foundSheetCount = foundSheetCount;
        }

        public override SheetImageSet GetLastSheetImageSet()
        {
            SheetImageSet imageSet = null;
            lock (sheetImageSetList)
            {
                if (this.sheetImageSetList.Count > 0)
                {
                    imageSet = this.sheetImageSetList[0];
                    this.sheetImageSetList.RemoveAt(0);
                }
            }

            if (imageSet != null)
            {
                imageSet.OnSheetImageSetDispose = SheetImageSet_OnSheetImageSetDispose;
                lock (sheetImageSetList2)
                    sheetImageSetList2.Add(imageSet);
            }

            if (this.sheetImageSetList.Count == 0)
                this.isFullImageGrabbed.Reset();

            return imageSet;
        }

        private void SheetImageSet_OnSheetImageSetDispose(SheetImageSet sheetImageSet)
        {
            lock (sheetImageSetList2)
                sheetImageSetList2.Remove(sheetImageSet);
        }

        public override void Start()
        {
            string tempFolder = Configuration.TempFolder;
            if (string.IsNullOrEmpty(tempFolder))
                tempFolder = Path.Combine(Environment.CurrentDirectory, "Temp");
            debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(tempFolder, "GrabProcesser"));
            
            Debug.Assert(this.algorithm != null);
            Debug.Assert(this.runningThreadHandler == null);

            WriteLog(-1, -1, null, -1, debugContext);

            //if(this.runningThreadHandler.IsRunning)
            //{
            //    this.runningThreadHandler.Stop();
            //    ThreadManager.RemoveThread(this.runningThreadHandler);
            //    this.runningThreadHandler = null;
            //}
            grabbedSheetHeight.Clear();

            this.runningThreadHandler = new ThreadHandler("SheetGrabProcesserG", new Thread(ThreadWorkProc), false);
            this.runningThreadHandler.WorkingThread.Priority = ThreadPriority.Highest;
            ThreadManager.AddThread(this.runningThreadHandler);
            isFullImageGrabbed.Reset();

            this.runningThreadHandler.Start();
        }

        public override void Stop()
        {
            if (this.runningThreadHandler != null)
            {
                this.runningThreadHandler.Stop();
                WriteLog(-1, -1, null, -1, debugContext);
                ThreadManager.RemoveThread(this.runningThreadHandler);
            }

            Task delegateTask = null;
            do
            {
                lock (delegateTaskList)
                    delegateTask = this.delegateTaskList.Find(f => f.IsCompleted == false);
                delegateTask?.Wait(100);
            } while (delegateTask != null);

            this.delegateTaskList.Clear();


            this.runningThreadHandler = null;
        }

        Queue<AlgoImage> grabbedImageQueue = new Queue<AlgoImage>();
        //ConcurrentQueue<AlgoImage> grabbedImageQueue = new ConcurrentQueue<AlgoImage>();
        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            //if (imageDevice is CameraVirtualMS)
            //{
            //    if (this.algoImageBuffer.ContainsKey(ptr))
            //    {
            //        this.algoImageBuffer[ptr].Dispose();
            //        this.algoImageBuffer.Remove(ptr);
            //    }
            //}

            if (this.algoImageBuffer.ContainsKey(ptr) == false)
            {
                ImageD imageD = (ImageD)imageDevice.GetGrabbedImage(ptr);
                imageD.ConvertFromData();   // 이거로 DataPtr을 정해주면 Mil에서 Alloc하지 않고 Pointer로 참조해감. (가상카메라에서 더 빨라짐)
                AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                this.algoImageBuffer.Add(ptr, algoImage);
            }

            bool isVirtualCamera = imageDevice is CameraVirtual;
            bool isVirtualCameraMS = imageDevice is CameraVirtualMS;
            if (isVirtualCamera && !(isVirtualCameraMS))
            {
                SheetImageSet sheetImageSet = new SheetImageSet(this.foundSheetCount++);
                Rectangle seheetImageRect = new Rectangle(Point.Empty, this.algoImageBuffer[ptr].Size);
                sheetImageSet.AddSubImage(this.algoImageBuffer[ptr].GetSubImage(seheetImageRect));
                sheetImageSet.Tag = DateTime.Now;
                OnFullSheeetImageGrabbed(sheetImageSet, debugContext);
                return;
            }

            //ImageD imageD2 = (ImageD)imageDevice.GetGrabbedImage(ptr);
            //AlgoImage algoImage2 = this.algoImageBuffer[ptr];
            grabbedImageQueue.Enqueue(this.algoImageBuffer[ptr]);

            onImageGrabbed.Set();
        }

        public void ImageGrabbed(AlgoImage algoImage)
        {
            algoImage.Tag = DateTime.Now;
            grabbedImageQueue.Enqueue(algoImage);
            onImageGrabbed.Set();
        }

        public void Clear()
        {
            Debug.Assert(this.delegateTaskList.Exists(f => f.IsCompleted == false) == false);

            lock (sheetImageSetList)
            {
                LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::Clear(), sheetImageSetList");
                foreach (SheetImageSet imageSet in this.sheetImageSetList)
                    imageSet.Dispose();
                this.sheetImageSetList.Clear();
            }

            LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::Clear(), algoImageBuffer");
            foreach (KeyValuePair<IntPtr, AlgoImage> pair in this.algoImageBuffer)
                pair.Value.Dispose();
            algoImageBuffer.Clear();
        }

        public bool IsWaiting()
        {
            return grabbedImageQueue.Count == 0;
        }

        public bool IsFullImageGrabbed()
        {
            return (this.sheetImageSetList.Count > 0);
        }

        private ManualResetEvent isFullImageGrabbed = new ManualResetEvent(false);
        public bool WaitFullImageGrabbed(int waitTimeMs = -1)
        {
            //TimeOutTimer tot = new TimeOutTimer();
            //bool result = IsFullImageGrabbed();
            //if (waitTimeMs >= 0)
            //    tot.Start(waitTimeMs);
            //while (tot.TimeOut == false && result == false)
            //{
            //    result = (this.sheetImageSetList.Count != 0);
            //}
            //return result;
            return isFullImageGrabbed.WaitOne(waitTimeMs);
        }

        private void ThreadWorkProc()
        {
            Stopwatch sw = new Stopwatch();

            //this.SetAffinity(0);
            isRunning = true;
            AlgoImage prevAlgoImage = null;
            //debugContext.SaveDebugImage = true;

            List<Fiducial> fiducialList = new List<Fiducial>();
            List<Task> saveTaskList = new List<Task>();
            try
            {
                while (this.runningThreadHandler.RequestStop == false || onImageGrabbed.WaitOne(0))
                {
                    // 1회 루프마다, 최대 500ms 주기로 작동함.
                    lock (delegateTaskList)
                        delegateTaskList.RemoveAll(f => f.IsCompleted);

                    // 버퍼가 비어있으면 찰때까지 기다림
                    if (onImageGrabbed.WaitOne(500) == false)
                    {
                        continue;
                    }
                    onImageGrabbed.Reset();

                    AlgoImage curAlgoImage = grabbedImageQueue.Dequeue();
                    //isTestMode = false;
                    this.isBusy = true;

                    CameraBufferTag currentTag = curAlgoImage.Tag as CameraBufferTag;
                    LogHelper.Debug(LoggerType.Inspection, string.Format("Frame,{0}({1}),Grabbed", currentTag.FrameId, currentTag.BufferId));
                    curAlgoImage.Save(string.Format("Frame {0}.bmp", currentTag.FrameId), debugContext);

                    // Fiducial 찾기
                    AlgoImage boundAlgoImage = null;
                    Image2D boundImage = null;

                    if (prevAlgoImage != null)
                    {
                        // 이전 프레임과 현재 프레임의 경계부분에서 검색.
                        CameraBufferTag previousTag = prevAlgoImage.Tag as CameraBufferTag;
                        Debug.Assert(prevAlgoImage.Width == curAlgoImage.Width);
                        //Debug.Assert(previousTag.FrameId < currentTag.FrameId, "Old FramdID Detected!");

                        Rectangle upperBoundRect = Rectangle.FromLTRB(0, 0, curAlgoImage.Width, boundSearchHeight2);
                        Rectangle lowerBoundRect = Rectangle.FromLTRB(0, prevAlgoImage.Height - boundSearchHeight2, prevAlgoImage.Width, prevAlgoImage.Height);

                        if (lowerBoundRect.Height > 0 && lowerBoundRect.Height > 0)
                        {
                            AlgoImage prevBoundAlgoImage = prevAlgoImage.GetSubImage(lowerBoundRect);
                            AlgoImage curBoundAlgoImage = curAlgoImage.GetSubImage(upperBoundRect);

                            if (false)
                            {
                                // testCode
                                ImageD imageD = new Image2D(@"D:\Temp\Frame 7-8.bmp");
                                boundAlgoImage = ImageBuilder.Build(curAlgoImage.LibraryType, imageD, curAlgoImage.ImageType);
                            }
                            else
                            {
                                boundAlgoImage = ImageBuilder.Build(curAlgoImage.LibraryType, curAlgoImage.ImageType, curAlgoImage.Width, boundSearchHeight2 * 2);
                                AlgorithmBuilder.GetImageProcessing(curAlgoImage).Stitch(prevBoundAlgoImage, curBoundAlgoImage, boundAlgoImage, Direction.Vertical);
                                //boundAlgoImage.Save(Path.Combine(debugContext.FullPath, string.Format("{0}-{1}.bmp", previousTag.FrameId, currentTag.FrameId)));
                            }
                            boundAlgoImage.Save(string.Format("Frame {0}-{1}.bmp", previousTag.FrameId, currentTag.FrameId), debugContext);
                            sw.Restart();

                            // 원래 이미지 및 좌표 찾아가기. Fiducial List 등록
                            DebugContext boundDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("Frame {0}-{1}", previousTag.FrameId, currentTag.FrameId)));
                            List<Rectangle> boundFiducialRectListTemp = FindFiducial(boundAlgoImage, 0, 0, boundDebugContext);
                            List<Rectangle> boundFiducialRectList = MergeFiducial(boundFiducialRectListTemp, 300, 300);
                            boundFiducialRectList.ForEach(f =>
                            {
                                Fiducial newFiducial = null;
                                int midY = (f.Top + f.Bottom) / 2;
                                if (midY < boundSearchHeight2)
                                {
                                    // 이전 이미지에서 검출됨.
                                    f.Offset(0, prevAlgoImage.Height - boundSearchHeight2);
                                    newFiducial = new Fiducial(prevAlgoImage, f);
                                }
                                else
                                {
                                    // 현재 이미지에서 검출됨
                                    f.Offset(0, -boundSearchHeight2);
                                    newFiducial = new Fiducial(curAlgoImage, f);
                                }

                                Fiducial oldFiducial = fiducialList.Find(g => newFiducial.IntersectWith(g));
                                if (oldFiducial != null && oldFiducial.IsValid == false)
                                {
                                    fiducialList.Remove(oldFiducial);
                                    newFiducial.GrabbedDateTime = oldFiducial.GrabbedDateTime;
                                }
                                fiducialList.Add(newFiducial);
                            });
                            LogHelper.Debug(LoggerType.Inspection, string.Format("Founded Fiducials(1/2): {0}", fiducialList.Count));

                            if (isTestMode)
                            {
                                saveTaskList.Add(Task.Run(() =>
                                {
                                    string saveImageName = string.Format("Frame {0}-{1}.bmp", previousTag.FrameId, currentTag.FrameId);
                                    //AlgoImage saveImage = boundAlgoImage.Clone(ImageType.Color);
                                    //ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(saveImage);
                                    //boundFiducialRectList.ForEach(f => ip.DrawRect(saveImage, f, Color.Red.ToArgb(), true));
                                    //saveImage?.Save(Path.Combine(this.frameImagePath, saveImageName), 0.1f, null);
                                    //saveImage?.Dispose();
                                }));
                            }

                            sw.Stop();
                            curBoundAlgoImage.Dispose();
                            prevBoundAlgoImage.Dispose();
                        }
                    }

                    // 현재 프레임에서 검색
                    sw.Restart();
                    DebugContext rangeDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("Frame {0}", currentTag.FrameId)));
                    List<Rectangle> curFiducialListTemp = FindFiducial(curAlgoImage, startSearchLine, 0, rangeDebugContext);
                    //List<Rectangle> curFiducialListTemp = FindFiducial(curAlgoImage, startSearchLine, averageFoundFidHeight, debugContext);
                    List<Rectangle> curFiducialList = MergeFiducial(curFiducialListTemp, 300, 300);
                    if (curFiducialList.Count == 0)
                    {
                        Fiducial newFiducial = new Fiducial(curAlgoImage, Rectangle.Empty);
                        bool exist = fiducialList.Exists(g => newFiducial.IntersectWith(g));
                        if (exist == false)
                        {
                            fiducialList.Add(newFiducial);
                            //foundedInCurrent = true;
                        }
                    }
                    else
                    {
                        curFiducialList.ForEach(f =>
                        {
                            Fiducial newFiducial = new Fiducial(curAlgoImage, f);
                            Fiducial oldFiducial = fiducialList.Find(g =>  newFiducial.IntersectWith(g));
                            if (oldFiducial != null)
                            {
                                newFiducial.GrabbedDateTime = oldFiducial.GrabbedDateTime;
                                fiducialList.Remove(oldFiducial);
                            }

                            fiducialList.Add(newFiducial);
                            foundFidHeight.Add(newFiducial.Rectnagle.Height);
                            foundFidHeight.Sort();
                        });
                    }
                    LogHelper.Debug(LoggerType.Inspection, string.Format("Founded Fiducials(2/2): {0}", fiducialList.Count));

                    if (isTestMode)
                    {
                        saveTaskList.Add(Task.Run(() =>
                        {
                            ulong curFrameId = (curAlgoImage.Tag as CameraBufferTag).FrameId;
                            string saveImageName = string.Format("Frame {0}.bmp", curFrameId);
                            string saveImageName2 = string.Format("Frame {0}_Marked.bmp", curFrameId);
                            curAlgoImage?.Save(Path.Combine(this.frameImagePath, saveImageName), 0.1f, null);

                            //AlgoImage saveImage = curAlgoImage.Clone(ImageType.Color);
                            //ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(saveImage);
                            //curFiducialList.ForEach(f => ip.DrawRect(saveImage, f, Color.Red.ToArgb(), true));
                            //saveImage?.Save(Path.Combine(this.frameImagePath, saveImageName2), 0.1f, null);
                            //saveImage?.Dispose();
                        }));
                    }

                    if (foundFidHeight.Count > 5)
                    {
                        // 6개 중 평균과 가장 멀리 떨어진 값을 제거.
                        double avg = foundFidHeight.Average();
                        int min = foundFidHeight.Min();
                        int max = foundFidHeight.Max();

                        double diffMin = avg - min;
                        double diffMax = max - avg;
                        if (diffMin > diffMax)
                            foundFidHeight.Remove(min);
                        else
                            foundFidHeight.Remove(max);
                    }

                    if (foundFidHeight.Count > 0)
                    {
                        averageFoundFidHeight = (int)Math.Round(foundFidHeight.Average());
                        this.boundSearchHeight2 = (int)(averageFoundFidHeight * 1.5);
                    }

                    WriteLog(curAlgoImage.Width, curAlgoImage.Height, fiducialList, sw.ElapsedMilliseconds, debugContext);

                    prevAlgoImage = curAlgoImage;

                    //Debug.Assert(fiducialList.Exists(f => {
                    //    if (f.Rectnagle.IsEmpty == false)
                    //    {
                    //        Rectangle imageRect = new Rectangle(Point.Empty, f.AlgoImage.Size);
                    //        return Rectangle.Intersect(imageRect, f.Rectnagle) != f.Rectnagle;
                    //    }
                    //    return false;
                    //}));

                    CalcFiducial(fiducialList, debugContext);
                    sw.Stop();
                    
                    startSearchLine = 0;
                    sw.Stop();

                    while (saveTaskList.Exists(f => f.IsCompleted == false))
                        Thread.Sleep(50);

                    saveTaskList.Clear();

                    boundAlgoImage?.Dispose();

                    this.isBusy = false;

                    if (grabbedImageQueue.Count >= 1)
                        onImageGrabbed.Set();

                    //GC.Collect();
                    //GC.WaitForFullGCComplete();
                }
            }
            catch (ThreadAbortException)
            {
                LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::ThreadWorkProc ThreadAbortException");
            }
            finally
            {
                //fiducialList.ForEach(f => f.AlgoImage.Dispose());
                isRunning = false;
                sw.Stop();
            }
        }

        private List<Rectangle> MergeFiducial(List<Rectangle> curFiducialList, int marginX, int marginY)
        {
            List<Rectangle> result = new List<Rectangle>();
            List<Rectangle> temp = new List<Rectangle>(curFiducialList);
            while (temp.Count > 0)
            {
                Rectangle rectangle = temp[0];
                Rectangle inflated = temp[0];
                inflated.Inflate(marginX, marginY);

                List<Rectangle> intersectList = temp.FindAll(f => inflated.IntersectsWith(f));
                temp.RemoveAll(f=>intersectList.Contains(f));
                intersectList.ForEach(f => rectangle = Rectangle.Union(rectangle, f));
                result.Add(rectangle);
            }
            return result;
        }

        List<int> grabbedSheetHeight = new List<int>();
        private void CalcFiducial(List<Fiducial> fiducialList, DebugContext debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("GrabProcesserG::CalcFiducial - Start (Valid fiducial is {0})", fiducialList.Count(f => f.IsValid)));
            do
            {
                int src = fiducialList.FindIndex(0, f => f.IsValid);
                int dst = fiducialList.FindIndex(src + 1, f => f.IsValid);
                if (dst < 0)
                {
                    LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::CalcFiducial - dst is negative");
                    if (src < 0)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::CalcFiducial - src is negative");
                        fiducialList.Clear();
                    }
                    if (src == 0 && fiducialList.Count > 3)
                    {
                        // 시트가 인쇄 중단됨 -> 시작GAP은 있으나 종료GAP이 없음.
                        LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::CalcFiducial - too long gap");
                        fiducialList.Clear();
                    }
                    else if (src > 0)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::CalcFiducial - src is positive");
                        fiducialList.RemoveRange(0, src);
                    }
                    break;
                }

                int count = dst - src + 1;
                List<Fiducial> calcFiducialList = fiducialList.GetRange(src, count);
                SheetImageSet tempSheetImageSet = new SheetImageSet(foundSheetCount);
                tempSheetImageSet.Name = tempSheetImageSet.SheetNo.ToString();
                tempSheetImageSet.Tag = fiducialList[src].GrabbedDateTime;

                if (fiducialList[src].AlgoImage == fiducialList[dst].AlgoImage)
                {
                    AlgoImage algoImage = fiducialList[src].AlgoImage;
                    int srcY = fiducialList[src].GetMidLine();
                    int dstY = fiducialList[dst].GetMidLine();

                    Rectangle clipRect = Rectangle.FromLTRB(0, srcY, algoImage.Width, dstY);
                    if (clipRect.Width > 0 && clipRect.Height > 0)
                        tempSheetImageSet.AddSubImage(algoImage.GetSubImage(clipRect));
                }
                else
                {
                    for (int i = src; i <= dst; i++)
                    {
                        Fiducial fiducial = fiducialList[i];
                        Rectangle clipRect = new Rectangle(Point.Empty, fiducial.AlgoImage.Size);
                        if (fiducial.IsValid)
                        {
                            int midLine = fiducial.GetMidLine();
                            if (i == src)
                                clipRect = Rectangle.FromLTRB(0, midLine, fiducial.AlgoImage.Width, fiducial.AlgoImage.Height);
                            else if (i == dst)
                                clipRect = Rectangle.FromLTRB(0, 0, fiducial.AlgoImage.Width, midLine);
                        }
                        if (clipRect.Width > 0 && clipRect.Height > 0)
                            tempSheetImageSet.AddSubImage(fiducial.AlgoImage.GetSubImage(clipRect));
                    }
                }

                if (tempSheetImageSet.Count > 0)
                {
                    string logMessage = string.Format("Sheet Founded: SheetNo {0}, SheetHeight {1}", foundSheetCount, tempSheetImageSet.Height);
                    LogHelper.Info(LoggerType.Inspection, logMessage);
                    
                    Debug.WriteLine(logMessage);
                    //if (debugContext.SaveDebugImage)
                    //File.AppendAllText(Path.Combine(debugContext.FullPath, "SheetLength.txt"), string.Format("[{0}],{1},{2},{3}\r\n",
                    //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), SystemManager.Instance().CurrentModel.Name, foundSheetCount, tempSheetImageSet.Height));

                    grabbedSheetHeight.Add(tempSheetImageSet.Height);
                    grabbedSheetHeight.Sort();
                    int grabbedSheetHeightCount = grabbedSheetHeight.Count;
                    if (grabbedSheetHeightCount > 10)
                    {
                        float average = (float)grabbedSheetHeight.Average();
                        float diff1 = average - grabbedSheetHeight.First();
                        float diff2 = grabbedSheetHeight.Last() - average;
                        if (diff1 > diff2)
                            grabbedSheetHeight.Remove(grabbedSheetHeight.First());
                        else
                            grabbedSheetHeight.Remove(grabbedSheetHeight.Last());

                        float min = average * 0.8f;
                        float max = average * 1.2f;
                        if (tempSheetImageSet.Height > min && tempSheetImageSet.Height < max)
                        {
                            OnFullSheeetImageGrabbed(tempSheetImageSet, debugContext);
                        }
                        else
                        {
                            LogHelper.Info(LoggerType.Inspection, string.Format("Sheet Size Fault: Sheet No {0}, SheetHeight {1}", foundSheetCount, tempSheetImageSet.Height));
                            tempSheetImageSet.Dispose();
                        }
                        foundSheetCount++;
                    }
                    else
                    {
                        OnFullSheeetImageGrabbed(tempSheetImageSet, debugContext);
                        foundSheetCount++;
                    }
                }
                else
                    tempSheetImageSet.Dispose();

                tempSheetImageSet = null;
                fiducialList.RemoveRange(src, count - 1);
            } while (fiducialList.Count > 0);
            LogHelper.Debug(LoggerType.Inspection, "GrabProcesserG::CalcFiducial - End");
        }

        private void OnFullSheeetImageGrabbed(SheetImageSet sheetImageSet, DebugContext debugContext)
        {
            LogHelper.Info(LoggerType.Inspection, string.Format("Sheet,{0},Grabbed,H{1}", sheetImageSet.SheetNo, sheetImageSet.Height));

            lock (sheetImageSetList)
            {
                this.sheetImageSetList.Add(sheetImageSet);
            }

            if (this.isTestMode)
            {
                string saveFile = "";
                if (sheetImageSet.SheetNo < 0)
                {
                    saveFile = string.Format("ImageGrabbed_{0}.bmp", sheetImageSet.SheetNo < 0 ? DateTime.Now.Ticks : sheetImageSet.SheetNo);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("ImageGrabbed");
                    for (int i = 0; i < sheetImageSet.Count; i++)
                    {
                        sb.Append(string.Format("_{0}", (sheetImageSet.GetChildImage(i).ParentImage.Tag as CameraBufferTag).FrameId.ToString()));
                    }
                    sb.Append(".bmp");

                    saveFile = sb.ToString();
                }
                Image2D currentImage = (Image2D)sheetImageSet.ToImageD();
                ImageD sacled = currentImage.Resize(0.05f, 0.05f);
                sacled.SaveImage(Path.Combine(this.sheetImagePath, saveFile));
                sacled.Dispose();
                currentImage.Dispose();
            }

            isFullImageGrabbed.Set();

            if (this.IsRunning && StartInspectionDelegate != null)
            {
                //StartInspectionDelegate?.Invoke(null, IntPtr.Zero);
                StartInspectionDelegate(null, IntPtr.Zero);
            }
        }

        private void WriteLog(int width, int height, List<Fiducial> fiducialList, long elapsedMilliseconds, DebugContext debugContext)
        {
#if DEBUG==false
            if (debugContext.SaveDebugImage == false)
                return;
#endif

            byte[] logBytes = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), width, height, elapsedMilliseconds, GC.GetTotalMemory(false)));
            if (fiducialList != null)
            {
                foreach (Fiducial fiducial in fiducialList)
                    sb.AppendLine(string.Format("\t{0},{1}", fiducial.AlgoImage.Tag.ToString(), fiducial.Rectnagle.ToString()));
            }
            sb.AppendLine();

            logBytes = Encoding.Default.GetBytes(sb.ToString());
            string path = debugContext.FullPath;
            string name = "Fiducial.txt";
            string file = Path.Combine(path, name);

            Directory.CreateDirectory(path);

            if (File.Exists(file) == false)
                File.WriteAllText(file, "DateTime,Width,Height,ElapsedTimeMs,MemoryUsage");

            FileStream fs = File.Open(Path.Combine(path, name), FileMode.Append, FileAccess.Write);
            fs.Seek(0, SeekOrigin.End);
            fs.Write(logBytes, 0, logBytes.Length);
            fs.Close();
        }

        public int GetBufferCount()
        {
            return this.grabbedImageQueue.Count + (this.isBusy ? 1 : 0);
        }

        private void AppendLog(SheetImageSet tempSheetImageSet)
        {
            Directory.CreateDirectory(debugContext.FullPath);

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} ", tempSheetImageSet.SheetNo));
            //tempSheetImageSet.subImageList.ForEach(f => sb.Append(string.Format("\t{0}", f.Tag)));

            //StreamWriter streamWriter = new StreamWriter(this.logSheetFile, true);
            //streamWriter.WriteLine(sb.ToString());
            //streamWriter.Close();
            //streamWriter.Dispose();
        }

        private void AppendLog(ulong frameId, int bufferId, List<Point> fidPointList)
        {
            Directory.CreateDirectory(debugContext.FullPath);

            StringBuilder sb = new StringBuilder();
            fidPointList.ForEach(f => sb.AppendLine(string.Format("Frame{0} Buffer{1} Position{2}", frameId, bufferId, f.ToString())));

            //StreamWriter streamWriter = new StreamWriter(this.logFiducialFile, true);
            //streamWriter.Write(sb.ToString());
            //streamWriter?.Close();
        }

        private List<Rectangle> FindFiducial(AlgoImage algoImage, int startSearch, int fidHeight, DebugContext debugContext)
        {
            List<Rectangle> rectList = new List<Rectangle>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Rectangle subRect = Rectangle.FromLTRB(0, startSearch, algoImage.Width, algoImage.Height);
            if (subRect.Height > 0 && subRect.Width > 0)
            {
                AlgoImage subAlgoImage = algoImage.GetSubImage(subRect);
                SheetInspectParam sheetInspectParam = new SheetInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext);
                sheetInspectParam.AlgoImage = subAlgoImage;

                //if (fidHeight > 0)
                ((SheetFinderBaseParam)algorithm.Param).FidSize = new Size(0, fidHeight);

                SheetFinderResult sheetFinderResult = (SheetFinderResult)algorithm.Inspect(sheetInspectParam);
                LogHelper.Debug(LoggerType.Debug, string.Format("SheetFinder: {0} ms in {1}", sheetFinderResult.SpandTime.TotalMilliseconds, subAlgoImage.Height));
                subAlgoImage.Dispose();

                foreach (Rectangle foundedFiducialRect in sheetFinderResult.FoundedFiducialRectList)
                {
                    foundedFiducialRect.Offset(0, startSearch);
                    rectList.Add(foundedFiducialRect);
                }
            }

            return rectList;
        }

        public bool IsDisposable()
        {
            if (this.IsRunning)
                return false;

            return sheetImageSetList2.Count == 0;
        }

        public override void Dispose()
        {
            Stop();
            Clear();
        }

        public override IntPtr GetGrabbedImagePtr()
        {
            throw new NotImplementedException();
        }
    }
}
//using DynMvp.Base;
//using DynMvp.Device.Device.FrameGrabber;
//using DynMvp.UI;
//using DynMvp.Vision;
//using DynMvp.Vision.Matrox;
//using UniScanG.Algorithms;
//using UniScanG.Operation.Data;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using UniEye.Base;
//using UniEye.Base.Settings;

//namespace UniScanG.Temp
//{
//    internal class GrabProcesser : ThreadHandler, IDisposable
//    {
//        // Fiducial 찾은 후 다음 Fiducial 검색 시작점까지 여유
//        public const int SkipLines = 500;

//        int sheetNo = -1;

//        public ImageGrabCompleteDelegate ImageGrabComplete = null;

//        //private Size sheetImageSize;
//        //public Size SheetImageSize
//        //{
//        //    get { return sheetImageSize; }
//        //    set { sheetImageSize = value; }
//        //}

//        private int clientIndex = -1;
//        public int ClientIndex
//        {
//            get { return clientIndex; }
//            set { clientIndex = value; }
//        }

//        Size sheetImageSize;
//        private Algorithm sheetCheckerFiducial = null;
//        //public void SetAlgorithm(SheetCheckerFiducial sheetCheckerFiducial)
//        //{
//        //    this.sheetCheckerFiducial = sheetCheckerFiducial;
//        //    FiducialFinderParam sheetCheckerParam = sheetCheckerFiducial.Param as FiducialFinderParam;
//        //    sheetImageSize = sheetCheckerParam.SheetSizePx;
//        //}

//        private bool isRunning = false;
//        public bool IsRunning
//        {
//            get { return isRunning; }
//        }

//        private bool isBusy = false;
//        public bool IsBusy
//        {
//            get { return isBusy || grabbedImageQueue.Count>0; }
//        }

//        private string logImagePath = "";
//        private string logSheetFile = "";
//        private string logFiducialFile = "";

//        private ManualResetEvent onImageGrabbed = new ManualResetEvent(false);
//        private List<SheetImageSet> sheetImageSet = new List<SheetImageSet>();
//        private SheetImageSet tempSheetImageSet = null;
//        private int startSearchLine = 500;
//        private int reamainLines = -1;
//        private int imageArrivedCount = 0;
//        private DateTime lastGrabDoneTime = DateTime.Now;

//        private DebugContext debugContext = null;

//        public GrabProcesser()
//        {
//            LogHelper.Debug(LoggerType.Grab, "GrabProcesser::GrabProcesser");
//            this.RequestStop = false;
//            this.WorkingThread = new Thread(ThreadWorkProc);
//        }

//        ~GrabProcesser()
//        {
//            Dispose();
//        }

//        public void Initialize(SheetCheckerParam param)
//        {
//            LogHelper.Debug(LoggerType.Grab, "GrabProcesser::Initialize");
//            sheetCheckerFiducial = new FiducialFinder(param);
//            this.sheetImageSize = param.GetSheetSizePx();
//        }

//        public SheetImageSet GetLastSheetImageSet()
//        {
//            LogHelper.Debug(LoggerType.Grab, "GrabProcesser::GetLastSheetImageSet");
//            SheetImageSet imageSet = null;
//            lock (sheetImageSet)
//            {
//                if (this.sheetImageSet.Count > 0)
//                {
//                    //LogHelper.Debug(LoggerType.Grab, "GrabProcesser::GetLastSheetImageSet TRUE");
//                    imageSet = this.sheetImageSet[0];
//                    this.sheetImageSet.RemoveAt(0);
//                }
//            }
//            return imageSet;
//        }

//        public void Start()
//        {
//            LogHelper.Debug(LoggerType.Grab, "GrabProcesser::Start");
//            Debug.Assert(this.sheetCheckerFiducial != null);

//            debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(Configuration.TempFolder, "GrabProcesser"));
            
//            this.logImagePath = Path.Combine(UniScanGSettings.Instance().SaveImageDebugDataPath, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
//            this.logSheetFile = Path.Combine(debugContext.Path, "_FullSheetImage.txt");
//            this.logFiducialFile = Path.Combine(debugContext.Path, "_FidPos.txt");
//            //if (File.Exists(this.logSheet))

//            reamainLines = this.sheetImageSize.Height;
//            startSearchLine = 00;//zmsong  sheetImageSize.Height / 10;

//            this.RequestStop = false;
//            this.WorkingThread = new Thread(ThreadWorkProc);
//            this.WorkingThread.Priority = ThreadPriority.Highest;
//            this.WorkingThread.Start();
//        }

//        public void Stop()
//        {
//            this.RequestStop = true;
//            //if (this.WorkingThread != null)
//            //    while (this.WorkingThread.IsAlive) ;
//            //this.WorkingThread = null;
//        }

//        ConcurrentQueue<AlgoImage> grabbedImageQueue = new ConcurrentQueue<AlgoImage>();

//        public void OnNewImageArrived(AlgoImage algoImage)
//        {
//            LogHelper.Info(LoggerType.Grab, "GrabProcesser::OnNewImageArrived");
//            grabbedImageQueue.Enqueue(algoImage);
//        }

//        public void Clear()
//        {
//            LogHelper.Debug(LoggerType.Grab, "GrabProcesser::Clear");
//            lock (sheetImageSet)
//            {
//                foreach (SheetImageSet imageSet in this.sheetImageSet)
//                {
//                    imageSet.Dispose();
//                }
//                this.sheetImageSet.Clear();

//                //if (curAlgoImage != null)
//                //    curAlgoImage.Dispose();

//                //if (prevAlgoImage != null)
//                //    prevAlgoImage.Dispose();

//                tempSheetImageSet?.Dispose();
//                this.reamainLines = this.sheetImageSize.Height;
//            }
//        }

//        public bool IsWaiting()
//        {
//            return grabbedImageQueue.Count == 0;
//        }

//        public bool IsFullImageGrabbed()
//        {
//            return (this.sheetImageSet.Count != 0);
//        }

//        private void ThreadWorkProc()
//        {
//            Stopwatch sw = new Stopwatch();
//            SheetCheckerParam sheetCheckerParam = sheetCheckerFiducial.Param as SheetCheckerParam;
//            FiducialFinderParam fiducialFinderParam = sheetCheckerParam.FiducialFinderParam;

//            int frameBoundStitchBufferHeight = 2 * fiducialFinderParam.FidSizeHeight;
//            int frameBoundStitchBufferHeight2 = frameBoundStitchBufferHeight / 1;

//            AlgoImage frameBoundStitchBuffer = null;
//            AlgoImage curAlgoImage = null;
//            AlgoImage prevAlgoImage = null;

//            try
//            {
//                this.SetAffinity(0);
//                isRunning = true;

//                while (this.RequestStop == false)
//                {
//                    if (IsWaiting())
//                    {
//                        Thread.Sleep(0);
//                        continue;
//                    }
//                    if (grabbedImageQueue.TryDequeue(out curAlgoImage) == false)
//                    {
//                        Thread.Sleep(0);
//                        continue;
//                    }

//                    this.isBusy = true;

//                    sw.Restart();

//                    CameraBufferTag currentTag = curAlgoImage.Tag as CameraBufferTag;

//                    bool saveImage = ((UniScanGSettings.Instance().SaveImageDebugData & SaveDebugData.Image) > 0);
//                    curAlgoImage.Save(Path.Combine("Frame", string.Format("FrameNo {0}.bmp", currentTag.FrameId)), 0.1f, new DebugContext(saveImage, this.logImagePath));

//                    LogHelper.Debug(LoggerType.Grab, string.Format("GrabProcesser::ThreadWorkProc Activate - BufferId: {0}, FrameId: {1}", currentTag.BufferId, currentTag.FrameId));

//                    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(curAlgoImage);

//                    if (HasFirstChild())
//                    // 첫번째 조각이 있다.
//                    {
//                        // 남은 줄만큼 두번째 조각으로 설정.
//                        SetChildImage(curAlgoImage, 0, 0, -1);
//                    }
//                    else if (prevAlgoImage != null)
//                    // 이전 이미지의 끝 + 현재 이미지의 처음 영상에서 검사 수행 - Fiducial이 잘려있을 경우 대비
//                    {
//                        // 이전 이미지의 일부와 새 이미지의 일부를 합쳐 새 이미지를 생성함.
//                        if (frameBoundStitchBuffer == null)
//                        {
//                            frameBoundStitchBuffer = ImageBuilder.Build(curAlgoImage.LibraryType, curAlgoImage.ImageType, curAlgoImage.Width, frameBoundStitchBufferHeight);
//                            frameBoundStitchBuffer.Name = "FrameBoundBuffer";
//                        }
//                        Debug.Assert(prevAlgoImage.Width == curAlgoImage.Width);
//                        CameraBufferTag prevTag = prevAlgoImage.Tag as CameraBufferTag;
//                        AlgoImage upper = prevAlgoImage.GetSubImage(Rectangle.FromLTRB(0, prevAlgoImage.Height - frameBoundStitchBufferHeight2, prevAlgoImage.Width, prevAlgoImage.Height));
//                        AlgoImage lower = curAlgoImage.GetSubImage(Rectangle.FromLTRB(0, 0, curAlgoImage.Width, frameBoundStitchBufferHeight2));
//                        imageProcessing.Stitch(upper, lower, frameBoundStitchBuffer, Direction.Vertical);
//                        //frameBoundStitchBuffer.Save(string.Format("Frame {0}_{1}.jpg", prevAlgoImage.Tag, curAlgoImage.Tag), 0.1f, new DebugContext(true, debugContext.Path));
//                        frameBoundStitchBuffer.Save(string.Format("Frame\\FrameNo {0}_{1}.jpg", prevTag.FrameId, currentTag.FrameId), 0.1f, debugContext);

//                        List<Point> fidPointList = FindFiducial(frameBoundStitchBuffer, 0, debugContext);
//                        if (fidPointList.Count>0)
//                        {
//                            Point fidPoint = fidPointList[0];
//                            int fidYSrc = fidPoint.Y - frameBoundStitchBufferHeight2;
//                            if (fidYSrc < 0)
//                            {
//                                SetChildImage(prevAlgoImage, fidPoint.X, prevAlgoImage.Height + fidYSrc, -1);
//                                SetChildImage(curAlgoImage, fidPoint.X, 0, -1);
//                                startSearchLine = 0;
//                            }
//                            else
//                            {
//                                SetChildImage(curAlgoImage, fidPoint.X, fidYSrc, -1);
//                            }
//                        }
//                        upper.Dispose();
//                        lower.Dispose();
//                        //frameBoundStitchBuffer.Clear();
//                    }

//                    // ---------------------------------------------------------------------Fiducial 찾기
//                    if (RequestStop == false)
//                    {
//                        List<Point> fidPointList = FindFiducial(curAlgoImage, startSearchLine, debugContext);
//                        foreach (Point fidPos in fidPointList)
//                        {
//                            if (fidPos.Y < startSearchLine)
//                                continue;

//                            SetChildImage(curAlgoImage, fidPos.X, fidPos.Y, -1);
//                            if (RequestStop)
//                                break;
//                        }
//                    }
                    
//                    prevAlgoImage = curAlgoImage;

//                    LogHelper.Debug(LoggerType.Grab, "GrabProcesser::ThreadWorkProc Finish.");
//                    startSearchLine = 0;
//                    sw.Stop();
//                    //(SystemManager.Instance().MainForm as UniScanGG.Operation.UI.MainForm).WriteTimeLog("GrabProcesser",(int)currentTag.FrameId, sw.ElapsedMilliseconds);

//                    onImageGrabbed.Reset();
//                    this.isBusy = false;
//                }
//            }
//            catch (ThreadAbortException )
//            {
//            }
//            finally
//            {
//                sw.Stop();
//                frameBoundStitchBuffer?.Dispose();
//                //prevAlgoImage?.Dispose(); // prev == current??
//                isRunning = false;
//            }
//        }

//        public int GetBufferCount()
//        {
//            return this.grabbedImageQueue.Count + (this.isBusy ? 1 : 0);
//        }

//        private void SetFullImageGrabbedFlag()
//        {
//            LogHelper.Info(LoggerType.Grab, "GrabProcesser::SetFullImageGrabbedFlag");
//            Size sheetSize = this.tempSheetImageSet.GetImageSize();
//            SaveDebugData saveDebugData = UniScanGSettings.Instance().SaveImageDebugData;

//            if (sheetSize.Height != this.sheetImageSize.Height) //22857
//            {
//                // grabbedImage.SaveImage(Path.Combine(Configuration.TempFolder, "GrabProcesser", "Grabbed.Png"), System.Drawing.Imaging.ImageFormat.Png);
//                Debug.Assert(false); //zmsong 확인필요..2222
//            }

//            sheetNo++;
//            this.tempSheetImageSet.sheetNo = sheetNo;

//            if ((saveDebugData & SaveDebugData.Text) > 0)
//                AppendLog(tempSheetImageSet);

//            if ((saveDebugData & SaveDebugData.Image) > 0)
//            {
//                Image2D wholdImage = (Image2D)tempSheetImageSet.ToImageD(0.1f);
//                wholdImage.SaveImage(Path.Combine(this.logImagePath, "Sheet", string.Format("SheetNo {0}.bmp", sheetNo)), System.Drawing.Imaging.ImageFormat.Bmp);
//                wholdImage.Dispose();
//            }

//            if (ImageGrabComplete != null)
//            {
//                LogHelper.Debug(LoggerType.Grab, "GrabProcesser::ImageGrabComplete Delegate");
//                this.sheetImageSet.Add(this.tempSheetImageSet);
//                ImageGrabComplete();
//                RequestStop = true;
//                return;
//            }

//            lock (sheetImageSet)
//            {
//                this.sheetImageSet.Add(this.tempSheetImageSet);
//                //this.tempSheetImageSet.Dispose();
//            }

//            this.tempSheetImageSet = null;
//            reamainLines = this.sheetImageSize.Height;
//        }

//        private void AppendLog(SheetImageSet tempSheetImageSet)
//        {
//            Directory.CreateDirectory(debugContext.Path);

//            StringBuilder sb = new StringBuilder();
//            sb.Append(string.Format("{0} ", sheetNo));
//            tempSheetImageSet.subImageList.ForEach(f => sb.Append(string.Format("\t{0}", f.Tag)));
            
//            StreamWriter streamWriter = new StreamWriter(this.logSheetFile, true);
//            streamWriter.WriteLine(sb.ToString());
//            streamWriter.Close();

//            streamWriter.Dispose();
//        }

//        private void AppendLog(ulong frameId, int bufferId, List<Point> fidPointList)
//        {
//            Directory.CreateDirectory(debugContext.Path);

//            StringBuilder sb = new StringBuilder();
//            fidPointList.ForEach(f => sb.AppendLine(string.Format("Frame{0} Buffer{1} Position{2}", frameId, bufferId, f.ToString())));

//            StreamWriter streamWriter = new StreamWriter(this.logFiducialFile, true);
//            streamWriter.Write(sb.ToString());
//            streamWriter?.Close();
//        }

//        private void SetChildImage(AlgoImage algoImage, int fidXPos, int srcLine, int dstLine)
//        {
//            if (dstLine < 0)
//                dstLine = algoImage.Height;

//            if (this.tempSheetImageSet == null)
//            {
//                this.tempSheetImageSet = new SheetImageSet();
//                //this.tempSheetImageSet.name = DateTime.Now.ToString("hh-mm-ss-fff");
//                reamainLines = this.sheetImageSize.Height;
//            }

//            int top = srcLine;
//            int bottom = Math.Min(dstLine, srcLine + reamainLines);
//            Rectangle subRect = Rectangle.FromLTRB(0, top, algoImage.Width, bottom);
//            LogHelper.Debug(LoggerType.Grab, string.Format("GrabProcesser::SetChildImage subRect: {0}", subRect.ToString()));
//            Debug.Assert(Rectangle.Intersect(subRect, new Rectangle(0, 0, algoImage.Width, algoImage.Height)) == subRect);
//            AlgoImage sheetImage = algoImage.GetSubImage(subRect);
//            sheetImage.Tag = algoImage.Tag;

//            this.tempSheetImageSet.subImageList.Add(sheetImage);
//            if (this.tempSheetImageSet.subImageList.Count == 1)
//                this.tempSheetImageSet.fidXPos = fidXPos;

//            // 남은 줄 수 업데이트
//            reamainLines -= sheetImage.Height;
//            //LogHelper.Debug(LoggerType.Grab, string.Format("GrabProcesser::SetChildImage. reamainLines-> {0}", reamainLines));

//            // 다음 Fiducial 찾기 시작점 업데이트
//            startSearchLine = bottom;//+ GrabProcesser.SkipLines;
//            //LogHelper.Debug(LoggerType.Grab, string.Format("GrabProcesser::SetChildImage. startSearchLine-> {0}", startSearchLine));

//            if (reamainLines <= 0)
//            {
//                // 검사 프로세스로 넘김.
//                SetFullImageGrabbedFlag();
//            }
//        }

//        private List<Point> FindFiducial(AlgoImage algoImage, int startSearch,  DebugContext debugContext)
//        {
//            LogHelper.Info(LoggerType.Grab, string.Format("GrabProcesser::FindFiducial Start. startSearch: {0}.", startSearch));
//            List<Point> fidPointList = new List<Point>();

//            Stopwatch sw = new Stopwatch();
//            sw.Start();
            
//            Rectangle clipRect = Rectangle.FromLTRB(0, startSearch, algoImage.Width, algoImage.Height);
//            if (clipRect.Height > 0 && clipRect.Width > 0)
//            {
//                AlgoImage fidFindImage = algoImage.GetSubImage(clipRect);

//                FiducialFinderInspectParam fiducialFinderInspectParam = new FiducialFinderInspectParam(fidFindImage, false,
//                    new AlgorithmInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext));
//                fiducialFinderInspectParam.DebugContext = debugContext;

//                AlgorithmResult algoResult = sheetCheckerFiducial.Inspect(fiducialFinderInspectParam);

//                fidFindImage.Dispose();

//                CameraBufferTag tag = algoImage.Tag as CameraBufferTag;
//                List<AlgorithmResultValue> itemList = algoResult.GetResultValues("FidRect");
//                foreach (AlgorithmResultValue item in itemList)
//                {
//                    Rectangle fidRect = (Rectangle)item.Value;
//                    fidRect.Offset(0, startSearch);
//                    fidPointList.Add(fidRect.Location);

//                    //FiducialFinderParam param = ((FiducialFinderParam)sheetCheckerFiducial.Param);
//                    //if (param.AdaptiveFidSearchRange)
//                    //{
//                    //    Point fidPos = fidPointList.Last();

//                    //    // Fiducial Search Range 보정
//                    //    float newFindLBound = param.FidSearchLBound;
//                    //    float newFindRBound = param.FidSearchRBound;
//                    //    switch (param.FidPosition)
//                    //    {
//                    //        case FiducialPosition.Left:
//                    //            newFindLBound = ((float)fidPos.X / (float)algoImage.Width) - ((float)param.FidSizeWidth / (float)algoImage.Width) - 0.001f;
//                    //            newFindRBound = ((float)fidPos.X / (float)algoImage.Width) + 0.005f;
//                    //            break;
//                    //        case FiducialPosition.Right:
//                    //            newFindLBound = ((float)fidPos.X / (float)algoImage.Width) - 0.005f;
//                    //            newFindRBound = ((float)fidPos.X / (float)algoImage.Width) + ((float)param.FidSizeWidth / (float)algoImage.Width) + 0.001f;
//                    //            break;
//                    //    }

//                    //    if (Math.Abs(param.FidSearchLBound - newFindLBound) < 0.1)
//                    //        param.FidSearchLBound = newFindLBound;
//                    //    if (Math.Abs(param.FidSearchRBound - newFindRBound) < 0.1)
//                    //        param.FidSearchRBound = newFindRBound;
//                    //}
//                }

//                if ((UniScanGSettings.Instance().SaveFiducialDebugData & SaveDebugData.Text) > 0)
//                {
//                    if (tag == null)
//                        AppendLog(ulong.MinValue, -1, fidPointList);
//                    else
//                        AppendLog(tag.FrameId, tag.BufferId, fidPointList);
//                }
//            }
//            LogHelper.Info(LoggerType.Grab, string.Format("GrabProcesser::FindFiducial {0}.", fidPointList.Count));
//            return fidPointList;
//        }
        
//        private bool HasFirstChild()
//        {
//            return (this.tempSheetImageSet != null);
//        }

//        public void Dispose()
//        {
//            Stop();
//            Clear();

//            if (sheetCheckerFiducial!= null)
//                sheetCheckerFiducial.Dispose();
//            sheetCheckerFiducial = null;
//        }
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Windows.Forms;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using DynMvp.Devices;
//using DynMvp.Devices.Light;
//using System.IO;
//using DynMvp.Inspection;
//using DynMvp.InspData;
//using DynMvp.UI.Touch;
//using UniEye.Base.Settings;
//using DynMvp.UI;
//using UniEye.Base.Data;
//using UniEye.Base;
//using System.Drawing.Imaging;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Threading;
//using DynMvp.Authentication;
//using DynMvp.Vision;
//using System.Collections;
//using UniScanG.Algorithms;
//using System.Collections.Concurrent;
//using System.Text;
//using UniEye.Base.UI;
////using UniScanGG.Operation.Data;

//namespace UniScanG.Temp
//{
//    public partial class InspectionPage : UserControl, IInspectionPage, IMainTabPage, IUserHandlerListener
//    {
//        struct InspectionTimeQueueItem
//        {
//            bool isValid;
//            public bool IsValid
//            {
//                get { return isValid; }
//                set { isValid = value; }
//            }


//            DateTime inspectionStartTime;
//            public DateTime InspectionStartTime
//            {
//                get { return inspectionStartTime; }
//                set { inspectionStartTime = value; }
//            }

//            DateTime inspectionEndTime;
//            public DateTime InspectionEndTime
//            {
//                get { return inspectionEndTime; }
//                set { inspectionEndTime = value; }
//            }

//            TimeSpan totalSpan;
//            public TimeSpan TotalSpan
//            {
//                get { return totalSpan; }
//                set { totalSpan = value; }
//            }
//            TimeSpan inspectionSpan;
//            public TimeSpan InspectionSpan
//            {
//                get { return inspectionSpan; }
//                set { inspectionSpan = value; }
//            }
//            TimeSpan exportSpan;
//            public TimeSpan ExportSpan
//            {
//                get { return exportSpan; }
//                set { exportSpan = value; }
//            }

//            public InspectionTimeQueueItem(DateTime inspectStartTime, DateTime inspectEndTime, TimeSpan inspectionSpan, TimeSpan exportSpan)
//            {
//                this.inspectionStartTime = inspectStartTime;
//                this.inspectionEndTime = inspectEndTime;
//                this.totalSpan = inspectEndTime- inspectStartTime;
//                this.inspectionSpan = inspectionSpan;
//                this.exportSpan = exportSpan;
//                isValid = true;
//            }

//            public InspectionTimeQueueItem(DateTime inspectEndTime)
//            {
//                this.inspectionStartTime = this.inspectionEndTime = inspectEndTime;
//                totalSpan = inspectionSpan = exportSpan = TimeSpan.MinValue;
//                isValid = false;
//            }
//        }
        
//        DefectFilterType filterState = DefectFilterType.Total;
//        List<InspectionTimeQueueItem> inspTimeQueue = new List<InspectionTimeQueueItem>();
//        SortedList<int, Data.SimpleInspectionResult> simpleInspectionResultList = new SortedList<int, Data.SimpleInspectionResult>();
//        //List<DataGridViewRow> sheetResultGridViewRowList = new List<DataGridViewRow>();

//        int imageViewResultNum = 10;

//        int highlightCount = 0;

//        bool lockSheetResultGridUpdate = false;

//        InspectionResult lastInspectionResult;
//        object lastInspectionResultLock = new object();
//        DrawBox camResultView;
//        int fontSize = 600;
//        int fontPosition = 500;
//        private object drawingLock = new object();

//        bool onProductInspected = false;

//        private int offlineImageIndex = 0;
//        private string[] offlineImagePathList = null;

//        Data.Production curProduction = null;

//        int lastIndex = -1;

//        private const int padding = 3;

//        ImageViewer imageViewer = new ImageViewer();
//        DefectImageViewer defectImageViewer = new DefectImageViewer();
//        string lastLotNo = "";

//        List<DataGridViewRow> totalGirdViewList = new List<DataGridViewRow>();
//        List<DataGridViewRow> blackGirdViewList = new List<DataGridViewRow>();
//        List<DataGridViewRow> whiteGirdViewList = new List<DataGridViewRow>();
//        List<DataGridViewRow> pinHoleGirdViewList = new List<DataGridViewRow>();
//        List<DataGridViewRow> shapeGirdViewList = new List<DataGridViewRow>();

//        public IInspectionPanel InspectionPanel
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public InspectionPage()
//        {
//            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor Inspection Page");

//            InitializeComponent();

//            labelModelName.Text = StringManager.GetString(this.GetType().FullName,labelModelName.Text);

//            camResultView = new DrawBox();

//            this.camViewPanel.ClientArea.Controls.Add(this.camResultView);
//            this.camResultView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.camResultView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.camResultView.Location = new System.Drawing.Point(3, 3);
//            this.camResultView.Name = "cam1ResultView";
//            this.camResultView.Size = new System.Drawing.Size(409, 523);
//            this.camResultView.TabIndex = 8;
//            this.camResultView.TabStop = false;
//            this.camResultView.Enable = false;
//            //this.camResultView.CoordScaleX = 0.1f;
//            //this.camResultView.CoordScaleY = 0.1f;
//            this.camResultView.MouseMoved += drawBox_MouseMoved;
//            this.camResultView.pictureBox.MouseLeave += PictureBox_MouseLeaveClient;
//            //this.imageViewer.CamView.pictureBox.MouseClick += Image_MouseClick;

//            //camResultView.pictureBox.Dock = DockStyle.Fill;
//            lastDefectView.RowTemplate.Height = lastDefectView.Height / 5;

//            //ExitWaitInspection();

//            //            SystemManager.Instance().InspectRunner.InspectRunnerExtender.LotNo = production.LastLotNo;

//            imageViewer.Hide();
//            //defectImageViewer.Hide();

//            buttonLotChange.Visible = false;

//            UserHandler.Instance().AddListener(this);

//            LogHelper.Debug(LoggerType.StartUp, "End Constructor Inspection Page");
//        }

//        public Production GetCurrentProduction()
//        {
//            return curProduction;
//        }

//        public void UpdatePage()
//        {
//            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdatePage");
//        }
        
//        public void InspectionFinished(object hintObj)
//        {

//        }

//        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new InspectionStepInspectedDelegate(InspectionStepInspected), inspectionStep, sequenceNo, inspectionResult);
//                return;
//            }
//        }

//        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new TargetGroupInspectedDelegate(TargetGroupInspected), targetGroup, inspectionResult, objectInspectionResult);
//                return;
//            }
//        }

//        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new TargetInspectedDelegate(TargetInspected), target, targetInspectionResult);
//                return;
//            }
//        }

//        public void ProductInspected(InspectionResult inspectionResult)
//        {
//            LogHelper.Debug(LoggerType.Inspection, "Product Insepected");

//            lock (lastInspectionResultLock)
//            {
//                lastInspectionResult = inspectionResult;
//            }

//            Data.InspectionResult mpisInspectionResult = (Data.InspectionResult)inspectionResult;
//            mpisInspectionResult.CalcDefectTypeCount();
//            lock (curProduction)
//            {
//                //curProduction.LastSequenceNo = SystemManager.Instance().InspectRunner.InspectRunnerExtender.SequenceCnt;

//                switch (inspectionResult.Judgment)
//                {
//                    case Judgment.Accept:
//                        curProduction.AddGood();
//                        break;
//                    case Judgment.Reject:
//                        curProduction.AddNG();
//                        break;
//                    case Judgment.Skip:
//                        curProduction.AddPass();
//                        break;
//                }

//                curProduction.BlackDefectNum += mpisInspectionResult.BlackDefectNum;
//                curProduction.WhiteDefectNum += mpisInspectionResult.WhiteDefectNum;
//                curProduction.ProductInspected();
//            }
//            ShowProductionInfo();


//            double inspct, avgInspct, avgExport, inspCountOverlap;
//            inspct = avgInspct = avgExport = inspCountOverlap = 0;
//            lock (inspTimeQueue)
//            {
//                if (inspTimeQueue.Count >= 500)
//                    inspTimeQueue.RemoveRange(0, inspTimeQueue.Count - 10 + 1);
//                if(inspectionResult.Judgment == Judgment.Skip)
//                    inspTimeQueue.Add(new InspectionTimeQueueItem(inspectionResult.InspectionEndTime));
//                else
//                    inspTimeQueue.Add(new InspectionTimeQueueItem(inspectionResult.InspectionStartTime, inspectionResult.InspectionEndTime, inspectionResult.InspectionTime, inspectionResult.ExportTime));

//                List<InspectionTimeQueueItem> validItemList = inspTimeQueue.FindAll(f => f.IsValid);
//                if (validItemList.Count > 0)
//                {
//                    inspct = validItemList.Average(f => f.TotalSpan.TotalMilliseconds);
//                    avgInspct = validItemList.Average(f => f.InspectionSpan.TotalMilliseconds);
//                    avgExport = validItemList.Average(f => f.ExportSpan.TotalMilliseconds);
//                }

//                TimeSpan overallInspectTime = inspTimeQueue.Last().InspectionEndTime - inspTimeQueue.First().InspectionStartTime;
//                inspCountOverlap = validItemList.Count / overallInspectTime.TotalMilliseconds * 1000;
//                //inspCountOverlap = overallInspectTime.TotalMilliseconds /inspTimeQueue.Count;
//            }

//            UpdateLabelControl(inspTimeLabel, string.Format("{0:.0} ({1:.0}%)", inspct, (inspct / inspct * 100)));
//            UpdateLabelControl(avgInspTimeLabel, string.Format("{0:.0} ({1:.0}%)", avgInspct, (avgInspct / inspct * 100)));
//            UpdateLabelControl(avgExportTimeLabel, string.Format("{0:.0} ({1:.0}%)", avgExport, (avgExport / inspct * 100)));

//            UpdateLabelControl(inspTimeOverlapLabel, string.Format("{0:0.000} EA/sec", inspCountOverlap));

//            if ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Text) > 0)
//            {
//                TimeSpan totalInspectTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;
//                (SystemManager.Instance().MainForm as UniScanG.Operation.UI.MainForm).WriteTimeLog("SheetInspected", int.Parse(inspectionResult.InspectionNo), (long)Math.Round(totalInspectTime.TotalMilliseconds));
//            }

//            AppendInspectionResultGrid(inspectionResult);

//            //SystemManager.Instance().MachineIf.ProductInspected(inspectionResult);
//            //if (SystemState.Instance().Pause == false)
//            //    ShowResult(SystemManager.Instance().InspectRunner.InspectRunnerExtender.SequenceCnt, inspectionResult, false);


//            inspectionResult.Clear();
//        }

//        private void AppendInspectionResultGrid(InspectionResult inspectionResult)
//        {
         
//            int sheetNo = int.Parse(inspectionResult.InspectionNo);
//            int defCount = inspectionResult.ProbeResultList.Count > 0 ? ((VisionProbeResult)inspectionResult.ProbeResultList[0]).AlgorithmResult.SubResultList.Count : -1;
//            DateTime inspStartTime = inspectionResult.InspectionStartTime;
//            string csvPath = inspectionResult.ResultPath;

//            Data.SimpleInspectionResult simpleInspectionResult = new Data.SimpleInspectionResult(sheetNo, defCount, inspStartTime, null, csvPath);
//            AppendInspectionResultGrid(simpleInspectionResult);
//        }

//        private delegate void AppendInspectionResultGridDelegate(Data.SimpleInspectionResult simpleInspectionResult);
//        private void AppendInspectionResultGrid(Data.SimpleInspectionResult simpleInspectionResult)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new AppendInspectionResultGridDelegate(AppendInspectionResultGrid), simpleInspectionResult);
//                return;
//            }

//            this.simpleInspectionResultList.Add(simpleInspectionResult.SheetNo, simpleInspectionResult);
//            this.sheetResultGrid.RowCount = simpleInspectionResultList.Count;
//        }

//        private delegate void ShowResultDelegate(InspectionResult inspectionResult);
//        private void ShowResult(InspectionResult inspectionResult)
//        {
//            if (inspectionResult.ProbeResultList.Count == 0)
//                return;

//            if (InvokeRequired)
//            {
//                Debug.Assert(inspectionResult.ProbeResultList.Count > 0);
//                LogHelper.Debug(LoggerType.Operation, "Begin Invoke ShowResult");
//                BeginInvoke(new ShowResultDelegate(ShowResult), inspectionResult);
//                return;
//            }

//            //Pen pen = new Pen(Color.Red, 3);
//            camResultView.TempFigureGroup.Clear();
//            UniScanG.Operation.Data.SheetCheckerAlgorithmResult algorithmResult = (UniScanG.Operation.Data.SheetCheckerAlgorithmResult)((VisionProbeResult)(inspectionResult.ProbeResultList[0])).AlgorithmResult;
//            //algorithmResult.SubResultList.ForEach(f => camResultView.TempFigureGroup.AddFigure(new CrossFigure(f.ResultRect.ToRectangle(), pen)));
//            //if (algorithmResult.WholeImage != null)
//            //{
//            //    camResultView.UpdateImage(algorithmResult.WholeImage);
//            //    camResultView.ZoomFit();
//            //    camResultView.Invalidate();
//            //    camResultView.Update();
//            //}


//            LogHelper.Debug(LoggerType.Operation, "InspectionPage - ShowResult");
//        }

//        private void UpdateLastDefectView(InspectionResult inspectionResult)
//        {
//            if (onProductInspected == true)
//                return;

//            onProductInspected = true;

//            if (inspectionResult == null)
//                return;

//            if (inspectionResult.ProbeResultList.Count == 0)
//                return;

//            int prevFirstDisplayedScrollingRowIndex = lastDefectView.FirstDisplayedScrollingRowIndex;

//            List<AlgorithmResult> subResultList = ((VisionProbeResult)(inspectionResult.ProbeResultList[0])).AlgorithmResult.SubResultList;

//            lastDefectView.Rows.Clear();
//            if (subResultList.Count > 0)
//            {
//                foreach (Data.SheetCheckerSubResult subResult in subResultList)
//                {
//                    if (subResult == null)
//                        continue;

//                    StringBuilder sb = new StringBuilder();
//                    string defectType = null;
//                    Color color = new Color();
//                    bool filtered = false;

//                    switch (subResult.DefectType)
//                    {
//                        case Data.SheetDefectType.BlackDefect:
//                            if (filterState != DefectFilterType.Total && filterState != DefectFilterType.Black)
//                                filtered = true;
//                            sb.AppendLine( "전극(Bright)");
//                            color = Color.Red;
//                            break;
//                        case Data.SheetDefectType.WhiteDefect:
//                            if (filterState != DefectFilterType.Total && filterState != DefectFilterType.White)
//                                filtered = true;
//                            sb.AppendLine("성형(Dark)");
//                            color = Color.Yellow;
//                            break;
//                    }

//                    sb.AppendLine(subResult.Message);
//                    defectType = sb.ToString();

//                    if (filtered == true)
//                        continue;

//                    Image image = subResult.Image;
//                    if (image == null)
//                        image = ImageHelper.LoadImage(Path.Combine(inspectionResult.ResultPath, string.Format("{0}.bmp", subResult.Index)));

//                    lastDefectView.Rows.Insert(0, inspectionResult.InspectionNo, defectType, image);

//                    lastDefectView.Rows[0].Tag = subResult;
//                }
//            }

//            lastDefectView.Invalidate();

//            onProductInspected = false;
//        }

//        private void ShowSheetResult()
//        {

//        }

//        private void sheetResultGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            if (e.RowIndex < 0)
//                return;

//            Data.SimpleInspectionResult simpleInspectionResult = this.simpleInspectionResultList.ElementAt(e.RowIndex).Value;

//            Color color= Color.Red;
//            switch (simpleInspectionResult.DefectCount)
//            {
//                case -1:
//                    color = Color.Yellow;
//                    break;
//                case 0:
//                    color = Color.LightGreen;
//                    break;
//            }
//            e.CellStyle.BackColor = color;
//            //e.CellStyle.Font.Bold = (inspectionResult.IsDefected() ? true : false);
//        }

//        private void sheetResultGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
//        {
//            Data.SimpleInspectionResult simpleInspectionResult = this.simpleInspectionResultList.ElementAt(e.RowIndex).Value;
//            switch (e.ColumnIndex)
//            {
//                case 0:
//                    e.Value = simpleInspectionResult.SheetNo;
//                    break;
//                case 1:
//                    e.Value = simpleInspectionResult.InspTime.ToString("HH:mm:ss");
//                    break;
//                case 2:
//                    e.Value = simpleInspectionResult.DefectCount<0?"Skip": simpleInspectionResult.DefectCount.ToString();
//                    break;
//            }

//            //LogHelper.Debug(LoggerType.Operation, "InspectionPage - AppendGridRow Start");

//            //bool isError = true;
//            //if (inspectionResult.ProbeResultList.Count > 0)
//            //{
//            //    Data.SheetCheckerAlgorithmResult algorithmResult = (Data.SheetCheckerAlgorithmResult)((VisionProbeResult)(inspectionResult.ProbeResultList[0])).AlgorithmResult;
//            //    isError = algorithmResult.Error != Data.InspectionError.None;
//            //}
//            //DataGridViewRow newRow = new DataGridViewRow();
//            //newRow.CreateCells(sheetResultGrid, sequenceStr, inspectionResult.InspectionStartTime.ToString("HH:mm:ss"), isError ? "Error" : totalDefectNum.ToString(), true);
//            //newRow.Tag = inspectionResult.ResultPath;
//            //newRow.Cells[0].Style.BackColor = inspectionResult.Judgment == Judgment.Accept ? Color.Green : Color.Red;

//            //lock (sheetResultGridViewRowList)
//            //{
//            //    sheetResultGridViewRowList.Add(newRow);
//            //}
//        }
        
//        private delegate void UpdateProductionInfoDelegate(InspectionResult inspectionResult);
//        private void UpdateProductionInfo(InspectionResult inspectionResult)
//        {
//            //if (InvokeRequired)
//            //{
//            //    LogHelper.Debug(LoggerType.OpDebug, "Begin Invoke UpdateProductionInfo");
//            //    BeginInvoke(new UpdateProductionInfoDelegate(UpdateProductionInfo), inspectionResult);
//            //    return;
//            //}

//            LogHelper.Debug(LoggerType.Operation, "InspectionPage - UpdateProductionInfo");

           
//            //bool isError = algorithmResult.Error != Data.InspectionError.None;

//            //if (isError == true)
//            //{
//            //    if (sheetResultGrid.Rows.Count == 0)
//            //        sheetResultGrid.Rows.Add(sequenceStr, inspectionResult.LastInspectionStartTime.ToString("HH:mm:ss"), "Error", true);
//            //    else
//            //        sheetResultGrid.Rows.Insert(0, sequenceStr, inspectionResult.LastInspectionStartTime.ToString("HH:mm:ss"), "Error", true);
//            //}
//            //else
//            //{
//            //    if (sheetResultGrid.Rows.Count == 0)
//            //        sheetResultGrid.Rows.Add(sequenceStr, inspectionResult.LastInspectionStartTime.ToString("HH:mm:ss"), totalDefectNum, true);
//            //    else
//            //        sheetResultGrid.Rows.Insert(0, sequenceStr, inspectionResult.LastInspectionStartTime.ToString("HH:mm:ss"), totalDefectNum, true);
//            //}

//            ////sheetResultGrid.Rows[0].Tag = sheetResult;
//            //sheetResultGrid.Rows[0].Tag = inspectionResult;
//            //if (inspectionResult.Judgment == Judgment.Accept)
//            //    sheetResultGrid.Rows[0].Cells[0].Style.BackColor = Color.Green;
//            //else
//            //    sheetResultGrid.Rows[0].Cells[0].Style.BackColor = Color.Red;

//            //if (sheetResultGrid.Rows.Count > 200)
//            //{
//            //    lockSheetResultGridUpdate = true;

//            //    sheetResultGrid.Rows.RemoveAt(200);

//            //    lockSheetResultGridUpdate = false;
//            //}

//            ShowProductionInfo();
//        }

//        private delegate void UpdateLabelControlDelegate(Infragistics.Win.Misc.UltraLabel ultraLabel, string v);
//        private void UpdateLabelControl(Infragistics.Win.Misc.UltraLabel ultraLabel, string v)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateLabelControlDelegate(UpdateLabelControl), ultraLabel, v);
//                return;
//            }
//            ultraLabel.Text = v;
//        }

//        private void EnterWaitInspection(bool acyncMode, float cvySpeedMPS)
//        {
//            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);

//            if (SystemState.Instance().GetOpState() == OpState.Idle)
//            {
//                if (SystemManager.Instance().InspectRunner.EnterWaitInspection() == false)
//                    return;

//                DynMvp.Devices.FrameGrabber.Camera camera = (DynMvp.Devices.FrameGrabber.Camera)imageDevice;
//                camera.SetLineScanMode();
//                if (acyncMode)
//                {
//                    camera.SetTriggerMode(TriggerMode.Software);
//                    Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];
//                    float grabHz = cvySpeedMPS / calibration.PelSize.Height * 1000000;  // Line per sec
//                    camera.SetExposureTime(grabHz * 1000000);
//                }
//                else
//                {
//                    camera.SetTriggerMode(TriggerMode.Hardware);
//                }

//                if (curProduction.StartTime.Ticks == 0)
//                    curProduction.StartTime = DateTime.Now;

//                ShowProductionInfo();
//            }

//            // Show Ref /10 Image
//            int stepNo = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//            VisionProbe visionProbe = (VisionProbe)SystemManager.Instance().CurrentModel.InspectionStepList[stepNo].TargetGroupList[0].TargetList[0].ProbeList[0];
//            GravureSheetChecker algorithm = (GravureSheetChecker)visionProbe.InspAlgorithm;
//            SheetCheckerParam param = (SheetCheckerParam)algorithm.Param;
//            //ImageProcessing imgProcessing = AlgorithmBuilder.GetImageProcessing(param.RefferenceBitmap);
//            //AlgoImage resImage = ImageBuilder.Build(param.ReferenceImage.LibraryType, ImageType.Grey, param.ReferenceImage.Width / 10, param.ReferenceImage.Height / 10);
//            //imgProcessing.Resize(param.ReferenceImage, resImage, -1);
//            //camResultView.Image = resImage.ToImageD().ToBitmap();
//            if(param.TrainerParam.RefferenceImage!=null)
//                camResultView.UpdateImage(param.TrainerParam.RefferenceImage.ToBitmap());
//            else
//                camResultView.UpdateImage(param.TrainerParam.InspectRegionInfoImage.ToBitmap());

//            camResultView.ZoomFit();
//            camResultView.Invalidate();
//            camResultView.Update();
//            //resImage.Dispose();

//            //if (curProduction.StartTime.Ticks == 0)
//            //    curProduction.StartTime = DateTime.Now;

//            buttonResetCount.Enabled = false;

//            buttonStop.Enabled = true;
//            buttonLotChange.Enabled = false;

//            buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Pause;
//            buttonStart.Text = "Pause";
//            StateUpdateTimer.Interval = 100;
//            StateUpdateTimer.Start();
//        }

//        delegate void ImageViewrUpdateDelegate(int index);
//        private void ImageViewrUpdate(int index)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new ImageViewrUpdateDelegate(ImageViewrUpdate), index);
//                return;
//            }

//            Data.InspectionResult inspectionResult = (Data.InspectionResult)sheetResultGrid.Rows[index].Tag;

//            FigureGroup tempFigureGroup2 = new FigureGroup();
//            inspectionResult.AppendResultFigures(tempFigureGroup2);

//            /*Bitmap bitmap = inspectionResult.TargetImageList[0].Image.ToBitmap();
//            byte[] bitByte = ImageHelper.GetByte(bitmap);
//            imageViewer.CamView.Image = ImageHelper.CreateBitmap(bitmap.Width, bitmap.Height, bitmap.Width, 1, bitByte);*/
//            imageViewer.CamView.UpdateImage(inspectionResult.TargetImageList[0].Image.ToBitmap());

//            imageViewer.CamView.TempFigureGroup = tempFigureGroup2;

//            imageViewer.CamView.ZoomFit();
//            imageViewer.CamView.Invalidate();
//            imageViewer.CamView.Update();

//            imageViewer.Clear();
//            imageViewer.DefectUpdate(int.Parse((string)sheetResultGrid.Rows[index].Cells[0].Value), inspectionResult);
//        }

//        private void InspectionImageCopy(InspectionResult inspectionResult)
//        {
//            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - InspectionImageCopy");
//            Task task = new Task(new Action(() =>
//            {
//                Bitmap bitmap = inspectionResult.TargetImageList[0].Image.ToBitmap();
//                byte[] bitByte = ImageHelper.GetByte(bitmap);

//                foreach (DataGridViewRow row in sheetResultGrid.Rows)
//                {
//                    if (((InspectionResult)row.Tag).TargetImageList[0].Image.IsUseIntPtr() == true)
//                    {
//                        row.Cells[0].Tag = ImageHelper.CreateBitmap(bitmap.Width, bitmap.Height, bitmap.Width, 1, bitByte);
//                        return;
//                    }
//                }
//            }));

//            task.Start();
//        }

//        private void EnterPauseInspection()
//        {
//            SystemState.Instance().Pause = true;
//            buttonStop.Enabled = true;

//            buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Start;
//            buttonStart.Text = "Start";
//        }

//        private void ExitPauseInspection()
//        {
//            SystemState.Instance().Pause = false;
//            buttonResetCount.Enabled = false;

//            buttonStop.Enabled = true;
//            buttonLotChange.Enabled = false;

//            buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Pause;
//            buttonStart.Text = "Pause";
//        }

//        public void ExitWaitInspection()
//        {
//            buttonStart.Enabled = false;
//            SystemManager.Instance().InspectRunner.ExitWaitInspection();

//            SystemState.Instance().Pause = false;

//            buttonResetCount.Enabled = true;

//            buttonStop.Enabled = false;
//            buttonLotChange.Enabled = true;

//            buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Start;
//            buttonStart.Text = "Start";

//            //((MonitoringClient)(SystemManager.Instance().MachineIf)).SendTeachDone(true,"");

//            buttonStart.Enabled = true;
//        }

//        private void buttonStart_Click(object sender, EventArgs e)
//        {
//            bool asyncMode = UniScanGSettings.Instance().AsyncMode;
//            float cvySpeedMPS = 0;
//            if (asyncMode)
//            {
//                cvySpeedMPS = ((MainForm)SystemManager.Instance().MainForm).QuaryConvSpeed();
//                if (cvySpeedMPS < 0)
//                    return;
//            }
//            ButtonStartClick(false, asyncMode, cvySpeedMPS);
//        }

//        private void ButtonEnale(bool value)
//        {
//            buttonStart.Enabled = value;
//            buttonStop.Enabled = value;
//            buttonResetCount.Enabled = value;
//        }

//        delegate bool ButtonStartClickDelegate(bool remoteCall, bool acyncMode, float convSpeed);
//        public bool ButtonStartClick(bool remoteCall, bool acyncMode, float cvySpeedMPS)
//        {
//            if (InvokeRequired)
//            {
//                return (bool)this.Invoke(new ButtonStartClickDelegate(ButtonStartClick), remoteCall, acyncMode, cvySpeedMPS);
//            }

//            if (SystemState.Instance().GetOpState() == OpState.Idle)
//            {
//                //ErrorManager.Instance().ResetAlarm();
//                //OperationSettings.Instance().DebugMode = false;

//                //if (remoteCall == false)
//                //{
//                //    // 마지막 생산된 Product.
//                //    List < Production > productionList = SystemManager.Instance().CurrentModel.ProductionList;
//                //    Production lastProduction = null;
//                //    if (SystemManager.Instance().CurrentModel.ProductionList.Count > 0)
//                //        lastProduction = SystemManager.Instance().CurrentModel.ProductionList.Last();

//                //    // LOT번호 초기값 입력
//                //    InputForm form = new InputForm("Input Lot No", lastProduction?.LotNo);
//                //    Point point = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (form.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (form.Height / 2));
//                //    form.ChangeLocation(point);
//                //    if (form.ShowDialog(Parent) == DialogResult.Cancel || form.InputText == "")
//                //        return false;

//                //    // LOT번호 중복 검색 - 마지막 검사 + 금일 검사된 Product와 비교
//                //    string newLotNo = form.InputText;
//                //    Production todayProduction = SystemManager.Instance().CurrentModel.ProductionList.Find(f => (f.StartTime.Date == DateTime.Today) && (f.LotNo == newLotNo));
//                //    if ((lastProduction != null && lastProduction.LotNo == newLotNo) || (todayProduction != null))
//                //    {
//                //        DialogResult dialogResult = MessageForm.Show(this.ParentForm, "Lot No is Duplicated. \r\nShall I Continue Inspection with Previous?", MessageFormType.YesNo);
//                //        if (dialogResult == DialogResult.No)
//                //            return false;
//                //    }

//                //    ChangeLot(newLotNo);

//                //    ShowProductionInfo();
//                //    //OperationSettings.Instance().DebugMode = (MessageForm.Show(null, "Would you want to DEBUG mode?", MessageFormType.YesNo) == DialogResult.Yes);
//                //}

//                //EnterWaitInspection(acyncMode, cvySpeedMPS);
//                //if (remoteCall)
//                //{
//                //    // 원격 시작 - 준비완료 신호 보냄.
//                //    // RcWaitInsp 함수에서 보냄
//                //}
//                //else
//                //{
//                //    // 로컬 시작 - 그랩 바로 시작
//                //    ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
//                //    imageDeviceHandler.GrabMulti();
//                //}
//            }
//            else
//            {
//                if (SystemState.Instance().Pause == false)
//                {
//                    EnterPauseInspection();
//                    CheckSheetResultGridIndex();

//                    //sheetResultGrid.SelectAll();
//                }
//                else
//                {
//                    ExitPauseInspection();
//                }
//            }
//            return true;
//        }

//        private void buttonStop_Click(object sender, EventArgs e)
//        {
//            ButtonStopClick(false);
//        }

//        delegate void ButtonStopClickDelegate(bool remoteCall);
//        public void ButtonStopClick(bool remoteCall)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new ButtonStopClickDelegate(ButtonStopClick), remoteCall);
//                return;
//            }

//            if (SystemState.Instance().GetOpState() == OpState.Wait || SystemState.Instance().Pause == true || SystemState.Instance().GetOpState() == OpState.Inspect)
//            {
//                ExitWaitInspection();
//                SimpleProgressForm stopInspectionForm = new SimpleProgressForm("Stop Inspection...");
//                stopInspectionForm.Show(new Action(() =>
//                {
//                    while (SystemManager.Instance().InspectRunner.IsOnInspect())
//                        Thread.Sleep(100);
//                }));

//                if (SystemManager.Instance().CurrentModel != null)
//                    SystemManager.Instance().CurrentModel.SaveProduction();

//                if (remoteCall)
//                {
//                    MonitoringClient monitoringClient = (MonitoringClient)SystemManager.Instance().MachineIf;
//                    //monitoringClient.SendJobDone("EXIT_WAIT", true);
//                }

//                SystemState.Instance().SetIdle();
//            }
//        }

//        //public void DeleteResult()
//        //{
//        //    if (string.IsNullOrEmpty(production.LotNo) == true)
//        //        return;

//        //    string resultMonitorPath = Path.Combine(
//        //                    ((Operation.Data.Production)SystemManager.Instance().CurrentModel.Production).StartTime.ToString("yyyy-MM-dd"),
//        //                    SystemManager.Instance().CurrentModel.Name, production.LotNo);

//        //    string resultPath = Path.Combine(PathSettings.Instance().Result, resultMonitorPath);
//        //    string overViewPath = string.Format("{0}\\OverView.csv", resultPath);

//        //    if (File.Exists(overViewPath) == true)
//        //        File.Delete(overViewPath);
//        //}

//        public void Reset()
//        {
//            ShowProductionInfo();

//            ErrorManager.Instance().ResetAlarm();

//            //DeleteResult();

//            camResultView.UpdateImage( null);
//            camResultView.FigureGroup.Clear();
//            camResultView.TempFigureGroup.Clear();

//            lastDefectView.Rows.Clear();

//            sheetResultGrid.RowCount = 0;
//            this.simpleInspectionResultList.Clear();


//            inspTimeQueue.Clear();
//            inspTimeLabel.Text = "";
//            avgInspTimeLabel.Text = "";
//            avgExportTimeLabel.Text = "";

//            totalGirdViewList.Clear();
//            blackGirdViewList.Clear();
//            whiteGirdViewList.Clear();
//            pinHoleGirdViewList.Clear();
//            shapeGirdViewList.Clear();
//        }

//        private void buttonResetCount_Click(object sender, EventArgs e)
//        {
//            SystemManager.Instance().CurrentModel.SaveProduction();
//            curProduction = null;

//            Reset();
//            ShowProductionInfo();
//        }

//        private void buttonSelectImage_Click(object sender, EventArgs e)
//        {
//            FolderBrowserDialog dialog = new FolderBrowserDialog();
//            if (dialog.ShowDialog() == DialogResult.OK)
//            {
//                string[] extensions = { ".jpg", ".gif", ".png", ".bmp" };
//                string[] fileNames = Directory.GetFiles(dialog.SelectedPath, "Image_*.*").Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();
//                if (fileNames.Count() > 0)
//                {
//                    offlineImagePathList = new string[1] { dialog.SelectedPath };
//                }
//                else
//                {
//                    offlineImagePathList = Directory.GetFiles(dialog.SelectedPath, "*.*");
//                }

//                offlineImageIndex = 0;
//            }
//        }

//        public void HighlightRatio()
//        {
//            Color color1 = Color.Transparent;
//            Color color2 = Color.Red;
//            Color color3 = Color.DarkRed;

//            string[] tokens = productionRatio.Text.Split(' ');
            
//            if (curProduction?.Ng > 10)
//            {
//                if (highlightCount % 2 == 1)
//                    productionRatio.Appearance.BackColor = color2;
//                else
//                    productionRatio.Appearance.BackColor = color3;
//            }
//            else
//            {
//                productionRatio.Appearance.BackColor = color1;
//                highlightCount = 0;
//            }
//        }

//        private void StateUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            StateUpdateTimer.Stop();
//            UpdateState();

//            HighlightRatio();

//            if (lastInspectionResult != null)
//            {
//                lock (lastInspectionResultLock)
//                {
//                    if(lastInspectionResult!=null)
//                    ShowResult(lastInspectionResult);

//                    lastInspectionResult = null;
//                }
//            }
//            StateUpdateTimer.Start();
//        }

//        void UpdateStatusLabel(string text, Color foreColor, Color backColor)
//        {
//            labelStatus.Appearance.BackColor = backColor;
//            labelStatus.Appearance.ForeColor = foreColor;
//            labelStatus.Text = StringManager.GetString(this.GetType().FullName,text);
//        }

//        public void UpdateState()
//        {
//            if (ErrorManager.Instance().IsAlarmed())
//            {
//                UpdateStatusLabel("Alarm", Color.White, Color.Red);
//            }
//            else
//            {
//                if (SystemState.Instance().Pause == true)
//                {
//                    UpdateStatusLabel("Pause", Color.Black, Color.Yellow);
//                    return;
//                }

//                switch (SystemState.Instance().GetOpState())
//                {
//                    case OpState.Inspect:
//                        UpdateStatusLabel("Inspection", Color.Black, Color.LimeGreen);

//                        /*switch (SystemState.Instance().InspectState)
//                        {
//                            case InspectState.Review:
//                            case InspectState.Done:
//                                switch (SystemState.Instance().InspectionResult)
//                                {
//                                    case Judgment.Accept:
//                                        UpdateStatusLabel("Good", Color.Black, Color.LimeGreen);
//                                        break;
//                                    case Judgment.FalseReject:
//                                        UpdateStatusLabel("Overkill", Color.Black, Color.Yellow);
//                                        break;
//                                    case Judgment.Reject:
//                                        UpdateStatusLabel("NG", Color.White, Color.Red);
//                                        break;
//                                }
//                                break;
//                            case InspectState.Run:
//                                UpdateStatusLabel("Inspecting..", Color.Black, Color.CornflowerBlue);
//                                break;
//                        }*/
//                        break;
//                    case OpState.Wait:
//                        UpdateStatusLabel("Wait", Color.Black, Color.CornflowerBlue);
//                        break;
//                    case OpState.Idle:
//                        UpdateStatusLabel("Idle", Color.Black, Color.Gray);
//                        break;
//                }
//            }
//        }

//        public void EnableControls()
//        {
//            ButtonEnale(true);
//        }

//        private delegate void ShowProductionInfoDelegate();
//        private void ShowProductionInfo()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new ShowProductionInfoDelegate(ShowProductionInfo));
//                return;
//            }

//            if (curProduction == null)
//            {
//                lotNo.Text = "";
//                productionNg.Text = "";
//                productionIndex.Text = "";
//                productionRatio.Text = "";
//                blackDefect.Text = "";
//                whiteDefect.Text = "";
//                lastUpdateTime.Text = "";
//            }
//            else
//            {
//                modelName.Text = SystemManager.Instance().CurrentModel.Name;
//                lotNo.Text = curProduction.LotNo;
//                startTime.Text = curProduction.StartTime.ToString("HH : mm : ss . fff");
//                lastUpdateTime.Text = curProduction.LastUpdateTime.ToString("HH : mm : ss . fff");

//                //TimeSpan timeSpan;
//                //if (inspTimeQueue.Count > 0)
//                //    timeSpan = new TimeSpan((long)inspTimeQueue.Average());
//                //else
//                //    timeSpan = TimeSpan.Zero;
//                //avgInspTime.Text = timeSpan.ToString(@"s\.fff");

//                //if (inspTimeQueue.Count > 0)
//                //    timeSpan = new TimeSpan((long)inspTimeQueue.Average());
//                //else
//                //    timeSpan = TimeSpan.Zero;
//                //avgInspTime.Text = timeSpan.ToString(@"s\.fff");

//                //if (exportTimeQueue.Count > 0)
//                //    timeSpan = new TimeSpan((long)exportTimeQueue.Average());
//                //else
//                //    timeSpan = TimeSpan.Zero;
//                //avgExportTime.Text = timeSpan.ToString(@"s\.fff");
                
//                productionNg.Text = curProduction.Ng.ToString();
//                productionIndex.Text = curProduction.Total.ToString();
//                productionRatio.Text = curProduction.NgRatio.ToString("0.0") + " %";
//                productionSkip.Text = curProduction.Pass.ToString();

//                blackDefect.Text = curProduction.BlackDefectNum.ToString();
//                whiteDefect.Text = curProduction.WhiteDefectNum.ToString();

//            }
//            //LogHelper.Debug(LoggerType.OpDebug, "InspectionPage - ShowProductionInfo End");
//        }

//        public void TabPageVisibleChanged(bool visible)
//        {
//            if (visible == true)
//            {
//                if (curProduction != null)
//                {
//                    modelName.Text = SystemManager.Instance().CurrentModel.Name;
                    
//                    ShowProductionInfo();
//                }
//            }
//            else
//            {
//                if (SystemState.Instance().OnInspectOrWaitOrPause)
//                {

//                }
//            }
//        }

//        private void camViewPanel_RegionChanged(object sender, EventArgs e)
//        {
//            camResultView.ZoomFit();
//        }

//        private void InspectionPage_Resize(object sender, EventArgs e)
//        {
//            camResultView.ZoomFit();
//        }

//        delegate void ChangeLotDelegate(string lotNum);
//        public void ChangeLot(string lotNum)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new ChangeLotDelegate(ChangeLot), lotNum);
//                return;
//            }

//            Production lastProduction = null;
//            Production todayProduction = null;
//            if (SystemManager.Instance().CurrentModel.ProductionList.Count > 0)
//            {
//                lastProduction = SystemManager.Instance().CurrentModel.ProductionList.Last();
//                todayProduction = SystemManager.Instance().CurrentModel.ProductionList.Find(f => (f.StartTime.Date == DateTime.Today) && (f.LotNo == lotNum));
//            }

//            Production curProduction = null;
//            if (lastProduction != null && lastProduction.LotNo == lotNum)
//            {
//                curProduction = lastProduction;
//            }
//            else if (todayProduction != null)
//            {
//                curProduction = todayProduction;
//            }
//            else
//            {
//                curProduction = ProductionManager.Instance().CreatProduction();
//                SystemManager.Instance().CurrentModel.ProductionList.Add(curProduction);
//                curProduction.LotNo = lotNum;

//                SystemManager.Instance().CurrentModel.SaveProduction();
//            }

//            if(this.curProduction != curProduction)
//                Reset();

//            this.curProduction = (Data.Production)curProduction;
//            SystemManager.Instance().InspectRunner.InspectRunnerExtender.Production = curProduction;
            
//            ShowProductionInfo();
//        }

//        private void buttonLotChange_Click(object sender, EventArgs e)
//        {
//        //    InputForm form = new InputForm("Input Lot No", curProduction.LotNo);
//        //    Point point = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (form.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (form.Height / 2));
//        //    form.ChangeLocation(point);
//        //    if (form.ShowDialog(Parent) == DialogResult.Cancel || form.InputText == "")
//        //    {
//        //        return;
//        //    }

//        //    ChangeLot(form.InputText);
//        }

//        private void buttonOfflineTest_Click(object sender, EventArgs e)
//        {
//            if (MachineSettings.Instance().VirtualMode == true)
//            {
//                FolderBrowserDialog dialog = new FolderBrowserDialog();
//                dialog.SelectedPath = PathSettings.Instance().Model;

//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    string[] extensions = { ".jpg", ".gif", ".png", ".bmp" };
//                    string[] fileNames = Directory.GetFiles(dialog.SelectedPath, "Image_*.*").Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();
//                    if (fileNames.Count() > 0)
//                    {
//                        offlineImagePathList = new string[1] { dialog.SelectedPath };
//                    }
//                    else
//                    {
//                        offlineImagePathList = Directory.GetFiles(dialog.SelectedPath, "*.*");
//                    }

//                    offlineImageIndex = 0;
//                }
//            }
//        }

//        private void lastDefectView_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            //if (SystemState.Instance().Pause == true)
//            //{
//            //    if (lastDefectView.SelectedRows.Count > 0)
//            //    {
//            //        Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView.Rows[lastDefectView.SelectedRows[0].Index].Tag;
//            //        if (sheetCheckerSubResult != null)
//            //        {
//            //            PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//            //            Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//            //                (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//            //            resultRect.Inflate(200, 200);
//            //            Pen pen = new Pen(Color.Red, 2);
//            //            camResultView.DrawEllipse(resultRect, pen);
//            //        }
//            //    }
//            //}
//        }

//        private void DrawDefectPos(Data.SheetCheckerSubResult sheetCheckerSubResult)
//        {
//            if (sheetCheckerSubResult == null)
//                return;

//            PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//            Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//                (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//            resultRect.Inflate(200, 200);
//            Pen pen = new Pen(Color.Red, 2);
//            camResultView.DrawEllipse(resultRect, pen);
//        }

//        private void zoomInToolStripButton_Click(object sender, EventArgs e)
//        {

//        }

//        private void buttonShowImageViewer_Click(object sender, EventArgs e)
//        {
//            if (imageViewer.Visible == true)
//            {
//                imageViewer.Hide();
//            }
//            else
//            {
//                if (sheetResultGrid.SelectedRows.Count == 0)
//                    return;

//                imageViewer.Show();
//                imageViewer.TopMost = true;
//                SimpleProgressForm loadingForm = new SimpleProgressForm("Image Viewer Loading");
//                loadingForm.Show(new Action(() =>
//                {
//                    ImageViewrUpdate(sheetResultGrid.SelectedRows[0].Index);
//                }));

//            }
//        }

//        private void lastDefectView_Paint(object sender, PaintEventArgs e)
//        {
//            //Brush defectBrush = new SolidBrush(Color.Red);
//            //Font font = new Font("Arial", 12);
//            //StringFormat stringFormat = new StringFormat();
//            //stringFormat.Alignment = StringAlignment.Near;
//            //stringFormat.LineAlignment = StringAlignment.Near;

//            //Brush typeBrush = new SolidBrush(Color.Blue);

//            //for (int i = 0; i < lastDefectView.Rows.Count; i++)
//            //{
//            //    if (lastDefectView.Rows[i].Cells[0].Displayed == true)
//            //    {
//            //        Rectangle cellRect = lastDefectView.GetCellDisplayRectangle(0, i, false);
//            //        Data.SheetCheckerSubResult subResult = (Data.SheetCheckerSubResult)lastDefectView.Rows[i].Tag;

//            //        if (subResult != null)
//            //            e.Graphics.DrawString(subResult.Message, font, defectBrush, new Point(cellRect.Right + padding, cellRect.Top + padding * 5), stringFormat);
//            //        if (string.IsNullOrEmpty(subResult.ShortMessage) == false)
//            //            e.Graphics.DrawString(subResult.ShortMessage, font, typeBrush, new Point(cellRect.Right + padding, cellRect.Bottom - padding * 15), stringFormat);
//            //    }
//            //}
//        }

//        private List<Data.SheetCheckerSubResult> GetSubResultList(Data.InspectionResult inspectionResult)
//        {
//            List<Data.SheetCheckerSubResult> resultList = new List<Data.SheetCheckerSubResult>(); ;

//            if (inspectionResult.ProbeResultList == null)
//                return resultList;

//            if (inspectionResult.ProbeResultList.Count == 0)
//                return resultList;

//            VisionProbeResult visionResult = (VisionProbeResult)inspectionResult.ProbeResultList[0];

//            if (visionResult.AlgorithmResult == null)
//                return resultList;

//            Data.SheetCheckerAlgorithmResult algorithmResult = (Data.SheetCheckerAlgorithmResult)visionResult.AlgorithmResult;

//            foreach (Data.SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//            {
//                resultList.Add(subResult);
//            }

//            return resultList;
//        }

//        void CheckSheetResultGridIndex()
//        {
//            if (SystemState.Instance().Pause != true)
//            {
//                buttonShowImageViewer.Enabled = false;
//                return;
//            }

//            if (sheetResultGrid.SelectedRows.Count == 0)
//                return;

//            int index = sheetResultGrid.SelectedRows[sheetResultGrid.SelectedRows.Count - 1].Index;
//            if (index < imageViewResultNum)
//                buttonShowImageViewer.Enabled = true;
//            else
//                buttonShowImageViewer.Enabled = false;
//        }

//        private void drawBox_MouseMoved(DrawBox senderView, Point movedPos, Image image, MouseEventArgs e, ref bool processingCancelled)
//        {

//            return;
//            if (lastDefectView.Rows.Count == 0)
//                return;

//            const float validBound = 500;
//            float minDistance = float.MaxValue;
//            int minIndex = 0;
//            Data.SheetCheckerSubResult minDistanceSheetCheckerSubResult = null;

//            foreach (DataGridViewRow dataGridViewRow in lastDefectView.Rows)
//            {
//                if (SystemState.Instance().Pause == false)
//                    //if ((int)dataGridViewRow.Cells[0].Value != lastIndex)
//                    break;

//                Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)(dataGridViewRow.Tag);
//                PointF centerPt = new PointF(sheetCheckerSubResult.DefectBlob.CenterPt.X, sheetCheckerSubResult.DefectBlob.CenterPt.Y);

//                float lenght = MathHelper.GetLength(centerPt, movedPos);
//                if (lenght < minDistance)
//                {
//                    minDistance = lenght;
//                    minIndex = dataGridViewRow.Index;
//                    minDistanceSheetCheckerSubResult = sheetCheckerSubResult;
//                }
//            }

//            if (minDistance < validBound)
//            {
//                defectImageViewer.UpdateDefectInfo(minDistanceSheetCheckerSubResult.Image, (int)minDistanceSheetCheckerSubResult.X,
//                        (int)minDistanceSheetCheckerSubResult.Y, (int)minDistanceSheetCheckerSubResult.Width,
//                        (int)minDistanceSheetCheckerSubResult.Height);

//                Point location = PointToScreen(new Point(e.X + camViewPanel.Location.X + inspectionPanel.Location.X + 10,
//                    e.Y + camViewPanel.Location.Y + inspectionPanel.Location.Y + 10));

//                defectImageViewer.Location = location;
//                defectImageViewer.Show();
//            }
//            else
//            {
//                defectImageViewer.Hide();
//            }
//        }

//        private void PictureBox_MouseLeaveClient(object sender, EventArgs e)
//        {
//            defectImageViewer.Hide();
//        }

//        delegate void RemoteResetDelegate();
//        public void RemoteReset()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new RemoteResetDelegate(RemoteReset));
//                return;
//            }

//            SystemManager.Instance().CurrentModel.SaveProduction();
//            Reset();
//            ShowProductionInfo();
//        }

//        private void lastDefectView_SelectionChanged(object sender, EventArgs e)
//        {
//            if (SystemState.Instance().OnInspectOrWait)
//                return;

//            if (lastDefectView.SelectedRows.Count > 0)
//            {
//                Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView.Rows[lastDefectView.SelectedRows[0].Index].Tag;
//                if (sheetCheckerSubResult != null)
//                {
//                    PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//                    Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//                        (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//                    resultRect.Inflate(200, 200);

//                    resultRect.X /= 10;
//                    resultRect.Y /= 10;
//                    resultRect.Width /= 10;
//                    resultRect.Height /= 10;
//                    Pen pen = new Pen(Color.Red, 2);
//                    camResultView.DrawEllipse(resultRect, pen);
//                }
//            }
//        }

//        private void buttonShowResultFolor_Click(object sender, EventArgs e)
//        {
//            if (sheetResultGrid.SelectedRows.Count != 1)
//                return;

//            InspectionResult inspectionResult = (InspectionResult)sheetResultGrid.SelectedRows[0].Tag;

//            System.Diagnostics.Process.Start(inspectionResult.ResultPath);
//        }

//        private void selectAllButton_Click(object sender, EventArgs e)
//        {
//            sheetResultGrid.SelectAll();
//        }

//        private void FilterStateChanged()
//        {

//        }

//        private void radioTotal2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                filterState = DefectFilterType.Total;

//                lastDefectView.Rows.Clear();

//                for (int i = totalGirdViewList.Count - 1; i >= 0; i--)
//                    lastDefectView.Rows.Add(totalGirdViewList[i]);

//                lastDefectView.Invalidate();
//            }
//        }

//        private void radioBlack2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                filterState = DefectFilterType.Black;

//                lastDefectView.Rows.Clear();
//                for (int i = blackGirdViewList.Count - 1; i >= 0; i--)
//                    lastDefectView.Rows.Add(blackGirdViewList[i]);

//                lastDefectView.Invalidate();
//            }
//        }

//        private void radioWhite2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                filterState = DefectFilterType.White;

//                lastDefectView.Rows.Clear();
//                for (int i = whiteGirdViewList.Count - 1; i >= 0; i--)
//                    lastDefectView.Rows.Add(whiteGirdViewList[i]);

//                lastDefectView.Invalidate();
//            }
//        }

//        private void radioPinHole2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                filterState = DefectFilterType.PinHole;

//                lastDefectView.Rows.Clear();
//                for (int i = pinHoleGirdViewList.Count - 1; i >= 0; i--)
//                    lastDefectView.Rows.Add(pinHoleGirdViewList[i]);

//                lastDefectView.Invalidate();
//            }
//        }

//        private void radioShape2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                filterState = DefectFilterType.Shape;

//                lastDefectView.Rows.Clear();
//                for (int i = shapeGirdViewList.Count - 1; i >= 0; i--)
//                    lastDefectView.Rows.Add(shapeGirdViewList[i]);

//                lastDefectView.Invalidate();
//            }
//        }

//        delegate void UserChangedDelegatge();
//        public void UserChanged()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UserChangedDelegatge(UserChanged));
//                return;
//            }

//            User curUser = UserHandler.Instance().CurrentUser;

//            groupImageSave.Visible = curUser.SuperAccount;
//        }

//        private void sheetResultGrid_SelectionChanged(object sender, EventArgs e)
//        {
//            if (SystemState.Instance().Pause == true)
//                return;

//            if (SystemManager.Instance().InspectRunner.IsOnInspect() == true)
//                return;

//            if (sheetResultGrid.SelectedRows.Count == 0)
//                return;

//            KeyValuePair<int, Data.SimpleInspectionResult> simpleInspectionResult = this.simpleInspectionResultList.ElementAt(sheetResultGrid.SelectedRows[0].Index);
//            string resultPath = simpleInspectionResult.Value.ResultCsv;
//            if (string.IsNullOrEmpty(resultPath))
//            {
//                MessageForm.Show(null, "ResultPath is null or empty");
//                return;
//            }

//            Data.MpisInspResultArchiver inspResultArchiver = new Data.MpisInspResultArchiver();
//            Data.InspectionResult inspectionResult = (Data.InspectionResult)inspResultArchiver.LoadInspResult(resultPath);

//            if (inspectionResult == null)
//                return;
//            //Data.InspectionResult inspectionResult = (Data.InspectionResult)sheetResultGrid.SelectedRows[0].Tag;
//            if (inspectionResult.ProbeResultList.Count == 0)
//                return;

//            Data.SheetCheckerAlgorithmResult algorithmResult = (Data.SheetCheckerAlgorithmResult)((VisionProbeResult)(inspectionResult.ProbeResultList[0])).AlgorithmResult;

//            //camResultView.Image = ImageHelper.CloneImage(algorithmResult.WholeImage);
//            camResultView.ShowCenterGuide = OperationSettings.Instance().ShowCenterGuide;
//            camResultView.ZoomFit();

//            Font font = new Font("Arial", 400, FontStyle.Bold);
//            string resultText = "OK";
//            Color resultColor = UniScanGUtil.Instance().Good;

//            switch (algorithmResult.Error)
//            {
//                case Data.InspectionError.LightOff:
//                    //resultText = "Light Off";
//                    resultText = "조명 상태를 확인해 주세요.";
//                    resultColor = Color.White;
//                    //if (ErrorManager.Instance().IsAlarmed() == false)
//                    //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "조명 상태를 확인해 주세요."));
//                    break;
//                case Data.InspectionError.DifferenceModel:
//                    //resultText = "Difference Model";
//                    resultText = "모델 정보를 확인해주세요.";
//                    resultColor = Color.White;
//                    //if (ErrorManager.Instance().IsAlarmed() == false)
//                    //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "모델 정보를 확인해주세요."));
//                    break;
//                case Data.InspectionError.PatternRegionInvalidParam:
//                    //resultText = "Invalid Param";
//                    resultText = "전극부(Black) 파라미터를 확인해주세요.";
//                    resultColor = Color.White;
//                    //if (ErrorManager.Instance().IsAlarmed() == false)
//                    //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "전극부(Black) 파라미터를 확인해주세요."));
//                    break;
//                case Data.InspectionError.WhiteRegionInvalidParam:
//                    //resultText = "Invalid Param";
//                    resultText = "성형부 파라미터를 확인해주세요.";
//                    resultColor = Color.White;
//                    //if (ErrorManager.Instance().IsAlarmed() == false)
//                    //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "성형부 파라미터를 확인해주세요."));
//                    break;
//                case Data.InspectionError.InfiniteDefect:
//                    //resultText = error.ToString();
//                    resultText = "불량 갯수가 너무 많습니다.(1000개 이상)";
//                    resultColor = Color.White;
//                    //if (ErrorManager.Instance().IsAlarmed() == false)
//                    //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "불량 갯수가 너무 많습니다.(1000개 이상)"));
//                    break;
//                case Data.InspectionError.BlankSheet:
//                    resultText = "Blank Sheet";
//                    resultColor = Color.Yellow;
//                    ErrorManager.Instance().ResetAlarm();
//                    break;
//                default:
//                    if (inspectionResult.Judgment == Judgment.Reject)
//                    {
//                        resultText = "NG";
//                        resultColor = UniScanGUtil.Instance().NG;
//                    }

//                    ErrorManager.Instance().ResetAlarm();
//                    break;
//            }

//            FigureGroup tempFigureGroup = new FigureGroup();
//            inspectionResult.AppendResultFigures(tempFigureGroup, FigureDrawOption.Default);
//            tempFigureGroup.Scale(0.1f, .1f);
//            TextFigure textFigure = null;

//            textFigure = new TextFigure(resultText, new Point(0, 0), font, resultColor);
//            textFigure.Alignment = StringAlignment.Far;
//            tempFigureGroup.AddFigure(textFigure);

//            camResultView.FigureGroup.Clear();
//            camResultView.TempFigureGroup = tempFigureGroup;

//            UpdateLastDefectView(inspectionResult);

//            camResultView.ZoomFit();
//            camResultView.Invalidate();
//            camResultView.Update();

//            inspectionResult.Clear();
//        }

//        private void useSaveImage_CheckedChanged(object sender, EventArgs e)
//        {
//            SystemManager.Instance().InspectRunner.SaveImageContext.Interval = (int)saveImageInterval.Value;
//            SystemManager.Instance().InspectRunner.SaveImageContext.Use = useSaveImage.Checked;
//        }

//        private void saveImageInterval_ValueChanged(object sender, EventArgs e)
//        {
//            SystemManager.Instance().InspectRunner.SaveImageContext.Interval = (int)saveImageInterval.Value;
//        }

//        private void buttonSavePath_Click(object sender, EventArgs e)
//        {
//            saveImageFolderBrowserDialog.SelectedPath = SystemManager.Instance().InspectRunner.SaveImageContext.Path;
//            if (saveImageFolderBrowserDialog.ShowDialog(this) != DialogResult.OK)
//                return;
//        }

//        private void InspectionPage_Load(object sender, EventArgs e)
//        {
//            saveImageInterval.Value = SystemManager.Instance().InspectRunner.SaveImageContext.Interval;
//            useSaveImage.Checked = SystemManager.Instance().InspectRunner.SaveImageContext.Use;
//        }

//        private void productionPanel_Paint(object sender, PaintEventArgs e)
//        {

//        }

//    }
//}

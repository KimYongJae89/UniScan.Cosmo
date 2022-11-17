//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using System.IO;
//using DynMvp.InspData;
//using DynMvp.UI.Touch;
//using UniEye.Base.Settings;
//using DynMvp.UI;
//using UniEye.Base.Data;
//using Infragistics.Win.Misc;
//using System.Threading.Tasks;
//using System.Diagnostics;

//namespace UniScanG.Temp
//{
//    using UniEye.Base.UI;
//    using SimpleResultList = SortedList<int, Data.SimpleInspectionResult>;

//    public partial class MonitoringPage : UserControl, IMainTabPage, IMonitoringPage
//    {
//        private MonitoringPageBase monitoringPageBase = new MonitoringPageBase();

//        DrawBox[] camResultView;
//        DrawBox[] defectView;

//        int highlightCount = 0;

//        private const int padding = 3;

//        private object drawingLock = new object();
//        private object lockObj = new object();

//        List<Task> inspectDoneTaskList = new List<Task>();

//        public MonitoringPage()
//        {
//            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor Monitoring Page");

//            InitializeComponent();

//            DrawBoxOption drawBoxOption = new DrawBoxOption();
//            drawBoxOption.LockDoubleClick = true;
//            camResultView = new DrawBox[2];
//            defectView = new DrawBox[2];
//            for (int ilil1i1li = 0; ilil1i1li < 2; ilil1i1li++)
//            {
//                this.camResultView[ilil1i1li] = new DrawBox(drawBoxOption);

//                this.camResultView[ilil1i1li].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//                this.camResultView[ilil1i1li].Dock = System.Windows.Forms.DockStyle.Fill;
//                this.camResultView[ilil1i1li].AutoFit(true);

//                this.defectView[ilil1i1li] = new DrawBox(drawBoxOption);
//                this.defectView[ilil1i1li].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//                this.defectView[ilil1i1li].Dock = System.Windows.Forms.DockStyle.Fill;
//                this.defectView[ilil1i1li].AutoFit(true);
//            }
//            this.panelCam1.Controls.Add(this.camResultView[0]);
//            this.panelCam2.Controls.Add(this.camResultView[1]);
//            this.defectViewPanelCam1.Controls.Add(this.defectView[0]);
//            this.defectViewPanelCam2.Controls.Add(this.defectView[1]);


//            LogHelper.Debug(LoggerType.StartUp, "End Constructor Monitoring Page");

//            this.lastDefectView1.RowTemplate.Height = lastDefectView1.Height / 3;
//            this.lastDefectView2.RowTemplate.Height = lastDefectView2.Height / 3;

//            UpdateClientState(ClientState.Disconnected, cam1AState);
//            UpdateClientState(ClientState.Disconnected, cam2AState);
//            UpdateClientState(ClientState.Disconnected, cam1BState);
//            UpdateClientState(ClientState.Disconnected, cam2BState);

//            buttonLotChange.Visible = false;
//        }

//        public void UpdateClientState(Client client)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateClientStateDelegate(UpdateClientState), client);
//                return;
//            }

//            if (client.CamIndex == 0 && client.ClientIndex == 0)
//                UpdateClientState(client.State, cam1AState);
//            else if (client.CamIndex == 0 && client.ClientIndex == 1)
//                UpdateClientState(client.State, cam1BState);
//            else if (client.CamIndex == 1 && client.ClientIndex == 0)
//                UpdateClientState(client.State, cam2AState);
//            else if (client.CamIndex == 1 && client.ClientIndex == 1)
//                UpdateClientState(client.State, cam2BState);
//        }

//        private void UpdateClientState(ClientState clientState, UltraLabel camState)
//        {
//            if (ErrorManager.Instance().IsAlarmed())
//            {
//                camState.Text = "Alarm";
//                //, Color.White, Color.Red);
//                camState.Appearance.BackColor = Color.Crimson;
//                camState.Appearance.ForeColor = Color.White;
//            }
//            else
//            {
//                camState.Text = clientState.ToString();
//                camState.Appearance.ForeColor = Color.Black;

//                if (clientState == ClientState.Disconnected)
//                {
//                    camState.Appearance.BackColor = UniScanGUtil.Instance().DisConnected;
//                }
//                else if (clientState == ClientState.Connected)
//                {
//                    camState.Appearance.BackColor = UniScanGUtil.Instance().Connected;
//                }
//                else
//                {
//                    switch (clientState)
//                    {
//                        default:
//                        case ClientState.Idle:
//                            camState.Appearance.BackColor = UniScanGUtil.Instance().Connected;
//                            break;
//                        case ClientState.Inspect:
//                            camState.Appearance.BackColor = UniScanGUtil.Instance().Inspect;
//                            break;
//                        case ClientState.Pause:
//                            camState.Appearance.BackColor = UniScanGUtil.Instance().Pause;
//                            break;
//                        case ClientState.Wait:
//                            camState.Appearance.BackColor = UniScanGUtil.Instance().Wait;
//                            break;
//                    }
//                }
//            }
//        }

//        //private void UpdateProc()
//        //{
//        //    while (true)
//        //    {
//        //        if (resultPaths.Count > 0)
//        //        {
//        //            UpdateData(resultPaths[0]);
//        //            lock (resultPaths)
//        //            {
//        //                resultPaths.RemoveAt(0);
//        //            }
//        //        }
//        //        Thread.Sleep(100);
//        //    }
//        //}

//        private void SetLabelStatusColor(ref Label label, string state)
//        {
//            Color color = Color.Black;
//            switch (state)
//            {
//                case "IDLE":
//                    label.BackColor = Color.Black;
//                    break;
//                case "Inspect":
//                    label.BackColor = Color.Orange;
//                    break;
//                case "Finished":
//                    label.BackColor = Color.CornflowerBlue;
//                    break;
//            }
//            label.Text = state;
//        }

//        public void InspectionDone(Client client, string resultPath)
//        {
//            InspectorInfo inspectorInfo = UniScanGSettings.Instance().ClientInfoList.Find(f => f.CamIndex == client.CamIndex && f.ClientIndex == client.ClientIndex);
//            if (inspectorInfo == null)
//                return;

//            Task task = Task.Factory.StartNew((Action)(() =>
//            {
//                LogHelper.Debug(LoggerType.Inspection, string.Format("MornitoringPage::InspectionDone::Task Start"));
//                InspectionResult inspectionResult = this.monitoringPageBase.LoadRemoteInspectionResult(inspectorInfo, resultPath);
//                if (inspectionResult == null)
//                    return;

//                this.monitoringPageBase.AddProductionInfo(inspectorInfo.CamIndex, inspectionResult);
//                UpdateProductionInfo(inspectorInfo.CamIndex);
//                UpdateSheetResultGrid(inspectorInfo.CamIndex);

//                FigureGroup fgAcc = new FigureGroup();
//                inspectionResult.AppendResultFigures(fgAcc, FigureDrawOption.Default);
//                fgAcc.Scale(.1f, .1f);
//                //this.monitoringPageBase.GetResultFigure(inspectionResult, null, fgAcc);
//                UpdateViewFigure(inspectorInfo.CamIndex, null, fgAcc);
//                //inspectionResult.Dispose();

//                inspectionResult.Clear();
//                LogHelper.Debug((LoggerType)LoggerType.Operation, string.Format("MornitoringPage::InspectionDone::Task End"));
//            }));
//        }

//        private delegate void UpdateViewFigureDelegate(int camIndex, FigureGroup fgCam, FigureGroup fgAcc);
//        private void UpdateViewFigure(int camIndex, FigureGroup fgCam, FigureGroup fgAcc)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateViewFigureDelegate(UpdateViewFigure), camIndex, fgCam, fgAcc);
//                return;
//            }

//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            LogHelper.Debug(LoggerType.Operation, string.Format("MornitoringPage::UpdateViewFigure Start"));
//            if (fgCam != null)
//            {
//                camResultView[camIndex].FigureGroup.Clear();
//                camResultView[camIndex].FigureGroup.AddFigure(fgCam);
//                camResultView[camIndex].Invalidate();
//            }

//            if (fgAcc != null)
//            {
//                defectView[camIndex].FigureGroup.AddFigure(fgAcc);
//                if (defectView[camIndex].FigureGroup.FigureList.Count > 10)
//                    defectView[camIndex].FigureGroup.FigureList.RemoveAt(0);
//                defectView[camIndex].Invalidate();
//            }
//            sw.Stop();
//            LogHelper.Debug(LoggerType.Operation, string.Format("MornitoringPage::UpdateViewFigure End - {0} ms", sw.ElapsedMilliseconds));
//        }

//        private void UpdateSheetResultGrid(int camIndex)
//        {
//            DataGridView dataGridView = sheetResultGrid1;
//            if (camIndex == 1)
//                dataGridView = sheetResultGrid2;

//            // 마지막 아이템이 선택되어 있으면 자동으로 스크롤
//            bool autoScroll = false;
//            if (dataGridView.Rows.Count == 0)
//            {
//                autoScroll = true;
//            }
//            else if (dataGridView.SelectedRows.Count == 1)
//            {
//                if (dataGridView.SelectedRows[0].Index == dataGridView.Rows.Count - 1)
//                    autoScroll = true;
//            }

//            //lock (resultPaths[camIndex])
//            //    resultPaths[camIndex].Sort((f, g) => int.Parse(f.InspectionNo).CompareTo(int.Parse(g.InspectionNo)));
//            SimpleResultList simpleResultList = this.monitoringPageBase.GetSimpleResultList(camIndex);
//            UpdateSheetResultGrid(dataGridView, simpleResultList, autoScroll);
//        }

//        private delegate void UpdateSheetResultGridDelegate(DataGridView dataGridView, SortedList<int, Data.SimpleInspectionResult> dic, bool autoScroll);
//        private void UpdateSheetResultGrid(DataGridView dataGridView, SortedList<int, Data.SimpleInspectionResult> dic, bool autoScroll)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateSheetResultGridDelegate(UpdateSheetResultGrid), dataGridView, dic, autoScroll);
//                return;
//            }

//            dataGridView.RowCount = dic.Count;

//            if (autoScroll)
//            {
//                dataGridView.Rows[dataGridView.Rows.Count - 1].Selected = true;
//                dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.Rows.Count - 1;
//            }
//        }

//        private void SetDefectValue(int index, int sheet, int pin, int shape, int etc)
//        {
//        }

//        public void InspectionFinished(object hintObj)
//        {

//        }

//        private void ShowCount()
//        {

//        }

//        private void ShowResult(InspectionResult inspectionResult)
//        {
//            LogHelper.Debug(LoggerType.Inspection, "MonitoringPage - ShowImageResult");

//            Bitmap preBitmap = (Bitmap)camResultView[1].Image;

//            camResultView[0].UpdateImage(inspectionResult.GrabImageList[0].ToBitmap());

//            if (preBitmap != null)
//                preBitmap.Dispose();

//            FigureGroup tempFigureGroup = new FigureGroup();
//            inspectionResult.AppendResultFigures(tempFigureGroup);

//            camResultView[0].ShowCenterGuide = OperationSettings.Instance().ShowCenterGuide;
//            camResultView[0].TempFigureGroup = tempFigureGroup;

//            camResultView[0].Invalidate();
//            camResultView[0].Update();

//            if (OperationSettings.Instance().SaveResultFigure)
//            {
//                string fileName = String.Format("{0}\\Image_C{1:00}_S{2:000}_L{3:00}R.jpeg", inspectionResult.ResultPath, 0, 0, 0);
//                camResultView[0].SaveImage(fileName);
//            }
//        }

//        private void buttonStart_Click(object sender, EventArgs e)
//        {
//            //MpisMonitorSystemManager systemManager = (MpisMonitorSystemManager)SystemManager.Instance();

//            //if (SystemState.Instance().GetOpState() == OpState.Idle/* || SystemState.Instance().Pause == true*/)
//            //{
//            //    string lotNo = this.monitoringPageBase.CheckLotNo();
//            //    if (string.IsNullOrEmpty(lotNo))
//            //        return;

//            //    this.monitoringPageBase.ChangeLot(lotNo);
//            //    UpdateProductionInfo();
//            //    StartInspect();
//            //}
//        }

//        public void StartInspect()
//        {
//            UpdateCamViewImage();
//            bool ok = this.monitoringPageBase.StartInspect();
//            if (ok)
//            {
//                Production curProduction = this.monitoringPageBase.CurProduction;
//                lotNo.Text = curProduction.LotNo;
//                startTime.Text = curProduction.StartTime.ToString("yyyy-MM-dd  HH:mm:ss");

//                buttonResetCount.Enabled = false;

//                buttonStop.Enabled = true;
//                buttonLotChange.Enabled = false;

//                buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Pause;
//                buttonStart.Text = "Pause";
//            }
//        }

//        private delegate void UpdateCamViewImageDelegate();
//        private void UpdateCamViewImage()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateCamViewImageDelegate(UpdateCamViewImage));
//                return;
//            }

//            Model curModel = SystemManager.Instance().CurrentModel;
//            foreach (InspectionStep insepctionStep in curModel.InspectionStepList)
//            {
//                VisionProbe visionProbe = insepctionStep.GetProbe(0, 1, 1) as VisionProbe;
//                SheetCheckerParam param = visionProbe.InspAlgorithm.Param as SheetCheckerParam;
//                if (param.TrainerParam.Traind)
//                {
//                    Bitmap image = param.TrainerParam.InspectRegionInfoImage.ToBitmap();
//                    camResultView[insepctionStep.StepNo].UpdateImage(image);
//                    defectView[insepctionStep.StepNo].UpdateImage(image);
//                }
//            }

//            //for (int camIndex = 0; camIndex < 2; camIndex++)
//            //{
//            //    InspectorInfo targetInfo = null;
//            //    foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
//            //    {
//            //        if (info.CamIndex == camIndex && info.ClientIndex == 0)
//            //        {
//            //            targetInfo = info;
//            //            break;
//            //        }
//            //    }

//            //    if (targetInfo == null)
//            //        return;

//            //    string modelName = SystemManager.Instance().CurrentModel.Name;
//            //    string path = Path.Combine(targetInfo.Path, "Model", modelName, "Image", "GrabImage.jpg");
//            //    if (path != null)
//            //    {
//            //        try
//            //        {
//            //            FileStream fs = new FileStream(path, FileMode.Open);
//            //            Bitmap image = new Bitmap(fs);
//            //            fs.Close();
//            //            //Bitmap image = new Bitmap(path);

//            //            camResultView[targetInfo.CamIndex].Image = image;
//            //            defectView[targetInfo.CamIndex].Image = image;
//            //        }
//            //        catch (UnauthorizedAccessException ex)
//            //        {

//            //        }
//            //        catch(Exception ex)
//            //        {

//            //        }
//            //    }
//            //}
//        }

//        public void StopInspection()
//        {
//            this.monitoringPageBase.StopInspection();

//            buttonResetCount.Enabled = true;

//            buttonStop.Enabled = false;
//            buttonLotChange.Enabled = true;

//            buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Start;
//            buttonStart.Text = "Start";
//        }

//        private void buttonStop_Click(object sender, EventArgs e)
//        {
//            if (SystemState.Instance().GetOpState() != OpState.Idle)
//            {
//                Production curProduction = this.monitoringPageBase.CurProduction;
//                string message = String.Format("Screen Model - {0}, Lot No - {1} 검사를 종료하시겠습니까?", SystemManager.Instance().CurrentModel.Name, curProduction.LotNo);
//                if (MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo) == DialogResult.No)
//                {
//                    return;
//                }

//                StopInspection();
//            }
//        }

//        delegate void UpdateInspectionNoDelegate(string serialNo);
//        public void UpdateInspectionNo(string serialNo)
//        {
//            if (InvokeRequired)
//            {
//                LogHelper.Debug(LoggerType.Inspection, "UpdateInspectionNo -> Delegated");

//                BeginInvoke(new UpdateInspectionNoDelegate(UpdateInspectionNo), serialNo);
//                return;
//            }

//            lock (startTime)
//            {
//                startTime.Text = serialNo;
//            }
//        }

//        private void UpdateProductionInfo()
//        {
//            for (int i = 0; i < 2; i++)
//                UpdateProductionInfo(i);
//        }

//        private void UpdateProductionInfo(int camIndex)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateProductionInfoDelegate(UpdateProductionInfo), camIndex);
//                return;
//            }

//            Production curProduction = this.monitoringPageBase.CurProduction;
//            Data.Production subProduction = (Data.Production)curProduction?.GetSubProduction(camIndex);
//            if (camIndex == 0)
//            {
//                if (subProduction != null)
//                {
//                    productionNg1.Text = subProduction.Ng.ToString();
//                    productionIndex1.Text = subProduction.Total.ToString();
//                    productionRatio1.Text = subProduction.NgRatio.ToString("0.0") + " %";

//                    numBlack1.Text = subProduction.BlackDefectNum.ToString();
//                    numWhite1.Text = subProduction.WhiteDefectNum.ToString();
//                }
//                else
//                {
//                    productionNg1.Text = productionIndex1.Text = numBlack1.Text = numWhite1.Text = "0";
//                    productionRatio1.Text = "- %";
//                }
//            }
//            else
//            {
//                if (subProduction != null)
//                {
//                    productionNg2.Text = subProduction.Ng.ToString();
//                    productionIndex2.Text = subProduction.Total.ToString();
//                    productionRatio2.Text = subProduction.NgRatio.ToString("0.0") + " %";

//                    numBlack2.Text = subProduction.BlackDefectNum.ToString();
//                    numWhite2.Text = subProduction.WhiteDefectNum.ToString();
//                }
//                else
//                {
//                    productionNg2.Text = productionIndex2.Text = numBlack2.Text = numWhite2.Text = "0";
//                    productionRatio2.Text = "- %";
//                }
//            }
//        }

//        private void Clear()
//        {
//            for (int i = 0; i < 2; i++)
//            {
//                camResultView[i].FigureGroup.Clear();
//                defectView[i].FigureGroup.Clear();

//                camResultView[i].Invalidate();
//                defectView[i].Invalidate();
//            }

//            lastDefectView1.Rows.Clear();
//            lastDefectView1.RowCount = 0;
//            lastDefectView2.Rows.Clear();
//            lastDefectView2.RowCount = 0;
//        }

//        public void Reset()
//        {
//            ErrorManager.Instance().ResetAlarm();

//            Clear();

//            startTime.Text = ("-");

//            camResultView[0].UpdateImage(null);
//            camResultView[1].UpdateImage(null);

//            sheetResultGrid1.Rows.Clear();
//            sheetResultGrid2.Rows.Clear();

//            (SystemManager.Instance().MachineIf as MonitoringServer).SendReset();
//            //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("RESET");

//            UpdateProductionInfo();
//        }

//        private void buttonResetCount_Click(object sender, EventArgs e)
//        {
//            Reset();
//        }

//        private void StateUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            //UpdateState();

//            HighlightRatio();
//        }

//        void UpdateStatusLabel(string text, Color foreColor, Color backColor)
//        {
//            labelStatus.Appearance.BackColor = backColor;
//            labelStatus.Appearance.ForeColor = foreColor;
//            labelStatus.Text = StringManager.GetString(this.GetType().FullName,text);
//        }

//        public void HighlightRatio()
//        {
//            Color color1 = Color.Transparent;
//            Color color2 = Color.Red;
//            Color color3 = Color.DarkRed;

//            if (this.monitoringPageBase.CurProduction == null)
//                return;

//            UltraLabel[] labelArray = new UltraLabel[] { productionRatio1, productionRatio2 };
//            for (int i = 0; i < 2; i++)
//            {
//                Data.Production subProduction = (Data.Production)this.monitoringPageBase.CurProduction.GetSubProduction(i);

//                if (subProduction != null && subProduction.Ng > 0)
//                {
//                    if (highlightCount % 2 == 1)
//                        labelArray[i].Appearance.BackColor = color2;
//                    else
//                        labelArray[i].Appearance.BackColor = color3;
//                }
//                else
//                {
//                    labelArray[i].Appearance.BackColor = color1;
//                    highlightCount = 0;
//                }
//            }

//            highlightCount++;
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
//                        break;
//                    case OpState.Wait:
//                        UpdateStatusLabel("Inspection", Color.Black, Color.CornflowerBlue);
//                        break;
//                    case OpState.Idle:
//                        UpdateStatusLabel("Idle", Color.Black, Color.Gray);
//                        break;
//                }
//            }


//        }

//        // No Use
//        private void buttonTrigger_Click(object sender, EventArgs e)
//        {
//            SystemManager.Instance().InspectRunner.StartInspection(InspectState.Run);
//        }

//        public void EnableControls()
//        {

//        }

//        public void TabPageVisibleChanged(bool visible)
//        {
//            if (visible == true)
//            {
//                modelName.Text = SystemManager.Instance().CurrentModel.Name;
//                UpdateProductionInfo();
//            }
//            else
//            {
//                /*foreach (System.Diagnostics.Process process in camVncProcess)
//                {
//                    if (process != null)
//                    {
//                        if (process.HasExited == false)
//                        {
//                            process.Kill();
//                        }
//                    }
//                }*/
//            }
//        }



//        //private List<Data.SheetCheckerSubResult> UpdateLastDefectView(int camIndex, ResultInfo resultInfo)
//        //{
//        //    string dataFile = String.Format("{0}\\result.csv", path);
//        //    List<Data.SheetCheckerSubResult> resultList = new List<Data.SheetCheckerSubResult>();
//        //    if (File.Exists(dataFile) == false)
//        //        return resultList;

//        //    onUpdateLastDefectView = true;

//        //    string inspectionNo = Path.GetFileName(path);
//        //    string sequenceNo = inspectionNo.Split('_')[1];

//        //    DataGridView dataGridView = null;

//        //    if (camIndex == 0)
//        //        dataGridView = lastDefectView1;
//        //    else
//        //        dataGridView = lastDefectView2;

//        //    string[] lines = File.ReadAllLines(dataFile, Encoding.Default);

//        //    string[] dateWords = lines[0].Split(';');
//        //    int sheetIndex = Convert.ToInt32(dateWords[0].Trim());

//        //    for (int i = 1; i < lines.Count(); i++)
//        //    {
//        //        string[] words = lines[i].Split(';');

//        //        if (words.Count() < 6)
//        //            continue;

//        //        string imagePath = String.Format("{0}\\{1}.bmp", path, words[0].Trim());
//        //        Image bitmap = ImageHelper.LoadImage(imagePath);

//        //        int defectX = (int)float.Parse(words[2]);
//        //        int defectY = (int)float.Parse(words[3]);
//        //        int defectWidth = (int)float.Parse(words[4]);
//        //        int defectHeight = (int)float.Parse(words[5]);

//        //        Rectangle rect = new Rectangle(defectX, defectY, defectWidth, defectHeight);

//        //        Data.SheetCheckerSubResult subResult = new Data.SheetCheckerSubResult();
//        //        subResult.ResultRect = new RotatedRect(rect, 0);
//        //        subResult.DefectType = (Data.SheetDefectType)Enum.Parse(typeof(Data.SheetDefectType), words[1].Trim());
//        //        subResult.Image = bitmap;
//        //        subResult.Message = String.Format("Size (um) : {0:.0} * {1:.0}\nPos. (mm) : {2:.0}(x), {3:.0}(y)", float.Parse(words[8]),
//        //                    float.Parse(words[9]), float.Parse(words[10]), float.Parse(words[11]));
//        //        subResult.ShortMessage = words[7];
//        //        //subResult.ShortMessage = 

//        //        string defectType = null;
//        //        Color color = new Color();
//        //        switch (subResult.DefectType)
//        //        {
//        //            case Data.SheetDefectType.BlackDefect:
//        //                defectType = "전극";
//        //                color = Color.Red;
//        //                break;
//        //            case Data.SheetDefectType.WhiteDefect:
//        //                defectType = "성형";
//        //                color = Color.Yellow;
//        //                break;
//        //        }

//        //        //if (dataGridView.RowCount > 200)
//        //        //    dataGridView.Rows.RemoveAt(dataGridView.RowCount - 1);

//        //        DataGridViewRow row = new DataGridViewRow();
//        //        DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
//        //        DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
//        //        DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();

//        //        DataGridViewImageCell cell4 = new DataGridViewImageCell();
//        //        DataGridViewTextBoxCell cellNull = new DataGridViewTextBoxCell();

//        //        cell1.Value = sequenceNo;
//        //        cell2.Value = words[0].Trim();
//        //        cell3.Value = defectType;
//        //        cell4.Value = bitmap;

//        //        //cell3.Style.BackColor = color;

//        //        cell4.ImageLayout = DataGridViewImageCellLayout.Zoom;

//        //        row.Cells.Add(cell1);
//        //        row.Cells.Add(cell2);
//        //        row.Cells.Add(cell3);
//        //        row.Cells.Add(cellNull);
//        //        row.Cells.Add(cell4);

//        //        row.Height = lastDefectView1.Height / 3;

//        //        row.Tag = subResult;
//        //        dataGridView.Rows.Add(row);

//        //        resultList.Add(subResult);
//        //    }

//        //    onUpdateLastDefectView = false;

//        //    return resultList;
//        //}

//        //delegate void UpdateDataDelegate(ResultInfo resultInfo);
//        //public void UpdateData(ResultInfo resultInfo)
//        //{
//        //    if (InvokeRequired)
//        //    {
//        //        Invoke(new UpdateDataDelegate(UpdateData), resultInfo);
//        //        return;
//        //    }

//        //    if (SystemState.Instance().OpState == OpState.Idle)
//        //        return;

//        //    DataGridView sheetResultGrid = null;

//        //    if (resultInfo.CamIndex == 0)
//        //        sheetResultGrid = sheetResultGrid1;
//        //    else
//        //        sheetResultGrid = sheetResultGrid2;


//        //    string inspResultPath = null;
//        //    foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
//        //    {
//        //        if (info.CamIndex == resultInfo.CamIndex && info.ClientIndex == resultInfo.ClientIndex)
//        //            inspResultPath = info.Path;
//        //    }

//        //    if (inspResultPath == null)
//        //        return;

//        //    inspResultPath = Path.Combine(inspResultPath, "Result", resultInfo.ResultPath);

//        //    string inspectionNo = Path.GetFileName(inspResultPath);
//        //    string sequenceNo = inspectionNo.Split('_')[1];

//        //    string dataFile = String.Format("{0}\\result.csv", inspResultPath);

//        //    Operation.Data.InspectionError inspectionErrorResult = Data.InspectionError.None;
//        //    Judgment judgment = Judgment.Accept;

//        //    if (File.Exists(dataFile) == true)
//        //    {
//        //        StreamReader sr = new StreamReader(dataFile);
//        //        string firstLine = sr.ReadLine();

//        //        string[] tokens = firstLine.Split(';');
//        //        if (tokens.Count() == 5)
//        //        {
//        //            DateTime inspTime = DateTime.Parse(tokens[2]);
//        //            if (sheetResultGrid.RowCount == 0)
//        //                sheetResultGrid.Rows.Add(sequenceNo, int.Parse(tokens[3]), inspTime.ToString("HH:mm:ss"));
//        //            else
//        //                sheetResultGrid.Rows.Insert(0, sequenceNo, int.Parse(tokens[3]), inspTime.ToString("HH:mm:ss"));

//        //            if (tokens[1] == "Reject")
//        //            {
//        //                sheetResultGrid.Rows[0].Cells[0].Style.BackColor = Color.Red;
//        //                judgment = Judgment.Reject;
//        //            }
//        //            else
//        //            {
//        //                sheetResultGrid.Rows[0].Cells[0].Style.BackColor = Color.Green;
//        //                judgment = Judgment.Accept;
//        //            }

//        //            inspectionErrorResult = (Data.InspectionError)Enum.Parse(typeof(Data.InspectionError), tokens[4]);

//        //            //resultInfo.ResultPath = inspResultPath;
//        //        }

//        //        sr.Close();
//        //    }

//        //    resultInfo.Judgment = judgment;
//        //    resultInfo.InspectionError = inspectionErrorResult;

//        //    sheetResultGrid.Rows[0].Tag = resultInfo;

//        //    ShowProductionInfo(resultInfo.CamIndex);
//        //}

//        void DrawErrorResult(int clientIndex, Judgment judgment, Data.InspectionError error)
//        {
//            Font font = new Font("Arial", 100, FontStyle.Bold);
//            string resultText = "OK";
//            Color resultColor = Color.Green;

//            if (judgment == Judgment.Reject)
//            {
//                switch (error)
//                {
//                    case Data.InspectionError.LightOff:
//                        //resultText = "Light Off";
//                        font = new Font("Arial", 60, FontStyle.Bold);
//                        resultText = "조명 상태를 확인해 주세요.";
//                        resultColor = Color.White;
//                        /*if (ErrorManager.Instance().IsAlarmed() == false)
//                            ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "조명 상태를 확인해 주세요."));*/
//                        break;
//                    case Data.InspectionError.DifferenceModel:
//                        //resultText = "Difference Model";
//                        font = new Font("Arial", 60, FontStyle.Bold);
//                        resultText = "모델 정보를 확인해주세요.";
//                        resultColor = Color.White;
//                        /*if (ErrorManager.Instance().IsAlarmed() == false)
//                            ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "모델 정보를 확인해주세요."));*/
//                        break;
//                    case Data.InspectionError.PatternRegionInvalidParam:
//                        //resultText = "Invalid Param";
//                        font = new Font("Arial", 60, FontStyle.Bold);
//                        resultText = "전극부(Black) 파라미터를 확인해주세요.";
//                        resultColor = Color.White;
//                        /*if (ErrorManager.Instance().IsAlarmed() == false)
//                            ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "전극부(Black) 파라미터를 확인해주세요."));*/
//                        break;
//                    case Data.InspectionError.WhiteRegionInvalidParam:
//                        font = new Font("Arial", 60, FontStyle.Bold);
//                        //resultText = "Invalid Param";
//                        resultText = "성형부 파라미터를 확인해주세요.";
//                        resultColor = Color.White;
//                        /*if (ErrorManager.Instance().IsAlarmed() == false)
//                            ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "성형부 파라미터를 확인해주세요."));*/
//                        break;
//                    case Data.InspectionError.InfiniteDefect:
//                        //resultText = error.ToString();
//                        font = new Font("Arial", 60, FontStyle.Bold);
//                        resultText = "불량 갯수가 너무 많습니다.\n(1000개 이상)";
//                        resultColor = Color.White;
//                        //if (ErrorManager.Instance().IsAlarmed() == false)
//                        //    ErrorManager.Instance().ErrorItemList.Add(new ErrorItem(true, "불량 갯수가 너무 많습니다.(1000개 이상)"));
//                        break;
//                    case Data.InspectionError.BlankSheet:
//                        resultText = "Blank Sheet";
//                        resultColor = Color.Yellow;
//                        //ErrorManager.Instance().ResetAlarm();
//                        break;
//                    default:
//                        resultText = "NG";
//                        resultColor = UniScanGUtil.Instance().NG;
//                        //ErrorManager.Instance().ResetAlarm();
//                        break;
//                }
//            }

//            /*if (ErrorManager.Instance().IsAlarmed() == true || critical)
//            {
//                alarmMessagerForm.UpdateData();
//                alarmMessagerForm.Show();
//            }*/

//            TextFigure textFigure = new TextFigure(resultText, new Point(camResultView[clientIndex].Image.Width - 50, 50), font, resultColor);
//            textFigure.Alignment = StringAlignment.Far;

//            camResultView[clientIndex].TempFigureGroup.AddFigure(textFigure);
//        }

//        private void MonitoringPage_Load(object sender, EventArgs e)
//        {

//        }

//        private void EnterWaitInspection()
//        {
//            if (SystemManager.Instance().InspectRunner.InspectRunnerExtender.Production == null)
//            {
//                MessageForm.Show(ParentForm, "Please, Input LotNo.", "UniEye");
//                return;
//            }

//            if (SystemState.Instance().GetOpState() == OpState.Idle)
//            {
//                /*if (MachineSettings.Instance().VirtualMode == true)
//                {
//                    if (offlineImagePathList == null)
//                    {
//                        MessageBox.Show("Offline image path is not defined.");
//                        return;
//                    }

//                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetImagePath(offlineImagePathList[offlineImageIndex]);
//                    offlineImageIndex++;
//                    if (offlineImageIndex >= offlineImagePathList.Count())
//                        offlineImageIndex = 0;
//                }*/

//                if (SystemManager.Instance().InspectRunner.EnterWaitInspection() == false)
//                    return;

//                buttonResetCount.Enabled = false;

//                buttonStop.Enabled = true;
//                buttonLotChange.Enabled = false;

//                buttonStart.Appearance.Image = global::UniScanG.Properties.Resources.Pause;
//                buttonStart.Text = "Pause";
//            }
//        }

//        private void buttonLotChange_Click(object sender, EventArgs e)
//        {
//            //InputForm form = new InputForm("Input Lot No");
//            //Point point = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (form.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (form.Height / 2));
//            //form.ChangeLocation(point);

//            //if (form.ShowDialog(Parent) == DialogResult.Cancel || form.InputText == "")
//            //{
//            //    return;
//            //}

//            //this.monitoringPageBase.ChangeLot(form.InputText);
//            //UpdateProductionInfo();

//            //(SystemManager.Instance().MachineIf as MonitoringServer).SendLotChange(form.InputText);
//        }

//        private void labelCamView1_DoubleClick(object sender, EventArgs e)
//        {

//        }

//        private void groupInspectionState_Click(object sender, EventArgs e)
//        {

//        }

//        private void lastDefectView1_Paint(object sender, PaintEventArgs e)
//        {
//            Brush defectBrush = new SolidBrush(Color.Black);
//            Font font = new Font("Arial", 12);
//            StringFormat stringFormat = new StringFormat();
//            stringFormat.Alignment = StringAlignment.Near;
//            stringFormat.LineAlignment = StringAlignment.Near;

//            Brush typeBrush = new SolidBrush(Color.Blue);

//            for (int i = 0; i < lastDefectView1.Rows.Count; i++)
//            {
//                if (lastDefectView1.Rows[i].Cells[0].Displayed == true)
//                {
//                    Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView1.Rows[i].Tag;
//                    if (sheetCheckerSubResult != null)
//                    {
//                        Rectangle cellRect = lastDefectView1.GetCellDisplayRectangle(3, i, false);

//                        e.Graphics.DrawString(sheetCheckerSubResult.Message,
//                            font, defectBrush, new Point(cellRect.Left + padding, cellRect.Top + (cellRect.Height / 3)), stringFormat);

//                        cellRect = lastDefectView1.GetCellDisplayRectangle(3, i, false);

//                        if (string.IsNullOrEmpty(sheetCheckerSubResult.Message) == false)
//                            e.Graphics.DrawString(sheetCheckerSubResult.Message, font, typeBrush, new Point(cellRect.Left + padding, cellRect.Bottom - padding * 10), stringFormat);
//                    }
//                }
//            }
//        }

//        private void lastDefectView2_Paint(object sender, PaintEventArgs e)
//        {
//            Brush defectBrush = new SolidBrush(Color.Black);
//            Font font = new Font("Arial", 12);
//            StringFormat stringFormat = new StringFormat();
//            stringFormat.Alignment = StringAlignment.Near;
//            stringFormat.LineAlignment = StringAlignment.Near;

//            Brush typeBrush = new SolidBrush(Color.Blue);

//            for (int i = 0; i < lastDefectView2.Rows.Count; i++)
//            {
//                if (lastDefectView2.Rows[i].Cells[0].Displayed == true)
//                {
//                    Data.SheetCheckerSubResult sheetCheckerSubResult = (Data.SheetCheckerSubResult)lastDefectView2.Rows[i].Tag;
//                    //Rectangle resultRect = ((Data.SheetCheckerSubResult)lastDefectView2.Rows[i]).ro;
//                    if (sheetCheckerSubResult != null)
//                    {
//                        Rectangle cellRect = lastDefectView2.GetCellDisplayRectangle(3, i, false);

//                        e.Graphics.DrawString(sheetCheckerSubResult.Message,
//                            font, defectBrush, new Point(cellRect.Left + padding, cellRect.Top + (cellRect.Height / 3)), stringFormat);

//                        cellRect = lastDefectView2.GetCellDisplayRectangle(3, i, false);

//                        if (string.IsNullOrEmpty(sheetCheckerSubResult.Message) == false)
//                            e.Graphics.DrawString(sheetCheckerSubResult.Message, font, typeBrush, new Point(cellRect.Left + padding, cellRect.Bottom - padding * 10), stringFormat);
//                    }
//                }
//            }
//        }


//        private void buttonShowImageViewer1_Click(object sender, EventArgs e)
//        {
//            /*((MpisMonitorSystemManager)SystemManager.Instance()).MonitoringServer.SendTabDisable(0, "Inspect", false);
//            ((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Connect..");

//            OpenVNC(0, UniScanGSettings.Instance().InspectorShared[0].IpAddress);

//            if (processWatchThread[0].IsAlive == false)
//            {
//                processWatchThread[0] = new Thread(ProcessWatch1);
//                processWatchThread[0].Start();
//            }*/
//        }

//        private void buttonShowImageViewer2_Click(object sender, EventArgs e)
//        {
//            /*((MpisMonitorSystemManager)SystemManager.Instance()).MonitoringServer.SendTabDisable(1, "Inspect", false);
//            ((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Connect..");

//            OpenVNC(1, UniScanGSettings.Instance().InspectorShared[1].IpAddress);

//            if (processWatchThread[1].IsAlive == false)
//            {
//                processWatchThread[1] = new Thread(ProcessWatch2);
//                processWatchThread[1].Start();
//            }*/
//        }

//        private void buttonStateGuide_Click(object sender, EventArgs e)
//        {
//            StateManualForm stateManualForm = new StateManualForm();
//            stateManualForm.Show();
//        }

//        private void sheetResultGrid_SelectionChanged(object sender, EventArgs e)
//        {
//            DataGridView selGridView = sender as DataGridView;
//            if (selGridView == null)
//                return;

//            DataGridView defectGridView = null;
//            int gridViewIndex = -1;
//            switch (selGridView.Name)
//            {
//                case "sheetResultGrid1": gridViewIndex = 0; defectGridView = lastDefectView1; break;
//                case "sheetResultGrid2": gridViewIndex = 1; defectGridView = lastDefectView2; break;
//                default: return; break;
//            }

//            if (selGridView.SelectedRows.Count == 0)
//                return;

//            SimpleResultList simpleResultList = this.monitoringPageBase.GetSimpleResultList(gridViewIndex);

//            int selectedRowIndex = selGridView.SelectedRows[0].Index;
//            if (simpleResultList.Count <= selectedRowIndex)
//                return;

//            Data.MpisInspResultArchiver archiver = new Data.MpisInspResultArchiver();
//            string resultPath = simpleResultList.ElementAt(selectedRowIndex).Value.ResultCsv;
//            Data.InspectionResult inspectionResult = archiver.LoadInspResult(resultPath) as Data.InspectionResult;
//            if (inspectionResult == null)
//                return;

//            //defectGridView.RowCount = 0;
//            //resultInfo.LoadImageRemote();
//            selGridView.Tag = inspectionResult;
//            Data.SheetCheckerAlgorithmResult algorithmResult = (inspectionResult.ProbeResultList[0] as VisionProbeResult).AlgorithmResult as Data.SheetCheckerAlgorithmResult;
//            defectGridView.RowCount = algorithmResult.SubResultList.Count;
//            defectGridView.Invalidate();

//            string wholeImageFile = String.Format("{0}\\WholeImage.jpg", inspectionResult.ResultPath);
//            if (File.Exists(wholeImageFile))
//            {
//                try
//                {
//                    Stream sr = new FileStream(wholeImageFile, FileMode.Open, FileAccess.Read);
//                    Bitmap bitmap = new Bitmap(sr);
//                    sr.Close();

//                    camResultView[gridViewIndex].UpdateImage(bitmap);
//                    camResultView[gridViewIndex].TempFigureGroup.Clear();
//                }
//                catch (IOException)
//                { }
//            }

//            FigureGroup fgResult = new FigureGroup();
//            inspectionResult.AppendResultFigures(fgResult, FigureDrawOption.Default);
//            //this.monitoringPageBase.GetResultFigure(inspectionResult, fgResult, null);
//            fgResult.Scale(.1f, .1f);
//            UpdateViewFigure(gridViewIndex, fgResult, null);

//            //DrawErrorResult(gridViewIndex, inspectionResult.Judgment, algorithmResult.Error);

//            //camResultView[resultInfo.CamIndex].Invalidate();
//        }


//        private void sheetResultGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
//        {
//            DataGridView selGridView = sender as DataGridView;
//            if (selGridView == null)
//                return;

//            int index = -1;
//            switch (selGridView.Name)
//            {
//                case "sheetResultGrid1": index = 0; break;
//                case "sheetResultGrid2": index = 1; break;
//                default: return; break;
//            }

//            SimpleResultList simpleResultList = this.monitoringPageBase.GetSimpleResultList(index);
//            if (simpleResultList.Count <= e.RowIndex)
//                return;

//            Data.SimpleInspectionResult simpleResult = simpleResultList.ElementAt(e.RowIndex).Value;
//            switch (e.ColumnIndex)
//            {
//                case 0:
//                    e.Value = simpleResult.SheetNo;
//                    selGridView.Rows[e.RowIndex].Cells[0].Style.BackColor = (simpleResult.DefectCount == 0 ? Color.LightGreen : Color.Red);
//                    break;
//                case 1:
//                    e.Value = simpleResult.DefectCount;
//                    break;
//                case 2:
//                    e.Value = simpleResult.InspTime.ToString("HH:mm:ss");
//                    break;
//            }
//        }

//        private void lastDefectView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
//        {
//            DataGridView selGridView = sender as DataGridView;
//            if (selGridView == null)
//                return;

//            DataGridView sheetGridView = null;
//            switch (selGridView.Name)
//            {
//                case "lastDefectView1": sheetGridView = sheetResultGrid1; break;
//                case "lastDefectView2": sheetGridView = sheetResultGrid2; break;
//                default: return; break;
//            }

//            InspectionResult resultInfo = sheetGridView.Tag as InspectionResult;
//            if (resultInfo == null)
//                return;

//            Data.SheetCheckerAlgorithmResult algorithmResult = (resultInfo.ProbeResultList[0] as VisionProbeResult).AlgorithmResult as Data.SheetCheckerAlgorithmResult;
//            Data.SheetCheckerSubResult subResult = algorithmResult.SubResultList[e.RowIndex] as Data.SheetCheckerSubResult;
//            switch (e.ColumnIndex)
//            {
//                case 0:
//                    e.Value = int.Parse(resultInfo.InspectionNo);
//                    break;
//                case 1:
//                    e.Value = e.RowIndex + 1;
//                    break;
//                case 2:
//                    e.Value = subResult.DefectType.ToString();
//                    break;
//                case 3:
//                    e.Value = subResult.Message;
//                    break;
//                case 4:
//                    e.Value = subResult.Image;
//                    if (e.Value == null)
//                    {
//                        e.Value = ImageHelper.LoadImage(Path.Combine(resultInfo.ResultPath, string.Format("{0}.bmp", e.RowIndex)));
//                    }

//                    break;
//            }
//        }
//    }
//}

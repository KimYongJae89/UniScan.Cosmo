//using System;
//using System.Windows.Forms;
//using DynMvp.Data;
//using DynMvp.Devices;
//using DynMvp.InspData;
//using DynMvp.UI;
//using DynMvp.Base;
//using DynMvp.Devices.Dio;
//using DynMvp.UI.Touch;
//using UniEye.Base;
//using UniEye.Base.UI;
//using UniEye.Base.Device;
//using UniEye.Base.Data;
//using System.Drawing;
//using System.Threading.Tasks;
//using System.Threading;
//using DynMvp.Authentication;
//using Infragistics.Win.Misc;
//using System.Runtime.InteropServices;
//using System.Collections.Generic;
//using UniScanG.Operation.Inspect;
//using UniEye.Base.MachineInterface;
//using System.IO.Compression;
//using System.IO;
//using UniEye.Base.Settings;
//using UniScanG.Operation.UI.Monitor;
//using System.Diagnostics;

//namespace UniScanG.Temp
//{
//    public partial class MainForm : Form, LoggingTarget, IMainForm, IMonitoringServerListener, IUserHandlerListener
//    {
//        VNCManager mainFormVNCManager = new VNCManager(null);

//        bool onRemoteTeachingMode = false;
//        bool onRemoteCommand = false;

//        CancellationTokenSource cancellationTokenSource;

//        private ModelManagePage modelManagerPage;
//        public ModelManagePage ModelManagerPage
//        {
//            get { return modelManagerPage; }
//        }

//        private ReportPanel reportPage;
//        public ReportPanel ReportPage
//        {
//            get { return reportPage; }
//        }

//        private SettingPage settingPage;

//        private IMonitoringPage monitoringPage;
//        public IMonitoringPage MonitoringPage
//        {
//            get { return monitoringPage; }
//        }

//        private TeachingPage teachingPage;
//        public TeachingPage TeachingPage
//        {
//            get { return teachingPage; }
//        }

//        private Monitor.TeachingPage remoteTeachingPage;
//        public Monitor.TeachingPage RemoteTeachingPage
//        {
//            get { return remoteTeachingPage; }
//        }

//        private InspectionPage inspectionPage;
//        public InspectionPage InspectionPage
//        {
//            get { return inspectionPage; }
//        }

//        private ViewerPage viewerPage;
//        public ViewerPage ViewerPage
//        {
//            get { return viewerPage; }
//        }

//        private AlarmMessageForm alarmMessageForm = new AlarmMessageForm();
//        public AlarmMessageForm AlarmMessageForm
//        {
//            get { return alarmMessageForm; }
//        }

//        IInspectionPage IMainForm.InspectionPage
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        private UltraLabel[,] camStateArray = new UltraLabel[2, 2];
//        private object updateTimeLogLockObject = new byte();

//        Task waitJobDoneTask = null;
//        bool waitJobDone = false;

//        public MainForm()
//        {
//            InitializeComponent();

//            LogHelper.LoggingTarget = this;

//            modelManagerPage = new ModelManagePage();
//            reportPage = new ReportPanel();
//            settingPage = new SettingPage();
//            //viewerPage = new ViewerPage();

//            this.tabPageModel.Controls.Add(this.modelManagerPage);
//            this.tabPageReport.Controls.Add(this.reportPage);
//            this.tabPageSetting.Controls.Add(this.settingPage);
//            //this.tabPageViewer.Controls.Add(this.viewerPage);

//            LogHelper.Debug(LoggerType.StartUp, "Init Model Page");

//            this.modelManagerPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.modelManagerPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.modelManagerPage.Location = new System.Drawing.Point(0, 313);
//            this.modelManagerPage.Name = "modelManagerPage";
//            this.modelManagerPage.Size = new System.Drawing.Size(466, 359);
//            this.modelManagerPage.TabIndex = 0;

//            LogHelper.Debug(LoggerType.StartUp, "Init Report Page");

//            this.reportPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.reportPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.reportPage.Location = new System.Drawing.Point(0, 313);
//            this.reportPage.Name = "reportPage";
//            this.reportPage.Size = new System.Drawing.Size(466, 359);
//            this.reportPage.TabIndex = 0;
//            this.reportPage.Initialize();

//            /*LogHelper.Debug(LoggerType.StartUp, "Init Viewer Page");

//            this.viewerPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.viewerPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.viewerPage.Location = new System.Drawing.Point(0, 313);
//            this.viewerPage.Name = "Viewer Page";
//            this.viewerPage.Size = new System.Drawing.Size(466, 359);
//            this.viewerPage.TabIndex = 0;*/

//            LogHelper.Debug(LoggerType.StartUp, "Init setting Page");

//            this.settingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.settingPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.settingPage.Location = new System.Drawing.Point(0, 313);
//            this.settingPage.Name = "settingPage";
//            this.settingPage.Size = new System.Drawing.Size(466, 359);
//            this.settingPage.TabIndex = 0;
//            this.settingPage.Initialize();
//            this.settingPage.LoadSettings();

//            this.alarmMessageForm.Hide();

//            camStateArray[0, 0] = statusCam1A;
//            camStateArray[0, 1] = statusCam1B;
//            camStateArray[1, 0] = statusCam2A;
//            camStateArray[1, 1] = statusCam2B;

//            //if (UniScanGSettings.Instance().SystemType == SystemType.Monitor)
//            //{
//            //    remoteTeachingPage = new Monitor.TeachingPage();
//            //    this.tabPageTeach.Controls.Add(this.remoteTeachingPage);

//            //    LogHelper.Debug(LoggerType.StartUp, "Init Remote Teaching Page");

//            //    this.remoteTeachingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            //    this.remoteTeachingPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            //    this.remoteTeachingPage.Location = new System.Drawing.Point(0, 313);
//            //    this.remoteTeachingPage.Name = "RemoteTeachingPage";
//            //    this.remoteTeachingPage.Size = new System.Drawing.Size(466, 359);
//            //    this.remoteTeachingPage.TabIndex = 0;

//            //    LogHelper.Debug(LoggerType.StartUp, "Init Monitoring Page");
//            //    switch (UniScanGSettings.Instance().MonitorInfo.Version)
//            //    {
//            //        case "1.0":
//            //            this.monitoringPage = new MonitoringPage();
//            //            break;
//            //        case "2.0":
//            //            this.monitoringPage = new MonitoringPageV2();
//            //            break;
//            //    }
//            //    UserControl monitoringPage = this.monitoringPage as UserControl;
//            //    this.tabPageMonitoring.Controls.Add(monitoringPage);

//            //    monitoringPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            //    monitoringPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            //    monitoringPage.Location = new System.Drawing.Point(0, 313);
//            //    monitoringPage.Name = "Monitoring Page";
//            //    //monitoringPage.Size = new System.Drawing.Size(466, 359);
//            //    monitoringPage.TabIndex = 0;

//            //    tabControlMain.Tabs["Inspect"].Visible = false;

//            //    if(SystemManager.Instance().MachineIf is MonitoringServer)
//            //        (SystemManager.Instance().MachineIf as MonitoringServer).Listener = this;

//            //}
//            //else
//            //{
//            //    teachingPage = new TeachingPage();
//            //    this.tabPageTeach.Controls.Add(this.teachingPage);

//            //    LogHelper.Debug(LoggerType.StartUp, "Init Teaching Page");

//            //    this.teachingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            //    this.teachingPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            //    this.teachingPage.Location = new System.Drawing.Point(0, 313);
//            //    this.teachingPage.Name = "TeachingPage";
//            //    this.teachingPage.Size = new System.Drawing.Size(466, 359);
//            //    this.teachingPage.TabIndex = 0;

//            //    inspectionPage = new InspectionPage();
//            //    this.tabPageInspect.Controls.Add(this.inspectionPage);

//            //    LogHelper.Debug(LoggerType.StartUp, "Init Inspection Page");

//            //    this.inspectionPage.BackColor = System.Drawing.SystemColors.ButtonFace;
//            //    this.inspectionPage.Dock = System.Windows.Forms.DockStyle.Fill;
//            //    this.inspectionPage.Location = new System.Drawing.Point(0, 313);
//            //    this.inspectionPage.Name = "Inspection Page";
//            //    this.inspectionPage.Size = new System.Drawing.Size(466, 359);
//            //    this.inspectionPage.TabIndex = 0;

//            //    tabControlMain.Tabs["Monitoring"].Visible = false;
//            //    tabControlMain.Tabs["Report"].Visible = false;
//            //    tabControlMain.Tabs["CCTV"].Visible = false;

//            //    statusCam1A.Visible = false;
//            //    statusCam1B.Visible = false;
//            //    statusCam2A.Visible = false;
//            //    statusCam2B.Visible = false;

//            //    statusDoor.Visible = false;
//            //    statusFan.Visible = false;
//            //    statusLight.Visible = false;

//            //    tableLayoutPanel2.RowStyles[0].Height = 0;
//            //    //labelMachineStateTitle.Visible = false;
//            //    //labelMachineState.Visible = false;
//            //}

//            panelTopTitle.BackgroundImage = global::UniScanG.Properties.Resources.title_bar_R2RT;

//            /*var pos = panelTopRight.PointToScreen(pictureOperator.Location);
//            pos = pictureBoxDummyRight.PointToClient(pos);
//            pictureOperator.Parent = pictureBoxDummyRight;*/
//            //pictureOperator.Location = pos;
//            //pictureOperator.BackColor = Color.Transparent;

//            timer.Start();

//            UserHandler.Instance().AddListener(this);
//            UserHandler.Instance().CurrentUser = UserHandler.Instance().CurrentUser;

//            EnableTabs("Inspect", false);
//            EnableTabs("Monitoring", false);
//            EnableTabs("Teach", false);
//        }

//        public void TransferModelPrepare()
//        {
//            Model curModel = SystemManager.Instance().CurrentModel;
//            string srcPath = curModel.ModelPath;
 
//            string tempPath = Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Temp, "TransferModel.zip");
//            if (File.Exists(tempPath))
//                File.Delete(tempPath);

//            SimpleProgressForm waitForm = new SimpleProgressForm("Compressing");
//            waitForm.Show(new Action(() => ZipFile.CreateFromDirectory(srcPath, tempPath)));
//        }

//        SimpleProgressForm modelTransferWaitform = null;
//        public void TransferModelWait()
//        {
//            modelTransferWaitform = new SimpleProgressForm("Transfer...");
//            modelTransferWaitform.Show();
//        }

//        public void TransferModel(string dstPath)
//        {
//            string srcFile = Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Temp, "TransferModel.zip");
//            string dstFile = Path.Combine(dstPath, "Temp", "TransferModel.zip");
//            if (File.Exists(dstFile))
//                File.Delete(dstFile);

//            SimpleProgressForm waitForm = new SimpleProgressForm("Transfer...");
//            waitForm.Show(new Action(() => File.Copy(srcFile, dstFile)));
//        }

//        public void TransferModelDone()
//        {
//            modelTransferWaitform.Close();
//            modelTransferWaitform = null;

//            ((MainForm)SystemManager.Instance().MainForm).TabChange("Model");

//            string curModelName = SystemManager.Instance().CurrentModel.Name;
//            string modelPath = SystemManager.Instance().CurrentModel.ModelPath;
//            Directory.Delete(modelPath, true);

//            string zipFile = Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Temp, "TransferModel.zip");

//            SimpleProgressForm waitForm = new SimpleProgressForm("Decompressing");
//            waitForm.Show(new Action(() => ZipFile.ExtractToDirectory(zipFile, modelPath)));

//            Model curModel = SystemManager.Instance().CurrentModel;
//            curModel.CloseModel();
//            SystemManager.Instance().ModelManager.LoadModel(curModel, null);

//            ((MainForm)SystemManager.Instance().MainForm).TabChange("Teach");
//        }

//        public bool RcEnterWait(bool acyncMode, float convSpeed)
//        {
//            if (tabControlMain.ActiveTab.Key == "Inspect")
//            {
//                return InspectionPage.ButtonStartClick(true, acyncMode, convSpeed);
//            }
//            return false;
//        }

//        public bool RcStartInspGrab()
//        {
//            // Master는 1초 후 그랩 시작 - Slave 먼저 시작해야 하니까
//            if (UniScanGSettings.Instance().InspectorInfo.ClientIndex == 0)
//                Thread.Sleep(1000);

//            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();
//            return true;
//        }

//        public float QuaryConvSpeed()
//        {
//            //if (UniScanGSettings.Instance().AsyncMode)
//            //{
//            //    float convSpeedMPS = 1.5f;
//            //    Model curModel = SystemManager.Instance().CurrentModel;
//            //    if (curModel != null)
//            //        convSpeedMPS = ((UniScanG.Operation.Data.ModelDescription)curModel.ModelDescription).ConvayorSpeedMPS;

//            //    float convSpeedMPM = convSpeedMPS * 60;
//            //    if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
//            //    {
//            //        DynMvp.UI.InputForm form = new DynMvp.UI.InputForm("Conveyer speed? [m/min]", convSpeedMPM.ToString());
//            //        if (form.ShowDialog() == DialogResult.Cancel)
//            //            return -1;

//            //        if (float.TryParse(form.InputText, out convSpeedMPM) == false)
//            //        {
//            //            MessageForm.Show(null, "Wrong Input.");
//            //            return -1;
//            //        }
//            //    }
//            //    float convayorSpeedMPS = convSpeedMPM / 60;
//            //    if(curModel!=null)
//            //        ((UniScanG.Operation.Data.ModelDescription)curModel.ModelDescription).ConvayorSpeedMPS = convayorSpeedMPS;
//            //    return convayorSpeedMPS;
//            //}
//            return 0;
//        }

//        public void InspectionDone(Client client, string resultPath)
//        {
//            if (SystemState.Instance().GetOpState() != OpState.Idle)
//                monitoringPage.InspectionDone(client, resultPath);

//        }

//        public void TeachDone(Client client, bool trained, string imagePath)
//        {
//        }

//        public void Log(string messgae)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new LogDelegate(Log), messgae);
//                return;
//            }

//            if (logList.Items.Count > 2000)
//                logList.Items.RemoveAt(0);

//            int index = logList.Items.Add(messgae);
//            logList.TopIndex = index;
//        }

//        public void ChangeInspectionSize()
//        {
//            throw new NotImplementedException();
//        }

//        public void EnableTabs()
//        {

//        }

//        delegate void EnableTabsDelegate2(string key, bool enable);
//        public void EnableTabs(string key, bool enable)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new EnableTabsDelegate2(EnableTabs), key, enable);
//                return;
//            }

//            tabControlMain.Tabs[key].Enabled = enable;
//        }

//        public void InspectionFinished(InspectionResult inspectionResult)
//        {
//            throw new NotImplementedException();
//        }

//        public void InspectionStarted()
//        {

//        }

//        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
//        {
//            throw new NotImplementedException();
//        }

//        public void Load2dImage(int cameraIndex, int stepIndex, int lightTypeIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public void ModifyTeaching(string imagePath)
//        {
//            throw new NotImplementedException();
//        }

//        public void PrepareInspection()
//        {
//            //throw new NotImplementedException();
//        }

//        public void ProductInspected(InspectionResult inspectionResult)
//        {
//            //if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
//            //    inspectionPage.ProductInspected(inspectionResult);
//        }

//        public void PostInspection(InspectionResult inspectionResult)
//        {

//        }

//        public void StopInspection()
//        {
//            //throw new NotImplementedException();
//        }

//        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
//        {

//        }

//        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
//        {

//        }

//        public void UpdateButton()
//        {
//        }

//        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult)
//        {
//            Data.InspectionResult mpisInspectionResult = (Data.InspectionResult)inspectionResult;



//        }

//        public void UpdateInspectionNo(string inspectionNo)
//        {
//            //throw new NotImplementedException();
//        }

//        private void MainForm_Load(object sender, EventArgs e)
//        {
//            //alarmMessageForm.Show();
//            //ErrorManager.Instance().OnResetAlarmStatus += errorManager_ResetAlarmStatus;
//            //this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
//            this.WindowState = FormWindowState.Maximized;

//            MachineIf machineIf = SystemManager.Instance().MachineIf;
            
//            if (machineIf is MonitoringClient)
//            {
//                (machineIf as MonitoringClient).SendStartClient();
//                //(machineIf as MonitoringClient).StartThread();
//            }
//            else
//            {
//                UpdateClientStateLamp();
//            }

//            InitMachineStateLable();

//            if (MachineSettings.Instance().VirtualMode)
//                MessageForm.Show(this, "VirtualMode Flag is ON");

//            if (UniScanGSettings.Instance().InspectorInfo.StandAlone)
//                MessageForm.Show(this, "StandAlone Flag is ON");
//        }

//        private void InitMachineStateLable()
//        {
//            IoPort airFanIoPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanG.Device.IoPortName.OutAirFan);
//            IoPort roomLightIoPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanG.Device.IoPortName.OutRoomLight);
//            List<IoPort> doorIoPortList = new List<IoPort>();
//            SystemManager.Instance().DeviceBox.PortMap.GetOutDoorPorts(doorIoPortList);

//            bool state = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(roomLightIoPort);
//            UpdateControl("Light", state ? "On" : "Off");

//            state = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(airFanIoPort);
//            UpdateControl("Fan", state ? "On" : "Off");

//            bool doorState = false;
//            foreach (IoPort doorIoPort in doorIoPortList)
//            {
//                if (SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(doorIoPort) == true)
//                {
//                    doorState = true;
//                    break;
//                }
//            }
//            state = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(roomLightIoPort);
//            UpdateControl("Light", state ? "On" : "Off");

//            state = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(airFanIoPort);
//            UpdateControl("Fan", state ? "On" : "Off");

//            state = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(airFanIoPort);
//            UpdateControl("Door", state ? "On" : "Off");
//        }

//        private void errorManager_ResetAlarmStatus()
//        {
//            throw new NotImplementedException();
//        }
//        delegate void UpdateMainTabDelegate(bool Teached);
//        public void UpdateMainTab(bool Teached)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateMainTabDelegate(UpdateMainTab), Teached);
//                return;
//            }

//            if (SystemManager.Instance().CurrentModel == null)
//            {
//                EnableTabs("Teach", false);
//                EnableTabs("Inspect", false);
//                EnableTabs("Monitoring", false);

//                TabChange("Model");
//                return;
//            }
            
//            EnableTabs("Teach", true);

//            if (Teached == true || true)
//            {
//                EnableTabs("Inspect", true);
//                EnableTabs("Monitoring", true);
//            }
//        }

//        private void tabControlMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
//        {
//            //LogHelper.Debug(LoggerType.Operation, "Start tabControlMain_SelectedTabChanged.");

//            //if (e.PreviousSelectedTab != null)
//            //{
//            //    Infragistics.Win.UltraWinTabControl.UltraTabPageControl preTabControl = e.PreviousSelectedTab.TabPage;

//            //    if (preTabControl.Controls.Count > 0 && preTabControl.Controls[0] is IMainTabPage)
//            //    {
//            //        IMainTabPage preTabPage = (IMainTabPage)preTabControl.Controls[0];
//            //        preTabPage.TabPageVisibleChanged(false);
//            //    }
//            //}
            
//            //if (UniScanGSettings.Instance().SystemType == SystemType.Monitor)
//            //{
//            //    if (e.Tab.Key == "Model" || e.Tab.Key == "Teach" || e.Tab.Key == "Monitoring")
//            //    {
//            //        string modeName = e.Tab.Key;
//            //        if (modeName == "Monitoring")
//            //        {
//            //            modeName = "Inspect";
//            //        }

//            //        try
//            //        {
//            //            (SystemManager.Instance().MachineIf as MonitoringServer).SendChangeMode(modeName);
//            //            //WaitJobDone("Change Mode");
//            //        }
//            //        catch(InvalidOperationException)
//            //        {
//            //            MessageForm.Show(null, "Invalid Operation");

//            //        }
//            //        catch (OperationCanceledException)
//            //        {
//            //            MessageForm.Show(null, "Operation Canceled");
//            //        }
//            //    }
//            //}
//            //else
//            //{
//            //    if (onRemoteCommand)
//            //        EnableTabs(true);

//            //    if(e.PreviousSelectedTab!=null && e.PreviousSelectedTab.Key == "Setting")
//            //    {
//            //        foreach (UltraLabel uLabel in camStateArray)
//            //            uLabel.Visible = false;

//            //        int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//            //        int clientIndex = UniScanGSettings.Instance().InspectorInfo.ClientIndex;
//            //        if(clientIndex>=0)
//            //            camStateArray[camIndex, clientIndex].Visible = true;
//            //    }
//            //}

//            //Infragistics.Win.UltraWinTabControl.UltraTabPageControl curTabControl = e.Tab.TabPage;
//            //if (curTabControl.Controls.Count > 0 && curTabControl.Controls[0] is IMainTabPage)
//            //{
//            //    IMainTabPage curTabPage = (IMainTabPage)curTabControl.Controls[0];
//            //    curTabPage.TabPageVisibleChanged(true);
//            //}

//            //LogHelper.Debug(LoggerType.Operation, "End tabControlMain_SelectedTabChanged.");
//        }

//        private void tabControlMain_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
//        {
//            if (onRemoteTeachingMode == true)
//            {
//                e.Cancel = true;
//            }
//            else if(e.Tab.Key == "Monitoring")
//            {
//                try
//                {
//                    if(SystemManager.Instance().CurrentModel.IsTaught()==false)
//                    {
//                        //e.Cancel =  MessageForm.Show(null, "Model is NOT Train. \r\nShell I Continue Anyway?", MessageFormType.YesNo)== DialogResult.No;
//                        e.Cancel = false;
//                        //e.Cancel = true;
//                    }
//                }
//                catch (OperationCanceledException)
//                { }
//            }
//            else if(e.Tab.Key == "CCTV")
//            {
//                e.Cancel = true;
//            }
//            else if (e.Tab.Key == "Exit")
//            {
//                e.Cancel = true;
//                this.Close();
//            }
//        }

//        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
//        {
//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;

//            //if (curModel != null)
//            //{
//            //    if (curModel.Modified)
//            //    {
//            //        SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model..");
//            //        loadingForm.Show(new Action(() =>
//            //        {
//            //            SystemManager.Instance().ModelManager.SaveModel(curModel);
//            //        }));
//            //    }

//            //    SystemManager.Instance().ModelManager.CloseModel(curModel);
//            //}

//            //ReleaseIo();
//        }

//        public void ReleaseIo()
//        {
//            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
//            SystemManager.Instance().DeviceController.TowerLamp?.Stop();
//            try
//            {
//                PortList portList = deviceBox.PortMap.OutPortList;
//                foreach (IoPort port in portList)
//                {
//                    deviceBox.DigitalIoHandler.WriteOutput(port, false);
//                }
//                deviceBox.Release();
//                SystemManager.Instance().DeviceController.Release();
//            }
//            catch (InvalidCastException)
//            {
//                return;
//            }

//            if (deviceBox.LightCtrlHandler != null)
//                deviceBox.LightCtrlHandler.TurnOff();
//        }

//        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            bool close = CheckFormCloseing();
//           // if(close && SystemManager.Instance() is MpisMonitorSystemManager)
//            {
//                MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//                //monitoringServer.Stop();
//                //monitoringServer.Close();
//            }
//            e.Cancel = !close;
//        }

//        private bool CheckFormCloseing()
//        {
//            if (SystemState.Instance().OnInspectOrWaitOrPause)
//            {
//                MessageForm.Show(this, "Please, Stop the inspection.", "UniEye");
//                return false;
//            }

//            if (MessageForm.Show(this, "Do you want to exit program?", MessageFormType.YesNo) == DialogResult.No)
//            {
//                return false;
//            }

//            Model curModel = SystemManager.Instance().CurrentModel;
//            if (curModel != null && curModel.Modified)
//                SystemManager.Instance().ModelManager.SaveModel(curModel);


//            curModel?.CloseModel();
//            return true;
//        }

//        public void OnPreInspection()
//        {
//        }

//        public void OnPostInspection()
//        {
//        }

//        delegate void TabChangeDelegate(string key);
//        public void TabChange(string key)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new TabChangeDelegate(TabChange), key);
//                return;
//            }

//            if (tabControlMain.Tabs[key].Visible == true && tabControlMain.Tabs[key].Enabled == true)
//                if (tabControlMain.SelectedTab != tabControlMain.Tabs[key])
//                    tabControlMain.SelectedTab = tabControlMain.Tabs[key];
//        }

//        public void RcSync(string xmlPath)
//        {
//            //SimpleProgressForm form = new SimpleProgressForm("Sync");
//            //form.Show(() =>
//            //{
//                string path = Path.Combine(@"\\", UniScanGSettings.Instance().UmxTcpIpInfo.IpAddress, "Gravure", xmlPath);
//                int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//                UpdateRemoteStepData(path, camIndex);

//                teachingPage?.UpdateControls(false);
//            //});
//        }

//        public string RcTeach()
//        {
//            if (teachingPage == null)
//                return "aaaaaaaa";
            
//            return teachingPage.RcTeach();
//        }

//        public void ClientAlive(Client client)
//        {
//            UpdateClientStateLamp(client);
//        }
        
//        private void timer_Tick(object sender, EventArgs e)
//        {
//            DateTime curTime = DateTime.Now;
//            labelDate.Text = curTime.ToString("yyyy - MM - dd");
//            labelTime.Text = curTime.ToString("HH : mm : ss");

//            //clientList.ForEach(f => f.DecAliveCount());
//            //UpdateClientStateLamp();

//            alarmMessageForm.UpdateData();
//        }

//        private void UpdateClientStateLamp()
//        {
//            (SystemManager.Instance().MachineIf as MonitoringServer).ClientList.ForEach(f => UpdateClientStateLamp(f));
//        }

//        private void UpdateClientStateLamp(Client client)
//        {
//            monitoringPage?.UpdateClientState(client);

//            UltraLabel camState = this.camStateArray[client.CamIndex, client.ClientIndex];
//            Color color;

//            if (client.State == ClientState.Disconnected)
//            {
//                //camState.Appearance.BackColor = Color.Red;
//                color = UniScanGUtil.Instance().DisConnected;
//            }
//            else if (client.State == ClientState.Connected)
//            {
//                color = UniScanGUtil.Instance().Connected;
//                //camState.Appearance.BackColor = Color.DarkGray;
//            }
//            else
//            {
//                switch (client.State)
//                {
//                    default:
//                    case ClientState.Idle:
//                        color = UniScanGUtil.Instance().Connected;
//                        break;
//                    case ClientState.Inspect:
//                        color = UniScanGUtil.Instance().Inspect;
//                        break;
//                    case ClientState.Pause:
//                        color = UniScanGUtil.Instance().Pause;
//                        break;
//                    case ClientState.Wait:
//                        color = UniScanGUtil.Instance().Wait;
//                        break;
//                }
//                //camState.Appearance.BackColor = color1;
//            }
//            UpdateClientStateLamp(camState, color);
//        }

//        public void UpdateClientStateLamp(bool connected)
//        {
//            int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//            int clientIndex = UniScanGSettings.Instance().InspectorInfo.ClientIndex;

//            UltraLabel stateLabel = camStateArray[camIndex, clientIndex];
//            Color color = (connected ? UniScanGUtil.Instance().Connected : UniScanGUtil.Instance().DisConnected);
//            UpdateClientStateLamp(stateLabel, color);
//        }

//        public delegate void UpdateClientStateLampDelegate(UltraLabel camState, Color color);
//        public void UpdateClientStateLamp(UltraLabel camState, Color color)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateClientStateLampDelegate(UpdateClientStateLamp), camState, color);
//                return;
//            }
//            camState.Appearance.BackColor = color;
//        }

//        private void tabControlMain_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
//        {
//            /*if (tabControlMain.ActiveTab.Key == "Model" || tabControlMain.ActiveTab.Key == "Setting")
//            {
//                if (modelManagerPage.ModelSelected == false)
//                {
//                    if (e.Tab.Key == "Teach" || e.Tab.Key == "Inspect" || e.Tab.Key == "Monitoring")
//                    {
//                        if (onRemoteCommand == false)
//                        {
//                            MessageForm.Show(this, "Please, select model first");
//                        }

//                        e.Cancel = true;
//                    }
//                }
//            }
//            else */
//            if (tabControlMain.ActiveTab.Key == "Inspect" || tabControlMain.ActiveTab.Key == "Monitoring")
//            {
//                if (e.Tab.Key == "Report" || e.Tab.Key == "Setting")
//                    return;

//                if (SystemState.Instance().OnInspectOrWaitOrPause)
//                {
//                    if (onRemoteCommand == true)
//                    {
//                        inspectionPage.ExitWaitInspection();
//                    }
//                    else
//                    {
//                        MessageForm.Show(this, "Please, stop the inspection first");
//                        e.Cancel = true;
//                    }
//                }
//            }
//            else if (tabControlMain.ActiveTab.Key == "Report" || tabControlMain.ActiveTab.Key == "Setting")
//            {
//                if (SystemState.Instance().OnInspectOrWaitOrPause)
//                {
//                    if (e.Tab.Key == "Teach" || e.Tab.Key == "Model")
//                    {
//                        if (onRemoteCommand == true)
//                        {
//                            inspectionPage.ExitWaitInspection();
//                        }     
//                        else
//                        {
//                            MessageForm.Show(this, "Please, stop the inspection first");
//                            e.Cancel = true;
//                        }
//                    }
//                }
//                /*else
//                {
//                    if (SystemManager.Instance().CurrentModel == null && e.Tab.Key != "Model")
//                    {
//                        if (onRemoteCommand == false)
//                        {
//                            MessageForm.Show(this, "Please, select model first");
//                        }

//                        e.Cancel = true;
//                    }
//                }*/
//            }
//        }

//        //public string RcInspect()
//        //{
//        //    onRemoteCommand = true;

//        //    string resultImageFIle = (teachingPage == null) ? "" : teachingPage.RcInspect();

//        //    onRemoteCommand = false;
//        //    return null; resultImageFIle;
//        //}

//        public void ChangeMode(string modeStr)
//        {
//            onRemoteCommand = true;

//            TabChange(modeStr);

//            onRemoteCommand = false;
//        }

//        public void Reset()
//        {
//            inspectionPage.RemoteReset();

//            onRemoteCommand = false;
//        }

//        public bool RcGrab(float cvySpeedMPS)
//        {
//            onRemoteCommand = true;

//            if (teachingPage == null)
//                return false;

//            bool ok = teachingPage.RcGrab(cvySpeedMPS);

//            onRemoteCommand = false;
//            return ok;
//        }

//        public bool RcFiducialGrab(float cvySpeedMPS, string xmlPath)
//        {
//            onRemoteCommand = true;

//            if (teachingPage == null)
//                return false;

//            bool ok = teachingPage.RcFiducialGrab(cvySpeedMPS);


//            onRemoteCommand = false;
//            return ok;
//        }

//        public void RcTestGrab(float cvySpeedMPS)
//        {
//            LogHelper.Debug(LoggerType.Operation, "MaonForm::RcTestGrab");
//            onRemoteCommand = true;
            
//            //Task task = new Task(() =>
//            //{
//                teachingPage?.RcTestGrab(cvySpeedMPS);
//            //});
//            //task.Start();

//            onRemoteCommand = false;
//        }

//        public void RcStop()
//        {
//            onRemoteCommand = true;

//            teachingPage?.RcStop();

//            //MonitoringClient monitoringClient = (MonitoringClient)SystemManager.Instance().MachineIf;
//            //monitoringClient.SendJobDone(new Comm.Protocol(Comm.ECommand.STOP, true);

//            onRemoteCommand = false;
//        }
        
//        public void TeachInspectionDone(Client client, string resultPath)
//        {
//            remoteTeachingPage?.TeachInspectionDone(client, resultPath);
//        }

//        public delegate void StartWaitJobDoneDelegate(string message, CancellationTokenSource cancellationTokenSource);
//        public void StartWaitJobDone(string message, CancellationTokenSource cancellationTokenSource)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new StartWaitJobDoneDelegate(StartWaitJobDone), message, cancellationTokenSource);
//                return;
//            }

//            if (waitJobDone)
//                return;
            
//            waitJobDone = true;
//            waitJobDoneTask = Task.Factory.StartNew(() =>
//             {
//                 SimpleProgressForm form = new SimpleProgressForm(message);
//                 form.Show(() =>
//                 {
//                     while (waitJobDone)
//                     {
//                         Thread.Sleep(10);
//                     }
//                     Thread.Sleep(0);
//                 }, cancellationTokenSource);
//             });
//        }

//        public delegate void EndWaitJobDoneDelegate();
//        public void EndWaitJobDone()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new EndWaitJobDoneDelegate(EndWaitJobDone));
//                return;
//            }

//            waitJobDone = false;
//            waitJobDoneTask.Wait();
//            waitJobDoneTask = null;
//        }

//        //public bool WaitJobDone(string message, int timeoutMs = -1)
//        //{
//        //    cancellationTokenSource = new CancellationTokenSource();

//        //    bool jobDone = true;
//        //    bool isTimeout = false;
//        //    SimpleProgressForm jobWaitForm = new SimpleProgressForm(message);
//        //    jobWaitForm.Show(new Action(() =>
//        //    {
//        //        Stopwatch sw = new Stopwatch();
//        //        sw.Start();
//        //        do
//        //        {
//        //            jobDone = IsJobDone();

//        //            if (timeoutMs >= 0 && sw.ElapsedMilliseconds > timeoutMs)
//        //            {
//        //                isTimeout = true;
//        //                break;
//        //             //   throw new TimeoutException();
//        //            }

//        //            if (cancellationTokenSource.IsCancellationRequested)
//        //                break;

//        //            Thread.Sleep(10);

//        //        } while (jobDone == false);

//        //    }), cancellationTokenSource);

//        //    if(isTimeout)
//        //        throw new TimeoutException();

//        //    cancellationTokenSource.Token.ThrowIfCancellationRequested();

//        //    return IsJobSuccess();
//        //}

//        //public bool IsJobDone()
//        //{
//        //    List<Client> clientList = (SystemManager.Instance().MachineIf as MonitoringServer).ClientList;
//        //    bool OnWorking = clientList.Exists(f => ((f.State != ClientState.Disconnected) && (f.JobDone.IsDone() == false)));
//        //    return OnWorking == false;
//        //}

//        //public bool IsJobSuccess()
//        //{
//        //    bool isNotSuccess = (SystemManager.Instance().MachineIf as MonitoringServer).ClientList.Exists(f => ((f.State != ClientState.Disconnected) && (f.JobDone.IsSuccess() == false)));
//        //    return isNotSuccess == false;
//        //}

//        public void ExitTeachJob(Client client)
//        {
//            remoteTeachingPage?.ExitTeachJob(client);
//        }

//        public void ClientDisconnected(Client client)
//        {
//            UpdateClientStateLamp(client);
//        }

//        public void ClientConnected(Client client)
//        {
//            UpdateClientStateLamp(client);
//        }

//        private void labelUserName_Click(object sender, EventArgs e)
//        {
//            //LogInForm loginForm = new LogInForm();
//            //if (loginForm.ShowDialog() == DialogResult.OK)
//            //{
//            //    UserHandler.Instance().CurrentUser = loginForm.LogInUser;

//            //    if (UniScanGSettings.Instance().SystemType == SystemType.Monitor)
//            //        (SystemManager.Instance().MachineIf as MonitoringServer).SendUserChange();
//            //}
//        }

//        delegate void EnableTabsDelegate(bool enable);
//        public void EnableTabs(bool enable)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new EnableTabsDelegate(EnableTabs), enable);
//                return;
//            }

//            foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabControlMain.Tabs)
//            {
//                tab.Enabled = enable;
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
            
//            if (UserHandler.Instance().CurrentUser.Id == "op")
//            {
//                labelUserName.Text = "operator";
//                if (tabControlMain.SelectedTab != null)
//                {
//                    if (tabControlMain.SelectedTab.Key == "Setting")
//                    {
//                        tabControlMain.SelectedTab = tabControlMain.Tabs["Model"];
//                    }
//                }
//            }
//            else
//            {
//                labelUserName.Text = UserHandler.Instance().CurrentUser.Id;
//            }

//            tabControlMain.Tabs["Setting"].Visible = UserHandler.Instance().CurrentUser.SuperAccount;
//        }

//        delegate void LoadTransferSettingsDelegate();
//        public void LoadTransferSettings()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new LoadTransferSettingsDelegate(LoadTransferSettings));
//                return;
//            }
//            settingPage.LoadSettings();
//        }
        
//        delegate void StartClientDelegatge(Client client);
//        public void StartClient(Client client)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new StartClientDelegatge(StartClient),client);
//                return;
//            }

//            if (SystemState.Instance().OnInspectOrWaitOrPause)
//            {
//                MonitoringPage.StopInspection();
//            }

//            SystemManager.Instance().CloseModel(); 
//            //CheckTraind();
//            TabChange("Model");
//        }
        

//        delegate void UpdateMachineStateDelegate(bool isRun);
//        public void UpdateMachineState(bool isRun)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateMachineStateDelegate(UpdateMachineState), isRun);
//                return;
//            }

//            if (isRun == true)
//            {
//                labelMachineState.Text = "Run";
//                labelMachineState.ForeColor = Color.Black;
//                labelMachineState.BackColor = Color.LightGreen;
//            }
//            else
//            {
//                labelMachineState.Text = "Stop";
//                labelMachineState.ForeColor = Color.White;
//                labelMachineState.BackColor = Color.Red;
//            }
//        }

//        public void UpdateControl(string item, string state)
//        {
//            UltraLabel label = null;
//            switch (item)
//            {
//                case "Light":
//                    label = statusLight;
//                    break;
//                case "Fan":
//                    label = statusFan;
//                    break;
//                case "Door":
//                    label = statusDoor;
//                    break;
//            }

//            if(label != null)
//                UpdateStateLabel(label, item, state.ToLower() == "on");
//        }

//        public delegate void UpdateStateLabelDelegate(Infragistics.Win.Misc.UltraLabel label, string item, bool state);
//        private void UpdateStateLabel(UltraLabel label, string item, bool state)
//        {
//            if(InvokeRequired)
//            {
//                Invoke(new UpdateStateLabelDelegate(UpdateStateLabel), label, item, state);
//                return;
//            }
//            //label.Text = item + (state ? " ON" : " OFF");
//            label.Text = item;
//            label.Appearance.BackColor = state ? Color.LightGreen : Color.Magenta;
//        }

//        private void statusLight_Click(object sender, EventArgs e)
//        {
//            IoPort roomLightIoPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanG.Device.IoPortName.OutRoomLight);
//            ToggleIoState(roomLightIoPort);
//        }

//        private void statusFan_Click(object sender, EventArgs e)
//        {
//            IoPort airFanIoPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanG.Device.IoPortName.OutAirFan);
//            ToggleIoState(airFanIoPort);
//        }

//        private void ToggleIoState(IoPort ioPort)
//        {
//            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

//            bool curState = digitalIoHandler.ReadOutput(ioPort);
//            digitalIoHandler.WriteOutput(ioPort, !curState);
//        }

//        private void statusDoor_Click(object sender, EventArgs e)
//        {
//            if (SystemManager.Instance().DeviceBox.MotionList?.Count != 0)
//            {
//                if (SystemManager.Instance().DeviceBox.MotionList[0]?.NumAxis > 0)
//                {
//                    if (SystemManager.Instance().DeviceBox.MotionList[0]?.IsMoveDone(0) == false)
//                    {
//                        MessageForm.Show(this, "Please, Stop the Motion.", "UniEye");
//                        return;
//                    }
//                }
//            }

//            IoPort doorIoPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanG.Device.IoPortName.OutDoorOpen);
//            ToggleIoState(doorIoPort);
//        }
        
//        //private bool GetDoorState()
//        //{
//        //    List<IoPort> ioPortList = new List<IoPort>();
//        //    SystemManager.Instance().DeviceBox.PortMap.GetInDoorPorts(ioPortList);
//        //    foreach(IoPort ioPort in ioPortList)
//        //    {
//        //        if (SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput().GetValue(ioPort) == false)
//        //            return false;
//        //    }
//        //    return true;
//        //}

//        public void UpdateRemoteStepData(string xmlPath, int stepNo)
//        {
//            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
//            xmlDoc.Load(xmlPath);
//            System.Xml.XmlElement xmlElement = xmlDoc.DocumentElement;

//            ModelReader modelReader = ModelReaderBuilder.Create(xmlPath);
//            UniEye.Base.Settings.MachineSettings machineSettings = UniEye.Base.Settings.MachineSettings.Instance();
//            modelReader.Initialize(SystemManager.Instance().AlgorithmArchiver, 1, machineSettings.NumCamera, machineSettings.NumLight, machineSettings.NumLightType);

//            Model tempModel = new Model();
//            modelReader.Load(tempModel, xmlPath, null);

//            Model curModel = SystemManager.Instance().CurrentModel;
//            for (int i = 0; i < tempModel.InspectionStepList.Count; i++)
//            {
//                if (i == stepNo)
//                {
//                    curModel.InspectionStepList[stepNo].Clear();
//                    curModel.InspectionStepList[stepNo] = tempModel.InspectionStepList[stepNo];
//                }
//                else
//                {
//                    tempModel.InspectionStepList[i].Clear();
//                }
//            }
//            tempModel.InspectionStepList.Clear();
//            tempModel.CloseModel();

//            SystemManager.Instance().ModelManager.SaveModel(curModel);
//        }

//        public delegate void UpdateStatusPanelDelegate(int id, string text);
//        public void UpdateStatusPanel(int pos, string text)
//        {
//            if(InvokeRequired)
//            {
//                BeginInvoke(new UpdateStatusPanelDelegate(UpdateStatusPanel), pos, text);
//                return;
//            }

//            string str;
//            switch(pos)
//            {
//                case 0:
//                    str = "Grab Buffer";
//                    break;
//                case 1:
//                    str = "Process Buffer";
//                    break;
//                case 2:
//                    str = "Detect Buffer";
//                    break;
//                case 3:
//                    str = "Save Buffer";
//                    break;
//                default:
//                    throw new DynMvp.Base.InvalidDataException();
//            }

//            ultraStatusBar2.Panels[pos].Text = string.Format("{0}: {1}", str, text);
//        }

//        public void WriteTimeLog(string stepName, int sequenceNo, long spandTimeMs)
//        {
//            string directory = Path.Combine(PathSettings.Instance().Temp, "TimeLog");
//            Directory.CreateDirectory(directory);

//            string temeLog = Path.Combine(directory, string.Format("{0}.txt", stepName));
//            float memGb = GC.GetTotalMemory(false) / 1024f / 1024f / 1024f;

//            if (File.Exists(temeLog) == false)
//            {
//                StreamWriter sw = new StreamWriter(temeLog, true);
//                sw.WriteLine("Name Date Time Milisec SheetNo SpendTImeMs MemoryGB");
//                sw.Close();
//            }

//            lock (updateTimeLogLockObject)
//            {
//                StreamWriter sw = new StreamWriter(temeLog, true);
//                sw.WriteLine(string.Format("{0} {1} {2} {3} {4:F4}", stepName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), sequenceNo, spandTimeMs, memGb));
//                sw.Close();
//            }
//        }

//        //public void WriteTimeLog(string stepName, InspectionResult inspResult, long spandTimeMs)
//        //{
//        //    string temeLog = Path.Combine(PathSettings.Instance().Temp, "TimeLog.txt");
//        //    float memGb = GC.GetTotalMemory(false) / 1024f / 1024f / 1024f;

//        //    lock (updateTimeLogLockObject)
//        //    {
//        //        StreamWriter sw = new StreamWriter(temeLog, true);
//        //        sw.WriteLine(string.Format("{0}\t{1}\tSheetNo {2}\t{3}\t{4:F4}", stepName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), inspResult.InspectionNo, spandTimeMs, memGb));
//        //        sw.Close();
//        //    }
//        //}

//        public void StartCommTest()
//        {
//            SimpleProgressForm form = new SimpleProgressForm("Comm Test");
//            string[] tabNames = new string[] { "Monitoring", "Model", "Teach" };
//            int tabIndex = 0;


//            string commLogPath = Path.Combine(PathSettings.Instance().Temp, "CommLog");
//            (SystemManager.Instance().MachineIf as MonitoringServer).CommLogPath = commLogPath;
//            if (Directory.Exists(commLogPath))
//                Directory.Delete(commLogPath, true);

//            Directory.CreateDirectory(commLogPath);
//            Thread.Sleep(200);

//            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//            form.Show(() =>
//            {
//                bool test = !cancellationTokenSource.IsCancellationRequested;
//                while (test)
//                {
//                    bool jobDone = false;
//                    this.TabChange(tabNames[tabIndex]);
//                    do
//                    {
//                        Thread.Sleep(200);
//                        jobDone =(SystemManager.Instance().MachineIf as MonitoringServer).ClientList.IsEchoDone();
//                        test = !cancellationTokenSource.IsCancellationRequested;
//                    } while (test == true && jobDone == false);

//                    tabIndex = (tabIndex + 1) % (tabNames.Length);
//                }
//            }, cancellationTokenSource);

//            MessageForm.Show(null, "Test Canceled");
           
//            System.Diagnostics.Process.Start(Path.GetFullPath(commLogPath));
//        }

//        public void Teach()
//        {
//            throw new NotImplementedException();
//        }

//        public void Scan()
//        {
//            throw new NotImplementedException();
//        }

//        public void UpdateControl(string item, object value)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

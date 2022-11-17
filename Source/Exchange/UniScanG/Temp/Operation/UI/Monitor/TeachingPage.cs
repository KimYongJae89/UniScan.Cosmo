//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using UniEye.Base;
//using System.Threading;
//using System.Diagnostics;
//using System.IO;
//using DynMvp.Devices;
//using DynMvp.Devices.Light;
//using System.Timers;
//using DynMvp.Base;
//using DynMvp.UI.Touch;
//using UniEye.Base.UI;
//using DynMvp.Data.Forms;
//using DynMvp.Data;
//using DynMvp.UI;
//using UniScanG.Algorithms;

//namespace UniScanG.Temp
//{
//    public partial class TeachingPage : UserControl, IMainTabPage
//    {
//        VNCManager remoteTeachVNCManager = new VNCManager("Teach");
//        Thread[] processWatchThread = new Thread[4];

//        bool onWaitGrab = false;

//        CancellationTokenSource cancellationTokenSource;

//        Thread TeachedCheckThread = null;

//        private CommandManager commandManager = null;
//        private TargetParamControl[] targetParamControlCamera;
//        private ModellerPageExtender modellerPageExtender;

//        //ITeachingPageController pageController;
//        //public ITeachingPageController PageController
//        //{
//        //    get { return pageController; }
//        //}

//        MachineControlPanel motionControlPanel;
//        public MachineControlPanel MotionControlPanel
//        {
//            get { return motionControlPanel; }
//        }

//        LightControlPanel lightControlPanel;
//        public LightControlPanel LightControlPanel
//        {
//            get { return lightControlPanel; }
//        }

//        public TeachingPage()
//        {
//            InitializeComponent();

//            this.commandManager = new CommandManager();

//            this.targetParamControlCamera = new TargetParamControl[2];

//            for (int i = 0; i < 2; i++)
//            {
//                this.targetParamControlCamera[i] = new TargetParamControl();

//                this.targetParamControlCamera[i].Location = new System.Drawing.Point(96, 95);
//                this.targetParamControlCamera[i].Name = "targetParamControlCamera";
//                this.targetParamControlCamera[i].Size = new System.Drawing.Size(74, 101);
//                this.targetParamControlCamera[i].TabIndex = 0;
//                this.targetParamControlCamera[i].Dock = System.Windows.Forms.DockStyle.Fill;
//                this.targetParamControlCamera[i].ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);
//                this.targetParamControlCamera[i].CommandManager = commandManager;
//                this.targetParamControlCamera[i].VisionParamControl.TabVisibleChange(false, false, true);


//            }
//            this.tabPageCamera1.Controls.Add(this.targetParamControlCamera[0]);
//            this.tabPageCamera2.Controls.Add(this.targetParamControlCamera[1]);

//            motionControlPanel = new MachineControlPanel(SystemManager.Instance().DeviceController.Convayor);
//            motionControlPanel.Dock = DockStyle.Fill;
//            this.panelMotion.Controls.Add(this.motionControlPanel);

//            lightControlPanel = new LightControlPanel();
//            lightControlPanel.Dock = DockStyle.Fill;
//            this.panelLight.Controls.Add(this.lightControlPanel);

//            this.ResumeLayout(false);

//            //pageController = new MonitorTeachingPageController();

//            //modellerPageExtender = new SamsungElectroGravureModellerPageExtender(this);
//            //modellerPageExtender.UpdateImage += GravureUpdateImage;
//            //teachControl = new TeachControl(pageController, teachHandlerProbe, teachBox, null);
//            //pageController = new InspectorTeachingPageController(this, (SamsungElectroGravureModellerPageExtender)modellerPageExtender);

//        }

//        delegate void ParamControl_ValueChangedDelegate(ValueChangedType valueChangedType, bool modified);
//        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new ParamControl_ValueChangedDelegate(ParamControl_ValueChanged), valueChangedType, modified);
//                return;
//            }
//        }

//        public void ExitTeachJob(Client client)
//        {
//            /*if (camVncProcess[clientIndex].HasExited == false)
//                camVncProcess[clientIndex].Kill();*/
//        }

//        public void EnableControls()
//        {
//            throw new NotImplementedException();
//        }

//        private delegate void UpdateModelNameDelegate();
//        private void UpdateModelName()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateModelNameDelegate(UpdateModelName));
//                return;
//            }

//            labelModelName.Text = SystemManager.Instance().CurrentModel.Name;
//        }

//        private delegate void UpdateTargetParamControlDelegate();
//        private void UpdateTargetParamControl()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateTargetParamControlDelegate(UpdateTargetParamControl));
//                return;
//            }

//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //if (curModel == null)
//            //    return;

//            //for (int i = 0; i < 2; i++)
//            //{
//            //    VisionProbe visionProbe = curModel.GetInspectionStep(i).GetTargetGroup(0).GetTarget(1).GetProbe(1) as VisionProbe;

//            //    targetParamControlCamera[i].ClearProbeData();
//            //    targetParamControlCamera[i].SelectProbe(visionProbe);
//            //    targetParamControlCamera[i].UpdateTargetGroupImage(null, 0);
//            //}
//        }

//        private void UpdateData()
//        {
//            //UpdateModelName();
//            //UpdateTargetParamControl();

//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //for (int i = 0; i < 2; i++)
//            //{
//            //    string imageFilePath = curModel.GetImageFilePath(i, 0, 0);
//            //    Bitmap image = null;
//            //    if (File.Exists(imageFilePath))
//            //    {
//            //        try
//            //        {
//            //            FileStream fs = new FileStream(imageFilePath, FileMode.Open);
//            //            image = new Bitmap(fs);
//            //            fs.Close();
//            //        }
//            //        catch (IOException ex)
//            //        {
//            //            LogHelper.Error(LoggerType.Error, string.Format("RemoteTeachingPage::UpdateData - GrabImage Load Fail. {0}", ex.Message));
//            //            image = null;
//            //        }
//            //        UpdateGrabImage(i, new Bitmap(image));
//            //    }
//            //    else
//            //    {
//            //        UpdateGrabImage(i, image);
//            //    }
//            //    UpdateTeachImage(i);
//            //}

//        }
//        public void TabPageVisibleChanged(bool visibleFlag)
//        {
//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //if (curModel == null)
//            //    return;

//            //if (visibleFlag == true)
//            //{
//            //    UpdateData();

//            //    if (SystemManager.Instance().CurrentModel.LightParamSet.LightParamList.Count > 0)
//            //        SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);
//            //}
//            //else
//            //{
//            //    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
//            //    if (curModel != null && curModel.IsEmpty() == false)
//            //    {
//            //        SystemManager.Instance().ModelManager.SaveModel(curModel);
//            //    }
//            //}
//        }

//        private void buttonLight_Click(object sender, EventArgs e)
//        {
//            List<LightParam> lightParamList = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList;

//            if (lightParamList == null)
//                return;

//            if (lightParamList.Count == 0)
//                return;

//            UniScanGLightParamForm lightParamForm = new UniScanGLightParamForm();

//            lightParamForm.Initialize(SystemManager.Instance().DeviceBox.LightCtrlHandler, lightParamList[0]);
//            lightParamForm.ShowDialog(Parent);
//        }

//        public void TeachDone(int camIndex, int clientNo, string xmlPath)
//        {
//            if (string.IsNullOrEmpty(xmlPath))
//                return;

//            InspectorInfo targetInfo = UniScanGSettings.Instance().ClientInfoList.Find(f => f.CamIndex == camIndex && f.ClientIndex == clientNo);
//            if (targetInfo == null)
//                return;

//            string absXmlPath = Path.Combine(targetInfo.Path, xmlPath);

//            try
//            {
//                ((MainForm)SystemManager.Instance().MainForm).UpdateRemoteStepData(absXmlPath, camIndex);

//                UpdateTargetParamControl();

//                UpdateTeachImage(targetInfo.CamIndex);
//            }
//            catch (UnauthorizedAccessException ex)
//            {

//            }
//            catch (FileNotFoundException ex)
//            {

//            }

//        }

//        private void buttonMotion_Click(object sender, EventArgs e)
//        {

//        }

//        private void toolStripButtonAutoTeach_Click(object sender, EventArgs e)
//        {
//            bool saveOk = SaveModel();
//            if (saveOk == false)
//            {
//                MessageForm.Show(null, "Save Fail.");
//                return;
//            }

//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            if (monitoringServer == null)
//            {
//                MessageForm.Show(null, "Server is not enable. please Restart program.");
//                return;
//            }

//            try
//            {
//                string modelXml = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml");
//                monitoringServer.SendSyncModel(modelXml);

//                bool teachOk = monitoringServer.SendTeach();
                
//                if (teachOk)
//                {
//                    SimpleProgressForm progressForm = new SimpleProgressForm("Update");
//                    progressForm.Show(() =>
//                    {
//                        UniScanGSettings.Instance().ClientInfoList.ForEach(f =>
//                        {
//                            if (f.ClientIndex == 0)
//                                TeachDone(f.CamIndex, f.ClientIndex, modelXml);
//                        });

//                        SystemManager.Instance().CurrentModel.IsTaught();
//                    });
//                }
//                else
//                {
//                    StringBuilder sb = new StringBuilder();
//                    sb.AppendLine("Teach Fail");
//                    monitoringServer.ClientList.ForEach(f =>
//                    {
//                        if (f.ClientIndex == 0)
//                        {
//                            sb.AppendLine(string.Format("Cam{0} - {1}", f.CamIndex, f.IsSuccess() ? "OK" : f.GetMessage()));
//                        }
//                    });
//                    MessageForm.Show(null, sb.ToString());
//                }

//                ((MainForm)SystemManager.Instance().MainForm).UpdateMainTab(true);
//            }
//            catch (OperationCanceledException ex)
//            {
//                monitoringServer.SendStop();
//                MessageForm.Show(null, "Operation Canceled");
//            }
//            finally
//            {

//            }

//        }

//        private void inspectionToolStripButton_Click(object sender, EventArgs e)
//        {
//            if (MessageForm.Show(null, "Test Inspection?", MessageFormType.YesNo) == DialogResult.No)
//                return;

//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            monitoringServer.SendSyncModel();

//            bool ok = monitoringServer.SendTeach_Inspect();
//            if (ok)
//            {
//                SimpleProgressForm form = new SimpleProgressForm("Update");
//                form.Show(() => monitoringServer.ClientList.ForEach(f => this.TeachInspectionDone(f, f.GetMessage())));
//            }
//        }

//        private void toolStripButtonSaveModel_Click(object sender, EventArgs e)
//        {
//            bool ok = SaveModel();
//            if (ok == false)
//                MessageForm.Show(null, "Save Fail.");
//        }

//        private bool SaveModel()
//        {
//            Model curModel = SystemManager.Instance().CurrentModel;
//            if (curModel == null)
//                return false;

//            bool ok = true;
//            SimpleProgressForm form = new SimpleProgressForm("Save");
//            form.Show(() => ok = SystemManager.Instance().ModelManager.SaveModel(curModel));

//            return ok;
//        }

//        private void toolStripButtonExportData_Click(object sender, EventArgs e)
//        {

//        }

//        private void remoteControlToolStripButton_Click(object sender, EventArgs e)
//        {

//        }

//        private void grabProcessToolStripButton_Click(object sender, EventArgs e)
//        {
//            //bool acyncMode = UniScanGSettings.Instance().AsyncMode;
//            //float curSpeed = acyncMode == false ? -1 : ((UniScanG.Operation.Data.ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS;
//            //MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            //if (monitoringServer != null)
//            //{
//            //    try
//            //    {
//            //        SetupGrabProcess();
//            //        bool ok = monitoringServer.SendStartGrab(curSpeed);

//            //        string[] paths = monitoringServer.ClientList.GetMessages();

//            //        monitoringServer.SendStop();

//            //        SimpleProgressForm jobWaitForm = new SimpleProgressForm("Update");
//            //        jobWaitForm.Show(new Action(() =>
//            //        {
//            //            ImageGrabed();

//            //            UpdateGrabImage(monitoringServer.ClientList, paths);
           
//            //        }), cancellationTokenSource);
//            //    }
//            //    catch (OperationCanceledException)
//            //    {
//            //        (SystemManager.Instance().MachineIf as MonitoringServer).SendStop();
//            //        ImageGrabed();
//            //        MessageForm.Show(null, "Operation Canceled");
//            //    }
//            //}
//        }

//        private void UpdateGrabImage(ClientList clientList, string[] paths)
//        {
//            for (int i = 0; i < clientList.Count; i++)
//            {
//                int camIndex = clientList[i].CamIndex;
//                int clientIndex = clientList[i].ClientIndex;
//                if (clientIndex > 0)
//                    continue;

//                if(string.IsNullOrEmpty(paths[i])==false)
//                    this.UpdateGrabImage(camIndex, Path.Combine(clientList[i].ClientInfo.Path, paths[i]));
//            }
//        }

//        private void SetupGrabProcess()
//        {
//            SimpleProgressForm jobWaitForm = new SimpleProgressForm("Prepare...");
//            jobWaitForm.Show(new Action(() =>
//            {
//                int numLight = SystemManager.Instance().CurrentModel.LightParamSet.NumLight;
//                int numLightType = SystemManager.Instance().CurrentModel.LightParamSet.NumLightType;
//                if (numLightType > 0)
//                {
//                    LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;
//                    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
//                }

//                motionControlPanel?.MoveForward();
//            }), cancellationTokenSource);

//        }

//        private void fiducialGrabProcessToolStripButton_Click(object sender, EventArgs e)
//        {
//            bool ok = SaveModel();
//            if (ok == false)
//            {
//                MessageForm.Show(null, "Save Fail.");
//                return;
//            }

//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            try
//            {
//                string modelXml = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml");
//                monitoringServer.SendSyncModel(modelXml);

//                SetupGrabProcess();
//                float curSpeed = (SystemManager.Instance().MainForm as MainForm).QuaryConvSpeed();
//                if (curSpeed < 0)
//                    return;

//                monitoringServer.SendStartGrab(curSpeed, Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml"));

//                string[] paths = monitoringServer.ClientList.GetMessages();

//                monitoringServer.SendStop();

//                SimpleProgressForm jobWaitForm = new SimpleProgressForm("Update");
//                jobWaitForm.Show(new Action(() =>
//                {
//                    ImageGrabed();

//                    UpdateGrabImage(monitoringServer.ClientList, paths);

//                }), cancellationTokenSource);
//            }
//            catch (OperationCanceledException)
//            {
//                monitoringServer.SendStop();
//                ImageGrabed();
//                MessageForm.Show(null, "Operation Canceled");
//            }
//        }

//        delegate void GrabTimerCallBackDelegate(object sender, ElapsedEventArgs e);
//        public void GrabTimerCallBack(object sender, ElapsedEventArgs e)
//        {
//            if (InvokeRequired)
//            {
//                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke GrabTimerCallBack");
//                BeginInvoke(new GrabTimerCallBackDelegate(GrabTimerCallBack), sender, e);
//                return;
//            }

//            if (cancellationTokenSource != null)
//                cancellationTokenSource.Cancel();

//            MessageForm.Show(this.ParentForm, "설비 가동 상태를 확인해 주세요.", "UniEye");
//        }

//        delegate void ImageGrabedDelegate();
//        void ImageGrabed()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new ImageGrabedDelegate(ImageGrabed));
//                return;
//            }

//            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
//            motionControlPanel?.MoveStop();
//            //StopGrab();
//        }

//        void OpenVnc(int camIndex, int clientIndex)
//        {
//            InspectorInfo targetInfo = null;
//            foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
//            {
//                if (info.CamIndex == camIndex && info.ClientIndex == clientIndex)
//                    targetInfo = info;
//            }

//            if (targetInfo == null)
//                return;

//            remoteTeachVNCManager.OpenVNC(0, 0, targetInfo.IpAddress);
//        }

//        private void cam1ARemoteView_Click(object sender, EventArgs e)
//        {
//            OpenVnc(0, 0);
//        }

//        private void cam2BRemoteView_Click(object sender, EventArgs e)
//        {
//            OpenVnc(1, 1);
//        }

//        private void cam1BRemoteView_Click(object sender, EventArgs e)
//        {
//            OpenVnc(0, 1);
//        }

//        private void cam2ARemoteView_Click(object sender, EventArgs e)
//        {
//            OpenVnc(1, 0);
//        }

//        public void UpdateGrabImage(int camIndex, string imagePath)
//        {
//             try
//            {
//                // Copy File
//                string savePath = SystemManager.Instance().CurrentModel.GetImageFilePath(camIndex, 0, 0, "bmp");
//                if (File.Exists(savePath))
//                    File.Delete(savePath);
//                File.Copy(imagePath, savePath);

//                // Open File
//                FileStream fs = new FileStream(savePath, FileMode.Open);
//                Bitmap image = new Bitmap(fs);
//                fs.Close();

//                // Update Control
//                UpdateGrabImage(camIndex, image);
//            }
//            catch (Exception ex)
//            {

//            }

//        }
        
//        delegate void UpdateGrabImageDelegate(int camIndex, Image image);
//        private void UpdateGrabImage(int camIndex, Image image)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateGrabImageDelegate(UpdateGrabImage), camIndex, image);
//                return;
//            }

//            if (camIndex == 0)
//            {
//                cam1Grab.Image = image;
//            }
//            else if (camIndex == 1)
//            {
//                cam2Grab.Image = image;
//            }
//        }

//        private delegate void UpdateTeachImageDelegate(int camIndex);
//        private void UpdateTeachImage(int camIndex)
//        {
//            //if (InvokeRequired)
//            //{
//            //    Invoke(new UpdateTeachImageDelegate(UpdateTeachImage), camIndex);
//            //    return;
//            //}

//            //Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
//            //VisionProbe visionProbe = model.GetVisionProbe(camIndex);
//            //SheetCheckerParam param = visionProbe.InspAlgorithm.Param as SheetCheckerParam;

//            //if (camIndex == 0)
//            //{
//            //    cam1Teach.Image = param.TrainerParam.InspectRegionInfoImage?.ToBitmap();
//            //}
//            //else if (camIndex == 1)
//            //{
//            //    cam2Teach.Image = param.TrainerParam.InspectRegionInfoImage?.ToBitmap();
//            //}
//        }

//        private void cam1SyncParam_Click(object sender, EventArgs e)
//        {
//            SyncParam(0);
//        }

//        private void cam2SyncParam_Click(object sender, EventArgs e)
//        {
//            SyncParam(1);
//        }

//        private void SyncParam(int camIndex)
//        {
//            try
//            {
//                // Get Inspector Info
//                string modelName = SystemManager.Instance().CurrentModel.Name;
//                MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
                
//                Client client = monitoringServer.GetClient(camIndex, 0);
//                if (client == null)
//                    throw new Exception("Client is not founded");

//                InspectorInfo clientInfo = UniScanGSettings.Instance().ClientInfoList.Find(f => f.CamIndex == client.CamIndex && f.ClientIndex == client.ClientIndex);
//                if (client.State == ClientState.Disconnected || clientInfo == null)
//                    return;

//                // Save Inspector Model
//                monitoringServer.SendSave();
//                //((MainForm)ParentForm).WaitJobDone("Save");

//                // Load Inspector Model
//                string xmlPath = Path.Combine(clientInfo.Path, "Model", modelName, "Model.xml");
//                SimpleProgressForm form = new SimpleProgressForm("Update Model");
//                form.Show(new Action(() => ((MainForm)ParentForm).UpdateRemoteStepData(xmlPath, camIndex)));

//                //Save Monitor Model
//                SimpleProgressForm form2 = new SimpleProgressForm("Save Model");
//                form2.Show(new Action(() => SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel)));

//                UpdateData();
//            }
//            catch (OperationCanceledException ex)
//            {
//                MessageForm.Show(null, "Operation Canceled");
//            }
//            catch (Exception ex)
//            {
//                MessageForm.Show(null, ex.Message);
//            }
//        }
        
//        public void TeachInspectionDone(Client client, string imagePath)
//        {
//            InspectorInfo info = client.ClientInfo;
//            if (info == null)
//                return;

//            string rcImage = Path.Combine(info.Path, imagePath);

//            if (File.Exists(rcImage)==false)
//                return;
            
//            Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(rcImage);
//            if (bitmap != null)
//            {
//                ImageD imageD = Image2D.ToImage2D(bitmap);
//                UpdateGrabImage(info.CamIndex, imageD.ToBitmap());

//                //teachBox.DrawBox.ZoomFit();
//            }

//            //defectList.Rows.Clear();
//            //int index = 0;
//            //DirectoryInfo dir = new DirectoryInfo(rcImagePath);
//            //foreach (FileInfo fi in dir.GetFiles())
//            //{
//            //    string defectImagePath = Path.Combine(rcImagePath, string.Format("{0}.jpg", index));

//            //    Bitmap defectBitmap = (Bitmap)ImageHelper.LoadImage(defectImagePath);
//            //    if (defectBitmap != null)
//            //    {
//            //        defectList.Rows.Add("NG", defectBitmap);
//            //    }

//            //    index++;
//            //}
//        }

//        private void toolStripButtonOverlapSet_Click(object sender, EventArgs e)
//        {
//            MonitorInfo monitorInfo = UniScanGSettings.Instance().MonitorInfo;

//            OverlapSettingForm overlapSettingForm = new OverlapSettingForm();
//            overlapSettingForm.Init(cam1Grab.Image, cam2Grab.Image, monitorInfo);
//            overlapSettingForm.ShowDialog();

//            UniScanGSettings.Instance().Save();
//        }
//    }
//}
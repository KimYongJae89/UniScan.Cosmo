using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Settings;
using DynMvp.Device.Serial;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using DynMvp.UI.Touch;
using System.Threading;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.Authentication;
using Infragistics.Win.Misc;
using UniScanM.UI;
using DynMvp.UI;
using UniScanM.Authorize;

namespace UniScanM
{
    public enum StartMode
    {
        Auto, Manual, Stop
    }

    public partial class MainForm : Form, IMainForm, IOpStateListener, IDisposable, IUserHandlerListener, IMultiLanguageSupport
    {
        IInspectionPage inspectionPage;
        public IInspectionPage InspectPage { get { return inspectionPage; } }

        ITeachPage teachPage;
        public ITeachPage TeachPage { get { return teachPage; } }

        IModelManagerPage modelManagerPage;
        public IModelManagerPage ModelManagerPage { get { return modelManagerPage; } }

        IReportPage reportPage;
        public IReportPage ReportPage { get { return reportPage; } }

        ISettingPage settingPage;
        public ISettingPage SettingPage { get { return settingPage; } }

        private UI.RollInfoControl infoControl;
        private UI.PLCStatusPanel plcStatusPanel;

        AlarmMessageForm alarmMessageForm;

        public MainForm()
        {
            InitializeComponent();

#if DEBUG
            //this.FormBorderStyle = FormBorderStyle.Sizable;
#endif
            alarmMessageForm = new AlarmMessageForm();
            alarmMessageForm.Show();

            this.inspectionPage = new UI.InspectionPage();
            this.buttonInspection.Tag = inspectionPage;

            if (SystemManager.Instance().ModelManager != null)
            {
                this.modelManagerPage = new UniScanM.UI.ModelManagerPage();
                this.buttonModelManager.Tag = modelManagerPage;
            }

            this.teachPage = SystemManager.Instance().UiChanger.CreateTeachPage();
            if (teachPage != null)
            this.buttonTeach.Tag = teachPage;

            this.reportPage = new UniScanM.UI.ReportPage();
            this.buttonReport.Tag = reportPage;

            this.settingPage = SystemManager.Instance().UiChanger.CreateSettingPage();
            this.buttonSetting.Tag = settingPage;

            labelTitle.Text = CustomizeSettings.Instance().Title;
            labelTitle.Text = CustomizeSettings.Instance().ProgramTitle;

            //this.modellerPage = new UniScanM.StillImage.UI.MenuPage.ModellerPage();
            //this.modellerPage.ShowHideControl = this.buttonTeach;
            //this.modellerPage.Dock = DockStyle.Fill;
            //this.buttonTeach.Tag = monitoringPage;


            ///COlor
            ////2. Inspecton Page (Right View)
            //inspectionPage = new UniScanM.ColorSens.UI.MenuPage.InspectPage();
            //inspectionPage.Dock = DockStyle.Fill;
            //buttonMonitoring.Tag = inspectionPage;

            ////3. Teach

            ////4. ReportPage
            //reportPage = new UniScanM.ColorSens.UI.MenuPage.ReportPage();
            //reportPage.Dock = DockStyle.Fill;
            //buttonReport.Tag = reportPage;

            ////5. Model
            //modelPage = new UniScanM.ColorSens.UI.MenuPage.ModelManagePage();
            //modelPage.Dock = DockStyle.Fill;
            //buttonModelManager.Tag = modelPage;

            ////6. Setting Page
            //settingPage = new UniScanM.ColorSens.UI.MenuPage.SettingPage();
            //settingPage.Dock = DockStyle.Fill;
            //buttonSetting.Tag = settingPage;

            this.buttonTeach.Visible = this.buttonTeach.Tag != null;
            this.buttonSetting.Visible = this.buttonSetting.Tag != null;

            InitInfoPanel();
            InitStatusBar();

            panelClock.Controls.Add(new ClockControl());

            SystemState.Instance().AddOpListener(this);
            UserHandler.Instance().AddListener(this);
            StringManager.AddListener(this);

            SystemManager.Instance().InspectStarter.OnStartModeChanged += EnableTabs;

            // String Table Add 이후에 Title 변경해야 함.
            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            string version = VersionHelper.Instance().VersionString;
            string buildDateTime = VersionHelper.Instance().BuildString;
            this.Text = string.Format("{0} @ {1}, Version {2} Build {3}", title, copyright, version, buildDateTime);
        }

        void InitInfoPanel()
        {
            infoControl = new UI.RollInfoControl();
            infoControl.Dock = DockStyle.Fill;
            infoControl.BackColor = Color.Transparent;
            panelInfoHeader.Controls.Add(infoControl);
        }
        
        void InitStatusBar()
        {
            plcStatusPanel = new UI.PLCStatusPanel();
            plcStatusPanel.Dock = DockStyle.Fill;
            panelPLCStatus.Controls.Add(plcStatusPanel);
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Screen primaryScreen = System.Windows.Forms.Screen.AllScreens.First(f => f.Primary);
            //this.Location = primaryScreen.WorkingArea.Location;
            this.MaximizedBounds = primaryScreen.WorkingArea;
            this.MinimumSize = primaryScreen.WorkingArea.Size;
            this.WindowState = FormWindowState.Maximized;

            SystemManager.Instance().DeviceBox.LightCtrlHandler?.TurnOff();
            SystemManager.Instance().DeviceController.RobotStage?.StopMove();
            SystemManager.Instance().DeviceController.Convayor?.StopMove();
                                               
            inspectionPage.UpdateControl("updatedata(false)", false);
       
            PageChange(this.InspectPage);
            
            //UniEye.Base.MachineInterface.MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            //if (machineIf != null)
            //{
            //    if (machineIf.IsConnected == false)
            //        ErrorManager.Instance().Report((int)ErrorSection.ExternalIF,(int)ErrorSubSection.CommonReason, ErrorLevel.Error, ErrorSection.ExternalIF.ToString(), "MachineIF", "Machine IF is not connected");
            //}

            SystemManager.Instance().InspectStarter.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "Quit?"), MessageFormType.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            // Quit Code
            OpState opState = SystemState.Instance().GetOpState();
            if (opState != OpState.Idle)
            {
                MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "Inspect is Running"));
                e.Cancel = true;
                return;

            }

            //((Form)SystemManager.Instance().MainForm).Close();
        }

        private void PageButton_Click(object sender, EventArgs e)
        {
            IMainTabPage page = ((UltraButton)sender).Tag as IMainTabPage;
            if (page==null)
            {
                MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "Not Implement Yet."));
                return;
            }

            if (page is ISettingPage || page is ITeachPage)
            {
                User user = AuthorizeHelper.Authorize();
                if (user == null)
                    return;

                if (user.UserType == UserType.Operator)
                {
                    MessageForm.Show(null, StringManager.GetString("Permission is invalid."));
                    return;
                }
                PageChange(page, user.UserType);
                return;
            }

            PageChange(page, UserType.Operator);
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance)
        {
            foreach (IMainTabPage curPage in panelBody.Controls)
            {
                curPage.PageVisibleChanged(false);
            }
                

            panelBody.Controls.Clear();
            panelBody.Controls.Add((UserControl)page);
            page.PageVisibleChanged(true);
            page.EnableControls(userType);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        delegate void EnableTabsDelegate();
        public void EnableTabs()
        {
            if (InvokeRequired)
            {
                Invoke(new EnableTabsDelegate(EnableTabs));
                return;
            }

            this.buttonInspection.Enabled = SystemManager.Instance().ModelManager == null || SystemManager.Instance().CurrentModel != null;
            this.buttonModelManager.Enabled = SystemState.Instance().InspectState == InspectState.Ready && SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop;
            this.buttonTeach.Enabled = SystemManager.Instance().CurrentModel != null && SystemState.Instance().InspectState == InspectState.Ready && SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop;
            //this.buttonReport.Enabled = SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop;
            this.buttonExit.Enabled = SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop;
            //this.buttonSetting.Enabled = SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop;
        } 
        
        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if(InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            EnableTabs();

            if (prevOpState == OpState.Idle && curOpState == OpState.Wait)
                PageChange(this.InspectPage);
        }

        public void UserChanged()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            } 
        }

        /// <summary>
        /// 모델이 바뀔 때 호출됨. UI 출력
        /// </summary>
        public void OnModelChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new OnModelChangedDelegate(OnModelChanged));
                return;
            }

            EnableTabs();
        }

        /// <summary>
        /// Lot가 바뀔 때 호출됨. UI출력
        /// </summary>
        public void OnLotChanged()
        {
            if(InvokeRequired)
            {
                Invoke(new OnLotChangedDelegate(OnLotChanged));
                return;
            }

            //this.curModel.Text = ProductionManager.Instance().CurProduction?.Name;
            //this.curLotNo.Text = ProductionManager.Instance().CurProduction?.LotNo;
        }

        public void WorkerChanged(string opName)
        {
            if (InvokeRequired)
            {
                Invoke(new OnWorkerChangedDelegate(WorkerChanged), opName);
                return;
            }
        }


        public void ModifyTeaching(string imageFileName)
        {
            throw new NotImplementedException();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            labelTitle.Text = CustomizeSettings.Instance().ProgramTitle;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            inspectionPage.UpdateControl("updatedata(true)", true);

            SystemManager.Instance().InspectStarter.Dispose();
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
            SystemManager.Instance().InspectRunner.Dispose();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //#if DEBUG == false
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            //#endif
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Normal)
            //    this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Normal)
            //    this.WindowState = FormWindowState.Maximized;
        }
    }
}
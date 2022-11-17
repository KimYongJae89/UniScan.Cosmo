using System;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using UniEye.Base;
using UniEye.Base.Device;
using UniEye.Base.Data;
using System.Drawing;
using System.Threading;
using DynMvp.Authentication;
using Infragistics.Win.UltraWinTabControl;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Util;
using UniScanG.UI.Etc;
using UniScan.Common.Data;
using System.Collections.Generic;
using System.IO;
using UniScan.Common.Settings;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Linq;

namespace UniScanG.UI
{
    public enum MainTabKey
    {
        Inspect, Model, Teach, Report, Setting, Log, Exit
    }

    public partial class MonitorMainform : Form, IMainForm, IMultiLanguageSupport
    {
        List<IStatusStripPanel> statusPanelList = new List<IStatusStripPanel>();
        AlarmMessageForm alarmMessageForm = new AlarmMessageForm();

        public MonitorMainform(IUiControlPanel mainTabPanel, string title)
        {
            InitializeComponent();
            this.labelTitle.Text = title;
            StringManager.AddListener(this);
            //UpdateLanguage();

            if (mainTabPanel != null)
                this.mainPanel.Controls.Add((Control)mainTabPanel);

            userPanel.Controls.Add(new UserPanel());
            modelPanel.Controls.Add(new ModelPanel());
            InitStatusStrip();

            timer.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            this.FormBorderStyle = FormBorderStyle.Sizable;
#endif

            alarmMessageForm.Show();

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            if (screen == null)
                screen = System.Windows.Forms.Screen.AllScreens.First();

            this.MaximizedBounds = screen.WorkingArea;
            this.WindowState = FormWindowState.Maximized;

            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            this.Text = string.Format("{0} @ {1}, Version {2} Build {3}", title, copyright, VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        ComboBox resultDataHandlerLog;
        public void InitStatusStrip()
        {
            this.resultDataHandlerLog = new ComboBox();
            resultDataHandlerLog.DropDownStyle = ComboBoxStyle.DropDownList;
            resultDataHandlerLog.Dock = DockStyle.Fill;
            stripPanel.Controls.Add(resultDataHandlerLog);

            List<IStatusStripPanel> statusStripList = SystemManager.Instance().UiChanger.GetStatusStrip();
            stripPanel.Controls.AddRange(statusStripList.ConvertAll(f => (Control)f).ToArray());
            statusPanelList.AddRange(statusStripList);

            DataManagerLockItem.OnWork = DataManagerLockItem_OnWork;
        }

        private void DataManagerLockItem_OnWork(string message)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new DataManagerLockItemWork(DataManagerLockItem_OnWork), message);
                return;
            }
            resultDataHandlerLog.SelectedIndex = resultDataHandlerLog.Items.Add(message);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            dateLabel.Text = curTime.ToString("yyyy - MM - dd");
            timeLabel.Text = curTime.ToString("HH : mm : ss");

            statusPanelList.ForEach(s => s.StateUpdate());
        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
        }
        
        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {

        }

        public IInspectionPage MonitoringPage { get { return null; } }

        public IReportPage ReportPage { get { throw new NotImplementedException(); } }

        public ISettingPage SettingPage { get { throw new NotImplementedException(); } }
        
        public IInspectionPage InspectPage => throw new NotImplementedException();

        IModelManagerPage IMainForm.ModelManagerPage => throw new NotImplementedException();

        ITeachPage IMainForm.TeachPage => throw new NotImplementedException();

        public void EnableTabs() { }
        public void Teach() { }
        public void Scan() { }
        public void UpdateControl(string item, object value) { }
        public void ModifyTeaching(string imagePath) { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            // 뭐 없네
        }

        private void gtcLogoPanel_Paint(object sender, PaintEventArgs e)
        {
       
        }

        private void gtcLogoPanel_Click(object sender, EventArgs e)
        {
        }

        public void PageChange(IMainTabPage page)
        {
            throw new NotImplementedException();
        }

        public void OnModelChanged()
        {

        }

        public void OnLotChanged()
        {

        }

        public void WorkerChanged(string OpName)
        {
            throw new NotImplementedException();
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance) { }
    }
}

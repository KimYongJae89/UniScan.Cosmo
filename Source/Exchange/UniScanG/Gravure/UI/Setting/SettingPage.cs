using System;
using System.Windows.Forms;
using DynMvp.Authentication;
using DynMvp.Base;
using Infragistics.Win.UltraWinTabControl;
using UniEye.Base.UI;
using UniScanG.Gravure.Vision;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage, IUserHandlerListener, IMultiLanguageSupport
    {
        enum SettingMenu
        {
            Classification, DataRemove, IO
        }

        SettingAlarmPage settingAlarmPage;
        SettingCommPage settingCommPage;
        SettingGeneralPage settingGeneralPage;

        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.settingAlarmPage = new SettingAlarmPage();
            this.ultraTabPageAlarm.Controls.Add(settingAlarmPage);

            this.settingGeneralPage = new SettingGeneralPage();
            this.ultraTabPageMonitoring.Controls.Add(settingGeneralPage);

            this.settingCommPage = new SettingCommPage();
            this.ultraTabPageComm.Controls.Add(settingCommPage);

            UserHandler.Instance().AddListener(this);
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            foreach(UltraTab ultraTab in this.settingTabControl.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Text);
        }

        public void Initialize()
        {
        }
        
        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {   
            if (visibleFlag == true)
            {
                UpdateData();
            }
        }

        public void UpdateData()
        {
        }

        public void SetData()
        {

        }
        
        public void LoadSettings()
        {

        }

        public void SaveSettings()
        {

        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            
            //settingTabControl.Tabs[ultraTabPageMonitoring.Tab.Key].Visible = curUser.Id.ToUpper() == "DEVELOPER";
        }

        private void tabControlParam_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            this.settingAlarmPage.UpdateData();
            this.settingCommPage.UpdateData();
            this.settingGeneralPage.UpdateData();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.settingAlarmPage.Initialize();
            this.settingGeneralPage.Initialize();
            this.settingCommPage.Initialize();
        }

        private void SettingPage_Load(object sender, EventArgs e)
        {

        }

        private void settingTabControl_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
       
        }
    }
}

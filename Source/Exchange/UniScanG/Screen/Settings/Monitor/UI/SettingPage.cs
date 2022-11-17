using System;
using System.Windows.Forms;
using DynMvp.Authentication;
using UniEye.Base.UI;

namespace UniScanG.Screen.Settings.Monitor.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage, IUserHandlerListener
    {
        enum SettingMenu
        {
            Classification, DataRemove, IO
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            groupDataRemove.Visible = false;
            groupIO.Visible = false;
            groupClassification.Visible = false;

            UserHandler.Instance().AddListener(this);
        }

        public void Initialize()
        {

        }

        private void buttonSelectLocalFolder_Click(object sender, EventArgs e)
        {
        //    FolderBrowserDialog dlg = new FolderBrowserDialog();
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        localResultPath.Text = dlg.SelectedPath;
        //    }
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
        //    if (visibleFlag == true)
        //    {
        //        LoadSettings();
        //    }
        //    else
        //    {
        //        SaveSettings();
        //        if (CustomSettings.Instance().SystemType == SystemType.Monitor)
        //        {
        //            ((MpisMonitorSystemManager)SystemManager.Instance()).MonitoringServer.SendSyncSetting();
        //            ((Operation.UI.MLCCSMainForm)this.ParentForm).WaitJobDone("Inspector Sync..");
        //        }
        //    }
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

        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            
            settingTabControl.Tabs[tabPageDeveloper.Tab.Key].Visible = curUser.Id.ToUpper() == "DEVELOPER";
        }

        private void tabControlParam_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }

        private void storingDays_ValueChanged(object sender, EventArgs e)
        {
        }

        private void startYPosition_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonCalibration_Click(object sender, EventArgs e)
        {
            //CameraCalibrationForm form = new CameraCalibrationForm();
            //form.Initialize();
            //form.ShowDialog();

            //cameraResolution.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width.ToString("0.000 [um/px]");
        }

        private void monitoringSettingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = monitoringSettingList.SelectedItem.ToString();

            if (string.IsNullOrEmpty(selectedItem) == true)
                return;

            selectedItem = selectedItem.Replace(" ", "");

            SettingMenu menu = (SettingMenu)Enum.Parse(typeof(SettingMenu), selectedItem);

            switch (menu)
            {
                case SettingMenu.Classification:
                    groupDataRemove.Visible = false;
                    groupIO.Visible = false;
                    groupClassification.Visible = true;
                    break;
                case SettingMenu.IO:
                    groupDataRemove.Visible = false;
                    groupIO.Visible = true;
                    groupClassification.Visible = false;
                    break;
                case SettingMenu.DataRemove:
                    groupDataRemove.Visible = true;
                    groupIO.Visible = false;
                    groupClassification.Visible = false;
                    break;
            }
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }
    }
}

using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;

namespace UniScanG.UI.Teach.Monitor
{
    public partial class TeachPage : UserControl, IMainTabPage, IVncContainer, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        List<IVncControl> vncButtonList = new List<IVncControl>();
        int lightOnCount = 0;

        IServerExchangeOperator server;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public TeachPage()
        {
            InitializeComponent();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            MonitorUiChanger monitorUiChanger = (MonitorUiChanger)SystemManager.Instance().UiChanger;

            List<Control> buttonList = monitorUiChanger.GetTeachButtons(this);
            foreach (Control button in buttonList)
            {
                if (button is IVncControl)
                {
                    IVncControl vncControl = button as IVncControl;
                    vncControl.InitHandle(remoteTeachingPanel.Handle);
                    vncButtonList.Add(vncControl);
                }

                if(button is LightSettingButton)
                {
                    LightSettingButton lightSettingButton = button as LightSettingButton;
                    //lightSettingButton.
                }
            }
            buttonPanel.Controls.AddRange(buttonList.ToArray());

            settingPanel.Controls.Add(monitorUiChanger.CreateTeachSettingPanel());
            settingPanel.Visible = settingPanel.Controls.Count > 0;

            StringManager.AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
        }

        public void TabPageVisibleChanged(bool visibleFlag) { }

        public void EnableControls(UserType user) { }

        public void ProcessStarted(IVncControl startVncButton)
        {
            vncButtonList.FindAll(f => !f.Equals(startVncButton)).ForEach(f => f.Disable());
            //foreach (IVncControl vncButton in vncButtonList)
            //{
            //    if (vncButton != startVncButton)
            //        vncButton.Disable();
            //}

            SystemManager.Instance().DeviceController.OnEnterWaitInspection();
        }

        public void ProcessExited()
        {
            new DynMvp.UI.Touch.SimpleProgressForm().Show(() =>
            {
                vncButtonList.ForEach(f =>
                {
                    f.ExitProcess();
                    f.Enable();
                });

                SystemManager.Instance().DeviceController.OnExitWaitInspection();
                SystemManager.Instance().ExchangeOperator.SaveModel();
                this.ModelTeachDone(-1);
            });
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }

        public void ModelChanged()
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;
;
            
            Bitmap image = SheetCombiner.CreateModelImage(SystemManager.Instance().CurrentModel.ModelDescription);
            if (image != null)
                UpdateImage(image);
        }

        public void ModelTeachDone(int camId)
        {
            if (camId < 0)
            {
                string imagePath = SystemManager.Instance().ModelManager.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription);
                if (File.Exists(imagePath))
                {
                    Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(imagePath);
                    UpdateImage(bitmap);
                }
            }
        }
        public void ModelRefreshed() { }

        delegate void UpdateImageDelegate(Bitmap image);
        private void UpdateImage(Bitmap image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateImageDelegate(UpdateImage), image);
                return;
            }

            prevImage.Image = image;
            prevImage.Invalidate();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            List<Control> list = vncButtonList.FindAll(f => f.GetInspector().Info.ClientIndex > 0).ConvertAll<Control>(g => (Control)g);
            list.ForEach(f => f.Visible = UserHandler.Instance().CurrentUser.SuperAccount);
        }
    }
}

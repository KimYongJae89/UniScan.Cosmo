using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;
using UniScanG.Data.UI;
using UniScanG.Gravure.Inspect;
using UniScanG.Inspect;

namespace UniScanG.UI.Inspect
{
    public partial class InspectPage : UserControl, IMainTabPage, IVncContainer, IOpStateListener, IMultiLanguageSupport, IUserHandlerListener
    {
        public IInspectDefectPanel InspectDefectPanel
        {
            get { return inspectDefectPanel; }
        }

        RepeatedDefectAlarmForm repeatedDefectAlarmForm;
        IInspectDefectPanel inspectDefectPanel;
        List<IVncControl> vncButtonList = new List<IVncControl>();
        IServerExchangeOperator server;
        InspectStarter inspectStarter;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            //server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            //MonitorUiChanger monitorUiChanger = (MonitorUiChanger)SystemManager.Instance().UiChanger;

            //List<Control> buttonList = monitorUiChanger.GetInspectButtons(this);            
            //foreach (Control button in buttonList)
            //{
            //    buttonPanel.Controls.Add(button);
            //    if (button is IVncControl)
            //        vncButtonList.Add((IVncControl)button);
            //}

            //foreach (IVncControl vncButton in vncButtonList)
            //    vncButton.InitHandle(layoutInspect.Handle);

            //// Vnc Post
            ImagePanel imageViewPanel = new ImagePanel();
            imagePanel.Controls.Add(imageViewPanel);

            inspectDefectPanel = SystemManager.Instance().UiChanger.CreateDefectPanel();
            inspectDefectPanel.AddDelegate(imageViewPanel.UpdateResult);

            defectPanel.Controls.Add((Control)inspectDefectPanel);
            panelInfo.Controls.Add((Control)SystemManager.Instance().UiChanger.CreateDefectInfoPanel());
            //resultPanel.Controls.Add(new ResultPanel());

            UpdateBlockStateChange(SystemManager.Instance().InspectRunner is InspectRunnerInspectorG);

            repeatedDefectAlarmForm = new RepeatedDefectAlarmForm();
            //repeatedDefectAlarmForm.WindowState = FormWindowState.Minimized;
            repeatedDefectAlarmForm.Show();
            repeatedDefectAlarmForm.Hide();
            //repeatedDefectAlarmForm.WindowState = FormWindowState.Normal;

            this.inspectStarter = (SystemManager.Instance().InspectRunner as UniScanG.Inspect.InspectRunner)?.InspectStarter;

            //SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(repeatedDefectAlarmForm.InspectDone);

            UserHandler.Instance().AddListener(this);
            SystemState.Instance().AddOpListener(this);

            buttonBlinkTimer.Start();
        }

        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            this.repeatedDefectAlarmForm?.Clear();

            System.Threading.CancellationTokenSource ss = new System.Threading.CancellationTokenSource();
            //ss.CancelAfter(2000);
            bool ok = this.inspectStarter.EnterWaitInspection(true, ss.Token);// new System.Threading.CancellationToken());
            if (ok)
                UpdateBlockStateChange(false);
        }

        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {

            }
            else
            {
                ProcessExited();
            }
        }
        
        public void ProcessStarted(IVncControl startVncButton)
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                if (vncButton != startVncButton)
                    vncButton.Disable();
            }
            SystemManager.Instance().DeviceController.OnEnterWaitInspection();
        }

        public void ProcessExited()
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                vncButton.ExitProcess();
                vncButton.Enable();
            }
            SystemManager.Instance().DeviceController.OnExitWaitInspection();
        }

        public void UpdateControl(string item, object value)
        {
            throw new System.NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            
        }

        delegate void OpStatusChangedDelegate(OpState curOpState, OpState prevOpState);
        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OpStatusChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            switch (curOpState)
            {
                case OpState.Idle:
                    buttonStart.Visible = true;
                    buttonPause.Visible = false;
                    buttonStop.Visible = false;
                    break;
                case OpState.Inspect:
                case OpState.Alarm:
                    buttonStart.Visible = false;
                    buttonPause.Visible = false;
                    buttonStop.Visible = true;
                    break;
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
        }

        private void buttonSplitter_Click(object sender, System.EventArgs e)
        {
            defectPanel.Visible = !defectPanel.Visible;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //StringManager.UpdateString(this.GetType().FullName, buttonStart);
            //StringManager.UpdateString(this.GetType().FullName, buttonPause);
            //StringManager.UpdateString(this.GetType().FullName, buttonStop);
            //StringManager.UpdateString(this.GetType().FullName, buttonLot);
            //StringManager.UpdateString(this.GetType().FullName, buttonReset);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            inspectDefectPanel.Reset();
            this.repeatedDefectAlarmForm.Clear();
        }

        private void ultraButtonUpdate_Click(object sender, EventArgs e)
        {
            UpdateBlockStateChange(!this.inspectDefectPanel.BlockUpdate);
        }

        private void UpdateBlockStateChange(bool v)
        {
            this.inspectDefectPanel.BlockUpdate = v;
            if (this.inspectDefectPanel.BlockUpdate)
                ultraButtonUpdate.Appearance.BackColor = Color.Red;
            else
                ultraButtonUpdate.Appearance.BackColor = Color.LightGreen;
        }

        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            ultraButtonObserver.Visible = UserHandler.Instance().CurrentUser.SuperAccount;

            //Form form = ((UniScanG.Inspect.InspectRunner)SystemManager.Instance().InspectRunner).InspectObserver as Form;
            //if (form != null)
            //{
            //    if (UserHandler.Instance().CurrentUser.SuperAccount)
            //        form.Show();
            //    else
            //        form.Hide();
            //}
        }

        private void ultraButtonObserver_Click(object sender, EventArgs e)
        {
            Form form = ((UniScanG.Inspect.InspectRunner)SystemManager.Instance().InspectRunner).InspectObserver as Form;
            if (form == null)
                return;

            if (form.Visible == false)
                form.Show();
            else
                form.Hide();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.EnterPauseInspection();
        }

        private void ultraButtonAlarm_Click(object sender, EventArgs e)
        {
            repeatedDefectAlarmForm.Show();
        }

        private void buttonBlinkTimer_Tick(object sender, EventArgs e)
        {
            if (repeatedDefectAlarmForm != null && repeatedDefectAlarmForm.IsAlarmState)
            {
                ultraButtonAlarm.Appearance.BackColor = (DateTime.Now.Millisecond / 500) == 0 ? Colors.Alarm : Colors.Normal;
                //ultraButtonAlarm.Enabled = true;
            }
            else
            {
                ultraButtonAlarm.Appearance.BackColor = Colors.Normal;
                //ultraButtonAlarm.Enabled = false;
            }
        }
    }
}
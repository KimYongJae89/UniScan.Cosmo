using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Util;
using System.Drawing;
using DynMvp.UI.Touch;
using System.Threading;
using System.Collections.Generic;
using DynMvp.Base;
using DynMvp.UI;
using UniEye.Base.Data;

namespace UniScanG.UI.Etc
{
    public partial class VncCamButton : UserControl, IVncControl, IModelListener, IMultiLanguageSupport, IOpStateListener
    {
        IServerExchangeOperator serverExchangeOperator;

        Process process;
        IntPtr handle;
        InspectorObj inspector;

        Action<IVncControl> vncViwerStarted;
        Action vncViwerExited;

        ExchangeCommand eVisit;
        
        public VncCamButton(ExchangeCommand eVisit, InspectorObj inspector, Action<IVncControl> vncViwerStarted, Action vncViwerExited)
        {
            InitializeComponent();

            this.eVisit = eVisit;

            this.inspector = inspector;

            this.vncViwerStarted = vncViwerStarted;
            this.vncViwerExited = vncViwerExited;

            serverExchangeOperator = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            this.TabIndex = 0;

            StringManager.AddListener(this);

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemState.Instance().AddOpListener(this);

        }

        public void InitHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        private void buttonCam_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        protected void ButtonClick()
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Connect"));
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            simpleProgressForm.Show(new Action(() =>
            {
                if (process == null)
                {
                    if (OpenVnc() == true)
                        ProcessStarted();
                }
                else
                {
                    ExitProcess();
                }
            }), cancellationTokenSource);

            simpleProgressForm.Close();
        }

        delegate void EnableDelegate();
        public void Disable()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableDelegate(Disable));
                return;
            }

            buttonCam.Enabled = false;
        }

        public void Enable()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EnableDelegate(Enable));
                return;
            }

            buttonCam.Enabled = true;
        }

        protected bool OpenVnc()
        {
            if (UniEye.Base.Data.SystemState.Instance().GetOpState() != UniEye.Base.Data.OpState.Idle)
                return false;

            Process newProcess = serverExchangeOperator.OpenVnc(eVisit, process, inspector.Info.Address, handle);

            bool existProcess = newProcess != null;

            if (newProcess != process)
            {
                newProcess.EnableRaisingEvents = true;
                newProcess.Exited += ProcessExited;

                process = newProcess;
            }

            return existProcess;
        }

        public void ExitProcess()
        {
            if (process != null)
            {
                if (process.HasExited == false)
                    process.Kill();

                process = null;
            }
        }

        public void ProcessStarted()
        {
            if (vncViwerStarted != null)
                vncViwerStarted(this);

            buttonCam.Appearance.BackColor = Colors.Wait;
        }

        public void ProcessExited(object sender, EventArgs e)
        {
            serverExchangeOperator.CloseVnc();

            process = null;

            if (vncViwerExited != null)
                vncViwerExited();

            ButtonStateChage();
        }
        
        public InspectorObj GetInspector()
        {
            return inspector;
        }

        public void ModelChanged()
        {
            ButtonStateChage();
        }

        public void ModelTeachDone(int camId)
        {
            ButtonStateChage();
        }
        public void ModelRefreshed() { }

        delegate void ButtonStateChageDelegate();
        private void ButtonStateChage()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ButtonStateChageDelegate(ButtonStateChage));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                buttonCam.Enabled = false;
                return;
            }

            //buttonCam.Enabled = true;

            if (serverExchangeOperator.ModelTrained(inspector.Info.CamIndex, inspector.Info.ClientIndex, SystemManager.Instance().CurrentModel.ModelDescription))
                buttonCam.Appearance.BackColor = Colors.Trained;
            else
                buttonCam.Appearance.BackColor = Colors.Untrained;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            buttonCam.Text = StringManager.GetString(this.GetType().FullName, inspector.Info.GetName());
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            this.Enabled = (curOpState == OpState.Idle);
            if (this.Enabled==false && process != null)
            {
                ExitProcess();
            }
        }
    }
}

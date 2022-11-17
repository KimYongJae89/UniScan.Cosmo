using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;

namespace DynMvp.UI
{
    public delegate bool SplashActionDelegate(SplashForm form);

    public partial class SplashForm : Form, IReportProgress
    {
        string lastError;
        bool doConfigAction = false;
        Thread workingThread = null;
        public SplashActionDelegate ConfigAction = null;
        public SplashActionDelegate SetupAlgorithmStrategyAction = null;
        public SplashActionDelegate SetupAction = null;
        public SplashActionDelegate PostSetupAction = null;

        public SplashForm()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            this.title.BackColor = Color.Transparent;
            this.title.Parent = backgroundImage;
            this.progressMessage.BackColor = Color.Transparent;
            this.progressMessage.Parent = backgroundImage;
            this.copyrightText.BackColor = Color.Transparent;
            this.copyrightText.Parent = backgroundImage;
            this.productLogo.BackColor = Color.Transparent;
            this.productLogo.Parent = backgroundImage;
            this.companyLogo.BackColor = Color.Transparent;
            this.companyLogo.Parent = backgroundImage;
            this.versionText.BackColor = Color.Transparent;
            this.versionText.Parent = backgroundImage;
            this.buildText.BackColor = Color.Transparent;
            this.buildText.Parent = backgroundImage;

            progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Loading...");
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(title.Text) == false)
                title.Font = UiHelper.AutoFontSize(title, title.Text);

            versionText.Text = string.Format("Version {0}", VersionHelper.Instance().VersionString);
            buildText.Text = string.Format("Build {0}", VersionHelper.Instance().BuildString);

            splashActionTimer.Start();
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        public void ReportProgress(int progressPos, string progressMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReportProgressDelegate(ReportProgress), progressPos, progressMessage);
                return;
            }

            this.progressBar.Value = progressPos;
            this.progressMessage.Text = progressMessage;
        }

        private void SpalashProc()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start SpalashProc.");

            if (SetupAction != null && SetupAction(this) == false)
            {
                MessageBox.Show("Some error is occurred. Please, check the configuration.\n\n" + lastError);
                DialogResult = DialogResult.Abort;
            }
        }

        private void SplashForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (workingThread != null)
                return;

            if (e.KeyCode == Keys.F12 /*&& e.Alt == true*/)
            {
                if (ConfigAction != null && !doConfigAction)
                {
                    doConfigAction = true;
                    progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Wait Configuration");

                    if (ConfigAction(this) == false)
                    {
                        this.DialogResult = DialogResult.Abort;
                        Close();
                    }

                    doConfigAction = false;
                    progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Loading...");
                }
            }
        }

        private void splashActionTimer_Tick(object sender, EventArgs e)
        {
            if (workingThread == null)
            {
                if (doConfigAction == false)
                {
                    doConfigAction = true;
                    LogHelper.Debug(LoggerType.StartUp, "Start Spalash Thread.");

                    progressMessage.Text = StringManager.GetString(this.GetType().FullName, "Start Setup...");
                    try
                    {
                        if (SetupAlgorithmStrategyAction != null)
                            SetupAlgorithmStrategyAction(this);

                        workingThread = new Thread(new ThreadStart(SpalashProc));
                        workingThread.IsBackground = true;
                        workingThread.Start();

                        splashActionTimer.Interval = 500;
                    }
                    catch (Exception ex)
                    {
                        MessageForm.Show(null, ex.Message);
                        DialogResult = DialogResult.Abort;
                        Close();
                    }
                }

            }
            else
            {
                if (workingThread.IsAlive == false)
                    Close();
            }
        }
    }
}

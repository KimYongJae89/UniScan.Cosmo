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

namespace UniScanG.UI.Etc
{
    public delegate void RunWorkerCompletedDelegate(bool result);

    public partial class ProgressForm : Form
    {
        public RunWorkerCompletedDelegate RunWorkerCompleted;

        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public BackgroundWorker BackgroundWorker
        {
          get { return backgroundWorker; }
        }

        private string titleText;
        public string TitleText
        {
            set { titleText = value; }
        }

        private string messageText;
        public string MessageText
        {
          set { messageText = value; }
        }

        private object argument = null;
        public object Argument
        {
            get { return argument; }
            set { argument = value; }
        }

        string lastError;

        public ProgressForm()
        {
            InitializeComponent();

#if DEBUG
            this.ControlBox = true;
#endif
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void SetLastError(string lastError)
        {
            this.lastError = lastError;
        }

        public string GetLastError()
        {
            return lastError;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelMessage.Text = e.UserState.ToString();
            progressBar.Value = e.ProgressPercentage;
            Invalidate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (messageText != null)
                labelMessage.Text = messageText;

            if (titleText != null)
                labelTitle.Text = titleText;

            startTimer.Start();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            startTimer.Stop();
            backgroundWorker.RunWorkerAsync(argument);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            while (backgroundWorker.IsBusy)
                Application.DoEvents();
            Close();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ok = false;
            if (e.Cancelled == true)
            {
                progressBar.Value = 0;
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Canceled");
                ok = false;
            }
            else if (!(e.Error == null))
            {
                progressBar.Value = 0;
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Error !!");
                ok = false;
            }
            else if (e.Result != null)
            {
                progressBar.Value = 100;
                progressBar.ForeColor = Color.Red;

                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Error !!");
                if(e.Result is Exception)
                    labelMessage.Text += (" : " + ((Exception)e.Result).Message);
                else
                    labelMessage.Text += (" : " + e.Result.ToString());

                ok = false;
            }
            else
            {
                labelMessage.Text = StringManager.GetString(this.GetType().FullName, "Done");
                ok = true;
                Close();
            }

            if (RunWorkerCompleted != null)
                RunWorkerCompleted(ok);

            Invalidate();
        }
    }
}

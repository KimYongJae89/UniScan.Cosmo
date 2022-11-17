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

namespace DynMvp.UI
{
    public delegate void ReportProgressDelegate(int percentage, string messge);

    public interface IReportProgress
    {
        void ReportProgress(int percentage, string messge);
        void SetLastError(string lastError);
        string GetLastError();
    }

    public partial class ProgressForm : Form, IReportProgress
    {
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
        
        string lastError;
        DialogResult pendingResult = DialogResult.None;
        
        public ProgressForm()
        {
            InitializeComponent();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel);
            message.Text = StringManager.GetString(this.GetType().FullName, message);
            //ProgressForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName,ProgressForm.ActiveForm.Text);
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
            message.Text = messageText;
            progressBar.Value = e.ProgressPercentage;
            Invalidate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            this.message.Text = messageText;
            this.Text = titleText;
            
            startTimer.Start();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            startTimer.Stop();
            backgroundWorker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
            else
            {
                DialogResult = pendingResult;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCancel.Text = StringManager.GetString("Close");
            progressBar.Value = progressBar.Maximum;

            if ((e.Cancelled == true))
            {
                message.Text = StringManager.GetString( "Cancelled!");
                pendingResult = DialogResult.Cancel;
            }
            else if (!(e.Error == null))
            {
                message.Text = string.Format(StringManager.GetString("Error! {0}"), e.Error.Message);
                pendingResult = DialogResult.No;
            }
            else
            {
                message.Text = "Done!";
                pendingResult = DialogResult.OK;
                Close();
            }
        }

        public void ReportProgress(int percentage, string messge)
        {
            //if (InvokeRequired)
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    BeginInvoke(new ReportProgressDelegate(ReportProgress), percentage, messge);
            //    return;
            //}

            MessageText = messge;
            backgroundWorker.ReportProgress(percentage);
        }
    }
}

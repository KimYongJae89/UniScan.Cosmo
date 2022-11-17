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
using System.Threading.Tasks;

namespace DynMvp.UI.Touch
{
    public partial class SimpleProgressForm : Form
    {         
        private string messageText;
        public string MessageText
        {
            set {
                messageText = value;
                SetLabelMessage(messageText);
            }
        }

        CancellationTokenSource cancellationTokenSource;

        Task task;
        public Task Task
        {
            get {return  task; }
        }

        public SimpleProgressForm(string message = null)
        {
            InitializeComponent();

#if DEBUG
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
#endif

            messageText = message;
            SetLabelMessage(messageText);

            this.TopMost = true;
            this.TopLevel = true;
        }

        public delegate void SetLabelMessageDelegate(string message);
        public void SetLabelMessage(string message)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new SetLabelMessageDelegate(SetLabelMessage), message);
                return;
            }

            messageText = message;
            if (messageText == null)
                this.labelMessage.Text = StringManager.GetString("Wait");
            else
                this.labelMessage.Text = message;
        }

        public static void Show(string message, Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            SimpleProgressForm form = new SimpleProgressForm(message);
            form.Show(action, cancellationTokenSource);
        }

        public void Show(Action action, CancellationTokenSource cancellationTokenSource = null)
        {
            this.cancellationTokenSource = cancellationTokenSource;

            if (cancellationTokenSource != null)
            {
                task = new Task(action, cancellationTokenSource.Token);
            }
            else
            {
                task = new Task(action);
            }
            
            task.Start();

#if DEBUG
            this.TopMost = false;
#endif

            base.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (task == null || task.IsCompleted)
            {
                Close();
            }

            if(cancellationTokenSource!=null && cancellationTokenSource.IsCancellationRequested)
            {
                Close();
                //throw new OperationCanceledException();
            }
        }

        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            if (cancellationTokenSource == null)
                buttonCancel.Visible = false;

            taskCheckTimer.Start();
        }
    }
}

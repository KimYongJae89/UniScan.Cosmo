using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using System.Drawing.Imaging;

namespace DynMvp.UI
{
    public partial class AlarmMessageForm : Form
    {
        bool startup = true;
        int curIndex;
        ErrorItem curErrorItem;

        private Point mousePoint;

        public AlarmMessageForm()
        {
            InitializeComponent();
        }

        private void AlarmMessageForm_Load(object sender, EventArgs e)
        {
            errorCheckTimer.Start();
        }

        private void buttonPrevError_Click(object sender, EventArgs e)
        {
            if (curIndex > 0)
            {
                curIndex--;
                curErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];
                UpdateData();
            }
        }

        private void buttonNextError_Click(object sender, EventArgs e)
        {
            if ((ErrorManager.Instance().ErrorItemList.Count - 1) > curIndex)
            {
                curIndex++;
                curErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];
                UpdateData();
            }
        }

        void UpdateData()
        {
            buttonPrevError.Visible = false;
            buttonNextError.Visible = false;

            if (curIndex > 0)
                buttonPrevError.Visible = true;

            int errorListCount = ErrorManager.Instance().ErrorItemList.Count;
            if ((ErrorManager.Instance().ErrorItemList.Count - 1) > curIndex)
                buttonNextError.Visible = true;

            errorCode.Text = curErrorItem.ErrorCode.ToString();
            errorLevel.Text = curErrorItem.ErrorLevel.ToString();
            errorTime.Text = curErrorItem.ErrorTime.ToString("yyyy. MM. dd. HH:mm:ss");
            errorSection.Text = curErrorItem.SectionStr;
            errorName.Text = curErrorItem.ErrorStr;
            messsage.Text = curErrorItem.Message.ToString();

            if(curErrorItem.ErrorLevel == ErrorLevel.Warning)
            imageIcon.BackgroundImage = global::DynMvp.Properties.Resources.Warning;
            else
            imageIcon.BackgroundImage = global::DynMvp.Properties.Resources.Error;

            curErrorItem.Displayed = true;
        }

        private void errorCheckTimer_Tick(object sender, EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                if (Visible == false || startup == true)
                {
                    CenterToScreen();
                    this.WindowState = FormWindowState.Normal;
                    Show();

                    curIndex = ErrorManager.Instance().ErrorItemList.Count() - 1;
                    curErrorItem = ErrorManager.Instance().ErrorItemList[curIndex];

                    UpdateData();

                    Focus();

                    startup = false;
                }
            }
            else
            {
                if (Visible == true)
                {
                    Hide();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().ResetAlarm();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X), this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void buttonAlarmOff_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().BuzzerOn = false;
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().StopProcess();
        }
    }
}

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
using UniEye.Base;

namespace UniScanG.Temp
{
    public delegate void AlarmProcess();

    public partial class AlarmMessageForm : Form
    {
        public AlarmProcess alarmProcess = null;

        ErrorItem curErrorItem;

        public AlarmMessageForm()
        {
            InitializeComponent();
        }

        public void UpdateData()
        {
            if (ErrorManager.Instance().ErrorItemList.Count == 0)
                return;

            curErrorItem = ErrorManager.Instance().ErrorItemList.Last();
            if (curErrorItem.Alarmed == true)
            {
                messsage.Text = curErrorItem.Message.ToString();
                Show();
            }else
                Hide();

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (alarmProcess != null)
                alarmProcess();

            Hide();
            ErrorManager.Instance().ResetAlarm();
        }

        private void AlarmMessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            if (alarmProcess != null)
                alarmProcess();

            ErrorManager.Instance().ResetAlarm();
            Hide();
        }

        private void buttonBuzzerOff_Click(object sender, EventArgs e)
        {
            ErrorManager.Instance().BuzzerOn = false;
        }
    }
}

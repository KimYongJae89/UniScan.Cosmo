using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public partial class TowerLampOptionForm : Form
    {
        Task task = null;
        bool blinkFlag = false;
        public TowerLampValue lampValue { get; set; } = TowerLampValue.Off;

        public TowerLampOptionForm()
        {
            InitializeComponent();

            task = new Task(new Action(BlinkProc));
            task.Start();

            CenterToScreen();
        }

        private void BlinkProc()
        {
            while(!task.IsCanceled)
            {
                if (blinkFlag)
                    btnBlink.Image = DynMvp.Device.Properties.Resources.Lamp_Blink;
                else
                    btnBlink.Image = DynMvp.Device.Properties.Resources.Lamp_Off;

                blinkFlag = !blinkFlag;

                Thread.Sleep(500);
            }
        }

        private void btnLampValue_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if(btn != null)
            {
                if (btn == btnOff)
                    lampValue = TowerLampValue.Off;
                else if (btn == btnOn)
                    lampValue = TowerLampValue.On;
                else if (btn == btnBlink)
                    lampValue = TowerLampValue.Blink;
            }
        }

        private void TowerLampOptionForm_Load(object sender, EventArgs e)
        {
            switch (lampValue)
            {
                case TowerLampValue.Off:
                    btnOff.BackColor = Color.FromArgb(3, 78, 162);
                    btnOff.ForeColor = Color.White;

                    btnOn.BackColor = Color.LightGray;
                    btnOn.ForeColor = Color.Black;

                    btnBlink.BackColor = Color.LightGray;
                    btnBlink.ForeColor = Color.Black;
                    break;
                case TowerLampValue.On:
                    btnOn.BackColor = Color.FromArgb(3, 78, 162);
                    btnOn.ForeColor = Color.White;

                    btnOff.BackColor = Color.LightGray;
                    btnOff.ForeColor = Color.Black;

                    btnBlink.BackColor = Color.LightGray;
                    btnBlink.ForeColor = Color.Black;
                    break;
                case TowerLampValue.Blink:
                    btnBlink.BackColor = Color.FromArgb(3, 78, 162);
                    btnBlink.ForeColor = Color.White;

                    btnOn.BackColor = Color.LightGray;
                    btnOn.ForeColor = Color.Black;

                    btnOff.BackColor = Color.LightGray;
                    btnOff.ForeColor = Color.Black;
                    break;
                default:
                    break;
            }
        }
    }
}

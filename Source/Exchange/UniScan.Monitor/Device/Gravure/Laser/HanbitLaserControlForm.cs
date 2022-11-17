using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScan.Monitor.Device.Gravure.Laser
{
    public partial class HanbitLaserControlForm : Form
    {
        HanbitLaser hanbitLaser = null;

        bool alive = false;
        bool ready = false;
        bool error = false;
        bool outofrange = false;

        Timer updateTimer = new Timer();

        public HanbitLaserControlForm(HanbitLaser hanbitLaser)
        {
            InitializeComponent();

            this.hanbitLaser = hanbitLaser;

            this.setEmergencyTog.Enabled = this.setResetTog.Enabled = true;

            this.setAliveOn.Enabled = this.setAliveOff.Enabled = this.hanbitLaser.IsVirtual;
            this.setReadyOn.Enabled = this.setReadyOff.Enabled = this.hanbitLaser.IsVirtual;
            this.setErrorOn.Enabled = this.setErrorOff.Enabled= this.hanbitLaser.IsVirtual;
            this.setOutofrangeOn.Enabled = this.setOutofrangeOff.Enabled = this.hanbitLaser.IsVirtual;

            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Interval = 250;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            //this.setAliveOn.BackColor = this.alive ? Color.LightGreen : SystemColors.Control;
            //this.setAliveOff.BackColor = this.alive ? SystemColors.Control : Color.LightGreen;

            //this.setReadyOn.BackColor = this.ready ? Color.LightGreen : SystemColors.Control;
            //this.setReadyOff.BackColor = this.ready ? SystemColors.Control : Color.LightGreen;

            //this.setErrorOn.BackColor = this.error ? Color.LightGreen : SystemColors.Control;
            //this.setErrorOff.BackColor = this.error ? SystemColors.Control : Color.LightGreen;

            //this.setOutofrangeOn.BackColor = this.outofrange ? Color.LightGreen : SystemColors.Control;
            //this.setOutofrangeOff.BackColor = this.outofrange ? SystemColors.Control : Color.LightGreen;

            this.cmStateAlive.Checked = this.hanbitLaser.IsSetAlive;
            this.cmStateEmergency.Checked = this.hanbitLaser.IsSetEmergency;
            this.cmStateReset.Checked = this.hanbitLaser.IsSetReset;
            this.cmStateRun.Checked = this.hanbitLaser.IsSetRun;
            this.cmStateNg.Checked = this.hanbitLaser.IsSetNG;

            this.laserStateAlive.Checked = this.hanbitLaser.IsAlive;
            this.laserStateReady.Checked = this.hanbitLaser.IsReady;
            this.laserStateError.Checked = this.hanbitLaser.IsError;
            this.laserStateOutofrange.Checked = this.hanbitLaser.IsOutOfRange;

        }

        private void Set_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name == setEmergencyTog.Name)
                this.hanbitLaser.SetEmergency(!this.hanbitLaser.IsSetEmergency);
            else if (button.Name == setResetTog.Name)
                this.hanbitLaser.SetReset(!this.hanbitLaser.IsSetReset);
            else if (button.Name == setRunTog.Name)
                this.hanbitLaser.SetRun(!this.hanbitLaser.IsSetRun);
            else if (button.Name == setNgTog.Name)
                this.hanbitLaser.SetNG(!this.hanbitLaser.IsSetNG);


            if (this.hanbitLaser is HanbitLaserVirtual)
            {
                HanbitLaserVirtual hanbitLaserVirtual = (HanbitLaserVirtual)this.hanbitLaser;
                
                if (button.Name == setAliveOn.Name)
                    this.alive = hanbitLaserVirtual.SetAlive(true);
                else if (button.Name == setAliveOff.Name)
                    this.alive = hanbitLaserVirtual.SetAlive(false);
                else if (button.Name == setReadyOn.Name)
                    this.ready = hanbitLaserVirtual.SetReady(true);
                else if (button.Name == setReadyOff.Name)
                    this.ready = hanbitLaserVirtual.SetReady(false);
                else if (button.Name == setErrorOn.Name)
                    this.error = hanbitLaserVirtual.SetError(true);
                else if (button.Name == setErrorOff.Name)
                    this.error = hanbitLaserVirtual.SetError(false);
                else if (button.Name == setOutofrangeOn.Name)
                    this.outofrange = hanbitLaserVirtual.SetOutOfRange(true);
                else if (button.Name == setOutofrangeOff.Name)
                    this.outofrange = hanbitLaserVirtual.SetOutOfRange(false);
            }
        }

        private void HanbitLaserControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}

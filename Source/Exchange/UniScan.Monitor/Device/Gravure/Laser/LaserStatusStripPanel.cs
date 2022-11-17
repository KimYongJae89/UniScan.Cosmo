using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.Etc;
using UniScan.Common.Util;

namespace UniScan.Monitor.Device.Gravure.Laser
{
    public partial class LaserStatusStripPanel : UserControl, IStatusStripPanel
    {
        HanbitLaser hanbitLaser = null;
        public LaserStatusStripPanel(HanbitLaser hanbitLaser)
        {
            InitializeComponent();

            this.hanbitLaser = hanbitLaser;
        }

        public void StateUpdate()
        {
            labelLaser.BackColor = this.hanbitLaser.IsAlive ? Colors.Connected : Colors.Disconnected;
            labelReady.BackColor = this.hanbitLaser.IsReady ? Colors.Run : Colors.Normal;
            labelRun.BackColor = this.hanbitLaser.IsSetRun? Colors.Run : Colors.Normal;
            labelError.BackColor = this.hanbitLaser.IsError ? Colors.Alarm : Colors.Normal;
            labelOfr.BackColor = this.hanbitLaser.IsOutOfRange ? Colors.Alarm : Colors.Normal;
        }

        private void labelLaser_DoubleClick(object sender, EventArgs e)
        {
            HanbitLaserControlForm form = new HanbitLaserControlForm(this.hanbitLaser);
            form.Show();
        }
    }
}

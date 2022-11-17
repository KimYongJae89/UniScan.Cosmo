using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanM.UI
{
    public partial class ClockControl : UserControl
    {
        public ClockControl()
        {
            InitializeComponent();
        }

        void UpdateClock()
        {
            string date = DateTime.Now.ToString("yyyy. MM. dd");
            if (labelDate.Text != date)
                labelDate.Text = date;

            string time = DateTime.Now.ToString("HH:mm:ss");
            if (labelTime.Text != time)
                labelTime.Text = time;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void ClockControl_Load(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}

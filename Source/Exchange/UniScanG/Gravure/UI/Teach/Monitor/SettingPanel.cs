using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanG.Gravure.UI.Teach.Monitor
{
    public partial class SettingPanel : UserControl
    {
        int camIdx = -1;
        public SettingPanel(int camIdx)
        {
            InitializeComponent();

            this.camIdx = camIdx;
        }
    }
}

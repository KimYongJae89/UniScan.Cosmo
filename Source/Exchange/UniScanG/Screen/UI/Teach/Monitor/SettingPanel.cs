using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using System.Reflection;
using DynMvp.Base;

namespace UniScanG.Screen.UI.Teach.Monitor
{
    public partial class SettingPanel : UserControl, IMultiLanguageSupport
    {
        public SettingPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonTurn_Click(object sender, EventArgs e)
        {

        }

        private void lightValue_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

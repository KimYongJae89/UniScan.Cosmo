using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniScanG.Gravure.Data;

namespace UniScan.Monitor.Device.Gravure.UI
{
    public partial class MachineSettingButton : UserControl, IMultiLanguageSupport
    {
        Form form = null;
        public MachineSettingButton()
        {
            InitializeComponent();

            this.TabIndex = 0;
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonMachine_Click(object sender, EventArgs e)
        {
            //((Gravure.Inspect.InspectRunnerMonitorG)SystemManager.Instance().InspectRunner).AutoTeach();

            if (this.form == null)
            {
                // TEST-Send Teach Command

                Form form = new Form();
                //form.Size = new System.Drawing.Size(579, 162);
                form.TopMost = true;
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                form.MinimizeBox = form.MaximizeBox = false;
                form.FormClosed += Form_FormClosed;
                form.AutoSize = true;
                
                MachineSettingPanel machineSettingPanel = new MachineSettingPanel(SystemManager.Instance().DeviceBox.PortMap);
                //lightSettingPanel.Dock = DockStyle.Fill;
                form.Controls.Add(machineSettingPanel);
                form.AutoSize = true;
                //form.Size = lightSettingPanel.Size;
                //lightCtrl.TurnOn(lightParam.LightValue);
                this.form = form;
                form.Show();
                
            }
            else
            {
                this.form.Focus();
            }
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemManager.Instance().CurrentModel.Modified = true;
            SystemManager.Instance().DeviceBox.AxisConfiguration.SaveConfiguration();
            this.form = null;
        }
    }
}

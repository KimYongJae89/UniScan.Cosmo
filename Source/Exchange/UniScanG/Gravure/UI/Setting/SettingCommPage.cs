using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using UniScanG.Gravure.Settings;
using UniScanG.Settings.UI;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingCommPage : UserControl, IMultiLanguageSupport
    {
        private bool onUpdate;
        MachineIfViewPanel machineIfViewPanel;

        public SettingCommPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            machineIfViewPanel = new MachineIfViewPanel();
            machineIfViewPanel.Dock = DockStyle.Fill;
            machineIfViewPanel.Initialize(UniEye.Base.Settings.MachineSettings.Instance().MachineIfSetting);
            groupBoxPlcCommStatus.Controls.Add(machineIfViewPanel);
            
            StringManager.AddListener(this);
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            onUpdate = true;
            onUpdate = false;
            UpdateData();
        }

        public void UpdateData()
        {
            onUpdate = true;

            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();
            this.checkBoxAutoOperation.Checked = settings.AutoOperation;
            
            onUpdate = false;
        }

        private void ApplyData()
        {
            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();
            settings.AutoOperation = this.checkBoxAutoOperation.Checked;

            settings.Save();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            ApplyData();
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            ApplyData();
        }

        private void openIoViewer_Click(object sender, EventArgs e)
        {
            Form form = new DynMvp.Devices.UI.IoPortViewer(SystemManager.Instance().DeviceBox.DigitalIoHandler, SystemManager.Instance().DeviceBox.PortMap);
            form.Show();
        }
    }
}

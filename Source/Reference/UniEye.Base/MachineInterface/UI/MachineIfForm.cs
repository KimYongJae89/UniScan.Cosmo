using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.MachineInterface.UI
{
    public partial class MachineIfForm : Form
    {
        IMachineIfPanel machineIfPanel = null;
        IMachineIfPanel protocolPanel = null;

        public MachineIfForm(MachineIfSetting machineIfSetting)
        {
            InitializeComponent();

            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            UpdateMachineIfPanel(machineIfSetting);
            protocolPanel = new ProtocolPanel();
            ((Control)protocolPanel).Dock = DockStyle.Fill;
            protocolPanel.Initialize(machineIfSetting);
            panelProtocol.Controls.Add((UserControl)protocolPanel);
        }
        
        private void UpdateMachineIfPanel(MachineIfSetting settings)
        {
            switch (settings.MachineIfType)
            {
                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    machineIfPanel = new TcpIpMachineIfPanel();
                    break;
                case MachineIfType.Melsec:
                    machineIfPanel = new MelsecMachineIfPanel();
                    break;
            }
            machineIfPanel.Initialize(settings);
            UserControl userControl = (UserControl)machineIfPanel;
            userControl.Dock = DockStyle.Fill;
            this.panelMachineIF.Controls.Add(userControl);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if(machineIfPanel.Verify() && protocolPanel.Verify())
            {
                machineIfPanel.Apply();
                protocolPanel.Apply();
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}

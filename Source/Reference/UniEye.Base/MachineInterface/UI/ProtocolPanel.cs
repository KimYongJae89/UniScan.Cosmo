using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.MachineInterface.UI
{
    public partial class ProtocolPanel : UserControl, IMachineIfPanel
    {
        MachineIfSetting machineIfSetting;

        public ProtocolPanel()
        {
            InitializeComponent();
        }

        public void Apply()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Enum command = (Enum)row.Cells[0].Value;
                bool use = (bool)row.Cells[1].Value;
                int waitResponceMs = int.Parse(row.Cells[2].Value.ToString());
                switch (machineIfSetting.MachineIfType)
                {
                    case MachineIfType.Melsec:
                        {
                            string address = (string)row.Cells[3].Value;
                            bool isReadComm = Convert.ToBoolean(row.Cells[4].Value);
                            int sizeWord = Convert.ToInt32(row.Cells[5].Value);
                            this.machineIfSetting.MachineIfProtocolList.Dic[command] = new MelsecMachineIfProtocol(command, use, waitResponceMs, address, isReadComm, sizeWord);
                        }
                        break;
                    case MachineIfType.TcpServer:
                    case MachineIfType.TcpClient:
                        this.machineIfSetting.MachineIfProtocolList.Dic[command] = new TcpIpMachineIfProtocol(command, use, waitResponceMs);
                        break;
                }
                
            }
        }

        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = machineIfSetting;

            DataGridViewColumn colCommand = new DataGridViewTextBoxColumn();
            colCommand.HeaderText = "Command";
            colCommand.ReadOnly = true;
            colCommand.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(colCommand);

            DataGridViewColumn colUse = new DataGridViewCheckBoxColumn();
            colUse.HeaderText = "Use";
            colCommand.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(colUse);

            DataGridViewColumn colWaitTimeMs = new DataGridViewTextBoxColumn();
            colWaitTimeMs.HeaderText = "ResponceWaitTimeMs";
            dataGridView1.Columns.Add(colWaitTimeMs);

            switch (machineIfSetting.MachineIfType)
            {
                case MachineIfType.Melsec:
                    AddMelsecColumns();
                    break;

                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
                    {
                        TcpIpMachineIfProtocol tcpIpMachineIfProtocol = (TcpIpMachineIfProtocol)pair.Value;
                        dataGridView1.Rows.Add(pair.Key, tcpIpMachineIfProtocol.Use, tcpIpMachineIfProtocol.WaitResponceMs);
                    }
                    break;
            }

            dataGridView1.AutoResizeColumns();
        }

        private void AddMelsecColumns()
        {
            DataGridViewColumn colAddr = new DataGridViewTextBoxColumn();
            colAddr.HeaderText = "Address";
            dataGridView1.Columns.Add(colAddr);

            DataGridViewColumn colCommType = new DataGridViewCheckBoxColumn();
            colCommType.HeaderText = "IsReadCommand";
            dataGridView1.Columns.Add(colCommType);

            DataGridViewColumn colWordSize = new DataGridViewTextBoxColumn();
            colWordSize.HeaderText = "ReadSizeWord";
            dataGridView1.Columns.Add(colWordSize);

            if (machineIfSetting.MachineIfProtocolList != null)
            {
                foreach (KeyValuePair<Enum, MachineIfProtocol> pair in machineIfSetting.MachineIfProtocolList.Dic)
                {
                    MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)pair.Value;

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1, pair.Key, melsecMachineIfProtocol.Use, melsecMachineIfProtocol.WaitResponceMs, melsecMachineIfProtocol.Address, melsecMachineIfProtocol.IsReadCommand, melsecMachineIfProtocol.SizeWord);
                    row.Tag = melsecMachineIfProtocol;

                    dataGridView1.Rows.Add(row);
                }
            }
        }

        public bool Verify()
        {
            return true;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {

        }
    }
}

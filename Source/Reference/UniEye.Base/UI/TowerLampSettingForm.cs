using DynMvp.Devices;
using Infragistics.Win.UltraDataGridView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Device;

namespace UniEye.Base.UI
{
    public partial class TowerLampSettingForm : Form
    {
        string[] lampValues = Enum.GetNames(typeof(TowerLampValue));//{ "OFF", "ON", "BLINK" };

        TowerLamp towerLamp;

        public string ConfigPath { get; set; }

        public TowerLampSettingForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public void UpdateTowerLamp()
        {
            towerLamp = SystemManager.Instance().DeviceController.TowerLamp;

            dgvTowerLamp.Rows.Clear();
            
            // DataGridView
            List<TowerLampState> towerLamplist = towerLamp.TowerLampStateList;

            //TowerLampValue

            DataGridViewButtonCell btnCell = null;

            foreach (TowerLampState state in towerLamplist)
            {
                int index = dgvTowerLamp.Rows.Add();
                dgvTowerLamp.Rows[index].Tag = state;
                dgvTowerLamp.Rows[index].Cells[0].Value = Enum.GetName(typeof(TowerLampStateType), state.Type);

                // Red
                btnCell = (DataGridViewButtonCell)dgvTowerLamp.Rows[index].Cells[1];
                btnCell.Value = lampValues[GetLampIndex(state.RedLamp)];
                btnCell.Tag = state.RedLamp;

                // Yellow
                btnCell = (DataGridViewButtonCell)dgvTowerLamp.Rows[index].Cells[2];
                btnCell.Value = lampValues[GetLampIndex(state.YellowLamp)];
                btnCell.Tag = state.YellowLamp;

                // Green
                btnCell = (DataGridViewButtonCell)dgvTowerLamp.Rows[index].Cells[3];
                btnCell.Value = lampValues[GetLampIndex(state.GreenLamp)];
                btnCell.Tag = state.GreenLamp;

                // Buzzer
                btnCell = (DataGridViewButtonCell)dgvTowerLamp.Rows[index].Cells[4];
                btnCell.Value = lampValues[GetLampIndex(state.Buzzer)];
                btnCell.Tag = state.Buzzer;
            }
        }

        private int GetLampIndex(Lamp lamp)
        {
            int[] values = Enum.GetValues(typeof(TowerLampValue)) as int[];
            return Array.FindIndex(values, e => e == (int)lamp.Value);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvTowerLamp.Rows)
            {
                TowerLampState state = (TowerLampState)row.Tag;
                if (state == null)
                    continue;

                for(int i = 1; i < row.Cells.Count; i++)
                {
                    DataGridViewButtonCell cmbCtrl = (DataGridViewButtonCell)row.Cells[i];
                    int index = Array.FindIndex(lampValues, element => element == cmbCtrl.Value.ToString());
                    Lamp lamp = (Lamp)cmbCtrl.Tag;
                    lamp.Value = (TowerLampValue)index;
                }
            }

            towerLamp.Save(ConfigPath);
        }

        private void dgvTowerLamp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                //TODO - Button Clicked - Execute Code Here

                DataGridViewButtonCell btn = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                if (btn != null && btn.Tag != null)
                {
                    TowerLampOptionForm form = new TowerLampOptionForm();
                    form.lampValue = (TowerLampValue)Array.FindIndex(lampValues, value => value == (string)btn.Value);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        btn.Value = lampValues[(int)form.lampValue];
                    }
                }
            }
        }
    }
}

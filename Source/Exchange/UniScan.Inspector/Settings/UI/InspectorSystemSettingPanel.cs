using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Settings.UI;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;

namespace UniScan.Inspector.Settings.Inspector.UI
{
    public partial class InspectorSystemSettingPanel : UserControl, ICustomConfigPage
    {
        public InspectorSystemSettingPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InItComponent();
        }

        public void InItComponent()
        {
            labelCamIndex.Text = StringManager.GetString(this.GetType().FullName, labelCamIndex.Text);
            labelClientIndex.Text = StringManager.GetString(this.GetType().FullName, labelClientIndex.Text);
            useLocalMonitorMode.Text = StringManager.GetString(this.GetType().FullName, useLocalMonitorMode.Text);

            calculatorVersion.DataSource = Enum.GetValues(typeof(CalculatorBase.Version));
        }

        public void UpdateData()
        {
            camIndex.Value = InspectorSystemSettings.Instance().CamIndex;
            clientIndex.Value = InspectorSystemSettings.Instance().ClientIndex;
            useLocalMonitorMode.Checked = InspectorSystemSettings.Instance().LocalExchangeMode;
            calculatorVersion.SelectedItem = AlgorithmSetting.Instance().CalculatorVersion;

            List<InspectorInfo> inspectorInfoList = InspectorSystemSettings.Instance().SlaveInfoList;
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
                AddGrid(inspectorInfo);
        }

        public bool SaveData()
        {
            int curCamIndex = (int)camIndex.Value;

            InspectorSystemSettings.Instance().CamIndex = (int)camIndex.Value;
            InspectorSystemSettings.Instance().ClientIndex = (int)clientIndex.Value;
            InspectorSystemSettings.Instance().LocalExchangeMode = useLocalMonitorMode.Checked;
            AlgorithmSetting.Instance().CalculatorVersion = (CalculatorBase.Version)calculatorVersion.SelectedItem;

            TcpIpInfo tcpIpInfo = new TcpIpInfo(useLocalMonitorMode.Checked ? "127.0.0.1" : AddressManager.Instance().GetMonitorAddress(), AddressManager.Instance().GetMonitorLinteningPort());
            InspectorSystemSettings.Instance().ClientSetting.TcpIpInfo = tcpIpInfo;

            List<InspectorInfo> inspectorInfoList = InspectorSystemSettings.Instance().SlaveInfoList;
            inspectorInfoList.Clear();
            for (int i=0; i< inspectorInfoGridView.Rows.Count; i++)
            {
                DataGridViewRow row = inspectorInfoGridView.Rows[i];
                if (row.IsNewRow)
                    continue;

                InspectorInfo inspectorInfo = row.Tag as InspectorInfo;
                inspectorInfo.UpdateAddress();
                if (inspectorInfo != null)
                    inspectorInfoList.Add(inspectorInfo);
                //inspectorInfo.Address = AddressManager.Instance().GetInspectorAddress(false, curCamIndex,)
                //inspectorInfoList.Add();
            }

            AlgorithmSetting.Instance().Save();
            InspectorSystemSettings.Instance().Save();
            return true;
        }

        private void clientIndex_ValueChanged(object sender, EventArgs e)
        {
            if (clientIndex.Value == 0)
                groupMaster.Show();
            else
                groupMaster.Hide();
        }

        private void buttonAddInspectorInfo_Click(object sender, EventArgs e)
        {
            InspectorInfo inspectorInfo = new InspectorInfo();
            AddGrid(inspectorInfo);
            //inspectorInfoGridView.Rows.Add("0", "");
        }

        private void AddGrid(InspectorInfo inspectorInfo)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(inspectorInfoGridView);
            newRow.Cells[0].Value = inspectorInfo.ClientIndex;
            newRow.Cells[1].Value = inspectorInfo.Path;
            newRow.Tag = inspectorInfo;

            inspectorInfoGridView.Rows.Add(newRow);
        }

        private void buttonDeleteInspectorInfo_Click(object sender, EventArgs e)
        {
            if (inspectorInfoGridView.SelectedCells.Count == 0)
                return;

            int selectedRowIndex = inspectorInfoGridView.SelectedCells[0].RowIndex;
            inspectorInfoGridView.Rows.RemoveAt(selectedRowIndex);
        }

        private void inspectorInfoGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = inspectorInfoGridView.Rows[e.RowIndex];
            InspectorInfo inspectorInfo = inspectorInfoGridView.Rows[e.RowIndex].Tag as InspectorInfo;
            int cliendIndex = int.Parse(row.Cells[0].Value.ToString());
            string path = row.Cells[1].Value.ToString();

            inspectorInfo.ClientIndex = cliendIndex;
            inspectorInfo.Path = path;
        }
    }
}

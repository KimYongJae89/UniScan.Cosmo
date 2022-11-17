using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.UI;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Settings.UI;

namespace UniScan.Monitor.Settings.Monitor.UI
{
    public partial class MonitorSystemSettingPanel : UserControl, ICustomConfigPage
    {
        bool onUpdateData = false;
        FovSettingForm fovSettingForm = null;
        
        public MonitorSystemSettingPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitComponent();
        }

        public void InitComponent()
        {
            labelVncViewerPath.Text = StringManager.GetString(this.GetType().FullName, labelVncViewerPath.Text);
        }

        public void UpdateData()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;
            
            vncViewerPath.Text = MonitorSystemSettings.Instance().VncPath;
            useLocalMonitorMode.Checked = MonitorSystemSettings.Instance().LocalExchangeMode;
            useTestbedStage.Checked = MonitorSystemSettings.Instance().UseTestbedStage;
            useLaserBurner.Checked = MonitorSystemSettings.Instance().UseLaserBurner;

            InspectorInfoGridView.Rows.Clear();

            foreach (InspectorInfo inspectorInfo in MonitorSystemSettings.Instance().InspectorInfoList)
            {
                int index = InspectorInfoGridView.Rows.Add(inspectorInfo.CamIndex, inspectorInfo.ClientIndex, inspectorInfo.Path);
                InspectorInfoGridView.Rows[index].Tag = inspectorInfo;
            }
            
            onUpdateData = false;
        }
        
        private void buttonAddInspectorInfo_Click(object sender, EventArgs e)
        {
            InspectorInfoGridView.Rows.Add("0","0", "");
        }

        private void buttonDeleteInspectorInfo_Click(object sender, EventArgs e)
        {
            if (InspectorInfoGridView.SelectedCells.Count == 0)
                return;

            int selectedRowIndex = InspectorInfoGridView.SelectedCells[0].RowIndex;
            InspectorInfoGridView.Rows.RemoveAt(selectedRowIndex);
        }

        public bool SaveData()
        {
            MonitorSystemSettings.Instance().VncPath = vncViewerPath.Text;
            MonitorSystemSettings.Instance().LocalExchangeMode = useLocalMonitorMode.Checked;
            MonitorSystemSettings.Instance().UseTestbedStage = useTestbedStage.Checked;
            MonitorSystemSettings.Instance().UseLaserBurner = useLaserBurner.Checked;

            bool localMode = MonitorSystemSettings.Instance().LocalExchangeMode;
            TcpIpInfo tcpIpInfo = new TcpIpInfo(localMode ? "127.0.0.1" : AddressManager.Instance().GetMonitorAddress(), AddressManager.Instance().GetMonitorLinteningPort());
            MonitorSystemSettings.Instance().ServerSetting.TcpIpInfo = tcpIpInfo;

            List<InspectorInfo> inspectorInfoList = MonitorSystemSettings.Instance().InspectorInfoList;
            inspectorInfoList.Clear();

            foreach (DataGridViewRow row in InspectorInfoGridView.Rows)
            {
                bool isEmpty = string.IsNullOrEmpty(row.Cells[0].Value?.ToString()) || string.IsNullOrEmpty(row.Cells[1].Value?.ToString());
                if (isEmpty == true)
                    continue;

                int camIndex = int.Parse(row.Cells[0].Value.ToString());
                int clientIndex = int.Parse(row.Cells[1].Value.ToString());
                string address = localMode ? "127.0.0.1" : AddressManager.Instance().GetInspectorAddress(camIndex, clientIndex);
                string path = row.Cells[2].Value?.ToString();
                if (string.IsNullOrEmpty(path))
                    path = Path.Combine(@"\\", address, "UniScan", "Gravure_Inspector");

                if (Directory.Exists(path) == false)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Can not find Path");
                    sb.AppendLine(string.Format("Camera: {0} Client: {1}", camIndex, clientIndex));
                    sb.AppendLine(string.Format("Path: {0}", path));
                    DynMvp.UI.Touch.MessageForm.Show(null, sb.ToString());
                    return false;
                }

                InspectorInfo inspectorInfo = new InspectorInfo();
                inspectorInfo.CamIndex = camIndex;
                inspectorInfo.ClientIndex = clientIndex;
                inspectorInfo.Path = path;
                inspectorInfo.Address = address;

                if (row.Tag != null)
                    inspectorInfo.Fov = ((InspectorInfo)row.Tag).Fov;

                inspectorInfoList.Add(inspectorInfo);
            }

            inspectorInfoList.Sort((f, g) => {
                return f.GetName().CompareTo(g.GetName());
            });

            MonitorSystemSettings.Instance().Save();
            return true;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            MachineIfForm form = new MachineIfForm(MonitorSystemSettings.Instance().ServerSetting);
            form.ShowDialog();
        }

        private void buttonFov_Click(object sender, EventArgs e)
        {
            List<InspectorInfo> inspectorInfoList = new List<InspectorInfo>();

            foreach (DataGridViewRow row in InspectorInfoGridView.Rows)
            {
                InspectorInfo inspectorInfo = null;

                if (row.Tag != null)
                    inspectorInfo = (InspectorInfo)row.Tag;
                else
                {
                    bool isEmpty = false;
                    foreach (DataGridViewTextBoxCell cell in row.Cells)
                    {
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()) == true)
                        {
                            isEmpty = true;
                            break;
                        }
                    }

                    if (isEmpty == true)
                        continue;

                    inspectorInfo.CamIndex = int.Parse(row.Cells[0].Value.ToString());
                
                    row.Tag = inspectorInfo;
                }

                inspectorInfoList.Add(inspectorInfo);
            }

            fovSettingForm = new FovSettingForm(inspectorInfoList);
            fovSettingForm.ShowDialog();

            if (fovSettingForm.DialogResult == DialogResult.OK)
            // Client==0 인 객체의 FOV를 Client!=0인 객체에게 전파
            {
                List<InspectorInfo> inspectorInfos = inspectorInfoList.FindAll(f => f.ClientIndex == 0);
                foreach (InspectorInfo inspectorInfo in inspectorInfos)
                {
                    inspectorInfoList.FindAll(f => f.CamIndex == inspectorInfo.CamIndex).ForEach(f => f.Fov = inspectorInfo.Fov);
                }
            }
        }
    }
}

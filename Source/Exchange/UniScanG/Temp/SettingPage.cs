//using System;
//using System.Windows.Forms;
//using System.Diagnostics;
//using System.IO;
//using DynMvp.Devices.UI;
//using DynMvp.UI;
//using DynMvp.Authentication;
//using DynMvp.Base;
//using DynMvp.Data;
//using UniEye.Base.UI;
//using UniEye.Base.Settings;
//using UniEye.Base;

//using System.Data;
//using UniScanG.Operation;
//using UniEye.Base.Util;
//using System.Collections.Generic;
//using UniScanG.Operation.UI;
//using DynMvp.UI.Touch;
//using UniScanG;

//namespace UniScanG.Temp
//{
//    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage, IUserHandlerListener
//    {
//        bool onUpdateData = false;

//        // modaless로 구동하기 위함.
//        public SettingPage()
//        {
//            //InitializeComponent();

//            //UserHandler.Instance().AddListener(this);

//            //labelLanguage.Text = StringManager.GetString(this.GetType().FullName,labelLanguage.Text);

//            //logLevel.Items.AddRange(Enum.GetNames(typeof(LogLevel)));

//            //if (UniScanGSettings.Instance().SystemType == SystemType.Monitor)
//            //{
//            //    tabControlParam.Tabs["Inspector"].Visible = false;
//            //}
//            //else
//            //{
//            //    tabControlParam.Tabs["Monitoring"].Visible = false;
//            //    buttonTransfer.Visible = false;
//            //}
//        }

//        public void Initialize()
//        {

//        }

//        public void EnableControls()
//        {

//        }

//        public void TabPageVisibleChanged(bool visibleFlag)
//        {
//            //if (visibleFlag == true)
//            //{
//            //    LoadSettings();
//            //}
//            //else
//            //{
//            //    SaveSettings();
//            //    if (UniScanGSettings.Instance().SystemType == SystemType.Monitor)
//            //    {
//            //        //(SystemManager.Instance().MachineIf as MonitoringServer).SendSetting_Sync();
//            //        //((MPIS.UI.MainForm)SystemManager.Instance().MainForm).WaitJobDone("RESET");
//            //    }
//            //}
//        }

//        public void LoadSettings()
//        {
//            //onUpdateData = true;

//            ////Developer
//            //language.Text = OperationSettings.Instance().Language.ToString();

//            //SystemType systemType = UniScanGSettings.Instance().SystemType;
//            //this.systemType.SelectedIndex = (int)systemType;
//            ////switch (systemType)
//            ////{
//            ////    case SystemType.Inspector:
//            ////        this.systemType.SelectedIndex = 0;
//            ////        break;
//            ////    case SystemType.Monitor:
//            ////        this.systemType.SelectedIndex = 1;
//            ////        break;
//            ////}

//            //saveImageText.Checked = OperationSettings.Instance().SaveDebugImage;
//            //maskThH.Value = (decimal)UniScanGSettings.Instance().MaskThH;
//            //maskThV.Value = (decimal)UniScanGSettings.Instance().MaskThV;
//            //saturationRange.Value = (decimal)UniScanGSettings.Instance().SaturationRange;
//            //circleRadius.Value = (decimal)UniScanGSettings.Instance().CircleRadius;
//            //maxPattern.Value = (decimal)UniScanGSettings.Instance().MaxPattern;

//            ////Monitor
//            //storingDays.Value = SamsungElectroTransferSettings.Instance().StoringDays;
//            //vncViewerPath.Text = UniScanGSettings.Instance().VncPath;

//            //inspectorDataGridView.Rows.Clear();
//            //foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
//            //{
//            //    inspectorDataGridView.Rows.Add(info.CamIndex, info.ClientIndex, info.IpAddress, info.Path);
//            //}

//            ////Inspector
//            //startXPosition.Value = (decimal)UniScanGSettings.Instance().StartXPosition;
//            //startYPosition.Value = (decimal)UniScanGSettings.Instance().StartYPosition;
//            //asyncMode.Checked = UniScanGSettings.Instance().AsyncMode;
//            //fovX.Value = (decimal)UniScanGSettings.Instance().FovX;
//            //if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count > 0)
//            //    cameraResolution.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width.ToString("0.000 [um/px]");
//            //else
//            //    cameraResolution.Text = "";

//            //monitorIpAddress.Text = UniScanGSettings.Instance().MonitorInfo.IpAddress;
//            //inspectorIpAddress.Text = UniScanGSettings.Instance().InspectorInfo.IpAddress;
//            //camIndex.Value = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//            //clientIndex.Value = UniScanGSettings.Instance().InspectorInfo.ClientIndex;
//            //standAlone.Checked = UniScanGSettings.Instance().InspectorInfo.StandAlone;
//            //bufferSize.Value = UniScanGSettings.Instance().BufferSize;

//            //// developer
//            //logLevel.Text = OperationSettings.Instance().LogLevel.ToString();

//            //SaveDebugData imageDebugData = UniScanGSettings.Instance().SaveImageDebugData;
//            //saveImageText.Checked = (imageDebugData == SaveDebugData.Both || imageDebugData == SaveDebugData.Text);
//            //saveImageImage.Checked = (imageDebugData == SaveDebugData.Both || imageDebugData == SaveDebugData.Image);

//            //SaveDebugData fiducialDebugData = UniScanGSettings.Instance().SaveFiducialDebugData;
//            //saveFiducialText.Checked = (fiducialDebugData == SaveDebugData.Both || fiducialDebugData == SaveDebugData.Text);
//            //saveFiducialImage.Checked = (fiducialDebugData == SaveDebugData.Both || fiducialDebugData == SaveDebugData.Image);

//            //SaveDebugData inspectionDebugData = UniScanGSettings.Instance().SaveInspectionDebugData;
//            //saveInspectText.Checked = (inspectionDebugData == SaveDebugData.Both || inspectionDebugData == SaveDebugData.Text);
//            //saveInspectImage.Checked = (inspectionDebugData == SaveDebugData.Both || inspectionDebugData == SaveDebugData.Image);

//            //onUpdateData = false;
//        }

//        public void SaveSettings()
//        {
//            PathSettings.Instance().Save();
//            OperationSettings.Instance().Save();
//            UniScanGSettings.Instance().Save();
//            SamsungElectroTransferSettings.Instance().Save();
//        }

//        private void systemType_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            //UniScanGSettings.Instance().SystemType = (SystemType)Enum.Parse(typeof(SystemType), systemType.Items[systemType.SelectedIndex].ToString());
//        }
        
//        private void buttonSaveSetting_Click(object sender, EventArgs e)
//        {
//            SaveSettings();

//            //if (UniScanGSettings.Instance().SystemType == SystemType.GravureInspector)
//            //{
//            //    (SystemManager.Instance().MachineIf as MonitoringServer).SendSetting_Sync();
//            //}
//        }

//        private void buttonIPSet_Click(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            TcpIpSettingForm ipSettingForm = new TcpIpSettingForm(UniScanGSettings.Instance().UmxTcpIpInfo);
            
//            if (ipSettingForm.ShowDialog() == DialogResult.OK)
//            {
//                UniScanGSettings.Instance().UmxTcpIpInfo = ipSettingForm.TcpIpInfo;
//            }

//        }

//        private void buttonSend_Click(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            MonitoringClient monitoringClient = (MonitoringClient)SystemManager.Instance().MachineIf;
//        }
        
//        private void startXPosition_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().StartXPosition = (float)startXPosition.Value;
//        }

//        private void fovX_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().FovX = (float)fovX.Value;
//        }

//        private void sheetHeight_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            //UniScanGSettings.Instance().SheetHeight = (float)sheetHeight.Value;
//        }

//        private void saturationRange_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().SaturationRange = (int)saturationRange.Value;
//        }

//        private void circleRadius_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().CircleRadius = (int)circleRadius.Value;
//        }

//        delegate void UserChangedDelegatge();
//        public void UserChanged()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UserChangedDelegatge(UserChanged));
//                return;
//            }

//            User curUser = UserHandler.Instance().CurrentUser;
            
//            tabControlParam.Tabs["Developer"].Visible = curUser.Id.ToUpper() == "DEVELOPER";
//        }

//        private void maxPattern_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().MaxPattern = (int)maxPattern.Value;
//        }

//        private void storingDays_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            SamsungElectroTransferSettings.Instance().StoringDays = (int)storingDays.Value;
//        }

//        private void startYPosition_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().StartYPosition = (float)startYPosition.Value;
//        }

//        private void buttonCalibration_Click(object sender, EventArgs e)
//        {
//            CameraCalibrationForm form = new CameraCalibrationForm();
//            form.Initialize();
//            form.ShowDialog();

//            cameraResolution.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width.ToString("0.000 [um/px]");
//        }

//        private void buttonApply_Click(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().ClientInfoList.Clear();

//            foreach (DataGridViewRow row in inspectorDataGridView.Rows)
//            {
//                if (row.Cells[0].Value == null || row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null)
//                    continue;

//                InspectorInfo info = new InspectorInfo();
//                info.CamIndex = Convert.ToInt32(row.Cells[0].Value);
//                info.ClientIndex = Convert.ToInt32(row.Cells[1].Value);
//                info.IpAddress = row.Cells[2].Value.ToString();
//                info.Path = row.Cells[3].Value.ToString();

//                UniScanGSettings.Instance().ClientInfoList.Add(info);
//            }
//        }

//        private void vncViewerPath_TextChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().VncPath = vncViewerPath.Text;
//        }

//        private void inspectorIpAddress_TextChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().InspectorInfo.IpAddress = inspectorIpAddress.Text;
//        }

//        private void monitorIpAddress_TextChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().MonitorInfo.IpAddress = monitorIpAddress.Text;
//        }

//        private void camIndex_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().InspectorInfo.CamIndex = (int)camIndex.Value;
//        }

//        private void clientIndex_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().InspectorInfo.ClientIndex = (int)clientIndex.Value;
//        }

//        private void buttonDelete_Click(object sender, EventArgs e)
//        {
//            List<DataGridViewRow> rowList = new List<DataGridViewRow>();

//            foreach (DataGridViewRow row in inspectorDataGridView.SelectedRows)
//            {
//                rowList.Add(row);
//            }

//            foreach (DataGridViewRow row in rowList)
//            {
//                inspectorDataGridView.Rows.Remove(row);
//            }
//        }

//        private void buttonNew_Click(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            inspectorDataGridView.Rows.Add();
//        }

//        private void asyncMode_CheckedChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().AsyncMode = asyncMode.Checked;
//        }

//        private void maskThH_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().MaskThH = (int)maskThH.Value;
//        }

//        private void maskThV_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            UniScanGSettings.Instance().MaskThV = (int)maskThV.Value;
//        }

//        private void buttonMonGrabTest_Click(object sender, EventArgs e)
//        {
//            MainForm mainForm = SystemManager.Instance().MainForm as MainForm;
//            float convSpeed = mainForm.QuaryConvSpeed();
//            if (convSpeed < 0)
//                return;
//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            monitoringServer.SendTestGrab(convSpeed);
//            try
//            {
//                //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Testing...");
//            }
//            catch (OperationCanceledException)
//            {
//                monitoringServer.SendStop();
//                MessageForm.Show(null, "Stopped");
//            }
//        }

//        private void buttonInspGrabTest_Click(object sender, EventArgs e)
//        {
//            MainForm mainForm = SystemManager.Instance().MainForm as MainForm;
//            float convSpeed = mainForm.QuaryConvSpeed();
//            if (convSpeed < 0)
//                return;

//            mainForm.TeachingPage.Grab(convSpeed, TeachingPage.GrabType.Test);
//        }

//        private void buttonMonCommTest_Click(object sender, EventArgs e)
//        {
//            if (SystemManager.Instance().CurrentModel == null)
//            {
//                MessageForm.Show(null, "Please, Load any model first");
//                return;
//            }

//            MainForm mainForm = SystemManager.Instance().MainForm as MainForm;
//            mainForm.StartCommTest();
//        }

//        private void standAlone_CheckedChanged(object sender, EventArgs e)
//        {
//            UniScanGSettings.Instance().InspectorInfo.StandAlone = standAlone.Checked;

//            (SystemManager.Instance().MachineIf as MonitoringClient).StandAloneModeChanged();
//        }

//        private void bufferSize_ValueChanged(object sender, EventArgs e)
//        {
//            if (bufferSize.Value <= 0)
//            {
//                bufferSize.Value = 1;
//                return;
//            }

//            UniScanGSettings.Instance().BufferSize = (int)bufferSize.Value;

//        }

//        private void logLevel_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            LogHelper.ChangeLevel(logLevel.Text);
//            OperationSettings.Instance().LogLevel = (LogLevel)logLevel.SelectedIndex;
//        }

//        private void saveInspect_CheckedChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            SaveDebugData newSaveDebugData = (SaveDebugData)((saveInspectText.Checked ? 1 : 0) + (saveInspectImage.Checked ? 2 : 0));
//            UniScanGSettings.Instance().SaveInspectionDebugData = newSaveDebugData;
//        }
        
//        private void saveFiducial_CheckedChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            SaveDebugData newSaveDebugData = (SaveDebugData)((saveFiducialText.Checked ? 1 : 0) + (saveFiducialImage.Checked ? 2 : 0));
//            UniScanGSettings.Instance().SaveFiducialDebugData = newSaveDebugData;
//        }

//        private void saveImageImage_CheckedChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            if (saveImageImage.Checked)
//            {
//                string curPath = PathSettings.Instance().Temp;
//                if (string.IsNullOrEmpty(UniScanGSettings.Instance().SaveImageDebugDataPath) == false)
//                    curPath = UniScanGSettings.Instance().SaveImageDebugDataPath;

//                curPath = Path.GetFullPath(curPath);
//                FolderBrowserDialog dlg = new FolderBrowserDialog();
//                dlg.SelectedPath = curPath;
//                if (dlg.ShowDialog() == DialogResult.Cancel)
//                {
//                    saveImageImage.Checked = false;
//                    return;
//                }
//                UniScanGSettings.Instance().SaveImageDebugDataPath = dlg.SelectedPath;
//            }
//            saveImageText_CheckedChanged(sender, e);
//        }

//        private void saveImageText_CheckedChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            SaveDebugData newSaveDebugData = (SaveDebugData)((saveImageText.Checked ? 1 : 0) + (saveImageImage.Checked ? 2 : 0));
//            UniScanGSettings.Instance().SaveImageDebugData = newSaveDebugData;
//        }
//    }
//}

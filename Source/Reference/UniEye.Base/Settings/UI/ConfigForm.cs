using System;
using System.Windows.Forms;
using System.IO;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Vision;
using DynMvp.Data;
using System.Drawing.Imaging;
using DynMvp.Data.UI;
using UniEye.Base.Inspect;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.UI;
using UniEye.Base.MachineInterface.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using System.Collections.Generic;

namespace UniEye.Base.Settings.UI
{
    public interface ICustomConfigPage
    {
        void UpdateData();
        bool SaveData();
    }

    public partial class ConfigForm : Form
    {
        AlignDataInterfaceInfo alignDataInterfaceInfo;
        ConfigDevicePanel devicePanel;

        string[] systemTypes;

        bool lowLevelUser = false;
        bool onInitialize = false;

        ICustomConfigPage customConfigPage;

        public ConfigForm(bool lowLevelUser = false)
        {
            InitializeComponent();

            comboBoxLogLevel.DataSource = Enum.GetValues(typeof(LogLevel));

            devicePanel = new ConfigDevicePanel();

            tabPageDeviceNew.Controls.Add(devicePanel);
            
            devicePanel.Location = new System.Drawing.Point(3, 3);
            devicePanel.Name = "devicePanel";
            devicePanel.Size = new System.Drawing.Size(409, 523);
            devicePanel.Dock = DockStyle.Fill;
            devicePanel.Visible = true;

            //labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle.Text);
            //labelSystemType.Text = StringManager.GetString(this.GetType().FullName, labelSystemType.Text);
            //labelLanguage.Text = StringManager.GetString(this.GetType().FullName, labelLanguage.Text);
            //labelMachineIf.Text = StringManager.GetString(this.GetType().FullName, labelMachineIf.Text);
            //labelProductLogo.Text = StringManager.GetString(this.GetType().FullName, labelProductLogo.Text);
            //labelCompanyLogo.Text = StringManager.GetString(this.GetType().FullName, labelCompanyLogo.Text);
            //labelImagingLibrary.Text = StringManager.GetString(this.GetType().FullName, labelImagingLibrary.Text);
            //buttonOk.Text = StringManager.GetString(this.GetType().FullName, buttonOk.Text);
            //buttonCancel.Text = StringManager.GetString(this.GetType().FullName, buttonCancel.Text);

            this.lowLevelUser = lowLevelUser;
        }

        public void InitSystemType(string[] systemTypes, string curSystemType = null)
        {
            this.systemTypes = systemTypes;
            cmbSystemType.DataSource = systemTypes;
            if (curSystemType != null)
                cmbSystemType.Text = curSystemType;
        }

        public void InitCustomConfigPage(ICustomConfigPage customConfigPage)
        {
            this.customConfigPage = customConfigPage;

            if (customConfigPage != null)
            {
                customConfigPage.UpdateData();
                tabPageCustom.Controls.Add((UserControl)customConfigPage);
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            //cmbSystemType.DataSource = systemTypes;
            cmbMachineIfType.DataSource = Enum.GetNames(typeof(MachineIfType));
            cmbMachineIfListType.DataSource = Enum.GetNames(typeof(MachineIfType));
            imagingLibrary.DataSource = Enum.GetNames(typeof(ImagingLibrary));

            OperationSettings operationSettings = OperationSettings.Instance();
            CustomizeSettings customizeSettings = CustomizeSettings.Instance();
            MachineSettings machineSettings = MachineSettings.Instance();
            PathSettings pathSettings = PathSettings.Instance();

            virtualMode.Checked = machineSettings.VirtualMode;

            checkShowScore.Checked = operationSettings.ShowScore;
            checkShowNGImage.Checked = operationSettings.ShowNGImage;
            useFiducialStep.Checked = operationSettings.UseFiducialStep;
            useFixedInspectionStep.Checked = operationSettings.UseFixedInspectionStep;
            numInspectionStep.Enabled = useFixedInspectionStep.Checked;
            numInspectionStep.Value = operationSettings.NumInspectionStep;
            //cmbSystemType.SelectedText = operationSettings.SystemType;
            imagingLibrary.SelectedIndex = (int)operationSettings.ImagingLibrary;
            useCudaProcesser.Checked = operationSettings.UseCuda;
            useNonPageMem.Checked = operationSettings.UseNonPagedMem;
            language.Text = operationSettings.Language.ToString();
            dataStoringDays.Value = (decimal)operationSettings.ResultStoringDays;
            hddMinFreeSpaceValue.Value = (decimal)operationSettings.MinimumFreeSpace;
            dataCopyDays.Value = (decimal)operationSettings.ResultCopyDays;
            comboBoxLogLevel.SelectedIndex = (int)operationSettings.LogLevel;

            comboShowFirstPage.SelectedIndex = operationSettings.ShowFirstPageIndex - 1;
            useSingleTargetModel.Checked = operationSettings.UseSingleTargetModel;
            dataPathType.SelectedIndex = (int)operationSettings.DataPathType;
            saveTargetImage.Checked = operationSettings.SaveTargetImage;
            saveProbeImage.Checked = operationSettings.SaveProbeImage;
            checkUseLoginForm.Checked = operationSettings.UseUserManager;
            saveTargetGroupImage.Checked = operationSettings.SaveTargetGroupImage;
            useSimpleLightParamForm.Checked = operationSettings.UseSimpleLightParamForm;
            comboBoxTargetGroupImageFormat.Text = operationSettings.TargetGroupImageFormat.ToString();

            if (machineSettings.MachineIfSetting != null)
            {
                cmbMachineIfType.SelectedIndex = (int)machineSettings.MachineIfSetting.MachineIfType;
                isVirtualMachineIf.Checked = machineSettings.MachineIfSetting.IsVirtualMode;
            }

            if (machineSettings.MachineIfSettingList.Count > 0)
            {
                for(int i = 0; i < machineSettings.MachineIfSettingList.Count; i++)
                    cmbMachineIfList.Items.Add(i.ToString());
                
                cmbMachineIfList.SelectedIndex = 0;
                if (machineSettings.MachineIfSettingList[0] == null)
                    cmbMachineIfList.SelectedIndex = 0;
                //else
                //    cmbMachineIfList.SelectedIndex = (int)machineSettings.MachineIfSettingList[0].MachineIfType;
                cmbMachineIfList.Update();
            }

            numLightType.Value = machineSettings.NumLightType;

            companyLogo.Text = pathSettings.CompanyLogo;
            productLogo.Text = pathSettings.ProductLogo;

            title.Text = customizeSettings.Title;
            devicePanel.Title = title.Text;
            programTitle.Text = customizeSettings.ProgramTitle;
            numOfResultView.SelectedIndex = customizeSettings.NumOfResultView - 1;
            checkUseReportPage.Checked = customizeSettings.UseReportPage;
            checkUseLiveCam.Checked = customizeSettings.UseLiveCam;


#if DEBUG == false
            // systemType.Enabled = false;
#endif
            EnableControls();

            if (lowLevelUser == true)
            {
                tabMain.TabPages.RemoveAt(4);
                tabMain.TabPages.RemoveAt(3);
                tabMain.TabPages.RemoveAt(1);
                tabMain.TabPages.RemoveAt(0);
            }

            this.propertyGridPath.SelectedObject = PathSettings.Instance();

            onInitialize = true;
        }

        private void EnableControls()
        {
            //bool flag = Settings.Instance().IsGeneralSystem() == true || virtualMode.Checked == true;

            //cmbGrabberType.Enabled = flag;
            //cmbDaqType.Enabled = flag;
            //numCamera.Enabled = flag;
            //numLight.Enabled = flag;
            //cmbMotionType.Enabled = flag;
            //cmbDioType.Enabled = flag;
            //numLightType.Enabled = flag;
            //imagingLibrary.Enabled = flag;
            //cmbMachineInterfaceType.Enabled = flag;
            //cmbInterfaceType.Enabled = flag;
            //cmbTriggerSource.Enabled = flag;
            //isAligner.Enabled = flag;
            //interactiveTrigger.Enabled = flag;
            //useFixedInspectionStep.Enabled = flag;
            //numInspectionStep.Enabled = flag;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            OperationSettings operationSettings = OperationSettings.Instance();
            CustomizeSettings customizeSettings = CustomizeSettings.Instance();
            MachineSettings machineSettings = MachineSettings.Instance();
            PathSettings pathSettings = PathSettings.Instance();
            
            operationSettings.NumInspectionStep = (int)numInspectionStep.Value;
            operationSettings.Language = language.Text;
            operationSettings.SystemType = cmbSystemType.Text;
            operationSettings.ImagingLibrary = (ImagingLibrary)imagingLibrary.SelectedIndex;
            operationSettings.UseNonPagedMem = useNonPageMem.Checked;
            operationSettings.UseCuda= useCudaProcesser.Checked;
            operationSettings.ShowFirstPageIndex = comboShowFirstPage.SelectedIndex + 1;
            operationSettings.DataPathType = (DataPathType)dataPathType.SelectedIndex;
            operationSettings.SaveProbeImage = saveProbeImage.Checked;
            operationSettings.SaveTargetImage = saveTargetImage.Checked;
            operationSettings.SaveTargetGroupImage = saveTargetGroupImage.Checked;
            operationSettings.IsAligner = isAligner.Checked;
            operationSettings.ShowScore = checkShowScore.Checked;
            operationSettings.ShowNGImage = checkShowNGImage.Checked;
            operationSettings.UseUserManager = checkUseLoginForm.Checked;
            if (comboBoxTargetGroupImageFormat.Text == "Jpeg")
                operationSettings.TargetGroupImageFormat = ImageFormat.Jpeg;
            else if (comboBoxTargetGroupImageFormat.Text == "Png")
                operationSettings.TargetGroupImageFormat = ImageFormat.Png;
            else
                operationSettings.TargetGroupImageFormat = ImageFormat.Bmp;

            operationSettings.UseFiducialStep = useFiducialStep.Checked;
            operationSettings.UseFixedInspectionStep = useFixedInspectionStep.Checked;
            operationSettings.UseSingleStepModel = useSingleStepModel.Checked;
            operationSettings.UseSingleTargetModel = useSingleTargetModel.Checked;
            operationSettings.UseSimpleLightParamForm = useSimpleLightParamForm.Checked;
            operationSettings.ResultStoringDays = (int)dataStoringDays.Value;
            operationSettings.MinimumFreeSpace = (int)hddMinFreeSpaceValue.Value;
            operationSettings.ResultCopyDays = (int)dataCopyDays.Value;
            operationSettings.LogLevel = (LogLevel)comboBoxLogLevel.SelectedIndex;

            pathSettings.CompanyLogo = companyLogo.Text;
            pathSettings.ProductLogo = productLogo.Text;

            machineSettings.VirtualMode = virtualMode.Checked;
            machineSettings.NumLightType = (int)numLightType.Value;

            customizeSettings.NumOfResultView = numOfResultView.SelectedIndex + 1;
            customizeSettings.UseReportPage = checkUseReportPage.Checked;
            customizeSettings.UseLiveCam = checkUseLiveCam.Checked;
            customizeSettings.ProgramTitle = programTitle.Text;
            customizeSettings.Title = title.Text;
            customizeSettings.Copyright= copyRight.Text;

            if (machineSettings.MachineIfSetting!=null)
                machineSettings.MachineIfSetting.IsVirtualMode = isVirtualMachineIf.Checked;

            devicePanel.SaveSetting();

            operationSettings.Save();
            customizeSettings.Save();
            machineSettings.Save();
            pathSettings.Save();

            bool ok = true;
            if (customConfigPage != null)
                ok = customConfigPage.SaveData();

            if(ok==false)
                this.DialogResult = DialogResult.None;
        }

        private void buttonSelectCompanyLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                companyLogo.Text = dialog.FileName;
            }
        }

        private void buttonSelectProductLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                productLogo.Text = dialog.FileName;
            }
        }

        private void buttonConfigLight_Click(object sender, EventArgs e)
        {
//            LightControlListForm form = new LightControlListForm();
//            form.ShowDialog();
        }

        private void useFixedInspectionStep_CheckedChanged(object sender, EventArgs e)
        {
            numInspectionStep.Enabled = useFixedInspectionStep.Checked;
        }

        private void buttonConfigAlignmentInterface_Click(object sender, EventArgs e)
        {
            AlignDataInterfaceInfoForm form = new AlignDataInterfaceInfoForm();
            form.AlignDataInterfaceInfo = alignDataInterfaceInfo;
            if (form.ShowDialog() == DialogResult.OK)
            {
                alignDataInterfaceInfo = form.AlignDataInterfaceInfo;
            }
        }

        private void configDepthScanner1_Click(object sender, EventArgs e)
        {
        }

        //private void UpdateMachineValue()
        //{
        //    if (virtualMode.Checked == false)
        //    {
        //        if (MessageBox.Show("Do you want to change machine settings?", "Settings", MessageBoxButtons.OKCancel) == DialogResult.OK)
        //        {
        //            OperationSettings.Instance().SystemType = (SystemType)(cmbSystemType.SelectedIndex);
        //            devicePanel.SystemType = (SystemType)(cmbSystemType.SelectedIndex);
        //            MachineSettings.Instance().VirtualMode = virtualMode.Checked;

        //            Settings.Instance().SetDefaultMachineSetting();
        //            SetMachineValue();
        //        }
        //    }
        //    EnableControls();
        //}

        //private void virtualMode_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (initialized)
        //        UpdateMachineValue();
        //}

        //private void cmbSystemType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (initialized)
        //        UpdateMachineValue();
        //}

        //private void buttonConfigMachineInterface_Click(object sender, EventArgs e)
        //{
        //    if (!initialized)
        //    {
        //        return;
        //    }

        //    if ((string)cmbMachineIfType.SelectedItem == MachineIfType.UniEyeExchange.ToString())
        //    {
        //        UmxMachineInterfaceForm umxMachineInterfaceForm = new UmxMachineInterfaceForm();
        //        WcfSetting wcfSetting = (WcfSetting)MachineSettings.Instance().MachineInterfaceSetting;
        //        umxMachineInterfaceForm.Init(wcfSetting);
        //        if (umxMachineInterfaceForm.ShowDialog() == DialogResult.OK)
        //        {
        //            if(wcfSetting == null)
        //            {
        //                wcfSetting = new WcfSetting();
        //            }
        //            wcfSetting.AbsoluteAddress = umxMachineInterfaceForm.AbsoluteAddress;
        //            wcfSetting.BaseAddress = umxMachineInterfaceForm.BaseAddress;
        //            wcfSetting.EndPointName = umxMachineInterfaceForm.EndPointName;
        //        }
        //    }
          
        //}

		 private void configDepthScanner2_Click(object sender, EventArgs e)
        {
        }


        private void buttonSelectWatchdog_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                updatePath.Text = dialog.FileName;
            }
        }

        private void imagingLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imagingLibrary.SelectedIndex < 0)
                return;

            ImagingLibrary selectLibrary = (ImagingLibrary)Enum.Parse(typeof(ImagingLibrary), imagingLibrary.Text);

            bool nonPagedMemEnable = false;

            switch (selectLibrary)
            {
                case ImagingLibrary.MatroxMIL:
                    nonPagedMemEnable = true;
                    break;
            }

            useNonPageMem.Enabled = nonPagedMemEnable;
        }

        private void buttonConfigMachineInterface_Click(object sender, EventArgs e)
        {
            MachineIfSetting settings = (MachineIfSetting)MachineSettings.Instance().MachineIfSetting;

            MachineIfForm form = new MachineIfForm(settings);
            form.ShowDialog();
        }

        private void buttonTestMachineInterface_Click(object sender, EventArgs e)
        {
            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            MachineIfTestForm form = new MachineIfTestForm();

            try
            {
                SimpleProgressForm spForm = new SimpleProgressForm();
                spForm.Show(() =>
                {
                    deviceBox.InitializeMachineIF(MachineSettings.Instance().MachineIfSetting, MachineSettings.Instance().VirtualMode);
                    form.Initialize();
                });
            }
            finally
            {
                form?.ShowDialog();
                deviceBox.Release();
            }
        }

        private void buttonConfigMachineIf_Click(object sender, EventArgs e)
        {
            if (cmbMachineIfList.SelectedIndex == -1)
                return;

            int index = cmbMachineIfList.SelectedIndex;

            MachineIfSetting settings = (MachineIfSetting)MachineSettings.Instance().MachineIfSettingList[index];

            MachineIfForm form = new MachineIfForm(settings);
            form.ShowDialog();
        }

        private void cmbMachineIfListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onInitialize == false)
                return;

            int index = cmbMachineIfList.SelectedIndex;
            MachineSettings.Instance().MachineIfSettingList[index] = MachineIfSetting.Create((MachineIfType)cmbMachineIfListType.SelectedIndex);
        }

        private void cmbMachineIfType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onInitialize == false)
                return;

            MachineSettings.Instance().MachineIfSetting = MachineIfSetting.Create((MachineIfType)cmbMachineIfType.SelectedIndex);
        }

        private void cmbMachineIfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onInitialize == false)
                return;

            int index = cmbMachineIfList.SelectedIndex;
            List<MachineIfSetting> machineIfSettingList = MachineSettings.Instance().MachineIfSettingList;

            if (MachineSettings.Instance().MachineIfSettingList[index] == null)
                return;

            cmbMachineIfListType.SelectedIndex = (int)MachineSettings.Instance().MachineIfSettingList[index].MachineIfType;
            MachineSettings.Instance().MachineIfSettingList[index] = MachineIfSetting.Create((MachineIfType)cmbMachineIfListType.SelectedIndex);
        }

        private void dataRemoveValue_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

using DynMvp.Device.Daq.UI;
using DynMvp.Device.Dio.UI;
using DynMvp.Device.FrameGrabber.UI;
using DynMvp.Device.MotionController.UI;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.Vision;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using WPF.Base.Helpers;
using DialogResult = System.Windows.Forms.DialogResult;

namespace WPF.Base.ViewModels
{
    public enum LanguageSettings
    {
        English,
        Korean,
        Chinese_Simpled,
    }

    public enum SystemType
    {
        None,
        ScriberAlign,
        CurvedGlassAlign,
    }

    public enum ImagingLibraryType
    {
        Open_CV,
        Open_eVision,
        VisionPro,
        MIL,
        Halcon,
        Cuda,
        Custom,
    }

    public enum DeviceListType
    {
        Grabber, Motion, DigitalIo, LightController, Daq
    }

    class ConfigWindowModel : Observable
    {
        #region Variable

        private bool isVirtualMode = false;
        public bool IsVirtualMode
        {
            get => isVirtualMode;
            set => Set(ref isVirtualMode, value);
        }

        private bool isShowScore = false;
        public bool IsShowScore
        {
            get => isShowScore;
            set => Set(ref isShowScore, value);
        }

        private bool isShowNGImage = false;
        public bool IsShowNGImage
        {
            get => isShowNGImage;
            set => Set(ref isShowNGImage, value);
        }

        private bool isSaveTargetImage = false;
        public bool IsSaveTargetImage
        {
            get => isSaveTargetImage;
            set => Set(ref isSaveTargetImage, value);
        }

        private bool isSaveProbeImage = false;
        public bool IsSaveProbeImage
        {
            get => isSaveProbeImage;
            set => Set(ref isSaveProbeImage, value);
        }

        private bool isSaveCameraImage = false;
        public bool IsSaveCameraImage
        {
            get => isSaveCameraImage;
            set => Set(ref isSaveCameraImage, value);
        }

        private bool isSaveResultFigure = false;
        public bool IsSaveResultFigure
        {
            get => isSaveResultFigure;
            set => Set(ref isSaveResultFigure, value);
        }

        private bool useUserManager = false;
        public bool UseUserManager
        {
            get => useUserManager;
            set => Set(ref useUserManager, value);
        }

        private bool highLevelUser = false;
        public bool HighLevelUser
        {
            get => highLevelUser;
            set => Set(ref highLevelUser, value);
        }

        private bool useDoorSensor = false;
        public bool UseDoorSensor
        {
            get => useDoorSensor;
            set => Set(ref useDoorSensor, value);
        }

        private bool useModelBarcode = false;
        public bool UseModelBarcode
        {
            get => useModelBarcode;
            set => Set(ref useModelBarcode, value);
        }

        private bool useRobotStage = false;
        public bool UseRobotStage
        {
            get => useRobotStage;
            set => Set(ref useRobotStage, value);
        }

        private bool useConveyorMotor = false;
        public bool UseConveyorMotor
        {
            get => useConveyorMotor;
            set => Set(ref useConveyorMotor, value);
        }

        private bool useConveyorSystem = false;
        public bool UseConveyorSystem
        {
            get => useConveyorSystem;
            set => Set(ref useConveyorSystem, value);
        }

        private bool useTowerLamp = false;
        public bool UseTowerLamp
        {
            get => useTowerLamp;
            set => Set(ref useTowerLamp, value);
        }

        private bool useSoundBuzzer = false;
        public bool UseSoundBuzzer
        {
            get => useSoundBuzzer;
            set => Set(ref useSoundBuzzer, value);
        }

        private bool isEnableDeviceEditButton = false;
        public bool IsEnableDeviceEditButton
        {
            get => isEnableDeviceEditButton;
            set => Set(ref isEnableDeviceEditButton, value);
        }

        private string[] languageList =
        {
            "English",
            "Korean[ko-kr]",
            "Chinese(Simplified)[zh-cn]",
        };

        private LanguageSettings languageSettings = LanguageSettings.English;
        public LanguageSettings LanguageSettings
        {
            get => languageSettings;
            set => Set(ref languageSettings, value);
        }

        private SystemType systemType = SystemType.None;
        public SystemType SystemType
        {
            get => systemType;
            set => Set(ref systemType, value);
        }

        private ImagingLibrary imagingLibrary = ImagingLibrary.OpenCv;
        public ImagingLibrary ImagingLibrary
        {
            get => imagingLibrary;
            set => Set(ref imagingLibrary, value);
        }

        private DataPathType dataPathType = DataPathType.Model_Day;
        public DataPathType DataPathType
        {
            get => dataPathType;
            set => Set(ref dataPathType, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private string programTitle;
        public string ProgramTitle
        {
            get => programTitle;
            set => Set(ref programTitle, value);
        }

        public string[] imageNameFormatList =
        {
            "Image_{0:0000}_C{1:00}.bmp",
            "Image_C{0:00}_S{1:000}_L{2:00}.bmp"
        };

        private string imageNameFormat;
        public string ImageNameFormat
        {
            get => imageNameFormat;
            set => Set(ref imageNameFormat, value);
        }

        private string companyLogo;
        public string CompanyLogo
        {
            get => companyLogo;
            set => Set(ref companyLogo, value);
        }

        private string productLogo;
        public string ProductLogo
        {
            get => productLogo;
            set => Set(ref productLogo, value);
        }

        private int resultStoringDays;
        public int ResultStoringDays
        {
            get => resultStoringDays;
            set => Set(ref resultStoringDays, value);
        }

        private int numLightType;
        public int NumLightType
        {
            get => numLightType;
            set => Set(ref numLightType, value);
        }

        private GrabberInfoList grabberInfoList;
        public GrabberInfoList GrabberInfoList
        {
            get => grabberInfoList;
            set => Set(ref grabberInfoList, value);
        }

        private MotionInfoList motionInfoList;
        public MotionInfoList MotionInfoList
        {
            get => motionInfoList;
            set => Set(ref motionInfoList, value);
        }

        private DigitalIoInfoList digitalIoInfoList;
        public DigitalIoInfoList DigitalIoInfoList
        {
            get => digitalIoInfoList;
            set => Set(ref digitalIoInfoList, value);
        }

        private LightCtrlInfoList lightCtrlInfoList;
        public LightCtrlInfoList LightCtrlInfoList
        {
            get => lightCtrlInfoList;
            set => Set(ref lightCtrlInfoList, value);
        }

        private DaqChannelPropertyList daqChannelPropertyList;
        public DaqChannelPropertyList DaqChannelPropertyList
        {
            get => daqChannelPropertyList;
            set => Set(ref daqChannelPropertyList, value);
        }

        private IEnumerable dataGridItem;
        public IEnumerable DataGridItem
        {
            get => dataGridItem;
            set => Set(ref dataGridItem, value);
        }

        private DeviceListType deviceListType;
        public DeviceListType DeviceListType
        {
            get => deviceListType;
            set => Set(ref deviceListType, value);
        }

        private object selectedDevice;
        public object SelectedDevice
        {
            get => selectedDevice;
            set
            {
                Set(ref selectedDevice, value);
                IsEnableDeviceEditButton = value != null;
            }
        }

        #endregion

        #region Command

        private ICommand selectDeviceCommand;
        public ICommand SelectDeviceCommand { get => selectDeviceCommand ?? (selectDeviceCommand = new RelayCommand<string>(SelectDeviceAction)); }

        private void SelectDeviceAction(string type)
        {
            if (Enum.TryParse<DeviceListType>(type, out var resultType))
                DeviceListType = resultType;

            UpdateDeviceItemSource();
        }

        private void UpdateDeviceItemSource()
        {
            DataGridItem = null;

            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    DataGridItem = GrabberInfoList;
                    break;
                case DeviceListType.Motion:
                    DataGridItem = MotionInfoList;
                    break;
                case DeviceListType.DigitalIo:
                    DataGridItem = DigitalIoInfoList;
                    break;
                case DeviceListType.LightController:
                    DataGridItem = LightCtrlInfoList;
                    break;
                case DeviceListType.Daq:
                    DataGridItem = DaqChannelPropertyList;
                    break;
            }
        }

        private ICommand addDevice;
        public ICommand AddDevice { get => addDevice ?? (addDevice = new RelayCommand(AddDeviceAction)); }

        private void AddDeviceAction()
        {
            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    {
                        NewGrabberForm form = new NewGrabberForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            GrabberInfo grabberInfo = new GrabberInfo(form.GrabberName, form.GrabberType, form.NumCamera);
                            GrabberInfoList.Add(grabberInfo);

                            CameraConfiguration cameraConfiguration = new CameraConfiguration();
                            for (int i = 0; i < form.NumCamera; i++)
                                cameraConfiguration.AddCameraInfo(CameraInfo.Create(grabberInfo.Type));

                            string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", UniEye.Base.Settings.PathSettings.Instance().Config, grabberInfo.Name);
                            cameraConfiguration.SaveCameraConfiguration(filePath);
                        }
                    }
                    break;
                case DeviceListType.Motion:
                    {
                        NewMotionForm form = new NewMotionForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(form.MotionType);
                            motionInfo.Name = form.MotionName;
                            MotionInfoList.Add(motionInfo);
                        }
                    }
                    break;
                case DeviceListType.DigitalIo:
                    {
                        NewDigitalIoForm form = new NewDigitalIoForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DigitalIoInfoList.Add(form.DigitalIoInfo);
                        }
                    }
                    break;
                case DeviceListType.LightController:
                    {
                        DigitalIoHandler digitalIoHandler = new DigitalIoHandler();
                        digitalIoHandler.Build(DigitalIoInfoList, MotionInfoList);

                        LightConfigForm form = new LightConfigForm();
                        form.LightCtrlName = string.Format("Light {0}", LightCtrlInfoList.Count() + 1);
                        form.DigitalIoHandler = digitalIoHandler;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LightCtrlInfoList.Add(form.LightCtrlInfo);
                        }
                    }
                    break;
                case DeviceListType.Daq:
                    {
                        NewDaqChannelForm form = new NewDaqChannelForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DaqChannelProperty daqChannelProperty = DaqChannelPropertyFactory.Create(form.DaqChannelType);
                            daqChannelProperty.Name = form.DaqChannelName;

                            DaqChannelPropertyList.Add(daqChannelProperty);
                        }
                    }
                    break;
            }

            UpdateDeviceItemSource();
        }

        private ICommand editDevice;
        public ICommand EditDevice { get => editDevice ?? (editDevice = new RelayCommand(EditDeviceAction)); }

        private void EditDeviceAction()
        {
            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    EditGrabber((GrabberInfo)SelectedDevice);
                    break;
                case DeviceListType.Motion:
                    EditMotionInfo((MotionInfo)SelectedDevice);
                    break;
                case DeviceListType.DigitalIo:
                    EditDigitalIoInfo((DigitalIoInfo)SelectedDevice);
                    break;
                case DeviceListType.LightController:
                    EditLightCtrlInfo((LightCtrlInfo)SelectedDevice);
                    break;
                case DeviceListType.Daq:
                    DaqPropertyForm form = new DaqPropertyForm();
                    form.DaqChannelProperty = (DaqChannelProperty)SelectedDevice;
                    form.ShowDialog();
                    //if (form.ShowDialog() == DialogResult.OK)
                    //{
                    //    dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = form.DaqChannelProperty.Name;
                    //}
                    break;
            }

            UpdateDeviceItemSource();
        }


        private void EditGrabber(GrabberInfo grabberInfo)
        {
            Grabber grabber = GrabberFactory.Create(grabberInfo);

            CameraConfiguration cameraConfiguration = new CameraConfiguration();
            string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabberInfo.Name);
            if (File.Exists(filePath) == true)
            {
                cameraConfiguration.LoadCameraConfiguration(filePath);
            }

            if (grabber.SetupCameraConfiguration((int)grabberInfo.NumCamera, cameraConfiguration) == true)
            {
                grabberInfo.NumCamera = cameraConfiguration.CameraInfoList.Count;
                //dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = grabberInfo.NumCamera.ToString();
            }

            if (grabberInfo.NumCamera > 0 && cameraConfiguration.CameraInfoList.Count < (int)grabberInfo.NumCamera)
            {
                System.Windows.MessageBox.Show("The number of camera is less then required number of camera");
                return;
            }

            cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private void EditMotionInfo(MotionInfo motionInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            if (motionInfo is PciMotionInfo)
            {
                PciMotionInfoForm form = new PciMotionInfoForm();
                form.PciMotionInfo = (PciMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is NetworkMotionInfo)
            {
                NetworkMotionInfoForm form = new NetworkMotionInfoForm();
                form.NetworkMotionInfo = (NetworkMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is SerialMotionInfo)
            {
                SerialMotionInfoForm form = new SerialMotionInfoForm();
                form.SerialMotionInfo = (SerialMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is VirtualMotionInfo)
            {
                VirtualMotionInfoForm form = new VirtualMotionInfoForm();
                form.VirtualMotionInfo = (VirtualMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                SelectedDevice = null;
                SelectedDevice = motionInfo;
            }
        }

        private void EditDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;
            if (digitalIoInfo is PciDigitalIoInfo)
            {
                PciDigitalIoInfoForm form = new PciDigitalIoInfoForm();
                form.PciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SlaveDigitalIoInfo)
            {
                SlaveDigitalIoInfoForm form = new SlaveDigitalIoInfoForm();
                form.SlaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
                form.MotionInfoList = motionInfoList;
                dialogResult = form.ShowDialog();
            }

            //if (dialogResult == DialogResult.OK)
            //{
            //    dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = digitalIoInfo.Name;
            //}
        }

        private void EditDaqChannelProperty(DaqChannelProperty daqChannelProperty)
        {
            DaqPropertyForm form = new DaqPropertyForm();
            form.DaqChannelProperty = (DaqChannelProperty)daqChannelProperty;
            if (form.ShowDialog() == DialogResult.OK)
            {
                //dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = daqChannelProperty.Name;
            }
        }

        private void EditLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            DigitalIoHandler digitalIoHandler = new DigitalIoHandler();
            digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

            LightConfigForm form = new LightConfigForm();
            form.LightCtrlInfo = lightCtrlInfo;
            form.DigitalIoHandler = digitalIoHandler;
            if (form.ShowDialog() == DialogResult.OK)
            {
                lightCtrlInfoList.Remove(lightCtrlInfo);

                lightCtrlInfo = form.LightCtrlInfo;

                lightCtrlInfoList.Add(lightCtrlInfo);

                //dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = lightCtrlInfo.Name;
                //dataGridViewDeviceList.SelectedRows[0].Cells[2].Value = lightCtrlInfo.Type.ToString();
                //dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = lightCtrlInfo.NumChannel.ToString();
                //dataGridViewDeviceList.SelectedRows[0].Tag = lightCtrlInfo;
            }
        }

        private ICommand deleteDevice;
        public ICommand DeleteDevice { get => deleteDevice ?? (deleteDevice = new RelayCommand(DeleteDeviceAction)); }

        private void DeleteDeviceAction()
        {
            if (System.Windows.MessageBox.Show("Do you want to delete this?", "Warning", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.OK)
            {
                switch (DeviceListType)
                {
                    case DeviceListType.Grabber:
                        DeleteGrabber((GrabberInfo)SelectedDevice);
                        break;
                    case DeviceListType.Motion:
                        DeleteMotionInfo((MotionInfo)SelectedDevice);
                        break;
                    case DeviceListType.DigitalIo:
                        DeleteDigitalIoInfo((DigitalIoInfo)SelectedDevice);
                        break;
                    case DeviceListType.LightController:
                        DeleteLightCtrlInfo((LightCtrlInfo)SelectedDevice);
                        break;
                    case DeviceListType.Daq:
                        DeleteDaqChannelProperty((DaqChannelProperty)SelectedDevice);
                        break;
                }

                UpdateDeviceItemSource();
            }
        }

        private void DeleteGrabber(GrabberInfo selectedDevice)
        {
            grabberInfoList.Remove(selectedDevice);
        }

        private void DeleteMotionInfo(MotionInfo selectedDevice)
        {
            MotionInfoList.Remove(selectedDevice);
        }

        private void DeleteDigitalIoInfo(DigitalIoInfo selectedDevice)
        {
            DigitalIoInfoList.Remove(selectedDevice);
        }

        private void DeleteLightCtrlInfo(LightCtrlInfo selectedDevice)
        {
            LightCtrlInfoList.Remove(selectedDevice);
        }

        private void DeleteDaqChannelProperty(DaqChannelProperty selectedDevice)
        {
            DaqChannelPropertyList.Remove(selectedDevice);
        }

        private ICommand okButtonClick;
        public ICommand OKButtonClick { get => okButtonClick ?? (okButtonClick = new RelayCommand<Window>(OKButtonAction)); }

        private void OKButtonAction(Window wnd)
        {
            wnd.DialogResult = true;

            SaveParameter();
            
            wnd.Close();
        }

        private ICommand cancelButtonClick;
        public ICommand CancelButtonClick { get => cancelButtonClick ?? (cancelButtonClick = new RelayCommand<Window>(CancelButtonAction)); }

        private void CancelButtonAction(Window wnd)
        {
            wnd.DialogResult = false;
            wnd.Close();
        }

        #endregion

        DataGrid deviceGridList;

        public ConfigWindowModel(DataGrid gridCtrl)
        {
            deviceGridList = gridCtrl;

            LoadParameter();

            SelectDeviceAction(DeviceListType.Grabber.ToString());
        }

        private void SaveParameter()
        {
            OperationSettings operationSettings = OperationSettings.Instance();
            CustomizeSettings customizeSettings = CustomizeSettings.Instance();
            MachineSettings machineSettings = MachineSettings.Instance();
            PathSettings pathSettings = PathSettings.Instance();

            operationSettings.Save();
            customizeSettings.Save();
            machineSettings.Save();
            pathSettings.Save();
        }

        private void LoadParameter()
        {
            //OperationSettings operationSettings = OperationSettings.Instance();
            //CustomizeSettings customizeSettings = CustomizeSettings.Instance();
            //MachineSettings machineSettings = MachineSettings.Instance();
            //PathSettings pathSettings = PathSettings.Instance();

            //virtualMode.Checked = machineSettings.VirtualMode;

            //checkShowScore.Checked = operationSettings.ShowScore;
            //checkShowNGImage.Checked = operationSettings.ShowNGImage;
            //useFiducialStep.Checked = operationSettings.UseFiducialStep;
            //useFixedInspectionStep.Checked = operationSettings.UseFixedInspectionStep;
            //numInspectionStep.Enabled = useFixedInspectionStep.Checked;
            //numInspectionStep.Value = operationSettings.NumInspectionStep;
            ////cmbSystemType.SelectedText = operationSettings.SystemType;
            //imagingLibrary.SelectedIndex = (int)operationSettings.ImagingLibrary;
            //useCudaProcesser.Checked = operationSettings.UseCuda;
            //useNonPageMem.Checked = operationSettings.UseNonPagedMem;
            //language.Text = operationSettings.Language.ToString();
            //dataStoringDays.Value = (decimal)operationSettings.ResultStoringDays;
            //hddMinFreeSpaceValue.Value = (decimal)operationSettings.MinimumFreeSpace;
            //dataCopyDays.Value = (decimal)operationSettings.ResultCopyDays;
            //comboBoxLogLevel.SelectedIndex = (int)operationSettings.LogLevel;

            //comboShowFirstPage.SelectedIndex = operationSettings.ShowFirstPageIndex - 1;
            //useSingleTargetModel.Checked = operationSettings.UseSingleTargetModel;
            //dataPathType.SelectedIndex = (int)operationSettings.DataPathType;
            //saveTargetImage.Checked = operationSettings.SaveTargetImage;
            //saveProbeImage.Checked = operationSettings.SaveProbeImage;
            //checkUseLoginForm.Checked = operationSettings.UseUserManager;
            //saveTargetGroupImage.Checked = operationSettings.SaveTargetGroupImage;
            //useSimpleLightParamForm.Checked = operationSettings.UseSimpleLightParamForm;
            //comboBoxTargetGroupImageFormat.Text = operationSettings.TargetGroupImageFormat.ToString();

            //if (machineSettings.MachineIfSetting != null)
            //{
            //    cmbMachineIfType.SelectedIndex = (int)machineSettings.MachineIfSetting.MachineIfType;
            //    isVirtualMachineIf.Checked = machineSettings.MachineIfSetting.IsVirtualMode;
            //}

            //if (machineSettings.MachineIfSettingList.Count > 0)
            //{
            //    for (int i = 0; i < machineSettings.MachineIfSettingList.Count; i++)
            //        cmbMachineIfList.Items.Add(i.ToString());

            //    cmbMachineIfList.SelectedIndex = 0;
            //    if (machineSettings.MachineIfSettingList[0] == null)
            //        cmbMachineIfList.SelectedIndex = 0;
            //    //else
            //    //    cmbMachineIfList.SelectedIndex = (int)machineSettings.MachineIfSettingList[0].MachineIfType;
            //    cmbMachineIfList.Update();
            //}

            //numLightType.Value = machineSettings.NumLightType;

            //companyLogo.Text = pathSettings.CompanyLogo;
            //productLogo.Text = pathSettings.ProductLogo;

            //title.Text = customizeSettings.Title;
            //devicePanel.Title = title.Text;
            //programTitle.Text = customizeSettings.ProgramTitle;
            //numOfResultView.SelectedIndex = customizeSettings.NumOfResultView - 1;
            //checkUseReportPage.Checked = customizeSettings.UseReportPage;
            //checkUseLiveCam.Checked = customizeSettings.UseLiveCam;

            //if (lowLevelUser == true)
            //{
            //    tabMain.TabPages.RemoveAt(4);
            //    tabMain.TabPages.RemoveAt(3);
            //    tabMain.TabPages.RemoveAt(1);
            //    tabMain.TabPages.RemoveAt(0);
            //}

            //this.propertyGridPath.SelectedObject = PathSettings.Instance();

            //onInitialize = true;
        }
    }
}

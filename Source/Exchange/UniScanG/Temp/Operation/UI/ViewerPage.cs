//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Windows.Forms;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using DynMvp.Devices;
//using DynMvp.Devices.Light;
//using System.IO;
//using DynMvp.Inspection;
//using DynMvp.InspData;
//using DynMvp.UI.Touch;
//using UniEye.Base.Settings;
//using DynMvp.UI;
//using UniEye.Base.Data;
//using UniEye.Base;
//using System.Drawing.Imaging;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Threading;
//using DynMvp.Authentication;
//using UniEye.Base.UI;
//using System.ComponentModel;
//using DynMvp.Devices.FrameGrabber;
//using Euresys;
//using DynMvp.Devices.MotionController;
//using DynMvp.Devices.Dio;

////using SamsungElectro.MPIS.Data;

//namespace UniScanG.Temp
//{
//    public partial class ViewerPage : UserControl, IInspectionPage, IMainTabPage
//    {
//        Production curProduction = null;
//        ModellerPageExtender modellerPageExtender = null;

//        VisionPropertyGridData visionPropertyGridData = null;
//        MachinePropertyGridData machinePropertyGridData = null;

//        public IInspectionPanel InspectionPanel
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public ViewerPage()
//        {
//            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor ViewerPage Page");

//            InitializeComponent();

//            modellerPageExtender = new UniScanGViewerPageExtender();

//            LogHelper.Debug(LoggerType.StartUp, "End Constructor ViewerPage Page");
//        }

//        public void EnableControls()
//        {

//        }

//        public void TabPageVisibleChanged(bool visibleFlag)
//        {

//        }

//        private void ViewerPage_VisibleChanged(object sender, EventArgs e)
//        {
//            UpdateProperty();
//        }

//        private void UpdateProperty()
//        {
//            UpdateVisionProperty();
//            UpdateMachineProperty();
//        }

//        private void UpdateVisionProperty()
//        {
//            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
//            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(0);
//            CameraGenTL cameraGenTL = null;

//            if (imageDevice != null && imageDevice is CameraGenTL)
//            {
//                cameraGenTL = (CameraGenTL)imageDevice;
//            }

//            if (visionPropertyGridData == null)
//            { 
//                visionPropertyGridData = new VisionPropertyGridData(cameraGenTL);
//                propertyGridVision.SelectedObject = visionPropertyGridData;
//            }

//            visionPropertyGridData.Update();
//            labelVisionCamera.Text = string.Format("{0}({1})", visionPropertyGridData.DeviceModel, visionPropertyGridData.DeviceSerial);
//            labelVisionState.Text = visionPropertyGridData.DeviceState;
//        }

//        private void UpdateMachineProperty()
//        {
//            if (machinePropertyGridData == null)
//            {
//                machinePropertyGridData = new MachinePropertyGridData(SystemManager.Instance().DeviceController.Convayor);
//                propertyGridMachine.SelectedObject = machinePropertyGridData;

//                machinePropertyGridData.Update();
//            }
//        }

//        private void propertyGridVision_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
//        {
//            visionPropertyGridData.OnValueChanged();
//        }

//        private void propertyGridMachine_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
//        {
//            machinePropertyGridData.OnValueChanged();
//        }

//        public Production GetCurrentProduction()
//        {
//            return curProduction;
//        }
//    }

//    [DefaultPropertyAttribute("Misc")]
//    public class VisionPropertyGridData
//    {
//        CameraGenTL cameraGenTL = null;

//        // Vision Property Items
//        public enum eScanMode { TDI, Area }
//        public enum eScanDirection { Forward, Reverse, LineIn }

//        string deviceModel;
//        string deviceSerial;
//        string deviceState;

//        eScanMode scanMode;
//        int scanWidth;
//        int scanLength;
//        eScanDirection scanDirection;
//        float scanRate;
//        float gain;
//        int tdiStage;

//        int frontLigthValue;
//        int backLigthValue;

//        [Browsable(false)]
//        public string DeviceModel
//        {
//            get { return deviceModel; }
//        }

//        [Browsable(false)]
//        public string DeviceSerial
//        {
//            get { return deviceSerial; }
//        }

//        [Browsable(false)]
//        public string DeviceState
//        {
//            get { return deviceState; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public eScanMode ScanMode
//        {
//            get { return scanMode; }
//            set { scanMode = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public int ScanWidth
//        {
//            get { return scanWidth; }
//            set { scanWidth = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public int ScanLength
//        {
//            get { return scanLength; }
//            set { scanLength = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public eScanDirection ScanDirection
//        {
//            get { return scanDirection; }
//            set { scanDirection = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public float ScanRate
//        {
//            get { return scanRate; }
//            set { scanRate = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public int TdiStage
//        {
//            get { return tdiStage; }
//            set { tdiStage = value; }
//        }

//        [CategoryAttribute("Scan Setting")]
//        public float Gain
//        {
//            get { return gain; }
//            set { gain = value; }
//        }

//        public int FrontLigthValue
//        {
//            get { return frontLigthValue; }
//            set { frontLigthValue = value; }
//        }

//        public int BackLigthValue
//        {
//            get { return backLigthValue; }
//            set { backLigthValue = value; }
//        }

//        public VisionPropertyGridData(CameraGenTL cameraGenTL)
//        {
//            this.cameraGenTL = cameraGenTL;
//        }

//        public void Update()
//        {
//            if (cameraGenTL == null)
//            {
//                this.deviceState = "N/C";
//                return;
//            }

//            deviceModel = cameraGenTL.GetPropertyData("DeviceModelName");
//            deviceSerial = cameraGenTL.GetPropertyData("DeviceSerialNumber");

//            scanMode = (eScanMode)Enum.Parse(typeof(eScanMode), cameraGenTL.GetPropertyData("OperationMode"));

//            tdiStage = Convert.ToInt32(cameraGenTL.GetPropertyData( "TDIStages").Substring(3));

//            scanDirection = (eScanDirection)Enum.Parse(typeof(eScanDirection), cameraGenTL.GetPropertyData("ScanDirection"));

//            scanRate = Convert.ToSingle(cameraGenTL.GetPropertyData( "AcquisitionLineRate"));

//            scanWidth= Convert.ToInt32(cameraGenTL.GetPropertyData( "Width"));
//            scanLength = Convert.ToInt32(cameraGenTL.GetPropertyData( "ScanLength"));

//            //float gain; grabber.setIntegerStreamModule("ScanLength", height);


//            //grabber.setIntegerStreamModule("BufferHeight", height);
//        }

//        public void OnValueChanged()
//        {
//            cameraGenTL.SetPropertyData("OperationMode", scanMode.ToString());

//            cameraGenTL.SetPropertyData( "TDIStages", string.Format("TDI{0}", tdiStage));

//            cameraGenTL.SetPropertyData( "ScanDirection", scanDirection.ToString());

//            cameraGenTL.SetPropertyData( "AcquisitionLineRate", scanRate.ToString());

//            cameraGenTL.SetPropertyData( "Width", scanWidth.ToString());
//            cameraGenTL.SetPropertyData( "ScanLength", scanLength.ToString());
//        }
//    }

//    [DefaultPropertyAttribute("Misc")]
//    public class MachinePropertyGridData
//    {
//        AxisHandler axisHandler;
//        DigitalIoHandler digitalIoHandler;

//        // Machine Property Items
//        float operationSpeed;
//        float accelTime;
//        float decelTime;
        
//        //[CategoryAttribute("Operation Speed")]
//        public float OperationSpeed
//        {
//            get { return operationSpeed; }
//            set { operationSpeed = value; }
//        }

//        //[CategoryAttribute("Scan Setting")]
//        public float AccelTime
//        {
//            get { return accelTime; }
//            set { accelTime = value; }
//        }

//        //[CategoryAttribute("Scan Setting")]
//        public float DecelTime
//        {
//            get { return decelTime; }
//            set { decelTime = value; }
//        }

//        public MachinePropertyGridData(AxisHandler axisHandler)
//        {
//            this.axisHandler = axisHandler;
//        }

//        public void Update()
//        {
//            if (axisHandler != null)
//            {
//                operationSpeed = (float)axisHandler.GetAxisByNo(0).AxisParam.JogParam.MaxVelocity;
//                accelTime = (float)axisHandler.GetAxisByNo(0).AxisParam.JogParam.AccelerationTimeMs;
//                decelTime = (float)axisHandler.GetAxisByNo(0).AxisParam.JogParam.DecelerationTimeMs;
//            }
//        }

//        public void OnValueChanged()
//        {
//            axisHandler.GetAxisByNo(0).AxisParam.JogParam.MaxVelocity = operationSpeed;
//            axisHandler.GetAxisByNo(0).AxisParam.JogParam.AccelerationTimeMs = accelTime;
//            axisHandler.GetAxisByNo(0).AxisParam.JogParam.DecelerationTimeMs = decelTime;
//        }
//    }
//}
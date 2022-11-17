//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using DynMvp.Devices.MotionController;
//using DynMvp.Base;
//using DynMvp.Devices.Light;
//using UniEye.Base;
//using UniScanG.Operation.Data;
//using DynMvp.Devices.Dio;
//using DynMvp.UI.Touch;
//using System.Threading;

//namespace UniScanG.Temp
//{
//    public partial class MachineControlPanel : UserControl
//    {
//        bool onUpdate = false;
        
//        enum TimeUnitType
//        {
//            MeterPerSecond, MeterPerMinute
//        }

//        TimeUnitType timeUnitType = TimeUnitType.MeterPerMinute;

//        AxisHandler motionStage = null;
//        public MachineControlPanel(AxisHandler axisHandler)
//        {
//            InitializeComponent();

//            this.motionStage = axisHandler;
//            if (axisHandler != null)
//            {
//                axisHandler?.TurnOnServo(true);
//            }

//            comboBoxSpeedUnit.Items.Clear();
//            //comboBoxSpeedUnit.Items.AddRange(Enum.GetNames(typeof(TimeUnitType)));
//            comboBoxSpeedUnit.Items.Add("[m/min]");
//            comboBoxSpeedUnit.Items.Add("[m/sec]");

//            onUpdate = true;
//            comboBoxSpeedUnit.SelectedIndex = 0;
//            ultraTrackBarSpeed.MaxValue = 120;
//            ultraTrackBarSpeed.Value = 55;

//            ultraTrackBarAccel.MinValue = 1;
//            ultraTrackBarAccel.MaxValue = 100;

//            if (axisHandler == null)
//            {
//                groupBoxDirection.Visible = false;
//                //buttonMoveForward.Visible = false;
//                //buttonMoveBackward.Visible = false;
//                //buttonMoveStop.Visible = false;
//            }
//            onUpdate = false;
//        }

//        private void SetTriggerTime(Axis axis, double signalTime, double potition)
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::SetTriggerTime");
//            if (axis.Motion is MotionAjin)
//            {
//                MotionAjin motionAjin = (MotionAjin)axis.Motion;
//                motionAjin.SetTriggerLevel(axis.AxisNo, DynMvp.Devices.TriggerType.RisingEdge, signalTime, true);
//                motionAjin.SetTriggerPosition(axis.AxisNo, potition, false);
//            }
//        }

//        private void ClearTriggerTime(Axis axis)
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::ClearTriggerTime");

//            if (axis.Motion is MotionAjin)
//            {
//                MotionAjin motionAjin = (MotionAjin)axis.Motion;
//                bool ok = motionAjin.ResetTrigger(axis.AxisNo);
//                System.Diagnostics.Debug.Assert(ok);
//            }
//        }

//        private void buttonSpeedFast_Click(object sender, EventArgs e)
//        {
//            Axis axis = motionStage.GetAxis("X");
//            axis.AxisParam.JogParam.MaxVelocity = 2294;
//        }

//        private void buttonSpeedMedium_Click(object sender, EventArgs e)
//        {
//            Axis axis = motionStage.GetAxis("X");
//            axis.AxisParam.JogParam.MaxVelocity = 1720;
//        }

//        private void buttonSpeedSlow_Click(object sender, EventArgs e)
//        {
//            Axis axis = motionStage.GetAxis("X");
//            axis.AxisParam.JogParam.MaxVelocity = 1500;
//        }

//        private void buttonMoveForward_Click(object sender, EventArgs e)
//        {
//            MoveForward();
//        }

//        public bool CheckDoorLock()
//        {
//            UniEye.Base.Settings.MachineSettings machineSettings = UniEye.Base.Settings.MachineSettings.Instance();
//            if (machineSettings.UseDoorSensor == false)
//                return true;

//            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;
//            List<IoPort> doorIoPortList = new List<IoPort>();
//            SystemManager.Instance().DeviceBox.PortMap.GetInDoorPorts(doorIoPortList);

//            bool doorState = true;

//            foreach (IoPort doorIoPort in doorIoPortList)
//            {
//                if (digitalIoHandler.ReadInput(doorIoPort) == false)
//                    doorState = false;
//            }
//            //digitalIoHandler.WriteOutput(doorIoPort, false);

//            return doorState;
//        }

//        public void MoveForward()
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::MoveForward");
//            if (ErrorManager.Instance().IsAlarmed())
//                ErrorManager.Instance().ResetAlarm();

//            if (motionStage == null)
//                return;

//            if (CheckDoorLock() == false)
//            {
//                ErrorManager.Instance().Report((int)ErrorSection.Safety, (int)SafetyError.DoorOpen, ErrorLevel.Error,
//                    ErrorSection.Safety.ToString(), SafetyError.DoorOpen.ToString(), "Door is opened");

//                return;
//            }

//            //float speedMPS = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS;
//            //float accelSec = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorAccelSec;
//            //Move(accelSec, speedMPS, false);
//        }

//        private float GetTriggerPosition()
//        {
//            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count == 0)
//                return 0.0170183f;

//            DynMvp.Vision.Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];

//            // 1회전당 이동 거리 [um/rev]
//            float distPerRev = (float)(2 * Math.PI * 50000);

//            // 거리당 회전 횟수 [rev/um]
//            float revPerMm = 1 / distPerRev;

//            // 단위 거리(Calibrate Resolution) [um/Unit]
//            float unitPerMm = calibration.PelSize.Height;

//            // 단위 거리당 회전 횟수 [rev/um]*[um/Unit]=[rev/Unit]
//            float revPerUnit = revPerMm * unitPerMm;

//            // 회전 횟수당 회전 각도 [deg/rev]
//            float degPerRev = 360.0f;

//            // 유닛 거리당 회전 각도 [rev/Unit]*[deg/rev]=[deg/Unit]
//            float degPerUnit = revPerUnit * degPerRev;

//            return degPerUnit;
//        }

//        private void buttonMoveBackward_Click(object sender, EventArgs e)
//        {
//            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//            //SimpleProgressForm form = new SimpleProgressForm("Wave Moving");
//            //form.Show(() =>
//            //{
//            //    while (cancellationTokenSource.IsCancellationRequested==false)
//            //    {
//            //        for (int i = 0; i < 2; i++)
//            //        {
//            //            switch(i)
//            //            {
//            //                case 0:
//            //                    MoveForward();
//            //            Thread.Sleep(500);
//            //                    break;
//            //                case 1:
//            //                    //MoveStop();
//            //                    MoveStop();
//            //                    break;
//            //            }

//            //            if (cancellationTokenSource.IsCancellationRequested)
//            //            {
//            //                MoveStop();
//            //                break;
//            //            }
//            //        }
//            //    }
//            //}, cancellationTokenSource);
//            MoveBackward();
//        }
        
//        public void MoveBackward()
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::MoveBackward");

//            if (ErrorManager.Instance().IsAlarmed())
//                ErrorManager.Instance().ResetAlarm();

//            if (motionStage == null)
//                return;

//            if (CheckDoorLock() == false)
//            {
//                ErrorManager.Instance().Report((int)ErrorSection.Safety, (int)SafetyError.DoorOpen, ErrorLevel.Error,
//                    ErrorSection.Safety.ToString(), SafetyError.DoorOpen.ToString(), "Door is opened");

//                return;
//            }


//            //float speedMPS = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS;
//            //float accelSec = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorAccelSec;
//            //Move(accelSec, speedMPS, true);
//        }

//        private new void Move(float accelSec, float speedMPS, bool backWord)
//        {
//            SetSpeed(accelSec, speedMPS);

//            Axis axis = motionStage.GetAxis("X");
//            float trigPos = GetTriggerPosition();
//            SetTriggerTime(axis, 5, trigPos);
//            //SetTriggerTime(axis, 5, 0.004);

//            //SetTriggerTime(axis, 3, 0.017);

//            if (axis.Motion is MotionAjin)
//            {
//                MotionAjin motionAjin = axis.Motion as MotionAjin;
//                System.Threading.Thread.Sleep(500);
//            }

//            axis.ContinuousMove(null, backWord);

//            //int stableTime = (int)(((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorAccelSec * 1000) + 500;
//            //System.Threading.Thread.Sleep(stableTime);
//        }

//        private void SetSpeed(float accelSec, float speedMPS)
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::SetSpeed");
        
//            if (motionStage == null)
//                return;

//            double scale = Math.PI / 3600;
//            Axis axis = motionStage.GetAxis("X");
//            axis.AxisParam.JogParam.AccelerationTimeMs =
//                axis.AxisParam.JogParam.DecelerationTimeMs = accelSec * 1000;
//            axis.AxisParam.JogParam.MaxVelocity = speedMPS / scale;
//        }

//        private void buttonMoveStop_Click(object sender, EventArgs e)
//        {
//            MoveStop();
//        }

//        public void MoveStop()
//        {
//            LogHelper.Debug(LoggerType.Machine, "MachineControlPanel::MoveStop");

//            if (motionStage == null)
//                return;

//            if (ErrorManager.Instance().IsAlarmed())
//                ErrorManager.Instance().ResetAlarm();

//            if (motionStage == null)
//                return;

//            Axis axis = motionStage.GetAxis("X");
//            ClearTriggerTime(axis);
//            axis.StopMove();
//            axis.WaitMoveDone();

//            if (axis.Motion is MotionAjin)
//            {
//                //System.Threading.Thread.Sleep(500);
//                MotionAjin motionAjin = axis.Motion as MotionAjin;
//                motionAjin.WriteOutputPort(0, 3, false);
//            }
//        }

//        private void ultraTrackBarSpeed_ValueChanged(object sender, EventArgs e)
//        {
//            float targetSpeed = ultraTrackBarSpeed.Value;    // [m/?]

//            switch (timeUnitType)
//            {
//                case TimeUnitType.MeterPerMinute:
//                    labelSpeed.Text = targetSpeed.ToString("0");
//                    targetSpeed /= 60.0f; // [m/min] => [m/sec]
//                    break;
//                case TimeUnitType.MeterPerSecond:
//                    targetSpeed *= 0.1f; // [10m/sec] => [m/sec]
//                    labelSpeed.Text = targetSpeed.ToString("0.0");
//                    break;
//            }

//            if (onUpdate)
//                return;

//            //((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS = targetSpeed;
//        }

//        private void comboBoxTimeUnitType_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            //if (onUpdate)
//            //    return;

//            //switch (comboBoxSpeedUnit.SelectedIndex)
//            //{
//            //    case 1:
//            //        timeUnitType = TimeUnitType.MeterPerMinute;
//            //        break;
//            //    case 0:
//            //        timeUnitType = TimeUnitType.MeterPerSecond;
//            //        break;
//            //}

//            //float curValue = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS;
//            //switch (timeUnitType)
//            //{
//            //    case TimeUnitType.MeterPerMinute:
//            //        ultraTrackBarSpeed.MaxValue = 120;
//            //        ultraTrackBarSpeed.Value = (int)(curValue*60);
//            //        break;
//            //    case TimeUnitType.MeterPerSecond:
//            //        ultraTrackBarSpeed.MaxValue = 20;
//            //        ultraTrackBarSpeed.Value = (int)(curValue*10);
//            //        break;
//            //}
//        }
        
//        private void ultraTrackBarAccel_ValueChanged(object sender, EventArgs e)
//        {
//            float valueSec = ultraTrackBarAccel.Value / 10.0f;
//            this.labelAccel.Text = valueSec.ToString("0.0");

//            if (onUpdate)
//                return;

//            //((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorAccelSec = valueSec;
//        }
        
//        private void MachineControlPanel_Load(object sender, EventArgs e)
//        {
//            UpdateData();
//        }

//        private void UpdateData()
//        {
//            //if (SystemManager.Instance().CurrentModel == null)
//            //    return;

//            //onUpdate = true;
//            //Model curModel = (Model)SystemManager.Instance().CurrentModel;
//            //ModelDescription modelDescription = (ModelDescription)curModel.ModelDescription;
            
//            //// Speed
//            //if (modelDescription.ConvayorSpeedMPS == 0)
//            //    modelDescription.ConvayorSpeedMPS = 1.5f;

//            //float scale = 10;
//            //if(comboBoxSpeedUnit.SelectedIndex ==0)
//            //    scale *= 6f;
//            //ultraTrackBarSpeed.Value = (int)Math.Round(modelDescription.ConvayorSpeedMPS * scale);

//            //// Accel
//            //ultraTrackBarAccel.Value = (int)(modelDescription.ConvayorAccelSec*10.0f);

//            //onUpdate = false;
//        }

//        private void MachineControlPanel_VisibleChanged(object sender, EventArgs e)
//        {
//            if (this.Visible)
//                UpdateData();
//        }
//    }
//}

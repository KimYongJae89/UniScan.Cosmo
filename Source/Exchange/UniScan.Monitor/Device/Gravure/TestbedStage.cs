using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Device.Device.MotionController;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using UniEye.Base.UI;

namespace UniScan.Monitor.Device.Gravure
{
    internal class StageState
    {
        int servoOnCnt = 0;
        int servoRunCnt = 0;
        bool servoFault = false;
        public void IncServeOnCnt() { servoOnCnt++; }
        public void DecServeOnCnt() { if (servoOnCnt > 0) servoOnCnt--; }
        public void IncServeRunCnt() { servoRunCnt++; }
        public void DecServeRunCnt() { if (servoRunCnt > 0) servoRunCnt--; }
        public void SetServoFault(bool serviFault) { this.servoFault = serviFault; }
        public bool IsServoFault() { return servoFault; }
        public bool IsServoOn() { return servoOnCnt > 0; }
        public bool IsServoRun() { return servoRunCnt > 0; }
    }

    public class TestbedStage: DeviceControllerExtender
    {
        StageState stageState = null;

        public TestbedStage(DeviceController deviceController) : base(deviceController) { }

        public void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.stageState = new StageState();

            AxisHandler axisHandler = deviceController.Convayor;
            if (axisHandler != null)
            {
                axisHandler.AxisList.ForEach(f =>
                {
                    float pulse = f.ToPulse(14.0f);
                    f.Motion.StartCmp(f.AxisNo, 0, pulse, true);
                });
            }

            InitializeIoEventHandler(deviceBox);
            InitializeMotionEventHandler(deviceBox);
        }
        
        public void InitializeIoEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            PortMap portMap = (PortMap)deviceBox.PortMap;

            IoPort ioPort = portMap.GetOutPort(PortMap.IoPortName.OutDoorOpen);
            if (ioPort != null)
                deviceBox.DigitalIoHandler.SetOutputDeactive(ioPort);

            IoEventHandler airFan = new IoEventHandler("AirFan", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutAirFan), IoEventHandlerDirection.Out)
            {
                OnActivate = airFan_OnInputOn,
                OnDeactivate = airFan_OnInputOff
            };
            deviceController.AddIoEventHandler(airFan);

            IoEventHandler roomLight = new IoEventHandler("RoomLight", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutRoomLight), IoEventHandlerDirection.Out)
            {
                OnActivate = roomLight_OnInputOn,
                OnDeactivate = roomLight_OnInputOff
            };
            deviceController.AddIoEventHandler(roomLight);

            IoEventHandler doorLock = new IoEventHandler("DoorLock", deviceBox.DigitalIoHandler, portMap.GetOutPort(PortMap.IoPortName.OutDoorOpen), IoEventHandlerDirection.Out)
            {
                OnActivate = doorLock_OnInputOn,
                OnDeactivate = doorLock_OnInputOff
            };
            deviceController.AddIoEventHandler(doorLock);
        }

        private bool airFan_OnInputOn(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Fan", eventSource.IsActivate.ToString());
            return true;
        }

        private bool airFan_OnInputOff(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Fan", "OFF");
            return true;
        }

        private bool roomLight_OnInputOn(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Light", "ON");
            return true;
        }

        private bool roomLight_OnInputOff(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Light", "OFF");
            return true;
        }

        private bool doorLock_OnInputOn(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Door", "On");
            return true;
        }

        private bool doorLock_OnInputOff(IoEventHandler eventSource)
        {
            IMainForm mainForm = SystemManager.Instance().MainForm;
            if (mainForm == null)
                return false;

            //mainForm.UpdateControl("Door", "OFF");
            return true;
        }

        //public bool OnEmergencyPressed(bool curValue)
        //{
        //    if (curValue)
        //        ErrorManager.Instance().Report(0, ErrorLevel.Fatal, "System", "EMG S/W Pushed", "EMG S/W Pushed");
        //    return true;
        //}

        //public bool OnDoorOpened(bool curValue)
        //{
        //    if (curValue)
        //        throw new NotImplementedException();
        //    return true;
        //}

        //public bool OnAirPressure(bool curValue)
        //{
        //    if (curValue == false)    // set 0 when air pressure is low
        //        throw new NotImplementedException();
        //    return true;
        //}

        public void InitializeMotionEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            foreach (AxisHandler axisHandler in deviceBox.AxisConfiguration)
            {
                MotionEventHandler motionEventHandler = new MotionEventHandler(axisHandler.Name, axisHandler, -1);
                motionEventHandler.OnServoOn = motionEventHandler_OnServoOn;
                motionEventHandler.OnServoOff = motionEventHandler_OnServoOff;
                motionEventHandler.OnStartMove = motionEventHandler_OnStartMove;
                motionEventHandler.OnMoveDone = motionEventHandler_OnMoveDone;
                motionEventHandler.OnFault = motionEventHandler_OnFault;
                deviceController.AddMotionEventHandler(motionEventHandler);
            }
        }

        private bool motionEventHandler_OnMoveDone(MotionEventHandler eventSource)
        {
            stageState.DecServeRunCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnStartMove(MotionEventHandler eventSource)
        {
            stageState.IncServeRunCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnServoOn(MotionEventHandler eventSource)
        {
            stageState.IncServeOnCnt();
            return true;
        }

        private bool motionEventHandler_OnServoOff(MotionEventHandler eventSource)
        {
            stageState.DecServeOnCnt();
            //throw new NotImplementedException();
            return true;
        }

        private bool motionEventHandler_OnFault(MotionEventHandler eventSource)
        {
            //motionState.DecServeOnCnt();
            //throw new NotImplementedException();
            ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)ErrorSubSection.CommonReason, ErrorLevel.Error, "Motion", "Motion Fault", "Motion is faulted");
            return true;
        }

        public TowerLampState GetTowerlampState()
        {
                if (ErrorManager.Instance().IsError() || stageState.IsServoFault())
                    return deviceController.TowerLamp.GetState(TowerLampStateType.Alarm);
                if (stageState.IsServoRun())
                    return deviceController.TowerLamp.GetState(TowerLampStateType.Working);
                if (stageState.IsServoOn())
                    return deviceController.TowerLamp.GetState(TowerLampStateType.Wait);

            return deviceController.TowerLamp.GetState(TowerLampStateType.Idle);
        }

        public bool UpdateAxisHandler(bool enable)
        {
            // StandAlone인 경우 컨베이어 굴리기
            AxisHandler axisHandler = deviceController.Convayor;
            if (axisHandler != null)
            {
                bool ok = true;
                if (enable)
                    ok = axisHandler.ContinuousMove(null, false);
                else
                    SystemManager.Instance().DeviceController.Convayor?.StopMove();

                if (ok)
                {
                    int accTimeMs = (int)axisHandler.AxisList.Max(f => f.AxisParam.JogParam.AccelerationTimeMs);
                    Thread.Sleep(accTimeMs * 3 / 2);
                    return true;
                }
            }
            return false;
        }
    }
}

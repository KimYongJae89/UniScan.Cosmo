using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.Device;

namespace UniScan.Monitor.Device.Gravure.Laser
{
    public class HanbitLaser : DeviceControllerExtender
    {
        protected const int HeartbeatIntervalMs = 1000;
        protected const int HeartbeatIntervalTimeoutMs = 2000;

        // CM => Laser
        protected IoPort c2lAlive = null;
        protected IoPort c2lEmg = null;
        protected IoPort c2lRst = null;
        protected IoPort c2lRun = null;
        protected IoPort c2lNg = null;

        // Laser => CM
        protected IoPort l2cAlive = null;
        protected IoPort l2cReady = null;
        protected IoPort l2cDone = null;
        protected IoPort l2cError = null;
        protected IoPort l2cOutOfRange = null;

        Timer c2lAliveTimer = null;
        TimeOutTimer aliveTimeoutCheckerCmSide = null;
        TimeOutTimer aliveTimeoutCheckerLaserSide = null;

        public int DoneCount { get => doneCount; }
        int doneCount = 0;

        public bool IsStartRequest { get => isStartRequest; }
        bool isStartRequest;

        public bool UseFromLocal
        {
            get => useFromLocal;
            set
            {
                useFromLocal = value;
                SetRun(CheckRunable());
            }
        }
        bool useFromLocal;

        public bool UseFromRemote
        {
            get => useFromRemote;
            set
            {
                useFromRemote = value;
                SetRun(CheckRunable());
            }
        }
        bool useFromRemote;

        public bool IsSetAlive { get => this.aliveTimeoutCheckerLaserSide == null ? false : !this.aliveTimeoutCheckerLaserSide.TimeOut; }

        public bool IsSetEmergency { get => isSetEmergency; }
        bool isSetEmergency;

        public bool IsSetReset { get => isSetReset; }
        bool isSetReset;

        public bool IsSetRun { get => isSetRun; }
        bool isSetRun;

        public bool IsSetNG { get => isSetNG; }
        bool isSetNG;

        public bool IsAlive { get => this.aliveTimeoutCheckerCmSide == null ? false : !this.aliveTimeoutCheckerCmSide.TimeOut; }

        public bool IsReady { get => isReady;}
        bool isReady;

        public bool IsError { get => isError;}
        bool isError;

        public bool IsOutOfRange { get => isOutOfRange;}
        bool isOutOfRange;

        public virtual bool IsVirtual { get => false; }

        public static HanbitLaser Create(DeviceController deviceController)
        {
            if (UniEye.Base.Settings.MachineSettings.Instance().VirtualMode)
                return new HanbitLaserVirtual(deviceController);

            return new HanbitLaser(deviceController);
        }

        public HanbitLaser(DeviceController deviceController) :base(deviceController)
        {
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmState;
        }

        private void ErrorManager_OnResetAlarmState()
        {
            SetReset(true);
        }

        public virtual void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.c2lAliveTimer = new Timer();
            this.c2lAliveTimer.Interval = HeartbeatIntervalMs;
            this.c2lAliveTimer.Tick += C2LAliveTimer_Tick;
            this.c2lAliveTimer.Start();

            this.aliveTimeoutCheckerCmSide = new TimeOutTimer(this);
            this.aliveTimeoutCheckerCmSide.OnTimeout = AliveTimeoutCheckerCmSide_OnTimeout;
            this.aliveTimeoutCheckerCmSide.Start(HeartbeatIntervalTimeoutMs);

            this.aliveTimeoutCheckerLaserSide = new TimeOutTimer(this);
            this.aliveTimeoutCheckerLaserSide.Start(HeartbeatIntervalTimeoutMs);
        }

        private void C2LAliveTimer_Tick(object sender, EventArgs e)
        {
            // CM -> Laser Alive
            if (this.c2lAlive != null)
            {
                bool curValue = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(this.c2lAlive);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lAlive, !curValue);
            }
        }

        private void AliveTimeoutCheckerCmSide_OnTimeout(object sender, EventArgs e)
        {
            if (!this.isStartRequest)
                return ;

            if (this.isSetRun)
                ErrorManager.Instance().Report(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                    StringManager.GetString("Laser Device Alive Timeout"));
        }

        public virtual void InitializeIoEventHandler(DeviceBox deviceBox)
        {
            this.c2lAlive = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserAlive);
            this.c2lEmg = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserEmergency);
            this.c2lRst = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserReset);
            this.c2lRun = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserRun);
            this.c2lNg = deviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutLaserNG);

            this.l2cAlive = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserAlive);
            this.l2cReady = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserReady);
            this.l2cDone = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserMarkDone);
            this.l2cError = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserError);
            this.l2cOutOfRange = deviceBox.PortMap.GetInPort(PortMap.IoPortName.InLaserOutOfRange);

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

            deviceController.AddIoEventHandler(new IoEventHandler("Alive", digitalIoHandler, this.c2lAlive, IoEventHandlerDirection.Out) { OnChanged = C2LAlive_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Emergency", digitalIoHandler, this.c2lEmg, IoEventHandlerDirection.Out) { OnChanged = C2LEmergency_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Reset", digitalIoHandler, this.c2lRst, IoEventHandlerDirection.Out) { OnChanged = C2LReset_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Run", digitalIoHandler, this.c2lRun, IoEventHandlerDirection.Out) { OnChanged = C2LRun_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("NG", digitalIoHandler, this.c2lNg, IoEventHandlerDirection.Out) { OnChanged = C2LNG_OnChanged });

            deviceController.AddIoEventHandler(new IoEventHandler("Alive", digitalIoHandler, this.l2cAlive, IoEventHandlerDirection.In) { OnChanged = L2CAlive_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Ready", digitalIoHandler, this.l2cReady, IoEventHandlerDirection.In) { OnChanged = L2CReady_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Done", digitalIoHandler, this.l2cDone, IoEventHandlerDirection.In) { OnChanged = L2CDone_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("Error", digitalIoHandler, this.l2cError, IoEventHandlerDirection.In) { OnChanged = L2CError_OnChanged });
            deviceController.AddIoEventHandler(new IoEventHandler("OutOfRange", digitalIoHandler, this.l2cOutOfRange, IoEventHandlerDirection.In) { OnChanged = L2COutOfRange_OnChanged });
        }

        private bool C2LAlive_OnChanged(IoEventHandler eventSource)
        {
            this.aliveTimeoutCheckerLaserSide.Restart(HeartbeatIntervalTimeoutMs);
            return true;
        }

        private bool C2LEmergency_OnChanged(IoEventHandler eventSource)
        {
            this.isSetEmergency = eventSource.IsActivate;
            return true;
        }

        private bool C2LReset_OnChanged(IoEventHandler eventSource)
        {
            this.isSetReset = eventSource.IsActivate;
            return true;
        }

        private bool C2LRun_OnChanged(IoEventHandler eventSource)
        {
            this.isSetRun = eventSource.IsActivate;
            return true;
        }

        private bool C2LNG_OnChanged(IoEventHandler eventSource)
        {
            this.isSetNG = eventSource.IsActivate;
            return true;
        }

        private bool L2CAlive_OnChanged(IoEventHandler eventSource)
        {
            this.aliveTimeoutCheckerCmSide.Restart(HeartbeatIntervalTimeoutMs);
            return true;
        }

        private bool L2CReady_OnChanged(IoEventHandler eventSource)
        {
            this.isReady = eventSource.IsActivate;

            if (!this.isStartRequest)
                return true;

            if (this.isSetRun && !this.isReady)
                ErrorManager.Instance().Report(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Warning, StringManager.GetString("Laser Device is not Ready"));

            bool isRunable = this.CheckRunable();
            this.SetRun(isRunable);

            return true;
        }

        private bool L2CDone_OnChanged(IoEventHandler eventSource)
        {
            if (!this.isStartRequest)
                return true;

            if (eventSource.IsActivate)
                this.doneCount++;

            return true;
        }
        
        private bool L2CError_OnChanged(IoEventHandler eventSource)
        {
            this.isError = eventSource.IsActivate;

            if (!isError && isSetReset)
                SetReset(!isSetReset);

            if (!this.isStartRequest)
                return true;

            if (this.isError && this.isSetRun)
                ErrorManager.Instance().Report(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error, StringManager.GetString("Laser Device Error"));

            bool isRunable = this.CheckRunable();
            this.SetRun(isRunable);

            return true;
        }

        private bool L2COutOfRange_OnChanged(IoEventHandler eventSource)
        {
            this.isOutOfRange = eventSource.IsActivate;

            if (!this.isStartRequest)
                return true;

            if (this.isOutOfRange && this.isSetRun)
                ErrorManager.Instance().Report(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Warning,
                    StringManager.GetString("Laser Device \'Out Of Range\' Warning"));

            bool isRunable = this.CheckRunable();
            this.SetRun(isRunable);

            return true;
        }

        internal void InitializeMotionEventHandler(DeviceBox deviceBox)
        {
        }
        
        public void SetEmergency(bool active)
        {
            if (this.c2lEmg != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lEmg, active);
        }

        public void SetReset(bool active)
        {
            if (this.c2lRst != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lRst, active);
        }

        public void SetRun(bool active)
        {
            if (this.c2lRun != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lRun, active);
        }

        public void SetNG(bool active)
        {
            if (this.c2lNg != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutput(this.c2lNg, active);
        }

        private bool CheckRunable()
        {
            return this.isStartRequest && this.IsAlive && this.isReady && !this.isError && (useFromLocal || useFromRemote);
        }

        public void ResetDoneCount()
        {
            this.doneCount = 0;
        }

        public bool Start(bool useFromLocal)
        {
            this.useFromLocal = useFromLocal;

            this.isStartRequest = true;

            if (this.useFromLocal || this.useFromRemote)
            {
                bool runable = CheckRunable();
                if (!runable)
                    //throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error, StringManager.GetString("Laser Device is not prepared."));
                    return false;
                SetRun(true);
            }

            return true;
        }

        public bool Restart(bool use)
        {
            Stop();
            return Start(use);
        }

        public bool Stop()
        {
            SetRun(false);
            this.isStartRequest = false;
            return true;
        }
    }

    public class HanbitLaserVirtual : HanbitLaser
    {
        Timer l2cAliveTimer = new Timer();

        public override bool IsVirtual { get => true; }

        public HanbitLaserVirtual(DeviceController deviceController) : base(deviceController)
        {
         
        }

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);

            this.l2cAliveTimer.Interval = HeartbeatIntervalMs;
            this.l2cAliveTimer.Tick += L2CAliveTimer_Tick;
        }



        public override void InitializeIoEventHandler(DeviceBox deviceBox)
        {
            base.InitializeIoEventHandler(deviceBox);

            DigitalIoHandler digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;

        }

        private void L2CAliveTimer_Tick(object sender, EventArgs e)
        {
            // Laser -> CM Alive
            this.l2cAliveTimer.Interval = HeartbeatIntervalMs;
            bool curState = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(this.l2cAlive);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cAlive, !curState);
        }

        public bool SetAlive(bool active)
        {
            this.l2cAliveTimer.Interval = 100;

            if (active)
                this.l2cAliveTimer.Start();
            else
                this.l2cAliveTimer.Stop();

            return active;
        }

        public bool SetReady(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cReady, active);
            return active;
        }

        public bool SetError(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cError, active);
            return active;
        }

        public bool SetOutOfRange(bool active)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteInput(this.l2cOutOfRange, active);
            return active;
        }
    }


}

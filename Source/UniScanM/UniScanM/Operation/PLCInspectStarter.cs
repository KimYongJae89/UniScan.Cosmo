using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.Data;
using UniScanM.MachineIF;
using UniScanM.Settings;

namespace UniScanM.Operation
{
    //PLC 읽고 쓰기 클래스, & 시작, 정지 및 리와인터컷 콜백 실행, 각 부가장치별 프로토콜에 따라 override 바람.
    public abstract class PLCInspectStarter : InspectStarter
    {
        protected MelsecMonitor melsecMonitor = new MelsecMonitor();
        public MelsecMonitor MelsecMonitor
        {
            get { return melsecMonitor; }
        }

        string preLotNo = "";
        bool preRewinderCut = false;

        //common
        protected PLCInspectStarter() : base()
        {
        }

        public override void Start()
        {
            isInspecting = false;
            this.RequestStop = false;
            this.WorkingThread = new System.Threading.Thread(ThreadProc);
            WorkingThread.Priority = ThreadPriority.Lowest;
            this.WorkingThread.Start();
        }

        protected override void ThreadProc()//********************************************************// ThreadProc()
        {
            SetFirstValue();
                               
            Thread.Sleep(100);

            while (RequestStop == false)
            {
                Thread.Sleep(250);
                if (ErrorManager.Instance().IsAlarmed() == true) continue;  //??

                //<개선>여기서 한방에 공통사항 다 읽어서 멤버변수에 저장
                //이후 멤버변수를 체크하여 콜백 실행 결정 -> 읽기 네트워크 부하를 줄일수 있음.
                melsecMonitor.ReadMachineState();
                //쓰기도 모아서 한방에?

                //0.0 only when autostart mode
                if (startMode == StartMode.Auto)
                {
                    if (isInspecting == false)
                    {
                        //0.주기적으로 Ready 신호 쏘기
                        //SetReadySignal(true); //0: off or manual, 1: on and Auto-mode

                        //1. ColorSensor Start 감시
                        Check_StartSignal();
                    }
                    //2. 이미 자동시작되어있는 running 중일때임.
                    else
                    {
                        //3. Stop 신호 체크
                        Check_StopSignal();

                        if (melsecMonitor.State.RewinderCut != preRewinderCut)  //4. 리와인터 컷 체크
                        {
                            Check_RewinderCut();
                        }
                        else if (melsecMonitor.State.LotNo != preLotNo) // 로트 번호 바뀜
                        {
                            Check_RewinderCut();// 리와인뜼 컸꽈 깥은 똥짞
                        }

                        //5. SetRunning Signal  //쓰기 한방에? result??
                        //SetRunningSignal(true); //0: idle, 1: run@ only auto-mode, Not Manual-Mode
                    }
                }//if

                preRewinderCut = melsecMonitor.State.RewinderCut;
                preLotNo = melsecMonitor.State.LotNo;

                SetVisionState();
            }// while
            isInspecting = false;
        }//*************************************************************************************8   ThreadProc

        void SetVisionState()
        {
            if (melsecMonitor.State.IsConnected == false)
                return;

            string ready = "0001";
            string run = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto? "0001" : "0000";

            string sendData = string.Format("{0}{1}", ready, run);
            //SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfCommonCommand.SET_VISION_STATE, sendData);  // Update PLC
        }
        public void Check_StartSignal()
        {
            if (GetAutoStartSignal() == true && isInspecting == false)
            {
                bool ok = PreStartInspect(true);
                if (ok)
                {
                    if (OnStartInspection != null)
                        ok = OnStartInspection.Invoke();//Start
                }
                isInspecting = ok;
            }
        }

        public void Check_StopSignal()
        {
            if (GetAutoStartSignal() == false && isInspecting == true)
            {
                OnStopInspection?.Invoke();//Stop
                isInspecting = false;
            }
        }

        public void Check_RewinderCut()
        {
            //2. 스타트 후 리와인터 컷 감시
            //2.1 리워인더 컷 후 로트 읽어서 변경, (저장경로도 같이 바뀜)

            //OnStopInspection?.Invoke();

            this.PreStartInspect(true);
            //OnStartInspection?.Invoke();
            

            if (OnRewinderCutting != null)
                OnRewinderCutting();
        }

        public override double GetLineSpeed()
        {
            return melsecMonitor.State.SpSpeed;
        }

        public override string GetLotNo()
        {
            string lotNo = melsecMonitor.State.LotNo;
            if (String.IsNullOrEmpty(lotNo))
                lotNo = string.Format("Unknown.{0}", DateTime.Now.ToString("yyMMddHHmm"));

            return lotNo;
        }

        public override string GetModelName()
        {
            string modelName = melsecMonitor.State.ModelName;
            if (string.IsNullOrEmpty(modelName))
                modelName = "Unknown";

            return modelName;
        }

        public override int GetPosition()
        {
            return (int)Math.Round(melsecMonitor.State.PvPosition);
        }

        public override string GetWorker()
        {
            return melsecMonitor.State.Worker;
        }

        public override string GetPaste()
        {
            string paste = melsecMonitor.State.Paste;
            if (String.IsNullOrEmpty(paste))
                paste = "Unknown";

            return melsecMonitor.State.Paste;
        }

        public override int GetRewinderSite()
        {
            return melsecMonitor.State.RewinderCut ? 1 : 0;
        }

        public override double GetRollerDia()
        {
            return melsecMonitor.State.RollDia;
        }

        public override bool PreStartInspect(bool autoStart)
        {
            string modelName = GetModelName();
            string paste = GetPaste();
            string lotNo = GetLotNo();
            int rewinderSite = (int)GetRewinderSite();
            int startPosition = (int)GetPosition();
            string worker = GetWorker();
            double lineSpeed = GetLineSpeed();
            if (autoStart && lineSpeed > UniScanMSettings.Instance().MaximumLineSpeed)
                return false;

            SystemManager.Instance().LoadDefaultModel();
            if (SystemManager.Instance().ProductionManager != null)
            {
                int sameCount = SystemManager.Instance().ProductionManager.LotExistCount(DateTime.Now, modelName, worker, lotNo, paste, (autoStart ? "Auto" : "Manual"), rewinderSite);
                if (sameCount > 0)
                    lotNo = string.Format("{0}_{1}", lotNo, sameCount);

                UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.LotChange(modelName, worker, lotNo, paste, (autoStart ? "Auto" : "Manual"), rewinderSite);
                lock (production)
                {
                    production.LastStartPosition = startPosition;

                    if (production.StartPosition < 0)
                        production.StartPosition = startPosition;

                    SystemManager.Instance().InspectStarter.OnLotChanged();
                }
            }

            return true;
        }

        protected abstract void SetFirstValue();
        public abstract bool GetAutoStartSignal();
        public abstract void SetReadySignal(bool imReady = true);//0: off or manual, 1: on and Auto-mode
        public abstract void SetRunningSignal(bool running = true); //0: idle, 1: run@ only auto-mode, Not Manual-Mode
    }
}
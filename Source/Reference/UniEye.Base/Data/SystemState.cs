using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Inspection;
using DynMvp.InspData;

namespace UniEye.Base.Data
{
    public enum OpState
    {
        Idle, Wait, Inspect, Review, Teach, Align, Alarm
    }

    public enum InspectState
    {
        Done, Align, Run, Pause, Wait, Scan, Tune, Stop, Ready
    }

    public delegate void InspectStateChangedDelegate(InspectState curInspectState);
    public delegate void OpStateChangedDelegate(OpState curOpState, OpState prevOpState);
    public interface IOpStateListener
    {
        void OpStateChanged(OpState curOpState, OpState prevOpState);
    }

    public interface IInspectStateListener
    {
        void InspectStateChanged(InspectState curInspectState);
    }

    public class SystemState
    {
        bool pause = false;
        public bool Pause
        {
            set { pause = value; }
            get { return pause; }
        }
        
        OpState prevOpState;
        OpState curOpState;
        private OpState OpState
        {
            set { prevOpState = curOpState; curOpState = value; }
        }

        InspectState inspectState;
        public InspectState InspectState
        {
            get { return inspectState; }
        }

        Judgment inspectionResult;
        public Judgment InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        // 검사 중지 신호를 보낸 후, 대기 중인지
        bool onWaitStop;
        public bool OnWaitStop
        {
            get { return onWaitStop; }
            set { onWaitStop = value; }
        }

        bool alarmed;
        public bool Alarmed
        {
            get { return alarmed; }
            set { alarmed = value; }
        }

        List<IOpStateListener> opListenerList = new List<IOpStateListener>();
        List<IInspectStateListener> inspectListenerList = new List<IInspectStateListener>();

        static SystemState _instance;
        public static SystemState Instance()
        {
            if (_instance == null)
                _instance = new SystemState();

            return _instance;
        }

        public void AddOpListener(IOpStateListener listener)
        {
            opListenerList.Add(listener);
            listener.OpStateChanged(curOpState, prevOpState);
        }

        public void AddInspectListener(IInspectStateListener listener)
        {
            inspectListenerList.Add(listener);
            listener.InspectStateChanged(inspectState);
        }

        public bool OnInspection
        {
            get { return curOpState == OpState.Inspect; }
        }

        public bool OnInspectOrWait
        {
            get { return curOpState == OpState.Inspect || curOpState == OpState.Wait; }
        }

        public bool OnInspectOrWaitOrPause
        {
            get { return curOpState == OpState.Inspect || curOpState == OpState.Wait || pause == true; }
        }

        private SystemState()
        {
            DynMvp.Base.ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarmState;
            DynMvp.Base.ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmState;
        }

        private void ErrorManager_OnStartAlarmState()
        {
            SetAlarm();
        }

        private void ErrorManager_OnResetAlarmState()
        {
            SetIdle();
        }

        public OpState GetOpState()
        {
            return curOpState;
        }

        /// <summary>
        /// 유휴 상태. Stop 버튼을 눌렀을 때.
        /// </summary>
        public void SetIdle()
        {
            if (curOpState != OpState.Idle)
            {
                OpState = OpState.Idle;
                OpStateNotifyChanged();
            }
        }

        /// <summary>
        /// 검사 준비 중. Start 버튼을 눌렀을 때.
        /// </summary>
        public void SetWait()
        {
            if (curOpState != OpState.Wait)
            {
                OpState = OpState.Wait;
                OpStateNotifyChanged();
            }
        }

        /// <summary>
        /// 오류 발생.
        /// </summary>
        public void SetAlarm()
        {
            if (curOpState != OpState.Alarm)
            {
                OpState = OpState.Alarm;
                OpStateNotifyChanged();
            }
        }

        public void SetAlign()
        {
            if (curOpState != OpState.Align)
            {
                OpState = OpState.Align;
                OpStateNotifyChanged();
            }
        }

        /// <summary>
        /// 검사 준비 완료. Trigger 대기 상태.
        /// </summary>
        public void SetInspect()
        {
            if (curOpState != OpState.Inspect)
            {
                OpState = OpState.Inspect;
                inspectState = InspectState.Wait;
                OpStateNotifyChanged();
            }
        }

        /// <summary>
        /// Inspect 상태일 때, Trigger 등으로 상태가 변화하면 이 함수를 사용한다.
        /// Idle / Wait 상태이면 함수 실행을 무시한다.
        /// </summary>
        /// <param name="inspectState"></param>
        public void SetInspectState(InspectState inspectState)
        {   
            if (this.inspectState != inspectState)
            {
                this.inspectState = inspectState;
                InspectStateNotifyChanged();
            }
        }

        public void SetTeach()
        {
            if (curOpState != OpState.Teach)
            {
                OpState = OpState.Teach;
                OpStateNotifyChanged();
            }
        }

        public void OpStateNotifyChanged()
        {
            foreach (IOpStateListener listener in opListenerList)
                listener.OpStateChanged(curOpState, prevOpState);
        }

        public void InspectStateNotifyChanged()
        {
            foreach (IInspectStateListener listener in inspectListenerList)
                listener.InspectStateChanged(inspectState);
        }
    }
}

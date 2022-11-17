//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.IO;
//using System.Diagnostics;

//using Standard.DynMvp.Base;

//using System.Threading;

//namespace Standard.DynMvp.Devices.MotionController
//{
//    public struct MotionStatus
//    {
//        public bool origin; // Origin sensor
//        public bool ez;     // signal
//        public bool emg;    // signal
//        public bool inp;    // signal
//        public bool alarm;  // signal
//        public bool posLimit;// pos-limit sensor
//        public bool negLimit;// neg-limit sensor
//        public bool run;    // signal
//        public bool err;    // signal
//        public bool home;   // signal
//        public bool homeOk;   // signal
//        public bool cClr;   // signal
//        public bool servoOn;    // signal
//        public bool aRst;   // signal
//    }

//    public struct AxisStatus
//    {
//        public bool isServoOn;
//        public bool isMoving;
//        public bool isFault;

//        public static AxisStatus operator |(AxisStatus a, AxisStatus b)
//        {
//            return new AxisStatus { isServoOn = a.isServoOn | b.isServoOn, isMoving = a.isMoving | b.isMoving, isFault = a.isFault | b.isFault };
//        }
//        public void ResetStatus()
//        {
//            isServoOn = isMoving = isFault = false;
//        }
//    }
    
//    public interface IMotion
//    {
//        bool CanSyncMotion();
//        void TurnOnServo(int axisNo, bool bOnOff);
//        float GetCommandPos(int axisNo);
//        float GetActualPos(int axisNo);
//        void SetPosition(int axisNo, float position);
//        bool StartHomeMove(int axisNo, HomeParam homeSpeed);
//        bool StartMove(int axisNo, float position, MovingParam movingParam);
//        bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
//        bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
//        void StopMove(int axisNo);
//        void EmergencyStop(int axisNo);
//        bool IsMoveDone(int axisNo);
//        bool IsAmpFault(int axisNo);
//        bool IsHomeOn(int axisNo);
//        bool IsPositiveOn(int axisNo);
//        bool IsNegativeOn(int axisNo);
//        bool IsEmgStop(int axisNo);
//    }

//    public class MotionList : List<Motion>
//    {
//        public void Initialize(MotionInfoList motionInfoList, bool isVirtual)
//        {
//            foreach (MotionInfo motionInfo in motionInfoList)
//            {
//                Motion motion = MotionFactory.Create(motionInfo, isVirtual);
//                if (motion != null)
//                {
//                    Add(motion);
//                }
//            }
//        }

//        public void Release()
//        {
//            //foreach (Motion motion in this)
//            //{
//            //    DeviceManager.Instance().RemoveDevice(motion);
//            //    bool isReady = motion.IsReady();
//            //    if (isReady)
//            //    {
//            //        motion.TurnOnServo(false);
//            //        motion.Release();
//            //    }
//            //}
//            //this.Clear();
//        }

//        public Motion GetMotion(string name)
//        {
//            return Find(x => x.DeviceInfo.Name == name);
//        }

//        public Motion GetMotion(int index)
//        {
//            return this[index];
//        }

//        public void StopMove()
//        {
//            foreach (Motion motion in this)
//            {
//                motion.StopMove();
//            }

//            Thread.Sleep(1000);
//        }

//        public void EmergencyStop()
//        {
//            foreach (Motion motion in this)
//            {
//                motion.EmergencyStop();
//            }

//            Thread.Sleep(1000);
//        }

//        public void ResetAlarm()
//        {
//            foreach (Motion motion in this)
//            {
//                motion.ResetAlarm();
//            }
//        }

//        public void TurnOnServo(bool bOnOff)
//        {
//            foreach (Motion motion in this)
//            {
//                motion.TurnOnServo(!bOnOff);
//            }

//            Thread.Sleep(500);
//        }
//    }

//    public abstract class Motion : Device, IMotion
//    {
//        private MotionType motionType;
//        public MotionType MotionType
//        {
//            get { return motionType; }
//        }

//        private int numAxis;
//        public int NumAxis
//        {
//            get { return numAxis; }
//            set { numAxis = value; }
//        }

//        public Motion(MotionInfo motionInfo) : base(motionInfo)
//        {
//            if (name == "")
//                Name = motionType.ToString();
//            else
//                Name = name;

//            DeviceType = DeviceType.MotionController;
//            UpdateState(DeviceState.Idle, "Created");
//        }

//        public abstract bool Initialize(MotionInfo motionInfo);
//        public abstract bool CanSyncMotion();

//        public abstract void TurnOnServo(int axisNo, bool bOnOff);

//        public abstract float GetCommandPos(int axisNo);
//        public abstract float GetActualPos(int axisNo);
//        public abstract float GetActualVel(int axisNo);

//        public abstract void SetPosition(int axisNo, float position);

//        public abstract bool ClearHomeDone(int axisNo);

//        public abstract bool StartMove(int axisNo, float position, MovingParam movingParam);
//        public abstract bool StartRelativeMove(int axisNo, float position, MovingParam movingParam);
//        public abstract bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative);
//        public abstract bool StartHomeMove(int axisNo, HomeParam homeSpeed);

//        public abstract void StopMove(int axisNo);
//        public abstract void EmergencyStop(int axisNo);

//        public abstract bool StartMultiMove(float[] position, MovingParam movingParam);

//        public abstract bool StartCmp(int axisNo, int startPos, float dist, bool plus);
//        public abstract bool EndCmp(int axisNo);

//        public bool IsServoOn(int axisNo)
//        {
//            return GetMotionStatus(axisNo).servoOn;
//        }

//        public bool IsMoveDone(int axisNo)
//        {
//            return GetMotionStatus(axisNo).inp && GetMotionStatus(axisNo).run == false;
//        }

//        public bool IsMoveOn(int axisNo)
//        {
//            return GetMotionStatus(axisNo).run;
//        }

//        public bool IsEmgStop(int axisNo)
//        {
//            return GetMotionStatus(axisNo).emg;
//        }

//        public bool IsHomeMoving(int axisNo)
//        {
//            return GetMotionStatus(axisNo).home;
//        }
//        public bool IsHomeDone(int axisNo)
//        {
//            return GetMotionStatus(axisNo).homeOk;
//        }

//        public bool IsAmpFault()
//        {
//            bool fault = false;
//            for (int i = 0; i < numAxis; i++)
//                fault |= IsAmpFault(i);
//            return fault;
//        }

//        public bool IsAmpFault(int axisNo)
//        {
//            return GetMotionStatus(axisNo).alarm;
//        }

//        public bool IsHomeOn(int axisNo)
//        {
//            return GetMotionStatus(axisNo).origin;
//        }

//        public bool IsPositiveOn(int axisNo)
//        {
//            return GetMotionStatus(axisNo).posLimit;
//        }

//        public bool IsNegativeOn(int axisNo)
//        {
//            return GetMotionStatus(axisNo).negLimit;
//        }

//        public abstract MotionStatus GetMotionStatus(int axisNo);
//        public abstract bool ResetAlarm(int axisNo);

//        public void TurnOnServo(bool bOnOff)
//        {
//            for (int i = 0; i < NumAxis; i++)
//                TurnOnServo(i, bOnOff);
//        }

//        public void StopMove()
//        {
//            for (int i = 0; i < NumAxis; i++)
//                StopMove(i);
//        }

//        public void EmergencyStop()
//        {
//            for (int i = 0; i < NumAxis; i++)
//            {
//                if (IsEmgStop(i) == false)
//                    EmergencyStop(i);
//            }
//        }

//        public void Move(int axisNo, float position, MovingParam movingParam)
//        {
//            StartMove(axisNo, position, movingParam);
//            while (IsMoveDone(axisNo) == false) ;
//        }

//        public void RelativeMove(int axisNo, float position, MovingParam movingParam)
//        {
//            StartRelativeMove(axisNo, position, movingParam);
//            while (IsMoveDone(axisNo) == false) ;
//        }

//        public void HomeMove(int axisNo, HomeParam homeParam)
//        {
//            StartHomeMove(axisNo, homeParam);
//            while (IsMoveDone(axisNo) == false)
//            {
//                Thread.Sleep(100);
//            }

//            Thread.Sleep(500);
//        }

//        public void ResetAlarm()
//        {
//            for (int i = 0; i < NumAxis; i++)
//            {
//                ResetAlarm(i);
//            }
//        }
//    }
//}

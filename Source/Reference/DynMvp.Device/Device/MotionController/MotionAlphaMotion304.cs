﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.Dio;

using Shared;

namespace DynMvp.Devices.MotionController
{
	class MotionAlphaMotion304 : Motion, IDigitalIo
    {
        static int loadCount = 0;
        ushort boardNo = 0;

        uint numDigitalInput;
        uint numDigitalOutput;

        public MotionAlphaMotion304(string name)
			: base(MotionType.AlphaMotion304, name)
		{

		}

        public string GetName() { return Name; }
        public int GetNumInPort() { return (int)numDigitalInput; }
        public int GetNumOutPort() { return (int)numDigitalOutput; }
        public int GetNumInPortGroup() { return 1; }
        public int GetNumOutPortGroup() { return 1; }
        public int GetInPortStartGroupIndex() { return 0; }
        public int GetOutPortStartGroupIndex() { return 0; }

        public bool IsReady()
		{
			return base.IsReady();
		}

		public override bool Initialize(MotionInfo motionInfo)
		{
			if (base.IsReady() == false)
			{
                int result = TMCAADLL.TMC302A_LoadDevice();
                if (result < 0)
                {
                    string errorMsg;
                    switch (result)
                    {
                        default:
                        case tmcDef.ERR_DEVICE_LOAD:
                            errorMsg = "Fail to Load Device Driver";
                            break;
                        case tmcDef.ERR_DEVICE_EXIST:
                            errorMsg = "Device number is exist";
                            break;
                        case tmcDef.ERR_DEVICE_PCI_BUS:
                            errorMsg = "PCI Bus line error";
                            break;
                        case tmcDef.ERR_UPBOARD_LOAD:
                            errorMsg = "UP board error";
                            break;
                    }

                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                        ErrorLevel.Fatal, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), string.Format("[TMC 304] {0}", errorMsg));

                    UpdateState(DeviceState.Error, "Can't find alpha motion 304 device.");
                    return false;
                }
                else
                {
                    loadCount++;
                    boardNo = (ushort)result;
                }

                TMCACDLL.TMC304A_LogCheck(3);

                PciMotionInfo pciMotionInfo = (PciMotionInfo)motionInfo;

                // boardNo = (ushort)pciMotionInfo.Index;

                NumAxis = (int)TMCACDLL.TMC304A_GetAxisNum(boardNo);
                numDigitalInput = TMCACDLL.TMC304A_GetDiNum(boardNo);
                numDigitalOutput = TMCACDLL.TMC304A_GetDoNum(boardNo);

                UpdateState(DeviceState.Ready, "Device Loaded");
            }

            return true;
		}

		public override void Release()
		{
            base.Release();

            loadCount--;

            if (loadCount == 0)
                TMCACDLL.TMC304A_UnloadDevice();

            UpdateState(DeviceState.Idle, "Device unloaded");
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public bool CheckError(int errorType, string errorStr, ErrorLevel errorLevel)
        {
            int errCode = TMCAADLL.TMC302A_GetErrorCode();
            if (errCode != tmcDef.ERR_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, errorType, errorLevel, ErrorSection.Motion.ToString(), errorStr, TMCAADLL.TMC302A_GetErrorString(errCode));
                return true;
            }

            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
		{
            TMCACDLL.TMC304A_PutSvOn(boardNo, (ushort)axisNo, Convert.ToUInt16(bOnOff));
		}

        //public override bool IsServoOn(int axisNo)
        //{
        //    ushort resopnse = TMCACDLL.TMC304A_GetSvOn(boardNo, (ushort)axisNo);
        //    if (resopnse == 1)
        //        return true;
        //    return false;
        //}

        public override float GetCommandPos(int axisNo)
		{
			return TMCACDLL.TMC304A_GetCommandPos(boardNo, (ushort)axisNo);
		}

		public override float GetActualPos(int axisNo)
		{
			return TMCACDLL.TMC304A_GetActualPos(boardNo, (ushort)axisNo);
		}

        public override float GetActualVel(int axisNo)
        {
            return TMCACDLL.TMC304A_GetCommandSpeed(boardNo, (ushort)axisNo);
        }

        public override void SetPosition(int axisNo, float position)
		{
			TMCACDLL.TMC304A_SetActualPos(boardNo, (ushort)axisNo, (int)position);
			TMCACDLL.TMC304A_SetCommandPos(boardNo, (ushort)axisNo, (int)position);
		}

		public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
		{
            LogHelper.Debug(LoggerType.Machine, String.Format("HomeMove : Axis Id {0}", axisNo));

            TMCACDLL.TMC304A_SetHomeSpeed(boardNo, (ushort)axisNo, (uint)(homeSpeed.HighSpeed.StartVelocity + 1),
				(uint)(Math.Abs(homeSpeed.HighSpeed.MaxVelocity) + 1), (uint)(homeSpeed.HighSpeed.AccelerationTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_SetHomeDir(boardNo, (ushort)axisNo, 1);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_SetHomeMode(boardNo, (ushort)axisNo, 2);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_Home_Move(boardNo, (ushort)axisNo);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
                return false;

            return true;
		}

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("StartMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
				TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 0);
			else
				TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 1);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_SetPosSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(movingParam.MaxVelocity + 1),
					(uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs), (uint)(movingParam.DecelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_Abs_Move(boardNo, (ushort)axisNo, (int)position);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
                return false;

            return true;
		}

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("StartRelativeMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
				TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 0);
			else
				TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 1);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_SetPosSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(movingParam.MaxVelocity + 1),
					(uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs), (uint)(movingParam.DecelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            TMCACDLL.TMC304A_Inc_Move(boardNo, (ushort)axisNo, (int)position);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
                return false;

            return true;
		}

		public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("ContinuousMove : Axis Id {0}", axisNo));

            TMCACDLL.TMC304A_SetJogSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(Math.Abs(movingParam.MaxVelocity) + 1),
					(uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
                return false;

            if (movingParam.MaxVelocity < 0)
				TMCACDLL.TMC304A_Jog_Move(boardNo, (ushort)axisNo, 0);
			else
				TMCACDLL.TMC304A_Jog_Move(boardNo, (ushort)axisNo, 1);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
                return false;

            return true;
        }

		public override void StopMove(int axisNo)
		{
			TMCACDLL.TMC304A_Decel_Stop(boardNo, (ushort)axisNo);
		}

        public override void EmergencyStop(int axisNo)
        {
            TMCACDLL.TMC304A_Sudden_Stop(boardNo, (ushort)axisNo);
        }

        private void IsMoveDone(int axisNo, out bool isMoveDone)
		{
            isMoveDone = (TMCACDLL.TMC304A_Done(boardNo, (ushort)axisNo) == 0);
		}

        public override bool ClearHomeDone(int axisNo)
        {
            throw new NotImplementedException();
        }
        
		public void WriteOutputGroup(int groupNo, uint outputPortStatus)
		{
			Debug.Assert(groupNo == 0, "Alpha Motion 304 has only 32 output port.");

			TMCACDLL.TMC304A_PutDODWord(boardNo, (ushort)groupNo, outputPortStatus);
		}

		public uint ReadOutputGroup(int groupNo)
		{
			Debug.Assert(groupNo == 0, "Alpha Motion 304 has only 32 output port.");

			uint value = 0;
			TMCACDLL.TMC304A_GetDODWord(boardNo, (ushort)groupNo, ref value);

			return value;
		}

		public uint ReadInputGroup(int groupNo)
		{
			Debug.Assert(groupNo == 0, "Alpha Motion 304 has only 32 input port.");

			uint value = 0;
			TMCACDLL.TMC304A_GetDIDWord(boardNo, (ushort)groupNo, ref value);

			return value;
		}

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            MotionStatus motionStatus = new MotionStatus();

            ushort axisStatus = TMCACDLL.TMC304A_GetCardStatus(boardNo, (ushort)axisNo);

            motionStatus.origin = (axisStatus & 0x0001) > 0;
            IsMoveDone(axisNo, out motionStatus.inp);
            //motionStatus.inp = (axisStatus & 0x0008) > 0;
            motionStatus.run = (axisStatus & 0x0100) > 0;
            motionStatus.posLimit = (axisStatus & 0x0020) > 0;
            motionStatus.negLimit = (axisStatus & 0x0040) > 0;
            motionStatus.servoOn = (axisStatus & 0x4000) > 0;
            motionStatus.emg = (axisStatus & 0x0004) > 0;
            motionStatus.alarm = (axisStatus & 0x0010) > 0;
            motionStatus.homeOk = (axisStatus & 0x0800) > 0;
            return motionStatus;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {

        }

        public override bool ResetAlarm(int axisNo)
        {
            return true;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            TMCACDLL.TMC304A_PutDOBit(boardNo, (ushort)(groupNo * 32 + portNo), (ushort)(value ? 1 : 0));
        }

        public override bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam)
        {
            throw new NotImplementedException();
        }

        public override bool StartCmp(int axisNo, int startPos, float dist, bool plus)
        {
            throw new NotImplementedException();
        }

        public override bool EndCmp(int axisNo)
        {
            throw new NotImplementedException();
        }

        public override bool ModifyPos(int axisNo, float position)
        {
            throw new NotImplementedException();
        }

        public uint ReadOutputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }

        public uint ReadInputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }
    }
}

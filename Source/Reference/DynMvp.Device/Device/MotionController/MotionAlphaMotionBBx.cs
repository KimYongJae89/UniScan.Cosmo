﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.Dio;

using Shared;
using System.Threading.Tasks;

namespace DynMvp.Devices.MotionController
{
	class MotionAlphaMotionBBx : Motion, IDigitalIo
    {
        static int loadCount = 0;
        ushort boardNo = 0;

        uint numDigitalInput;
        uint numDigitalOutput;

        public MotionAlphaMotionBBx(string name)
			: base(MotionType.AlphaMotionBx, name)
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
                try
                {
                    int iBoardNo = 0;
                    int result = nmiMNApi.nmiSysLoad(nmiMNApiDefs.emFALSE, ref iBoardNo);
                    // TMC_RV_OK 1 라이브러리 성공 
                    // TMC_RV_NOT_OPEN - 1001 라이브러리 초기화 실패 
                    // TMC_RV_LOC_MEM_ERR -1004 메모리 생성 에러 
                    // TMC_RV_HANDLE_ERR -1026 드바이스 핸들값 에러 
                    // TMC_RV_PCI_BUS_LINE_ERR -1058 PCI 버스 라인 이상 에러 
                    // TMC_RV_CON_DIP_SW_ERR -1056 동일한 DIP SWITCH를 설정 에러 
                    // TMC_RV_MODULE_POS_ERR -1059 모듈 순서 에러 
                    // TMC_RV_SUPPORT_PROCESS -1060 지원하지 않은 프로세스 에러

                    if (result != nmiMNApiDefs.TMC_RV_OK)
                        throw new Exception(string.Format("Motion Controller Initialize Fail. {0}", result));


                    nmiMNApi.nmiConParamLoad();
                    loadCount++;

                    if (this.boardNo >= iBoardNo)
                        return false;

                    PciMotionInfo pciMotionInfo = (PciMotionInfo)motionInfo;

                    int numAxis = 0;
                    result = nmiMNApi.nmiGnGetAxesNumAll(boardNo, ref numAxis);
                    if (result != nmiMNApiDefs.TMC_RV_OK)
                        throw new Exception(string.Format("Can not find Axis info. {0}", result));
                    NumAxis = (int)numAxis;

                    int numDioIn = 0, numDioOut = 0;
                    result = nmiMNApi.nmiGnGetDioNum(boardNo, ref numDioIn, ref numDioOut);
                    if (result != nmiMNApiDefs.TMC_RV_OK)
                        throw new Exception(string.Format("Can not find DIO info. {0}", result));
                    numDigitalInput = (uint)numDioIn;
                    numDigitalOutput = (uint)numDioOut;

                    UpdateState(DeviceState.Ready, "Device Loaded");
                }   
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                        ErrorLevel.Fatal, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), string.Format("[TMC 304] {0}", errorMsg));

                    UpdateState(DeviceState.Error, "Can't find alpha motion Bx device.");
                    return false;
                }
            }

            return true;
		}

		public override void Release()
		{
            base.Release();

            loadCount--;

            if (loadCount == 0)
                nmiMNApi.nmiSysUnload();

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

        //public bool CheckError(int errorType, string errorStr, ErrorLevel errorLevel)
        //{
        //    int errCode = TMCAADLL.TMC302A_GetErrorCode();
        //    if (errCode != tmcDef.ERR_SUCCESS)
        //    {
        //        ErrorManager.Instance().Report((int)ErrorSection.Motion, errorType, errorLevel, ErrorSection.Motion.ToString(), errorStr, TMCAADLL.TMC302A_GetErrorString(errCode));
        //        return true;
        //    }

        //    return false;
        //}

        public override void TurnOnServo(int axisNo, bool bOnOff)
		{
            nmiMNApi.nmiAxSetServoOn(boardNo, axisNo, Convert.ToUInt16(bOnOff));
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
            double comPos = 0;
            nmiMNApi.nmiAxGetCmdPos(boardNo, axisNo, ref comPos);
            return (float)comPos;

        }

        public override float GetActualPos(int axisNo)
		{
            double actPos = 0;
            nmiMNApi.nmiAxGetActPos(boardNo, axisNo,ref actPos);
            return (float)actPos;
        }

        public override float GetActualVel(int axisNo)
        {
            double actVel = 0;
            //nmiMNApi.getvel
            //nmiMNApi.getvel(boardNo, axisNo, ref actVel);
            return (float)actVel;
        }

        public override void SetPosition(int axisNo, float position)
		{
            nmiMNApi.nmiAxSetCmdPos(boardNo, axisNo, position);
            nmiMNApi.nmiAxSetActPos(boardNo, axisNo, position);
		}

		public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
		{
            LogHelper.Debug(LoggerType.Machine, String.Format("HomeMove : Axis Id {0}", axisNo));

            //        TMCACDLL.TMC304A_SetHomeSpeed(boardNo, (ushort)axisNo, (uint)(homeSpeed.HighSpeed.StartVelocity + 1),
            //(uint)(Math.Abs(homeSpeed.HighSpeed.MaxVelocity) + 1), (uint)(homeSpeed.HighSpeed.AccelerationTimeMs));
            //        if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //            return false;

            int type = 0x01;    // 사다리꼴 가감속
            double dVel = homeSpeed.HighSpeed.MaxVelocity; // 검출속도
            double dRefVel = homeSpeed.FineSpeed.MaxVelocity; // 되돌아가는 속도
            double dTacc = homeSpeed.HighSpeed.AccelerationTimeMs; // 가감속 시간
            nmiMNApi.nmiAxHomeSetInitVel(boardNo, axisNo, 100);
            
            nmiMNApi.nmiAxHomeSetType(boardNo, axisNo, homeSpeed.HomeMethod);    // ORG ON -> Stop -> Go back(Rev Spd) -> ORG OFF -> Stop on EZ signal
            nmiMNApi.nmiAxHomeSetCrcEnable(boardNo, axisNo, 0x01);    // 종료시 CRC 사용
            nmiMNApi.nmiAxHomeSetVelProf(boardNo, axisNo, type, dVel, dRefVel, dTacc);
            
            //TMCACDLL.TMC304A_SetHomeDir(boardNo, (ushort)axisNo, 1);
            //if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //    return false;

            int nDir = homeSpeed.HomeDirection == MoveDirection.CW ? nmiMNApiDefs.emDIR_P : nmiMNApiDefs.emDIR_N; // 방향
            nmiMNApi.nmiAxHomeSetDir(boardNo, axisNo, nDir);


            //TMCACDLL.TMC304A_SetHomeMode(boardNo, (ushort)axisNo, 2);
            //if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //    return false;

            nmiMNApi.nmiAxHomeMove(boardNo, axisNo);
            //TMCACDLL.TMC304A_Home_Move(boardNo, (ushort)axisNo);
            //if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            //    return false;

            return true;
		}

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("StartMove : Axis Id {0} / Position {1} ", axisNo, position));

            //if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
            //    TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 0);
            //else
            //    TMCACDLL.TMC304A_SetSpeedMode(boardNo, (ushort)axisNo, 1);
            //if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //    return false;

            //TMCACDLL.TMC304A_SetPosSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(movingParam.MaxVelocity + 1),
            //        (uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs), (uint)(movingParam.DecelerationTimeMs + movingParam.SCurveTimeMs));
            //if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //    return false;

            //TMCACDLL.TMC304A_Abs_Move(boardNo, (ushort)axisNo, (int)position);
            //if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            //    return false;

            int nType = 0;
            double dVel = movingParam.MaxVelocity;
            double dTacc = 0, dTdec = 0;
            //switch (AxisHandler.MovingProfileType)
            //{
            //    case MovingProfileType.None: nType = 0; break;
            //    case MovingProfileType.TCurve: nType = 1; break;
            //    case MovingProfileType.SCurve: nType = 2; break;
            //}

            if (movingParam.SCurveTimeMs > 0)
            {
                nType = 2;
                dTacc = dTdec = movingParam.SCurveTimeMs;
            }
            else if (movingParam.AccelerationTimeMs > 0 || movingParam.DecelerationTimeMs > 0)
            {
                nType = 1;
                dTacc = movingParam.AccelerationTimeMs;
                dTdec = movingParam.DecelerationTimeMs;
                if (dTdec == 0)
                    dTdec = dTacc;
            }
            else
            {
                nType = 0;
            }

            nmiMNApi.nmiAxSetVelProf(boardNo, axisNo, nType, dVel, dTacc, dTdec);
            nmiMNApi.nmiAxPosMove(boardNo, axisNo, 1, position);
            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("StartRelativeMove : Axis Id {0} / Position {1} ", axisNo, position));

            int nType = 0;
            double dVel = movingParam.MaxVelocity;
            double dTacc = 0, dTdec = 0;
            if (movingParam.SCurveTimeMs > 0)
            {
                nType = 2;
                dTacc = dTdec = movingParam.SCurveTimeMs;
            }
            else if (movingParam.AccelerationTimeMs > 0 || movingParam.DecelerationTimeMs > 0)
            {
                nType = 1;
                dTacc = movingParam.AccelerationTimeMs;
                dTdec = movingParam.DecelerationTimeMs;
                if (dTdec == 0)
                    dTdec = dTacc;
            }
            else
            {
                nType = 0;
            }

            nmiMNApi.nmiAxSetVelProf(boardNo, axisNo, nType, dVel, dTacc, dTdec);
            nmiMNApi.nmiAxPosMove(boardNo, axisNo, 0, position);

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
		{
			LogHelper.Debug(LoggerType.Machine, String.Format("ContinuousMove : Axis Id {0}", axisNo));

            //       TMCACDLL.TMC304A_SetJogSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(Math.Abs(movingParam.MaxVelocity) + 1),
            //(uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs));
            //if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            //    return false;

            int nType = 0x01; //사다리꼴 가감속
            double dVel = movingParam.MaxVelocity;
            double dTacc=movingParam.AccelerationTimeMs;
            int result = nmiMNApi.nmiAxSetJogVelProf(boardNo, axisNo, nType, dVel, dTacc);

            //if (movingParam.MaxVelocity < 0)
            //    TMCACDLL.TMC304A_Jog_Move(boardNo, (ushort)axisNo, 0);
            //else
            //    TMCACDLL.TMC304A_Jog_Move(boardNo, (ushort)axisNo, 1);
            //if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            //    return false;

            int nDir = negative ? 1 : 0;
            result = nmiMNApi.nmiAxJogMove(boardNo, axisNo, nDir);

            return true;
        }

        public override void StopMove(int axisNo)
		{
            nmiMNApi.nmiAxStop(boardNo, axisNo);
		}

        public override void EmergencyStop(int axisNo)
        {
            nmiMNApi.nmiAxEStop(boardNo, axisNo);
        }

  //      private void IsMoveDone(int axisNo, out bool isMoveDone)
		//{
  //          isMoveDone = (TMCACDLL.TMC304A_Done(boardNo, (ushort)axisNo) == 0);
		//}

        public override bool ClearHomeDone(int axisNo)
        {
            int result =nmiMNApi.nmiAxHomeSetStatus(boardNo, axisNo, nmiMNApiDefs.emHOME_POS_RST0);
            return result == nmiMNApiDefs.TMC_RV_OK;
        }
        
		public void WriteOutputGroup(int groupNo, uint outputPortStatus)
		{
			Debug.Assert(groupNo == 0, "Alpha Motion Bx has only 32 output port.");

			TMCACDLL.TMC304A_PutDODWord(boardNo, (ushort)groupNo, outputPortStatus);
		}

        public uint ReadInputGroup(int groupNo)
        {
            uint value = 0;
            nmiMNApi.nmiDiGetData(boardNo, groupNo * 32, ref value);
            return value;
        }

        public uint ReadOutputGroup(int groupNo)
		{
			uint value = 0;
            var result = nmiMNApi.nmiDiGetData(boardNo, (ushort)(groupNo * 32), ref value);

            return value;
		}

        public uint ReadOutputGroup(int groupNo, int portNum)
        {
            uint value = 0;
            var result = nmiMNApi.nmiDoGetBit(boardNo, 1, (ushort)(groupNo * 32 + portNum), ref value);

            return value;
        }

        public uint ReadInputGroup(int groupNo, int portNum)
		{
			uint value = 0;
            var result = nmiMNApi.nmiDiGetBit(boardNo, 1, (ushort)(groupNo * 32 + portNum), ref value);
            return value;
		}

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            uint axisStatus = 0;
            nmiMNApi.nmiAxGetMechanical(boardNo, axisNo, ref axisStatus);
            //0x00000001 비상정지(EMG) 신호 입력 상태
            //0x00000002 Alarm 신호 입력 상태
            //0x00000004 + EL 정지 신호 입력 상태
            //0x00000008 - EL 정지 신호 입력 상태
            //0x00000010 원점 신호 상태
            //0x00000020 펄스 출력 방향 상태( 0 : +방향, - : -방향 ) 
            //0x00000040 원점 검색 완료 성공 여부
            //0x00000080 PCS(Position Override) 신호 입력 상태
            //0x00000100 CRC 신호 입력 상태
            //0x00000200 Z상 신호 입력 상태
            //0x00000400 CLR 입력 신호 상태
            //0x00000800 LATCH(Position Latch) 신호 입력 상태
            //0x00001000 SD(Slow Down) 신호 입력 상태
            //0x00002000 Inpos 신호 입력 상태
            //0x00004000 서보 온 신호 입력 상태
            //0x00008000 알람 리셋 신호 입력 상태
            //0x00010000 STA 신호 입력 상태
            //0x00020000 STP 신호 입력 상태
            //0x00040000 마스터 / 슬레이브 편차 에러 상태
            //0x00080000 겐트리 편차 에러 상태
            //0x00100000 구동중
            //0x00200000 CMP 사용중
            //0x00400000 SYNC 사용중
            //0x00800000 겐트리 사용중
            int home = 0;
            nmiMNApi.nmiAxHomeCheckDone(boardNo, axisNo, ref home);
            MotionStatus motionStatus = new MotionStatus()
            {
                origin = (axisStatus & 0x00000010) > 0,
                inp = (axisStatus & 0x00002000) > 0,
                run = (axisStatus & 0x00100000) > 0,
                posLimit = (axisStatus & 0x00000004) > 0,
                negLimit = (axisStatus & 0x00000008) > 0,
                servoOn = (axisStatus & 0x00004000) > 0,
                emg = (axisStatus & 0x00000001) > 0,
                alarm = (axisStatus & 0x00000002) > 0,
                home = home > 0,
                homeOk = (axisStatus & 0x00000040) > 0
            };

            return motionStatus;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {

        }

        public override bool ResetAlarm(int axisNo)
        {
            nmiMNApi.nmiAxSetServoReset(boardNo, axisNo, nmiMNApiDefs.emON);
            return true;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            var result = nmiMNApi.nmiDoSetBit(boardNo, 1, (ushort)(groupNo * 32 + portNo), (ushort)(value ? 1 : 0));
        }
        
        public override bool StartMultiMove(int[] axisNos, float[] position, MovingParam movingParam)
        {
            int nType = 0;
            double dVel = movingParam.MaxVelocity;
            double dTacc = 0, dTdec = 0;

            if (movingParam.SCurveTimeMs > 0)
            {
                nType = 2;
                dTacc = dTdec = movingParam.SCurveTimeMs;
            }
            else if (movingParam.AccelerationTimeMs > 0 || movingParam.DecelerationTimeMs > 0)
            {
                nType = 1;
                dTacc = movingParam.AccelerationTimeMs;
                dTdec = movingParam.DecelerationTimeMs;
                if (dTdec == 0)
                    dTdec = dTacc;
            }
            else
            {
                nType = 0;
            }


            double[] distLengths = new double[axisNos.Length];
            for (int i = 0; i < axisNos.Length; i++)
            {
                var comPos = GetCommandPos(axisNos[i]);
                distLengths[i] = Math.Abs(comPos - position[i]);
            }

            double maxDist = distLengths.Max();

            for (int i = 0; i < axisNos.Length; i++)
            {
                nmiMNApi.nmiAxSetVelProf(boardNo, axisNos[i], nType, dVel * distLengths[i] / maxDist, dTacc, dTdec);
            }
            
            nmiMNApi.nmiMxPosMove(boardNo, axisNos.Length, nmiMNApiDefs.TMC_ABS, axisNos, Array.ConvertAll(position, value => (double)value));            
            return true;
        }
        
        public override bool StartCmp(int axisNo, int startPos, float dist, bool plus)
        {
            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpEnd(boardNo, 0))
                return false;


            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpSetAxis(boardNo, 0, axisNo))
                return false;

            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpSetLevel(boardNo, 0, plus == true ? nmiMNApiDefs.TMC_LOGIC_A : nmiMNApiDefs.TMC_LOGIC_B))
                return false;

            //	트리거 신호가 유지 되는 Pulse를 설정합니다.
            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpSetHoldTime(boardNo, 0, 1))
                return false;

            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpSetMultPos(boardNo, 0, plus == true ? 0 : 1, 10000000, startPos, dist))
                return false;

            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiAxSetCmdPos(boardNo, axisNo, GetActualPos(axisNo)))
                return false;

            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpBegin(boardNo, 0))
                return false;

            return true;
        }


        public override bool EndCmp(int axisNo)
        {
            if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiCmpEnd(boardNo, axisNo))
                return false;

            return true;
        }

        //public override bool SetCoordMove(int[] axisNos, float[] positions, MovingParam movingParam)
        //{
        //    // 초기 속도를 설정합니다.
        //    if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiMCsSetInitVel(boardNo, 0, movingParam.StartVelocity))
        //        return false;

        //    // 속도 모드는 T-Curve로 합니다.
        //    if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiMCsSetVelProf(boardNo, 0, 0, 1, movingParam.MaxVelocity, movingParam.AccelerationTimeMs, movingParam.DecelerationTimeMs))
        //        return false;

        //    // 축은 0, 1번 축으로 설정합니다.
        //    if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiMCsSetAxis(boardNo, 0, 1, axisNos))
        //        return false;

        //    return true;
        //}

        //public override Task<bool> CoordMoveAbs(int[] axisNos, float[] positions)
        //{
        //    // 직선 보간 이송을 시작합니다.
        //    return Task.Run(() =>
        //    {
        //        if (nmiMNApiDefs.TMC_RV_OK != nmiMNApi.nmiMCsLinMove(boardNo, 0, nmiMNApiDefs.TMC_ABS, positions.Cast<double>().ToArray()))
        //            return false;

        //        return true;
        //    });
        //}

        public override bool ModifyPos(int axisNo, float position)
        {
            var iRet = nmiMNApi.nmiAxModifyPos(boardNo, axisNo, (double)position);
            if (nmiMNApiDefs.TMC_RV_DRV_MODIFY_POS_ERR == iRet)
                nmiMNApi.nmiAxPosMove(boardNo, axisNo, nmiMNApiDefs.TMC_ABS, position);
            else if (nmiMNApiDefs.TMC_RV_OK != iRet)
                return false;

            return true;
        }
    }
}

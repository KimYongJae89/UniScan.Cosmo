///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAcp.cs
//*
//*	AUTHOR :
//*		K.C. Lee
//*
//*	DESCRIPTION :
//*		the header file for RC files of project.
//*
//*
///****************************************************************************/


using System;
using System.Runtime.InteropServices;

namespace Shared
{
    /// <summary>
    /// Import tmcMApiAcp�� ���� ��� �����Դϴ�.
    /// </summary>
    public class TMCACDLL
    {
        public delegate void EventFunc(IntPtr lParam);


        //======================  Loading/Unloading function ====================================================//

        // �ϵ���� ��ġ�� �ε��ϰ� �ʱ�ȭ�Ѵ�.
        // 1. TMC304A_LoadDevice
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_LoadDevice();

        // �ϵ���� ��ġ�� ��ε��Ѵ�.
        // 2. TMC304A_UnloadDevice
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_UnloadDevice();


        //======================  						��ġ �ʱ�ȭ               ====================================================//

        // �ϵ���� �� ����Ʈ��� �ʱ�ȭ�Ѵ�.
        // 3. TMC304A_Reset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Reset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // ����Ʈ��� �ʱ�ȭ�Ѵ�.			
        // 4. TMC304A_SetSystemDefault
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Servo-On ��ȣ�� ����Ѵ�.
        // 5. TMC304A_PutSvOn
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� Servo-On��ȣ�� ��� ������ ��ȯ�Ѵ�.
        // 6. TMC304A_GetSvOn
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ó��               ====================================================//

        // ���� �ֱٿ� ����� �Լ��� �����ڵ带 ��ȯ�Ѵ�.
        // 7. TMC304A_GetErrorCode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetErrorCode();

        // ���� �ֱٿ� ����� �Լ��� �����ڵ带 ���ڿ��� ��ȯ���ݴϴ�.
        // 8. TMC304A_GetErrorString
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern string TMC304A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);

        // ��� �Ϸ� �� ��� ���� ���θ� ��ȯ�Ѵ�.
        // 9. TMC304A_GetErrorString
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  �ý��� I/O ȯ�� ����    ====================================================//

        // ���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� �����Ѵ�.
        // 10. TMC304A_SetSvAlm
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� ��ȯ�Ѵ�.
        // 11. TMC304A_GetSvAlm
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ �����Ѵ�.
        // 12. TMC304A_SetSvInpos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ ��ȯ�Ѵ�.
        // 13. TMC304A_GetSvInpos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ �����Ѵ�.
        // Hardware Limit Sensor ��ȣ �Է� �� �������� �Ǵ� �������� ���� ������ �����ϴ�.
        // w_stop: CMD_EMG (0), CMD_DEC (1)
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 14. TMC304A_SetHlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStopMethod, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ ��ȯ�Ѵ�.
        // 15. TMC304A_SetHlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Home sensor �� �Է� ������ �����Ѵ�. 
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 16. TMC304A_SetOrg
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Home sensor �� �Է� ������ ��ȯ�Ѵ�.
        // 17. TMC304A_GetOrg
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Z �� ������ �����Ѵ�.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 18. TMC304A_SetEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Z �� ������ ��ȯ�Ѵ�.
        // 19. TMC304A_GetEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //===================Ŭ���� ī��Ʈ�� �׻� ����� 

        // ���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ �����Ѵ�.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 20. TMC304A_SetSvCClr
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ ��ȯ�Ѵ�.
        // 21. TMC304A_GetSvCClr
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Counter Clear ��� �ð��� �����Ѵ�.
        // 22. TMC304A_SetSvCClrTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // ���� ���� Counter Clear ��� �ð��� ��ȯ�Ѵ�.
        // 23. TMC304A_GetSvCClrTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 24. TMC304A_PutSvCClrDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // 25. TMC304A_GetSvCClrDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Counter Clear ��ȣ�� On/Off ����Ѵ�.
        // 26. TMC304A_PutSvCClrCmd
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Servo-Alarm Reset ��ȣ�� ����Ѵ�.
        // w_Status: CMD_OFF(0), CMD_ON(1)
        // 27. TMC304A_PutSvAlmRst
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� Servo-Alarm Reset ��ȣ�� ��ȯ�Ѵ�. 
        // 28. TMC304A_GetSvAlmRst
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ��� ���� ��ȣ�� ������ �����Ѵ�.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        // 29. TMC304A_SetEmergency
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ��� ���� ��ȣ�� ������ ��ȯ�Ѵ�. 
        // 30. TMC304A_GetEmergency
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  ��� ���� ȯ�� ����    ====================================================//

        // ���� ���� �ִ� �ӵ��� �����Ѵ�. 
        // w_Mode:  1 = 1[PPS]   ~ 8000[PPS]
        //			10 = 10[PPS]  ~ 80000[PPS]
        //			100 = 100[PPS] ~ 800000[PPS]
        //			200 = 200[PPS] ~ 1600000[PPS]
        //			300 = 300[PPS] ~ 2400000[PPS]
        //			400 = 400[PPS] ~ 3200000[PPS]
        //			500 = 500[PPS] ~ 4000000[PPS]
        // 31. TMC304A_SetRangeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        // ���� ���� �ִ� �ӵ��� ��ȯ�Ѵ�. 
        // 32. TMC304A_GetRangeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Command �޽� ��� ��带 �����Ѵ�.
        // w_OutMode : 0	CW/CCW Positive (2 Pulse ����)
        //      	   1	CW/CCW Negative (2 Pulse �γ�)
        //			   2	Pulse Positive/Direction Low (1 Pulse ����)
        //			   3	Pulse Positive/Direction High (1 Pulse ����)
        //			   4	Pulse Negative/Direction Low (1 Pulse �γ�)
        //			   5	Pulse Negative/Direction High (1 Pulse �γ�)
        // 33. TMC304A_SetPulseMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wOutMode);

        // ���� ���� Command �޽� ��� ��带 ��ȯ�Ѵ�.
        // 34. TMC304A_GetPulseMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Feedback �޽��� �Է� ��带 �����Ѵ�.
        // w_InMode : 0	X4 (A/B�� Pulse �Է��� 4ü��� Counter��)
        //            1	X2 (A/B�� Pulse �Է��� 2ü��� Counter��)
        //            2	X1 (A/B�� Pulse �Է��� 1ü��� Counter��)
        //            3	Up/Down (Up/Down Pulse �Է����� Counter��)
        // 35. TMC304A_SetEncoderMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInMode);

        // ���� ���� Feedback �޽��� �Է� ��带 ��ȯ�Ѵ�.
        // 36. TMC304A_GetEncoderMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ �����Ѵ�.
        // w_InDir : 0 Feedback ī��Ʈ�� UP/DOWN ������ �ٲ��� �ʽ��ϴ�.
        //           1 Feedback ī��Ʈ�� UP/DOWN ������ �ݴ�� �մϴ�.
        // 37. TMC304A_SetEncoderDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        // ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ ��ȯ�Ѵ�.
        // 38. TMC304A_GetEncoderDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ��ġ �� ��� ��带 �����Ѵ�.
        // w_Mode : CMD_COMM(0), CMD_FEED(1)
        // 39. TMC304A_SetCompCountMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode);

        // ���� ���� ��ġ �� ��� ��带 ��ȯ�Ѵ�.
        // 40. TMC304A_GetCompCountMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� �����Ѵ�.
        // l_SlmtP :  -134217728 ~ 134217727
        // l_SlmtM :  -134217728 ~ 134217727
        // 41. TMC304A_SetSlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� ��ȯ�Ѵ�.
        // 42. TMC304A_GetSlmt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� �����Ѵ�.
        // dw_CommandPos :  0 ~ 134217727
        // dw_ActualPos  :  0 ~ 134217727
        // 43. TMC304A_SetCounterRing
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� ��ȯ�Ѵ�.
        // 44. TMC304A_GetCounterRing
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // ���� ���� �ý��� �Է½�ȣ�� ���� ���� �ð��� �����Ѵ�.
        // w_Time : 0x00 	0    [msec]
        //			0x01	0.25 [msec]
        //          0x02 	0.5  [msec]
        //          0x03	1    [msec]
        //          0x04	2    [msec]
        //          0x05	4    [msec]
        //          0x06	8    [msec]
        //          0x07	16   [msec]
        // 45. TMC304A_SetFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // ���� ���� �ý��� �Է� ��ȣ�� ���� ���� �ð��� ��ȯ�Ѵ�.
        // 46. TMC304A_GetFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ �����Ѵ�.
        // 47. TMC304A_SetFilterSensor
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ ��ȯ�Ѵ�.
        // 48. TMC304A_GetFilterSensor
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ �����Ѵ�. 
        // 49. TMC304A_SetFilterEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�.
        // 50. TMC304A_GetFilterEncoderZ
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ �����Ѵ�.
        // 51. TMC304A_SetFilterSvIF
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�.
        // 52. TMC304A_GetFilterSvIF
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ��� ����         ====================================================//

        // ���� ���� ����� �����ӵ� ���������� �����Ѵ�.
        // w_SpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(��ٸ��� �簨��)
        //               CMD_SMODE = 1        S-CURVE (S-CURVE ������)
        // 53. TMC304A_SetSpeedMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

        // ���� ���� ����� �����ӵ� ���������� ��ȯ�Ѵ�.
        // 54. TMC304A_GetSpeedMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Jog �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.
        // dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        // 55. TMC304A_SetJogSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        // ���� ���� �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�.
        // 56. TMC304A_GetJogSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint StartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint WorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint AccTime);

        // ���� ���� �۾��ӵ����� ������ �Ŀ� �۾��ӵ��� �����ϸ� �����κ����� ������� �Ǵ� �ܺη� ���� ������ȣ�� Active �ɶ����� ������ ���������� ����� ��� �����Ѵ�.
        // w_Direction : CMD_DIR_N(0) (-) ����, CMD_DIR_P(1) (+) ����
        // 57. TMC304A_Jog_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);

        // ���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.  
        // dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        // dwTdec    : 1 ~ 640000(5800)   ���Ӽӽð�, ������ [msec]
        // 58. TMC304A_SetPosSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime, [MarshalAs(UnmanagedType.U4)] uint dwDecTime);

        // ���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�.
        // 59. TMC304A_GetPosSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

        // ���� ���� ������ ��ġ���� ������ �Ÿ�(��� �Ÿ�)��ŭ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
        // l_Position  : - 268,435,455 ~ 268,435,455	[pulses]
        // 60. TMC304A_Inc_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

        // ���� ���� ������ ��ġ���� ������ ������ǥ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
        // l_Distance   : - 268,435,455 ~ 268,435,455	[pulses]
        // 61. TMC304A_Abs_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

        // ���� ���� ����� �Ϸ�ƴ����� üũ�մϴ�  
        // 0 ��� �۾��� �Ϸ��
        // 1 ��� �۾��� �Ϸ���� ����
        // 62. TMC304A_Done
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� �� ������ �����մϴ�.
        // 63. TMC304A_Decel_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���Ӿ��� ��� ������ �����Ѵ�.
        // 64. TMC304A_Sudden_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ���� ��� ����         ====================================================//

        // ���� �࿡ ���Ͽ� ��� �۾��� ���ÿ� �����մϴ�. 
        // nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        // waAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        // waDirList  : ������ �����ϴ� ���� �迭 �ּҰ�
        // 65. TMC304A_Multi_Jog_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] wDirList);

        // ���� �࿡ ���Ͽ� ������ ��ġ���� ������ �Ÿ���ŭ �̵��� ���ÿ� �����մϴ�
        // nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        // naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        // laDisList : �̵��� �Ÿ����� �迭 �ּҰ�
        // 66. TMC304A_Multi_Abs_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lDisList);

        // ���� �࿡ ���Ͽ� ������ ������ǥ���� �̵��� �����մϴ�
        // nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        // naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        // laPosList : �̵��� �Ÿ����� �迭 �ּҰ�
        // 67. TMC304A_Multi_Inc_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosList);

        // ���� �࿡ ���Ͽ� ������ ��� ���� ����� �Ϸ�ƴ����� üũ�մϴ�.
        // 68. TMC304A_Multi_Done
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        // ���� �࿡ ���Ͽ� ���� �� ������ �����մϴ�.
        // 69. TMC304A_Multi_Decel_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

        // ���� �࿡ ���Ͽ� ���Ӿ��� ��� ������ �����Ѵ�.
        // 70. TMC304A_Multi_Sudden_Stop
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


        //======================  ���� ����                 ====================================================//

        // ���� ���� ���� ���� ������ �����Ѵ�.
        // w_HomDir : CMD_CW (0), CMD_CCW (1)
        // 71. TMC304A_SetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

        // ���� ���� ���� ���� ������ ��ȯ�Ѵ�. 
        // 72. TMC304A_GetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ���� ��带 �����Ѵ�.  
        // w_HomMode : 0	���� ORIGIN ���� (ORG)
        //			   1	���� ORIGIN ���� (ORG + EZ)
        // 			   2	��� ORIGIN ���� (ORG)
        // 			   3	��� ORIGIN ���� (ORG + EZ)
        // 			   4	���� LIMIT ���� (LMT)
        // 			   5	���� LIMIT ���� (LMT + EZ)
        // 			   6	��� LIMIT ���� (LMT)
        // 			   7	��� LIMIT ���� (LMT + EZ)
        // 			   8	���� EZ ���� (EZ)
        // 			   9	���� ��ġ ORG ����
        // 73. TMC304A_SetHomeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

        // ���� ���� ���� ���� ��带 ��ȯ�Ѵ�. 	
        // 74. TMC304A_GetHomeMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ���� �ӵ� �� ������ �ð��� �����Ѵ�.
        // dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        // dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        // 75. TMC304A_SetHomeSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

        // ���� ���� ���� ���� �ӵ� �� ������ �ð��� ��ȯ�Ѵ�.
        // 76. TMC304A_GetHomeSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        // ���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ �����Ѵ�.
        // 77. TMC304A_SetHomeOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

        // ���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ ��ȯ�Ѵ�.
        // 78. TMC304A_GetHomeDir
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ���� �۾��� �����Ѵ�.
        // 79. TMC304A_Home_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� �࿡ ���� ���� ���� �۾��� ���ÿ� �����Ѵ�.
        // 80. TMC304A_Multi_Home_Move
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


        //======================  �ӵ� �� ��ġ �������̵�    ====================================================//

        // ���� ���� ����� ����ǰ� �ִ� �߿� �ӵ��� �������̵��� �����Ѵ�. 
        // 81. TMC304A_OverrideSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

        // ���� ���� ����� ����ǰ� �ִ� �߿� ��� ��ġ�� �������̵��� �����Ѵ�.
        // 82. TMC304A_Inc_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance);

        // ���� ���� ����� ����ǰ� �ִ� �߿� ���� ��ġ�� �������̵��� �����Ѵ�.
        // 83. TMC304A_Abs_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition);

        // ���� �࿡ ���� ����� ����ǰ� �ִ� �߿� �ӵ��� �������̵��� �����Ѵ�.
        // 84. TMC304A_Multi_OverrideSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] uint[] dwaNewWorkSpeed);

        // ���� �࿡ ���� ����� ����ǰ� �ִ� �߿� ��� ��ġ�� �������̵��� �����Ѵ�.
        // 85. TMC304A_Multi_Inc_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Inc_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewDistance);

        // ���� �࿡ ���� ����� ����ǰ� �ִ� �߿� ���� ��ġ�� �������̵��� �����Ѵ�.
        // 86. TMC304A_Multi_Abs_OverrideMove
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_Multi_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_Multi_Abs_OverrideMove([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewPosition);


        //======================  ��� �ý��� ���� ����͸� �� ��ġ �� �ӵ� ����   ====================================================//

        // ���� ���� ��� �� ���õ� �ý����� ����� ���¸� ��ȯ�Ѵ�.
        // Bit0		ORG	Origin(Home) Sensor (���� ����) [Software Check]
        // Bit1		EZ	Encoder Z (���ڴ� Z��) [Software Check]
        // Bit2		EMG	Emergency (�������) [Hardware Check]
        // Bit3		INP	Servo Inposition (���� ��ġ ���� �Ϸ�) [Software Check]
        // Bit4		ALM	Servo Alarm (���� �˶�) [Software Check]
        // Bit5		LMT+	+Limit Sensor (+����Ʈ ����) [Software Check]
        // Bit6		LMT-	-Limit Sensor (-����Ʈ ����) [Software Check]
        // Bit7		NC	NC
        // Bit8		RUN	Motion ���� ���� ��
        // Bit9		ERR	Error �߻�
        // Bit10	HOME	���� ���� Motion ���� ��
        // Bit11	H_OK	���� ���� Motion �Ϸ�
        // Bit12	NC	NC
        // Bit13	C.CLR	Servo Error Counter Clear (���� ���� ī���� Ŭ����)
        // Bit14	SON	Servo On (���� ��)
        // Bit15	A.RST	Servo Alarm Reset (���� �˶� ����)
        // 87. TMC304A_GetCardStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ��� �� ���� ���¸� ��ȯ�Ѵ�. 
        // Bit0		RUN	0�� Motion ���� ���� ��
        // Bit1		RUN	1�� Motion ���� ���� ��
        // Bit2		RUN	2�� Motion ���� ���� ��
        // Bit3		RUN	3�� Motion ���� ���� ��
        // Bit4		ERR	0�� ERROR (Error �߻�)
        // Bit5		ERR +	1�� ERROR (Error �߻�)
        // Bit6		ERR -	2�� ERROR (Error �߻�)
        // Bit7		ERR	3�� ERROR (Error �߻�)
        // Bit8		HOME	0�� ���� ���� Motion ���� ��
        // Bit9		HOME	1�� ���� ���� Motion ���� ��
        // Bit10	HOME	2�� ���� ���� Motion ���� ��
        // Bit11	HOME	3�� ���� ���� Motion ���� ��
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        // 88. TMC304A_GetMainStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ����� ���� �� ���� ���¸� ��ȯ�Ѵ�.
        // Bit0		CMP+	���� Position ���� COMP+ ������ ũ�ų� ���� ��
        // Bit1		CMP-	���� Position ���� COMP- ������ ���� ��
        // Bit2		ASND	���� �����ӿ��� ������ ��
        // Bit3		CNST	���� �����ӿ��� ����� ��
        // Bit4		DSND	���� �����ӿ��� ������ ��
        // Bit5		AASND	S�� �����ӿ��� �����ӵ��� ������ ��
        // Bit6		ACNST	S�� �����ӿ��� �����ӵ��� ������ ��
        // Bit7		ADSND	S�� �����ӿ��� �����ӵ��� ������ ��
        // Bit8		NC	NC
        // Bit9		S-ORG	ORG ��ȣ�� ���� ������ ��
        // Bit10	S-EZ	EZ ��ȣ�� ���� ������ ��
        // Bit11	NC	
        // Bit12	S-LMT+	+LMT ��ȣ�� ���� ������ ��
        // Bit13	S-LMT-	-LMT ��ȣ�� ���� ������ ��
        // Bit14	S-ALM	ALM ��ȣ�� ���� ������ ��
        // Bit15	S-EMG	EMG ��ȣ�� ���� ������ ��
        // 89. TMC304A_GetDrvStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ����� ���� ���¸� ��ȯ�Ѵ�.  
        // Bit0		E-SLMT+	+Soft Limit�� �������� ��
        // Bit1		E-SLMT-	-Soft Limit�� �������� ��
        // Bit2		E-HLMT+	LMT+ ��ȣ�� �������� ��
        // Bit3		E-HLMT-	LMT- ��ȣ�� �������� ��
        // Bit4		E-ALM	ALM ��ȣ�� �������� ��
        // Bit5		E-EMG	EMG ��ȣ�� �������� ��
        // Bit6		NC	NC
        // Bit7		E-HOME	���� ���� Motion ���� �� Error�� �߻����� ��
        // Bit8		HMST0	���� ���� Motion ���� �� Step ���� ����(���� ����)
        // Bit9		HMST1	���� ���� Motion ���� �� Step ���� ����(���� ����)
        // Bit10	HMST2	���� ���� Motion ���� �� Step ���� ����(���� ����)
        // Bit11	HMST3	���� ���� Motion ���� �� Step ���� ����(���� ����)
        // Bit12	HMST4	���� ���� Motion ���� �� Step ���� ����(���� ����)
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        // 90. TMC304A_GetErrStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ����� �ý��� ����� ���¸� ��ȯ�Ѵ�.
        // Bit0		NC	NC
        // Bit1		ORG	Origin(Home) Sensor (���� ����) ���� ��[Hardware Check]
        // Bit2		EZ	Encoder Z (���ڴ� Z��) ���� ��[Hardware Check]
        // Bit3		EMG	EMG ��ȣ ���� ��
        // Bit4		EX+	�ܺο��� + ������ ����̺�
        // Bit5		EX-	�ܺο��� - ������ ����̺�
        // Bit6		INP	Servo InPosition (���� ��ġ ���� �Ϸ�) ���� ��[Hardware Check]
        // Bit7		ALM	Servo Alarm (���� �˶�) ���� ��[Hardware Check]
        // Bit8		NC	NC
        // Bit9		NC	NC
        // Bit10	NC	NC
        // Bit11	NC	NC
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	LMT+	+Limit Sensor (+����Ʈ ����) ���� ��[Hardware Check]
        // Bit15	LMT-	Limit Sensor (-����Ʈ ����) ���� ��[Hardware Check]
        // 91. TMC304A_GetInputStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� ����� �̺�Ʈ ���¸� ��ȯ�Ѵ�.
        // Bit0		NC	NC
        // Bit1		P>=C-	�޽� ī��Ʈ�� CMP- �� ���� ���ų� ŭ
        // Bit2		P<C-	�޽� ī��Ʈ�� CMP- �� ���� ����
        // Bit3		P<C+	�޽� ī��Ʈ�� CMP+ �� ���� ����
        // Bit4		P>=C+	�޽� ī��Ʈ�� CMP+ �� ���� ���ų� ŭ
        // Bit5		C-END	���� Motion ����
        // Bit6		C-STA	���� Motion ����
        // Bit7		D-END	Motion ����
        // Bit8		NC	NC
        // Bit9		NC	NC
        // Bit10	NC	NC
        // Bit11	NC	NC
        // Bit12	NC	NC
        // Bit13	NC	NC
        // Bit14	NC	NC
        // Bit15	NC	NC
        // 92. TMC304A_GetEvtStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetEvtStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetEvtStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ������ġ(Command)���� ������ �����Ѵ�.
        // 93. TMC304A_SetCommandPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

        // ���� ���� ������ġ(Command)���� ������ ��ȯ�Ѵ�.
        // 94. TMC304A_GetCommandPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� �ǵ����ġ(Feedback)���� ������ �����Ѵ�.
        // 95. TMC304A_SetActualPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

        // ���� ���� �ǵ����ġ(Feedback)���� ������ ��ȯ�Ѵ�.
        // 96. TMC304A_GetActualPos
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // 97. TMC304A_GetCommandSpeed
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ������ ����� ====================================================//

        // 32 ���� Digital Output ä�ο� ����Ѵ�.  ( Classic )
        // 98. TMC304A_PutDO	
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // 32 ���� Digital Output ä�ο� ��ȯ�Ѵ�.  ( Classic )
        // 99. TMC304A_GetDO
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // ������ �ش� ä�ο� ��ȣ�� ����Ѵ�.
        // nChannelNo : �� ��Ʈ��
        // 0 ~ 31:
        // 100. TMC304A_PutDOBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // ������ �ش� ä�ο� ��� ��ȣ�� ��ȯ�Ѵ�.
        // 101. TMC304A_GetDOBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        // 8 ���� Digital Output ä�ο� ����Ѵ�.
        // wGroupNo : ����� ���� �� [0~7]
        // 0 : 0  ~ 7
        // 1 : 8  ~ 15
        // 2 : 16 ~ 23
        // 3 : 24 ~ 31
        // 102. TMC304A_PutDOByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

        // 8 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        // 103. TMC304A_GetDOByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);

        // 16 ���� Digital Output ä�ο� ����Ѵ�.
        // wGroupNo : ����� ���� �� [0~3]
        // 0 : 0  ~ 15
        // 1 : 16 ~ 31
        // 104. TMC304A_PutDOWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        // 16 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        // 105. TMC304A_GetDOWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort OutStatus);

        // 32 ���� Digital Output ä�ο� ����Ѵ�.
        // wGroupNo : ����� ���� �� [0~1]
        // 0 : 0  ~ 31
        // 106. TMC304A_PutDODWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

        // 32 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        // 107. TMC304A_GetDODWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint OutStatus);

        // 32 ���� Digital Input ä�ο� ��ȯ�Ѵ�. ( Classic )
        // 108. TMC304A_GetDI
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDI", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDI([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // ������ �ش� ä�ο� �Է� ��ȣ�� ��ȯ�Ѵ�.
        // 109. TMC304A_GetDIBit
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        // 8 ���� Digital Input ä�ο� ��ȯ�Ѵ�.
        // wGroupNo : ����� ���� �� [0~3]
        // 0 : 0  ~ 7
        // 1 : 8  ~ 15
        // 2 : 16 ~ 23
        // 3 : 24 ~ 31
        // 110. TMC304A_GetDIByte
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte InStatus);

        // 16 ���� Digital Input ä�ο� ��ȯ�Ѵ�.
        // wGroupNo : ����� ���� �� [0~1]
        // 0 : 0  ~ 15
        // 1 : 16 ~ 31
        // 111. TMC304A_GetDIWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort InStatus);

        // 32 ���� Digital Input ä�ο� ��ȯ�Ѵ�.
        // wGroupNo : ����� ���� �� [0]
        // 0 : 0  ~ 31
        // 112. TMC304A_GetDIDWord
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

        // Digital �Է� ��ȣ ���� ��� ������ �����Ѵ�.
        // 113. TMC304A_SetDiFilter
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // Digital �Է� ��ȣ ���� ��� ������ ��ȯ�Ѵ�.
        // 114. TMC304A_GetDiFilter
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // Digital �Է� ��ȣ ���� �ð��� �����Ѵ�.
        // 0	1.00(��sec)	0.875(��sec)	8	0.256(msec)	0.224(msec)
        // 1	2.00(��sec)	1.75(��sec)		9	0.512(msec)	0.448(msec)
        // 2	4.00(��sec)	3.50(��sec)		A	1.02(msec)	0.896(msec)
        // 3	8.00(��sec)	7.00(��sec)		B	2.05(msec)	1.79(msec)
        // 4	16.0(��sec)	14.0(��sec)		C	4.10(msec)	3.58(msec)
        // 5	32.0(��sec)	28.0(��sec)		D	8.19(msec)	7.17(msec)
        // 6	64.0(��sec)	56.0(��sec)		E	16.4(msec)	14.3(msec)
        // 7	128(��sec)	112(��sec)		F	32.8(msec)	28.7(msec)
        // 115. TMC304A_SetDiFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // Digital �Է� ��ȣ ���� �ð��� ��ȯ�Ѵ�.
        // 116. TMC304A_GetDiFilterTime
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);


        //======================  �ܺ� ��ȣ�� ���� ��� ����    ====================================================//

        // ���� ���� MPG(�����޽�) ��� ���� �����Ѵ�.
        // w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
        // w_Rate : 1 ~ 10000
        // 117. TMC304A_SetExtMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate);

        // ���� ���� MPG(�����޽�) ��� ���� ��ȯ�Ѵ�.
        // 118. TMC304A_GetExtMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetExtMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate);

        // ���� ���� MPG(�����޽�) ���� ��� ���� �����Ѵ�.
        // 119. TMC304A_SetFilterExt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� MPG(�����޽�) ���� ��� ���� ��ȯ�Ѵ�.
        // 120. TMC304A_GetFilterExt
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFilterExt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //====================== ���ͷ�Ʈ =============================================================//

        // ��Ʈ�ѷ��� ���ͷ�Ʈ ��� ������ �����Ѵ�.
        // 121. TMC304A_SetEventEnable
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEventEnable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent);

        // hWnd : ������ �ڵ�, ������ �޼����� ������ ���. ������� ������ NULL�� �Է�.
        // wMsg : ������ �ڵ��� �޼���, ������� �ʰų� ����Ʈ���� ����Ϸ��� 0�� �Է�.
        // 122. TMC304A_SetEventHandler
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetEventHandler([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint Handler, [MarshalAs(UnmanagedType.U4)] uint UiMessage);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        // EVT_NONE  = &H0  disable all event
        // EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        // EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        // EVT_D_END = &H4  D-END,    interrupt active when end of drive
        // 123. TMC304A_SetMotEventMask
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetMotEventMask([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nEventMask);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        // 124. TMC304A_GetMotEventStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort nEventMask);

        // ���� ä���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        // 125. TMC304A_SetDiEventMask
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetDiEventMask([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] uint ChannelMask1, [MarshalAs(UnmanagedType.U4)] uint ChannelMask2);

        // ���� ä���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        // 126. TMC304A_GetDiEventStatus
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint ChannelMask1, [MarshalAs(UnmanagedType.U4)] ref uint ChannelMask2);


        //====================== Advanced FUNCTIONS ===================================================//

        // ���� ���� ���� �۾� ���� ��ȯ�Ѵ�.
        // 127. TMC304A_HomeIsBusy
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_HomeIsBusy([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ���� �۾� ���� ���� �����Ѵ�.
        // 128. TMC304A_SetHomeSuccess
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� ���� �۾� ���� ���� ��ȯ�Ѵ�.
        // 129. TMC304A_GetHomeSuccess
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetHomeSuccess([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� �ְ� �ӵ� ������ �����Ѵ�.
        // 130. TMC304A_SetFixedRange
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� �ְ� �ӵ� ������ ��ȯ�Ѵ�.
        // 131. TMC304A_GetFixedRange
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetFixedRange([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ������ ��Ʈ�ѷ� ID ��ȣ�� ��ȯ�Ѵ�.
        // 132. TMC302A_GetBoardID
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        // ������ ��Ʈ�ѷ����� �����ϴ� ������ ���� ��ȯ�Ѵ�.
        // 133. TMC304A_GetAxisNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // ������ ��Ʈ�ѷ����� �����ϴ� ���� �������Է� ä�� ���� ��ȯ�Ѵ�.
        // 134. TMC304A_GetDiNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // ������ ��Ʈ�ѷ����� �����ϴ� ���� ��������� ä�� ���� ��ȯ�Ѵ�.
        // 135. TMC304A_GetDoNum
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

        // �α� ���� ���� ������ �����Ѵ�.
        // 136. TMC304A_LogCheck
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);

        // ������ ��Ʈ�ѷ��� LED �� ���� ���� Ȯ���Ѵ�.
        // 137. TMC304A_PutSvRun
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ������ ��Ʈ�ѷ��� LED �� ���� ���� ��ȯ�Ѵ�.	
        // 138. TMC304A_GetSvRun
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // Block Mode ���� ��带 �����Ѵ�.
        // 139. TMC304A_SetBlockMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);

        // Block Mode ���� ��带 ��ȯ�Ѵ�.
        // 140. TMC304A_GetBlockMode
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC304A_GetBlockMode();

        // ���� ���� ���� �ɼ� ī��Ʈ �����Ѵ�.
        // 141. TMC304A_SetAccOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset);

        // ���� ���� ���� �ɼ� ī��Ʈ ��ȯ�Ѵ�.
        // 142. TMC304A_GetAccOffset
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC304A_GetAccOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ��Ʈ�ѷ��� ������ ��ȯ�Ѵ�.
        // 143. TMC304A_BoardInfo
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);

        // �Ķ���� ������ ���Ͽ� �����Ѵ�.
        // 144. TMC304A_SaveFile
        [DllImport("tmcMApiAcp_x64.dll", EntryPoint = "TMC304A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC304A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

    }
}

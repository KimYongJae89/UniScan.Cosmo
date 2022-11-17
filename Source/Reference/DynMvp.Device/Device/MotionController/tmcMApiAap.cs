//******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAap.cs
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
	/// Import tmcMApiAap�� ���� ��� �����Դϴ�.
	/// </summary>
    public  class TMCAADLL
    {
        public delegate void EventFunc(IntPtr lParam);


        //======================  Loading/Unloading function ====================================================//
        //�ϵ���� ��ġ�� �ε��ϰ� �ʱ�ȭ�Ѵ�.
        // 1. TMC302A_LoadDevice
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC302A_LoadDevice();

        //�ϵ���� ��ġ�� ��ε��Ѵ�.	
        // 2. TMC302A_UnloadDevice
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_UnloadDevice();


        //======================  ��ġ �ʱ�ȭ     ====================================================//
        //�ϵ���� �� ����Ʈ��� �ʱ�ȭ�Ѵ�.
        //3. TMC302A_Reset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Reset([MarshalAs(UnmanagedType.U2)] ushort nConNo);
        //����Ʈ��� �ʱ�ȭ�Ѵ�.	
        //4. TMC302A_SetSystemDefault
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Servo-On ��ȣ�� ����Ѵ�.                                            
        // wStatus : CMD_OFF(0), CMD_ON(1)
        //5. TMC302A_PutSvOn
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wStatus);

        // ���� ���� Servo-On��ȣ�� ��� ������ ��ȯ�Ѵ�. 	
        //6. TMC302A_GetSvOn
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ó��               ====================================================//
        //���� �ֱٿ� ����� �Լ��� �����ڵ带 ��ȯ�Ѵ�.
        //7. TMC302A_GetErrorCode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC302A_GetErrorCode();
        //���� �ֱٿ� ����� �Լ��� �����ڵ带 ���ڿ��� ��ȯ���ݴϴ�.		
        //8. TMC302A_GetErrorString
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern   string TMC302A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);
        //��� �Ϸ� �� ��� ���� ���θ� ��ȯ�Ѵ�.		
        //9. TMC302A_GetErrorString
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC302A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  �ý��� ����� ��ȣ ���� �����Լ�    ====================================================//

        //���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� �����Ѵ�.                                            
        // wLogic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)			
        //10. TMC302A_SetSvAlm
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
        //���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� ��ȯ�Ѵ�. 
        //11. TMC302A_GetSvAlm
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);
        // ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ �����Ѵ�
        // wLogic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)				
        //12. TMC302A_SetSvInpos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ ��ȯ�Ѵ� 					
        //13. TMC302A_GetSvInpos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);


        //���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ �����Ѵ�.
        // Hardware Limit Sensor ��ȣ �Է� �� �������� �Ǵ� �������� ���� ������ �����ϴ�.
        // wStopMethod : CMD_EMG (0), CMD_DEC (1)
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        //14. TMC302A_SetHlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo,  [MarshalAs(UnmanagedType.U2)] ushort wStopMethod , [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ ��ȯ�Ѵ�.		
        //15. TMC302A_GetHlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Home sensor �� �Է� ������ �����Ѵ�.                                            
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)			
        //16. TMC302A_SetOrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
        //���� ���� Home sensor �� �Է� ������ ��ȯ�Ѵ�.				
        //17. TMC302A_GetOrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  ushort TMC302A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Z �� ������ �����Ѵ�. 
        // wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)  
        //18. TMC302A_SetEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        //���� ���� Z �� ������ ��ȯ�Ѵ�.	
        //18. TMC302A_GetEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  ushort TMC302A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ �����Ѵ�.
        //wLogic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)		  
        //Ŭ���� ī��Ʈ�� �׻� ����� 
        //19. TMC302A_SetSvCClr
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ ��ȯ�Ѵ�.		
        //20. TMC302A_GetSvCClr
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

        // ���� ���� Counter Clear ��� �ð��� �����Ѵ�.
        //wTime :  0x00 = 0.01[msec]
        //         0x01 = 0.02[msec]
        //         0x02 = 0.1 [msec]
        //         0x03 = 0.2 [msec]
        //         0x04 = 1   [msec]
        //	   0x05 = 2   [msec]
        //	   0x06 = 10  [msec]
        //	   0x07 = 20  [msec]
        //21. TMC302A_SetSvCClrTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wTime);

        // ���� ���� Counter Clear ��� �ð��� ��ȯ�Ѵ�.
        //22. TMC302A_GetSvCClrTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  ushort TMC302A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //23. TMC302A_PutSvCClrDO
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //24. TMC302A_GetSvCClrDO
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  ushort TMC302A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� Counter Clear ��ȣ�� On/Off ����Ѵ�. 
        //25. TMC302A_PutSvCClrCmd
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Servo-Alarm Reset ��ȣ�� ����Ѵ�.
        // wStatus: CMD_OFF(0), CMD_ON(1)
        //26. TMC302A_PutSvAlmRst
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wStatus);

        // ���� ���� Servo-Alarm Reset ��ȣ�� ��ȯ�Ѵ�.
        //27. TMC302A_GetSvAlmRst
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  ushort TMC302A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ��� ���� ��ȣ�� ������ �����Ѵ�.
        // w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        //28. TMC302A_SetEmergency
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
        public static extern  void TMC302A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

        // ��� ���� ��ȣ�� ������ ��ȯ�Ѵ�.
        //29. TMC302A_GetEmergency
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        //======================  ��� ���� ȯ�� ����    ====================================================//
        // ���� ���� �ִ� �ӵ��� �����Ѵ�. 
        // wMode:  1 = 1[PPS]   ~ 8000[PPS]
        //        10 = 10[PPS]  ~ 80000[PPS]
        //       100 = 100[PPS] ~ 800000[PPS]
        //       200 = 200[PPS] ~ 1600000[PPS]
        //       300 = 300[PPS] ~ 2400000[PPS]
        //       400 = 400[PPS] ~ 3200000[PPS]
        //       500 = 500[PPS] ~ 4000000[PPS]			
        //30. TMC302A_SetRangeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wMode);

        // ���� ���� �ִ� �ӵ��� ��ȯ�Ѵ�. 
        //31. TMC302A_GetRangeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Command �޽� ��� ��带 �����Ѵ�.
        //wOutMode : 0	CW/CCW Positive (2 Pulse ����)
        //      	     1	CW/CCW Negative (2 Pulse �γ�)
        //	     2	Pulse Positive/Direction Low (1 Pulse ����)
        //	     3	Pulse Positive/Direction High (1 Pulse ����)
        //	     4	Pulse Negative/Direction Low (1 Pulse �γ�)
        //	     5	Pulse Negative/Direction High (1 Pulse �γ�)
        //32. TMC302A_SetPulseMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wOutMode);

        // ���� ���� Command �޽� ��� ��带 ��ȯ�Ѵ�. 
        //33. TMC302A_GetPulseMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Feedback �޽��� �Է� ��带 �����Ѵ�.
        //wInMode : 0	X4 (A/B�� Pulse �Է��� 4ü��� Counter��)
        //          1	X2 (A/B�� Pulse �Է��� 2ü��� Counter��)
        //          2	X1 (A/B�� Pulse �Է��� 1ü��� Counter��)
        //          3	Up/Down (Up/Down Pulse �Է����� Counter��)
        //34. TMC302A_SetEncoderMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wInMode);

        // ���� ���� Feedback �޽��� �Է� ��带 ��ȯ�Ѵ�.
        //35. TMC302A_GetEncoderMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ �����Ѵ�.
        //wInDir : 0 Feedback ī��Ʈ�� UP/DOWN ������ �ٲ��� �ʽ��ϴ�.
        //         1 Feedback ī��Ʈ�� UP/DOWN ������ �ݴ�� �մϴ�.
        //36. TMC302A_SetEncoderDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

        // ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ ��ȯ�Ѵ�.
        //37. TMC302A_GetEncoderDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ��ġ �� ��� ��带 �����Ѵ�.
        // wMode : CMD_COMM(0), CMD_FEED(1)
        //38. TMC302A_SetCompCountMode(
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode);

        // ���� ���� ��ġ �� ��� ��带 ��ȯ�Ѵ�. 
        //39. TMC302A_GetCompCountMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� �����Ѵ�.
        // lSlmtP :  -134217728 ~ 134217727
        // lSlmtM :  -134217728 ~ 134217727
        //40. TMC302A_SetSlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� ��ȯ�Ѵ�.
        //41. TMC302A_GetSlmt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

        // ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� �����Ѵ�.
        // dwCommandPos :  0 ~ 134217727
        // FeedbackPos  :  0 ~ 134217727
        //42. TMC302A_SetCounterRing
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� ��ȯ�Ѵ�.  
        //43. TMC302A_GetCounterRing
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);


        // ���� ���� �ý��� �Է½�ȣ�� ���� ���� �ð��� �����Ѵ�.
        //wTime :  0x00 	0    [msec]
        //	  0x01	0.25 [msec]
        //         0x02 	0.5  [msec]
        //         0x03	1    [msec]
        //         0x04	2    [msec]
        //         0x05	4    [msec]
        //         0x06	8    [msec]
        //         0x07	16   [msec]
        //44. TMC302A_SetFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

        // ���� ���� �ý��� �Է� ��ȣ�� ���� ���� �ð��� ��ȯ�Ѵ�. 
        //45. TMC302A_GetFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        // ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ �����Ѵ�.
        //46. TMC302A_SetFilterSensor
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ ��ȯ�Ѵ�. 
        //47. TMC302A_GetFilterSensor
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        // ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ �����Ѵ�.
        //48. TMC302A_SetFilterEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

        // ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�. 
        //49. TMC302A_GetFilterEncoderZ
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        // ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ �����Ѵ�. 
        //50. TMC302A_SetFilterSvIF
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        // ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�. 
        //51. TMC302A_GetFilterSvIF
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  ���� ��� ����   ====================================================//
        //���� ���� ����� �����ӵ� ���������� �����Ѵ�.
        //wSpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(��ٸ��� �簨��)
        //             CMD_SMODE = 1        S-CURVE (S-CURVE ������)
        //52. TMC302A_SetSpeedMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

        //���� ���� ����� �����ӵ� ���������� ��ȯ�Ѵ�. 
        //53. TMC302A_GetSpeedMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //���� ���� Jog �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.
        //dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        //54. TMC302A_SetJogSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc );

        //���� ���� Jog �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�.
        //55. TMC302A_GetJogSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint StartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint WorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint AccTime);

        //���� ���� �۾��ӵ����� ������ �Ŀ� �۾��ӵ��� �����ϸ� �����κ����� ������� �Ǵ� �ܺη� ���� ������ȣ�� Active �ɶ����� ������ ���������� ����� ��� �����Ѵ�.
        //wDir : CMD_DIR_N(0) (-) ����, CMD_DIR_P(1) (+) ����
        //56. TMC302A_Jog_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);

        //���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.  
        //dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        //dwTdec    : 1 ~ 640000(5800)   ���Ӽӽð�, ������ [msec]  
        //57. TMC302A_SetPosSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc , [MarshalAs(UnmanagedType.U4)] uint dwTdec);

        //���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�. 
        //58. TMC302A_GetPosSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

        //���� ���� ������ ��ġ���� ������ �Ÿ�(��� �Ÿ�)��ŭ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
        // lDistance : - 268,435,455 ~ 268,435,455	[pulses]	
        //59. TMC302A_Inc_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

        //���� ���� ������ ��ġ���� ������ ������ǥ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
        // lPosition : - 268,435,455 ~ 268,435,455	[pulses]
        //60. TMC302A_Abs_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

        //���� ���� ����� �Ϸ�ƴ����� üũ�մϴ�  
        // 0 ��� �۾��� �Ϸ��
        // 1 ��� �۾��� �Ϸ���� ����
        //61. TMC302A_Done
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_Done([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� �� ������ �����մϴ�.
        //62. TMC302A_Decel_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���Ӿ��� ��� ������ �����Ѵ�.
        //63. TMC302A_Sudden_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //======================  ���� ���� ��� ����         ====================================================//

        //���� �࿡ ���Ͽ� ��� �۾��� ���ÿ� �����մϴ�. 
        //nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        //waAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        //waDirList  : ������ �����ϴ� ���� �迭 �ּҰ�
        //64. TMC302A_Multi_Jog_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] waDirList);

        //���� �࿡ ���Ͽ� ������ ��ġ���� ������ �Ÿ���ŭ �̵��� ���ÿ� �����մϴ�
        //nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        //naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        //laDisList : �̵��� �Ÿ����� �迭 �ּҰ�
        //65. TMC302A_Multi_Inc_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laDisList);

        //���� �࿡ ���Ͽ� ������ ������ǥ���� �̵��� �����մϴ�
        //nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
        //naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
        //laPosList : �̵��� �Ÿ����� �迭 �ּҰ�
        //66. TMC302A_Multi_Abs_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laPosList);


        //���� �࿡ ���Ͽ� ������ ��� ���� ����� �Ϸ�ƴ����� üũ�մϴ�
        //67. TMC302A_Multi_Done
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);

        //���� �࿡ ���Ͽ� ���� �� ������ �����մϴ�. 
        //68. TMC302A_Multi_Decel_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);

        //���� �࿡ ���Ͽ� ���Ӿ��� ��� ������ �����Ѵ�. 
        //69. TMC302A_Multi_Sudden_Stop
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);


        //======================  ���� ����                 ====================================================//

        //���� ���� ���� ���� ������ �����Ѵ�.
        // wHomDir : CMD_CW (0), CMD_CCW (1)
        //70. TMC302A_SetHomeDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

        //���� ���� ���� ���� ������ ��ȯ�Ѵ�.
        //71. TMC302A_GetHomeDir
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ���� ��带 �����Ѵ�.  
        //wHomMode :  0	���� ORIGIN ���� (ORG)
        //	     1	���� ORIGIN ���� (ORG + EZ)
        //	     2	��� ORIGIN ���� (ORG)
        //	     3	��� ORIGIN ���� (ORG + EZ)
        //	     4	���� LIMIT ���� (LMT)
        //	     5	���� LIMIT ���� (LMT + EZ)
        //	     6	��� LIMIT ���� (LMT)
        //	     7	��� LIMIT ���� (LMT + EZ)
        //	     8	���� EZ ���� (EZ)
        //	     9	���� ��ġ ORG ����
        //72. TMC302A_SetHomeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

        //���� ���� ���� ���� ��带 ��ȯ�Ѵ�.  
        //73. TMC302A_GetHomeMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ���� �ӵ� �� ������ �ð��� �����Ѵ�.
        //dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
        //dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
        //74. TMC302A_SetHomeSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc );

        //���� ���� ���� ���� �ӵ� �� ������ �ð��� ��ȯ�Ѵ�.
        //75. TMC302A_GetHomeSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

        //���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ �����Ѵ�.
        //76. TMC302A_SetHomeOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

        //���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ ��ȯ�Ѵ�. 
        //77. TMC302A_GetHomeOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC302A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ���� �۾��� �����Ѵ�.
        //78. TMC302A_Home_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� �࿡ ���� ���� ���� �۾��� ���ÿ� �����Ѵ�. 
        //79. TMC302A_Multi_Home_Move
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList);


        //======================  �ӵ� �� ��ġ �������̵�    ====================================================//


        //���� ���� ����� ����ǰ� �ִ� �߿� ��� ��ġ�� �������̵��� �����Ѵ�.
        //80. TMC302A_Inc_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Inc_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance );

        //���� ���� ����� ����ǰ� �ִ� �߿� ���� ��ġ�� �������̵��� �����Ѵ�.
        //81. TMC302A_Abs_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Abs_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition );

        //���� ���� ����� ����ǰ� �ִ� �߿� �ӵ��� �������̵��� �����Ѵ�.
        //82. TMC302A_OverrideSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

        //���� �࿡ ���� ����� ����ǰ� �ִ� �߿� �ӵ��� �������̵��� �����Ѵ�.
        //83. TMC302A_Multi_OverrideSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] uint[] dwaNewWorkSpeed);

        //���� �࿡ ���� ����� ����ǰ� �ִ� �߿� ��� ��ġ�� �������̵��� �����Ѵ�
        //84. TMC302A_Multi_Inc_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Inc_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewDistance );

        //���� �࿡ ���� ����� ����ǰ� �ִ� �߿� ���� ��ġ�� �������̵��� �����Ѵ�.
        //85. TMC302A_Multi_Abs_OverrideMove
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_Multi_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_Multi_Abs_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] naAxisList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] laNewPosition );


        //======================  ��� �ý��� ���� ����͸� �� ��ġ �� �ӵ� ����   ====================================================//
        //���� ���� ��� �� ���õ� �ý����� ����� ���¸� ��ȯ�Ѵ�.
        //Bit0	ORG	Origin(Home) Sensor (���� ����) [Software Check]
        //Bit1	EZ	Encoder Z (���ڴ� Z��) [Software Check]
        //Bit2	EMG	Emergency (�������) [Hardware Check]
        //Bit3	INP	Servo Inposition (���� ��ġ ���� �Ϸ�) [Software Check]
        //Bit4	ALM	Servo Alarm (���� �˶�) [Software Check]
        //Bit5	LMT+	+Limit Sensor (+����Ʈ ����) [Software Check]
        //Bit6	LMT-	-Limit Sensor (-����Ʈ ����) [Software Check]
        //Bit7	NC	NC
        //Bit8	RUN	Motion ���� ���� ��
        //Bit9	ERR	Error �߻�
        //Bit10	HOME	���� ���� Motion ���� ��
        //Bit11	H_OK	���� ���� Motion �Ϸ�
        //Bit12	NC	NC
        //Bit13	C.CLR	Servo Error Counter Clear (���� ���� ī���� Ŭ����)
        //Bit14	SON	Servo On (���� ��)
        //Bit15	A.RST	Servo Alarm Reset (���� �˶� ����)
        //86. TMC302A_GetCardStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //���� ���� ��� �� ���� ���¸� ��ȯ�Ѵ�. 
        //Bit0	RUN	0�� Motion ���� ���� ��
        //Bit1	RUN	1�� Motion ���� ���� ��
        //Bit2	RUN	2�� Motion ���� ���� ��
        //Bit3	RUN	3�� Motion ���� ���� ��
        //Bit4	ERR	0�� ERROR (Error �߻�)
        //Bit5	ERR 	1�� ERROR (Error �߻�)
        //Bit6	ERR 	2�� ERROR (Error �߻�)
        //Bit7	ERR	3�� ERROR (Error �߻�)
        //Bit8	HOME	0�� ���� ���� Motion ���� ��
        //Bit9	HOME	1�� ���� ���� Motion ���� ��
        //Bit10	HOME	2�� ���� ���� Motion ���� ��
        //Bit11	HOME	3�� ���� ���� Motion ���� ��
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //87. TMC302A_GetMainStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ����� ���� �� ���� ���¸� ��ȯ�Ѵ�.
        //Bit0	CMP+	���� Position ���� COMP+ ������ ũ�ų� ���� ��
        //Bit1	CMP-	���� Position ���� COMP- ������ ���� ��
        //Bit2	ASND	���� �����ӿ��� ������ ��
        //Bit3	CNST	���� �����ӿ��� ����� ��
        //Bit4	DSND	���� �����ӿ��� ������ ��
        //Bit5	AASND	S�� �����ӿ��� �����ӵ��� ������ ��
        //Bit6	ACNST	S�� �����ӿ��� �����ӵ��� ������ ��
        //Bit7	ADSND	S�� �����ӿ��� �����ӵ��� ������ ��
        //Bit8	NC	NC
        //Bit9	S-ORG	ORG ��ȣ�� ���� ������ ��
        //Bit10	S-EZ	EZ ��ȣ�� ���� ������ ��
        //Bit11	NC	
        //Bit12	S-LMT+	+LMT ��ȣ�� ���� ������ ��
        //Bit13	S-LMT-	-LMT ��ȣ�� ���� ������ ��
        //Bit14	S-ALM	ALM ��ȣ�� ���� ������ ��
        //88. TMC302A_GetDrvStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ����� ���� ���¸� ��ȯ�Ѵ�.  
        //Bit0	E-SLMT+	+Soft Limit�� �������� ��
        //Bit1	E-SLMT-	-Soft Limit�� �������� ��
        //Bit2	E-HLMT+	LMT+ ��ȣ�� �������� ��
        //Bit3	E-HLMT-	LMT- ��ȣ�� �������� ��
        //Bit4	E-ALM	ALM ��ȣ�� �������� ��
        //Bit5	E-EMG	EMG ��ȣ�� �������� ��
        //Bit6	NC	NC
        //Bit7	E-HOME	���� ���� Motion ���� �� Error�� �߻����� ��
        //Bit8	HMST0	���� ���� Motion ���� �� Step ���� ����(���� ����)
        //Bit9	HMST1	���� ���� Motion ���� �� Step ���� ����(���� ����)
        //Bit10	HMST2	���� ���� Motion ���� �� Step ���� ����(���� ����)
        //Bit11	HMST3	���� ���� Motion ���� �� Step ���� ����(���� ����)
        //Bit12	HMST4	���� ���� Motion ���� �� Step ���� ����(���� ����)
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //89. TMC302A_GetErrStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ����� �ý��� ����� ���¸� ��ȯ�Ѵ�.
        //Bit0	NC	NC
        //Bit1	ORG	Origin(Home) Sensor (���� ����) ���� ��[Hardware Check]
        //Bit2	EZ	Encoder Z (���ڴ� Z��) ���� ��[Hardware Check]
        //Bit3	EMG	EMG ��ȣ ���� ��
        //Bit4	EX+	�ܺο��� + ������ ����̺�
        //Bit5	EX-	�ܺο��� - ������ ����̺�
        //Bit6	INP	Servo InPosition (���� ��ġ ���� �Ϸ�) ���� ��[Hardware Check]
        //Bit7	ALM	Servo Alarm (���� �˶�) ���� ��[Hardware Check]
        //Bit8	NC	NC
        //Bit9	NC	NC
        //Bit10	NC	NC
        //Bit11	NC	NC
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	LMT+	+Limit Sensor (+����Ʈ ����) ���� ��[Hardware Check]
        //Bit15	LMT-	Limit Sensor (-����Ʈ ����) ���� ��[Hardware Check]
        //90. TMC302A_GetInputStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� ����� �̺�Ʈ ���¸� ��ȯ�Ѵ�.
        //Bit0	NC	NC
        //Bit1	P>=C-	�޽� ī��Ʈ�� CMP- �� ���� ���ų� ŭ
        //Bit2	P<C-	�޽� ī��Ʈ�� CMP- �� ���� ����
        //Bit3	P<C+	�޽� ī��Ʈ�� CMP+ �� ���� ����
        //Bit4	P>=C+	�޽� ī��Ʈ�� CMP+ �� ���� ���ų� ŭ
        //Bit5	C-END	���� Motion ����
        //Bit6	C-STA	���� Motion ����
        //Bit7	D-END	Motion ����
        //Bit8	NC	NC
        //Bit9	NC	NC
        //Bit10	NC	NC
        //Bit11	NC	NC
        //Bit12	NC	NC
        //Bit13	NC	NC
        //Bit14	NC	NC
        //Bit15	NC	NC
        //91. TMC302A_GetEvtStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetEvtStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetEvtStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ������ġ(Command)���� ������ �����Ѵ�.
        //92. TMC302A_SetCommandPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

        //���� ���� ������ġ(Command)���� ������ ��ȯ�Ѵ�. 
        //93. TMC302A_GetCommandPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� �ǵ����ġ(Feedback)���� ������ �����Ѵ�. 
        //94. TMC302A_SetActualPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

        //���� ���� �ǵ����ġ(Feedback)���� ������ ��ȯ�Ѵ�. 
        //95. TMC302A_GetActualPos
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� ���� �ӵ����� ��ȯ�Ѵ�.  
        //96. TMC302A_GetCommandSpeed
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int TMC302A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //======================  Trigger Motion ====================================================//

        //���� ���� �� ��ġ ��� �޽� ���� �����Ѵ�.
        //97. TMC302A_SetCompTrgWidth
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpWidth);


        //���� ���� �� ��ġ ��� �޽� ���� ��ȯ�Ѵ�.
        //98. TMC302A_GetCompTrgWidth
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetCompTrgWidth([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //���� ���� �� ��ġ ��� ��������� �����Ѵ�.
        //99. TMC302A_SetCompTrgMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort wLogic, [MarshalAs(UnmanagedType.U2)] ushort wEnable);


        //���� ���� �� ��ġ ��� ��������� ��ȯ�Ѵ�.
        //100. TMC302A_GetCompTrgMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetCompTrgMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ref ushort wLogic, [MarshalAs(UnmanagedType.U2)] ref ushort wEnable);

        //���� ���� ��ġ����±⿡ 1ȸ �񱳵����͸� �����Ѵ�.
        //101. TMC302A_SetCompTrgOneData
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgOneData", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetCompTrgOneData([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lStartData);


        //���� ���� �������� ��ġ �� ��� ����� ����ϱ� ���ؼ� ����������ġ �����͸� ����Ѵ�.
        //102. TMC302A_SetCompTrgTable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetCompTrgTable([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wNumData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosition);


        //���� ���� �������� ��ġ �� ��� ����� ����ϱ� ���ؼ� ������ ��ġ ������ ������ �������� ��ġ �����͸� �ڵ����� �����Ͽ� ����Ѵ�,
        //103. TMC302A_SetCompTrgContTable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetCompTrgContTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetCompTrgContTable([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wNumData, [MarshalAs(UnmanagedType.I4)] int StartData, [MarshalAs(UnmanagedType.I4)] int lInterval);


        ////�������� ��ġ �� ��� ����� �����Ѵ�.
        //104. TMC302A_SetInitCompTrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetInitCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetInitCompTrg([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        ////�������� ��ġ �� ��� ����� �����Ѵ�.
        //105. TMC302A_SetFreeCompTrg
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFreeCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetFreeCompTrg([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //======================  ���� ������ ����� ====================================================//

        //������ �ش� ä�ο� ��ȣ�� ����Ѵ�. 
        //108. TMC302A_PutDOBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //������ �ش� ä�ο� ��� ��ȣ�� ��ȯ�Ѵ�.
        //109. TMC302A_GetDOBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //8 ���� Digital Output ä�ο� ����Ѵ�.
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31
        //110. TMC302A_PutDOByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

        //8 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        //111. TMC302A_GetDOByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);

        //16 ���� Digital Output ä�ο� ����Ѵ�.                                            
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31
        //112. TMC302A_PutDOWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

        //16 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        //113. TMC302A_GetDOWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort OutStatus);

        //32 ���� Digital Output ä�ο� ����Ѵ�.                                            
        //nGroupNo : 0 	CH0  ~ CH31
        //113. TMC302A_PutDODWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

        //32 ���� Digital Output ä�ο� ��ȯ�Ѵ�.      
        //114. TMC302A_GetDODWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint OutStatus);

        //������ �ش� ä�ο� �Է� ��ȣ�� ��ȯ�Ѵ�. 
        //116. TMC302A_GetDIBit
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

        //8 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31 
        //117. TMC302A_GetDIByte
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte InStatus);

        //16 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31 
        //118. TMC302A_GetDIWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort InStatus);

        //32 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH31 
        //119. TMC302A_GetDIDWord
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

        // Digital �Է� ��ȣ ���� ��� ������ �����Ѵ�.
        //120. TMC302A_SetDiFilter
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable );


        // Digital �Է� ��ȣ ���� ��� ������ ��ȯ�Ѵ�.
        //121. TMC302A_GetDiFilter
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nConNo );


        // Digital �Է� ��ȣ ���� �ð��� �����Ѵ�.
        //0	1.00(��sec)	0.875(��sec)	8	0.256(msec)	0.224(msec)
        //1	2.00(��sec)	1.75(��sec)	9	0.512(msec)	0.448(msec)
        //2	4.00(��sec)	3.50(��sec)	A	1.02(msec)	0.896(msec)
        //3	8.00(��sec)	7.00(��sec)	B	2.05(msec)	1.79(msec)
        //4	16.0(��sec)	14.0(��sec)	C	4.10(msec)	3.58(msec)
        //5	32.0(��sec)	28.0(��sec)	D	8.19(msec)	7.17(msec)
        //6	64.0(��sec)	56.0(��sec)	E	16.4(msec)	14.3(msec)
        //7	128(��sec)	112(��sec)	F	32.8(msec)	28.7(msec)
        //122. TMC302A_SetDiFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort wTime );

        // Digital �Է� ��ȣ ���� �ð��� ��ȯ�Ѵ�.  
        //123. TMC302A_GetDiFilterTime
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nConNo );


        //======================  �ܺ� ��ȣ�� ���� ��� ����    ====================================================//
        // ���� ���� MPG(�����޽�) ��� ���� �����Ѵ�.
        //w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
        //w_Rate : 1 ~ 10000
        //124. TMC302A_SetExtMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void  TMC302A_SetExtMode( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate );


        // ���� ���� MPG(�����޽�) ��� ���� ��ȯ�Ѵ�. 
        //125. TMC302A_GetExtMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetExtMode([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate);


        //���� ���� MPG(�����޽�) ���� ��� ���� �����Ѵ�.
        //126. TMC302A_SetFilterExt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void  TMC302A_SetFilterExt( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );

        //���� ���� MPG(�����޽�) ���� ��� ���� ��ȯ�Ѵ�. 
        //127. TMC302A_GetFilterExt
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort  TMC302A_GetFilterExt( [MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo );


        //====================== ���ͷ�Ʈ �Լ� ===================================================//

        //��Ʈ�ѷ��� ���ͷ�Ʈ ��� ������ �����Ѵ�.
        //128. TMC302A_SetEventEnable
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetEventEnable( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent);

        //hWnd : ������ �ڵ�, ������ �޼����� ������ ���. ������� ������ NULL�� �Է�.
        //wMsg : ������ �ڵ��� �޼���, ������� �ʰų� ����Ʈ���� ����Ϸ��� 0�� �Է�.
        //129. TMC302A_SetEventHandler
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetEventHandler( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint hWnd, [MarshalAs(UnmanagedType.U4)] uint wMsg);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        //EVT_NONE  = &H0  disable all event
        //EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        //EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        //EVT_D_END = &H4  D-END,    interrupt active when end of drive
        //130. TMC302A_SetMotEventMask
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetMotEventMask( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEventMask);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        //131. TMC302A_GetMotEventStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wEventStatus);


        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //132. TMC302A_SetDiEventMask
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetDiEventMask( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask1, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask2);


        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //133. TMC302A_GetDiEventStatus
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus1, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus2);


        //====================== ��Ÿ ===================================================//

        // ���� ���� ���� �۾� ���� ��ȯ�Ѵ�.
        //134. TMC302A_HomeIsBusy
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort  TMC302A_HomeIsBusy( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

        // ���� ���� ���� �۾� ���� ���� �����Ѵ�.
        //135. TMC302A_SetHomeSuccess
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void  TMC302A_SetHomeSuccess( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );

        // ���� ���� ���� �۾� ���� ���� ��ȯ�Ѵ�.
        //136. TMC302A_GetHomeSuccess
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort  TMC302A_GetHomeSuccess( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

        // ���� ���� �ְ� �ӵ� ������ �����Ѵ�.
        //137. TMC302A_SetFixedRange
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void  TMC302A_SetFixedRange( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );

        // ���� ���� �ְ� �ӵ� ������ ��ȯ�Ѵ�.
        //138. TMC302A_GetFixedRange
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort  TMC302A_GetFixedRange( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


        //������ ��Ʈ�ѷ� ID ��ȣ�� ��ȯ�Ѵ�.
        //139. TMC302A_GetBoardID
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort TMC302A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //������ ��Ʈ�ѷ����� �����ϴ� ������ ���� ��ȯ�Ѵ�.
        //140. TMC302A_GetAxisNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //������ ��Ʈ�ѷ����� �����ϴ� ���� �������Է� ä�� ���� ��ȯ�Ѵ�.
        //141. TMC302A_GetDiNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //������ ��Ʈ�ѷ����� �����ϴ� ���� ��������� ä�� ���� ��ȯ�Ѵ�. 
        //142. TMC302A_GetDoNum
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nConNo);

        //�α� ���� ���� ������ �����Ѵ�.
        //143. TMC302A_LogCheck
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);

        //������ ��Ʈ�ѷ��� LED �� ���� ���� Ȯ���Ѵ�.
        //144. TMC302A_PutSvRun
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable);

        //������ ��Ʈ�ѷ��� LED �� ���� ���� ��ȯ�Ѵ�.
        //145. TMC302A_GetSvRun
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nConNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //Block Mode ���� ��带 �����Ѵ�.
        //146. TMC302A_SetBlockMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);

        //Block Mode ���� ��带 ��ȯ�Ѵ�.			
        //147. TMC302A_GetBlockMode
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC302A_GetBlockMode();

        //���� ���� ���� �ɼ� ī��Ʈ �����Ѵ�.
        //148. TMC302A_SetAccOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SetAccOffset( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset );

        //���� ���� ���� �ɼ� ī��Ʈ �����Ѵ�.			
        //149. TMC302A_GetAccOffset
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC302A_GetAccOffset( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

        //��Ʈ�ѷ��� ������ ��ȯ�Ѵ�.
        //150. TMC302A_SaveFile
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC302A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nConNo);


        //��Ʈ�ѷ��� ������ ��ȯ�Ѵ�.
        //151. TMC302A_BoardInfo
        [DllImport("tmcMApiAap_x64.dll", EntryPoint = "TMC302A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC302A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);
        //-------------------------------------------------------------------------------------------------------------------
    }
}

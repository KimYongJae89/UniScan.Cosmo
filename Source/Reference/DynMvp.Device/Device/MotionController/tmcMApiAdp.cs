///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiAdp.cs
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
	/// Import tmcMApiAdp�� ���� ��� �����Դϴ�.
	/// </summary>
    public  class TMCADDLL
    {
        public delegate void EventFunc(IntPtr lParam);
        
       
		 //======================  Loading/Unloading function ====================================================//
		// 1. TMC314A_LoadDevice
		// �ϵ���� ��ġ�� �ε��ϰ� �ʱ�ȭ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_LoadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  int TMC314A_LoadDevice();

		// 2. TMC314A_UnloadDevice
		// �ϵ���� ��ġ�� ��ε��Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_UnloadDevice", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_UnloadDevice();


		//======================  						��ġ �ʱ�ȭ               ====================================================//
		//3. TMC314A_Reset
		// �ϵ���� �� ����Ʈ��� �ʱ�ȭ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Reset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_Reset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);
			
		//4. TMC314A_SetSystemDefault
		// ����Ʈ��� �ʱ�ȭ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSystemDefault", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_SetSystemDefault([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//5. TMC314A_PutSvOn
		// ���� ���� Servo-On ��ȣ�� ����Ѵ�.
		// w_Status : CMD_OFF(0), CMD_ON(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_PutSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//6. TMC314A_GetSvOn
		// ���� ���� Servo-On��ȣ�� ��� ������ ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvOn", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  ushort TMC314A_GetSvOn([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


		//======================  ���� ó��               ====================================================//

		//7. TMC314A_GetErrorCode
		// ���� �ֱٿ� ����� �Լ��� �����ڵ带 ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrorCode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  int TMC314A_GetErrorCode();
			
		//8. TMC314A_GetErrorString
		// ���� �ֱٿ� ����� �Լ��� �����ڵ带 ���ڿ��� ��ȯ���ݴϴ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrorString", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
	   	public static extern  string TMC314A_GetErrorString([MarshalAs(UnmanagedType.I4)] int nErrorCode);
				
		//9. TMC314A_GetErrorString
		// ��� �Ϸ� �� ��� ���� ���θ� ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMotionErrCod", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  int TMC314A_GetMotionErrCod([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//======================  �ý��� I/O ȯ�� ����    ====================================================//
		//10. TMC314A_SetSvAlm
		// ���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� �����Ѵ�.                                            
		// w_Logic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

		//11. TMC314A_GetSvAlm
		// ���� ���� ���� �˶� �Է� ��ȣ�� ��� ���� �� ���� ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvAlm", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_GetSvAlm([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);
				
		//12. TMC314A_SetSvInpos
		// ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ �����Ѵ�.
		// w_Logic   : CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
				
		//13. TMC314A_GetSvInpos
		// ���� ���� Inpositon ��ȣ ��� ���� �� ��ȣ �Է� ������ ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvInpos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_GetSvInpos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);
				
		//14. TMC314A_SetHlmt
		// ���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ �����Ѵ�.
		// Hardware Limit Sensor ��ȣ �Է� �� �������� �Ǵ� �������� ���� ������ �����ϴ�.
		// w_stop: CMD_EMG (0), CMD_DEC (1)
		// w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo,  [MarshalAs(UnmanagedType.U2)] ushort wStopMethod , [MarshalAs(UnmanagedType.U2)] ushort wLogic);
				
		//15. TMC314A_SetHlmt
		// ���� ���� Hardware Limit Sensor�� ��� ���� �� ��ȣ�� �Է� ������ ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_GetHlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort StopMethod, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);
				
		//16. TMC314A_SetHlmt
		// ���� ���� Home sensor �� �Է� ������ �����Ѵ�.                                            
		// w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
				
		//17. TMC314A_SetHlmt
		// ���� ���� Home sensor �� �Է� ������ ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetOrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  ushort TMC314A_GetOrg([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
			  
		//18. TMC314A_SetEncoderZ
		// ���� ���� Z �� ������ �����Ѵ�. 
		// w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
				
		//18. TMC314A_GetEncoderZ
		// ���� ���� Z �� ������ ��ȯ�Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  ushort TMC314A_GetEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
			  
			  
		//Ŭ���� ī��Ʈ�� �׻� ����� 
		//19. TMC314A_SetSvCClr
		// ���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ �����Ѵ�.
		// w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable, [MarshalAs(UnmanagedType.U2)] ushort wLogic);
			
		//20. TMC314A_GetSvCClr
		// ���� ���� Counter Clear �Է� ��ȣ�� ��� ���� �� �Է� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClr", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_GetSvCClr([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable, [MarshalAs(UnmanagedType.U2)] ref ushort Logic);

		//21. TMC314A_SetSvCClrTime
		// ���� ���� Counter Clear ��� �ð��� �����Ѵ�.
		// w_Time : 0x00 = 0.01[msec]
		//          0x01 = 0.02[msec]
		//          0x02 = 0.1 [msec]
		//          0x03 = 0.2 [msec]
		//          0x04 = 1   [msec]
		//			0x05 = 2   [msec]
		//	 		0x06 = 10  [msec]
		//		 	0x07 = 20  [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wTime);

		//22. TMC314A_GetSvCClrTime
		// ���� ���� Counter Clear ��� �ð��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClrTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  ushort TMC314A_GetSvCClrTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//23. TMC314A_PutSvCClrDO
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_PutSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//24. TMC314A_GetSvCClrDO
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvCClrDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  ushort TMC314A_GetSvCClrDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
		  
		//25. TMC314A_PutSvCClrCmd
		// ���� ���� Counter Clear ��ȣ�� On/Off ����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvCClrCmd", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_PutSvCClrCmd([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//26. TMC314A_PutSvAlmRst
		// ���� ���� Servo-Alarm Reset ��ȣ�� ����Ѵ�.
		// w_Status: CMD_OFF(0), CMD_ON(1)	
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_PutSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//27. TMC314A_GetSvAlmRst
		// ���� ���� Servo-Alarm Reset ��ȣ�� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvAlmRst", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  ushort TMC314A_GetSvAlmRst([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//28. TMC314A_SetEmergency
		// ��� ���� ��ȣ�� ������ �����Ѵ�.
		// w_Logic: CMD_LOGIC_A(0), CMD_LOGIC_B(1)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]			  	
		public static extern  void TMC314A_SetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wLogic);

		//29. TMC314A_GetEmergency
		// ��� ���� ��ȣ�� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEmergency", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetEmergency([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);

		//======================  ��� ���� ȯ�� ����    ====================================================//
			
		//30. TMC314A_SetRangeMode
		// ���� ���� �ִ� �ӵ��� �����Ѵ�. 
		// w_Mode:  1 = 1[PPS]   ~ 8000[PPS]
		//			10 = 10[PPS]  ~ 80000[PPS]
		//			100 = 100[PPS] ~ 800000[PPS]
		//			200 = 200[PPS] ~ 1600000[PPS]
		//			300 = 300[PPS] ~ 2400000[PPS]
		//			400 = 400[PPS] ~ 3200000[PPS]
		//			500 = 500[PPS] ~ 4000000[PPS]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wMode);

		//31. TMC314A_GetRangeMode
		// ���� ���� �ִ� �ӵ��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetRangeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetRangeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//32. TMC314A_SetPulseMode
		// ���� ���� Command �޽� ��� ��带 �����Ѵ�.
		// w_OutMode : 0	CW/CCW Positive (2 Pulse ����)
		//       	   1	CW/CCW Negative (2 Pulse �γ�)
		// 			   2	Pulse Positive/Direction Low (1 Pulse ����)
		//			   3	Pulse Positive/Direction High (1 Pulse ����)
		// 			   4	Pulse Negative/Direction Low (1 Pulse �γ�)
		//			   5	Pulse Negative/Direction High (1 Pulse �γ�)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wOutMode);

		//33. TMC314A_GetPulseMode
		// ���� ���� Command �޽� ��� ��带 ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPulseMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetPulseMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//�߰�. TMC314A_SetPulseDir
		// ���� ���� Feedback �޽��� �Է� ��带 �����Ѵ�.
		// w_InMode : 0	X4 (A/B�� Pulse �Է��� 4ü��� Counter��)
		//            1	X2 (A/B�� Pulse �Է��� 2ü��� Counter��)
		//            2	X1 (A/B�� Pulse �Է��� 1ü��� Counter��)
		//            3	Up/Down (Up/Down Pulse �Է����� Counter��)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPulseDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetPulseDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);
		
		//�߰�. TMC314A_GetPulseDir
		// ���� ���� Feedback �޽��� �Է� ��带 ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPulseDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetPulseDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//34. TMC314A_SetEncoderMode
		// ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ �����Ѵ�.
		// w_InDir : 0 Feedback ī��Ʈ�� UP/DOWN ������ �ٲ��� �ʽ��ϴ�.
		//           1 Feedback ī��Ʈ�� UP/DOWN ������ �ݴ�� �մϴ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wInMode);

		//35. TMC314A_GetEncoderMode
		// ���� ���� Feedback �޽� ī��Ʈ ���� UP/DOWN ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetEncoderMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//36. TMC314A_SetEncoderDir
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wInDir);

		//37. TMC314A_GetEncoderDir
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEncoderDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetEncoderDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//38. TMC314A_SetCompCountMode
		// ���� ���� ��ġ �� ��� ��带 �����Ѵ�.
		// w_Mode : CMD_COMM(0), CMD_FEED(1) 
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode);

		//39. TMC314A_GetCompCountMode
		// ���� ���� ��ġ �� ��� ��带 ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompCountMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetCompCountMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//40. TMC314A_SetSlmt
		// ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� �����Ѵ�.
		// l_SlmtP :  -134217728 ~ 134217727
		// l_SlmtM :  -134217728 ~ 134217727
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lSlmtP, [MarshalAs(UnmanagedType.I4)] int lSlmtM, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//41. TMC314A_GetSlmt
		// ���� ���� ����Ʈ���� ����Ʈ ��� ���� �� ��ġ�� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSlmt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetSlmt([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] ref int SlmtP, [MarshalAs(UnmanagedType.I4)] ref int SlmtM, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);

		//42. TMC314A_SetCounterRing
		// ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� �����Ѵ�.
		// dw_CommandPos :  0 ~ 134217727
		// dw_ActualPos  :  0 ~ 134217727
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCommandPos, [MarshalAs(UnmanagedType.U4)] uint dwFeedbackPos, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//43. TMC314A_GetCounterRing
		// ���� ���� ��ġ �� ī���� ��� ���� �� ��ġ �ʱ�ȭ �� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCounterRing", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetCounterRing([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint CommandPos, [MarshalAs(UnmanagedType.U4)] ref uint FeedbackPos, [MarshalAs(UnmanagedType.U2)] ref ushort IsEnable);
		
		//44. TMC314A_SetFilterTime
		// ���� ���� �ý��� �Է½�ȣ�� ���� ���� �ð��� �����Ѵ�.
		// w_Time : 0x00 	0    [msec]
		// 			0x01	0.25 [msec]
		//          0x02 	0.5  [msec]
		//          0x03	1    [msec]
		//          0x04	2    [msec]
		//          0x05	4    [msec]
		//          0x06	8    [msec]
		//          0x07	16   [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wTime);

		//45. TMC314A_GetFilterTime
		// ���� ���� �ý��� �Է� ��ȣ�� ���� ���� �ð��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//46. TMC314A_SetFilterSensor
		// ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//47. TMC314A_GetFilterSensor
		// ���� ���� Emergency, Hardware Limit Sensor �� Origin(Home) Sensor ��ȣ�� ���� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterSensor", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetFilterSensor([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//48. TMC314A_SetFilterEncoderZ
		// ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//49. TMC314A_GetFilterEncoderZ
		// ���� ����  Z �� �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterEncoderZ", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetFilterEncoderZ([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
  
		//50. TMC314A_SetFilterSvIF
		// ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable);

		//51. TMC314A_GetFilterSvIF
		// ���� ����  Servo Inposition �� Servo Alarm  �Է� ��ȣ�� ���� ��� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterSvIF", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetFilterSvIF([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//======================  ���� ��� ����         ====================================================//

		//52. TMC314A_SetSpeedMode
		// ���� ���� ����� �����ӵ� ���������� �����Ѵ�.
		// w_SpeedMode : CMD_TMODE = 0        TRAPEZOIDAL(��ٸ��� �簨��)
		//               CMD_SMODE = 1        S-CURVE (S-CURVE ������)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wSpeedMode);

		//53. TMC314A_GetSpeedMode
		// ���� ���� ����� �����ӵ� ���������� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSpeedMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_GetSpeedMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//54. TMC314A_SetJogSpeed
		// ���� ���� Jog �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.
		// dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);
			
		//55. TMC314A_GetJogSpeed
		// ���� ���� �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetJogSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetJogSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

		//56. TMC314A_Jog_Move
		// ���� ���� �۾��ӵ����� ������ �Ŀ� �۾��ӵ��� �����ϸ� �����κ����� ������� �Ǵ� �ܺη� ���� ������ȣ�� Active �ɶ����� ������ ���������� ����� ��� �����Ѵ�.
		// w_Direction : CMD_DIR_N(0) (-) ����, CMD_DIR_P(1) (+) ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wDir);
			
		//57. TMC314A_SetPosSpeed
		// ���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� �����Ѵ�.  
		// dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
		// dwTdec    : 1 ~ 640000(5800)   ���Ӽӽð�, ������ [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime, [MarshalAs(UnmanagedType.U4)] uint dwDecTime);

		//58. TMC314A_GetPosSpeed
		// ���� ���� Point to Point �ʱ�ӵ�,�۾��ӵ�,���ӵ��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetPosSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime, [MarshalAs(UnmanagedType.U4)] ref uint dwpDecTime);

		//59. TMC314A_Inc_Move
		// ���� ���� ������ ��ġ���� ������ �Ÿ�(��� �Ÿ�)��ŭ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
		// l_Position  : - 268,435,455 ~ 268,435,455	[pulses]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lDistance);

		//60. TMC314A_Abs_Move
		// ���� ���� ������ ��ġ���� ������ ������ǥ �̵��� �����մϴ�. ����� ���۽�Ų �Ŀ� �ٷ� ��ȯ�մϴ�.
		// l_Distance   : - 268,435,455 ~ 268,435,455	[pulses]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lPosition);

		//61. TMC314A_Done
		// ���� ���� ����� �Ϸ�ƴ����� üũ�մϴ�  
		// 0 ��� �۾��� �Ϸ��
		// 1 ��� �۾��� �Ϸ���� ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern ushort TMC314A_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//62. TMC314A_Decel_Stop
		// ���� ���� ���� �� ������ �����մϴ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//63. TMC314A_Sudden_Stop
		// ���� ���� ���Ӿ��� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		
		//======================  ���� ���� ��� ����         ====================================================//
			
		//64. TMC314A_Multi_Jog_Move
		// ���� �࿡ ���Ͽ� ��� �۾��� ���ÿ� �����մϴ�. 
		// nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
		// waAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
		// waDirList  : ������ �����ϴ� ���� �迭 �ּҰ�
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Jog_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Jog_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] wDirList);

		//65. TMC314A_Multi_Abs_Move
		// ���� �࿡ ���Ͽ� ������ ������ǥ���� �̵��� �����մϴ�.
		// nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
		// naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
		// laPosList : �̵��� �Ÿ����� �迭 �ּҰ�
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Abs_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Abs_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lDisList);

		//66. TMC314A_Multi_Inc_Move
		// ���� �࿡ ���Ͽ� ������ ��ġ���� ������ �Ÿ���ŭ �̵��� ���ÿ� �����մϴ�.
		// nAxisNum : ���ÿ� �۾��� ������ ��� �� ����
		// naAxisList : ���ÿ� �۾��� ������ ��� ���� �迭 �ּҰ�
		// laDisList : �̵��� �Ÿ����� �迭 �ּҰ�
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Inc_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Inc_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPosList);

		//67. TMC314A_Multi_Done
		// ���� �࿡ ���Ͽ� ������ ��� ���� ����� �Ϸ�ƴ����� üũ�մϴ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Done", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_Multi_Done([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

		//68. TMC314A_Multi_Decel_Stop
		// ���� �࿡ ���Ͽ� ���� �� ������ �����մϴ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Decel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Decel_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

		//69. TMC314A_Multi_Sudden_Stop
		// ���� �࿡ ���Ͽ� ���Ӿ��� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Sudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Sudden_Stop([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum,[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);

		//======================  ���� ����                 ====================================================//
		
		//70. TMC314A_SetHomeDir
		// ���� ���� ���� ���� ������ �����Ѵ�.
		// w_HomDir : CMD_CW (0), CMD_CCW (1)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomDir);

		//71. TMC314A_GetHomeDir
		// ���� ���� ���� ���� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeDir", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetHomeDir([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//72. TMC314A_SetHomeMode
		// ���� ���� ���� ���� ��带 �����Ѵ�.  
		// w_HomMode : 0	���� ORIGIN ���� (ORG)
		//		       1	���� ORIGIN ���� (ORG + EZ)
		//	 	       2	��� ORIGIN ���� (ORG)
		//	 	       3	��� ORIGIN ���� (ORG + EZ)
		//	 	       4	���� LIMIT ���� (LMT)
		//	 	       5	���� LIMIT ���� (LMT + EZ)
		//	 	       6	��� LIMIT ���� (LMT)
		//	 	       7	��� LIMIT ���� (LMT + EZ)
		//	 	       8	���� EZ ���� (EZ)
		//	 		   9	���� ��ġ ORG ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wHomMode);

 		//73. TMC314A_GetHomeMode
		// ���� ���� ���� ���� ��带 ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetHomeMode([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//74. TMC314A_SetHomeSpeed
		// ���� ���� ���� ���� �ӵ� �� ������ �ð��� �����Ѵ�.
		// dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwStartSpeed, [MarshalAs(UnmanagedType.U4)] uint dwWorkSpeed, [MarshalAs(UnmanagedType.U4)] uint dwAccTime);

		//75. TMC314A_GetHomeSpeed
		// ���� ���� ���� ���� �ӵ� �� ������ �ð��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetHomeSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpStartSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpWorkSpeed, [MarshalAs(UnmanagedType.U4)] ref uint dwpAccTime);

		//76. TMC314A_SetHomeOffset
		// ���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lHomOffset);

		//77. TMC314A_GetHomeOffset
		// ���� ���� ���� ���ͽ� ������� ������ �ƴ� ����ڰ� ���������� �۾� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  int TMC314A_GetHomeOffset([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//78. TMC314A_Home_Move
		// ���� ���� ���� ���� �۾��� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//79. TMC314A_Multi_Home_Move
		// ���� �࿡ ���� ���� ���� �۾��� ���ÿ� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Multi_Home_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Multi_Home_Move([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNoNum, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] nAxisNoList);


		//======================  �ӵ� �� ��ġ �������̵�    ====================================================//
			
			
		//80. TMC314A_OverrideSpeed
		// ���� ���� ����� ����ǰ� �ִ� �߿� �ӵ��� �������̵��� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_OverrideSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_OverrideSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwNewWorkSpeed);

		//81. TMC314A_Inc_OverrideMove
		// ���� ���� ����� ����ǰ� �ִ� �߿� ��� ��ġ�� �������̵��� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Inc_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewDistance );

		//82. TMC314A_Abs_OverrideMove
		// ���� ���� ����� ����ǰ� �ִ� �߿� ���� ��ġ�� �������̵��� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_OverrideMove", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Abs_OverrideMove( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lNewPosition );

		//======================  ���� ��� ���� (�߰�)    ====================================================//

		// TMC314A_SetIPMapAxes
		// nMapNo		: ���������ϱ� ���� �ݵ�� 0���� ����
		// wMapIndex	: BIT 0		: 0x01
		//				  BIT 1		: 0x02
		//				  BIT 2		: 0x04
		//				  BIT 3		: 0x08
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetIPMapAxes", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetIPMapAxes	( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMapIndex );
		
		// TMC314A_SetIPPosSpeed
		// ���� �̼��� �ӵ��� �����Ѵ�.
		// dwInitVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwWorkVel : 1 ~ 4000000	�ּ� �ӵ� 1, �ִ� �ӵ� 4000000 [PPS]
		// dwTacc    : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
		// dwTdec	 : 1 ~ 640000(5800)   ���ӽð�, ������ [msec]
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetIPPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetIPPosSpeed ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwInitVel, [MarshalAs(UnmanagedType.U4)] uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] uint dwTacc, [MarshalAs(UnmanagedType.U4)] uint dwTdec );

		// TMC314A_GetIPPosSpeed
		// ���� �̼��� �ӵ��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetIPPosSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetIPPosSpeed ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] ref uint dwInitVel, [MarshalAs(UnmanagedType.U4)] ref uint dwWorkVel, [MarshalAs(UnmanagedType.U4)] ref uint dwTacc, [MarshalAs(UnmanagedType.U4)] ref uint dwTdec );

		// TMC314A_Inc_Line
		// �� �� �̻� ���Ͽ� ������ ��ġ���� ������ �Ÿ���ŭ ���� ���� ��� �����մϴ�.
		// laDisList : �̵��� �Ÿ����� �迭 �ּҰ�
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Line", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Inc_Line ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] ushort[] laDisList);

		// TMC314A_Abs_Line
		// �� �� �̻� ���Ͽ� ���� ��ġ���� ���� ���� ��� �����մϴ�.
		// laDisList : �̵��� �Ÿ����� �迭 �ּҰ�
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Line", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Abs_Line ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] ushort[] laDisList);

		// TMC314A_Inc_Arc
		// �� �࿡ ���Ͽ� ���� ��ġ���� ��ȣ ���� ��� �����մϴ�.
		// lXCentOffset : ���� �߽ɰ�
		// lYCentOffset : ������ �߽ɰ�
		// lXEndPtDist  : ���� ���ɰ� 
		// lYEndPtDist  : ������ ���ɰ� 
		// wDirection ȸ������ 0 : CW, 1 : CCW
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Arc", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Inc_Arc ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lXEndPtDist, [MarshalAs(UnmanagedType.I4)] int lYEndPtDist, [MarshalAs(UnmanagedType.U2)] ushort wDirection );

		// TMC314A_Abs_Arc
		// �� �࿡ ���Ͽ� ������ ��ġ���� ������ �Ÿ���ŭ ��ȣ ���� ��� �����մϴ�.
		// lXCentOffset : ���� �߽ɰ�
		// lYCentOffset : ������ �߽ɰ�
		// lXEndPtDist  : ���� ���ɰ� 
		// lYEndPtDist  : ������ ���ɰ� 
		// wDirection ȸ������ 0 : CW, 1 : CCW
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Arc", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Abs_Arc ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lXEndPtDist, [MarshalAs(UnmanagedType.I4)] int lYEndPtDist, [MarshalAs(UnmanagedType.U2)] ushort wDirection );

		// TMC314A_Inc_Theta
		// �� �࿡ ���Ͽ� ������ ��ġ���� ������ ������ŭ ��ȣ ���� ��� �����մϴ�.
		// lCentOffset : ����   �߽ɰ�
		// lCentOffset : ������ �߽ɰ�
		// lEnaAngle   : +�̸� CCW ���� 
		//             : -�̸� CW  ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Inc_Theta", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Inc_Theta ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lEndAngle );

		// TMC314A_Abs_Theta
		// �� �࿡ ���Ͽ� ���� ��ǥ���� ������ ������ŭ ��ȣ ���� ��� �����մϴ�.
		// lCentOffset : ����   �߽ɰ�
		// lCentOffset : ������ �߽ɰ�
		// lEnaAngle   : +�̸� CCW ���� 
		//             : -�̸� CW  ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Abs_Theta", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Abs_Theta ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lXCentOffset, [MarshalAs(UnmanagedType.I4)] int lYCentOffset, [MarshalAs(UnmanagedType.I4)] int lEndAngle );

		// TMC314A_IPDone
		// ���� ���� ���� ����� �Ϸ�ƴ����� üũ�մϴ�.
		// 0 ��� �۾��� �Ϸ��
		// 1 ��� �۾��� �Ϸ���� ����
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPDone", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_IPDone ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

		// TMC314A_IPDecel_Stop
		// ���� ���� �࿡ ���ؼ� ���� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPDecel_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_IPDecel_Stop ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

		// TMC314A_IPSudden_Stop
		// ���� ���� �࿡ ���ؼ� ���Ӿ��� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_IPSudden_Stop", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_IPSudden_Stop ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

		// TMC314A_SetInitContiTable
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetInitContiTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  uint TMC314A_SetInitContiTable ();

		// TMC314A_SetFreeContiTable
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFreeContiTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  uint TMC314A_SetFreeContiTable ();

		// TMC314A_Conti_Move
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Conti_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Conti_Move ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

		// TMC314A_Const_Move
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_Const_Move", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_Const_Move ( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );
		
		//====================== �񱳱� (�߰�) ====================================================//

		// TMC314A_SetCompTrgWidth
		// ���� ���� �� ��ġ ��� �޽� ���� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompTrgWidth( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U4)] uint dwCmpWidth );

		// TMC314A_GetCompTrgWidth
		// ���� ���� �� ��ġ ��� �޽� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompTrgWidth", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern uint TMC314A_GetCompTrgWidth( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );

		// TMC314A_SetCompTrgMode
		// ���� ���� �� ��ġ ��� ��������� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompTrgMode( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wCmpMode );

		// TMC314A_GetCompTrgMode
		// ���� ���� �� ��ġ ��� ��������� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCompTrgMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetCompTrgMode( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ref ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wCmpMode );

		// TMC314A_SetCompTrgOneData
		// ���� ���� ��ġ����±⿡ 1ȸ �񱳵����͸� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgOneData", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompTrgOneData( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lStartData );

		// TMC314A_SetCompTrgTable
		// ���� ���� �������� ��ġ �� ��� ����� ����ϱ� ���ؼ� ����������ġ �����͸� ����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompTrgTable( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nNumData, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] lPositionList );

		// TMC314A_SetCompTrgContTable
		// ���� ���� �������� ��ġ �� ��� ����� ����ϱ� ���ؼ� ������ ��ġ ������ ������ �������� ��ġ �����͸� �ڵ����� �����Ͽ� ����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCompTrgContTable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCompTrgContTable( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort nNumData, [MarshalAs(UnmanagedType.I4)] int lStartData, [MarshalAs(UnmanagedType.I4)] int lInterval );

		// TMC314A_SetInitCompTrg
		// �������� ��ġ �� ��� ����� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetInitCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetInitCompTrg( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo );

		// TMC314A_SetFreeCompTrg
		// �������� ��ġ �� ��� ����� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFreeCompTrg", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetFreeCompTrg( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo );


		//======================  ��� �ý��� ���� ����͸� �� ��ġ �� �ӵ� ����   ====================================================//

		//86. TMC314A_GetCardStatus
		// ���� ���� ��� �� ���õ� �ý����� ����� ���¸� ��ȯ�Ѵ�.
		// Bit0	ORG	Origin(Home) Sensor (���� ����) [Software Check]
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCardStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetCardStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//87. TMC314A_GetMainStatus
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMainStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetMainStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//88. TMC314A_GetDrvStatus
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDrvStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetDrvStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//89. TMC314A_GetErrStatus
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetErrStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetErrStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//90. TMC314A_GetInputStatus
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetInputStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetInputStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//91. TMC314A_GetEventStatus
		//���� ���� ���� ����� �̺�Ʈ ���¸� ��ȯ�Ѵ�.
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
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetEventStatus([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//92. TMC314A_SetCommandPos
		// ���� ���� ������ġ(Command)���� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lCommandPos);

		//93. TMC314A_GetCommandPos
		// ���� ���� ������ġ(Command)���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCommandPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int TMC314A_GetCommandPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
 
		//94. TMC314A_SetActualPos
		// ���� ���� �ǵ����ġ(Feedback)���� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_SetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lFeedbackPos);

		//95. TMC314A_GetActualPos
		// ���� ���� �ǵ����ġ(Feedback)���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetActualPos", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int TMC314A_GetActualPos([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//96. TMC314A_GetCommandSpeed
		// ���� ���� ���� �ӵ����� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetCommandSpeed", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int TMC314A_GetCommandSpeed([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


		//======================  ���� ������ ����� ====================================================//
		
		// TMC314A_PutDO
		// 32 ���� Digital Output ä�ο� ����Ѵ�.  ( Classic )
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U4)] uint wOutStatus );

		// TMC314A_GetDO
		// 32 ���� Digital Input ä�ο� ��ȯ�Ѵ�.  ( Classic )
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDO", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  uint TMC314A_GetDO([MarshalAs(UnmanagedType.U2)] ushort nBoardNo );
		
		//108. TMC314A_PutDOBit
		// ������ �ش� ä�ο� ��ȣ�� ����Ѵ�.
		// nChannelNo : �� ��Ʈ��
		// 0 ~ 31:
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nChannelNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

		//109. TMC314A_GetDOBit
		// ������ �ش� ä�ο� ��� ��ȣ�� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetDOBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nChannelNo);

		//110. TMC314A_PutDOByte
		// 8 ���� Digital Output ä�ο� ����Ѵ�.
		// nGroupNo : 0 	CH0  ~ CH7
		//		 	  1		CH8  ~ CH15
		// 			  2 	CH16 ~ CH23
		//            3		CH24 ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] byte bOutStatus);

		//111. TMC314A_GetDOByte
		// 8 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetDOByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpOutStatus);
		
		//112. TMC314A_PutDOWord
		// 16 ���� Digital Output ä�ο� ����Ѵ�.
		// nGroupNo : 0 	CH0  ~ CH15
		// 			  1 	CH16 ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ushort wOutStatus);

		//113. TMC314A_GetDOWord
		// 16 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDOWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetDOWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort wpOutStatus);

		//113. TMC314A_PutDODWord
		// 32 ���� Digital Output ä�ο� ����Ѵ�.
		// nGroupNo : 0 	CH0  ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] uint dwOutStatus);

		//114. TMC314A_GetDODWord
		// 32 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDODWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetDODWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpOutStatus);

		// TMC314A_GetDI
		// 32 ���� Digital Input ä�ο� ��ȯ�Ѵ�. ( Classic )
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDI", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  uint TMC314A_GetDI([MarshalAs(UnmanagedType.U2)] ushort nBoardNo );

		//116. TMC314A_GetDIBit
		// ������ �ش� ä�ο� �Է� ��ȣ�� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIBit", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetDIBit([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nChannelNo);
		
		//117. TMC314A_GetDIByte
		// 8 ���� Digital Input ä�ο� ��ȯ�Ѵ�.
		// nGroupNo : 0 	CH0  ~ CH7
		// 			  1		CH8  ~ CH15
		// 			  2 	CH16 ~ CH23
		//            3		CH24 ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIByte", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetDIByte([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U1)] ref byte bpInStatus);
		
        //118. TMC314A_GetDIWord
		// 16 ���� Digital Input ä�ο� ��ȯ�Ѵ�. 
		// nGroupNo : 0 	CH0  ~ CH15
		//			  1 	CH16 ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void TMC314A_GetDIWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U2)] ref ushort wpInStatus);
		
		//119. TMC314A_GetDIDWord
		// 32 ���� Digital Input ä�ο� ��ȯ�Ѵ�. 
		// nGroupNo : 0 	CH0  ~ CH31
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDIDWord", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_GetDIDWord([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort wGroupNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpInStatus);

		//120. TMC314A_SetDiFilter		
		// Digital �Է� ��ȣ ���� ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable );
    		
		//121. TMC314A_GetDiFilter
		// Digital �Է� ��ȣ ���� ��� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiFilter", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  void TMC314A_GetDiFilter([MarshalAs(UnmanagedType.U2)] ushort nBoardNo );

		//122. TMC314A_SetDiFilterTime
		// Digital �Է� ��ȣ ���� �ð��� �����Ѵ�.
		// 0	1.00(��sec)	0.875(��sec)	8	0.256(msec)	0.224(msec)
		// 1	2.00(��sec)	1.75(��sec)		9	0.512(msec)	0.448(msec)
		// 2	4.00(��sec)	3.50(��sec)		A	1.02(msec)	0.896(msec)
		// 3	8.00(��sec)	7.00(��sec)		B	2.05(msec)	1.79(msec)
		// 4	16.0(��sec)	14.0(��sec)		C	4.10(msec)	3.58(msec)
		// 5	32.0(��sec)	28.0(��sec)		D	8.19(msec)	7.17(msec)
		// 6	64.0(��sec)	56.0(��sec)		E	16.4(msec)	14.3(msec)
		// 7	128(��sec)	112(��sec)		F	32.8(msec)	28.7(msec)
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort wTime );

		//123. TMC314A_GetDiFilterTime
		// Digital �Է� ��ȣ ���� �ð��� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiFilterTime", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetDiFilterTime([MarshalAs(UnmanagedType.U2)] ushort nBoardNo );


		//======================  �ܺ� ��ȣ�� ���� ��� ����    ====================================================//
		
		//124. TMC314A_SetExtMode
		// ���� ���� MPG(�����޽�) ��� ���� �����Ѵ�.
		// w_Mode : CMD_DISABLE(0) , CMD_ENABLE(1) 
		// w_Rate : 1 ~ 10000 
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void  TMC314A_SetExtMode( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wMode, [MarshalAs(UnmanagedType.U2)] ushort wRate );

		//125. TMC314A_GetExtMode
		// ���� ���� MPG(�����޽�) ��� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetExtMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void  TMC314A_GetExtMode( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,  [MarshalAs(UnmanagedType.U2)] ref ushort wMode, [MarshalAs(UnmanagedType.U2)] ref ushort wRate );

		//126. TMC314A_SetFilterExt
		// ���� ���� MPG(�����޽�) ���� ��� ���� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void  TMC314A_SetFilterExt( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );

		//127. TMC314A_GetFilterExt
		// ���� ���� MPG(�����޽�) ���� ��� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFilterExt", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
   		public static extern  ushort  TMC314A_GetFilterExt( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo );



		//====================== Advanced FUNCTIONS ===================================================//

		//128. TMC314A_HomeIsBusy
		// ���� ���� ���� �۾� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_HomeIsBusy", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort  TMC314A_HomeIsBusy( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );


		//129. TMC314A_SetHomeSuccess
		// ���� ���� ���� �۾� ���� ���� �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
 		public static extern  void  TMC314A_SetHomeSuccess( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );


		//130. TMC314A_GetHomeSuccess
		// ���� ���� ���� �۾� ���� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetHomeSuccess", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
 		public static extern  ushort  TMC314A_GetHomeSuccess( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo );


		//131. TMC314A_SetFixedRange
		// ���� ���� �ְ� �ӵ� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
 		public static extern  void  TMC314A_SetFixedRange( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEnable );


		//132. TMC314A_GetFixedRange
		// ���� ���� �ְ� �ӵ� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetFixedRange", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
 		public static extern  ushort  TMC314A_GetFixedRange( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);


		//133. TMC314A_GetBoardID
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetBoardID", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort  TMC314A_GetBoardID([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);
  		
		//134. TMC314A_GetAxisNum
		// ������ ��Ʈ�ѷ����� �����ϴ� ������ ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetAxisNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetAxisNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);
  		
		//135. TMC314A_GetDiNum
		// ������ ��Ʈ�ѷ����� �����ϴ� ���� �������Է� ä�� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetDiNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);
  		
		//136. TMC314A_GetDoNum
		// ������ ��Ʈ�ѷ����� �����ϴ� ���� ��������� ä�� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDoNum", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  ushort TMC314A_GetDoNum([MarshalAs(UnmanagedType.U2)] ushort nBoardNo);
  		
		//137. TMC314A_LogCheck
		// �α� ���� ���� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_LogCheck", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_LogCheck([MarshalAs(UnmanagedType.U2)] ushort wLogCheck);
			
		//138. TMC314A_PutSvRun
		// ������ ��Ʈ�ѷ��� LED �� ���� ���� Ȯ���Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_PutSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_PutSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo,[MarshalAs(UnmanagedType.U2)] ushort wEnable);
						
		//139. TMC314A_GetSvRun
		// ������ ��Ʈ�ѷ��� LED �� ���� ���� ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetSvRun", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetSvRun([MarshalAs(UnmanagedType.U2)] ushort nBoardNo,[MarshalAs(UnmanagedType.U2)] ushort nAxisNo);
			
		//140. TMC314A_SetBlockMode
		// Block Mode ���� ��带 �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetBlockMode([MarshalAs(UnmanagedType.U2)] ushort wBlocking);
			
		//141. TMC314A_GetBlockMode
		// Block Mode ���� ��带 ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetBlockMode", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  		public static extern  ushort TMC314A_GetBlockMode();
			
		//142. TMC314A_SetAccOffset
		// ���� ���� ���� �ɼ� ī��Ʈ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetAccOffset( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.I4)] int lOffset );
			
		//143. TMC314A_GetAccOffset
		// ���� ���� ���� �ɼ� ī��Ʈ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetAccOffset", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
 		public static extern  int TMC314A_GetAccOffset( [MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo);

		//144. TMC314A_SaveFile
		// ������ �Ķ���͸� �����Ѵ�.
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SaveFile", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_SaveFile([MarshalAs(UnmanagedType.U2)] ushort nConNo);
			
		//145. TMC314A_BoardInfo
		// ��Ʈ�ѷ��� ������ ��ȯ�Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_BoardInfo", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_BoardInfo([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U4)] ref uint dwpBoard, [MarshalAs(UnmanagedType.U4)] ref uint dwpComm, [MarshalAs(UnmanagedType.U4)] ref uint dwpAxis, [MarshalAs(UnmanagedType.U4)] ref uint dwpDiNum, [MarshalAs(UnmanagedType.U4)] ref uint dwpDoNum);


		//====================== ���ͷ�Ʈ ( �߰� ) ===================================================//
		
		// TMC314A_SetEventEnable
		// ��Ʈ�ѷ��� ���ͷ�Ʈ ��� ������ �����Ѵ�.
		[DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEventEnable", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern  void TMC314A_SetEventEnable([MarshalAs(UnmanagedType.U2)] ushort nBoardNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent );
		
		///hWnd : ������ �ڵ�, ������ �޼����� ������ ���. ������� ������ NULL�� �Է�.
        //wMsg : ������ �ڵ��� �޼���, ������� �ʰų� ����Ʈ���� ����Ϸ��� 0�� �Է�.
        //129. TMC314A_SetEventHandler
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetEventHandler", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_SetEventHandler( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort wIsEvent, [MarshalAs(UnmanagedType.U4)] ref uint hWnd, [MarshalAs(UnmanagedType.U4)] uint wMsg);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        //EVT_NONE  = &H0  disable all event
        //EVT_C_END = &H1  C-END,    interrupt active when end of constant drive
        //EVT_C_STA = &H2  C-STA,    interrupt active when start of constant drive
        //EVT_D_END = &H4  D-END,    interrupt active when end of drive
        //130. TMC314A_SetMotEventMask
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetMotEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_SetMotEventMask( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ushort wEventMask);

        // ���� ���� ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        //131. TMC314A_GetMotEventStatus
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetMotEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetMotEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U2)] ushort nAxisNo, [MarshalAs(UnmanagedType.U2)] ref ushort wEventStatus);


        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //132. TMC314A_SetDiEventMask
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_SetDiEventMask", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern  void TMC314A_SetDiEventMask( [MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask1, [MarshalAs(UnmanagedType.U4)] uint dwChannelMask2);


        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //133. TMC314A_GetDiEventStatus
        [DllImport("tmcMApiAdp_x64.dll", EntryPoint = "TMC314A_GetDiEventStatus", ExactSpelling = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void TMC314A_GetDiEventStatus([MarshalAs(UnmanagedType.U2)] ushort nConNo, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus1, [MarshalAs(UnmanagedType.U4)] ref uint dwChannelStatus2);
    }
}

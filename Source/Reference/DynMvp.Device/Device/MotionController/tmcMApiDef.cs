///******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcMApiDef.cs
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
using System.Text;

namespace Shared
{
    public sealed class tmcDef
    {


    public const int MAX_STR_LEN_AXIS_TITLE             = 30; // Maximum string length of axis title
    public const int MAX_STR_LEN_DIST_UNIT              = 30; // Maximum string length of distance unit
    public const int MAX_STR_LEN_VEL_UNIT               = 30; // Maximum string length of velocity unit
    public const int MAX_STR_LEN_ERR                    = 128;  // Maximum error string length: a buffer to receive error string must be larger than this size.

       //***********************************************************************************************
       //									ERROR CODE DEFINITIONs										*
       //***********************************************************************************************
	//'����� �̺�Ʈ �޽���
    public const uint WM_USER = 0x400;
    public const uint WM_MC_INTERRUPT = (WM_USER + 2006);
    public const uint WM_DI_INTERRUPT = (WM_USER + 2007);
			
				/* Error code = Erroc code */ 
    public const int ERR_SUCCESS                        = 0;		// ���� ����

    public const int ERR_DEVICE_LOAD                    = -1;	// ����̽��� �ε� �Ǿ� ���� �ʽ��ϴ�.
    public const int ERR_PRM_LOAD						= -2;	// �Ķ���� ��ġ ���� �ʽ��ϴ�.
    public const int ERR_DEVICE_EXIST				    = -3;	// ������ DEVICE ID�� ���� �մϴ�.
    public const int ERR_DEVICE_PCI_BUS			        = -4;	// PCI ���� ����Ÿ�� �̻��մϴ�.
    public const int ERR_UPBOARD_LOAD				   	= -5;	// ��� ������ �߸��Ǿ����ϴ�.=  MCX302���� ���� ������  
    public const int ERR_CPLD_VER						= -6;	// CPLD VERSION ���� 
    public const int ERR_HW_TYPE						= -7;	// H/W TYPE ���� 

    public const int ERR_INVALID_HANDLE					= -100;	// ����̽� �ڵ鰪�� ����
    public const int ERR_CREATE_KERNEL_DRIVER_FAIL	    = -101;	// Ŀ�� ����̺� ���� ����=  MCX302���� ���� ������ 
    public const int ERR_CREATE_MEM_FAIL				= -102;	// �޸� ���� ����
    public const int ERR_INVALID_CHANNEL				= -200;	// ä�� ������ �߸� �Ǿ����ϴ�.
    public const int ERR_INVALID_PARAMETER			    = -300;	// �ϳ� �Ǵ� �ټ��� �Ķ���� ����
    public const int ERR_INVALID_BOARD_ID				= -400;	// ���� ID ���� ����

    public const int ERR_FILE_CREATE					= -500;	// ���� ���� ����=  MCX302���� ���� ������ 
    public const int ERR_EVENT_CREATE					= -510;	// �̺�Ʈ ���� ����=  MCX302���� ���� ������ 
    public const int ERR_THREAD_CREATE				    = -520;	// ������ ���� ����=  MCX302���� ���� ������ 

    public const int ERR_MIN_STARTV						= -512;	// START SPEED VALUE IS UNDER VALID SV
    public const int ERR_MAX_STARTV						= -533;	// START SPEED VALUE IS OVER VALID SV
    public const int ERR_MIN_WORKV						= -534;	// WORK SPEED VALUE IS UNDER VALID WV
    public const int ERR_MAX_WORKV						= -535;	// WORK SPEED IS OVER VALID WV
    public const int ERR_MIN_ACC						= -536;	// ACC TIME IS UNDER VALID AC
    public const int ERR_MAX_ACC						= -537;	// ACC TIME IS OVER VALID AC
    public const int ERR_MIN_DEC						= -538;	// DEC TIME IS UNDER VALID AC
    public const int ERR_MAX_DEC						= -539;	// DEC TIME IS OVER VALID AC

    public const int ERR_AXIS_NUM						= -540;	// �� ��ȣ ����

    public const int ERR_MAX_DISTANCE					= -541;	// �̵� �Ÿ��� �ִ밪 �̻� 

    public const int ERR_ALLOCATE_BUF					= -542;	// ���� ���� ����=  MCX302���� ���� ������ 
    public const int ERR_FREE_BUF						= -543;	// ���� �ڵ� ����=  MCX302���� ���� ������ 
    public const int ERR_BUF_DATA_NOT_ENOUGH		    = -544;	// ���� �Ѱ��� ����Ÿ�� ����=  MCX302���� ���� ������ 
    public const int ERR_BUF_DATA_OVERFLOW			    = -545;	// ���� ���� OVERFLOW=  MCX302���� ���� ������ 

    public const int ERR_AXIS_DRV_BUSY					= -546;	// �Ҵ�� ���� ��� ���� ����
    public const int ERR_AXIS_HOME_BUSY					= -547;	// �Ҵ�� ���� ORG SEARCH ���� ����
    public const int ERR_AXIS_STEADY					= -548;	// ���� ����

    public const int ERR_OVERRIDE_TYPE					= -549;	// �������̵� Ÿ�� ����

    public const int ERR_MPG_MODE						= -550;	// MPG ����� ����

    public const int ERR_COUNTER_MODE					= -551;	// ����Ʈ ����Ʈ ����� ��� ����
    public const int ERR_COMP_MODE						= -552;	// COMP Ʈ���� ����� ��� ����

    public const int ERR_STOP_BY_SLPT					= -600;	// = + ���� ����Ʈ���� ����Ʈ�� ���� ����
    public const int ERR_STOP_BY_SLMT					= -610;	// = - ���� ����Ʈ���� ����Ʈ�� ���� ����
    public const int ERR_STOP_BY_HLPT					= -620;	// = + ���� �ϵ���� ����Ʈ�� ���� ����
    public const int ERR_STOP_BY_HLMT					= -630;	// = - ���� �ϵ���� ����Ʈ�� ���� ����
    public const int ERR_STOP_BY_ALM					= -640;	// ���� �˶��� ���� ����
    public const int ERR_STOP_BY_EMG					= -650;	// ��������� ���� ����
    public const int ERR_STOP_BY_EZ						= -660;	// EZ�� ���� ����

    public const int ERR_STOP_BY_ERR_LIMIT			    = -700;	// ������ ���� ���� ���� �ʰ�
    public const int ERR_STOP_BY_COMPARE_NO			    = -701;	// ������ ���� ���� ȸ�� �ʰ�
    public const int ERR_STOP_BY_HUNTING_NO			    = -702;	// ������ ���� ���� ȸ�� �ʰ�

    public const int ERR_UNKNOWN						= -9999;	// �˼� ���� ����	
    				
    				
    public const int CMD_FALSE						    =  0;  //OFF
    public const int CMD_TRUE							=  1;  // ON

    public const int CMD_TMODE						    =  0;   //TRAPEZOIDAL= ��ٸ��� �簨��
    public const int CMD_SMODE				            =  1;  //S-CURVE = S-CURVE ������

    public const int CMD_DIR_N							=  0;  //= - ����
    public const int CMD_DIR_P							=  1;  //= + ����

    public const int CMD_CW								=  0;  //CW ���� 
    public const int CMD_CCW							=  1;  //CCW ����				

    public const int CMD_STAND						    =  0;  //��� ���� ����
    public const int CMD_RUNNING						=  1;  //������� ����

    public const int ARC_CW								=  0;  //��ȣ�ð����= CW ȸ��
    public const int ARC_CCW							=  1;  //��ȣ�ݽð����= CCW ȸ��				

    public const int CMD_LOGIC_A						=  0;   //A ���� 
    public const int CMD_LOGIC_B					    =  1;   //B ����

    public const int CMD_EMG							=  0;  //���� ���� ��� ����
    public const int CMD_DEC							=  1;  //���� ����

    public const int CMD_OFF							=  0;	// OFF = 0
    public const int CMD_ON								=  1;	// ON = 1 

    public const int CMD_DISABLE						=  0;	// DISABLE = 0
    public const int CMD_ENABLE							=  1;	// ONLY DEBUG = 1
    public const int CMD_PARAMETER						=  2;	// ONLY PARAMTER = 2
    public const int CMD_ALL 							=  3;	// DEBUG AND PARAMETER = 3				

    public const int CMD_COMM							=  0;   // ������ġ= Command Position
    public const int CMD_FEED							=  1;   // ������ġ= Feedback Position

    public const int CMD_EAX4							=  0;   //4ü��
    public const int CMD_EAX2							=  1;   //2ü��
    public const int CMD_EAX1							=  2;   //1ü��
    public const int CMD_UP_DOWN						=  3;   //UP_DOWN

    public const int CMD_OVER_NONE						=  0;   // �������̵� �ȵ�
    public const int CMD_OVER_INC						=  1;   // INC �������̵� 
    public const int CMD_OVER_ABS						=  2;   // ABS �������̵� 
            

	public const int CMD_INT_MESSAGE                   = 0;            //������ �޽��� ���� ����� ���Ͽ� ���ͷ�Ʈ ����
	public const int CMD_INT_CALLBACK                  = 1;           //�ݹ��Լ��� ���Ͽ� ���ͷ�Ʈ ����
    public const int CMD_INT_EVENT                     = 2;           //�̺�Ʈ�� ���Ͽ� ���ͷ�Ʈ ����

	public const int CMD_EVT_MC                        = 0;                 //motion evnet �߻�
	public const int CMD_EVT_IO                        = 1;                 //���� I/O evnet �߻�

	public const int EVT_DISABLE                       = 0;                // disable all event
	public const int EVT_ENABLE                        = 1;                 // enable  all event

	public const int EVT_NONE                          = 0x00;               //disable all event
	public const int EVT_C_END                         = 0x01;               //C-END,    interrupt active when end of constant drive
	public const int EVT_C_STA                         = 0x02;               //C-STA,    interrupt active when start of constant drive
	public const int EVT_D_END                         = 0x04;               // D-END,    interrupt active when end of drive


	public const int EVT_P_GE_CM                       = 0x02;             // P >= C-, interrupt active when P counter >= Comp- counter
	public const int EVT_P_L_CM                        = 0x04;             // P < C-,  interrupt active when P counter < Comp- counter
	public const int EVT_P_L_CP                        = 0x08;             // P < C-,   interrupt active when P counter < Comp- counter
    public const int EVT_P_GE_CP                       = 0x10;              // P >= C+,  interrupt active when P counter >= Comp+ counter












				

    }
}

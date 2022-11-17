//******************************************************************************
//*
//*	File Version: 1,0,0,0
//*
//*	Copyright (c) Alpha Motion 2011-
//*
//*	This file is strictly confidential and do not distribute it outside.
//*
//*	MODULE NAME :
//*		tmcDApiAed.cs
//*
//*	AUTHOR :
//*		K.C. Lee
//*
//*	DESCRIPTION :
//*		the header file for RC files of project.
//*
//*
///****************************************************************************/
/******************************************************************************
*	TMC-Axxxxx �ø����� �Լ��� TMC-Bxxxxx �ø��� �Լ��� ü�谡 ���� �ִµ�
    ����� ���Ǹ� ���� ���� ����� �Լ��� ������ �ø�� �´� �Լ��� �и�
	�����Ͽ� ����	
*	TMC-Axxxxx �Լ��� ����ڴ� // TMC-Axxxxx Series // �κ� ����
*	TMC-Bxxxxx �Լ��� ����ڴ� // TMC-Bxxxxx Series // �κ� ����

*	Return code �ٸ�
/****************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace Shared
{
	/// <summary>
    /// Import tmcDApiAed�� ���� ��� �����Դϴ�.
	/// </summary>
    public  class TMCAEDLL
    {
        public delegate void EventFunc(IntPtr lParam);

        ///////////////////////////////////////////////////////////
        //                                                       //
        //           TMC-AxxxxP Series Function Name             //
        //                                                       //
        ///////////////////////////////////////////////////////////
        //******************************************************************************************************//

        //======================  Loading/Unloading function ====================================================//
        //�ϵ���� ��ġ�� �ε��ϰ� �ʱ�ȭ�Ѵ�.
        // 1. AIO_LoadDevice
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_LoadDevice();

        //�ϵ���� ��ġ�� ��ε��Ѵ�.	
        // 2. AIO_UnloadDevice
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_UnloadDevice();

        //======================  ���� ó��               ====================================================//
        //���� �ֱٿ� ����� �Լ��� �����ڵ带 ��ȯ�Ѵ�.
        //7. AIO_GetErrorCode
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetErrorCode();

        //���� �ֱٿ� ����� �Լ��� �����ڵ带 ���ڿ��� ��ȯ���ݴϴ�.		
        //8. AIO_GetErrorString
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern string AIO_GetErrorString( int nErrorCode );

        //======================  ��ġ �ʱ�ȭ     ====================================================//
        //�ϵ���� �� ����Ʈ��� �ʱ�ȭ�Ѵ�.
        //3. AIO_Reset
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_Reset( ushort nConNo );

        //����Ʈ��� �ʱ�ȭ�Ѵ�.	
        //4. AIO_SetSystemDefault
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetSystemDefault( ushort nConNo );

        //======================  ���� ������ ����� ====================================================//
        // Digital �Է� ��ȣ ���� ��� ������ �����Ѵ�.
        //5. AIO_SetDiFilter
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetDIFilter( ushort nConNo, ushort wEnable );


        // Digital �Է� ��ȣ ���� ��� ������ ��ȯ�Ѵ�.
        //6. AIO_GetDiFilter
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIFilter( ushort nConNo, ref ushort wpEnable );

        // Digital �Է� ��ȣ ���� �ð��� �����Ѵ�.
        //0	1.00(��sec)	0.875(��sec)	8	0.256(msec)	0.224(msec)
        //1	2.00(��sec)	1.75(��sec)	9	0.512(msec)	0.448(msec)
        //2	4.00(��sec)	3.50(��sec)	A	1.02(msec)	0.896(msec)
        //3	8.00(��sec)	7.00(��sec)	B	2.05(msec)	1.79(msec)
        //4	16.0(��sec)	14.0(��sec)	C	4.10(msec)	3.58(msec)
        //5	32.0(��sec)	28.0(��sec)	D	8.19(msec)	7.17(msec)
        //6	64.0(��sec)	56.0(��sec)	E	16.4(msec)	14.3(msec)
        //7	128(��sec)	112(��sec)	F	32.8(msec)	28.7(msec)
        //7. AIO_SetDiFilterTime
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetDIFilterTime( ushort nConNo, ushort wTime );

        // Digital �Է� ��ȣ ���� �ð��� ��ȯ�Ѵ�.  
        //8. AIO_GetDiFilterTime
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIFilterTime( ushort nConNo, ref ushort wpTime );

        //������ �ش� ä�ο� ��ȣ�� ����Ѵ�. 
        //9. AIO_PutDOBit
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_PutDOBit( ushort nConNo,  ushort nChannelNo, ushort wOutStatus );

        //������ �ش� ä�ο� ��� ��ȣ�� ��ȯ�Ѵ�.
        //10. AIO_GetDOBit
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDOBit( ushort nConNo, ushort nChannelNo, ref ushort wpOutStatus );

        //8 ���� Digital Output ä�ο� ����Ѵ�.
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31
        //11. AIO_PutDOByte
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_PutDOByte( ushort nConNo, ushort wGroupNo, byte byOutStatus );

        //8 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        //12. AIO_GetDOByte
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDOByte( ushort nConNo, ushort wGroupNo, ref byte bypOutStatus );

        //16 ���� Digital Output ä�ο� ����Ѵ�.                                            
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31
        //13. AIO_PutDOWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_PutDOWord( ushort nConNo, ushort wGroupNo, ushort wOutStatus );

        //16 ���� Digital Output ä�ο� ��ȯ�Ѵ�.
        //14. AIO_GetDOWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDOWord( ushort nConNo, ushort wGroupNo, ref ushort wpOutStatus );

        //32 ���� Digital Output ä�ο� ����Ѵ�.                                            
        //nGroupNo : 0 	CH0  ~ CH31
        //15. AIO_PutDODWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_PutDODWord( ushort nConNo,  ushort wGroupNo,  uint dwOutStatus );

        //32 ���� Digital Output ä�ο� ��ȯ�Ѵ�.      
        //16. AIO_GetDODWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDODWord( ushort nConNo,  ushort wGroupNo,  ref uint dwpOutStatus );

        //������ �ش� ä�ο� �Է� ��ȣ�� ��ȯ�Ѵ�. 
        //17. AIO_GetDIBit
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIBit( ushort nConNo, ushort nChannelNo, ref ushort wpInStatus );

        //8 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH7
        //	    1	CH8  ~ CH15
        //	    2 	CH16 ~ CH23
        //           3	CH24 ~ CH31 
        //18. AIO_GetDIByte
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIByte( ushort nConNo, ushort wGroupNo, ref byte bypInStatus );

        //16 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH15
        //	    1 	CH16 ~ CH31 
        //19. AIO_GetDIWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIWord( ushort nConNo, ushort wGroupNo, ref ushort wpInStatus );

        //32 ���� Digital Input ä�ο� ��ȯ�Ѵ�.   
        //nGroupNo : 0 	CH0  ~ CH31 
        //20. AIO_GetDIDWord
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDIDWord( ushort nConNo, ushort wGroupNo, ref uint dwpInStatus );

        //������ ��Ʈ�ѷ��� LED �� ���� ���� Ȯ���Ѵ�.
        //21. AIO_PutIoRun
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_PutIoRun( ushort nConNo, ushort wStatus );

        //������ ��Ʈ�ѷ��� LED �� ���� ���� ��ȯ�Ѵ�.
        //22. AIO_GetIoRun
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetIoRun( ushort nConNo, ref ushort wpStatus );

        //====================== ���ͷ�Ʈ �Լ� ===================================================//
        //��Ʈ�ѷ��� ���ͷ�Ʈ ��� ������ �����Ѵ�.
        //23. AIO_SetEventEnable
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetEventEnable( ushort nConNo, ushort wIsEvent );

        //hWnd : ������ �ڵ�, ������ �޼����� ������ ���. ������� ������ NULL�� �Է�.
        //wMsg : ������ �ڵ��� �޼���, ������� �ʰų� ����Ʈ���� ����Ϸ��� 0�� �Է�.
        //24. AIO_SetEventHandler
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetEventHandler( ushort nConNo, ushort wHandleType, int hHandle, uint wiMessage );

        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �����Ѵ�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //25. AIO_SetDiEventMask
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_SetDiEventMask( ushort nConNo, uint dwChannelMask1, uint dwChannelMask2 );

        // ���� ä������ ����ڰ� ������ ���ͷ�Ʈ �߻� ���θ� �д´�.
        // nChNoMask1 : CH0  ~ CH31
        // nChNoMask2 : CH32 ~ CH63
        //26. AIO_GetDiEventStatus
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDiEventStatus( ushort nConNo, ref uint dwpChannelStatus1, ref uint dwpChannelStatus2 );

        //====================== ��Ÿ ===================================================//
        //������ ��Ʈ�ѷ� ID ��ȣ�� ��ȯ�Ѵ�.
        //27. AIO_GetBoardID
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetBoardID( ushort nConNo );

        //��Ʈ�ѷ��� ������ ��ȯ�Ѵ�.
        //28. AIO_BoardInfo
        //npModel = Board Model
        //0xAE	= AE 
        //0xAF1 = AF(Only DI)
        //0xAF2 = AF(Only DO)
        //0xAF3 = AF(DIO)
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_BoardInfo( ushort nConNo, ref uint npModel, ref uint dwpComm, ref uint dwpDiNum, ref uint dwpDoNum );

        //������ ��Ʈ�ѷ����� �����ϴ� ���� �������Է� ä�� ���� ��ȯ�Ѵ�.
        //29. AIO_GetDiNum
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDiNum( ushort nConNo, ref ushort wpDiNum );

        //������ ��Ʈ�ѷ����� �����ϴ� ���� ��������� ä�� ���� ��ȯ�Ѵ�. 
        //30. AIO_GetDoNum
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_GetDoNum( ushort nConNo, ref ushort wpDoNum );

        //������ ��Ʈ�ѷ����� Dll ������ ��ȯ�Ѵ�. 
        //31. AIO_GetDllVersion
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern uint AIO_GetDllVersion( ref string cpDllName );

        //��Ʈ�ѷ��� ������ �����Ѵ�.
        //32. AIO_SaveFile
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern uint AIO_SaveFile( ushort nConNo );

        //�α� ���� ���� ������ �����Ѵ�.
        //33. AIO_LogCheck
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_LogCheck( ushort wLogCheck );       

        //-------------------------------------------------------------------------------------------------------------------
        ///////////////////////////////////////////////////////////
        //                                                       //
        //            TMC-BxxxxP Series Function Name            //
        //                                                       //
        ///////////////////////////////////////////////////////////
        //******************************************************************************************************//

        //====================== System Management Function ====================================================//
        //bManual = FALE, Con Number is set automatically
        //bManual = TRUE, Con Number is set to dip switch(Default)
        //npNumCons In a computer-set number of board
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiSysLoad( int bManual, ref int nNumCons );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiSysUnload();

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConInit( int nConNo );

        //====================== Digital In/Out FUNCTIONS =============================================//
        //0 nGroupNo => 0  ~ 31
        //1 nGroupNo => 32 ~ 63
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiGetData( int nConNo, int nGroupNo, ref uint dwpInStatus );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiGetBit( int nConNo, int nChannelNo, ref uint npInStatus );

        //0 nGroupNo => 0  ~ 31
        //1 nGroupNo => 32 ~ 63
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDoSetData( int nConNo, int nGroupNo, uint dwOutStatus );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDoGetData( int nConNo, int nGroupNo, ref uint dwpOutStatus );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDoSetBit( int nConNo, int nGroupNo, uint nOutStatus );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDoGetBit( int nConNo, int nGroupNo, ref uint npOutStatus );

        //0 nId => 0  ~ 15
        //1 nId => 16 ~ 31
        //2 nId => 32 ~ 47
        //3 nId => 48 ~ 63
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiSetFilterEnable( int nConNo, int nId, int nEnable );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiGetFilterEnable( int nConNo, int nId, ref int npEnable );

        //0 nId => 0  ~ 15
        //1 nId => 16 ~ 31
        //2 nId => 32 ~ 47
        //3 nId => 48 ~ 63
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiSetFilterTime( int nConNo, int nId, int nTime );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDiGetFilterTime( int nConNo, int nId, ref int npTime );

        //====================== Motion Parameter Management Function ====================================================//
        // �Ķ���� ���� ��δ� C:\Program Files\Alpha Motion\DigitalPro\Parameter\' �� ����
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConParamSave( int nConNo );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConParamLoad( int nConNo );

        //====================== DEBUG-LOGGING FUNCTIONS ==============================================//
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDLogSetFile( string szFilename );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDLogGetFile( ref string szpFilename );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDLogSetLevel( int nLevel );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiDLogGetLevel( ref int npLevel );

        //====================== ERROR HANDLING FUNCTIONS =============================================//
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiErrGetCode( int nConNo, ref int npCode );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiErrGetString( int nConNo, int nErrorCode, ref string szpStr );

        //====================== interrupt FUNCTIONS ==================================================//
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiIntSetHandler( int nConNo, int nType, int hHandler, uint nMsg );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiIntSetHandlerEnable( int nConNo, int nEnable );

        //0 nGroup => 0  ~ 31
        //1 nGroup => 32 ~ 63
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiIntSetDiEnable( int nConNo, int nGroup, uint nMask );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiIntGetDiStatus( int nConNo, int nGroup, ref uint npStatus );

        //===============================================================================================//
        //��� ���忡 ��ġ�� ������ ���� I/O ����
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiGnGetDioNum( int nConNo, ref int npDiChNum, ref int npDoChNum );

        //DLL ���� �о����
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConGetMApiVersion( int nConNo, ref int npVer );

        //���� LED ON/OFF
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConSetCheckOn( int nConNo, int nOn );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConGetCheckOn( int nConNo, ref int npOn );

        //npModel = Board Model
        //0xAE	= AE 
        //0xAF1 = AF(Only DI)
        //0xAF2 = AF(Only DO)
        //0xAF3 = AF(DIO)
        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConGetModel( int nConNo, ref int npModel );

        [DllImport("tmcDApiAed_x64.dll")]
        public static extern int AIO_pmiConGetFwVersion( int nConNo, ref int npver );
    }
}

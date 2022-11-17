;NSIS Modern User Interface version 1.63
;MirInspector 4.5 패치 프로그램
;Written by Curtis Lee

!define COMPANY_NAME "UniEye" ;Define your own software name here
!define SETUP_PATH "UniScan" ;Define your own software name here
!define PRODUCT_NAME "Gravure Annexation Equipment (GAE)" ;Define your own software name here
!define SIMPLE_PRODUCT_NAME "GAE" ;Define your own software name here
!define PRODUCT_VERSION "2.1" ;Define your own software name here
!define BUILD_PATH "..\..\Build\Release" ;Define your own software version here
!define SHARED_PATH1 "..\..\..\Shared\ReferenceDll" ;Define your own software version her
!define SHARED_PATH2 "..\..\..\Shared\DependenctDll" ;Define your own software version her
!define CONFIG_PATH "..\..\Runtime\Config" ;Define your own software version here
!define RESULT_PATH "..\..\Runtime\Result" ;Define your own software version here

!define MUI_INSTALL_KEY "Software\${COMPANY_NAME}\${SIMPLE_PRODUCT_NAME}\Install" ; 인스톨 정보가 저장되는 레지스트리 위치

!include "MUI.nsh"
!include "Sections.nsh"
!include "nsDialogs.nsh"

;--------------------------------
;Configuration

  Name "${PRODUCT_NAME}"

  InstallDirRegKey HKCU ${MUI_INSTALL_KEY} "D:\${SETUP_PATH}"
  InstallDir "D:\${SETUP_PATH}"
  
  ;General
  OutFile "..\Setup\Setup_V${PRODUCT_VERSION}.exe"

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU"
  !define MUI_LANGDLL_REGISTRY_KEY "Software\${COMPANY_NAME}\${SIMPLE_PRODUCT_NAME}"
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  

  ;!define MUI_SKIN "Windows XP"
  ;!define MUI_SKIN "Orange"
  ;!define MUI_DISABLEBG
  ;!define MUI_BGGRADIENT false
;--------------------------------
;Modern UI Configuration
	;!insertmacro MUI_LANGUAGE
	!insertmacro MUI_PAGE_WELCOME
	!insertmacro MUI_PAGE_DIRECTORY
	!insertmacro MUI_PAGE_COMPONENTS
	Page custom AdditionalTasks AdditionalTasksLeave
	!insertmacro MUI_PAGE_INSTFILES
	;!insertmacro MUI_PAGE_FINISH
	
	!define MUI_ABORTWARNING
	!define MUI_UNCONFIRMPAGE
	
;!define MUI_FINISHPAGE_SHOWREADME_FUNCTION finishpageaction
;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "Korean"
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Language Strings

  ;Description
  LangString DESC_UNIEYES ${LANG_KOREAN} "${PRODUCT_NAME} 를 설치합니다."
  LangString DESC_UNIEYES ${LANG_ENGLISH} "Install ${PRODUCT_NAME}"

;--------------------------------
;Data

  BrandingText /TRIMRIGHT "${COMPANY_NAME} ${PRODUCT_NAME} ${PRODUCT_VERSION}"

;--------------------------------
;Reserve Files

  ;Things that need to be extracted on first (keep these lines before any File command!)
  ;Only useful for BZIP2 compression
  ;!insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections
Var DEVICETYPE
Var BINFILE
Var LNKFILE

Var ExtractDevConfig
Var InstDevUtil
Var InstCommonUtil

Function AdditionalTasks
	Var /GLOBAL MUI_PAGE_CUSTOM
	Var /GLOBAL CheckboxExtractDevConfig
	Var /GLOBAL CheckboxInstDevUtil
	Var /GLOBAL CheckboxInstCommonUtil

	nsDialogs::Create /NOUNLOAD 1018
	Pop $MUI_PAGE_CUSTOM

	${If} $MUI_PAGE_CUSTOM = error
		Abort
	${EndIf} 

	${NSD_CreateCheckBox} 5 40 100% 20 $2
	Pop $CheckboxExtractDevConfig

	${NSD_CreateCheckBox} 5 70 100% 20 $3
	Pop $CheckboxInstDevUtil

	${NSD_CreateCheckBox} 5 100 100% 20 $4
	Pop $CheckboxInstCommonUtil
	
	nsDialogs::Show
FunctionEnd

Function AdditionalTasksLeave
${NSD_GetState} $CheckboxExtractDevConfig $ExtractDevConfig
${NSD_GetState} $CheckboxInstDevUtil $InstDevUtil
${NSD_GetState} $CheckboxInstCommonUtil $InstCommonUtil

FunctionEnd

Function UnzipDeviceConfigFile
	${If} $ExtractDevConfig  == ${BST_CHECKED}
		DetailPrint "Extract Device Config File(s)..."
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\7z.exe"
		File "..\7z.dll"
		nsExec::exec '7z.exe e *.7z -aoa'
		Delete "7z.exe"
		Delete "7z.dll"
${EndIf}
FunctionEnd

Function  SetupDevice 
	${If} $InstCommonUtil  == ${BST_CHECKED}
		Call InstCommon
	${EndIf}
	
	; 기존 파일 백업
	RMDir $INSTDIR\$DEVICETYPE\Bin_backup
	CreateDirectory $INSTDIR\$DEVICETYPE\Bin_backup
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UnieyeLauncher.exe" "$INSTDIR\$DEVICETYPE\Bin_backup\UnieyeLauncher.exe"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Device.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Device.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Data.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Data.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\DynMvp.Vision.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\DynMvp.Vision.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniEye.Base.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniEye.Base.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\UniScanM.dll" "$INSTDIR\$DEVICETYPE\Bin_backup\UniScanM.dll"
	CopyFiles "$INSTDIR\$DEVICETYPE\Bin\$BINFILE" "$INSTDIR\RVMS\Bin_backup\$BINFILE"
	
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		ExecWait "$EXEDIR\Setup\Bin\SharedDLL.exe -y -o$INSTDIR\$DEVICETYPE\Bin\"
	;SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	;	File "${SHARED_PATH1}\*.dll"
	;	File "${SHARED_PATH2}\*.dll"

	; UniScanM 프레임워크 복사
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	;	FILE "${BUILD_PATH}\UnieyeLauncher.exe"
	;	FILE "${BUILD_PATH}\UserManager.exe"
	;	FILE "${BUILD_PATH}\DynMvp.dll"
	;	FILE "${BUILD_PATH}\DynMvp.Device.dll"
	;	FILE "${BUILD_PATH}\DynMvp.Data.dll"
	;	FILE "${BUILD_PATH}\DynMvp.Vision.dll"
	;	FILE "${BUILD_PATH}\UniEye.Base.dll"
	;	FILE "${BUILD_PATH}\UniScanM.dll"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\*.dll" "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UserManager.exe" "$INSTDIR\$DEVICETYPE\Bin\UserManager.exe"
	
	; Copy Config files
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
	;	FILE "${CONFIG_PATH}\log4net.xml"
	;	FILE "${CONFIG_PATH}\StringTable_ko-kr.xml"
	;	FILE "${CONFIG_PATH}\StringTable_zh-cn.xml"
	;	FILE "${CONFIG_PATH}\Unieye.png"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\log4net.xml" "$INSTDIR\$DEVICETYPE\Config\log4net.xml"
		CopyFiles "$EXEDIR\Setup\Config\StringTable_ko-kr.xml" "$INSTDIR\$DEVICETYPE\Config\StringTable_ko-kr.xml"
		CopyFiles "$EXEDIR\Setup\Config\StringTable_zh-cn.xml" "$INSTDIR\$DEVICETYPE\Config\StringTable_zh-cn.xml"
		CopyFiles "$EXEDIR\Setup\Config\Unieye.png" "$INSTDIR\$DEVICETYPE\Config\Unieye.png"
		
	; 바로가기 만들기
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	CreateShortCut "$DESKTOP\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"
	CreateShortCut "$DESKTOP\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\Bin\UnieyeLauncher.exe"
		
	CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\$LNKFILE" "$INSTDIR\$DEVICETYPE\Bin\$BINFILE"	
	CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\UnieyeLauncher.lnk" "$INSTDIR\$DEVICETYPE\Bin\UnieyeLauncher.exe"
	
	CreateShortCut “$APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\UnieyeLauncher.lnk” "$INSTDIR\$DEVICETYPE\Bin\UnieyeLauncher.exe"
	
	; 공유폴더 설정
	ExecWait '"cmd.exe" /C net share UniScan=$INSTDIR /grant:Everyone,full'
	ExecWait '"cmd.exe" /C ICACLS $INSTDIR /grant Everyone:F /t'

	; administrator 계정 활성화
	ExecWait '"cmd.exe" /C net user administrator /active:yes'
	
	; 콘솔 로그온 시 로컬 계정에서 빈 암호 사용 제한 '아니오'
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Lsa" "limitblankpassworduse" 0	
	
	; 전원옵션 '빠른시작켜기' 해제
	WriteRegDWORD "HKLM" "SYSTEM\CurrentControlSet\Control\Session Manager\Power" "HiberbootEnabled" 0	
	
FunctionEnd

Section /o "RVMS" SecRVMS
	StrCpy $DEVICETYPE "RVMS"
	StrCpy $BINFILE "UniScanM.RVMS.exe"
	StrCpy $LNKFILE "RVMS.lnk"

	Call SetupDevice
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Bin"
	;	FILE "${BUILD_PATH}\UniScanM.RVMS.exe"
	;	FILE /oname=UniEyeLauncher.xml  ${BUILD_PATH}\UniEyeLauncher_RVMS.xml
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.RVMS.exe" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.RVMS.exe"
		CopyFiles "$EXEDIR\Setup\Bin\UniEyeLauncher_RVMS.xml" "$INSTDIR\$DEVICETYPE\Bin\UniEyeLauncher.xml"

	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\Config_RVMS.7z" "$INSTDIR\$DEVICETYPE\Config\"
		Call UnzipDeviceConfigFile
	
	SetOutPath "$INSTDIR\$DEVICETYPE\Result"
		FILE "${RESULT_PATH}\RawDataTemplate_RVMS.xlsx"
			
			; NTP 서버 레지스트리 수정
	${If} $InstDevUtil  == ${BST_CHECKED}		
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time\Config" "AnnounceFlags" 5
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer" "Enabled" 1
		
		; NTP 서비스 시작
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\w32Time\w32Time_Server.bat"
		ExecWait "w32Time_Server.bat"
		
		; NTP 방화벽 예외 등록
		ExecWait '"cmd.exe" /C netsh advfirewall firewall add rule name="NTP" dir=in action=allow protocol=UDP localPort=123'
		
	${EndIf}
SectionEnd

Section /o "Pinhole" SecPinhole
	StrCpy $DEVICETYPE "Pinhole"
	StrCpy $BINFILE "UniScanM.Pinhole.exe"
	StrCpy $LNKFILE "Pinhole.lnk"	
		
	Call SetupDevice 
		
	SetOutPath "$INSTDIR\Pinhole\Bin"
	;	FILE "${BUILD_PATH}\UniScanM.Pinhole.exe"
	;	FILE /oname=UniEyeLauncher.xml  "${BUILD_PATH}\UniEyeLauncher_Pinhole.xml"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.Pinhole.exe" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.Pinhole.exe"
		CopyFiles "$EXEDIR\Setup\Bin\UniEyeLauncher_Pinhole.xml" "$INSTDIR\$DEVICETYPE\Bin\UniEyeLauncher.xml"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\Config_Pinhole.7z" "$INSTDIR\$DEVICETYPE\Config\"
		Call UnzipDeviceConfigFile

	SetOutPath "$INSTDIR\Pinhole\Result"
		FILE "${RESULT_PATH}\RawDataTemplate_PinHole.xlsx"

	${If} $InstDevUtil  == ${BST_CHECKED}
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
		
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat"
		
		Call InstPylon	
		Call InstDigitalPro
		Call InstVCRedist
		Call InstMIL
	${EndIf}
SectionEnd

Section /o "ColorSens" SecColor
	StrCpy $DEVICETYPE "ColorSensor"
	StrCpy $BINFILE "UniScanM.ColorSens.exe"
	StrCpy $LNKFILE "ColorSensor.lnk"	

	Call SetupDevice 
	
	SetOutPath "$INSTDIR\ColorSensor\Bin"
	;	FILE "${BUILD_PATH}\UniScanM.ColorSens.exe"
	;	FILE /oname=UniEyeLauncher.xml  "${BUILD_PATH}\UniEyeLauncher_ColorSensor.xml"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.ColorSens.exe" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.ColorSens.exe"
		CopyFiles "$EXEDIR\Setup\Bin\UniEyeLauncher_ColorSensor.xml" "$INSTDIR\$DEVICETYPE\Bin\UniEyeLauncher.xml"

		
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\Config_Color.7z" "$INSTDIR\$DEVICETYPE\Config\"
		Call UnzipDeviceConfigFile
		
	SetOutPath "$INSTDIR\ColorSensor\Result"
		FILE "${RESULT_PATH}\RawDataTemplate_ColorSensor.xlsx"

	${If} $InstDevUtil  == ${BST_CHECKED}
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
		
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat"
		
		Call InstPylon	
	${EndIf}
SectionEnd

Section /o "EDMS" SecEDMS
	StrCpy $DEVICETYPE "EDMS"
	StrCpy $BINFILE "UniScanM.EDMS.exe"
	StrCpy $LNKFILE "EDMS.lnk"

	Call SetupDevice 
	
	SetOutPath "$INSTDIR\EDMS\Bin"
	;	FILE "${BUILD_PATH}\UniScanM.EDMS.exe"
	;	FILE "${BUILD_PATH}\UniScanM.EDMS.exe.config"
	;	FILE /oname=UniEyeLauncher.xml  "${BUILD_PATH}\UniEyeLauncher_EDMS.xml"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.EDMS.exe" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.EDMS.exe"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.EDMS.exe.config" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.EDMS.exe.config"
		CopyFiles "$EXEDIR\Setup\Bin\UniEyeLauncher_EDMS.xml" "$INSTDIR\$DEVICETYPE\Bin\UniEyeLauncher.xml"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\Config_EDMS.7z" "$INSTDIR\$DEVICETYPE\Config\"
		Call UnzipDeviceConfigFile

	SetOutPath "$INSTDIR\EDMS\Result"
		FILE "${RESULT_PATH}\RawDataTemplate_EDMS.xlsx"
	
	${If} $InstDevUtil  == ${BST_CHECKED}
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
		
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat"

		Call InstCoaxLink
	${EndIf}
SectionEnd

Section /o "StopImage" SecSI
	StrCpy $DEVICETYPE "StillImage"
	StrCpy $BINFILE "UniScanM.StillImage.exe"
	StrCpy $LNKFILE "StillImage.lnk"
	Call SetupDevice
	
	SetOutPath "$INSTDIR\StillImage\Bin"
	;	FILE "${BUILD_PATH}\UniScanM.StillImage.exe"
	;	FILE "${BUILD_PATH}\UniScanM.StillImage.exe.config"
	;	FILE /oname=UniEyeLauncher.xml  "${BUILD_PATH}\UniEyeLauncher_StillImage.xml"
	CreateDirectory "$INSTDIR\$DEVICETYPE\Bin"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.StillImage.exe" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.StillImage.exe"
		CopyFiles "$EXEDIR\Setup\Bin\UniScanM.StillImage.exe.config" "$INSTDIR\$DEVICETYPE\Bin\UniScanM.StillImage.exe.config"
		CopyFiles "$EXEDIR\Setup\Bin\UniEyeLauncher_StillImage.xml" "$INSTDIR\$DEVICETYPE\Bin\UniEyeLauncher.xml"
		
	SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		CopyFiles "$EXEDIR\Setup\Config\Config_Stillimage.7z" "$INSTDIR\$DEVICETYPE\Config\"
		Call UnzipDeviceConfigFile
		
	SetOutPath "$INSTDIR\StillImage\Result"
		FILE "${RESULT_PATH}\RawDataTemplate_StillImage.xlsx"
		
	${If} $InstDevUtil  == ${BST_CHECKED}
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "DelayedAutostart" 0
		WriteRegDWORD HKLM "SYSTEM\CurrentControlSet\Services\W32Time" "Start" 2
		
		SetOutPath "$INSTDIR\$DEVICETYPE\Config"
		File "..\w32Time\w32Time_Client.bat"
		ExecWait "w32Time_Client.bat"

		Call InstCoaxLink
		Call InstMotionComposer	
		Call InstVCRedist
		Call InstFujiA5
	${EndIf}
SectionEnd

Function InstMIL
		DetailPrint "Install MIL10..."
		ExecWait "$EXEDIR\Setup\MIL10\MIL64Setup.exe"	
FunctionEnd

Function InstPylon
	DetailPrint "Install Pylon 5.0..."
	ExecWait "$EXEDIR\Setup\Pylon5\Basler_pylon_5.0.5.8999.exe /s"
FunctionEnd

Function InstCoaxLink
	DetailPrint "Install CoaxLink 9.5..."
	ExecWait "$EXEDIR\Setup\CoaxLink9\coaxlink-win10-9.5.2.131.exe /s"
FunctionEnd

Function InstFujiA5
	DetailPrint "Install FujiA5 3.5..."
	ExecWait "$EXEDIR\Setup\Fuji_A5_V3.5\setup.exe /s /v/qn"
FunctionEnd

Function InstMotionComposer
		DetailPrint "Install MotionComposer 16.0..."
		ExecWait "$EXEDIR\Setup\MotionComposer16\MotionComposerSoftware_V16.0.0.0002_win64.exe /s"
FunctionEnd

Function InstDigitalPro
		DetailPrint "Install DigitalPro 16..."
		ExecWait "$EXEDIR\Setup\DigitalPro16\DigitalProSoftware_V16.0.0.0002_win64.exe /s"
		
FunctionEnd

Function InstVCRedist
		DetailPrint "Install VCRedist..."
		ExecWait "$EXEDIR\Setup\VCRedist\vcredist_x64 /q"
FunctionEnd

Function InstCommon
	SetOutPath "$INSTDIR\Utility\"
	CreateDirectory $SMPROGRAMS\${SIMPLE_PRODUCT_NAME}
	
	DetailPrint "Install Etc. Utility"
	CreateDirectory "$INSTDIR\Utility"
		CopyFiles "$EXEDIR\Setup\Common\LED-JCON-VIT.exe" "$INSTDIR\Utility\LED-JCON-VIT.exe"
		CreateShortCut "$DESKTOP\LED-JCON-VIT.lnk" "$INSTDIR\Utility\LED-JCON-VIT.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\LED-JCON-VIT.lnk" "$INSTDIR\Utility\LED-JCON-VIT.exe"
		CopyFiles "$EXEDIR\Setup\Common\putty.exe" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$DESKTOP\putty.lnk" "$INSTDIR\Utility\putty.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\putty.lnk" "$INSTDIR\Utility\putty.exe"
		ExecWait "$EXEDIR\Setup\Common\ij149-jre8-64.exe -y -o$INSTDIR\Utility\"
		CreateShortCut "$DESKTOP\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		CreateShortCut "$SMPROGRAMS\${SIMPLE_PRODUCT_NAME}\ImageJ.lnk" "$INSTDIR\Utility\ij149-jre8-64\ImageJ\ImageJ.exe"
		
	DetailPrint "Install 7z..."	
		ExecWait "$EXEDIR\Setup\Common\7z1805-x64.msi /passive"
	
	DetailPrint "Install npp 7.5..."
		ExecWait "$EXEDIR\Setup\Common\npp.7.5.4.Installer.x64.exe /s"
	
	DetailPrint "Install RealVNC 4.6..."
		ExecWait "$EXEDIR\Setup\Common\vnc-E4_6_3-x86_x64_win32.exe /s"
	
FunctionEnd

Function GetParams
	Push $R0
	Push $R1
	Push $R2
	Push $R3

	StrCpy $R2 1
	StrLen $R3 $CMDLINE

	;Check for quote or space
	StrCpy $R0 $CMDLINE $R2
	StrCmp $R0 '"' 0 +3
		StrCpy $R1 '"'
		Goto loop
	StrCpy $R1 " "

	loop:
		IntOp $R2 $R2 + 1
		StrCpy $R0 $CMDLINE 1 $R2
		StrCmp $R0 $R1 get
		StrCmp $R2 $R3 get
		Goto loop
   
	get:
		IntOp $R2 $R2 + 1
		StrCpy $R0 $CMDLINE 1 $R2
		StrCmp $R0 " " get
		StrCpy $R0 $CMDLINE "" $R2
   
	Pop $R3
	Pop $R2
	Pop $R1
	Exch $R0
FunctionEnd

Function .onInit
	;ReadRegStr $R0 HKCU ${MUI_INSTALL_KEY} "Path"
	;StrCmp $R0 "" +2
	;StrCpy $R0 "D:\${SETUP_PATH}\"
	;WriteRegStr HKCU ${MUI_INSTALL_KEY} "Path" $R0

	!insertmacro MUI_LANGDLL_DISPLAY
	StrCpy $1 "Setup Device"
	StrCpy $2 "초기 설정 복사"
	StrCpy $3 "종속 유틸리티 설치"
	StrCpy $4 "공통 유틸리티 설치"
FunctionEnd

Function .onSelChange
	!insertmacro StartRadioButtons $1
		!insertmacro RadioButton ${SecRVMS}
		!insertmacro RadioButton ${SecPinhole}
		!insertmacro RadioButton ${SecColor}
		!insertmacro RadioButton ${SecEDMS}
		!insertmacro RadioButton ${SecSI}
	!insertmacro EndRadioButtons
FunctionEnd

Function .onInstSuccess
FunctionEnd
;--------------------------------
;Descriptions

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SecRVMS} ""
	!insertmacro MUI_DESCRIPTION_TEXT ${SecPinhole} "MIL10, Pylon5, DigitalPro16"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecColor} "Pylon5"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecEDMS} "CoaxLink10"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecSI} "CoaxLink10, MotionComposer16"
!insertmacro MUI_FUNCTION_DESCRIPTION_END


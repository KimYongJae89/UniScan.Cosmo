@echo off
cls
if "%1"=="/d" (
	@echo on
)
cd /d %~dp0

echo Debug mode: use /d flag
echo 0:All, 1:RVMS, 2:Pinhole, 4:ColorSensor, 8:EDMS, 16:StillImage
echo ex) 1(RVMS) + 8(EDMS) = 9(RVMS and EDMS)
set /p code=Code? 

set /a rvmsCode="(%code%>>0)&1"
set /a pinholeCode="(%code%>>1)&1"
set /a colorSensorCode="(%code%>>2)&1"
set /a edmsCode="(%code%>>3)&1"
set /a stillImageCode="(%code%>>4)&1"
if "%code%"=="0" (
	set rvmsCode=1
	set pinholeCode=1
	set colorSensorCode=1
	set edmsCode=1
	set stillImageCode=1
)

echo rvmsCode: %rvmsCode%
echo pinholeCode: %pinholeCode%
echo colorSensorCode: %colorSensorCode%
echo edmsCode: %edmsCode%
echo stillImageCode: %stillImageCode%

if "%rvmsCode%"=="1" (
	echo.
	echo Copy RVMS
	call :Patch "\\192.168.1.100\UniScan\RVMS" "UniScanM.RVMS.exe" "RawDataTemplate_RVMS.xlsx" "UniEyeLauncher_RVMS.xml"
	echo Copy RVMS Done
)

if "%pinholeCode%"=="1" (
	echo.
	echo Copy Pinhole
	call :Patch "\\192.168.1.102\UniScan\Pinhole" "UniScanM.Pinhole.exe" "RawDataTemplate_PinHole.xlsx" "UniEyeLauncher_Pinhole.xml"
	echo Copy Pinhole Done
)

if "%colorSensorCode%"=="1" (
	echo.
	echo Copy ColorSensor
	call :Patch "\\192.168.1.103\UniScan\ColorSensor" "UniScanM.ColorSens.exe" "RawDataTemplate_ColorSensor.xlsx" "UniEyeLauncher_ColorSensor.xml"
	echo Copy ColorSensor Done
)

if "%edmsCode%"=="1" (
	echo.
	echo Copy EDMS
	call :Patch "\\192.168.1.104\UniScan\EDMS" "UniScanM.EDMS.exe" "RawDataTemplate_EDMS.xlsx" "UniEyeLauncher_EDMS.xml"
	echo Copy EDMS Done
)

if "%stillImageCode%"=="1" (
	echo.
	echo Copy StillImage
	call :Patch "\\192.168.1.105\UniScan\StillImage" "UniScanM.StillImage.exe" "RawDataTemplate_StillImage.xlsx" "UniEyeLauncher_StillImage.xml"
	echo Copy StillImage Done
)

pause
goto :EOF

:Patch
echo TargetPath: %1
echo Binary: %2
echo Template: %3
echo Launcher: %4

net use %1 "" /user:administrator
copy ..\Runtime\Config\StringTable_ko-kr.xml %1\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml %1\Config\
copy ..\Runtime\Config\log4net.xml %1\Config\

copy ..\Runtime\Result\%3 %1\Result\

copy Release\UnieyeLauncher.exe %1\bin
copy Release\%4 %1\bin\UniEyeLauncher.xml
copy Release\UserManager.exe %1\bin

copy Release\DynMvp.dll %1\Update
copy Release\DynMvp.Data.dll %1\Update
copy Release\DynMvp.Device.dll %1\Update
copy Release\DynMvp.Vision.dll %1\Update
copy Release\UniEye.Base.dll %1\Update
copy Release\UniScanM.dll %1\Update

copy Release\%2 %1\Update
net use %1 /delete
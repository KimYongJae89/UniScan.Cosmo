mkdir .\UniScanM\Build\Release
copy ..\Build\Release\DynMvp.dll .\UniScanM\Build\Release\
copy ..\Build\Release\DynMvp.Data.dll .\UniScanM\Build\Release\
copy ..\Build\Release\DynMvp.Device.dll .\UniScanM\Build\Release\
copy ..\Build\Release\DynMvp.Vision.dll .\UniScanM\Build\Release\

copy ..\Build\Release\UniEye.Base.dll .\UniScanM\Build\Release\
copy ..\Build\Release\UniScanM.dll .\UniScanM\Build\Release\

copy ..\Build\Release\UniScanM.Pinhole.exe .\UniScanM\Build\Release\
copy ..\Build\Release\UniScanM.StillImage.exe .\UniScanM\Build\Release\
copy ..\Build\Release\UniScanM.ColorSens.exe .\UniScanM\Build\Release\
copy ..\Build\Release\UniScanM.EDMS.exe .\UniScanM\Build\Release\
copy ..\Build\Release\UniScanM.RVMS.exe .\UniScanM\Build\Release\

copy ..\Build\Release\UnieyeLauncher.exe .\UniScanM\Build\Release\
copy ..\Build\Release\UserManager.exe .\UniScanM\Build\Release\

mkdir .\UniScanM\Runtime\Config
copy ..\Runtime\Config\StringTable_ko-kr.xml .\UniScanM\Runtime\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml .\UniScanM\Runtime\Config\
copy ..\Runtime\Config\log4net.xml .\UniScanM\Runtime\Config\

mkdir .\UniScanM\Runtime\Result
copy ..\Runtime\Result\RawDataTemplate_ColorSensor.xlsx .\UniScanM\Runtime\Result\
copy ..\Runtime\Result\RawDataTemplate_EDMS.xlsx .\UniScanM\Runtime\Result\
copy ..\Runtime\Result\RawDataTemplate_PinHole.xlsx .\UniScanM\Runtime\Result\
copy ..\Runtime\Result\RawDataTemplate_RVMS.xlsx .\UniScanM\Runtime\Result\
copy ..\Runtime\Result\RawDataTemplate_StillImage.xlsx .\UniScanM\Runtime\Result\

copy ..\Build\Copy_UniScanM.bat .\UniScanM\Build\

pause
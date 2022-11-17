mkdir .\UniScanWPF\Build\Release
copy ..\Build\Release\DynMvp.dll .\UniScanWPF\Build\Release\
copy ..\Build\Release\DynMvp.Data.dll .\UniScanWPF\Build\Release\
copy ..\Build\Release\DynMvp.Device.dll .\UniScanWPF\Build\Release\
copy ..\Build\Release\DynMvp.Vision.dll .\UniScanWPF\Build\Release\
copy ..\Build\Release\UniEye.Base.dll .\UniScanWPF\Build\Release\

copy ..\Build\Release\WpfControlLibrary.dll .\UniScanWPF\Build\Release\
copy ..\Build\Release\UniScanWPF.Table.exe .\UniScanWPF\Build\Release\

copy ..\Build\Release\UnieyeLauncher.exe .\UniScanWPF\Build\Release\

mkdir .\UniScanWPF\Runtime\Config
copy ..\Runtime\Config\StringTable_ko-kr.xml .\UniScanWPF\Runtime\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml .\UniScanWPF\Runtime\Config\

mkdir .\UniScanWPF\Runtime\Result
copy ..\Runtime\Result\RawDataTemplate_Offline.xlsx .\UniScanWPF\Runtime\Result\

copy ..\Build\Copy_UniScanWPF.bat .\UniScanWPF\Build\

pause
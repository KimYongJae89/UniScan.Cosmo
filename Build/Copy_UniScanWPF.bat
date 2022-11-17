cd /d %~dp0

set targetPath=D:\UniScan
set templateFile=RawDataTemplate_Offline.xlsx
copy .\Release\*.dll %targetPath%\Update
copy ..\Runtime\Config\StringTable_ko-kr.xml %targetPath%\Config\
copy ..\Runtime\Config\StringTable_zh-cn.xml %targetPath%\Config\
copy ..\Runtime\Result\%templateFile% %targetPath%\Result\
copy .\Release\UnieyeLauncher.exe %targetPath%\Bin
copy .\Release\UniScanWPF.Table.exe %targetPath%\Update

echo %targetPath% Copy Done
pause
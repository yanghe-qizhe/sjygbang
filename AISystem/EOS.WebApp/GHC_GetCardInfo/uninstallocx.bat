@echo off

regsvr32 /u "%~dp0GHC_GetCardInfo.ocx"

echo "Press Any Key Continue..."
pause
exit
@echo off

set Version=1.3.3

echo %Version%

dotnet new -u XUCore.Template.FreeSql

dotnet new --install XUCore.Template.FreeSql::%Version%

pause


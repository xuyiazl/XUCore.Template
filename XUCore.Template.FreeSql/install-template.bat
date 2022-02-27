@echo off

set Version=6.1.0

echo %Version%

dotnet new -u XUCore.Template.FreeSql

dotnet new --install XUCore.Template.FreeSql::%Version%

pause


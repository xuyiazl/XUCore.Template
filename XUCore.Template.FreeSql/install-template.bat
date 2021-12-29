@echo off

set Version=6.0.8

echo %Version%

dotnet new -u XUCore.Template.FreeSql

dotnet new --install XUCore.Template.FreeSql::%Version%

pause


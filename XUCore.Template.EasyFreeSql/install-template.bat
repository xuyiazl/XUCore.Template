@echo off

set Version=0.0.1

echo %Version%

dotnet new -u XUCore.Template.EasyFreeSql

dotnet new --install XUCore.Template.EasyFreeSql::%Version%

pause


@echo off

set Version=1.3.1

echo %Version%

dotnet new -u XUCore.Template.EasyFreeSql

dotnet new --install XUCore.Template.EasyFreeSql::%Version%

pause


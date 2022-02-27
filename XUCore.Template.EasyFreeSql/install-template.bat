@echo off

set Version=6.1.0

echo %Version%

dotnet new -u XUCore.Template.EasyFreeSql

dotnet new --install XUCore.Template.EasyFreeSql::%Version%

pause


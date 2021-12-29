@echo off

set Version=6.0.8

echo %Version%

dotnet new -u XUCore.Template.EasyFreeSql

dotnet new --install XUCore.Template.EasyFreeSql::%Version%

pause


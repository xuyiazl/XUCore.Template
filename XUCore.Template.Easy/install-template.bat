@echo off

set Version=1.2.0

echo %Version%

dotnet new -u XUCore.Template.Easy

dotnet new --install XUCore.Template.Easy::%Version%

pause


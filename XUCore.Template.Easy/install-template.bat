@echo off

set Version=1.1.9

echo %Version%

dotnet new -u XUCore.Template.Easy

dotnet new --install XUCore.Template.Easy::%Version%

pause


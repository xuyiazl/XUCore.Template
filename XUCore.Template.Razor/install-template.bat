@echo off

set Version=6.0.0

echo %Version%

dotnet new -u XUCore.Template.Razor

dotnet new --install XUCore.Template.Razor::%Version%

pause

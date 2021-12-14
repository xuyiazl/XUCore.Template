@echo off

set Version=6.0.2

echo %Version%

dotnet new -u XUCore.Template.Razor2

dotnet new --install XUCore.Template.Razor2::%Version%

pause


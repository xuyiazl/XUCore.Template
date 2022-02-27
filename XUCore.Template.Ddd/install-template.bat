@echo off

set Version=6.1.0

echo %Version%

dotnet new -u XUCore.Template.Ddd

dotnet new --install XUCore.Template.Ddd::%Version%

pause


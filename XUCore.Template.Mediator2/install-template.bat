@echo off

set Version=6.0.4

echo %Version%

dotnet new -u XUCore.Template.Mediator2

dotnet new --install XUCore.Template.Mediator2::%Version%

pause


@echo off

set Version=6.0.4

echo %Version%

dotnet new -u XUCore.Template.Mediator

dotnet new --install XUCore.Template.Mediator::%Version%

pause


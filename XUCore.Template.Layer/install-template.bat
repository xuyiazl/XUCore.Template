@echo off

set Version=1.3.0

echo %Version%

dotnet new -u XUCore.Template.Layer

dotnet new --install XUCore.Template.Layer::%Version%

pause


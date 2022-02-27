@echo off

set Version=6.1.0

echo %Version%

dotnet new -u XUCore.Template.WeChat

dotnet new --install XUCore.Template.WeChat::%Version%

pause


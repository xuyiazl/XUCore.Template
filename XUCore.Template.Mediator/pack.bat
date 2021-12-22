@echo off

set version=6.0.1

echo version=%version%

nuget pack XUCore.Template.Mediator.nuspec -NoDefaultExcludes -OutputDirectory .

pause


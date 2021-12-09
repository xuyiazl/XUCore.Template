@echo off

set version=6.0.4

echo version=%version%

nuget pack XUCore.Template.Razor.nuspec -NoDefaultExcludes -OutputDirectory .

pause


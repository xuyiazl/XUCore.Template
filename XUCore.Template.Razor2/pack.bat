@echo off

set version=6.0.1

echo version=%version%

nuget pack XUCore.Template.Razor2.nuspec -NoDefaultExcludes -OutputDirectory .

pause


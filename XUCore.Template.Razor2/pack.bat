@echo off

set version=6.0.0

echo version=%version%

nuget pack XUCore.Template.Razor2.nuspec -NoDefaultExcludes -OutputDirectory .

pause


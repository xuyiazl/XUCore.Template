@echo off

set version=6.0.6

echo version=%version%

nuget pack XUCore.Template.Ddd.nuspec -NoDefaultExcludes -OutputDirectory .

pause


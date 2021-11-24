@echo off

set version=6.0.5

echo version=%version%

nuget pack XUCore.Template.FreeSql.nuspec -NoDefaultExcludes -OutputDirectory .

pause


@echo off

set version=6.0.5

echo version=%version%

nuget pack XUCore.Template.EasyFreeSql.nuspec -NoDefaultExcludes -OutputDirectory .

pause


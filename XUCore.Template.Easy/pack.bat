@echo off

set version=1.2.0

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.Easy

nuget pack XUCore.Template.Easy.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause


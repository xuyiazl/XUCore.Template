@echo off

set version=1.3.2

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.Layer

nuget pack XUCore.Template.Layer.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause


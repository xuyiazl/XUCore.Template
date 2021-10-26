@echo off

set version=1.3.1

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.EasyFreeSql

nuget pack XUCore.Template.EasyFreeSql.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause


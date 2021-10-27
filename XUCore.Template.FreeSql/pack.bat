@echo off

set version=1.3.2

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.FreeSql

nuget pack XUCore.Template.FreeSql.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause


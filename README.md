

## 🍄 框架脚手架

|																																		| 名称								| 下载																																		| 版本																																								| 描述										|
| ------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Ddd)				| XUCore.Template.Ddd				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Ddd.svg)](https://nuget.org/packages/XUCore.Template.Ddd)					| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Ddd.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Ddd)						| Ddd 架构模板（Mvc/Api,底层相通）				|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| XUCore.Template.FreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.FreeSql.svg)](https://nuget.org/packages/XUCore.Template.FreeSql)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.FreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| 基于FreeSql的 Api分层应用模板(默认WebApi,底层相通)					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.EasyFreeSql)				| XUCore.Template.EasyFreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.EasyFreeSql.svg)](https://nuget.org/packages/XUCore.Template.EasyFreeSql)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.EasyFreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.EasyFreeSql)				| 基于FreeSql的 Api 单层应用模板(默认WebApi,底层相通)					|





## 如何使用脚手架 

所有脚手架在模板中默认形态是支持WebApi（因为目前都是前后端分离），但是底层是相通的，所以只需自行创建对应的web工程即可。

### [XUCore.Template.Ddd](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Ddd)

基于Ddd模型架构，使用EFCore

### [XUCore.Template.FreeSql](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.FreeSql)

建议使用，基于FreeSql的简单分层应用（动态API+业务+FreeSql的数据层），缩减了Controller，独立业务，数据持久化（FreeSql的性能上优于EFCore，但EFCore毕竟是官方出品）


### [XUCore.Template.EasyFreeSql](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.EasyFreeSql)

建议使用，基于FreeSql的单层应用（动态API+业务+FreeSql的结合），缩减了开发时间

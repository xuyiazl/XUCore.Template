

## 🍄 框架脚手架

|																																		| 名称								| 下载																																		| 版本																																								| 描述										|
| ------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Ddd)				| XUCore.Template.Ddd				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Ddd.svg)](https://nuget.org/packages/XUCore.Template.Ddd)					| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Ddd.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Ddd)						| Ddd 架构模板（Mvc/WebApi）				|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Layer)			| XUCore.Template.Layer				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Layer.svg)](https://nuget.org/packages/XUCore.Template.Layer)				| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Layer.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Layer)					| Layer 三层快速模板 (WebApi)						|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.EasyLayer)		| XUCore.Template.EasyLayer			| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.EasyLayer.svg)](https://nuget.org/packages/XUCore.Template.EasyLayer)				| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.EasyLayer.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.EasyLayer)					| EasyLayer 精简分层模板 (WebApi)						|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Easy)				| XUCore.Template.Easy				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Easy.svg)](https://nuget.org/packages/XUCore.Template.Easy)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Easy.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Easy)				| Easy Api 单层应用模板（WebApi）					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| XUCore.Template.FreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.FreeSql.svg)](https://nuget.org/packages/XUCore.Template.FreeSql)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.FreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| 基于FreeSql的 Api 分层应用模板（WebApi）					|





## 如何使用脚手架 

### [XUCore.Template.Ddd](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Ddd)

### [XUCore.Template.Layer](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Layer)

建议使用，基于EFCore的简单分层应用（传统Controller+业务+EFCore的数据层），没有使用动态Controller，独立业务，数据持久化

### [XUCore.Template.EasyLayer](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.EasyLayer) 

建议使用，基于EFCore的简单分层应用（动态API+业务+EFCore的数据层），缩减了Controller，独立业务，数据持久化

### [XUCore.Template.Easy](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Easy) 

不太建议使用，毕竟单层（动态API+业务+数据）集合在一起。

### [XUCore.Template.FreeSql](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.FreeSql)

建议使用，基于FreeSql的简单分层应用（动态API+业务+FreeSql的数据层），缩减了Controller，独立业务，数据持久化（FreeSql的性能上优于EFCore，但EFCore毕竟是官方出品）



using AutoMapper;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using XUCore.Ddd.Domain.Filters;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.NetCore.MessagePack;
using XUCore.Template.EasyFreeSql.Applaction.Filters;
using XUCore.Template.EasyFreeSql.Core;

namespace XUCore.Template.EasyFreeSql.Applaction
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    [CommandValidation]
    [MessagePackResponseContentType]
    public class AppService<TEntity> : IAppService where TEntity : class
    {
        protected readonly FreeSqlUnitOfWorkManager muowm;
        protected readonly IBaseRepository<TEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;
        public AppService(IServiceProvider serviceProvider)
        {
            this.muowm = serviceProvider.GetService<FreeSqlUnitOfWorkManager>();
            this.repo = muowm.Orm.GetRepository<TEntity>();
            this.mapper = serviceProvider.GetService<IMapper>();
            this.user = serviceProvider.GetService<IUserInfo>();
        }
    }
}

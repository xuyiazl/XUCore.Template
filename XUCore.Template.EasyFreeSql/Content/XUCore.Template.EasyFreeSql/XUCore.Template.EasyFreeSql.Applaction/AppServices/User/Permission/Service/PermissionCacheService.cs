using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Permission
{
    public class PermissionCacheService : IPermissionCacheService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        public PermissionCacheService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
        }

        [AspectCache(HashKey = CacheKey.AuthUser, Key = "{0}", Seconds = CacheTime.Min5)]
        public async Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm
                   .Select<UserRoleEntity, RoleMenuEntity, MenuEntity>()
                   .LeftJoin((userRole, roleMenu, menu) => userRole.RoleId == roleMenu.RoleId)
                   .LeftJoin((userRole, roleMenu, menu) => roleMenu.MenuId == menu.Id)
                   .Where((userRole, roleMenu, menu) => userRole.UserId == userId)
                   .Distinct()
                   .ToListAsync((userRole, roleMenu, menu) => menu, cancellationToken);

            return res;
        }
    }
}

using XUCore.Template.Razor2.Core;
using XUCore.Template.Razor2.DbService.Auth.Menu;
using XUCore.Template.Razor2.Persistence.Entities.Auth;
using XUCore.Template.Razor2.Persistence.Entities.User;

namespace XUCore.Template.Razor2.DbService.Auth.Permission
{
    public class PermissionCacheService : FreeSqlCurdService<long, MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>,
        IPermissionCacheService
    {
        public PermissionCacheService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        [AspectCache(HashKey = CacheKey.AuthUser, Key = "{0}", Seconds = CacheTime.Min5)]
        public async Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken)
        {
            var res = await freeSql
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

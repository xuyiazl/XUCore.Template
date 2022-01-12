using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Entities.User;

namespace XUCore.Template.WeChat.DbService.Auth.Permission
{
    public class PermissionCacheService : IPermissionCacheService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<UserEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public PermissionCacheService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<UserEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
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

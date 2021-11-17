using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
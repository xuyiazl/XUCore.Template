using XUCore.Template.Razor2.Persistence.Entities.Auth;

namespace XUCore.Template.Razor2.DbService.Auth.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
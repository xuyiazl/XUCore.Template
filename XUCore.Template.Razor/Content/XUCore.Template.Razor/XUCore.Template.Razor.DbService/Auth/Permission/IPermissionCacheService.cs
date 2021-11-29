using XUCore.Template.Razor.Persistence.Entities.User;

namespace XUCore.Template.Razor.DbService.Auth.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
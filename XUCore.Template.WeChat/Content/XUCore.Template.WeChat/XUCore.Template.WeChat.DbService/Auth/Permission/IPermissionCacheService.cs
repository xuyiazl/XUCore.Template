using XUCore.Template.WeChat.Persistence.Entities.Auth;

namespace XUCore.Template.WeChat.DbService.Auth.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
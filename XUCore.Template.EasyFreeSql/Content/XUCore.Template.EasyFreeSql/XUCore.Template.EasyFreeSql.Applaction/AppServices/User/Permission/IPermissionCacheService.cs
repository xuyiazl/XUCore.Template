using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
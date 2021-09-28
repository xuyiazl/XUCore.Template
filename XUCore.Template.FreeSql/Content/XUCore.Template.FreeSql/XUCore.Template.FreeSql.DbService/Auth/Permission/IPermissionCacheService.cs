using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<MenuEntity>> GetAllAsync(long userId, CancellationToken cancellationToken);
    }
}
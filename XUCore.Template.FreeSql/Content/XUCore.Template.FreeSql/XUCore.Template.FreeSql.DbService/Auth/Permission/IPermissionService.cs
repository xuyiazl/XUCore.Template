using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;

namespace XUCore.Template.FreeSql.DbService.Auth.Permission
{
    public interface IPermissionService : IScoped
    {
        Task<bool> ExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long userId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long userId, CancellationToken cancellationToken);
    }
}
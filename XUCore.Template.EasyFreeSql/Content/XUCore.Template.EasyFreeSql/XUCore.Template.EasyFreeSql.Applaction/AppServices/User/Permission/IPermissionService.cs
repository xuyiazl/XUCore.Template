using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Permission
{
    public interface IPermissionService : IAppService
    {
        Task<bool> ExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long userId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long userId, CancellationToken cancellationToken);
    }
}
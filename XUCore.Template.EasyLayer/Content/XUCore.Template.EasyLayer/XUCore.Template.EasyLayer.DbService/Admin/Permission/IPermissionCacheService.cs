using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.Permission
{
    public interface IPermissionCacheService : IScoped
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}
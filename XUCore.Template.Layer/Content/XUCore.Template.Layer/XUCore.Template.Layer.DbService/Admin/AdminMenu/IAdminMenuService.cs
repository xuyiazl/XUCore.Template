﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.AdminMenu
{
    public interface IAdminMenuService : ICurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>,
        IScoped
    {
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
    }
}
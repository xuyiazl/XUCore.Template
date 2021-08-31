using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.DbService.Basics.ChinaArea
{
    public interface IChinaAreaService : ICurdService<long, ChinaAreaEntity, ChinaAreaDto, ChinaAreaCreateCommand, ChinaAreaUpdateCommand, ChinaAreaQueryCommand, ChinaAreaQueryPagedCommand>, IDbService
    {
        Task<IList<ChinaAreaTreeDto>> GetListByTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken);
    }
}
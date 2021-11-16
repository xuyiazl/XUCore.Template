using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.FreeSql;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.DbService.Basics.ChinaArea
{
    public class ChinaAreaService : FreeSqlCurdService<long, ChinaAreaEntity, ChinaAreaDto, ChinaAreaCreateCommand, ChinaAreaUpdateCommand, ChinaAreaQueryCommand, ChinaAreaQueryPagedCommand>,
        IChinaAreaService
    {
        public ChinaAreaService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<ChinaAreaEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override Task<ChinaAreaDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return base.GetByIdAsync(id, cancellationToken);
        }

        public override async Task<IList<ChinaAreaDto>> GetListAsync(ChinaAreaQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                .Where(c => c.ParentId == request.CityId)
                .OrderByDescending(c => c.Sort);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<ChinaAreaDto>(cancellationToken);

            return res;
        }

        public override async Task<PagedModel<ChinaAreaDto>> GetPagedListAsync(ChinaAreaQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                .OrderBy(c => c.Sort)
                .ToPagedListAsync<ChinaAreaEntity, ChinaAreaDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<IList<ChinaAreaTreeDto>> GetListByTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                .Where(c => c.Id == request.CityId);

            var res = await select
                      .AsTreeCte(level: request.Level < 1 ? -1 : request.Level)
                      .OrderByDescending(c => c.Sort)
                      .ToTreeListAsync(cancellationToken);

            var tree = mapper.Map<IList<ChinaAreaEntity>, IList<ChinaAreaTreeDto>>(res);

            return tree;
        }
    }
}

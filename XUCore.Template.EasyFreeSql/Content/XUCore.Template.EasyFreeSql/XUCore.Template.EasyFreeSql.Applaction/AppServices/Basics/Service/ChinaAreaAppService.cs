using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities;

namespace XUCore.Template.EasyFreeSql.Applaction.Basics
{
    /// <summary>
    /// 城市区域管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class ChinaAreaAppService : IChinaAreaAppService, IDynamicWebApi
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<ChinaAreaEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;
        public ChinaAreaAppService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<ChinaAreaEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        /// <summary>
        /// 创建城市区域
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateAsync([Required][FromBody] ChinaAreaCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<ChinaAreaCreateCommand, ChinaAreaEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
                return RestFull.Success(data: res.Id);
            else
                return RestFull.Fail(data: 0L);
        }
        /// <summary>
        /// 更新城市区域信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateAsync([Required][FromBody] ChinaAreaUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<ChinaAreaEntity>(cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);


            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除城市区域（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Delete<ChinaAreaEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取城市区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<ChinaAreaDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await repo.Select.WhereDynamic(id).ToOneAsync<ChinaAreaDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取城市区域树形结构
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<ChinaAreaTreeDto>>> GetTreeAsync(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken = default)
        {
            var select = repo.Select
                .Where(c => c.Id == request.CityId);

            var res = await select
                      .AsTreeCte(level: request.Level < 1 ? -1 : request.Level)
                      .OrderByDescending(c => c.Sort)
                      .ToTreeListAsync(cancellationToken);

            var tree = mapper.Map<List<ChinaAreaEntity>, List<ChinaAreaTreeDto>>(res);

            return RestFull.Success(data: tree);
        }
        /// <summary>
        /// 获取城市区域列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<ChinaAreaDto>>> GetListAsync([Required][FromQuery] ChinaAreaQueryCommand request, CancellationToken cancellationToken = default)
        {
            var select = repo.Select
                .Where(c => c.ParentId == request.CityId)
                .OrderByDescending(c => c.Sort);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<ChinaAreaDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
    }
}

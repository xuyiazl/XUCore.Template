using XUCore.Template.EasyFreeSql.Core;
using XUCore.Template.EasyFreeSql.Persistence.Entities.User;

namespace XUCore.Template.EasyFreeSql.Applaction.User.Role
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    [DynamicWebApi]
    public class RoleAppService : IRoleAppService, IDynamicWebApi
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<RoleEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;
        public RoleAppService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<RoleEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateAsync([Required][FromBody] RoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<RoleCreateCommand, RoleEntity>(request);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                await unitOfWork.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

                return RestFull.Success(data: res.Id);
            }
            else
                return RestFull.Fail(data: res.Id);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateAsync([Required][FromBody] RoleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<RoleEntity>(cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            //先清空导航集合，确保没有冗余信息
            await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => c.RoleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
            {
                await unitOfWork.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var entity = await repo.Select.WhereDynamic(id).ToOneAsync(cancellationToken);

            if (entity.IsNull())
                Failure.Error("没有找到该记录");

            switch (field.ToLower())
            {
                case "name":
                    entity.Name = value;
                    break;
                case "sort":
                    entity.Sort = value.ToInt();
                    break;
            }

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<RoleEntity>(cancellationToken);

            list.ForEach(c => c.Enabled = enabled);

            var res = await repo.UpdateAsync(list, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Delete<RoleEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<RoleDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await repo.Select.WhereDynamic(id).ToOneAsync<RoleDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<RoleDto>>> GetListAsync([Required][FromQuery] RoleQueryCommand request, CancellationToken cancellationToken = default)
        {
            var select = repo.Select
                     .Where(c => c.Enabled == request.Enabled)
                     .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                     .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<RoleDto>(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<RoleDto>>> GetPageAsync([Required][FromQuery] RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await repo.Select
                     .Where(c => c.Enabled == request.Enabled)
                     .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                     .OrderBy(c => c.Id)
                     .ToPagedListAsync<RoleEntity, RoleDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<long>>> GetRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await unitOfWork.Orm.Select<RoleMenuEntity>().Where(c => c.RoleId == roleId).OrderBy(c => c.MenuId).ToListAsync(c => c.MenuId, cancellationToken);

            return RestFull.Success(data: res);
        }
    }
}

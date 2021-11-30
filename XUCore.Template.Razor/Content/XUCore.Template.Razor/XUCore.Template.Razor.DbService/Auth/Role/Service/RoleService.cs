using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities.Auth;
using XUCore.Template.Razor.Persistence.Entities.User;

namespace XUCore.Template.Razor.DbService.Auth.Role
{
    public class RoleService : FreeSqlCurdService<long, RoleEntity, RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>,
        IRoleService
    {
        public RoleService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        public override async Task<RoleEntity> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<RoleCreateCommand, RoleEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                await freeSql.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync(cancellationToken);

                CreatedAction?.Invoke(entity);

                return res;
            }

            return default;
        }

        public override async Task<int> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
            {
                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                //先清空导航集合，确保没有冗余信息
                await freeSql.Delete<RoleMenuEntity>().Where(c => c.RoleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

                await freeSql.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync(cancellationToken);

                UpdatedAction?.Invoke(entity);
            }
            return res;
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(id).ToOneAsync(cancellationToken);

            if (entity.IsNull())
                Failure.Error("没有找到该记录");

            switch (field.ToLower())
            {
                case "name":
                    entity.Name = value;
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<RoleEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<RoleEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await freeSql.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await freeSql.Delete<UserRoleEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<RoleDto>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<RoleDto>(cancellationToken);

            return res;
        }

        public async Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken)
        {
            return await freeSql.Select<RoleMenuEntity>().Where(c => c.RoleId == roleId).OrderBy(c => c.MenuId).ToListAsync(c => c.MenuId, cancellationToken);
        }

        public override async Task<PagedModel<RoleDto>> GetPagedListAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderBy(request.OrderBy)
                   .OrderBy(c => c.CreatedAt)
                   .ToPagedListAsync<RoleEntity, RoleDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}

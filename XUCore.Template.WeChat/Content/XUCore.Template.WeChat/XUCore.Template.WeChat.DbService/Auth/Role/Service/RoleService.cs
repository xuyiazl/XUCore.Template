using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities.Auth;
using XUCore.Template.WeChat.Persistence.Entities.User;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Auth.Role
{
    public class RoleService : IRoleService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<RoleEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public RoleService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<RoleEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<RoleEntity> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken)
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

                await unitOfWork.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync(cancellationToken);

                return res;
            }

            return default;
        }

        public async Task<int> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken)
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
                await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => c.RoleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

                await unitOfWork.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync(cancellationToken);
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
        
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<RoleEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<RoleEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await unitOfWork.Orm.Delete<UserRoleEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
            }

            return res;
        }

        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
            => await repo.Select.AnyAsync(c => c.Name == name, cancellationToken);
        public async Task<RoleDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<RoleDto>(cancellationToken);
        }

        public async Task<IList<RoleDto>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken)
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
            return await unitOfWork.Orm.Select<RoleMenuEntity>().Where(c => c.RoleId == roleId).OrderBy(c => c.MenuId).ToListAsync(c => c.MenuId, cancellationToken);
        }

        public async Task<PagedModel<RoleDto>> GetPagedListAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken)
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

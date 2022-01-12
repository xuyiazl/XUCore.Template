using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities.Auth;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Auth.Menu
{
    public class MenuService : IMenuService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<MenuEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public MenuService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<MenuEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<MenuEntity> CreateAsync(MenuCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<MenuCreateCommand, MenuEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
                return res;

            return default;
        }

        public async Task<int> UpdateAsync(MenuUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

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
                case "icon":
                    entity.Icon = value;
                    break;
                case "url":
                    entity.Url = value;
                    break;
                case "onlycode":
                    entity.OnlyCode = value;
                    break;
                case "sort":
                    entity.Weight = value.ToInt();
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }
        
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<MenuEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<MenuEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await unitOfWork.Orm.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);
            }

            return res;
        }

        public async Task<MenuDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<MenuDto>(cancellationToken);
        }

        public async Task<IList<MenuDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var select = repo.Select.OrderByDescending(c => c.Weight).OrderBy(c => c.CreatedAt);

            var res = await select.ToListAsync<MenuDto>(cancellationToken);

            return res;
        }

        public async Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                //.Where(c => c.IsMenu == request.IsMenu)
                .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                .OrderByDescending(c => c.Weight)
                .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<MenuDto>(cancellationToken);

            return res;
        }

        public async Task<PagedModel<MenuDto>> GetPagedListAsync(MenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                .OrderByDescending(c => c.Weight)
                .OrderBy(c => c.CreatedAt)
                .ToPagedListAsync<MenuEntity, MenuDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken)
        {
            //var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToListAsync<MenuTreeDto>(cancellationToken);

            //var tree = res.ToTree(
            //    rootWhere: (r, c) => c.ParentId == 0,
            //    childsWhere: (r, c) => r.Id == c.ParentId,
            //    addChilds: (r, datalist) =>
            //    {
            //        r.Childs ??= new List<MenuTreeDto>();
            //        r.Childs.AddRange(datalist);
            //    });

            var res = await repo.Select.OrderByDescending(c => c.Weight).OrderBy(c => c.CreatedAt).ToTreeListAsync(cancellationToken);

            var tree = mapper.Map<IList<MenuEntity>, IList<MenuTreeDto>>(res);

            return tree;
        }
    }
}

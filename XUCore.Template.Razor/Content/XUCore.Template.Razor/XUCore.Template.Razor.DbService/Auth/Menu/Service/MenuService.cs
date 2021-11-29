using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities.User;

namespace XUCore.Template.Razor.DbService.Auth.Menu
{
    public class MenuService : FreeSqlCurdService<long, MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>,
        IMenuService
    {
        public MenuService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

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
                    entity.Sort = value.ToInt();
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<MenuEntity>(cancellationToken);

            list.ForEach(c => c.Enabled = enabled);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<MenuEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await freeSql.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                .Where(c => c.IsMenu == request.IsMenu)
                .Where(c => c.Enabled == request.Enabled)
                .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<MenuDto>(cancellationToken);

            return res;
        }

        public override async Task<PagedModel<MenuDto>> GetPagedListAsync(MenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .Where(c => c.Enabled == request.Enabled)
                .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                .OrderBy(c => c.Id)
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

            var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToTreeListAsync(cancellationToken);

            var tree = mapper.Map<IList<MenuEntity>, IList<MenuTreeDto>>(res);

            return tree;
        }
    }
}

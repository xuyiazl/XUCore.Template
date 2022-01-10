using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    public class CategoryService : FreeSqlCurdService<long, CategoryEntity, CategoryDto, CategoryCreateCommand, CategoryUpdateCommand, CategoryQueryCommand, CategoryQueryPagedCommand>,
        ICategoryService
    {
        public CategoryService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }
        /// <summary>
        /// 更新部分字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                case "weight":
                    entity.Weight = value.ToInt();
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
        public async Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<CategoryEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        /// <summary>
        /// 检查名字是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
            => await repo.Select.AnyAsync(c => c.Name == name, cancellationToken);

        public override async Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<CategoryDto>(cancellationToken);
        }

        public override async Task<IList<CategoryDto>> GetListAsync(CategoryQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderByDescending(c => c.Weight)
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<CategoryDto>(cancellationToken);

            return res;
        }

        public override async Task<PagedModel<CategoryDto>> GetPagedListAsync(CategoryQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), (c) => c.Name.Contains(request.Keyword))
                   .OrderBy(request.OrderBy.NotEmpty(), request.OrderBy)
                   .OrderByDescending((c) => c.CreatedAt)
                   .ToPagedListAsync<CategoryEntity, CategoryDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}


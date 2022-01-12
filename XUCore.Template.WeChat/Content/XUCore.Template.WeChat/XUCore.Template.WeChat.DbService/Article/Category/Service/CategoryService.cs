using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    public class CategoryService : ICategoryService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<CategoryEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public CategoryService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<CategoryEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<CategoryEntity> CreateAsync(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<CategoryCreateCommand, CategoryEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
                return res;

            return default;
        }

        public async Task<int> UpdateAsync(CategoryUpdateCommand request, CancellationToken cancellationToken)
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
                case "weight":
                    entity.Weight = value.ToInt();
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }
        
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<CategoryEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<CategoryEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            return res;
        }

        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
            => await repo.Select.AnyAsync(c => c.Name == name, cancellationToken);

        public async Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<CategoryDto>(cancellationToken);
        }

        public async Task<IList<CategoryDto>> GetListAsync(CategoryQueryCommand request, CancellationToken cancellationToken)
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

        public async Task<PagedModel<CategoryDto>> GetPagedListAsync(CategoryQueryPagedCommand request, CancellationToken cancellationToken)
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

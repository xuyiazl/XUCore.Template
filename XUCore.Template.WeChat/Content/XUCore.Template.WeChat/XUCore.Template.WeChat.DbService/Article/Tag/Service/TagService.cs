using XUCore.Template.WeChat.Core;
using XUCore.Template.WeChat.Persistence.Entities;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.DbService.Article
{
    public class TagService : ITagService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<TagEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public TagService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<TagEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<TagEntity> CreateAsync(TagCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TagCreateCommand, TagEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
                return res;

            return default;
        }

        public async Task<int> UpdateAsync(TagUpdateCommand request, CancellationToken cancellationToken)
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
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<TagEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<TagEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await unitOfWork.Orm.Delete<ArticleTagEntity>().Where(c => ids.Contains(c.TagId)).ExecuteAffrowsAsync(cancellationToken);
            }

            return res;
        }

        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
            => await repo.Select.AnyAsync(c => c.Name == name, cancellationToken);

        public async Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<TagDto>(cancellationToken);
        }

        public async Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderByDescending(c => c.Weight)
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<TagDto>(cancellationToken);

            return res;
        }

        public async Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), (c) => c.Name.Contains(request.Keyword))
                   .OrderBy(request.OrderBy.NotEmpty(), request.OrderBy)
                   .OrderByDescending((c) => c.CreatedAt)
                   .ToPagedListAsync<TagEntity, TagDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}

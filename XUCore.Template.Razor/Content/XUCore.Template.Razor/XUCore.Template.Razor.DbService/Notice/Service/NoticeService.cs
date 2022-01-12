using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Notice
{
    public class NoticeService : INoticeService
    {
        protected readonly FreeSqlUnitOfWorkManager unitOfWork;
        protected readonly IBaseRepository<NoticeEntity> repo;
        protected readonly IMapper mapper;
        protected readonly IUserInfo user;

        public NoticeService(IServiceProvider serviceProvider)
        {
            this.unitOfWork = serviceProvider.GetRequiredService<FreeSqlUnitOfWorkManager>();
            this.repo = unitOfWork.Orm.GetRepository<NoticeEntity>();
            this.mapper = serviceProvider.GetRequiredService<IMapper>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        public async Task<NoticeEntity> CreateAsync(NoticeCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<NoticeCreateCommand, NoticeEntity>(request);

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
                return res;

            return default;
        }

        public async Task<int> UpdateAsync(NoticeUpdateCommand request, CancellationToken cancellationToken)
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
                case "title":
                    entity.Title = value;
                    break;
                case "weight":
                    entity.Weight = value.ToInt();
                    break;
            }

            return await repo.UpdateAsync(entity, cancellationToken);
        }
        
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<NoticeEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await unitOfWork.Orm.Delete<NoticeEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            return res;
        }

        public async Task<NoticeDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<NoticeDto>(cancellationToken);
        }

        public async Task<IList<NoticeDto>> GetListAsync(NoticeQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .WhereIf(request.Status != Status.Default, c => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Title.Contains(request.Keyword))
                   .OrderBy(c => c.CreatedAt);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<NoticeDto>(cancellationToken);

            return res;
        }

        public async Task<PagedModel<NoticeDto>> GetPagedListAsync(NoticeQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .WhereIf(request.Status != Status.Default, (c) => c.Status == request.Status)
                   .WhereIf(request.Keyword.NotEmpty(), (c) => c.Title.Contains(request.Keyword))
                   .OrderBy(request.OrderBy.NotEmpty(), request.OrderBy)
                   .OrderByDescending((c) => c.CreatedAt)
                   .ToPagedListAsync<NoticeEntity, NoticeDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}

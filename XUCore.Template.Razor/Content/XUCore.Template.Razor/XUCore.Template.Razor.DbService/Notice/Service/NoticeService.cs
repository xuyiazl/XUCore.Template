using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.Persistence.Entities;

namespace XUCore.Template.Razor.DbService.Notice
{
    public class NoticeService : FreeSqlCurdService<long, NoticeEntity, NoticeDto, NoticeCreateCommand, NoticeUpdateCommand, NoticeQueryCommand, NoticeQueryPagedCommand>,
        INoticeService
    {
        public NoticeService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
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
                case "title":
                    entity.Title = value;
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
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            var list = await repo.Select.Where(c => ids.Contains(c.Id)).ToListAsync<NoticeEntity>(cancellationToken);

            list.ForEach(c => c.Status = status);

            return await repo.UpdateAsync(list, cancellationToken);
        }

        public override async Task<NoticeDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await repo.Select.WhereDynamic(id).ToOneAsync<NoticeDto>(cancellationToken);
        }
        public override async Task<IList<NoticeDto>> GetListAsync(NoticeQueryCommand request, CancellationToken cancellationToken)
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

        public override async Task<PagedModel<NoticeDto>> GetPagedListAsync(NoticeQueryPagedCommand request, CancellationToken cancellationToken)
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

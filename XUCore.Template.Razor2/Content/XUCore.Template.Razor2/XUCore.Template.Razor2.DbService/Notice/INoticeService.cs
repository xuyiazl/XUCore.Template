using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Notice
{
    public interface INoticeService : IScoped
    {
        Task<NoticeEntity> CreateAsync(NoticeCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(NoticeUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<NoticeDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<NoticeDto>> GetListAsync(NoticeQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<NoticeDto>> GetPagedListAsync(NoticeQueryPagedCommand request, CancellationToken cancellationToken);
    }
}
using XUCore.Template.Razor2.Persistence.Entities;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.DbService.Article
{
    public interface ITagService :IScoped
    {
        Task<TagEntity> CreateAsync(TagCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(TagUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);

        Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken);
    }
}
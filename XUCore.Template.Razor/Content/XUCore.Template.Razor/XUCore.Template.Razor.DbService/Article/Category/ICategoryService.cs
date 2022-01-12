using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    public interface ICategoryService : IScoped
    {
        Task<CategoryEntity> CreateAsync(CategoryCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(CategoryUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);

        Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<CategoryDto>> GetListAsync(CategoryQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<CategoryDto>> GetPagedListAsync(CategoryQueryPagedCommand request, CancellationToken cancellationToken);
    }
}
using XUCore.Template.Razor.Persistence.Entities;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.DbService.Article
{
    public interface IArticleService : IScoped
    {
        Task<ArticleEntity> CreateAsync(ArticleCreateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(ArticleUpdateCommand request, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);

        Task<ArticleDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IList<ArticleDto>> GetListAsync(ArticleQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<ArticleDto>> GetPagedListAsync(ArticleQueryPagedCommand request, CancellationToken cancellationToken);
    }
}
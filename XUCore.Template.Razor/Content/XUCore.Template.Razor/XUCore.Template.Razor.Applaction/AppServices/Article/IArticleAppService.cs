using XUCore.Template.Razor.DbService.Article;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.Article
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface IArticleAppService : IScoped
    {
        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ArticleCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新文章信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ArticleUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新文章指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除文章（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArticleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<ArticleDto>> GetListAsync(ArticleQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取文章分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<ArticleDto>> GetPagedListAsync(ArticleQueryPagedCommand request, CancellationToken cancellationToken = default);
    }
}

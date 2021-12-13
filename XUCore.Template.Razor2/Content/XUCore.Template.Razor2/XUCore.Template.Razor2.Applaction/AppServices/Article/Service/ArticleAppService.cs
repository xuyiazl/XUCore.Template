using XUCore.Template.Razor2.DbService.Article;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Applaction.Article
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public class ArticleAppService : IArticleAppService
    {
        private readonly IArticleService articleService;

        public ArticleAppService(IServiceProvider serviceProvider)
        {
            this.articleService = serviceProvider.GetRequiredService<IArticleService>();
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(ArticleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await articleService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新文章信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ArticleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await articleService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新文章指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await articleService.UpdateAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            return await articleService.UpdateStatusAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除文章（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await articleService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ArticleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await articleService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<ArticleDto>> GetListAsync(ArticleQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await articleService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取文章分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<ArticleDto>> GetPagedListAsync(ArticleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await articleService.GetPagedListAsync(request, cancellationToken);
        }
    }
}

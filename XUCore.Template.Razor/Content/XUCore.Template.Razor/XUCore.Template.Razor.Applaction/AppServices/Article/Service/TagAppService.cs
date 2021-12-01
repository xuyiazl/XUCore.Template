using XUCore.Template.Razor.DbService.Article;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.Article
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class TagAppService : ITagAppService
    {
        private readonly ITagService TagService;

        public TagAppService(IServiceProvider serviceProvider)
        {
            this.TagService = serviceProvider.GetRequiredService<ITagService>();
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(TagCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await TagService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新标签信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(TagUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await TagService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新标签指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await TagService.UpdateAsync(id, field, value, cancellationToken);
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
            return await TagService.UpdateStatusAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除标签（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await TagService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await TagService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await TagService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取标签分页
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await TagService.GetPagedListAsync(request, cancellationToken);
        }
    }
}

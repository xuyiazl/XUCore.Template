using XUCore.Template.Razor2.DbService.Article;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Applaction.Article
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class TagAppService : ITagAppService
    {
        private readonly ITagService tagService;

        public TagAppService(IServiceProvider serviceProvider)
        {
            this.tagService = serviceProvider.GetRequiredService<ITagService>();
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(TagCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await tagService.CreateAsync(request, cancellationToken);

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
            return await tagService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新标签指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await tagService.UpdateAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            return await tagService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除标签（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await tagService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TagDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await tagService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取目录下拉菜单
        /// </summary>
        /// <param name="checkeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<SelectListItem>> GetSelectItemsCheckAsync(long checkeId = 0, CancellationToken cancellationToken = default)
        {
            var list = await GetListAsync(new TagQueryCommand { Status = Status.Show }, cancellationToken);

            return list.ForEach(item =>
            {
                if (checkeId == item.Id)
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name, Selected = true };
                else
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name };
            });
        }
        /// <summary>
        /// 获取目录下拉菜单
        /// </summary>
        /// <param name="checkedArray"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<SelectListItem>> GetSelectItemsCheckArrayAsync(long[] checkedArray = null, CancellationToken cancellationToken = default)
        {
            var list = await GetListAsync(new TagQueryCommand { Status = Status.Show }, cancellationToken);
            return list.ForEach(item =>
            {
                if (checkedArray != null && checkedArray.Length > 0)
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name, Selected = checkedArray.Contains(item.Id) };
                else
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name };
            });
        }
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<TagDto>> GetListAsync(TagQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await tagService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取标签分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<TagDto>> GetPagedListAsync(TagQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await tagService.GetPagedListAsync(request, cancellationToken);
        }
    }
}

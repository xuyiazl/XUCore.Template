using XUCore.Template.WeChat.DbService.Article;
using XUCore.Template.WeChat.Persistence.Enums;

namespace XUCore.Template.WeChat.Applaction.Article
{
    /// <summary>
    /// 目录管理
    /// </summary>
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryService CategoryService;

        public CategoryAppService(IServiceProvider serviceProvider)
        {
            this.CategoryService = serviceProvider.GetRequiredService<ICategoryService>();
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(CategoryCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await CategoryService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新目录信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(CategoryUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await CategoryService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新目录指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await CategoryService.UpdateAsync(id, field, value, cancellationToken);
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
            return await CategoryService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除目录（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await CategoryService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取目录信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await CategoryService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取目录下拉菜单
        /// </summary>
        /// <param name="checkeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<SelectListItem>> GetSelectItemsCheckAsync(long checkeId = 0, CancellationToken cancellationToken = default)
        {
            var list = await GetListAsync(new CategoryQueryCommand { Status = Status.Show }, cancellationToken);

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
            var list = await GetListAsync(new CategoryQueryCommand { Status = Status.Show }, cancellationToken);
            return list.ForEach(item =>
            {
                if (checkedArray != null && checkedArray.Length > 0)
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name, Selected = checkedArray.Contains(item.Id) };
                else
                    return new SelectListItem { Value = item.Id.SafeString(), Text = item.Name };
            });
        }
        /// <summary>
        /// 获取目录列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<CategoryDto>> GetListAsync(CategoryQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await CategoryService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取目录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<CategoryDto>> GetPagedListAsync(CategoryQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await CategoryService.GetPagedListAsync(request, cancellationToken);
        }
    }
}


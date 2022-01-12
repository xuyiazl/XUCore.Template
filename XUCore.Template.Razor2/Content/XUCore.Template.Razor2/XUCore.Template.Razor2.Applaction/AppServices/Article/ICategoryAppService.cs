using XUCore.Template.Razor2.DbService.Article;
using XUCore.Template.Razor2.Persistence.Enums;

namespace XUCore.Template.Razor2.Applaction.Article
{
    /// <summary>
    /// 目录管理
    /// </summary>
    public interface ICategoryAppService : IScoped
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(CategoryCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新目录信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(CategoryUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新目录指定字段内容
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
        /// 删除目录（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取目录信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取目录下拉菜单
        /// </summary>
        /// <param name="checkeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<SelectListItem>> GetSelectItemsCheckAsync(long checkeId = 0, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取目录下拉菜单
        /// </summary>
        /// <param name="checkedArray"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<SelectListItem>> GetSelectItemsCheckArrayAsync(long[] checkedArray = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取目录列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<CategoryDto>> GetListAsync(CategoryQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取目录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<CategoryDto>> GetPagedListAsync(CategoryQueryPagedCommand request, CancellationToken cancellationToken = default);
    }
}

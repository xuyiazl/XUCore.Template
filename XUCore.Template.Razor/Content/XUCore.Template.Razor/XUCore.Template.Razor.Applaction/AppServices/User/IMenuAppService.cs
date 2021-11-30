using XUCore.Template.Razor.Core.Enums;
using XUCore.Template.Razor.DbService.Auth.Menu;

namespace XUCore.Template.Razor.Applaction.User
{
    /// <summary>
    /// 用户导航管理
    /// </summary>
    public interface IMenuAppService : IScoped
    {
        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CreateAsync(MenuCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(MenuUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateEnabledAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MenuDto> GetAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<MenuTreeDto>> GetTreeAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<MenuDto>> GetListAsync(CancellationToken cancellationToken = default);
    }
}

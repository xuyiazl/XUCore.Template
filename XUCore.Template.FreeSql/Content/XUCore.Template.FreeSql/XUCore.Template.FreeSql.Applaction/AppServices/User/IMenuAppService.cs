using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Template.FreeSql.DbService.Auth.Menu;

namespace XUCore.Template.FreeSql.Applaction.User
{
    /// <summary>
    /// 用户导航管理
    /// </summary>
    public interface IMenuAppService : IAppService
    {
        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<long>> CreateAsync(MenuCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(MenuUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateEnabledAsync(long[] ids, bool enabled, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<MenuDto>> GetAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<MenuTreeDto>>> GetTreeAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<MenuDto>>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken = default);
    }
}

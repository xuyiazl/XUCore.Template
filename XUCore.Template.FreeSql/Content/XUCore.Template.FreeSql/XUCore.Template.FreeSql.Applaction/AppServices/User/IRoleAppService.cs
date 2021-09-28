using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.FreeSql.DbService.Auth.Role;

namespace XUCore.Template.FreeSql.Applaction.User
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public interface IRoleAppService : IAppService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<long>> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色指定字段内容
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
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<RoleDto>> GetAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<RoleDto>>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<RoleDto>>> GetPageAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken = default);
    }
}

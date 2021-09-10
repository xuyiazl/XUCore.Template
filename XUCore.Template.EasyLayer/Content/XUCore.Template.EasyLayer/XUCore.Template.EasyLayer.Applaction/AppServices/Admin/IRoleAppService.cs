using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.DbService.Admin.AdminMenu;
using XUCore.Template.EasyLayer.DbService.Admin.AdminRole;
using XUCore.Template.EasyLayer.DbService.Admin.AdminUser;

namespace XUCore.Template.EasyLayer.Applaction.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IRoleAppService : IAppService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<long>> CreateAsync(AdminRoleCreateCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(AdminRoleUpdateCommand request, CancellationToken cancellationToken = default);
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
        /// 更新角色状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateStatusAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
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
        Task<Result<AdminRoleDto>> GetAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminRoleDto>>> GetListAsync(AdminRoleQueryCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<AdminRoleDto>>> GetPageAsync(AdminRoleQueryPagedCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken = default);
    }
}

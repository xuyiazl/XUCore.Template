using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.DbService.Auth.Role;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.User
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleService roleService;

        public RoleAppService(IServiceProvider serviceProvider)
        {
            this.roleService = serviceProvider.GetRequiredService<IRoleService>();
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            return await roleService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await roleService.UpdateAsync(id, field, value, cancellationToken);
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
            return await roleService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await roleService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RoleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await roleService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<RoleDto>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await roleService.GetListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<RoleDto>> GetPageAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await roleService.GetPagedListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await roleService.GetRelevanceMenuAsync(roleId, cancellationToken);
        }
    }
}

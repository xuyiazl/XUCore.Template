using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Core.Enums;
using XUCore.Template.FreeSql.DbService.Auth.Menu;
using XUCore.Template.FreeSql.DbService.Auth.Role;
using XUCore.Template.FreeSql.DbService.User.User;

namespace XUCore.Template.FreeSql.Applaction.User
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class RoleAppService : AppService, IRoleAppService
    {
        private readonly IRoleService roleService;

        public RoleAppService(IServiceProvider serviceProvider)
        {
            this.roleService = serviceProvider.GetService<IRoleService>();
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<long>> CreateAsync([Required][FromBody] RoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.CreateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateAsync([Required][FromBody] RoleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(ids, enabled, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await roleService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<RoleDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<RoleDto>>> GetListAsync([Required][FromQuery] RoleQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<RoleDto>>> GetPageAsync([Required][FromQuery] RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetRelevanceMenuAsync(roleId, cancellationToken);

            return RestFull.Success(data: res);
        }
    }
}

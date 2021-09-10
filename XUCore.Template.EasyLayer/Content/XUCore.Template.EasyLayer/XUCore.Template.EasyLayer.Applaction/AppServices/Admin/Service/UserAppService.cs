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
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.DbService.Admin.AdminUser;

namespace XUCore.Template.EasyLayer.Applaction.Admin
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IAdminUserService adminUserService;

        public UserAppService(IServiceProvider serviceProvider)
        {
            this.adminUserService = serviceProvider.GetService<IAdminUserService>();
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建初始账号
        /// </summary>
        /// <remarks>
        /// 初始账号密码：
        ///     <para>username : admin</para>
        ///     <para>password : admin</para>
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<long>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
        {
            var command = new AdminUserCreateCommand
            {
                UserName = "admin",
                Password = "admin",
                Company = "",
                Location = "",
                Mobile = "13500000000",
                Name = "admin",
                Position = ""
            };

            command.IsVaild();

            return await CreateAsync(command, cancellationToken);
        }
        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<long>> CreateAsync([Required][FromBody] AdminUserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.CreateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateAsync([Required][FromBody] AdminUserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdatePasswordAsync([Required][FromBody] AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateStatusAsync([Required][FromQuery] long[] ids, [Required][FromQuery] Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(ids, status, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result<AdminUserDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminUserDto>> GetAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetByAccountAsync(accountMode, account, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GetAnyAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.AnyByAccountAsync(accountMode, account, notId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<AdminUserDto>>> GetPageAsync([Required][FromQuery] AdminUserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateRelevanceRoleAsync([Required][FromBody] AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.CreateRelevanceRoleAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetRelevanceRoleKeysAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRoleKeysAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<AdminUserLoginRecordDto>>> GetRecordListAsync([Required][FromQuery] AdminUserLoginRecordQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRecordListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<AdminUserLoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRecordPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}

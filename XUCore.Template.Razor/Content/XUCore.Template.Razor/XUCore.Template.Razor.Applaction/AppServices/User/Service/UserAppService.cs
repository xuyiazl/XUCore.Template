using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.DbService.User.User;
using XUCore.Template.Razor.Persistence.Enums;

namespace XUCore.Template.Razor.Applaction.User
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserAppService : IUserAppService
    {
        private readonly IUserService userService;

        public UserAppService(IServiceProvider serviceProvider)
        {
            this.userService = serviceProvider.GetRequiredService<IUserService>();
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(UserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.CreateAsync(request, cancellationToken);

            if (res != null)
                return res.Id;
            else
                return 0L;
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(UserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdatePasswordAsync(UserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(request, cancellationToken);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            return await userService.UpdateAsync(id, field, value, cancellationToken);
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
            return await userService.UpdateAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            return await userService.DeleteAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await userService.GetByIdAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserDto> GetAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken = default)
        {
            return await userService.GetByAccountAsync(accountMode, account, cancellationToken);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> GetAnyAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken = default)
        {
            return await userService.AnyByAccountAsync(accountMode, account, notId, cancellationToken);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<UserDto>> GetPageAsync(UserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.GetPagedListAsync(request, cancellationToken);
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CreateRelevanceRoleAsync(UserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.CreateRelevanceRoleAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<long>> GetRelevanceRoleKeysAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await userService.GetRoleKeysAsync(userId, cancellationToken);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<UserLoginRecordDto>> GetRecordListAsync(UserLoginRecordQueryCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.GetRecordListAsync(request, cancellationToken);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<UserLoginRecordDto>> GetRecordPageAsync(UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            return await userService.GetRecordPagedListAsync(request, cancellationToken);
        }

        #endregion
    }
}

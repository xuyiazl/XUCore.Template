using XUCore.Template.Razor.Core;
using XUCore.Template.Razor.DbService.Auth.Permission;
using XUCore.Template.Razor.DbService.User.User;

namespace XUCore.Template.Razor.Applaction.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    public class LoginAppService : ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly IUserService userService;
        private readonly IUserInfo user;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetRequiredService<IPermissionService>();
            this.userService = serviceProvider.GetRequiredService<IUserService>();
            this.user = serviceProvider.GetRequiredService<IUserInfo>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var userDto = await userService.LoginAsync(request, cancellationToken);

            await user.LoginAsync(userDto.Id, userDto.UserName, userDto.Name, userDto.Name, cancellationToken);

            return true;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            await user.LoginOutAsync(cancellationToken);
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> GetPermissionExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken = default)
        {
            return await permissionService.ExistsAsync(userId, onlyCode, cancellationToken);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<PermissionMenuDto>> GetPermissionMenusAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await permissionService.GetMenusAsync(userId, cancellationToken);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<PermissionMenuDto>> GetPermissionMenuExpressAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await permissionService.GetMenuExpressAsync(userId, cancellationToken);
        }

        #endregion
    }
}

using XUCore.Template.WeChat.DbService.Auth.Permission;
using XUCore.Template.WeChat.DbService.User.User;

namespace XUCore.Template.WeChat.Applaction.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    public interface ILoginAppService : IScoped
    {

        #region [ 登录 ]

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LoginOutAsync(CancellationToken cancellationToken);
        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> GetPermissionExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<PermissionMenuDto>> GetPermissionMenusAsync(long userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<PermissionMenuDto>> GetPermissionMenuExpressAsync(long userId, CancellationToken cancellationToken = default);

        #endregion
    }
}


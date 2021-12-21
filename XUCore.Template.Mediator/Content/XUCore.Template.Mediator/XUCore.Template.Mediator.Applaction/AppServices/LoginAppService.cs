namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 用户登录接口
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class LoginAppService : IDynamicWebApi
{
    protected readonly IMediatorHandler bus;

    public LoginAppService(IServiceProvider serviceProvider)
    {
        this.bus = serviceProvider.GetRequiredService<IMediatorHandler>();
    }
    #region [ 登录 ]

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    public async Task<Result<LoginTokenDto>> PostAsync([Required][FromBody] UserLoginCommand request, CancellationToken cancellationToken)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task PostOutAsync(CancellationToken cancellationToken)
        => await bus.SendCommand(new UserLoginOutCommand(), cancellationToken);

    #endregion

    #region [ 登录后的权限获取 ]

    /// <summary>
    /// 查询是否有权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> GetPermissionExistsAsync([Required][FromQuery] PermissionExistCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 查询权限导航
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required][FromQuery] PermissionQueryMenuTreeCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 查询权限导航（快捷导航）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required][FromQuery] PermissionQueryMenuExpressCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);

    #endregion
}
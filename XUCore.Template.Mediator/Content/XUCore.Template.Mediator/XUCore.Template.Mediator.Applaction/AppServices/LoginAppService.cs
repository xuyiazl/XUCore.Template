namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 用户登录接口
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class LoginAppService : IDynamicWebApi
{
    protected readonly IMediator mediator;

    public LoginAppService(IServiceProvider serviceProvider)
    {
        this.mediator = serviceProvider.GetRequiredService<IMediator>();
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
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task PostOutAsync(CancellationToken cancellationToken)
        => await mediator.Send(new UserLoginOutCommand(), cancellationToken);

    #endregion

    #region [ 登录后的权限获取 ]

    /// <summary>
    /// 查询是否有权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> GetPermissionExistsAsync([Required][FromQuery] PermissionExistCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 查询权限导航
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required][FromQuery] PermissionQueryMenuTreeCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
    /// <summary>
    /// 查询权限导航（快捷导航）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required][FromQuery] PermissionQueryMenuExpressCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);

    #endregion
}
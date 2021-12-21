namespace XUCore.Template.Mediator.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi]
public class UserAppService : IDynamicWebApi
{
    protected readonly IMediatorHandler bus;
    public UserAppService(IServiceProvider serviceProvider)
    {
        this.bus = serviceProvider.GetRequiredService<IMediatorHandler>();
    }

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
    [AllowAnonymous]
    public async Task<Result<long>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
    {
        var command = new UserCreateCommand
        {
            UserName = "admin",
            Password = "admin",
            Company = "",
            Location = "",
            Mobile = "13500000000",
            Name = "admin",
            Position = ""
        };

        return await CreateAsync(command, cancellationToken);
    }

    #region [ 账号管理 ]

    /// <summary>
    /// 创建用户账号
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([Required][FromBody] UserCreateCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新账号信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([Required][FromBody] UserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新密码
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdatePasswordAsync([Required][FromBody] UserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新指定字段内容
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateFieldAsync([Required][FromBody] UserUpdateFieldCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateEnabledAsync([Required][FromQuery] UserUpdateStatusCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 删除账号（物理删除）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([Required][FromQuery] UserDeleteCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取账号信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<UserDto>> GetAsync([Required] long id, CancellationToken cancellationToken = default)
        => await bus.SendCommand(new UserQueryByIdCommand { Id = id }, cancellationToken);
    /// <summary>
    /// 获取账号信息（根据账号或手机号码）
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<UserDto>> GetAccountAsync([Required][FromQuery] UserQueryByAccountCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 检查账号或者手机号是否存在
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> GetAnyAsync([Required][FromQuery] UserAnyByAccountCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取账号分页
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<UserDto>>> GetPageAsync([Required][FromQuery] UserQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);

    #endregion

    #region [ 账号&角色 关联操作 ]

    /// <summary>
    /// 账号关联角色
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> CreateRelevanceRoleAsync([Required][FromBody] UserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取账号关联的角色id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<long>>> GetRelevanceRoleKeysAsync([Required] long userId, CancellationToken cancellationToken = default)
        => await bus.SendCommand(new UserQueryRelevanceRoleCommand { UserId = userId }, cancellationToken);

    #endregion

    #region [ 登录记录 ]

    /// <summary>
    /// 获取最近登录记录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<UserLoginRecordDto>>> GetRecordListAsync([Required][FromQuery] UserQueryLoginRecordCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);
    /// <summary>
    /// 获取所有登录记录分页
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] UserQueryLoginRecordPagedCommand request, CancellationToken cancellationToken = default)
        => await bus.SendCommand(request, cancellationToken);

    #endregion
}
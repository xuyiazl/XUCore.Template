namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserQueryByAccountCommand : Command<Result<UserDto>>, IDynamicWebApi
{
    /// <summary>
    /// 账号类型
    /// </summary>
    /// <value></value>
    [Required]
    public AccountMode AccountMode { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    /// <value></value>
    [Required]
    public string Account { get; set; }

    /// <summary>
    /// 获取账号信息（根据账号或手机号码）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<UserDto>> GetAccountAsync([FromServices] IMediator mediator, [Required][FromQuery] UserQueryByAccountCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


internal class UserQueryByAccountCommandValidator : CommandValidator<UserQueryByAccountCommand>
{
    public UserQueryByAccountCommandValidator()
    {

    }
}

internal class UserQueryByAccountCommandHandler : CommandHandler<UserQueryByAccountCommand, Result<UserDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryByAccountCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<UserDto>> Handle(UserQueryByAccountCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var user = default(UserDto);

        switch (request.AccountMode)
        {
            case AccountMode.UserName:
                user = await repo.Select.Where(c => c.UserName.Equals(request.Account)).ToOneAsync<UserDto>(cancellationToken);
                break;
            case AccountMode.Mobile:
                user = await repo.Select.Where(c => c.Mobile.Equals(request.Account)).ToOneAsync<UserDto>(cancellationToken);
                break;
        }

        return RestFull.Success(data: user);
    }
}
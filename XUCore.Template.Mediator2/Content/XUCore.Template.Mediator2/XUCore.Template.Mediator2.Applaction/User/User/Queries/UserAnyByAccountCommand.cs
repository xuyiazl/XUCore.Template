namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserAnyByAccountCommand : Command<Result<bool>>, IDynamicWebApi
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
    /// 需要排除的id
    /// </summary>
    /// <value></value>
    [Required]
    public long NotId { get; set; }

    /// <summary>
    /// 检查账号或者手机号是否存在
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> GetAnyAsync([FromServices] IMediator mediator, [Required][FromQuery] UserAnyByAccountCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

internal class UserAnyByAccountCommandValidator : CommandValidator<UserAnyByAccountCommand>
{
    public UserAnyByAccountCommandValidator()
    {

    }
}

internal class UserAnyByAccountCommandHandler : CommandHandler<UserAnyByAccountCommand, Result<bool>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserAnyByAccountCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<bool>> Handle(UserAnyByAccountCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var res = true;
        if (request.NotId > 0)
        {
            switch (request.AccountMode)
            {
                case AccountMode.UserName:
                    res = await repo.Select.AnyAsync(c => c.Id != request.NotId && c.UserName == request.Account, cancellationToken);
                    break;
                case AccountMode.Mobile:
                    res = await repo.Select.AnyAsync(c => c.Id != request.NotId && c.Mobile == request.Account, cancellationToken);
                    break;
            }
        }
        else
        {
            switch (request.AccountMode)
            {
                case AccountMode.UserName:
                    res = await repo.Select.AnyAsync(c => c.UserName == request.Account, cancellationToken);
                    break;
                case AccountMode.Mobile:
                    res = await repo.Select.AnyAsync(c => c.Mobile == request.Account, cancellationToken);
                    break;
            }
        }

        return RestFull.Success(data: res);
    }
}
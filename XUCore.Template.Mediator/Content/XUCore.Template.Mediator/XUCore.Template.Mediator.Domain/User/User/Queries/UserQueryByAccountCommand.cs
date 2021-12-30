namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 用户查询
/// </summary>
public class UserQueryByAccountCommand : Command<Result<UserDto>>
{
    /// <summary>
    /// 账号类型
    /// </summary>
    /// <value></value>
    public AccountMode AccountMode { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    /// <value></value>
    public string Account { get; set; }
}


public class UserQueryByAccountCommandValidator : CommandValidator<UserQueryByAccountCommand>
{
    public UserQueryByAccountCommandValidator()
    {

    }
}

public class UserQueryByAccountCommandHandler : CommandHandler<UserQueryByAccountCommand, Result<UserDto>>
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
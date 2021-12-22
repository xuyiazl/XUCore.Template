namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 查询账号是否存在
/// </summary>
public class UserAnyByAccountCommand : Command<Result<bool>>
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
    /// <summary>
    /// 需要排除的id
    /// </summary>
    /// <value></value>
    public long NotId { get; set; }
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

    public UserAnyByAccountCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
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
namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 用户删除命令
/// </summary>
public class UserDeleteCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }
}

public class UserDeleteCommandValidator : CommandValidator<UserDeleteCommand>
{
    public UserDeleteCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

public class UserDeleteCommandHandler : CommandHandler<UserDeleteCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Delete<UserEntity>(request.Ids).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
        {
            //删除关联的导航
            await db.Orm.Delete<UserLoginRecordEntity>().Where(c => request.Ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);
            //删除用户关联的角色
            await db.Orm.Delete<UserRoleEntity>().Where(c => request.Ids.Contains(c.UserId)).ExecuteAffrowsAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        else
            return RestFull.Fail(data: res);
    }
}
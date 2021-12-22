namespace XUCore.Template.Mediator.Applaction.Commands;

/// <summary>
/// 获取角色关联的所有导航id集合
/// </summary>
public class UserQueryRelevanceRoleCommand : Command<Result<List<long>>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    /// <value></value>
    [Required]
    public long UserId { get; set; }
}

public class UserQueryRelevanceRoleCommandValidator : CommandValidator<UserQueryRelevanceRoleCommand>
{
    public UserQueryRelevanceRoleCommandValidator()
    {

    }
}

public class UserQueryRelevanceRoleCommandHandler : CommandHandler<UserQueryRelevanceRoleCommand, Result<List<long>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryRelevanceRoleCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<long>>> Handle(UserQueryRelevanceRoleCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Select<UserRoleEntity>().Where(c => c.UserId == request.UserId).OrderBy(c => c.RoleId).ToListAsync(c => c.RoleId, cancellationToken);

        return RestFull.Success(data: res);
    }
}
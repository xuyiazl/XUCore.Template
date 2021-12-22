namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 关联角色命令
/// </summary>
public class UserRelevanceRoleCommand : Command<Result<int>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public long UserId { get; set; }
    /// <summary>
    /// 角色id集合
    /// </summary>
    public long[] RoleIds { get; set; }
}

internal class UserRelevanceRoleCommandValidator : CommandValidator<UserRelevanceRoleCommand>
{
    public UserRelevanceRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithName("UserId");
    }
}

internal class UserRelevanceRoleCommandHandler : CommandHandler<UserRelevanceRoleCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserRelevanceRoleCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(UserRelevanceRoleCommand request, CancellationToken cancellationToken)
    {
        //先清空用户的角色，确保没有冗余的数据
        await db.Orm.Delete<UserRoleEntity>().Where(c => c.UserId == request.UserId).ExecuteAffrowsAsync(cancellationToken);

        var userRoles = Array.ConvertAll(request.RoleIds, roleid => new UserRoleEntity
        {
            RoleId = roleid,
            UserId = request.UserId
        });

        var res = 0;
        //添加角色
        if (userRoles.Length > 0)
            res = await db.Orm.Insert(userRoles).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
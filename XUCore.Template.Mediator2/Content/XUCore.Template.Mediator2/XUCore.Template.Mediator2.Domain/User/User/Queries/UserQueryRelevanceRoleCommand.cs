namespace XUCore.Template.Mediator2.Domain;

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

internal class UserQueryRelevanceRoleCommandValidator : CommandValidator<UserQueryRelevanceRoleCommand>
{
    public UserQueryRelevanceRoleCommandValidator()
    {

    }
}

internal class UserQueryRelevanceRoleCommandHandler : CommandHandler<UserQueryRelevanceRoleCommand, Result<List<long>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryRelevanceRoleCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
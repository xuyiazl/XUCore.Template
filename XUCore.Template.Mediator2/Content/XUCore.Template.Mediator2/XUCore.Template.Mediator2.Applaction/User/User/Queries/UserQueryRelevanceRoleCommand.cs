namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserQueryRelevanceRoleCommand : Command<Result<List<long>>>, IDynamicWebApi
{
    /// <summary>
    /// 用户id
    /// </summary>
    /// <value></value>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// 获取账号关联的角色id集合
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<long>>> GetRelevanceRoleKeysAsync([FromServices] IMediator mediator, [Required] long userId, CancellationToken cancellationToken = default)
        => await mediator.Send(new UserQueryRelevanceRoleCommand { UserId = userId }, cancellationToken);
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
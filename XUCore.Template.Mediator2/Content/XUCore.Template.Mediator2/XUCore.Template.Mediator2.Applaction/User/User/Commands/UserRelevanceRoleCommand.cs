namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserRelevanceRoleCommand : Command<Result<int>>, IDynamicWebApi
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

    /// <summary>
    /// 账号关联角色
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> CreateRelevanceRoleAsync([FromServices] IMediator mediator, [Required][FromBody] UserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
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

    public UserRelevanceRoleCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
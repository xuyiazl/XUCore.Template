namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserDeleteCommand : Command<Result<int>>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }

    /// <summary>
    /// 删除账号（物理删除）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([FromServices] IMediator mediator, [Required][FromQuery] UserDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
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

    public UserDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
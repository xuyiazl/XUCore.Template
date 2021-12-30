namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleDeleteCommand : Command<Result<int>>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }

    /// <summary>
    /// 删除角色（物理删除）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([FromServices] IMediator mediator, [Required][FromQuery] RoleDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class RoleDeleteCommandValidator : CommandValidator<RoleDeleteCommand>
{
    public RoleDeleteCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

public class RoleDeleteCommandHandler : CommandHandler<RoleDeleteCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Delete<RoleEntity>(request.Ids).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
        {
            //删除关联的导航
            await db.Orm.Delete<RoleMenuEntity>().Where(c => request.Ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
            //删除用户关联的角色
            await db.Orm.Delete<UserRoleEntity>().Where(c => request.Ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        else
            return RestFull.Fail(data: res);
    }
}
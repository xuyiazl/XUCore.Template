namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuDeleteCommand : Command<Result<int>>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }

    /// <summary>
    /// 删除导航（物理删除）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([FromServices] IMediator mediator, [Required][FromQuery] MenuDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class MenuDeleteCommandValidator : CommandValidator<MenuDeleteCommand>
{
    public MenuDeleteCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

public class MenuDeleteCommandHandler : CommandHandler<MenuDeleteCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(MenuDeleteCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Delete<MenuEntity>(request.Ids).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
        {
            await db.Orm.Delete<RoleMenuEntity>().Where(c => request.Ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);
        }

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
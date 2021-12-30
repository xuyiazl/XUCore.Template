namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 权限操作
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Login")]
public class PermissionQueryMenuExpressCommand : Command<Result<List<PermissionMenuDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// 查询权限导航（快捷导航）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuDto>>> GetPermissionMenuExpressAsync([FromServices] IMediator mediator, [Required][FromQuery] PermissionQueryMenuExpressCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


public class PermissionQueryMenuExpressCommandValidator : CommandValidator<PermissionQueryMenuExpressCommand>
{
    public PermissionQueryMenuExpressCommandValidator()
    {

    }
}

public class PermissionQueryMenuExpressCommandHandler : CommandHandler<PermissionQueryMenuExpressCommand, Result<List<PermissionMenuDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public PermissionQueryMenuExpressCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<PermissionMenuDto>>> Handle(PermissionQueryMenuExpressCommand request, CancellationToken cancellationToken)
    {
        var menus = await mediator.Send(new PermissionQueryCacheCommand { UserId = request.UserId }, cancellationToken);

        var list = menus
            .Where(c => c.IsMenu == true && c.IsExpress == true)
            .OrderByDescending(c => c.Sort)
            .ToList();

        var dto = mapper.Map<List<MenuEntity>, List<PermissionMenuDto>>(list);

        return RestFull.Success(data: dto);
    }
}
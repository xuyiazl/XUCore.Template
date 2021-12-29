namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 权限查询快捷菜单命令
/// </summary>
public class PermissionQueryMenuExpressCommand : Command<Result<List<PermissionMenuDto>>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
}


internal class PermissionQueryMenuExpressCommandValidator : CommandValidator<PermissionQueryMenuExpressCommand>
{
    public PermissionQueryMenuExpressCommandValidator()
    {

    }
}

internal class PermissionQueryMenuExpressCommandHandler : CommandHandler<PermissionQueryMenuExpressCommand, Result<List<PermissionMenuDto>>>
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
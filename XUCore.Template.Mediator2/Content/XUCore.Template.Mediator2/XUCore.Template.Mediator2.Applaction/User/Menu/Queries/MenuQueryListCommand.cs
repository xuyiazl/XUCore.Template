namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuQueryListCommand : Command<Result<List<MenuDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    [Required]
    public int Limit { get; set; }
    /// <summary>
    /// 是否是导航
    /// </summary>
    public bool IsMenu { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// 获取导航列表
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<MenuDto>>> GetListAsync([FromServices] IMediator mediator, [Required][FromQuery] MenuQueryListCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class MenuQueryListCommandValidator : CommandValidator<MenuQueryListCommand>
{
    public MenuQueryListCommandValidator()
    {

    }
}

public class MenuQueryListCommandHandler : CommandHandler<MenuQueryListCommand, Result<List<MenuDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<MenuDto>>> Handle(MenuQueryListCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var select = repo.Select
           .Where(c => c.IsMenu == request.IsMenu)
           .Where(c => c.Enabled == request.Enabled)
           .OrderBy(c => c.Id);

        if (request.Limit > 0)
            select = select.Take(request.Limit);

        var res = await select.ToListAsync<MenuDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}

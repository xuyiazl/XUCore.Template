namespace XUCore.Template.Mediator.Applaction.Commands;

/// <summary>
/// 菜单查询命令
/// </summary>
public class MenuQueryListCommand : Command<Result<List<MenuDto>>>
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    public int Limit { get; set; }
    /// <summary>
    /// 是否是导航
    /// </summary>
    public bool IsMenu { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
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

    public MenuQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
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

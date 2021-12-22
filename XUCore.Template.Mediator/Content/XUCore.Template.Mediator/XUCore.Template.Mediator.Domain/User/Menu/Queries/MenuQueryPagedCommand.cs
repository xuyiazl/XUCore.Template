namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 菜单查询命令
/// </summary>
public class MenuQueryPagedCommand : Command<Result<PagedModel<MenuDto>>>
{
    /// <summary>
    /// 当前页码
    /// </summary>
    /// <value></value>
    [Required]
    public int CurrentPage { get; set; }
    /// <summary>
    /// 分页大小
    /// </summary>
    /// <value></value>
    [Required]
    public int PageSize { get; set; }
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}

internal class MenuQueryPagedCommandValidator : CommandValidator<MenuQueryPagedCommand>
{
    public MenuQueryPagedCommandValidator()
    {
        RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
    }
}

internal class MenuQueryPagedCommandHandler : CommandHandler<MenuQueryPagedCommand, Result<PagedModel<MenuDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuQueryPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<PagedModel<MenuDto>>> Handle(MenuQueryPagedCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var res = await repo.Select
                      .Where(c => c.Enabled == request.Enabled)
                      .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                      .OrderBy(c => c.Id)
                      .ToPagedListAsync<MenuEntity, MenuDto>(request.CurrentPage, request.PageSize, cancellationToken);

        var model = res.ToModel();

        return RestFull.Success(data: model);
    }
}
namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuQueryPagedCommand : Command<Result<PagedModel<MenuDto>>>, IDynamicWebApi
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
    /// <summary>
    /// 获取分页
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<MenuDto>>> GetPageAsync([FromServices] IMediator mediator, [Required][FromQuery] MenuQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
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

    public MenuQueryPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
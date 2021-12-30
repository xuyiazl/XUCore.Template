namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 省市区域管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "ChinaArea")]
public class ChinaAreaQueryPagedCommand : Command<Result<PagedModel<ChinaAreaDto>>>, IDynamicWebApi
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
    /// 获取城市区域分页
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<ChinaAreaDto>>> GetPageAsync([FromServices] IMediator mediator, [Required][FromQuery] ChinaAreaQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class ChinaAreaQueryPagedCommandValidator : CommandValidator<ChinaAreaQueryPagedCommand>
{
    public ChinaAreaQueryPagedCommandValidator()
    {
        RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
    }
}


public class ChinaAreaQueryPagedCommandHandler : CommandHandler<ChinaAreaQueryPagedCommand, Result<PagedModel<ChinaAreaDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaQueryPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<PagedModel<ChinaAreaDto>>> Handle(ChinaAreaQueryPagedCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<ChinaAreaEntity>();

        var res = await repo.Select
                      .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                      .OrderBy(c => c.Id)
                      .ToPagedListAsync<ChinaAreaEntity, ChinaAreaDto>(request.CurrentPage, request.PageSize, cancellationToken);

        var model = res.ToModel();

        return RestFull.Success(data: model);
    }
}
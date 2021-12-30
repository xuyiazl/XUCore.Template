namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 省市区域查询命令
/// </summary>
public class ChinaAreaQueryPagedCommand : Command<Result<PagedModel<ChinaAreaDto>>>
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
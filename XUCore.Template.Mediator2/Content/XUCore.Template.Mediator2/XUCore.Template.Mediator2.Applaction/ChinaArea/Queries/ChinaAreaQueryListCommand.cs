namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 省市区域管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "ChinaArea")]
public class ChinaAreaQueryListCommand : Command<Result<List<ChinaAreaDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    [Required]
    public int Limit { get; set; }
    /// <summary>
    /// 城市Id
    /// </summary>
    [Required]
    public long CityId { get; set; }

    /// <summary>
    /// 获取城市区域列表
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<ChinaAreaDto>>> GetListAsync([FromServices] IMediator mediator, [Required][FromQuery] ChinaAreaQueryListCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


public class ChinaAreaQueryListCommandValidator : CommandValidator<ChinaAreaQueryListCommand>
{
    public ChinaAreaQueryListCommandValidator()
    {

    }
}

public class ChinaAreaQueryListCommandHandler : CommandHandler<ChinaAreaQueryListCommand, Result<List<ChinaAreaDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<ChinaAreaDto>>> Handle(ChinaAreaQueryListCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<ChinaAreaEntity>();

        var select = repo.Select
            .Where(c => c.ParentId == request.CityId)
            .OrderByDescending(c => c.Sort);

        if (request.Limit > 0)
            select = select.Take(request.Limit);

        var res = await select.ToListAsync<ChinaAreaDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 省市区域查询命令
/// </summary>
public class ChinaAreaQueryTreeCommand : Command<Result<List<ChinaAreaTreeDto>>>
{
    /// <summary>
    /// 城市Id
    /// </summary>
    [Required]
    public long CityId { get; set; }
    /// <summary>
    /// 行政区域等级 0-全部 1-到市级 2-到县级 3-到街道
    /// </summary>
    [Required]
    public short Level { get; set; }
}

internal class ChinaAreaQueryTreeCommandValidator : CommandValidator<ChinaAreaQueryTreeCommand>
{
    public ChinaAreaQueryTreeCommandValidator()
    {

    }
}


internal class Handler : CommandHandler<ChinaAreaQueryTreeCommand, Result<List<ChinaAreaTreeDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public Handler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<ChinaAreaTreeDto>>> Handle(ChinaAreaQueryTreeCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<ChinaAreaEntity>();

        var select = repo.Select
                        .Where(c => c.Id == request.CityId);

        var res = await select
                  .AsTreeCte(level: request.Level < 1 ? -1 : request.Level)
                  .OrderByDescending(c => c.Sort)
                  .ToTreeListAsync(cancellationToken);

        var tree = mapper.Map<List<ChinaAreaEntity>, List<ChinaAreaTreeDto>>(res);

        return RestFull.Success(data: tree);
    }
}
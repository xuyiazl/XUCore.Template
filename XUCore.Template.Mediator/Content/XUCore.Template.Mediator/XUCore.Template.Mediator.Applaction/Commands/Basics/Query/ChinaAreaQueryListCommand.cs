namespace XUCore.Template.Mediator.Applaction.Commands;

/// <summary>
/// 省市区域查询命令
/// </summary>
public class ChinaAreaQueryListCommand : Command<Result<List<ChinaAreaDto>>>
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    public int Limit { get; set; }
    /// <summary>
    /// 城市Id
    /// </summary>
    [Required]
    public long CityId { get; set; }
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

    public ChinaAreaQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
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
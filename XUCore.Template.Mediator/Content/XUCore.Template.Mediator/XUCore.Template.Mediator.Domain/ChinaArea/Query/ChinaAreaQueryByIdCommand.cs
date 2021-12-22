namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 查询一区域记录
/// </summary>
public class ChinaAreaQueryByIdCommand : Command<Result<ChinaAreaDto>>
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }
}

public class ChinaAreaQueryByIdCommandValidator : CommandValidator<ChinaAreaQueryByIdCommand>
{
    public ChinaAreaQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

public class ChinaAreaQueryByIdCommandHandler : CommandHandler<ChinaAreaQueryByIdCommand, Result<ChinaAreaDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<ChinaAreaDto>> Handle(ChinaAreaQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<ChinaAreaEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<ChinaAreaDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
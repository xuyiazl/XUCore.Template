namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 省市区域删除命令
/// </summary>
public class ChinaAreaDeleteCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }
}

internal class ChinaAreaDeleteCommandValidator : CommandValidator<ChinaAreaDeleteCommand>
{
    public ChinaAreaDeleteCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}


internal class ChinaAreaDeleteCommandHandler : CommandHandler<ChinaAreaDeleteCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(ChinaAreaDeleteCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Delete<ChinaAreaEntity>(request.Ids).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
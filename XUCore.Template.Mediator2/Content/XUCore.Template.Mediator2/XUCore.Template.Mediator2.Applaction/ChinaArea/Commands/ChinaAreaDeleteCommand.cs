namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 省市区域管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "ChinaArea")]
public class ChinaAreaDeleteCommand : Command<Result<int>>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }

    /// <summary>
    /// 删除城市区域（物理删除）
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> DeleteAsync([FromServices] IMediator mediator, [Required][FromQuery] ChinaAreaDeleteCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
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

    public ChinaAreaDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
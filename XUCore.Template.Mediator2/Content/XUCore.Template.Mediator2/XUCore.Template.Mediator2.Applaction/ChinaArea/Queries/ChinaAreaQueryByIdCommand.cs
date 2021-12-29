namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 省市区域管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "ChinaArea")]
public class ChinaAreaQueryByIdCommand : Command<Result<ChinaAreaDto>>, IDynamicWebApi
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// 获取城市区域信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<ChinaAreaDto>> GetAsync([FromServices] IMediator mediator, [Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new ChinaAreaQueryByIdCommand { Id = id }, cancellationToken);
}

internal class ChinaAreaQueryByIdCommandValidator : CommandValidator<ChinaAreaQueryByIdCommand>
{
    public ChinaAreaQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

internal class ChinaAreaQueryByIdCommandHandler : CommandHandler<ChinaAreaQueryByIdCommand, Result<ChinaAreaDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public ChinaAreaQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
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
namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuQueryByIdCommand : Command<Result<MenuDto>>, IDynamicWebApi
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 获取导航信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<MenuDto>> GetAsync([FromServices] IMediator mediator, [Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new MenuQueryByIdCommand { Id = id }, cancellationToken);
}

public class MenuQueryByIdCommandValidator : CommandValidator<MenuQueryByIdCommand>
{
    public MenuQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

public class MenuQueryByIdCommandHandler : CommandHandler<MenuQueryByIdCommand, Result<MenuDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<MenuDto>> Handle(MenuQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<MenuDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}

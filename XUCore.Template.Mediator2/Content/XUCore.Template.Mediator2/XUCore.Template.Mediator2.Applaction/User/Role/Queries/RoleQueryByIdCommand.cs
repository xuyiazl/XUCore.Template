namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleQueryByIdCommand : Command<Result<RoleDto>>, IDynamicWebApi
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Result<RoleDto>> GetAsync([FromServices] IMediator mediator, [Required] long id, CancellationToken cancellationToken = default)
        => await mediator.Send(new RoleQueryByIdCommand { Id = id }, cancellationToken);
}

internal class RoleQueryByIdCommandValidator : CommandValidator<RoleQueryByIdCommand>
{
    public RoleQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

internal class RoleQueryByIdCommandHandler : CommandHandler<RoleQueryByIdCommand, Result<RoleDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<RoleDto>> Handle(RoleQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<RoleDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
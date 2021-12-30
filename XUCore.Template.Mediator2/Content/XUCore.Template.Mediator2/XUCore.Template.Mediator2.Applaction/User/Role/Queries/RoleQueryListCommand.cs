namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleQueryListCommand : Command<Result<List<RoleDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    [Required]
    public int Limit { get; set; }
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<RoleDto>>> GetListAsync([FromServices] IMediator mediator, [Required][FromQuery] RoleQueryListCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class RoleQueryListCommandValidator : CommandValidator<RoleQueryListCommand>
{
    public RoleQueryListCommandValidator()
    {

    }
}

public class RoleQueryListCommandHandler : CommandHandler<RoleQueryListCommand, Result<List<RoleDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<RoleDto>>> Handle(RoleQueryListCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var select = repo.Select
             .Where(c => c.Enabled == request.Enabled)
             .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
             .OrderBy(c => c.Id);

        if (request.Limit > 0)
            select = select.Take(request.Limit);

        var res = await select.ToListAsync<RoleDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
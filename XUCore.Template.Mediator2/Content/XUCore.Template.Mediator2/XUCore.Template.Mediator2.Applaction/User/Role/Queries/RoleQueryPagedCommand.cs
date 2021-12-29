namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleQueryPagedCommand : Command<Result<PagedModel<RoleDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 当前页码
    /// </summary>
    /// <value></value>
    [Required]
    public int CurrentPage { get; set; }
    /// <summary>
    /// 分页大小
    /// </summary>
    /// <value></value>
    [Required]
    public int PageSize { get; set; }
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// 获取角色分页
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<RoleDto>>> GetPageAsync([FromServices] IMediator mediator, [Required][FromQuery] RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


internal class RoleQueryPagedCommandValidator : CommandValidator<RoleQueryPagedCommand>
{
    public RoleQueryPagedCommandValidator()
    {
        RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
    }
}

internal class RoleQueryPagedCommandHandler : CommandHandler<RoleQueryPagedCommand, Result<PagedModel<RoleDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<PagedModel<RoleDto>>> Handle(RoleQueryPagedCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var res = await repo.Select
             .Where(c => c.Enabled == request.Enabled)
             .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
             .OrderBy(c => c.Id)
             .ToPagedListAsync<RoleEntity, RoleDto>(request.CurrentPage, request.PageSize, cancellationToken);

        return RestFull.Success(data: res.ToModel());
    }
}
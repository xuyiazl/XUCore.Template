namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleQueryRelevanceMenuCommand : Command<Result<List<long>>>, IDynamicWebApi
{
    /// <summary>
    /// 角色id
    /// </summary>
    /// <value></value>
    [Required]
    public long RoleId { get; set; }

    /// <summary>
    /// 获取角色关联的所有导航id集合
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<long>>> GetRelevanceMenuAsync([FromServices] IMediator mediator, [Required][FromQuery] RoleQueryRelevanceMenuCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


internal class RoleQueryRelevanceMenuCommandValidator : CommandValidator<RoleQueryRelevanceMenuCommand>
{
    public RoleQueryRelevanceMenuCommandValidator()
    {

    }
}

internal class RoleQueryRelevanceMenuCommandHandler : CommandHandler<RoleQueryRelevanceMenuCommand, Result<List<long>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryRelevanceMenuCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<long>>> Handle(RoleQueryRelevanceMenuCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Select<RoleMenuEntity>().Where(c => c.RoleId == request.RoleId).OrderBy(c => c.MenuId).ToListAsync(c => c.MenuId, cancellationToken);

        return RestFull.Success(data: res);
    }
}
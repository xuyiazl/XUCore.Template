namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 获取角色关联的所有导航id集合
/// </summary>
public class RoleQueryRelevanceMenuCommand : Command<Result<List<long>>>
{
    /// <summary>
    /// 角色id
    /// </summary>
    /// <value></value>
    [Required]
    public long RoleId { get; set; }
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
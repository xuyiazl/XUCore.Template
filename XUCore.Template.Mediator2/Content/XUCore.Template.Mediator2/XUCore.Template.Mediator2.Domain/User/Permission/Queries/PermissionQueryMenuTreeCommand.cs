namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 权限查询菜单tree命令
/// </summary>
public class PermissionQueryMenuTreeCommand : Command<Result<List<PermissionMenuTreeDto>>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
}

internal class PermissionQueryMenuTreeCommandValidator : CommandValidator<PermissionQueryMenuTreeCommand>
{
    public PermissionQueryMenuTreeCommandValidator()
    {

    }
}

internal class PermissionQueryMenuTreeCommandHandler : CommandHandler<PermissionQueryMenuTreeCommand, Result<List<PermissionMenuTreeDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public PermissionQueryMenuTreeCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<PermissionMenuTreeDto>>> Handle(PermissionQueryMenuTreeCommand request, CancellationToken cancellationToken)
    {
        var menus = await mediator.Send(new PermissionQueryCacheCommand { UserId = request.UserId }, cancellationToken);

        var list = menus
                .Where(c => c.IsMenu == true)
                .OrderByDescending(c => c.Sort)
                .ToList();

        var treeDtos = mapper.Map<IList<MenuEntity>, IList<PermissionMenuTreeDto>>(list);

        var tree = treeDtos.ToTree(
            rootWhere: (r, c) => c.ParentId == 0,
            childsWhere: (r, c) => r.Id == c.ParentId,
            addChilds: (r, datalist) =>
            {
                r.Child ??= new List<PermissionMenuTreeDto>();
                r.Child.AddRange(datalist);
            });

        return RestFull.Success(data: tree);
    }
}
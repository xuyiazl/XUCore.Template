namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 权限查询命令
/// </summary>
public class PermissionQueryCacheCommand : Command<List<MenuEntity>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
}

internal class PermissionQueryCacheCommandValidator : CommandValidator<PermissionQueryCacheCommand>
{
    public PermissionQueryCacheCommandValidator()
    {

    }
}

internal class PermissionQueryCacheCommandHandler : CommandHandler<PermissionQueryCacheCommand, List<MenuEntity>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public PermissionQueryCacheCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    [AspectCache(HashKey = CacheKey.AuthUser, Key = "{0}", Seconds = CacheTime.Min5)]
    public override async Task<List<MenuEntity>> Handle(PermissionQueryCacheCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var res = await db.Orm
           .Select<UserRoleEntity, RoleMenuEntity, MenuEntity>()
           .LeftJoin((userRole, roleMenu, menu) => userRole.RoleId == roleMenu.RoleId)
           .LeftJoin((userRole, roleMenu, menu) => roleMenu.MenuId == menu.Id)
           .Where((userRole, roleMenu, menu) => userRole.UserId == request.UserId)
           .Distinct()
           .ToListAsync((userRole, roleMenu, menu) => menu, cancellationToken);

        return res;
    }
}

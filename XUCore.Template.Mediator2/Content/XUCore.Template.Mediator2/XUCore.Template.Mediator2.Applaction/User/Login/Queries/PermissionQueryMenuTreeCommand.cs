namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 权限操作
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Login")]
public class PermissionQueryMenuTreeCommand : Command<Result<List<PermissionMenuTreeDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// 查询权限导航
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<PermissionMenuTreeDto>>> GetPermissionMenusAsync([FromServices] IMediator mediator, [Required][FromQuery] PermissionQueryMenuTreeCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class PermissionQueryMenuTreeCommandValidator : CommandValidator<PermissionQueryMenuTreeCommand>
{
    public PermissionQueryMenuTreeCommandValidator()
    {

    }
}

public class PermissionQueryMenuTreeCommandHandler : CommandHandler<PermissionQueryMenuTreeCommand, Result<List<PermissionMenuTreeDto>>>
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
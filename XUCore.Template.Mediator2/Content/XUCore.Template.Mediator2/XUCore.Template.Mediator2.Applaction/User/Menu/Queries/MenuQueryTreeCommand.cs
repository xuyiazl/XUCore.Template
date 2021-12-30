namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuQueryTreeCommand : Command<Result<List<MenuTreeDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 获取导航树形结构
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<MenuTreeDto>>> GetTreeAsync([FromServices] IMediator mediator, CancellationToken cancellationToken = default)
        => await mediator.Send(new MenuQueryTreeCommand(), cancellationToken);
}

public class MenuQueryTreeCommandValidator : CommandValidator<MenuQueryTreeCommand>
{
    public MenuQueryTreeCommandValidator()
    {

    }
}

public class MenuQueryTreeCommandHandler : CommandHandler<MenuQueryTreeCommand, Result<List<MenuTreeDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuQueryTreeCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<MenuTreeDto>>> Handle(MenuQueryTreeCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        //var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToListAsync<MenuTreeDto>(cancellationToken);

        //var tree = res.ToTree(
        //    rootWhere: (r, c) => c.ParentId == 0,
        //    childsWhere: (r, c) => r.Id == c.ParentId,
        //    addChilds: (r, datalist) =>
        //    {
        //        r.Childs ??= new List<MenuTreeDto>();
        //        r.Childs.AddRange(datalist);
        //    });

        var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToTreeListAsync(cancellationToken);

        var tree = mapper.Map<List<MenuEntity>, List<MenuTreeDto>>(res);

        return RestFull.Success(data: tree);
    }
}

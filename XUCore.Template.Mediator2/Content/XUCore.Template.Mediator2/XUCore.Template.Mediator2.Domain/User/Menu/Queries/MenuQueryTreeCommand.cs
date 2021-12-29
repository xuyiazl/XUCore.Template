namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 菜单tree查询命令
/// </summary>
public class MenuQueryTreeCommand : Command<Result<List<MenuTreeDto>>>
{

}

internal class MenuQueryTreeCommandValidator : CommandValidator<MenuQueryTreeCommand>
{
    public MenuQueryTreeCommandValidator()
    {

    }
}

internal class MenuQueryTreeCommandHandler : CommandHandler<MenuQueryTreeCommand, Result<List<MenuTreeDto>>>
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

namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 导航删除命令
/// </summary>
public class MenuDeleteCommand : Command<Result<int>>
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }
}

public class MenuDeleteCommandValidator : CommandValidator<MenuDeleteCommand>
{
    public MenuDeleteCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

public class MenuDeleteCommandHandler : CommandHandler<MenuDeleteCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuDeleteCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(MenuDeleteCommand request, CancellationToken cancellationToken)
    {
        var res = await db.Orm.Delete<MenuEntity>(request.Ids).ExecuteAffrowsAsync(cancellationToken);

        if (res > 0)
        {
            await db.Orm.Delete<RoleMenuEntity>().Where(c => request.Ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);
        }

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
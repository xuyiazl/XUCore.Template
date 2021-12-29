namespace XUCore.Template.Mediator2.Domain;

/// <summary>
/// 查询一条菜单记录
/// </summary>
public class MenuQueryByIdCommand : Command<Result<MenuDto>>
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }
}

internal class MenuQueryByIdCommandValidator : CommandValidator<MenuQueryByIdCommand>
{
    public MenuQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

internal class MenuQueryByIdCommandHandler : CommandHandler<MenuQueryByIdCommand, Result<MenuDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<MenuDto>> Handle(MenuQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<MenuDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}

namespace XUCore.Template.Mediator.Applaction.Commands;

/// <summary>
/// 查询一条角色记录
/// </summary>
public class RoleQueryByIdCommand : Command<Result<RoleDto>>
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public long Id { get; set; }
}

public class RoleQueryByIdCommandValidator : CommandValidator<RoleQueryByIdCommand>
{
    public RoleQueryByIdCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
    }
}

public class RoleQueryByIdCommandHandler : CommandHandler<RoleQueryByIdCommand, Result<RoleDto>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryByIdCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<RoleDto>> Handle(RoleQueryByIdCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var res = await repo.Select.WhereDynamic(request.Id).ToOneAsync<RoleDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
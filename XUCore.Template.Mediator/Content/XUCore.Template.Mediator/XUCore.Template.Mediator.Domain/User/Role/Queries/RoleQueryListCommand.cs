namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 角色查询命令
/// </summary>
public class RoleQueryListCommand : Command<Result<List<RoleDto>>>
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    public int Limit { get; set; }
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}

public class RoleQueryListCommandValidator : CommandValidator<RoleQueryListCommand>
{
    public RoleQueryListCommandValidator()
    {

    }
}

public class RoleQueryListCommandHandler : CommandHandler<RoleQueryListCommand, Result<List<RoleDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleQueryListCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<RoleDto>>> Handle(RoleQueryListCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var select = repo.Select
             .Where(c => c.Enabled == request.Enabled)
             .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
             .OrderBy(c => c.Id);

        if (request.Limit > 0)
            select = select.Take(request.Limit);

        var res = await select.ToListAsync<RoleDto>(cancellationToken);

        return RestFull.Success(data: res);
    }
}
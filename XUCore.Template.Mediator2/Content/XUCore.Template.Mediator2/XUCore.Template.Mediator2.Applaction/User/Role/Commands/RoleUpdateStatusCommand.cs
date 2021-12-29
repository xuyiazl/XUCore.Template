namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleUpdateStatusCommand : Command<Result<int>>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long[] Ids { get; set; }
    /// <summary>
    /// 修改的值
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateEnabledAsync([FromServices] IMediator mediator, [Required][FromQuery] RoleUpdateStatusCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


internal class RoleUpdateStatusCommandalidator : CommandValidator<RoleUpdateStatusCommand>
{
    public RoleUpdateStatusCommandalidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithName("Id不可为空");
    }
}

internal class RoleUpdateStatusCommandHandler : CommandHandler<RoleUpdateStatusCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleUpdateStatusCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(RoleUpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var list = await repo.Select.Where(c => request.Ids.Contains(c.Id)).ToListAsync<RoleEntity>(cancellationToken);

        list.ForEach(c => c.Enabled = request.Enabled);

        var res = await repo.UpdateAsync(list, cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleCreateCommand : Command<Result<long>>, IMapFrom<RoleEntity>, IDynamicWebApi
{
    /// <summary>
    /// 角色名
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 导航关联id集合
    /// </summary>
    public long[] MenuIds { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    [NonDynamicMethod]
    public void Mapping(Profile profile) =>
            profile.CreateMap<RoleCreateCommand, RoleEntity>()
            ;

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<long>> CreateAsync([FromServices] IMediator mediator, [Required][FromBody] RoleCreateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

internal class RoleCreateCommandValidator : CommandValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
    }
}

internal class RoleCreateCommandHandler : CommandHandler<RoleCreateCommand, Result<long>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleCreateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<long>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var entity = mapper.Map<RoleCreateCommand, RoleEntity>(request);

        //保存关联导航
        if (request.MenuIds != null && request.MenuIds.Length > 0)
        {
            entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
            {
                RoleId = entity.Id,
                MenuId = key
            });
        }

        var res = await repo.InsertAsync(entity, cancellationToken);

        if (res != null)
        {
            await db.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

            return RestFull.Success(data: res.Id);
        }
        else
            return RestFull.Fail(data: res.Id);
    }
}
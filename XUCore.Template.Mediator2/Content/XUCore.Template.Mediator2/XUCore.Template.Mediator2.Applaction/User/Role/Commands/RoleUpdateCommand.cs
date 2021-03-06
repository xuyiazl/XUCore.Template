namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 角色管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Role")]
public class RoleUpdateCommand : Command<Result<int>>, IMapFrom<RoleEntity>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 角色名
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 导航id集合
    /// </summary>
    public long[] MenuIds { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    [NonDynamicMethod]
    public void Mapping(Profile profile) =>
            profile.CreateMap<RoleUpdateCommand, RoleEntity>()
            ;

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([FromServices] IMediator mediator, [Required][FromBody] RoleUpdateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}


public class RoleUpdateCommandValidator : CommandValidator<RoleUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
    }
}

public class RoleUpdateCommandHandler : CommandHandler<RoleUpdateCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public RoleUpdateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<RoleEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<RoleEntity>(cancellationToken);

        if (entity == null)
            return RestFull.Fail(data: 0);

        //先清空导航集合，确保没有冗余信息
        await db.Orm.Delete<RoleMenuEntity>().Where(c => c.RoleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

        //保存关联导航
        if (request.MenuIds != null && request.MenuIds.Length > 0)
        {
            entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
            {
                RoleId = entity.Id,
                MenuId = key
            });
        }

        var res = await repo.UpdateAsync(entity, cancellationToken);

        if (res > 0)
        {
            await db.Orm.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

            return RestFull.Success(data: res);
        }
        else
            return RestFull.Fail(data: res);
    }
}
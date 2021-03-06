namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 菜单管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Menu")]
public class MenuUpdateCommand : Command<Result<int>>, IMapFrom<MenuEntity>, IDynamicWebApi
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// 导航父级id
    /// </summary>
    public long ParentId { get; set; }
    /// <summary>
    /// 名字
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// 图标样式
    /// </summary>s
    public string Icon { get; set; }
    /// <summary>
    /// 链接地址
    /// </summary>
    [Required]
    public string Url { get; set; }
    /// <summary>
    /// 唯一代码（权限使用）
    /// </summary>
    [Required]
    public string OnlyCode { get; set; }
    /// <summary>
    /// 是否是导航
    /// </summary>
    public bool IsMenu { get; set; }
    /// <summary>
    /// 排序权重
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 是否是快捷导航
    /// </summary>
    public bool IsExpress { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    [NonDynamicMethod]
    public void Mapping(Profile profile) =>
        profile.CreateMap<MenuUpdateCommand, MenuEntity>()
            .ForMember(c => c.Url, c => c.MapFrom(s => s.Url.IsEmpty() ? "#" : s.Url))
        ;

    /// <summary>
    /// 更新导航信息
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<int>> UpdateAsync([FromServices] IMediator mediator, [Required][FromBody] MenuUpdateCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class MenuUpdateCommandValidator : CommandValidator<MenuUpdateCommand>
{
    public MenuUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithName("Id不可为空");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("菜单名");
        RuleFor(x => x.Url).NotEmpty().MaximumLength(50).WithName("Url");
        RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("唯一代码");
    }
}

public class MenuUpdateCommandHandler : CommandHandler<MenuUpdateCommand, Result<int>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuUpdateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<int>> Handle(MenuUpdateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<MenuEntity>(cancellationToken);

        if (entity == null)
            return RestFull.Fail(data: 0);

        entity = mapper.Map(request, entity);

        var res = await repo.UpdateAsync(entity, cancellationToken);

        if (res > 0)
            return RestFull.Success(data: res);
        else
            return RestFull.Fail(data: res);
    }
}
namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 创建导航命令
/// </summary>
public class MenuCreateCommand : Command<Result<long>>, IMapFrom<MenuEntity>
{
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
    /// </summary>
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

    public void Mapping(Profile profile) =>
        profile.CreateMap<MenuCreateCommand, MenuEntity>()
            .ForMember(c => c.Url, c => c.MapFrom(s => s.Url.IsEmpty() ? "#" : s.Url))
        ;
}


public class MenuCreateCommandValidator : CommandValidator<MenuCreateCommand>
{
    public MenuCreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("菜单名");
        RuleFor(x => x.Url).NotEmpty().MaximumLength(50).WithName("Url");
        RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("唯一代码");
    }
}

public class MenuCreateCommandHandler : CommandHandler<MenuCreateCommand, Result<long>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public MenuCreateCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<long>> Handle(MenuCreateCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<MenuEntity>();

        var entity = mapper.Map<MenuCreateCommand, MenuEntity>(request);

        var res = await repo.InsertAsync(entity, cancellationToken);

        if (res != null)
            return RestFull.Success(data: res.Id);
        else
            return RestFull.Fail(data: 0L);
    }
}
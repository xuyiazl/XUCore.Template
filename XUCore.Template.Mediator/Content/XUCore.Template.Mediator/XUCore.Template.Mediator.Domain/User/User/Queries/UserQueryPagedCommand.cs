namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 用户查询分页
/// </summary>
public class UserQueryPagedCommand : Command<Result<PagedModel<UserDto>>>
{
    /// <summary>
    /// 当前页码
    /// </summary>
    /// <value></value>
    [Required]
    public int CurrentPage { get; set; }
    /// <summary>
    /// 分页大小
    /// </summary>
    /// <value></value>
    [Required]
    public int PageSize { get; set; }
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 启用s
    /// </summary>
    public bool Enabled { get; set; } = true;
}

public class UserQueryPagedCommandValidator : CommandValidator<UserQueryPagedCommand>
{
    public UserQueryPagedCommandValidator()
    {
        RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
    }
}

public class UserQueryPagedCommandHandler : CommandHandler<UserQueryPagedCommand, Result<PagedModel<UserDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<PagedModel<UserDto>>> Handle(UserQueryPagedCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserEntity>();

        var res = await repo.Select
          .Where(c => c.Enabled == request.Enabled)
          .WhereIf(request.Keyword.NotEmpty(), c =>
                       c.Name.Contains(request.Keyword) ||
                       c.Mobile.Contains(request.Keyword) ||
                       c.UserName.Contains(request.Keyword))
          .OrderBy(c => c.Id)
          .ToPagedListAsync<UserEntity, UserDto>(request.CurrentPage, request.PageSize, cancellationToken);

        return RestFull.Success(data: res.ToModel());
    }
}
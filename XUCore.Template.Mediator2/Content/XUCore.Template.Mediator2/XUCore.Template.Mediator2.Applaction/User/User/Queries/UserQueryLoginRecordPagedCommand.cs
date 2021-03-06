using XUCore.NetCore.FreeSql.Curd;

namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserQueryLoginRecordPagedCommand : Command<Result<PagedModel<UserLoginRecordDto>>>, IDynamicWebApi
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
    /// 获取所有登录记录分页
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordPageAsync([FromServices] IMediator mediator, [Required][FromQuery] UserQueryLoginRecordPagedCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

public class UserQueryLoginRecordPagedCommandValidator : CommandValidator<UserQueryLoginRecordPagedCommand>
{
    public UserQueryLoginRecordPagedCommandValidator()
    {
        RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
    }
}

public class UserQueryLoginRecordPagedCommandHandler : CommandHandler<UserQueryLoginRecordPagedCommand, Result<PagedModel<UserLoginRecordDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryLoginRecordPagedCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<PagedModel<UserLoginRecordDto>>> Handle(UserQueryLoginRecordPagedCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserLoginRecordEntity>();

        var res = await repo.Select
          .WhereIf(request.Keyword.NotEmpty(), c => c.User.Name.Contains(request.Keyword) || c.User.Mobile.Contains(request.Keyword) || c.User.UserName.Contains(request.Keyword))
          .OrderByDescending(c => c.CreatedAt)
          .ToPagedListAsync(request.CurrentPage, request.PageSize, c => new UserLoginRecordDto
          {
              UserId = c.UserId,
              Id = c.Id,
              LoginIp = c.LoginIp,
              LoginTime = c.CreatedAt.Value,
              LoginWay = c.LoginWay,
              Mobile = c.User.Mobile,
              Name = c.User.Name,
              Picture = c.User.Picture,
              UserName = c.User.UserName
          }, cancellationToken);

        return RestFull.Success(data: res.ToModel());
    }
}
namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 用户管理
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "User")]
public class UserQueryLoginRecordCommand : Command<Result<List<UserLoginRecordDto>>>, IDynamicWebApi
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    [Required]
    public int Limit { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// 获取最近登录记录
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<List<UserLoginRecordDto>>> GetRecordListAsync([FromServices] IMediator mediator, [Required][FromQuery] UserQueryLoginRecordCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
}

internal class UserQueryLoginRecordCommandValidator : CommandValidator<UserQueryLoginRecordCommand>
{
    public UserQueryLoginRecordCommandValidator()
    {

    }
}

internal class UserQueryLoginRecordCommandHandler : CommandHandler<UserQueryLoginRecordCommand, Result<List<UserLoginRecordDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryLoginRecordCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<List<UserLoginRecordDto>>> Handle(UserQueryLoginRecordCommand request, CancellationToken cancellationToken)
    {
        var repo = db.Orm.GetRepository<UserLoginRecordEntity>();

        var res = await repo.Select
          .Where(c => c.UserId == request.UserId)
          .OrderByDescending(c => c.CreatedAt)
          .Take(request.Limit)
          .ToListAsync(c => new UserLoginRecordDto
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

        return RestFull.Success(data: res);
    }
}
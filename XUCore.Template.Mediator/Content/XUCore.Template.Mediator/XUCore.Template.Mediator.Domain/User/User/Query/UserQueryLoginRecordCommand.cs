namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 查询命令
/// </summary>
public class UserQueryLoginRecordCommand : Command<Result<List<UserLoginRecordDto>>>
{
    /// <summary>
    /// 记录数
    /// </summary>
    /// <value></value>
    public int Limit { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
}

public class UserQueryLoginRecordCommandValidator : CommandValidator<UserQueryLoginRecordCommand>
{
    public UserQueryLoginRecordCommandValidator()
    {

    }
}

public class UserQueryLoginRecordCommandHandler : CommandHandler<UserQueryLoginRecordCommand, Result<List<UserLoginRecordDto>>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public UserQueryLoginRecordCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
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
namespace XUCore.Template.Mediator.Domain;

/// <summary>
/// 权限查询是否存在命令
/// </summary>
public class PermissionExistCommand : Command<Result<bool>>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 唯一代码
    /// </summary>
    public string OnlyCode { get; set; }
}

public class PermissionExistCommandValidator : CommandValidator<PermissionExistCommand>
{
    public PermissionExistCommandValidator()
    {

    }
}

public class PermissionExistCommandHandler : CommandHandler<PermissionExistCommand, Result<bool>>
{
    protected readonly FreeSqlUnitOfWorkManager db;
    protected readonly IUserInfo user;

    public PermissionExistCommandHandler(FreeSqlUnitOfWorkManager db, IMediatorHandler bus, IMapper mapper, IUserInfo user) : base(bus, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<bool>> Handle(PermissionExistCommand request, CancellationToken cancellationToken)
    {
        var menus = await bus.SendCommand(new PermissionQueryCacheCommand { UserId = request.UserId }, cancellationToken);

        var res = menus.Any(c => c.OnlyCode == request.OnlyCode);

        return RestFull.Success(data: res);
    }
}

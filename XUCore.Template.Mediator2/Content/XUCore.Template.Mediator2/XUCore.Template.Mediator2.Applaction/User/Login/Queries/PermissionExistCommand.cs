namespace XUCore.Template.Mediator2.Applaction;

/// <summary>
/// 权限操作
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroup.Admin)]
[DynamicWebApi(Name = "Login")]
public class PermissionExistCommand : Command<Result<bool>>, IDynamicWebApi
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public long UserId { get; set; }
    /// <summary>
    /// 唯一代码
    /// </summary>
    [Required]
    public string OnlyCode { get; set; }

    /// <summary>
    /// 查询是否有权限
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<bool>> GetPermissionExistsAsync([FromServices] IMediator mediator, [Required][FromQuery] PermissionExistCommand request, CancellationToken cancellationToken = default)
        => await mediator.Send(request, cancellationToken);
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

    public PermissionExistCommandHandler(FreeSqlUnitOfWorkManager db, IMediator mediator, IMapper mapper, IUserInfo user) : base(mediator, mapper)
    {
        this.db = db;
        this.user = user;
    }

    public override async Task<Result<bool>> Handle(PermissionExistCommand request, CancellationToken cancellationToken)
    {
        var menus = await mediator.Send(new PermissionQueryCacheCommand { UserId = request.UserId }, cancellationToken);

        var res = menus.Any(c => c.OnlyCode == request.OnlyCode);

        return RestFull.Success(data: res);
    }
}
